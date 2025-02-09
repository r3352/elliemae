// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.SkyDrive.SkyDriveLiteMigrationDialog
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.SkyDrive
{
  public class SkyDriveLiteMigrationDialog : Form
  {
    private const string className = "SkyDriveLiteMigrationDialog�";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr _loanDataMgr;
    private Queue<string> _fileKeyQueue;
    private CancellationTokenSource _cancelToken;
    private IContainer components;
    private Label lblUpload;
    private ProgressBar pbUpload;
    private Button btnCancel;
    private Label lblDownload;
    private ProgressBar pbDownload;

    public SkyDriveLiteMigrationDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this._loanDataMgr = loanDataMgr;
    }

    public bool MigrateLoan(IWin32Window owner)
    {
      Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Calling GetLoanPropertySettings");
      LoanProperty loanProperty = ((IEnumerable<LoanProperty>) this._loanDataMgr.LoanObject.GetLoanPropertySettings()).FirstOrDefault<LoanProperty>((Func<LoanProperty, bool>) (x => x.Category.Equals("LoanStorage", StringComparison.OrdinalIgnoreCase) && x.Attribute.Equals("SupportingData", StringComparison.OrdinalIgnoreCase)));
      if (loanProperty != null && !loanProperty.Value.Equals("CIFS", StringComparison.OrdinalIgnoreCase))
        return true;
      Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Calling GetSupportingDataKeysOnCIFs");
      string[] supportingDataKeysOnCiFs = this._loanDataMgr.LoanObject.GetSupportingDataKeysOnCIFs();
      this._fileKeyQueue = new Queue<string>();
      foreach (string file in supportingDataKeysOnCiFs)
      {
        if (!Utils.IsCIFsOnlyFile(file))
          this._fileKeyQueue.Enqueue(file);
      }
      Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Checking File Count: " + (object) this._fileKeyQueue.Count);
      if (this._fileKeyQueue.Count > 0 && this.ShowDialog(owner) != DialogResult.OK)
        return false;
      Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Calling SetLoanPropertySetting");
      this._loanDataMgr.LoanObject.SetLoanPropertySetting(new LoanProperty("LoanStorage", "SupportingData", "SkyDriveLite"));
      Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Setting UseSkyDriveLite");
      this._loanDataMgr.UseSkyDriveLite = true;
      return true;
    }

    private void SkyDriveLiteMigrationDialog_Load(object sender, EventArgs e)
    {
      this.pbUpload.Maximum = this._fileKeyQueue.Count;
      this.pbDownload.Maximum = this._fileKeyQueue.Count;
      Progress<string> downloadProgress = new Progress<string>((Action<string>) (x => ++this.pbDownload.Value));
      Progress<string> uploadProgress = new Progress<string>((Action<string>) (x => ++this.pbUpload.Value));
      this._cancelToken = new CancellationTokenSource();
      Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Starting File Migration");
      Task.Run((Func<Task>) (async () =>
      {
        List<Task> taskList = new List<Task>();
        while (this._fileKeyQueue.Count > 0 || taskList.Count > 0)
        {
          while (this._fileKeyQueue.Count > 0 && taskList.Count < 3)
          {
            string fileKey = this._fileKeyQueue.Dequeue();
            if (!this._cancelToken.IsCancellationRequested)
            {
              Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Creating File Migration Task: " + fileKey);
              string str;
              Task task = Task.Run((Func<Task>) (async () => str = await this.MigrateFile(fileKey, (IProgress<string>) downloadProgress, (IProgress<string>) uploadProgress, this._cancelToken.Token)));
              Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Storing File Migration Task: " + (object) task.Id);
              taskList.Add(task);
            }
          }
          Task task1 = await Task.WhenAny((IEnumerable<Task>) taskList);
          taskList.Remove(task1);
          Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Checking File Migration Task Result: " + (object) task1.Id);
          if (task1.IsFaulted)
          {
            Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Error, nameof (SkyDriveLiteMigrationDialog), "Cancelling (Exception)");
            this._cancelToken.Cancel();
            int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to prepare the loan:\n\n" + task1.Exception.InnerException.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Closing Dialog: " + (!this._cancelToken.IsCancellationRequested).ToString());
        if (!this._cancelToken.IsCancellationRequested)
          this.DialogResult = DialogResult.OK;
        else
          this.DialogResult = DialogResult.Cancel;
      }));
    }

    private async Task<string> MigrateFile(
      string fileKey,
      IProgress<string> downloadProgress,
      IProgress<string> uploadProgress,
      CancellationToken ct)
    {
      SkyDriveLiteMigrationDialog liteMigrationDialog = this;
      Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Entering MigrateFile: " + fileKey);
      try
      {
        string tempFileName = SystemSettings.GetTempFileName(fileKey);
        Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Calling GetSupportingDataOnCIFs: " + fileKey);
        using (BinaryObject supportingDataOnCiFs = liteMigrationDialog._loanDataMgr.LoanObject.GetSupportingDataOnCIFs(fileKey))
        {
          if (supportingDataOnCiFs == null)
            throw new FileNotFoundException(fileKey);
          supportingDataOnCiFs.DownloadProgress += new DownloadProgressEventHandler(liteMigrationDialog.TransferProgress);
          try
          {
            Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Calling Download: " + fileKey);
            supportingDataOnCiFs.Download();
          }
          finally
          {
            supportingDataOnCiFs.DownloadProgress -= new DownloadProgressEventHandler(liteMigrationDialog.TransferProgress);
          }
          Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Calling Write: " + tempFileName);
          supportingDataOnCiFs.Write(tempFileName);
        }
        downloadProgress.Report(fileKey);
        using (BinaryObject data = new BinaryObject(tempFileName, false))
        {
          Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Creating SkyDriveStreamingClient");
          SkyDriveStreamingClient client = new SkyDriveStreamingClient(liteMigrationDialog._loanDataMgr);
          client.UploadProgress += new DownloadProgressEventHandler(liteMigrationDialog.TransferProgress);
          try
          {
            Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Calling SaveSupportingData: " + fileKey);
            string str = await client.SaveSupportingData(fileKey, data);
          }
          finally
          {
            client.UploadProgress -= new DownloadProgressEventHandler(liteMigrationDialog.TransferProgress);
          }
          client = (SkyDriveStreamingClient) null;
        }
        uploadProgress.Report(fileKey);
        return fileKey;
      }
      catch (CanceledOperationException ex)
      {
        Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Migration Cancelled: " + fileKey);
        return (string) null;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "SkyDriveLiteMigrationDialog: Error in MigrateFile. Ex: " + (object) ex);
        throw;
      }
    }

    private void TransferProgress(object sender, DownloadProgressEventArgs e)
    {
      if (!this._cancelToken.IsCancellationRequested)
        return;
      e.Cancel = true;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(SkyDriveLiteMigrationDialog.sw, TraceLevel.Verbose, nameof (SkyDriveLiteMigrationDialog), "Cancelling (User)");
      this._cancelToken.Cancel();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblUpload = new Label();
      this.pbUpload = new ProgressBar();
      this.btnCancel = new Button();
      this.lblDownload = new Label();
      this.pbDownload = new ProgressBar();
      this.SuspendLayout();
      this.lblUpload.AutoSize = true;
      this.lblUpload.Location = new Point(12, 47);
      this.lblUpload.Name = "lblUpload";
      this.lblUpload.Size = new Size(90, 14);
      this.lblUpload.TabIndex = 2;
      this.lblUpload.Text = "Upload Progress:";
      this.pbUpload.Location = new Point(12, 63);
      this.pbUpload.Name = "pbUpload";
      this.pbUpload.Size = new Size(374, 16);
      this.pbUpload.TabIndex = 3;
      this.btnCancel.Location = new Point(312, 87);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblDownload.AutoSize = true;
      this.lblDownload.Location = new Point(12, 11);
      this.lblDownload.Name = "lblDownload";
      this.lblDownload.Size = new Size(106, 14);
      this.lblDownload.TabIndex = 0;
      this.lblDownload.Text = "Download Progress:";
      this.pbDownload.Location = new Point(12, 27);
      this.pbDownload.Name = "pbDownload";
      this.pbDownload.Size = new Size(374, 16);
      this.pbDownload.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(398, 121);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblUpload);
      this.Controls.Add((Control) this.pbUpload);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblDownload);
      this.Controls.Add((Control) this.pbDownload);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SkyDriveLiteMigrationDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Preparing Loan";
      this.Load += new EventHandler(this.SkyDriveLiteMigrationDialog_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
