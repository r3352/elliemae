// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.FileAttachmentCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class FileAttachmentCollection : CollectionBase
  {
    private const string className = "FileAttachmentCollection�";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private string downloadPath;
    private long totalBytes;
    private long uploadedBytes;
    private bool isEFolderActiveUser;
    private object nativeDownloadLock = new object();
    private object pageDownloadLock = new object();
    private object thumbnailDownloadLock = new object();
    private int pageDownloadTotal;
    private int pageDownloadCount;
    private int thumbnailDownloadTotal;
    private int thumbnailDownloadCount;
    private Dictionary<string, string> cacheFileTable = new Dictionary<string, string>();
    private const string Category = "eFolder�";
    private const string ActiveUser = "ActiveUser�";

    public FileAttachmentCollection(LoanDataMgr loanDataMgr)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 114, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      this.loanDataMgr = loanDataMgr;
      this.loanDataMgr.LoanClosing += new EventHandler(this.loanDataMgr_LoanClosing);
      this.downloadPath = SystemSettings.TempFolderRoot + "eFolder\\";
      if (!Directory.Exists(this.downloadPath))
        Directory.CreateDirectory(this.downloadPath);
      if (this.loanDataMgr.LoanObject != null)
      {
        foreach (FileAttachment fileAttachment in this.getAttachmentsFromServer())
        {
          if (!(fileAttachment is BackgroundAttachment) || BackgroundAttachmentDialog.IsProcessing(fileAttachment.ID))
          {
            fileAttachment.AttachLoanHistoryMonitor((IFileHistoryMonitor) loanDataMgr.LoanHistory);
            fileAttachment.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For Constructor");
            lock (this.List.SyncRoot)
              this.List.Add((object) fileAttachment);
          }
        }
      }
      if (BackgroundAttachmentDialog.Instance != null)
      {
        BackgroundAttachmentDialog.RegisterLoan(loanDataMgr);
        BackgroundAttachmentDialog.Instance.BackgroundAttachmentUploaded += new FileAttachmentEventHandler(this.BackgroundAttachmentDialog_BackgroundAttachmentUploaded);
      }
      this.isEFolderActiveUser = loanDataMgr.UseSkyDriveClassic && loanDataMgr.SessionObjects.ConfigurationManager.GetCompanySetting("eFolder", nameof (ActiveUser))?.ToUpper() == "TRUE";
      PerformanceMeter.Current.AddCheckpoint("END", 163, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
    }

    private FileAttachment[] getAttachmentsFromServer()
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 171, nameof (getAttachmentsFromServer), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling GetFileAttachments");
      FileAttachment[] collection1 = this.loanDataMgr.LoanObject.GetFileAttachments();
      System.Collections.Generic.List<FileAttachment> collection2 = new System.Collections.Generic.List<FileAttachment>();
      string[] strArray = (string[]) null;
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      for (int index = 0; index < collection1.Length; ++index)
      {
        FileAttachment attachment = collection1[index];
        if (!string.IsNullOrEmpty(attachment.DocumentID) && logList.GetRecordByID(attachment.DocumentID, false, true) is DocumentLog)
        {
          attachment.GetType().GetField("documentID", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) attachment, (object) null);
          attachment.MarkAsDirty();
        }
        if (attachment is NativeAttachment && !attachment.IsRemoved && Guid.TryParse(attachment.ID, out Guid _))
        {
          string id = attachment.ID;
          if (strArray == null)
          {
            if (this.loanDataMgr.UseSkyDriveLite)
            {
              Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling Loan.GetSkyDriveSupportingDataKeys");
              strArray = this.loanDataMgr.LoanObject.GetSkyDriveSupportingDataKeys();
            }
            else
            {
              Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling Loan.GetSupportingDataKeysOnCIFs");
              strArray = this.loanDataMgr.LoanObject.GetSupportingDataKeysOnCIFs();
            }
          }
          string str1 = (string) null;
          foreach (string str2 in strArray)
          {
            if (str2.IndexOf(id, StringComparison.OrdinalIgnoreCase) >= 0)
              str1 = str2;
          }
          if (str1 != null)
          {
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Creating Good NativeAttachment: " + str1);
            NativeAttachment nativeAttachment = new NativeAttachment(attachment, attachment.FileSize);
            nativeAttachment.GetType().GetField("id", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) nativeAttachment, (object) str1);
            nativeAttachment.MarkAsDirty();
            collection2.Add((FileAttachment) nativeAttachment);
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Removing Bad NativeAttachment: " + id);
            attachment.IsRemoved = true;
            DocumentLog linkedDocument = this.GetLinkedDocument(id, false);
            if (linkedDocument != null)
            {
              FileAttachmentReference file = linkedDocument.Files[id];
              Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Fixing FileAttachmentReference: " + id);
              file.GetType().GetField("attachmentID", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) file, (object) str1);
              this.loanDataMgr.LoanData.Dirty = true;
            }
            RemoteLogger.Write(TraceLevel.Error, "FileAttachmentCollection: Fixed NativeAttachment. Loan: " + this.loanDataMgr.LoanData.GUID + " Bad: " + id + " Good: " + str1);
          }
        }
      }
      PerformanceMeter.Current.AddCheckpoint("END", 281, nameof (getAttachmentsFromServer), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      if (collection2.Count > 0)
      {
        System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>((IEnumerable<FileAttachment>) collection1);
        fileAttachmentList.AddRange((IEnumerable<FileAttachment>) collection2);
        collection1 = fileAttachmentList.ToArray();
      }
      return collection1;
    }

    private void loanDataMgr_LoanClosing(object sender, EventArgs e)
    {
      this.loanDataMgr.LoanClosing -= new EventHandler(this.loanDataMgr_LoanClosing);
      if (BackgroundAttachmentDialog.Instance == null)
        return;
      BackgroundAttachmentDialog.Instance.BackgroundAttachmentUploaded -= new FileAttachmentEventHandler(this.BackgroundAttachmentDialog_BackgroundAttachmentUploaded);
      foreach (FileAttachment allFile in this.GetAllFiles(false, true))
      {
        if (allFile is BackgroundAttachment && allFile.IsNew)
          BackgroundAttachmentDialog.Remove(allFile.ID);
      }
    }

    private void attachment_FileAttachmentChanged(object sender, EventArgs e)
    {
      this.OnFileAttachmentChanged((FileAttachment) sender);
    }

    private void BackgroundAttachmentDialog_BackgroundAttachmentUploaded(
      object source,
      FileAttachmentEventArgs e)
    {
      bool flag = false;
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For BackgroundAttachmentUploaded");
      lock (this.List.SyncRoot)
      {
        for (int index = 0; index < this.List.Count; ++index)
        {
          FileAttachment fileAttachment = (FileAttachment) this.List[index];
          if (fileAttachment.ID == e.File.ID)
          {
            if (fileAttachment.IsRemoved)
              e.File.IsRemoved = true;
            e.File.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
            e.File.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
            this.List[index] = (object) e.File;
            flag = true;
          }
        }
      }
      if (!flag)
        return;
      this.OnBackgroundAttachmentUploaded(e.File);
    }

    public bool ReadOnly
    {
      get
      {
        return !this.loanDataMgr.Writable || (this.loanDataMgr.LoanData.ContentAccess & LoanContentAccess.DocumentTracking) != LoanContentAccess.DocumentTracking && (this.loanDataMgr.LoanData.ContentAccess & LoanContentAccess.DocTrackingPartial) != LoanContentAccess.DocTrackingPartial;
      }
    }

    public FileAttachment AddAttachment(
      AddReasonType reason,
      string filepath,
      string title,
      DocumentLog doc,
      bool delayConversion = false)
    {
      return this.AddAttachment(reason, filepath, title, doc, (DocumentIdentity[]) null, delayConversion);
    }

    public FileAttachment AddAttachment(
      AddReasonType reason,
      string filepath,
      string title,
      DocumentLog doc)
    {
      return this.AddAttachment(reason, filepath, title, doc, (DocumentIdentity[]) null);
    }

    public FileAttachment AddAttachment(
      AddReasonType reason,
      string filepath,
      string title,
      DocumentLog doc,
      DocumentIdentity[] identityList)
    {
      Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "AddAttachment: filepath(" + filepath + ") starts at" + DateTime.Now.ToString());
      FileInfo fileInfo = new FileInfo(filepath);
      using (BinaryObject data = new BinaryObject(filepath, false))
      {
        FileAttachment fileAttachment = this.AddAttachment(reason, data, fileInfo.Extension, title, doc, identityList);
        Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "AddAttachment: filepath(" + filepath + ") ends at" + DateTime.Now.ToString(), data.Length);
        return fileAttachment;
      }
    }

    public FileAttachment AddAttachment(
      AddReasonType reason,
      string filepath,
      string title,
      DocumentLog doc,
      DocumentIdentity[] identityList,
      bool delayConversion = false)
    {
      Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "AddAttachment: filepath(" + filepath + ") starts at" + DateTime.Now.ToString());
      FileInfo fileInfo = new FileInfo(filepath);
      using (BinaryObject data = new BinaryObject(filepath, false))
      {
        FileAttachment fileAttachment = this.AddAttachment(reason, data, fileInfo.Extension, title, doc, identityList, delayConversion);
        Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "AddAttachment: filepath(" + filepath + ") ends at" + DateTime.Now.ToString(), data.Length);
        return fileAttachment;
      }
    }

    public FileAttachment AddAttachment(
      AddReasonType reason,
      BinaryObject data,
      string extension,
      string title,
      DocumentLog doc)
    {
      return this.AddAttachment(reason, data, extension, title, doc, (DocumentIdentity[]) null);
    }

    public FileAttachment AddAttachment(
      AddReasonType reason,
      BinaryObject data,
      string extension,
      string title,
      DocumentLog doc,
      DocumentIdentity[] identityList,
      bool delayConversion = false)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 465, nameof (AddAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (this.loanDataMgr.IsNew())
          this.loanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
        bool useSkyDrive = this.loanDataMgr.UseSkyDrive;
        bool flag1 = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.UseBackgroundConversion;
        if (reason == AddReasonType.Printer || reason == AddReasonType.SDK || reason == AddReasonType.SDKImage)
          flag1 = false;
        bool createImageAttachment = this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
        switch (reason)
        {
          case AddReasonType.SDK:
            createImageAttachment = false;
            break;
          case AddReasonType.SDKImage:
            createImageAttachment = true;
            break;
          default:
            if (delayConversion)
            {
              createImageAttachment = false;
              break;
            }
            break;
        }
        bool flag2 = this.loanDataMgr.UseSkyDriveClassic && !createImageAttachment;
        FileAttachment fileAttachment = !useSkyDrive ? (!flag2 ? (!flag1 ? (!createImageAttachment ? (FileAttachment) this.createNativeAttachment(reason, data, extension, title, (FileAttachment[]) null, identityList) : (FileAttachment) this.createImageAttachment(reason, data, extension, title, (string[]) null, (FileAttachment[]) null, identityList, doc)) : (FileAttachment) this.createBackgroundAttachment(reason, data, extension, title, (FileAttachment[]) null, identityList, doc, createImageAttachment)) : this.createSDCNativeAttachment(reason, data, extension, title, (FileAttachment[]) null, identityList, "")) : (FileAttachment) this.createCloudAttachment(reason, data, extension, title, identityList);
        if (doc != null)
        {
          bool flag3 = true;
          bool isActive = true;
          switch (reason)
          {
            case AddReasonType.Retrieve:
            case AddReasonType.Esign:
            case AddReasonType.Fax:
            case AddReasonType.ScanDoc:
            case AddReasonType.Upload:
              eFolderAccessRights folderAccessRights1 = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
              if (folderAccessRights1.RetrieveDocumentMethod == RetrieveDocumentMethod.AssignNotCurrent)
              {
                isActive = false;
                break;
              }
              if (folderAccessRights1.RetrieveDocumentMethod == RetrieveDocumentMethod.Unassigned)
              {
                flag3 = false;
                break;
              }
              break;
            case AddReasonType.Services:
              eFolderAccessRights folderAccessRights2 = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
              if (folderAccessRights2.RetrieveServiceMethod == RetrieveDocumentMethod.AssignNotCurrent)
              {
                isActive = false;
                break;
              }
              if (folderAccessRights2.RetrieveServiceMethod == RetrieveDocumentMethod.Unassigned)
              {
                flag3 = false;
                break;
              }
              break;
          }
          if (flag3)
            doc.Files.Add(fileAttachment.ID, this.loanDataMgr.SessionObjects.UserID, isActive);
        }
        return fileAttachment;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 540, nameof (AddAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public FileAttachment AddAttachment(
      AddReasonType reason,
      BinaryObject data,
      string extension,
      string title,
      DocumentLog doc,
      DocumentIdentity[] identityList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 551, nameof (AddAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (this.loanDataMgr.IsNew())
          this.loanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
        bool useSkyDrive = this.loanDataMgr.UseSkyDrive;
        bool flag1 = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.UseBackgroundConversion;
        if (reason == AddReasonType.Printer || reason == AddReasonType.SDK || reason == AddReasonType.SDKImage)
          flag1 = false;
        bool createImageAttachment = this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
        switch (reason)
        {
          case AddReasonType.SDK:
            createImageAttachment = false;
            break;
          case AddReasonType.SDKImage:
            createImageAttachment = true;
            break;
        }
        bool flag2 = this.loanDataMgr.UseSkyDriveClassic && !createImageAttachment;
        FileAttachment fileAttachment = !useSkyDrive ? (!flag2 ? (!flag1 ? (!createImageAttachment ? (FileAttachment) this.createNativeAttachment(reason, data, extension, title, (FileAttachment[]) null, identityList) : (FileAttachment) this.createImageAttachment(reason, data, extension, title, (string[]) null, (FileAttachment[]) null, identityList, doc)) : (FileAttachment) this.createBackgroundAttachment(reason, data, extension, title, (FileAttachment[]) null, identityList, doc, createImageAttachment)) : this.createSDCNativeAttachment(reason, data, extension, title, (FileAttachment[]) null, identityList, "")) : (FileAttachment) this.createCloudAttachment(reason, data, extension, title, identityList);
        if (doc != null)
        {
          bool flag3 = true;
          bool isActive = true;
          switch (reason)
          {
            case AddReasonType.Retrieve:
            case AddReasonType.Esign:
            case AddReasonType.Fax:
            case AddReasonType.ScanDoc:
            case AddReasonType.Upload:
              eFolderAccessRights folderAccessRights1 = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
              if (folderAccessRights1.RetrieveDocumentMethod == RetrieveDocumentMethod.AssignNotCurrent)
              {
                isActive = false;
                break;
              }
              if (folderAccessRights1.RetrieveDocumentMethod == RetrieveDocumentMethod.Unassigned)
              {
                flag3 = false;
                break;
              }
              break;
            case AddReasonType.Services:
              eFolderAccessRights folderAccessRights2 = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
              if (folderAccessRights2.RetrieveServiceMethod == RetrieveDocumentMethod.AssignNotCurrent)
              {
                isActive = false;
                break;
              }
              if (folderAccessRights2.RetrieveServiceMethod == RetrieveDocumentMethod.Unassigned)
              {
                flag3 = false;
                break;
              }
              break;
          }
          if (flag3)
            doc.Files.Add(fileAttachment.ID, this.loanDataMgr.SessionObjects.UserID, isActive, fileAttachment.Date);
        }
        return fileAttachment;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 627, nameof (AddAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public FileAttachment AddAttachment(
      AddReasonType reason,
      string[] fileList,
      string title,
      DocumentLog doc)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 638, nameof (AddAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "AddAttachment: title(" + title + ") starts at" + DateTime.Now.ToString());
        FileAttachment fileAttachment = (FileAttachment) null;
        if (this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments)
        {
          fileAttachment = (FileAttachment) this.createImageAttachment(reason, (BinaryObject) null, (string) null, title, fileList, (FileAttachment[]) null, (DocumentIdentity[]) null, doc);
        }
        else
        {
          string str = new PdfCreator().ConvertMetafiles(fileList);
          FileInfo fileInfo = new FileInfo(str);
          using (BinaryObject data = new BinaryObject(str))
          {
            fileAttachment = (FileAttachment) this.createNativeAttachment(reason, data, fileInfo.Extension, title, (FileAttachment[]) null, (DocumentIdentity[]) null);
            Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "createNativeAttachment: filepath(" + str + ")", data.Length);
          }
        }
        if (doc != null)
        {
          bool flag = true;
          bool isActive = true;
          switch (reason)
          {
            case AddReasonType.Retrieve:
            case AddReasonType.Esign:
            case AddReasonType.Fax:
            case AddReasonType.ScanDoc:
            case AddReasonType.Upload:
              eFolderAccessRights folderAccessRights1 = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
              if (folderAccessRights1.RetrieveDocumentMethod == RetrieveDocumentMethod.AssignNotCurrent)
              {
                isActive = false;
                break;
              }
              if (folderAccessRights1.RetrieveDocumentMethod == RetrieveDocumentMethod.Unassigned)
              {
                flag = false;
                break;
              }
              break;
            case AddReasonType.Services:
              eFolderAccessRights folderAccessRights2 = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
              if (folderAccessRights2.RetrieveServiceMethod == RetrieveDocumentMethod.AssignNotCurrent)
              {
                isActive = false;
                break;
              }
              if (folderAccessRights2.RetrieveServiceMethod == RetrieveDocumentMethod.Unassigned)
              {
                flag = false;
                break;
              }
              break;
          }
          if (flag)
            doc.Files.Add(fileAttachment.ID, this.loanDataMgr.SessionObjects.UserID, isActive);
        }
        Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "AddAttachment: title(" + title + ") ends at" + DateTime.Now.ToString());
        return fileAttachment;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 700, nameof (AddAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public FileAttachment ReplaceAttachment(
      AddReasonType reason,
      NativeAttachment attachment,
      string filepath,
      DocumentLog doc,
      bool defaultActive = true)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 711, nameof (ReplaceAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "ReplaceAttachment: " + attachment.ID + ") starts at" + DateTime.Now.ToString());
        bool flag = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.UseBackgroundConversion;
        if (reason == AddReasonType.Conversion || reason == AddReasonType.ConversionForMerge)
          flag = false;
        bool createImageAttachment = this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
        if (reason == AddReasonType.Annotate || reason == AddReasonType.Edit)
          createImageAttachment = false;
        FileAttachment[] sourceList = new FileAttachment[1]
        {
          (FileAttachment) attachment
        };
        FileAttachment fileAttachment = (FileAttachment) null;
        if (flag)
        {
          FileInfo fileInfo = new FileInfo(filepath);
          using (BinaryObject data = new BinaryObject(filepath))
          {
            fileAttachment = (FileAttachment) this.createBackgroundAttachment(reason, data, fileInfo.Extension, attachment.Title, sourceList, (DocumentIdentity[]) null, doc, createImageAttachment);
            Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "createBackgroundAttachment: filepath(" + filepath + ")", data.Length);
          }
        }
        else if (createImageAttachment)
        {
          string[] fileList = new string[1]{ filepath };
          ImageAttachment imageAttachment = this.createImageAttachment(reason, (BinaryObject) null, (string) null, attachment.Title, fileList, sourceList, (DocumentIdentity[]) null, doc);
          if (imageAttachment != null)
          {
            foreach (PageImage page in imageAttachment.Pages)
              page.SetNativeKey(attachment.ID);
          }
          fileAttachment = (FileAttachment) imageAttachment;
        }
        else
        {
          FileInfo fileInfo = new FileInfo(filepath);
          using (BinaryObject data = new BinaryObject(filepath))
            fileAttachment = (FileAttachment) this.createNativeAttachment(reason, data, fileInfo.Extension, attachment.Title, sourceList, (DocumentIdentity[]) null);
        }
        if (doc != null)
        {
          int index = doc.Files.IndexOf(attachment.ID);
          if (index < 0)
            index = doc.Files.Count;
          doc.Files.Insert(index, fileAttachment.ID, this.loanDataMgr.SessionObjects.UserID, defaultActive || doc.Files[index].IsActive);
          doc.Files.Remove(attachment.ID);
        }
        attachment.IsRemoved = true;
        Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "ReplaceAttachment: " + attachment.ID + ") ends at" + DateTime.Now.ToString());
        return fileAttachment;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 792, nameof (ReplaceAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public FileAttachment[] SplitAttachment(
      NativeAttachment attachment,
      string[] fileList,
      DocumentLog[] docList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 803, nameof (SplitAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
        bool flag = true;
        if (fileList.Length == docList.Length)
          flag = false;
        bool backgroundConversion = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.UseBackgroundConversion;
        for (int index = 0; index < fileList.Length; ++index)
        {
          FileInfo fileInfo = new FileInfo(fileList[index]);
          using (BinaryObject data = new BinaryObject(fileList[index]))
          {
            string title = string.Format(attachment.Title + " ({0})", (object) (index + 1));
            FileAttachment[] sourceList = new FileAttachment[1]
            {
              (FileAttachment) attachment
            };
            DocumentLog doc = !flag ? docList[index] : docList[0];
            FileAttachment fileAttachment = !backgroundConversion ? (FileAttachment) this.createNativeAttachment(AddReasonType.Split, data, fileInfo.Extension, title, sourceList, (DocumentIdentity[]) null) : (FileAttachment) this.createBackgroundAttachment(AddReasonType.Split, data, fileInfo.Extension, title, sourceList, (DocumentIdentity[]) null, doc, false);
            fileAttachmentList.Add(fileAttachment);
            doc?.Files.Add(fileAttachment.ID, this.loanDataMgr.SessionObjects.UserID, true);
          }
        }
        this.Remove(RemoveReasonType.Split, (FileAttachment) attachment);
        return fileAttachmentList.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 861, nameof (SplitAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public FileAttachment[] SplitAttachmentForSDC(
      NativeAttachment attachment,
      string[] fileList,
      DocumentLog[] docList,
      Dictionary<string, System.Collections.Generic.List<int>> pageIndexTable,
      string[] filesWithoutEdits)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 878, nameof (SplitAttachmentForSDC), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
        bool flag = true;
        if (fileList.Length == docList.Length)
          flag = false;
        for (int index = fileList.Length - 1; index >= 0; --index)
        {
          string title = string.Format(attachment.Title + " ({0})", (object) index);
          DocumentLog documentLog = !flag ? docList[index] : docList[0];
          string key = pageIndexTable.ElementAt<KeyValuePair<string, System.Collections.Generic.List<int>>>(index).Key;
          if (key == "SourceFile")
          {
            FileInfo fileInfo = new FileInfo(fileList[index]);
            BinaryObject data = new BinaryObject(filesWithoutEdits[index]);
            FileAttachment[] sourceList = new FileAttachment[1]
            {
              (FileAttachment) attachment
            };
            FileAttachment nativeAttachment = this.createSDCNativeAttachment(AddReasonType.Split, data, fileInfo.Extension, attachment.Title, sourceList, (DocumentIdentity[]) null, fileList[index]);
            fileAttachmentList.Add(nativeAttachment);
            if (!flag && documentLog != null)
              documentLog.Files.Add(nativeAttachment.ID, this.loanDataMgr.SessionObjects.UserID, true);
          }
          else
          {
            System.Collections.Generic.List<int> pageIndexes = pageIndexTable[key];
            FileAttachment attachmentForSdcSplit = this.CreateNativeAttachmentForSDCSplit(attachment, title, fileList[index], pageIndexes, filesWithoutEdits[index]);
            fileAttachmentList.Add(attachmentForSdcSplit);
            documentLog?.Files.Add(attachmentForSdcSplit.ID, this.loanDataMgr.SessionObjects.UserID, true);
          }
        }
        this.Remove(RemoveReasonType.Split, (FileAttachment) attachment);
        return fileAttachmentList.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 946, nameof (SplitAttachmentForSDC), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    private FileAttachment CreateNativeAttachmentForSDCSplit(
      NativeAttachment nativeAttachment,
      string title,
      string splitFile,
      System.Collections.Generic.List<int> pageIndexes,
      string splitFileWithoutEdits)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 954, nameof (CreateNativeAttachmentForSDCSplit), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        string str1 = "Attachment-" + Guid.NewGuid().ToString() + ".pdf";
        string str2 = string.Empty;
        BinaryObject data = new BinaryObject(splitFileWithoutEdits);
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("FilAtmtColCrtSDCAtmtSplt", "Upload eFolder document", true, 967, nameof (CreateNativeAttachmentForSDCSplit), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs"))
        {
          SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.loanDataMgr);
          driveStreamingClient.UploadProgress += new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
          try
          {
            Task<string> task = driveStreamingClient.SaveSupportingData(str1, data, this.loanDataMgr.UseSkyDriveClassic);
            Task.WaitAll((Task) task);
            str2 = task.Result;
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: AttacchmentId-" + str1 + " uploaded with SkyDriveObjectId-" + str2);
          }
          finally
          {
            driveStreamingClient.UploadProgress -= new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
          }
          performanceMeter.AddVariable("Name", (object) title);
          performanceMeter.AddVariable("Size", (object) data.Length);
        }
        string str3 = this.downloadPath + str1;
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Write to Temp path-" + str3);
        data.Write(str3);
        SDCHelper sdcHelper = new SDCHelper(this.loanDataMgr);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), "SkyDriveClassic: Starting PDF conversion for skyDriveObjectId-" + str2);
        SDCHelper.FileDetails pdf = sdcHelper.ConvertFileToPDF(str3, str2, "version-1.pdf");
        if (pdf == null)
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), "SkyDriveClassic: Error in PDF conversion for skyDriveObjectId-" + str2);
          return (FileAttachment) null;
        }
        string empty = string.Empty;
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Preparing document json for skyDriveObjectId-" + str2);
        SDCDocument sourceDocument = nativeAttachment.CurrentSDCDocument == null ? Utils.DeepClone<SDCDocument>(nativeAttachment.OriginalSDCDocument) : Utils.DeepClone<SDCDocument>(nativeAttachment.CurrentSDCDocument);
        SDCDocument sdcDocument = new SDCDocument(1, "Incomplete", pdf.PageCount);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Calling SplitDocumentJson for skyDriveObjectId-" + str2);
        sdcHelper.SplitDocumentJson(sourceDocument, sdcDocument, pageIndexes);
        string camelCaseJsonString = sdcDocument.ToCamelCaseJsonString();
        nativeAttachment.CurrentSDCDocument = sourceDocument;
        Dictionary<string, BinaryObject> partnerFileBinaryObjects = new Dictionary<string, BinaryObject>();
        BinaryObject binaryObject1 = new BinaryObject(camelCaseJsonString, Encoding.Default);
        partnerFileBinaryObjects.Add("document.json", binaryObject1);
        BinaryObject binaryObject2 = new BinaryObject(pdf.FilePath);
        partnerFileBinaryObjects.Add("version-1.pdf", binaryObject2);
        long length = binaryObject2.Length;
        SDCHelper.FileDetails fileDetails = (SDCHelper.FileDetails) null;
        if (sourceDocument.Version != 1)
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Calling ConvertFileToPDF for skyDriveObjectId-" + str2);
          fileDetails = sdcHelper.ConvertFileToPDF(splitFile, str2, "version-2.pdf");
          if (fileDetails == null)
          {
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), "SkyDriveClassic: Error in PDF conversion for skyDriveObjectId-" + str2);
            return (FileAttachment) null;
          }
          BinaryObject binaryObject3 = new BinaryObject(fileDetails.FilePath);
          partnerFileBinaryObjects.Add("version-2.pdf", binaryObject3);
          length = binaryObject3.Length;
        }
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Calling UploadPartnerFilesForNewAttachment for skyDriveObjectId-" + str2);
        sdcHelper.UploadPartnerFilesForNewAttachment(partnerFileBinaryObjects, str2);
        NativeAttachment file = new NativeAttachment(str1, title, length, this.loanDataMgr.SessionObjects.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, new FileAttachment[1]
        {
          (FileAttachment) nativeAttachment
        }, (DocumentIdentity[]) null)
        {
          ObjectId = str2,
          OriginalSDCDocument = sdcDocument
        };
        if (fileDetails != null)
          file.SetConvertedFile(fileDetails.FilePath);
        else
          file.SetConvertedFile(pdf.FilePath);
        file.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
        file.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For CreateNativeAttachmentForSDCSplit");
        lock (this.List.SyncRoot)
          this.List.Add((object) file);
        this.loanDataMgr.LoanHistory.TrackChange((FileAttachment) file, "File created by split");
        this.OnFileAttachmentAdded((FileAttachment) file);
        if (this.isEFolderActiveUser)
        {
          ImageProperties[] images = sdcHelper.ConvertAttachmentToImages(pdf.FilePath);
          sdcHelper.UploadAttachmentImagesToSkyDrive(images, str2, sdcDocument);
        }
        return (FileAttachment) file;
      }
      catch (Exception ex)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), "CreateNativeAttachmentForSDCSplit failed. Ex: " + (object) ex);
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW", 1106, nameof (CreateNativeAttachmentForSDCSplit), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        throw;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1111, nameof (CreateNativeAttachmentForSDCSplit), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public ImageAttachment SplitAttachment(PageImage[] pageList, string title, DocumentLog doc)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1122, nameof (SplitAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
        foreach (PageImage page in pageList)
        {
          if (!fileAttachmentList.Contains((FileAttachment) page.Attachment))
            fileAttachmentList.Add((FileAttachment) page.Attachment);
        }
        ImageAttachment file = new ImageAttachment(title, this.loanDataMgr.SessionObjects.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, pageList, fileAttachmentList.ToArray(), (DocumentIdentity[]) null);
        file.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
        file.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For SplitAttachment");
        lock (this.List.SyncRoot)
          this.List.Add((object) file);
        this.OnFileAttachmentAdded((FileAttachment) file);
        if (doc != null)
        {
          int index = doc.Files.IndexOf(fileAttachmentList[0].ID);
          if (index < 0)
            index = doc.Files.Count;
          doc.Files.Insert(index, file.ID, this.loanDataMgr.SessionObjects.UserID, true);
        }
        return file;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1161, nameof (SplitAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public void Resync()
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 1170, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      try
      {
        if (this.loanDataMgr.LoanObject == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT LoanObject == null", 1177, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        }
        else
        {
          FileAttachment[] attachmentsFromServer = this.getAttachmentsFromServer();
          System.Collections.Generic.List<FileAttachment> fileAttachmentList1 = new System.Collections.Generic.List<FileAttachment>();
          System.Collections.Generic.List<FileAttachment> fileAttachmentList2 = new System.Collections.Generic.List<FileAttachment>();
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For Resync");
          lock (this.List.SyncRoot)
          {
            foreach (FileAttachment fileAttachment1 in attachmentsFromServer)
            {
              for (int index = 0; index < this.List.Count; ++index)
              {
                FileAttachment fileAttachment2 = (FileAttachment) this.List[index];
                if (!(fileAttachment2.ID != fileAttachment1.ID) && (!fileAttachment2.IsDirty || fileAttachment1.IsRemoved))
                {
                  fileAttachment1.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
                  fileAttachment1.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
                  this.List[index] = (object) fileAttachment1;
                  fileAttachmentList2.Add(fileAttachment1);
                }
              }
              if (!fileAttachmentList2.Contains(fileAttachment1))
              {
                fileAttachment1.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
                fileAttachment1.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
                this.List.Add((object) fileAttachment1);
                fileAttachmentList1.Add(fileAttachment1);
              }
            }
          }
          foreach (FileAttachment file in fileAttachmentList1)
            this.OnFileAttachmentAdded(file);
          foreach (FileAttachment file in fileAttachmentList2)
            this.OnFileAttachmentChanged(file);
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1240, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public bool Resync(CloudAttachment cloudAttachment)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 1249, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      try
      {
        bool flag = false;
        for (int index = 1; index <= 5; ++index)
        {
          if (((IEnumerable<FileAttachment>) this.getAttachmentsFromServer()).FirstOrDefault<FileAttachment>((Func<FileAttachment, bool>) (x => cloudAttachment.ID == x.ID)) is CloudAttachment cloudAttachment1)
          {
            if (cloudAttachment1.ObjectId != cloudAttachment.ObjectId)
            {
              cloudAttachment.ObjectId = cloudAttachment1.ObjectId;
              flag = true;
            }
            if (cloudAttachment1.Title != cloudAttachment.Title)
            {
              cloudAttachment.Title = cloudAttachment1.Title;
              flag = true;
            }
            if (cloudAttachment1.IsRemoved != cloudAttachment.IsRemoved)
            {
              cloudAttachment.IsRemoved = cloudAttachment1.IsRemoved;
              flag = true;
            }
          }
          if (flag)
            break;
        }
        return flag;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1301, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public FileAttachment[] Resync(string[] attachmentIds, AddReasonType reason = AddReasonType.Unknown)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 1310, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      try
      {
        if (this.loanDataMgr.LoanObject == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT LoanObject == null", 1317, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
          return (FileAttachment[]) null;
        }
        IEnumerable<string> resyncIds = ((IEnumerable<string>) attachmentIds).Distinct<string>();
        FileAttachment[] resyncList = (FileAttachment[]) null;
        for (int index = 1; index <= 5; ++index)
        {
          resyncList = ((IEnumerable<FileAttachment>) this.getAttachmentsFromServer()).Where<FileAttachment>((Func<FileAttachment, bool>) (x => resyncIds.Contains<string>(x.ID))).ToArray<FileAttachment>();
          if (resyncIds.All<string>((Func<string, bool>) (x => ((IEnumerable<FileAttachment>) resyncList).Any<FileAttachment>((Func<FileAttachment, bool>) (y => y.ID == x)))))
          {
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Resync: Located all attachments");
            break;
          }
          if (index == 5)
          {
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Warning, nameof (FileAttachmentCollection), "Resync: Failed to locate all attachments");
            PerformanceMeter.Current.AddCheckpoint("EXIT Failed to locate all attachments", 1349, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
            return (FileAttachment[]) null;
          }
          Thread.Sleep(1000);
        }
        System.Collections.Generic.List<FileAttachment> fileAttachmentList1 = new System.Collections.Generic.List<FileAttachment>();
        System.Collections.Generic.List<FileAttachment> fileAttachmentList2 = new System.Collections.Generic.List<FileAttachment>();
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For Resync");
        lock (this.List.SyncRoot)
        {
          foreach (FileAttachment file in resyncList)
          {
            file.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
            file.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
            for (int index = 0; index < this.List.Count; ++index)
            {
              if (!(((FileAttachment) this.List[index]).ID != file.ID))
              {
                this.List[index] = (object) file;
                fileAttachmentList2.Add(file);
              }
            }
            if (!fileAttachmentList2.Contains(file))
            {
              this.List.Add((object) file);
              fileAttachmentList1.Add(file);
              if (reason != AddReasonType.Unknown)
                this.loanDataMgr.LoanHistory.TrackChange(file, this.getReasonDetails(reason));
            }
          }
        }
        foreach (FileAttachment file in fileAttachmentList1)
          this.OnFileAttachmentAdded(file);
        foreach (FileAttachment file in fileAttachmentList2)
          this.OnFileAttachmentChanged(file);
        return resyncList;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1412, nameof (Resync), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public FileAttachment MergeAttachments(
      FileAttachment[] attachmentList,
      string filePath,
      DocumentLog doc)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1423, nameof (MergeAttachments), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "MergeAttachments: filepath(" + filePath + ") starts at" + DateTime.Now.ToString());
        FileInfo fileInfo = new FileInfo(filePath);
        using (BinaryObject data = new BinaryObject(filePath))
        {
          bool backgroundConversion = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.UseBackgroundConversion;
          FileAttachment fileAttachment = !this.loanDataMgr.UseSkyDriveClassic || this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments ? (!backgroundConversion ? (FileAttachment) this.createNativeAttachment(AddReasonType.Merge, data, fileInfo.Extension, "Merged File", attachmentList, (DocumentIdentity[]) null) : (FileAttachment) this.createBackgroundAttachment(AddReasonType.Merge, data, fileInfo.Extension, "Merged File", attachmentList, (DocumentIdentity[]) null, doc, false)) : this.createSDCNativeAttachment(AddReasonType.Merge, data, fileInfo.Extension, "Merged File", attachmentList, (DocumentIdentity[]) null, "");
          if (doc != null)
          {
            int index = doc.Files.IndexOf(attachmentList[0].ID);
            if (index < 0)
              index = doc.Files.Count;
            doc.Files.Insert(index, fileAttachment.ID, this.loanDataMgr.SessionObjects.UserID, true);
          }
          foreach (FileAttachment attachment in attachmentList)
            this.Remove(RemoveReasonType.Merge, attachment);
          Tracing.Log(FileAttachmentCollection.sw, nameof (FileAttachmentCollection), TraceLevel.Info, "MergeAttachments: filepath(" + filePath + ")", data.Length);
          return fileAttachment;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1468, nameof (MergeAttachments), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public ImageAttachment MergeAttachments(ImageAttachment[] imageAttachmentList, DocumentLog doc)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1479, nameof (MergeAttachments), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        System.Collections.Generic.List<PageImage> pageImageList = new System.Collections.Generic.List<PageImage>();
        foreach (ImageAttachment imageAttachment in imageAttachmentList)
        {
          foreach (PageImage page in imageAttachment.Pages)
            pageImageList.Add(page);
        }
        ImageAttachment file = new ImageAttachment("Merged File", this.loanDataMgr.SessionObjects.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, pageImageList.ToArray(), (FileAttachment[]) imageAttachmentList, (DocumentIdentity[]) null);
        file.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
        file.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For MergeAttachments");
        lock (this.List.SyncRoot)
          this.List.Add((object) file);
        this.OnFileAttachmentAdded((FileAttachment) file);
        if (doc != null)
        {
          int index = doc.Files.IndexOf(imageAttachmentList[0].ID);
          if (index < 0)
            index = doc.Files.Count;
          doc.Files.Insert(index, file.ID, this.loanDataMgr.SessionObjects.UserID, true);
        }
        foreach (FileAttachment imageAttachment in imageAttachmentList)
          this.Remove(RemoveReasonType.Merge, imageAttachment);
        return file;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1522, nameof (MergeAttachments), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    private BackgroundAttachment createBackgroundAttachment(
      AddReasonType reason,
      BinaryObject data,
      string extension,
      string title,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList,
      DocumentLog doc,
      bool createImageAttachment)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1537, nameof (createBackgroundAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (extension != null && !extension.StartsWith("."))
          extension = "." + extension;
        BackgroundAttachment backgroundAttachment;
        if (createImageAttachment)
        {
          ImageAttachmentSettings attachmentSettings = this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings;
          ImageConversionType conversionType = attachmentSettings.ConversionType;
          bool saveOriginalFormat = attachmentSettings.SaveOriginalFormat;
          if (doc != null)
          {
            DocumentTemplate byName = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(doc.Title);
            if (byName != null)
            {
              conversionType = byName.ConversionType;
              saveOriginalFormat = byName.SaveOriginalFormat;
            }
          }
          backgroundAttachment = new BackgroundAttachment(title, extension, this.loanDataMgr.SessionObjects.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, sourceList, identityList, conversionType, saveOriginalFormat, attachmentSettings.DpiX, attachmentSettings.DpiY);
        }
        else
          backgroundAttachment = new BackgroundAttachment(title, extension, this.loanDataMgr.SessionObjects.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, sourceList, identityList);
        backgroundAttachment.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
        backgroundAttachment.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For CreateBackgroundAttachment");
        lock (this.List.SyncRoot)
          this.List.Add((object) backgroundAttachment);
        this.loanDataMgr.LoanHistory.TrackChange((FileAttachment) backgroundAttachment, this.getReasonDetails(reason));
        this.OnFileAttachmentAdded((FileAttachment) backgroundAttachment);
        BackgroundAttachmentDialog.Insert(backgroundAttachment, data, this.loanDataMgr.LoanData.GUID, this.loanDataMgr.LoanData.LoanNumber, this.loanDataMgr.LoanData.GetBorrowerPairs()[0].ToString());
        return backgroundAttachment;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1600, nameof (createBackgroundAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    private NativeAttachment createNativeAttachment(
      AddReasonType reason,
      BinaryObject data,
      string extension,
      string title,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1611, nameof (createNativeAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (extension != null && !extension.StartsWith("."))
          extension = "." + extension;
        string str = "Attachment-" + Guid.NewGuid().ToString() + extension;
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("FilAtmtColCrtNtvAtmt", "DOCS NDE-13455 Upload eFolder document", true, 1621, nameof (createNativeAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs"))
        {
          if (this.loanDataMgr.UseSkyDriveLite)
          {
            SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.loanDataMgr);
            driveStreamingClient.UploadProgress += new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
            try
            {
              Task.WaitAll((Task) driveStreamingClient.SaveSupportingData(str, data));
            }
            finally
            {
              driveStreamingClient.UploadProgress -= new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
            }
          }
          else
          {
            data.UploadProgress += new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
            try
            {
              Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SaveSupportingDataOnCIFs: " + str);
              this.loanDataMgr.LoanObject.SaveSupportingDataOnCIFs(str, data);
            }
            finally
            {
              data.UploadProgress -= new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
            }
          }
          performanceMeter.AddVariable("Name", (object) title);
          performanceMeter.AddVariable("Size", (object) data.Length);
        }
        string path = this.downloadPath + str;
        if (path.ToLower().EndsWith(".findingshtml"))
          path += ".htm";
        else if (path.ToLower().EndsWith(".creditprintfile"))
          path += ".txt";
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Write: " + path);
        data.Write(path);
        NativeAttachment file = new NativeAttachment(str, title, data.Length, this.loanDataMgr.SessionObjects.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, sourceList, identityList);
        file.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
        file.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For CreateNativeAttachment");
        lock (this.List.SyncRoot)
          this.List.Add((object) file);
        this.loanDataMgr.LoanHistory.TrackChange((FileAttachment) file, this.getReasonDetails(reason));
        this.OnFileAttachmentAdded((FileAttachment) file);
        return file;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1696, nameof (createNativeAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    private FileAttachment createSDCNativeAttachment(
      AddReasonType reason,
      BinaryObject data,
      string extension,
      string title,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList,
      string sourceFilePathForSplit = "�")
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1704, nameof (createSDCNativeAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (extension != null && !extension.StartsWith("."))
          extension = "." + extension;
        string str1 = "Attachment-" + Guid.NewGuid().ToString() + extension;
        string str2 = string.Empty;
        SDCDocument sdcDocument = (SDCDocument) null;
        BinaryObject binaryObject1 = (BinaryObject) null;
        string str3 = "version-2.pdf";
        SDCHelper sdcHelper = new SDCHelper(this.loanDataMgr);
        Dictionary<string, BinaryObject> partnerFileBinaryObjects = new Dictionary<string, BinaryObject>();
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("FilAtmtColCrtSDCAtmt", "Upload eFolder document", true, 1722, nameof (createSDCNativeAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs"))
        {
          SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.loanDataMgr);
          switch (reason)
          {
            case AddReasonType.Split:
              NativeAttachment source = (NativeAttachment) sourceList[0];
              sdcDocument = source.CurrentSDCDocument == null ? source.OriginalSDCDocument : source.CurrentSDCDocument;
              ++sdcDocument.Version;
              sdcHelper.RebaseDocJSonPageIds(sdcDocument);
              str3 = string.Format(string.Format("version-{0}.pdf", (object) sdcDocument.Version));
              binaryObject1 = new BinaryObject(sourceFilePathForSplit);
              break;
            case AddReasonType.Merge:
              sdcDocument = sdcHelper.CreateMergedSDCDocument(((IEnumerable<FileAttachment>) sourceList).ToList<FileAttachment>());
              if (sdcDocument.Version == 2)
              {
                string unmodifiedMergedFile = sdcHelper.CreateUnmodifiedMergedFile(((IEnumerable<FileAttachment>) sourceList).ToList<FileAttachment>());
                binaryObject1 = data;
                data = new BinaryObject(unmodifiedMergedFile);
                break;
              }
              break;
          }
          driveStreamingClient.UploadProgress += new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
          try
          {
            Task<string> task = driveStreamingClient.SaveSupportingData(str1, data, this.loanDataMgr.UseSkyDriveClassic);
            Task.WaitAll((Task) task);
            str2 = task.Result;
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: AttacchmentId-" + str1 + " uploaded with SkyDriveObjectId-" + str2);
          }
          finally
          {
            driveStreamingClient.UploadProgress -= new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
          }
          performanceMeter.AddVariable("Name", (object) title);
          performanceMeter.AddVariable("Size", (object) data.Length);
        }
        string str4 = this.downloadPath + str1;
        if (str4.ToLower().EndsWith(".findingshtml"))
          str4 += ".htm";
        else if (str4.ToLower().EndsWith(".creditprintfile"))
          str4 += ".txt";
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Write to Temp path-" + str4);
        data.Write(str4);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Starting PDF conversion for skyDriveObjectId-" + str2);
        SDCHelper.FileDetails pdf = sdcHelper.ConvertFileToPDF(str4, str2, "version-1.pdf");
        if (pdf == null)
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), "SkyDriveClassic: Error in PDF conversion for skyDriveObjectId-" + str2);
          return (FileAttachment) null;
        }
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Preparing document json for skyDriveObjectId-" + str2);
        if (sdcDocument == null)
          sdcDocument = new SDCDocument(1, "Incomplete", pdf.PageCount);
        string data1 = JsonConvert.SerializeObject((object) sdcDocument, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver(),
          DefaultValueHandling = DefaultValueHandling.Ignore
        });
        string str5 = pdf.FilePath;
        BinaryObject binaryObject2 = new BinaryObject(data1, Encoding.Default);
        partnerFileBinaryObjects.Add("document.json", binaryObject2);
        BinaryObject binaryObject3 = new BinaryObject(pdf.FilePath);
        partnerFileBinaryObjects.Add("version-1.pdf", binaryObject3);
        binaryObject3.Write(str5);
        if (binaryObject1 != null)
        {
          partnerFileBinaryObjects.Add(str3, binaryObject1);
          string withGivenFileName = SystemSettings.GetTempFileNameWithGivenFileName(str2, str3);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SkyDriveClassic: Write to local path-" + withGivenFileName);
          binaryObject1.Write(withGivenFileName);
          str5 = withGivenFileName;
        }
        sdcHelper.UploadPartnerFilesForNewAttachment(partnerFileBinaryObjects, str2);
        NativeAttachment file = new NativeAttachment(str1, title, data.Length, this.loanDataMgr.SessionObjects.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, sourceList, identityList)
        {
          ObjectId = str2,
          OriginalSDCDocument = sdcDocument
        };
        file.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
        file.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For createSDCNativeAttachment");
        lock (this.List.SyncRoot)
          this.List.Add((object) file);
        this.loanDataMgr.LoanHistory.TrackChange((FileAttachment) file, this.getReasonDetails(reason));
        this.OnFileAttachmentAdded((FileAttachment) file);
        file.SetConvertedFile(str5);
        if (this.isEFolderActiveUser)
        {
          ImageProperties[] images = sdcHelper.ConvertAttachmentToImages(pdf.FilePath);
          sdcHelper.UploadAttachmentImagesToSkyDrive(images, str2, sdcDocument);
        }
        return (FileAttachment) file;
      }
      catch (Exception ex)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), "createSDCNativeAttachment failed. Ex: " + (object) ex);
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW", 1878, nameof (createSDCNativeAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        throw;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1883, nameof (createSDCNativeAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    private ImageAttachment createImageAttachment(
      AddReasonType reason,
      BinaryObject data,
      string extension,
      string title,
      string[] fileList,
      FileAttachment[] sourceList,
      DocumentIdentity[] identityList,
      DocumentLog doc)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("FilAtmtColCrtImg", "DOCS Create eFolder images/thumbnails", true, 1893, nameof (createImageAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1895, nameof (createImageAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (extension != null && extension.StartsWith("."))
          extension = extension.Substring(1);
        if (fileList == null)
        {
          string nameWithExtension = SystemSettings.GetTempFileNameWithExtension(extension);
          data.Write(nameWithExtension);
          fileList = new string[1]{ nameWithExtension };
        }
        ImageAttachmentSettings attachmentSettings = this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings;
        ImageConversionType conversionType = attachmentSettings.ConversionType;
        bool saveOriginalFormat = attachmentSettings.SaveOriginalFormat;
        if (doc != null)
        {
          DocumentTemplate byName = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(doc.Title);
          if (byName != null)
          {
            conversionType = byName.ConversionType;
            saveOriginalFormat = byName.SaveOriginalFormat;
          }
        }
        ImageCreator imageCreator = new ImageCreator(this.downloadPath, conversionType, (float) attachmentSettings.DpiX, (float) attachmentSettings.DpiY);
        imageCreator.ProgressChanged += new ProgressEventHandler(this.creator_ProgressChanged);
        System.Collections.Generic.List<PageImage> pageImageList = new System.Collections.Generic.List<PageImage>();
        try
        {
          string str1 = string.Empty;
          string str2 = string.Empty;
          long num = 2000000;
          IDictionary efolderSettings = this.loanDataMgr.SessionObjects.StartupInfo.EFolderSettings;
          if (efolderSettings != null && efolderSettings.Contains((object) "eFolder.MaxZipSize"))
            num = Convert.ToInt64(efolderSettings[(object) "eFolder.MaxZipSize"]);
          System.Collections.Generic.List<ImageProperties> imagePropertiesList = new System.Collections.Generic.List<ImageProperties>();
          foreach (string file in fileList)
          {
            ImageProperties[] collection = imageCreator.ConvertFile(file);
            imagePropertiesList.AddRange((IEnumerable<ImageProperties>) collection);
            str1 = Path.GetFileNameWithoutExtension(file);
            str2 = imageCreator.ConverterUsed;
          }
          PerformanceMeter.Current.AddCheckpoint("After Convert File Loop", 1961, nameof (createImageAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
          if (imagePropertiesList.Count == 0)
            throw new Exception("Failed to convert. Please check the page sizes and (if needed) reduce to letter or legal size and reattach for conversion.");
          System.Collections.Generic.List<string> stringList = new System.Collections.Generic.List<string>();
          ZipWriter zipWriter1 = (ZipWriter) null;
          string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("zip");
          using (ZipWriter zipWriter2 = new ZipWriter(nameWithExtension, 0))
          {
            string str3 = (string) null;
            foreach (ImageProperties properties in imagePropertiesList)
            {
              if (zipWriter1 != null)
              {
                FileInfo fileInfo = new FileInfo(properties.ImageFile);
                if (zipWriter1.Size + fileInfo.Length > num)
                {
                  zipWriter1.CreateZip();
                  zipWriter1.Dispose();
                  zipWriter1 = (ZipWriter) null;
                }
              }
              if (zipWriter1 == null)
              {
                str3 = SystemSettings.GetTempFileNameWithExtension("zip");
                zipWriter1 = new ZipWriter(str3, 0);
                stringList.Add(str3);
              }
              zipWriter1.AddFile(properties.ImageFile);
              zipWriter2.AddFile(properties.Thumbnail.ImageFile);
              PageImage pageImage = new PageImage(properties);
              pageImage.SetZipKey(str3);
              pageImageList.Add(pageImage);
            }
            zipWriter1.CreateZip();
            zipWriter1.Dispose();
            zipWriter2.CreateZip();
          }
          PerformanceMeter.Current.AddCheckpoint("AFter thumbnailZipWriter", 2020, nameof (createImageAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
          this.totalBytes = new FileInfo(nameWithExtension).Length;
          this.uploadedBytes = 0L;
          foreach (string fileName in stringList)
            this.totalBytes += new FileInfo(fileName).Length;
          if (saveOriginalFormat && data != null)
          {
            this.totalBytes += data.Length;
            string str4 = "Attachment-" + Guid.NewGuid().ToString() + "." + extension;
            this.uploadFile(str4, data);
            string path = this.downloadPath + str4;
            if (path.ToLower().EndsWith(".findingshtml"))
              path += ".htm";
            else if (path.ToLower().EndsWith(".creditprintfile"))
              path += ".txt";
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Write: " + path);
            data.Write(path);
            foreach (PageImage pageImage in pageImageList)
              pageImage.SetNativeKey(str4);
          }
          string str5 = "Thumbnails-" + Guid.NewGuid().ToString() + ".zip";
          using (BinaryObject data1 = new BinaryObject(nameWithExtension))
          {
            this.uploadFile(str5, data1);
            PerformanceMeter.Current.AddCheckpoint("Thumbnail Upload", 2066, nameof (createImageAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
          }
          foreach (PageImage pageImage in pageImageList)
            pageImage.Thumbnail.SetZipKey(str5);
          foreach (string path in stringList)
          {
            string str6 = "Images-" + Guid.NewGuid().ToString() + ".zip";
            using (BinaryObject data2 = new BinaryObject(path))
              this.uploadFile(str6, data2);
            foreach (PageImage pageImage in pageImageList)
            {
              if (pageImage.ZipKey == path)
                pageImage.SetZipKey(str6);
            }
          }
          PerformanceMeter.Current.AddCheckpoint("After Zip Upload Loop", 2088, nameof (createImageAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
          performanceMeter.AddVariable("Name", (object) title);
          performanceMeter.AddVariable("FileGuid", (object) str1);
          if (data != null)
            performanceMeter.AddVariable("Size", (object) data.Length);
          if (pageImageList != null)
            performanceMeter.AddVariable("PageCount", (object) pageImageList.Count);
          performanceMeter.AddVariable("FileType", (object) extension);
          performanceMeter.AddVariable("ImageConverter", (object) str2);
          performanceMeter.AddVariable("Time", (object) DateTime.Now);
          performanceMeter.AddVariable("ClientId", (object) this.loanDataMgr.SessionObjects.CompanyInfo.ClientID);
          performanceMeter.AddVariable("UserId", (object) this.loanDataMgr.SessionObjects.UserID);
          string userId = reason == AddReasonType.Conversion ? sourceList[0].UserID : this.loanDataMgr.SessionObjects.UserID;
          string userName = reason == AddReasonType.Conversion ? sourceList[0].UserName : this.loanDataMgr.SessionObjects.UserInfo.FullName;
          ImageAttachment file1 = new ImageAttachment(title, userId, userName, pageImageList.ToArray(), sourceList, identityList);
          file1.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
          file1.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
          if (reason == AddReasonType.Conversion && sourceList != null && sourceList.Length != 0)
          {
            FieldInfo field1 = sourceList[0].GetType().GetField("date", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo field2 = file1.GetType().GetField("date", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field1 != (FieldInfo) null && field2 != (FieldInfo) null)
              field2.SetValue((object) file1, field1.GetValue((object) sourceList[0]));
          }
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For CreateImageAttachment");
          lock (this.List.SyncRoot)
            this.List.Add((object) file1);
          this.loanDataMgr.LoanHistory.TrackChange((FileAttachment) file1, this.getReasonDetails(reason));
          this.OnFileAttachmentAdded((FileAttachment) file1);
          return file1;
        }
        finally
        {
          imageCreator.ProgressChanged -= new ProgressEventHandler(this.creator_ProgressChanged);
          PerformanceMeter.Current.AddCheckpoint("END", 2141, nameof (createImageAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        }
      }
    }

    private CloudAttachment createCloudAttachment(
      AddReasonType reason,
      BinaryObject data,
      string extension,
      string title,
      DocumentIdentity[] identityList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2153, nameof (createCloudAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (extension != null && !extension.StartsWith("."))
          extension = "." + extension;
        string str = "Attachment-" + Guid.NewGuid().ToString() + extension;
        string contentType = FileContentTypes.GetContentType(str);
        long length = data.Length;
        CreateAttachmentUrlResponse result;
        try
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Creating EOSClient");
          EOSClient eosClient = new EOSClient(this.loanDataMgr);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling CreateAttachmentUrl");
          Task<CreateAttachmentUrlResponse> attachmentUrl = eosClient.CreateAttachmentUrl(reason, str, title, length, contentType);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Waiting for CreateAttachmentUrl Task");
          Task.WaitAll((Task) attachmentUrl);
          result = attachmentUrl.Result;
        }
        catch (Exception ex)
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), "CreateAttachmentUrl failed. Ex: " + (object) ex);
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW", 2187, nameof (createCloudAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
          throw;
        }
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Creating SkyDriveStreamingClient");
        SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.loanDataMgr);
        driveStreamingClient.UploadProgress += new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
        try
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling UploadAttachment: " + result.attachmentId);
          Task<string> task = driveStreamingClient.UploadAttachment(result, data, contentType);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Waiting for UploadAttachment Task");
          Task.WaitAll((Task) task);
        }
        catch (Exception ex)
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), "UploadAttachment failed. Ex: " + (object) ex);
          throw;
        }
        finally
        {
          driveStreamingClient.UploadProgress -= new DownloadProgressEventHandler(this.nativeAttachment_uploadProgress);
        }
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling Resync: " + result.attachmentId);
        FileAttachment[] fileAttachmentArray = this.Resync(new string[1]
        {
          result.attachmentId
        }, reason);
        if (fileAttachmentArray == null)
          throw new Exception("Cloud Attachment was not created: " + result.attachmentId);
        if (identityList != null)
        {
          foreach (DocumentIdentity identity in identityList)
            fileAttachmentArray[0].Identities.Add(identity);
          fileAttachmentArray[0].Identities.Complete = true;
        }
        return (CloudAttachment) fileAttachmentArray[0];
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2243, nameof (createCloudAttachment), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    private string getReasonDetails(AddReasonType reason)
    {
      string reasonDetails = (string) null;
      switch (reason)
      {
        case AddReasonType.Browse:
          reasonDetails = "File created by browse and attach";
          break;
        case AddReasonType.Forms:
          reasonDetails = "File created by attach forms";
          break;
        case AddReasonType.Scan:
          reasonDetails = "File created by scan and attach";
          break;
        case AddReasonType.Retrieve:
          reasonDetails = "File created by retrieve";
          break;
        case AddReasonType.Services:
          reasonDetails = "File created by services";
          break;
        case AddReasonType.Printer:
          reasonDetails = "File created by eFolder printer";
          break;
        case AddReasonType.SDK:
          reasonDetails = "File created by SDK";
          break;
        case AddReasonType.Esign:
          reasonDetails = "File created by eSign";
          break;
        case AddReasonType.Fax:
          reasonDetails = "File created by fax";
          break;
        case AddReasonType.ScanDoc:
          reasonDetails = "File created by ScanDoc";
          break;
        case AddReasonType.Upload:
          reasonDetails = "File created by upload";
          break;
      }
      return reasonDetails;
    }

    private void uploadFile(string key, BinaryObject data)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("FilAtmtColImgUpld", "DOCS NDE-13455 Upload eFolder document", true, false, true, 2280, nameof (uploadFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs"))
      {
        if (this.loanDataMgr.UseSkyDriveLite)
        {
          SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.loanDataMgr);
          driveStreamingClient.UploadProgress += new DownloadProgressEventHandler(this.uploadFile_UploadProgress);
          try
          {
            Task.WaitAll((Task) driveStreamingClient.SaveSupportingData(key, data));
          }
          finally
          {
            driveStreamingClient.UploadProgress -= new DownloadProgressEventHandler(this.uploadFile_UploadProgress);
          }
        }
        else
        {
          data.UploadProgress += new DownloadProgressEventHandler(this.uploadFile_UploadProgress);
          try
          {
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "SaveSupportingDataOnCIFs: " + key);
            this.loanDataMgr.LoanObject.SaveSupportingDataOnCIFs(key, data);
          }
          finally
          {
            data.UploadProgress -= new DownloadProgressEventHandler(this.uploadFile_UploadProgress);
          }
        }
        performanceMeter.AddVariable("Time", (object) DateTime.Now);
        performanceMeter.AddVariable("ClientId", (object) this.loanDataMgr.SessionObjects.CompanyInfo.ClientID);
        performanceMeter.AddVariable("UserId", (object) this.loanDataMgr.SessionObjects.UserID);
        performanceMeter.AddVariable("Name", (object) key);
        performanceMeter.AddVariable("Size", (object) data.Length);
      }
      this.uploadedBytes += data.Length;
      this.OnFileAttachmentUploadProgress(new TransferProgressEventArgs(50 + Convert.ToInt32(50L * this.uploadedBytes / this.totalBytes)));
    }

    private void uploadFile_UploadProgress(object source, DownloadProgressEventArgs e)
    {
      TransferProgressEventArgs e1 = new TransferProgressEventArgs(50 + Convert.ToInt32(50L * (this.uploadedBytes + e.BytesDownloaded) / this.totalBytes));
      this.OnFileAttachmentUploadProgress(e1);
      if (!e1.Cancel)
        return;
      e.Cancel = true;
    }

    private void creator_ProgressChanged(object source, ProgressEventArgs e)
    {
      TransferProgressEventArgs e1 = new TransferProgressEventArgs(e.PercentCompleted / 2);
      this.OnFileAttachmentUploadProgress(e1);
      if (!e1.Cancel)
        return;
      e.Abort = true;
    }

    protected void nativeAttachment_uploadProgress(object source, DownloadProgressEventArgs e)
    {
      TransferProgressEventArgs e1 = new TransferProgressEventArgs(e);
      this.OnFileAttachmentUploadProgress(e1);
      if (!e1.Cancel)
        return;
      e.Cancel = true;
    }

    public new int Count => this.GetAllFiles().Length;

    public FileAttachment[] GetAllFiles() => this.GetAllFiles(true, false);

    public FileAttachment[] GetAllFiles(bool checkAccess, bool includeRemoved)
    {
      System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For GetAllFiles");
      lock (this.List.SyncRoot)
      {
        foreach (FileAttachment fileAttachment in (IEnumerable) this.List)
        {
          if ((includeRemoved || !fileAttachment.IsRemoved) && !DragDropDetailCollection.IsAttachmentHidden(this.loanDataMgr.LoanData.GUID, fileAttachment.ID))
            fileAttachmentList.Add(fileAttachment);
        }
      }
      return fileAttachmentList.ToArray();
    }

    public new IEnumerator GetEnumerator() => this.GetAllFiles().GetEnumerator();

    public FileAttachment this[int index] => this.GetAllFiles()[index];

    public FileAttachment this[string id] => this[id, true, false];

    public FileAttachment this[string id, bool checkAccess, bool includeRemoved]
    {
      get
      {
        foreach (FileAttachment allFile in this.GetAllFiles(checkAccess, includeRemoved))
        {
          if (allFile.ID == id)
            return allFile;
        }
        return (FileAttachment) null;
      }
    }

    public bool ContainsAttachment(DocumentLog doc)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For ContainsAttachment");
      lock (this.List.SyncRoot)
      {
        foreach (FileAttachment fileAttachment in (IEnumerable) this.List)
        {
          if (doc.Files.Contains(fileAttachment.ID) && !DragDropDetailCollection.IsAttachmentHidden(this.loanDataMgr.LoanData.GUID, fileAttachment.ID))
            return true;
        }
      }
      return false;
    }

    public FileAttachment GetAttachment(FileAttachmentReference fileRef)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For GetAttachment");
      lock (this.List.SyncRoot)
      {
        foreach (FileAttachment attachment in (IEnumerable) this.List)
        {
          if (attachment.ID == fileRef.AttachmentID && !DragDropDetailCollection.IsAttachmentHidden(this.loanDataMgr.LoanData.GUID, attachment.ID))
            return attachment;
        }
      }
      return (FileAttachment) null;
    }

    public FileAttachment GetHiddenAttachment(string attachmentId)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For GetHiddenAttachment");
      lock (this.List.SyncRoot)
      {
        foreach (FileAttachment hiddenAttachment in (IEnumerable) this.List)
        {
          if (hiddenAttachment.ID == attachmentId && DragDropDetailCollection.IsAttachmentHidden(this.loanDataMgr.LoanData.GUID, hiddenAttachment.ID))
            return hiddenAttachment;
        }
      }
      return (FileAttachment) null;
    }

    public FileAttachment[] GetAttachments(DocumentLog doc) => this.GetAttachments(doc, true);

    public FileAttachment[] GetAttachments(DocumentLog doc, bool checkActive)
    {
      System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
      foreach (FileAttachmentReference fileRef in doc.Files.ToArray())
      {
        if (!checkActive || fileRef.IsActive)
        {
          FileAttachment attachment = this.GetAttachment(fileRef);
          if (attachment != null)
            fileAttachmentList.Add(attachment);
        }
      }
      return fileAttachmentList.ToArray();
    }

    public FileAttachment[] GetAttachments(DocumentLog[] docList)
    {
      System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
      foreach (DocumentLog doc in docList)
      {
        FileAttachment[] attachments = this.GetAttachments(doc, true);
        fileAttachmentList.AddRange((IEnumerable<FileAttachment>) attachments);
      }
      return fileAttachmentList.ToArray();
    }

    public FileAttachment[] GetUnassignedAttachments() => this.GetUnassignedAttachments(false);

    public FileAttachment[] GetUnassignedAttachments(bool includeRemoved)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2586, nameof (GetUnassignedAttachments), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        DocumentLog[] allDocuments = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments(false);
        System.Collections.Generic.List<string> stringList = new System.Collections.Generic.List<string>();
        foreach (DocumentLog documentLog in allDocuments)
        {
          foreach (FileAttachmentReference attachmentReference in documentLog.Files.ToArray())
            stringList.Add(attachmentReference.AttachmentID);
        }
        FileAttachment[] allFiles = this.GetAllFiles(false, includeRemoved);
        if (includeRemoved)
        {
          foreach (FileAttachment fileAttachment in allFiles)
            stringList.AddRange((IEnumerable<string>) fileAttachment.Sources);
        }
        System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
        foreach (FileAttachment fileAttachment in allFiles)
        {
          if (!stringList.Contains(fileAttachment.ID) && !DragDropDetailCollection.IsAttachmentHidden(this.loanDataMgr.LoanData.GUID, fileAttachment.ID))
            fileAttachmentList.Add(fileAttachment);
        }
        return fileAttachmentList.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2616, nameof (GetUnassignedAttachments), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public DocumentLog GetLinkedDocument(string attachmentID)
    {
      return this.GetLinkedDocument(attachmentID, true);
    }

    public DocumentLog GetLinkedDocument(string attachmentID, bool checkActive)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2632, nameof (GetLinkedDocument), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        foreach (DocumentLog allDocument in this.loanDataMgr.LoanData.GetLogList().GetAllDocuments(false))
        {
          if (allDocument.Files.Contains(attachmentID, checkActive))
            return allDocument;
        }
        return (DocumentLog) null;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2647, nameof (GetLinkedDocument), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public bool Remove(RemoveReasonType reason, FileAttachment attachment)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2658, nameof (Remove), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (this.ReadOnly)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW Read Only", 2663, nameof (Remove), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
          throw new NotSupportedException("File attachment collection is read-only");
        }
        foreach (DocumentLog allDocument in this.loanDataMgr.LoanData.GetLogList().GetAllDocuments())
          allDocument.Files.Remove(attachment.ID);
        if (attachment is BackgroundAttachment)
          BackgroundAttachmentDialog.Remove(attachment.ID);
        attachment.IsRemoved = true;
        string details = (string) null;
        switch (reason)
        {
          case RemoveReasonType.User:
            details = "File deleted";
            break;
          case RemoveReasonType.Split:
            details = "File split";
            break;
          case RemoveReasonType.Merge:
            details = "File merged";
            break;
          case RemoveReasonType.SDK:
            details = "File deleted by SDK";
            break;
        }
        this.loanDataMgr.LoanHistory.TrackChange(attachment, details);
        this.OnFileAttachmentRemoved(attachment);
        return true;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2698, nameof (Remove), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public void Remove(RemoveReasonType reason, string id)
    {
      FileAttachment attachment = this[id];
      if (attachment == null)
        return;
      this.Remove(reason, attachment);
    }

    public void Remove(DocumentLog doc)
    {
    }

    public event FileAttachmentEventHandler FileAttachmentAdded;

    public event FileAttachmentEventHandler FileAttachmentChanged;

    public event FileAttachmentEventHandler FileAttachmentRemoved;

    public event TransferProgressEventHandler FileAttachmentUploadProgress;

    public event TransferProgressEventHandler FileAttachmentDownloadProgress;

    public event ExtractProgressEventHandler PageDownloaded;

    public event ExtractProgressEventHandler ThumbnailDownloaded;

    public event FileAttachmentEventHandler BackgroundAttachmentUploaded;

    internal void OnFileAttachmentAdded(FileAttachment file)
    {
      if (this.loanDataMgr.LoanData != null)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Setting LoanData.Dirty Flag");
        this.loanDataMgr.LoanData.Dirty = true;
      }
      if (this.FileAttachmentAdded == null)
        return;
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Raising FileAttachmentAdded Event");
      this.FileAttachmentAdded((object) this, new FileAttachmentEventArgs(file));
    }

    public void OnFileAttachmentChanged(FileAttachment file)
    {
      if (this.loanDataMgr.LoanData != null)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Setting LoanData.Dirty Flag");
        this.loanDataMgr.LoanData.Dirty = true;
      }
      if (this.FileAttachmentChanged == null)
        return;
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Raising FileAttachmentChanged Event");
      this.FileAttachmentChanged((object) this, new FileAttachmentEventArgs(file));
    }

    internal void OnFileAttachmentRemoved(FileAttachment file)
    {
      if (this.loanDataMgr.LoanData != null)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Setting LoanData.Dirty Flag");
        this.loanDataMgr.LoanData.Dirty = true;
      }
      if (this.FileAttachmentRemoved == null)
        return;
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Raising FileAttachmentRemoved Event");
      this.FileAttachmentRemoved((object) this, new FileAttachmentEventArgs(file));
    }

    internal void OnFileAttachmentUploadProgress(TransferProgressEventArgs e)
    {
      if (this.FileAttachmentUploadProgress == null)
        return;
      this.FileAttachmentUploadProgress((object) this, e);
    }

    internal void OnBackgroundAttachmentUploaded(FileAttachment file)
    {
      if (this.loanDataMgr.LoanData != null)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Setting LoanData.Dirty Flag");
        this.loanDataMgr.LoanData.Dirty = true;
      }
      if (this.BackgroundAttachmentUploaded == null)
        return;
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Raising BackgroundAttachmentUploaded Event");
      this.BackgroundAttachmentUploaded((object) this, new FileAttachmentEventArgs(file));
    }

    public string CreatePdf(ImageAttachment imageAttachment)
    {
      return this.CreatePdf(imageAttachment, AnnotationExportType.Public);
    }

    public string CreatePdf(
      ImageAttachment imageAttachment,
      AnnotationExportType annotationExportType)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2882, nameof (CreatePdf), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        this.DownloadAttachment(imageAttachment);
        System.Collections.Generic.List<string> stringList = new System.Collections.Generic.List<string>();
        foreach (PageImage page in imageAttachment.Pages)
        {
          string imageFile = this.GetImageFile(page);
          stringList.Add(imageFile);
        }
        System.Collections.Generic.List<PdfTextAnnotation> pdfTextAnnotationList = new System.Collections.Generic.List<PdfTextAnnotation>();
        if (annotationExportType != AnnotationExportType.None)
        {
          for (int index = 0; index < imageAttachment.Pages.Count; ++index)
          {
            foreach (PageAnnotation annotation in imageAttachment.Pages[index].Annotations)
            {
              if (annotationExportType == AnnotationExportType.All || annotationExportType == AnnotationExportType.Personal && annotation.Visibility == PageAnnotationVisibilityType.Personal || annotationExportType == AnnotationExportType.Public && annotation.Visibility == PageAnnotationVisibilityType.Public)
                pdfTextAnnotationList.Add(new PdfTextAnnotation()
                {
                  Title = annotation.AddedBy + " - " + annotation.Date.ToString("MM/dd/yy hh:mm tt"),
                  Contents = annotation.Text,
                  Color = Color.FromArgb(254, 228, 57),
                  Icon = "Comment",
                  PageIndex = index + 1,
                  X = (float) annotation.Left,
                  Y = (float) annotation.Top
                });
            }
          }
        }
        return new PdfCreator().MergeImages(stringList.ToArray(), pdfTextAnnotationList.ToArray());
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2932, nameof (CreatePdf), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public string CreatePdf(
      CloudAttachment[] cloudAttachments,
      AnnotationExportType annotationExportType)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 2943, nameof (CreatePdf), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      try
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Creating EOSClient");
        EOSClient eosClient = new EOSClient(this.loanDataMgr);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling CreateExportJob");
        Task<string> exportJob = eosClient.CreateExportJob((FileAttachment[]) cloudAttachments, annotationExportType);
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Waiting for CreateExportJob Task");
        Task.WaitAll((Task) exportJob);
        string result = exportJob.Result;
        SkyDriveUrl url;
        Task<SkyDriveUrl> task1;
        for (url = (SkyDriveUrl) null; url == null; url = task1.Result)
        {
          if (this.FileAttachmentDownloadProgress != null)
          {
            TransferProgressEventArgs e = new TransferProgressEventArgs(0);
            this.FileAttachmentDownloadProgress((object) this, e);
            if (e.Cancel)
              throw new CanceledOperationException();
          }
          Thread.Sleep(1000);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling CheckExportJobStatus: " + result);
          task1 = eosClient.CheckExportJobStatus(result);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Waiting for CreateExportJob Task: " + result);
          Task.WaitAll((Task) task1);
        }
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Creating SkyDriveStreamingClient");
        SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.loanDataMgr);
        driveStreamingClient.DownloadProgress += new DownloadProgressEventHandler(this.downloadFile_downloadProgress);
        try
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling DownloadFile: " + result);
          Task<string> task2 = driveStreamingClient.DownloadFile(url, "Export-" + result + ".pdf");
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Waiting for DownloadFile Task: " + result);
          Task.WaitAll((Task) task2);
          return task2.Result;
        }
        finally
        {
          driveStreamingClient.DownloadProgress -= new DownloadProgressEventHandler(this.downloadFile_downloadProgress);
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3017, nameof (CreatePdf), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public string CreateSDCNativePdf(NativeAttachment attachment)
    {
      return new SDCHelper(this.loanDataMgr).GetConvertedVersionFile((FileAttachment) attachment);
    }

    public string GetAutoSaveXml()
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3042, nameof (GetAutoSaveXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        XmlDocument parentDocument = new XmlDocument();
        parentDocument.LoadXml("<Attachments/>");
        if (this.ReadOnly)
          return parentDocument.OuterXml;
        foreach (FileAttachment allFile in this.GetAllFiles(false, true))
        {
          if (allFile.IsDirty)
          {
            XmlNode newChild = (XmlNode) parentDocument.DocumentElement;
            if (allFile.IsRemoved)
            {
              newChild = parentDocument.DocumentElement.SelectSingleNode("Removed");
              if (newChild == null)
              {
                newChild = (XmlNode) parentDocument.CreateElement("Removed");
                parentDocument.DocumentElement.AppendChild(newChild);
              }
            }
            string name = (string) null;
            switch (allFile)
            {
              case BackgroundAttachment _:
                name = "Background";
                break;
              case ImageAttachment _:
                name = "Image";
                break;
              case NativeAttachment _:
                name = "File";
                break;
              case CloudAttachment _:
                name = "Cloud";
                break;
            }
            XmlElement element = parentDocument.CreateElement(name);
            newChild.AppendChild((XmlNode) element);
            allFile.ToXml(element);
            if (this.loanDataMgr.UseSkyDriveClassic && name == "File")
              SDCUtils.AppendLoanAutoSaveSDCXmls(allFile, parentDocument, element);
          }
        }
        return parentDocument.OuterXml;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3101, nameof (GetAutoSaveXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public void LoadAutoSaveXml(string xml)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3112, nameof (LoadAutoSaveXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
        foreach (XmlElement childNode1 in xmlDocument.DocumentElement.ChildNodes)
        {
          if (childNode1.Name == "File")
          {
            NativeAttachment attachment = new NativeAttachment(childNode1, false);
            if (this.loanDataMgr.UseSkyDriveClassic)
              SDCUtils.LoadAutoSaveSDCDocuments((FileAttachment) attachment, childNode1);
            fileAttachmentList.Add((FileAttachment) attachment);
          }
          else if (childNode1.Name == "Image")
          {
            ImageAttachment imageAttachment = new ImageAttachment(childNode1, false);
            fileAttachmentList.Add((FileAttachment) imageAttachment);
          }
          else if (childNode1.Name == "Cloud")
          {
            CloudAttachment cloudAttachment = new CloudAttachment(childNode1, false);
            fileAttachmentList.Add((FileAttachment) cloudAttachment);
          }
          else if (childNode1.Name == "Background")
          {
            BackgroundAttachment backgroundAttachment = new BackgroundAttachment(childNode1, false);
            fileAttachmentList.Add((FileAttachment) backgroundAttachment);
          }
          else if (childNode1.Name == "Removed")
          {
            foreach (XmlElement childNode2 in childNode1.ChildNodes)
            {
              if (childNode2.Name == "File")
              {
                NativeAttachment nativeAttachment = new NativeAttachment(childNode2, true);
                fileAttachmentList.Add((FileAttachment) nativeAttachment);
              }
              else if (childNode2.Name == "Image")
              {
                ImageAttachment imageAttachment = new ImageAttachment(childNode2, true);
                fileAttachmentList.Add((FileAttachment) imageAttachment);
              }
              else if (childNode1.Name == "Cloud")
              {
                CloudAttachment cloudAttachment = new CloudAttachment(childNode1, true);
                fileAttachmentList.Add((FileAttachment) cloudAttachment);
              }
              else if (childNode1.Name == "Background")
              {
                BackgroundAttachment backgroundAttachment = new BackgroundAttachment(childNode2, true);
                fileAttachmentList.Add((FileAttachment) backgroundAttachment);
              }
            }
          }
        }
        foreach (FileAttachment file in fileAttachmentList)
        {
          if (!(file is BackgroundAttachment) || BackgroundAttachmentDialog.IsProcessing(file.ID))
          {
            file.MarkAsDirty();
            bool flag = false;
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Locking base.List.SyncRoot For LoadAutoSaveXml");
            lock (this.List.SyncRoot)
            {
              for (int index = 0; index < this.List.Count; ++index)
              {
                FileAttachment fileAttachment = (FileAttachment) this.List[index];
                if (fileAttachment.ID == file.ID)
                {
                  if (fileAttachment.IsRemoved)
                    file.IsRemoved = true;
                  file.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
                  file.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
                  this.List[index] = (object) file;
                  flag = true;
                }
              }
              if (!flag)
              {
                file.AttachLoanHistoryMonitor((IFileHistoryMonitor) this.loanDataMgr.LoanHistory);
                file.FileAttachmentChanged += new EventHandler(this.attachment_FileAttachmentChanged);
                this.List.Add((object) file);
              }
            }
            if (flag)
              this.OnFileAttachmentChanged(file);
            else
              this.OnFileAttachmentAdded(file);
          }
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3245, nameof (LoadAutoSaveXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    internal void SaveXml()
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3256, nameof (SaveXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        if (this.ReadOnly)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT ReadOnly", 3261, nameof (SaveXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        }
        else
        {
          FileAttachment[] allFiles = this.GetAllFiles(false, true);
          System.Collections.Generic.List<FileAttachment> fileAttachmentList = new System.Collections.Generic.List<FileAttachment>();
          foreach (FileAttachment fileAttachment in allFiles)
          {
            if (fileAttachment.IsDirty)
              fileAttachmentList.Add(fileAttachment);
          }
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling SaveFileAttachments");
          this.loanDataMgr.LoanObject.SaveFileAttachments(fileAttachmentList.ToArray());
          foreach (FileAttachment fileAttachment in fileAttachmentList)
            fileAttachment.MarkAsClean();
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3284, nameof (SaveXml), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public string DownloadAttachment(NativeAttachment attachment)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "DownloadNative: " + attachment.ID);
      lock (this.nativeDownloadLock)
        return this.loanDataMgr.UseSkyDriveClassic ? this.downloadFile(attachment.ID, attachment.ObjectId) : this.downloadFile(attachment.ID, "");
    }

    public string[] DownloadOriginalAttachments(NativeAttachment attachment)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "DownloadOriginalNative: " + attachment.ID);
      return this.GetOriginalAttachments(attachment.ID);
    }

    private string[] GetOriginalAttachments(string attachmentID)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3328, nameof (GetOriginalAttachments), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        System.Collections.Generic.List<string> stringList = new System.Collections.Generic.List<string>();
        FileAttachment fileAttachment = this[attachmentID, false, true];
        if (fileAttachment != null)
        {
          if (fileAttachment.Sources == null || fileAttachment.Sources.Length == 0)
          {
            string objectId = string.Empty;
            string str;
            lock (this.nativeDownloadLock)
            {
              if (this.loanDataMgr.UseSkyDriveClassic)
                objectId = ((NativeAttachment) fileAttachment).ObjectId;
              str = this.downloadFile(fileAttachment.ID, objectId);
            }
            stringList.Add(str);
          }
          else
          {
            foreach (string source in fileAttachment.Sources)
              stringList.AddRange((IEnumerable<string>) this.GetOriginalAttachments(source));
          }
        }
        return stringList.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3372, nameof (GetOriginalAttachments), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public void DownloadAttachment(ImageAttachment attachment)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "DownloadAttachment: " + attachment.ID);
      this.DownloadPages(attachment.Pages.ToArray());
    }

    public void DownloadPages(PageImage[] pageList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3393, nameof (DownloadPages), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "DownloadPages: " + (object) pageList.Length);
        lock (this.pageDownloadLock)
        {
          System.Collections.Generic.List<PageImage> pageImageList = new System.Collections.Generic.List<PageImage>();
          foreach (PageImage page in pageList)
          {
            if (!File.Exists(this.downloadPath + page.ImageKey))
              pageImageList.Add(page);
          }
          if (pageImageList.Count == 0)
            return;
          System.Collections.Generic.List<string> stringList1 = new System.Collections.Generic.List<string>();
          foreach (PageImage pageImage in pageImageList)
          {
            if (!stringList1.Contains(pageImage.ZipKey))
              stringList1.Add(pageImage.ZipKey);
          }
          this.pageDownloadTotal = pageImageList.Count;
          this.pageDownloadCount = 0;
          foreach (string str in stringList1)
          {
            System.Collections.Generic.List<string> stringList2 = new System.Collections.Generic.List<string>();
            foreach (PageImage pageImage in pageImageList)
            {
              if (pageImage.ZipKey == str)
                stringList2.Add(pageImage.ImageKey);
            }
            ZipReader zipReader = (ZipReader) null;
            try
            {
              if (this.loanDataMgr.UseSkyDriveLite)
              {
                string filepath = this.downloadFile(str, "");
                if (filepath != null)
                {
                  Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "CreateZipReader: " + filepath);
                  zipReader = new ZipReader(filepath);
                }
                else
                  continue;
              }
              else
              {
                Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "CreateZipReader: " + str);
                zipReader = this.loanDataMgr.LoanObject.CreateZipReader(str);
                if (zipReader == null)
                  continue;
              }
              zipReader.ExtractProgress += new ExtractProgressEventHandler(this.pageZipReader_ExtractProgress);
              Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "ExtractFiles: " + (object) stringList2.Count);
              zipReader.ExtractFiles(stringList2.ToArray(), this.downloadPath);
            }
            finally
            {
              if (zipReader != null)
              {
                zipReader.ExtractProgress -= new ExtractProgressEventHandler(this.pageZipReader_ExtractProgress);
                zipReader.Dispose();
              }
            }
          }
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3475, nameof (DownloadPages), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    public string GetImageFile(PageImage page)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "GetImageFile: " + page.ImageKey);
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3488, nameof (GetImageFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        string imageFile1 = this.downloadPath + page.ImageKey;
        if (page.Rotation == 0)
        {
          if (File.Exists(imageFile1))
            return imageFile1;
        }
        else
        {
          string imageFile2 = page.GetRotatedFile();
          if (imageFile2 != null || !File.Exists(imageFile1))
            return imageFile2;
          using (Image image = Image.FromFile(imageFile1))
          {
            PixelFormat pixelFormat = image.PixelFormat;
            switch (page.Rotation)
            {
              case 90:
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                break;
              case 180:
                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                break;
              case 270:
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                break;
            }
            if (pixelFormat == PixelFormat.Format1bppIndexed || pixelFormat == PixelFormat.Format4bppIndexed)
            {
              imageFile2 = SystemSettings.GetTempFileNameWithExtension("png");
              Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Saving Image: " + imageFile2);
              image.Save(imageFile2, ImageFormat.Png);
            }
            else
            {
              ImageCodecInfo encoder = (ImageCodecInfo) null;
              foreach (ImageCodecInfo imageDecoder in ImageCodecInfo.GetImageDecoders())
              {
                if (imageDecoder.MimeType == "image/jpeg")
                  encoder = imageDecoder;
              }
              EncoderParameters encoderParams = new EncoderParameters(1);
              encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
              imageFile2 = SystemSettings.GetTempFileNameWithExtension("jpg");
              Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Saving Image: " + imageFile2);
              image.Save(imageFile2, encoder, encoderParams);
            }
            page.SetRotatedFile(imageFile2);
          }
          return imageFile2;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), ex.ToString());
        PerformanceMeter.Current.AddCheckpoint("EAT EXCEPTION - " + ex.Message, 3561, nameof (GetImageFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3565, nameof (GetImageFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
      return (string) null;
    }

    public Image GetImage(PageImage page)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "GetImage: " + page.ImageKey);
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 3579, nameof (GetImage), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      try
      {
        string imageFile = this.GetImageFile(page);
        if (imageFile != null)
          return Image.FromFile(imageFile);
      }
      catch (Exception ex)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), ex.ToString());
        PerformanceMeter.Current.AddCheckpoint("EAT EXCEPTION - " + ex.Message, 3592, nameof (GetImage), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
      PerformanceMeter.Current.AddCheckpoint("END", 3594, nameof (GetImage), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      return (Image) null;
    }

    private void pageZipReader_ExtractProgress(object source, ExtractProgressEventArgs e)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "ExtractProgress: " + e.EntryName);
      if (this.PageDownloaded == null)
        return;
      ++this.pageDownloadCount;
      int percentCompleted = this.pageDownloadCount * 100 / this.pageDownloadTotal;
      ExtractProgressEventArgs e1 = new ExtractProgressEventArgs(e.EntryName, percentCompleted);
      this.PageDownloaded((object) this, e1);
      if (!e1.Cancel)
        return;
      e.Cancel = true;
    }

    public void DownloadThumbnails(PageImage[] pageList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3630, nameof (DownloadThumbnails), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        PageThumbnail[] pageThumbnailArray = new PageThumbnail[pageList.Length];
        for (int index = 0; index < pageList.Length; ++index)
          pageThumbnailArray[index] = pageList[index].Thumbnail;
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "DownloadThumbnails: " + (object) pageThumbnailArray.Length);
        lock (this.thumbnailDownloadLock)
        {
          System.Collections.Generic.List<PageThumbnail> pageThumbnailList = new System.Collections.Generic.List<PageThumbnail>();
          foreach (PageThumbnail pageThumbnail in pageThumbnailArray)
          {
            if (!File.Exists(this.downloadPath + pageThumbnail.ImageKey))
              pageThumbnailList.Add(pageThumbnail);
          }
          if (pageThumbnailList.Count == 0)
            return;
          System.Collections.Generic.List<string> stringList1 = new System.Collections.Generic.List<string>();
          foreach (PageThumbnail pageThumbnail in pageThumbnailList)
          {
            if (!stringList1.Contains(pageThumbnail.ZipKey))
              stringList1.Add(pageThumbnail.ZipKey);
          }
          this.thumbnailDownloadTotal = pageThumbnailList.Count;
          this.thumbnailDownloadCount = 0;
          foreach (string str in stringList1)
          {
            System.Collections.Generic.List<string> stringList2 = new System.Collections.Generic.List<string>();
            foreach (PageThumbnail pageThumbnail in pageThumbnailList)
            {
              if (pageThumbnail.ZipKey == str)
                stringList2.Add(pageThumbnail.ImageKey);
            }
            ZipReader zipReader = (ZipReader) null;
            try
            {
              if (this.loanDataMgr.UseSkyDriveLite)
              {
                string filepath = this.downloadFile(str, "");
                if (filepath != null)
                {
                  Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "CreateZipReader: " + filepath);
                  zipReader = new ZipReader(filepath);
                }
                else
                  continue;
              }
              else
              {
                Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "CreateZipReader: " + str);
                zipReader = this.loanDataMgr.LoanObject.CreateZipReader(str);
                if (zipReader == null)
                  continue;
              }
              zipReader.ExtractProgress += new ExtractProgressEventHandler(this.thumbnailZipReader_ExtractProgress);
              Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "ExtractFiles: " + (object) stringList2.Count);
              zipReader.ExtractFiles(stringList2.ToArray(), this.downloadPath);
            }
            finally
            {
              if (zipReader != null)
              {
                zipReader.ExtractProgress -= new ExtractProgressEventHandler(this.thumbnailZipReader_ExtractProgress);
                zipReader.Dispose();
              }
            }
          }
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3717, nameof (DownloadThumbnails), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    private void thumbnailZipReader_ExtractProgress(object source, ExtractProgressEventArgs e)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "ExtractProgress: " + e.EntryName);
      if (this.ThumbnailDownloaded == null)
        return;
      ++this.thumbnailDownloadCount;
      int percentCompleted = this.thumbnailDownloadCount * 100 / this.thumbnailDownloadTotal;
      ExtractProgressEventArgs e1 = new ExtractProgressEventArgs(e.EntryName, percentCompleted);
      this.ThumbnailDownloaded((object) this, e1);
      if (!e1.Cancel)
        return;
      e.Cancel = true;
    }

    public string GetImageFile(PageThumbnail thumbnail)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "GetImageFile: " + thumbnail.ImageKey);
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3753, nameof (GetImageFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        string imageFile1 = this.downloadPath + thumbnail.ImageKey;
        if (thumbnail.Page.Rotation == 0)
        {
          if (File.Exists(imageFile1))
            return imageFile1;
        }
        else
        {
          string imageFile2 = thumbnail.GetRotatedFile();
          if (imageFile2 != null || !File.Exists(imageFile1))
            return imageFile2;
          using (Image image = Image.FromFile(imageFile1))
          {
            switch (thumbnail.Page.Rotation)
            {
              case 90:
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                break;
              case 180:
                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                break;
              case 270:
                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                break;
            }
            imageFile2 = SystemSettings.GetTempFileName(thumbnail.ImageKey);
            image.Save(imageFile2);
            thumbnail.SetRotatedFile(imageFile2);
          }
          return imageFile2;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), ex.ToString());
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3802, nameof (GetImageFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
      return (string) null;
    }

    public Image GetImage(PageThumbnail thumbnail)
    {
      Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "GetImage: " + thumbnail.ImageKey);
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3818, nameof (GetImage), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        string imageFile = this.GetImageFile(thumbnail);
        if (imageFile != null)
          return Image.FromFile(imageFile);
      }
      catch (Exception ex)
      {
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Error, nameof (FileAttachmentCollection), ex.ToString());
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3832, nameof (GetImage), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
      return (Image) null;
    }

    public string DownloadNative(PageImage page)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3846, nameof (DownloadNative), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "DownloadNative: " + page.NativeKey);
        if (string.IsNullOrEmpty(page.NativeKey))
          return (string) null;
        if (Guid.TryParse(page.NativeKey, out Guid _) && this.loanDataMgr.UseSkyDriveLite)
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Checking Cache List: " + page.NativeKey);
          if (this.cacheFileTable.ContainsKey(page.NativeKey) && File.Exists(this.cacheFileTable[page.NativeKey]))
            return this.cacheFileTable[page.NativeKey];
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Calling GetSkyDriveUrlForGet: " + page.NativeKey);
          SkyDriveUrl skyDriveUrlForGet = this.loanDataMgr.LoanObject.GetSkyDriveUrlForGet(page.NativeKey);
          if (skyDriveUrlForGet == null)
          {
            Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "File Not Found: " + page.NativeKey);
            return (string) null;
          }
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Creating SkyDriveStreamingClient: " + page.NativeKey);
          SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(this.loanDataMgr);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Start DownloadFile Task: " + page.NativeKey);
          Task<string> task = driveStreamingClient.DownloadFile(skyDriveUrlForGet, page.NativeKey, true);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Wait DownloadFile Task: " + page.NativeKey);
          Task.WaitAll((Task) task);
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Caching Downloaded File: " + task.Result);
          this.cacheFileTable[page.NativeKey] = task.Result;
          return task.Result;
        }
        lock (this.nativeDownloadLock)
          return this.downloadFile(page.NativeKey, "");
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3901, nameof (DownloadNative), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
      }
    }

    private string downloadFile(string fileKey, string objectId = "�")
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("FilAtmtColDnld", "DOCS Download eFolder Document", true, false, true, 3911, nameof (downloadFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3913, nameof (downloadFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
        string str1 = this.downloadPath + fileKey;
        if (fileKey.ToLower().EndsWith(".findingshtml"))
          str1 += ".htm";
        else if (fileKey.ToLower().EndsWith(".creditprintfile"))
          str1 += ".txt";
        else if (!fileKey.Contains("."))
        {
          string[] strArray = new string[12]
          {
            "pdf",
            "doc",
            "docx",
            "txt",
            "tif",
            "jpg",
            "jpeg",
            "jpe",
            "emf",
            "htm",
            "html",
            "xps"
          };
          foreach (string str2 in strArray)
          {
            if (fileKey.ToLower().EndsWith(str2))
            {
              str1 = str1 + "." + str2;
              break;
            }
          }
        }
        if (File.Exists(str1))
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT file exists already", 3941, nameof (downloadFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
          return str1;
        }
        if (this.loanDataMgr.UseSkyDriveLite || this.loanDataMgr.UseSkyDriveClassic)
        {
          SkyDriveStreamingClient skyDriveClient = new SkyDriveStreamingClient(this.loanDataMgr);
          skyDriveClient.DownloadProgress += new DownloadProgressEventHandler(this.downloadFile_downloadProgress);
          try
          {
            if (this.loanDataMgr.UseSkyDriveClassic && !string.IsNullOrWhiteSpace(objectId))
            {
              Task<string> OrignalPath = (Task<string>) null;
              SkyDriveUrl preSignedOriginalFile = skyDriveClient.GetPresignedUrlForPartnerFilesDownload(objectId);
              Task.WaitAll((Task) Task.Run<string>((Func<Task<string>>) (() => OrignalPath = skyDriveClient.DownloadFile(preSignedOriginalFile, fileKey))));
              string result = OrignalPath.Result;
              if (!File.Exists(str1))
                File.Move(result, str1);
              return str1;
            }
            Task<BinaryObject> supportingData = skyDriveClient.GetSupportingData(fileKey);
            Task.WaitAll((Task) supportingData);
            if (supportingData.Result == null)
            {
              PerformanceMeter.Current.AddCheckpoint("EXIT null data", 3978, nameof (downloadFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
              return (string) null;
            }
            using (supportingData.Result)
            {
              performanceMeter.AddVariable("Name", (object) fileKey);
              performanceMeter.AddVariable("Size", (object) supportingData.Result.Length);
              supportingData.Result.Write(str1);
            }
          }
          finally
          {
            skyDriveClient.DownloadProgress -= new DownloadProgressEventHandler(this.downloadFile_downloadProgress);
          }
        }
        else
        {
          Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "GetSupportingDataOnCIFs: " + fileKey);
          using (BinaryObject supportingDataOnCiFs = this.loanDataMgr.LoanObject.GetSupportingDataOnCIFs(fileKey))
          {
            if (supportingDataOnCiFs == null)
            {
              PerformanceMeter.Current.AddCheckpoint("EXIT null data", 4009, nameof (downloadFile), "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\eFolder\\FileAttachmentCollection.cs");
              return (string) null;
            }
            performanceMeter.AddVariable("Name", (object) fileKey);
            performanceMeter.AddVariable("Size", (object) supportingDataOnCiFs.Length);
            if (supportingDataOnCiFs.RequiresDownload)
            {
              supportingDataOnCiFs.DownloadProgress += new DownloadProgressEventHandler(this.downloadFile_downloadProgress);
              try
              {
                Tracing.Log(FileAttachmentCollection.sw, TraceLevel.Verbose, nameof (FileAttachmentCollection), "Download: " + fileKey);
                supportingDataOnCiFs.Download();
              }
              finally
              {
                supportingDataOnCiFs.DownloadProgress -= new DownloadProgressEventHandler(this.downloadFile_downloadProgress);
              }
            }
            supportingDataOnCiFs.Write(str1);
          }
        }
        return str1;
      }
    }

    private void downloadFile_downloadProgress(object source, DownloadProgressEventArgs e)
    {
      if (this.FileAttachmentDownloadProgress == null)
        return;
      TransferProgressEventArgs e1 = new TransferProgressEventArgs(e);
      this.FileAttachmentDownloadProgress((object) this, e1);
      if (!e1.Cancel)
        return;
      e.Cancel = true;
    }
  }
}
