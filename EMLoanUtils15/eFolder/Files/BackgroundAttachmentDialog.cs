// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.BackgroundAttachmentDialog
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class BackgroundAttachmentDialog : Form
  {
    private const string className = "BackgroundAttachmentDialog�";
    private static readonly string sw = Tracing.SwEFolder;
    private static BackgroundAttachmentDialog _instance = (BackgroundAttachmentDialog) null;
    private ISession session;
    private ISessionStartupInfo startupInfo;
    private List<string> registeredLoanList = new List<string>();
    private Queue<ConversionQueueItem> conversionQueue = new Queue<ConversionQueueItem>();
    private Queue<UploadQueueItem> uploadQueue = new Queue<UploadQueueItem>();
    private ConversionQueueItem currentConversionQueueItem;
    private UploadQueueItem currentUploadQueueItem;
    private IContainer components;
    private GroupContainer gcFiles;
    private GridView gvFiles;
    private Panel pnlTop;
    private Label lblFiles;
    private Panel pnlBottom;
    private Button btnClose;
    private Panel pnlFiles;
    private BackgroundWorker uploadWorker;
    private BackgroundWorker conversionWorker;

    private BackgroundAttachmentDialog(ISession session, ISessionStartupInfo startupInfo)
    {
      this.InitializeComponent();
      this.session = session;
      this.startupInfo = startupInfo;
      this.registeredLoanList = new List<string>();
      this.uploadQueue = new Queue<UploadQueueItem>();
      this.conversionQueue = new Queue<ConversionQueueItem>();
      this.restoreQueues();
    }

    private void restoreQueues()
    {
      Process currentProcess = Process.GetCurrentProcess();
      Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
      int num = 0;
      foreach (Process process in processesByName)
      {
        if (process.SessionId == currentProcess.SessionId)
          ++num;
      }
      Tracing.Log(BackgroundAttachmentDialog.sw, nameof (BackgroundAttachmentDialog), TraceLevel.Info, "Cannot restore Background attaching as Encompass session already exists");
      if (num > 1)
        return;
      ConversionQueueItem[] conversionQueueItems = this.getConversionQueueItems();
      UploadQueueItem[] uploadQueueItems = this.getUploadQueueItems();
      if (conversionQueueItems.Length != 0 || uploadQueueItems.Length != 0)
      {
        StringBuilder stringBuilder = new StringBuilder("You have files that need to be attached. \t\t\r\r");
        foreach (ConversionQueueItem conversionQueueItem in conversionQueueItems)
          stringBuilder.AppendLine(conversionQueueItem.Attachment.Title);
        foreach (UploadQueueItem uploadQueueItem in uploadQueueItems)
          stringBuilder.AppendLine(uploadQueueItem.Attachment.Title);
        stringBuilder.AppendLine("\rDo you want to continue to attach them?");
        if (Utils.Dialog((IWin32Window) Form.ActiveForm, stringBuilder.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        {
          foreach (ConversionQueueItem queueItem in conversionQueueItems)
            this.addQueueItem(queueItem, false);
          foreach (UploadQueueItem queueItem in uploadQueueItems)
            this.addQueueItem(queueItem, false);
          return;
        }
      }
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) Directory.GetFiles(BackgroundAttachmentDialog.ConversionPath));
      stringList.AddRange((IEnumerable<string>) Directory.GetFiles(BackgroundAttachmentDialog.UploadPath));
      foreach (string path in stringList)
      {
        try
        {
          File.Delete(path);
        }
        catch (Exception ex)
        {
          Tracing.Log(BackgroundAttachmentDialog.sw, nameof (BackgroundAttachmentDialog), TraceLevel.Error, ex.ToString());
        }
      }
    }

    private ConversionQueueItem[] getConversionQueueItems()
    {
      string[] files = Directory.GetFiles(BackgroundAttachmentDialog.ConversionPath, "Manifest-*.xml");
      List<ConversionQueueItem> conversionQueueItemList = new List<ConversionQueueItem>();
      foreach (string manifestFile in files)
      {
        try
        {
          ConversionQueueItem conversionQueueItem = new ConversionQueueItem(manifestFile);
          conversionQueueItemList.Add(conversionQueueItem);
        }
        catch (Exception ex)
        {
          Tracing.Log(BackgroundAttachmentDialog.sw, nameof (BackgroundAttachmentDialog), TraceLevel.Error, ex.ToString());
        }
      }
      return conversionQueueItemList.ToArray();
    }

    private UploadQueueItem[] getUploadQueueItems()
    {
      string[] files = Directory.GetFiles(BackgroundAttachmentDialog.UploadPath, "Manifest-*.xml");
      List<UploadQueueItem> uploadQueueItemList = new List<UploadQueueItem>();
      foreach (string manifestFile in files)
      {
        try
        {
          UploadQueueItem uploadQueueItem = new UploadQueueItem(manifestFile);
          uploadQueueItemList.Add(uploadQueueItem);
        }
        catch (Exception ex)
        {
          Tracing.Log(BackgroundAttachmentDialog.sw, nameof (BackgroundAttachmentDialog), TraceLevel.Error, ex.ToString());
        }
      }
      return uploadQueueItemList.ToArray();
    }

    public static BackgroundAttachmentDialog Instance => BackgroundAttachmentDialog._instance;

    public static void CreateInstance(
      ISession session,
      ISessionStartupInfo startupInfo,
      bool recreate = false)
    {
      if (!(BackgroundAttachmentDialog._instance == null | recreate))
        return;
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Creating BackgroundAttachmentDialog");
      BackgroundAttachmentDialog._instance = new BackgroundAttachmentDialog(session, startupInfo);
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking IsHandleCreated");
      if (BackgroundAttachmentDialog._instance.IsHandleCreated)
        return;
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Calling CreateHandle");
      BackgroundAttachmentDialog._instance.CreateHandle();
    }

    public static void CloseInstance()
    {
      if (BackgroundAttachmentDialog._instance == null)
        return;
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Locking Conversion Queue For Clear");
      lock (BackgroundAttachmentDialog._instance.conversionQueue)
      {
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Clearing Conversion Queue");
        BackgroundAttachmentDialog._instance.conversionQueue.Clear();
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking Conversion Worker");
        if (BackgroundAttachmentDialog._instance.conversionWorker.IsBusy)
        {
          Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Cancelling Conversion Worker");
          BackgroundAttachmentDialog._instance.conversionWorker.CancelAsync();
          Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Waiting For Conversion Worker To Complete");
          while (BackgroundAttachmentDialog._instance.conversionWorker.IsBusy)
            Application.DoEvents();
        }
      }
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Locking Upload Queue For Clear");
      lock (BackgroundAttachmentDialog._instance.uploadQueue)
      {
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Clearing Upload Queue");
        BackgroundAttachmentDialog._instance.uploadQueue.Clear();
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking Upload Worker");
        if (!BackgroundAttachmentDialog._instance.uploadWorker.IsBusy)
          return;
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Cancelling Upload Worker");
        BackgroundAttachmentDialog._instance.uploadWorker.CancelAsync();
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Waiting For Upload Worker To Complete");
        while (BackgroundAttachmentDialog._instance.uploadWorker.IsBusy)
          Application.DoEvents();
      }
    }

    public static void ShowInstance()
    {
      if (BackgroundAttachmentDialog._instance == null)
        return;
      if (BackgroundAttachmentDialog._instance.WindowState == FormWindowState.Minimized)
      {
        BackgroundAttachmentDialog._instance.WindowState = FormWindowState.Normal;
        BackgroundAttachmentDialog._instance.Activate();
      }
      else
      {
        BackgroundAttachmentDialog._instance.Show();
        BackgroundAttachmentDialog._instance.BringToFront();
      }
    }

    public static string ConversionPath
    {
      get => EnConfigurationSettings.GlobalSettings.AppBackgroundConversionDirectory + "\\";
    }

    public static string UploadPath
    {
      get => EnConfigurationSettings.GlobalSettings.AppBackgroundUploadDirectory + "\\";
    }

    public static bool IsActive
    {
      get
      {
        if (BackgroundAttachmentDialog._instance == null)
          return false;
        lock (BackgroundAttachmentDialog._instance.conversionQueue)
        {
          if (BackgroundAttachmentDialog._instance.conversionQueue.Count > 0)
            return true;
        }
        lock (BackgroundAttachmentDialog._instance.uploadQueue)
        {
          if (BackgroundAttachmentDialog._instance.uploadQueue.Count > 0)
            return true;
        }
        return BackgroundAttachmentDialog._instance.startupInfo.UseBackgroundConversion;
      }
    }

    public static void RegisterLoan(LoanDataMgr loanDataMgr)
    {
      if (BackgroundAttachmentDialog._instance == null)
        return;
      lock (BackgroundAttachmentDialog._instance.registeredLoanList)
        BackgroundAttachmentDialog._instance.registeredLoanList.Add(loanDataMgr.LoanData.GUID);
      BackgroundAttachmentDialog._instance.processConversionQueue();
      BackgroundAttachmentDialog._instance.processUploadQueue();
    }

    public static void Insert(
      BackgroundAttachment attachment,
      BinaryObject data,
      string loanGuid,
      string loanNumber,
      string borrowerName)
    {
      if (BackgroundAttachmentDialog._instance == null)
        return;
      if (attachment.DocumentConversionOn)
      {
        string path = Path.Combine(BackgroundAttachmentDialog.ConversionPath, attachment.ID + attachment.Extension);
        data.Write(path);
        ConversionQueueItem queueItem = new ConversionQueueItem(attachment, loanGuid, loanNumber, borrowerName, data.Length);
        string manifestFile = Path.Combine(BackgroundAttachmentDialog.ConversionPath, "Manifest-" + queueItem.Attachment.ID + ".xml");
        queueItem.CreateManifest(manifestFile);
        BackgroundAttachmentDialog._instance.addQueueItem(queueItem, true);
      }
      else
      {
        string path = Path.Combine(BackgroundAttachmentDialog.UploadPath, attachment.ID);
        data.Write(path);
        UploadQueueItem queueItem = new UploadQueueItem(attachment, loanGuid, loanNumber, borrowerName, data.Length);
        string manifestFile = Path.Combine(BackgroundAttachmentDialog.UploadPath, "Manifest-" + queueItem.Attachment.ID + ".xml");
        queueItem.CreateManifest(manifestFile);
        BackgroundAttachmentDialog._instance.addQueueItem(queueItem, true);
      }
    }

    private void addQueueItem(ConversionQueueItem queueItem, bool processItem)
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking InvokeRequired For AddConversionQueueItem");
      if (this.InvokeRequired)
      {
        BackgroundAttachmentDialog.AddConversionQueueItem method = new BackgroundAttachmentDialog.AddConversionQueueItem(this.addQueueItem);
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Calling BeginInvoke For AddConversionQueueItem");
        this.BeginInvoke((Delegate) method, (object) queueItem, (object) processItem);
      }
      else
      {
        if (this.gvFiles.Items.GetItemByTag((object) queueItem.Attachment.ID) == null)
        {
          string str = "0 KB";
          if (queueItem.OriginalFileSize > 0L)
            str = (queueItem.OriginalFileSize / 1024L).ToString() + " KB";
          this.gvFiles.Items.Add(new GVItem()
          {
            SubItems = {
              (object) queueItem.Attachment.Title,
              (object) str,
              (object) queueItem.BorrowerName,
              (object) queueItem.LoanNumber
            },
            Tag = (object) queueItem.Attachment.ID
          });
        }
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Lock Conversion Queue For Add: " + queueItem.Attachment.ID);
        lock (this.conversionQueue)
        {
          Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Add To Conversion Queue: " + queueItem.Attachment.ID);
          this.conversionQueue.Enqueue(queueItem);
        }
        if (!processItem)
          return;
        this.processConversionQueue();
      }
    }

    private void addQueueItem(UploadQueueItem queueItem, bool processItem)
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking InvokeRequired For AddUploadQueueItem");
      if (this.InvokeRequired)
      {
        BackgroundAttachmentDialog.AddUploadQueueItem method = new BackgroundAttachmentDialog.AddUploadQueueItem(this.addQueueItem);
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Calling BeginInvoke For AddUploadQueueItem");
        this.BeginInvoke((Delegate) method, (object) queueItem, (object) processItem);
      }
      else
      {
        if (this.gvFiles.Items.GetItemByTag((object) queueItem.Attachment.ID) == null)
        {
          string str = "0 KB";
          if (queueItem.OriginalFileSize > 0L)
            str = (queueItem.OriginalFileSize / 1024L).ToString() + " KB";
          this.gvFiles.Items.Add(new GVItem()
          {
            SubItems = {
              (object) queueItem.Attachment.Title,
              (object) str,
              (object) queueItem.BorrowerName,
              (object) queueItem.LoanNumber,
              (object) "N/A"
            },
            Tag = (object) queueItem.Attachment.ID
          });
        }
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Lock Upload Queue For Add: " + queueItem.Attachment.ID);
        lock (this.uploadQueue)
        {
          Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Add To Upload Queue: " + queueItem.Attachment.ID);
          this.uploadQueue.Enqueue(queueItem);
        }
        if (!processItem)
          return;
        this.processUploadQueue();
      }
    }

    public static bool IsProcessing(string[] loanGuidList)
    {
      if (BackgroundAttachmentDialog._instance == null)
        return false;
      lock (BackgroundAttachmentDialog._instance.conversionQueue)
      {
        if (BackgroundAttachmentDialog._instance.conversionWorker.IsBusy)
        {
          foreach (string loanGuid in loanGuidList)
          {
            if (string.Compare(BackgroundAttachmentDialog._instance.currentConversionQueueItem.LoanGUID, loanGuid, true) == 0)
              return true;
          }
        }
        foreach (ConversionQueueItem conversion in BackgroundAttachmentDialog._instance.conversionQueue)
        {
          foreach (string loanGuid in loanGuidList)
          {
            if (string.Compare(conversion.LoanGUID, loanGuid, true) == 0)
              return true;
          }
        }
      }
      lock (BackgroundAttachmentDialog._instance.uploadQueue)
      {
        if (BackgroundAttachmentDialog._instance.uploadWorker.IsBusy)
        {
          foreach (string loanGuid in loanGuidList)
          {
            if (string.Compare(BackgroundAttachmentDialog._instance.currentUploadQueueItem.LoanGUID, loanGuid, true) == 0)
              return true;
          }
        }
        foreach (UploadQueueItem upload in BackgroundAttachmentDialog._instance.uploadQueue)
        {
          foreach (string loanGuid in loanGuidList)
          {
            if (string.Compare(upload.LoanGUID, loanGuid, true) == 0)
              return true;
          }
        }
      }
      return false;
    }

    public static bool IsProcessing(string attachmentID)
    {
      if (BackgroundAttachmentDialog._instance == null)
        return false;
      lock (BackgroundAttachmentDialog._instance.conversionQueue)
      {
        if (BackgroundAttachmentDialog._instance.conversionWorker.IsBusy && BackgroundAttachmentDialog._instance.currentConversionQueueItem.Attachment.ID == attachmentID)
          return true;
        foreach (ConversionQueueItem conversion in BackgroundAttachmentDialog._instance.conversionQueue)
        {
          if (conversion.Attachment.ID == attachmentID)
            return true;
        }
      }
      lock (BackgroundAttachmentDialog._instance.uploadQueue)
      {
        if (BackgroundAttachmentDialog._instance.uploadWorker.IsBusy && BackgroundAttachmentDialog._instance.currentUploadQueueItem.Attachment.ID == attachmentID)
          return true;
        foreach (UploadQueueItem upload in BackgroundAttachmentDialog._instance.uploadQueue)
        {
          if (upload.Attachment.ID == attachmentID)
            return true;
        }
      }
      return false;
    }

    public static bool IsRegistered(string loanGuid)
    {
      if (BackgroundAttachmentDialog._instance == null)
        return false;
      lock (BackgroundAttachmentDialog._instance.registeredLoanList)
        return BackgroundAttachmentDialog._instance.registeredLoanList.Contains(loanGuid);
    }

    public static void Remove(string attachmentID)
    {
      if (BackgroundAttachmentDialog._instance == null)
        return;
      BackgroundAttachmentDialog._instance.removeQueueItem(attachmentID);
    }

    private void removeQueueItem(string attachmentID)
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking InvokeRequired For RemoveQueueItem");
      if (this.InvokeRequired)
      {
        BackgroundAttachmentDialog.RemoveQueueItem method = new BackgroundAttachmentDialog.RemoveQueueItem(this.removeQueueItem);
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Calling BeginInvoke For RemoveQueueItem");
        this.BeginInvoke((Delegate) method, (object) attachmentID);
      }
      else
      {
        lock (this.conversionQueue)
        {
          if (this.conversionWorker.IsBusy && this.currentConversionQueueItem.Attachment.ID == attachmentID)
          {
            Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Cancelling Conversion Worker");
            this.conversionWorker.CancelAsync();
            this.currentConversionQueueItem.RemoveManifest();
          }
          else
          {
            ConversionQueueItem[] array = this.conversionQueue.ToArray();
            ConversionQueueItem conversionQueueItem1 = (ConversionQueueItem) null;
            foreach (ConversionQueueItem conversionQueueItem2 in array)
            {
              if (conversionQueueItem2.Attachment.ID == attachmentID)
                conversionQueueItem1 = conversionQueueItem2;
            }
            if (conversionQueueItem1 != null)
            {
              this.conversionQueue.Clear();
              foreach (ConversionQueueItem conversionQueueItem3 in array)
              {
                if (conversionQueueItem3 != conversionQueueItem1)
                  this.conversionQueue.Enqueue(conversionQueueItem3);
              }
              conversionQueueItem1.RemoveManifest();
              GVItem itemByTag = this.gvFiles.Items.GetItemByTag((object) conversionQueueItem1.Attachment.ID);
              if (itemByTag != null)
                itemByTag.SubItems["CONVERSIONPROGRESSSTATUS"].Value = (object) "Cancelled";
            }
          }
        }
        lock (this.uploadQueue)
        {
          if (this.uploadWorker.IsBusy && this.currentUploadQueueItem.Attachment.ID == attachmentID)
          {
            Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Cancelling Upload Worker");
            this.uploadWorker.CancelAsync();
            this.currentUploadQueueItem.RemoveManifest();
          }
          else
          {
            UploadQueueItem[] array = this.uploadQueue.ToArray();
            UploadQueueItem uploadQueueItem1 = (UploadQueueItem) null;
            foreach (UploadQueueItem uploadQueueItem2 in array)
            {
              if (uploadQueueItem2.Attachment.ID == attachmentID)
                uploadQueueItem1 = uploadQueueItem2;
            }
            if (uploadQueueItem1 == null)
              return;
            this.uploadQueue.Clear();
            foreach (UploadQueueItem uploadQueueItem3 in array)
            {
              if (uploadQueueItem3 != uploadQueueItem1)
                this.uploadQueue.Enqueue(uploadQueueItem3);
            }
            uploadQueueItem1.RemoveManifest();
            GVItem itemByTag = this.gvFiles.Items.GetItemByTag((object) uploadQueueItem1.Attachment.ID);
            if (itemByTag == null)
              return;
            itemByTag.SubItems["UPLOADPROGRESSSTATUS"].Value = (object) "Cancelled";
          }
        }
      }
    }

    private void processUploadQueue()
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking Upload Worker");
      if (this.uploadWorker.IsBusy)
        return;
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Lock Upload Queue For Processing");
      lock (this.uploadQueue)
      {
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking Queue Count: " + (object) this.uploadQueue.Count);
        if (this.uploadQueue.Count == 0)
          return;
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Getting Next Upload Queue Item");
        this.currentUploadQueueItem = this.uploadQueue.Dequeue();
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Starting Upload Thread: " + this.currentUploadQueueItem.Attachment.ID);
        this.uploadWorker.RunWorkerAsync((object) this.currentUploadQueueItem);
      }
    }

    private void uploadWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Upload Worker Started");
      UploadQueueItem userState = (UploadQueueItem) e.Argument;
      userState.FileAttachmentUploadProgress += new TransferProgressEventHandler(this.fileAttachmentUploadProgress);
      try
      {
        userState.Upload(this.session, this.startupInfo);
        userState.RemoveManifest();
        this.OnBackgroundAttachmentUploaded(userState.Attachment);
      }
      catch (CanceledOperationException ex)
      {
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Worker Cancelled");
        this.uploadWorker.ReportProgress(int.MinValue, (object) userState);
      }
      catch (Exception ex)
      {
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), ex.ToString());
        this.uploadWorker.ReportProgress(int.MaxValue, (object) userState);
      }
      finally
      {
        userState.FileAttachmentUploadProgress -= new TransferProgressEventHandler(this.fileAttachmentUploadProgress);
      }
    }

    private void fileAttachmentUploadProgress(object source, TransferProgressEventArgs e)
    {
      if (this.uploadWorker.CancellationPending)
        e.Cancel = true;
      this.uploadWorker.ReportProgress(e.PercentCompleted, source);
    }

    private void uploadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      GVItem itemByTag = this.gvFiles.Items.GetItemByTag((object) ((UploadQueueItem) e.UserState).Attachment.ID);
      if (itemByTag == null)
        return;
      string str = e.ProgressPercentage != int.MinValue ? (e.ProgressPercentage != int.MaxValue ? e.ProgressPercentage.ToString() + "%" : "Failed") : "Cancelled";
      itemByTag.SubItems["UPLOADPROGRESSSTATUS"].Value = (object) str;
    }

    private void uploadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Upload Worker Complete");
      this.processUploadQueue();
    }

    private void processConversionQueue()
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking Conversion Worker");
      if (this.conversionWorker.IsBusy)
        return;
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Lock Conversion Queue For Processing");
      lock (this.conversionQueue)
      {
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Checking Queue Count: " + (object) this.conversionQueue.Count);
        if (this.conversionQueue.Count == 0)
          return;
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Getting Next Conversion Queue Item");
        this.currentConversionQueueItem = this.conversionQueue.Dequeue();
        Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Starting Conversion Thread: " + this.currentConversionQueueItem.Attachment.ID);
        this.conversionWorker.RunWorkerAsync((object) this.currentConversionQueueItem);
      }
    }

    private void conversionWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Conversion Worker Started");
      ConversionQueueItem conversionQueueItem = (ConversionQueueItem) e.Argument;
      conversionQueueItem.FileConversionProgress += new TransferProgressEventHandler(this.fileAttachmentConversionProgress);
      try
      {
        long maxZipFileSize = 2000000;
        IDictionary efolderSettings = this.startupInfo.EFolderSettings;
        if (efolderSettings != null && efolderSettings.Contains((object) "eFolder.MaxZipSize"))
          maxZipFileSize = Convert.ToInt64(efolderSettings[(object) "eFolder.MaxZipSize"]);
        PageImage[] pageList = conversionQueueItem.Convert(maxZipFileSize);
        UploadQueueItem queueItem = new UploadQueueItem(conversionQueueItem, pageList);
        string manifestFile = Path.Combine(BackgroundAttachmentDialog.UploadPath, "Manifest-" + queueItem.Attachment.ID + ".xml");
        queueItem.CreateManifest(manifestFile);
        this.addQueueItem(queueItem, true);
        conversionQueueItem.RemoveManifest();
      }
      catch (Exception ex)
      {
        if (ex.InnerException is CanceledOperationException)
        {
          Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Worker Cancelled");
          this.conversionWorker.ReportProgress(int.MinValue, (object) conversionQueueItem);
        }
        else
        {
          Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Error, nameof (BackgroundAttachmentDialog), ex.ToString());
          this.conversionWorker.ReportProgress(int.MaxValue, (object) conversionQueueItem);
        }
      }
      finally
      {
        conversionQueueItem.FileConversionProgress -= new TransferProgressEventHandler(this.fileAttachmentConversionProgress);
      }
    }

    private void fileAttachmentConversionProgress(object source, TransferProgressEventArgs e)
    {
      if (this.conversionWorker.CancellationPending)
        e.Cancel = true;
      this.conversionWorker.ReportProgress(e.PercentCompleted, source);
    }

    private void conversionWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      GVItem itemByTag = this.gvFiles.Items.GetItemByTag((object) ((ConversionQueueItem) e.UserState).Attachment.ID);
      if (itemByTag == null)
        return;
      string str = e.ProgressPercentage != int.MinValue ? (e.ProgressPercentage != int.MaxValue ? (e.ProgressPercentage >= 0 ? e.ProgressPercentage.ToString() + "%" : "Page " + Math.Abs(e.ProgressPercentage).ToString()) : "Failed") : "Cancelled";
      itemByTag.SubItems["CONVERSIONPROGRESSSTATUS"].Value = (object) str;
    }

    private void conversionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Tracing.Log(BackgroundAttachmentDialog.sw, TraceLevel.Verbose, nameof (BackgroundAttachmentDialog), "Conversion Worker Complete");
      this.processConversionQueue();
    }

    public event FileAttachmentEventHandler BackgroundAttachmentUploaded;

    public void OnBackgroundAttachmentUploaded(FileAttachment attachment)
    {
      if (this.BackgroundAttachmentUploaded == null)
        return;
      this.BackgroundAttachmentUploaded((object) this, new FileAttachmentEventArgs(attachment));
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Hide();

    private void BackgroundAttachmentDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing)
        return;
      e.Cancel = true;
      this.Hide();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BackgroundAttachmentDialog));
      this.gcFiles = new GroupContainer();
      this.gvFiles = new GridView();
      this.pnlTop = new Panel();
      this.lblFiles = new Label();
      this.pnlBottom = new Panel();
      this.btnClose = new Button();
      this.pnlFiles = new Panel();
      this.uploadWorker = new BackgroundWorker();
      this.conversionWorker = new BackgroundWorker();
      this.gcFiles.SuspendLayout();
      this.pnlTop.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      this.pnlFiles.SuspendLayout();
      this.SuspendLayout();
      this.gcFiles.Controls.Add((Control) this.gvFiles);
      this.gcFiles.Dock = DockStyle.Fill;
      this.gcFiles.HeaderForeColor = SystemColors.ControlText;
      this.gcFiles.Location = new Point(0, 0);
      this.gcFiles.Name = "gcFiles";
      this.gcFiles.Size = new Size(930, 258);
      this.gcFiles.TabIndex = 0;
      this.gcFiles.Text = "Files";
      this.gvFiles.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "NAMEWITHICON";
      gvColumn1.Text = "File Name";
      gvColumn1.Width = 225;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "FILESIZE";
      gvColumn2.Text = "File Size";
      gvColumn2.Width = 105;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "BORROWER";
      gvColumn3.Text = "Borrower Name";
      gvColumn3.Width = 145;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "LOANNUMBER";
      gvColumn4.Text = "Loan #";
      gvColumn4.Width = 160;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "CONVERSIONPROGRESSSTATUS";
      gvColumn5.Text = "Conversion Progress";
      gvColumn5.Width = 160;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "UPLOADPROGRESSSTATUS";
      gvColumn6.Text = "Upload Progress";
      gvColumn6.Width = 100;
      this.gvFiles.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvFiles.Dock = DockStyle.Fill;
      this.gvFiles.Location = new Point(1, 26);
      this.gvFiles.Name = "gvFiles";
      this.gvFiles.Size = new Size(928, 231);
      this.gvFiles.TabIndex = 0;
      this.pnlTop.Controls.Add((Control) this.lblFiles);
      this.pnlTop.Dock = DockStyle.Top;
      this.pnlTop.Location = new Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Size = new Size(930, 40);
      this.pnlTop.TabIndex = 2;
      this.lblFiles.AutoSize = true;
      this.lblFiles.Location = new Point(10, 12);
      this.lblFiles.Name = "lblFiles";
      this.lblFiles.Size = new Size(239, 14);
      this.lblFiles.TabIndex = 2;
      this.lblFiles.Text = "Files that are currently being attached in eFolder";
      this.pnlBottom.Controls.Add((Control) this.btnClose);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(0, 298);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(930, 40);
      this.pnlBottom.TabIndex = 3;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(847, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.pnlFiles.Controls.Add((Control) this.gcFiles);
      this.pnlFiles.Dock = DockStyle.Fill;
      this.pnlFiles.Location = new Point(0, 40);
      this.pnlFiles.Name = "pnlFiles";
      this.pnlFiles.Size = new Size(930, 258);
      this.pnlFiles.TabIndex = 4;
      this.uploadWorker.WorkerReportsProgress = true;
      this.uploadWorker.WorkerSupportsCancellation = true;
      this.uploadWorker.DoWork += new DoWorkEventHandler(this.uploadWorker_DoWork);
      this.uploadWorker.ProgressChanged += new ProgressChangedEventHandler(this.uploadWorker_ProgressChanged);
      this.uploadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.uploadWorker_RunWorkerCompleted);
      this.conversionWorker.WorkerReportsProgress = true;
      this.conversionWorker.WorkerSupportsCancellation = true;
      this.conversionWorker.DoWork += new DoWorkEventHandler(this.conversionWorker_DoWork);
      this.conversionWorker.ProgressChanged += new ProgressChangedEventHandler(this.conversionWorker_ProgressChanged);
      this.conversionWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.conversionWorker_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(930, 338);
      this.Controls.Add((Control) this.pnlFiles);
      this.Controls.Add((Control) this.pnlBottom);
      this.Controls.Add((Control) this.pnlTop);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.Name = nameof (BackgroundAttachmentDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Background Attaching";
      this.FormClosing += new FormClosingEventHandler(this.BackgroundAttachmentDialog_FormClosing);
      this.gcFiles.ResumeLayout(false);
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.pnlBottom.ResumeLayout(false);
      this.pnlFiles.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private delegate void AddConversionQueueItem(ConversionQueueItem queueItem, bool processItem);

    private delegate void AddUploadQueueItem(UploadQueueItem queueItem, bool processItem);

    private delegate void RemoveQueueItem(string attachmentID);
  }
}
