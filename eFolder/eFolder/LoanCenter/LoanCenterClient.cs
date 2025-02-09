// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.LoanCenterClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using Microsoft.Web.Services2.Attachments;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class LoanCenterClient : Form
  {
    private const string className = "LoanCenterClient";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanCenterMessage msg;
    private string packageID;
    private const string ErrorMessage = "Your connection to the Encompass Server was lost and the electronic document package was not sent. Please try again.";
    private IContainer components;
    private Button btnCancel;
    private ProgressBar pbTransfer;
    private Label lblTransfer;
    private ProgressBar pbOverall;
    private Label lblOverall;
    private BackgroundWorker worker;

    private void VerifyEncompassServerConnection()
    {
      if (!(Session.ServerRealTime != DateTime.MinValue))
      {
        Tracing.Log(LoanCenterClient.sw, TraceLevel.Error, nameof (LoanCenterClient), "Lost connection to encompass server.");
        throw new EncompassServerConnectionException("Your connection to the Encompass Server was lost and the electronic document package was not sent. Please try again.");
      }
    }

    public LoanCenterClient() => this.InitializeComponent();

    public string OriginatorSignerUrl { get; private set; }

    public string SendMessage(LoanCenterMessage msg)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 58, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
      Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Checking Requirements");
      if (!msg.CheckRequirements())
      {
        PerformanceMeter.Current.AddCheckpoint("EXIT !msg.CheckRequirements()", 64, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
        return (string) null;
      }
      Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Initializing Request");
      this.msg = msg;
      this.packageID = (string) null;
      Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Starting Thread");
      this.worker.RunWorkerAsync();
      PerformanceMeter.Current.AddCheckpoint("RunWorkerAsync", 76, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
      Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Showing Dialog");
      PerformanceMeter.Current.AddCheckpoint("BEFORE this.ShowDialog", 80, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
      DialogResult dialogResult = this.ShowDialog((IWin32Window) Form.ActiveForm);
      PerformanceMeter.Current.AddCheckpoint("AFTER this.ShowDialog", 82, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
      if (dialogResult != DialogResult.OK)
      {
        PerformanceMeter.Current.AddCheckpoint("EXIT - result != DialogResult.OK", 85, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
        return (string) null;
      }
      Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Logging Transaction");
      Transaction.Log(msg);
      PerformanceMeter.Current.AddCheckpoint("END", 93, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
      return this.packageID;
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      using (PerformanceMeter.StartNew("LnCtrClntWkThrdDoWrk", "DOCS LoanCenterClient Worker Thread Do Work", 112, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 114, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
        Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Thread Started");
        try
        {
          string xml = this.msg.ToXml();
          using (StreamWriter text = System.IO.File.CreateText(Path.GetTempPath() + "LastEDMRequest.xml"))
            text.Write(xml);
          PerformanceMeter.Current.AddCheckpoint("Write File LastEDMRequest.xml", (int) sbyte.MaxValue, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
          using (ePackageWse ePackageWse = new ePackageWse(Session.SessionObjects?.StartupInfo?.ServiceUrls?.EPackageServiceUrl))
          {
            int num1 = this.msg.Attachments.Length + 2;
            int num2 = 0;
            ePackageWse.ChunkSent += new ChunkHandler(this.chunkSent);
            try
            {
              this.VerifyEncompassServerConnection();
              if (this.msg.MsgType == LoanCenterMessageType.Consent && this.InvokeRequired)
                this.Invoke((Delegate) (() => this.btnCancel.Visible = false));
              Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "RegisterRequest");
              string str = ePackageWse.RegisterRequest(xml);
              PerformanceMeter.Current.AddCheckpoint("service.RegisterRequest", 154, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
              int num3;
              this.worker.ReportProgress((num3 = num2 + 1) * 100 / num1, (object) this.pbOverall);
              using (StreamWriter text = System.IO.File.CreateText(Path.GetTempPath() + "LastEDMResponse.xml"))
                text.Write(str);
              PerformanceMeter.Current.AddCheckpoint("Write File LastEDMResponse.xml", 164, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
              XmlDocument xmlDocument = new XmlDocument();
              xmlDocument.LoadXml(str);
              string attribute = xmlDocument.DocumentElement.GetAttribute("packageguid");
              if (string.IsNullOrWhiteSpace(attribute))
              {
                PerformanceMeter.Current.AddCheckpoint("EXIT THROW Unknown Package Identifier", 174, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
                throw new Exception("Unknown Package Identifier");
              }
              if (this.msg.MsgType != LoanCenterMessageType.Consent)
              {
                foreach (LoanCenterAttachment attachment in this.msg.Attachments)
                {
                  using (PerformanceMeter.Current.BeginOperation("LoanCenterClient.workerDoWork.ProcessAttachment"))
                  {
                    if (this.worker.CancellationPending)
                    {
                      PerformanceMeter.Current.AddCheckpoint("EXIT worker.CancellationPending", 189, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
                      return;
                    }
                    this.VerifyEncompassServerConnection();
                    Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Clearing Attachments");
                    ePackageWse.RequestSoapContext.Attachments.Clear();
                    Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Adding Attachment");
                    ePackageWse.RequestSoapContext.Attachments.Add((Attachment) attachment.DimeAttachment);
                    Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "UploadAttachment");
                    ePackageWse.UploadAttachment(str);
                    this.worker.ReportProgress(++num3 * 100 / num1, (object) this.pbOverall);
                  }
                }
                if (this.worker.CancellationPending)
                  return;
                Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Clearing Attachments");
                ePackageWse.RequestSoapContext.Attachments.Clear();
              }
              this.VerifyEncompassServerConnection();
              if (this.InvokeRequired)
                this.Invoke((Delegate) (() => this.btnCancel.Visible = false));
              Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "ProcessRequest");
              this.OriginatorSignerUrl = ePackageWse.ProcessRequestDocuSign(str);
              this.packageID = attribute;
            }
            finally
            {
              ePackageWse.ChunkSent -= new ChunkHandler(this.chunkSent);
            }
          }
        }
        catch (WebException ex)
        {
          if (ex.Status == WebExceptionStatus.RequestCanceled)
          {
            PerformanceMeter.Current.AddCheckpoint("CATCH - Transfer Cancelled", 250, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
            Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Transfer Cancelled");
          }
          else
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT throw from within catch", (int) byte.MaxValue, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
            throw;
          }
        }
        finally
        {
          PerformanceMeter.Current.AddCheckpoint("END", 261, nameof (worker_DoWork), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
        }
      }
    }

    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      ((ProgressBar) e.UserState).Value = e.ProgressPercentage;
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        Tracing.Log(LoanCenterClient.sw, TraceLevel.Error, nameof (LoanCenterClient), e.Error.ToString());
        PerformanceMeter.Current.AddCheckpoint("BEFORE error ShowDialog()", 287, nameof (worker_RunWorkerCompleted), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to send the package:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        PerformanceMeter.Current.AddCheckpoint("AFTER error ShowDialog()", 291, nameof (worker_RunWorkerCompleted), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterClient.cs");
      }
      Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Closing Window");
      if (!string.IsNullOrWhiteSpace(this.packageID))
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void chunkSent(object sender, ChunkHandlerEventArgs e)
    {
      if (this.worker.CancellationPending)
        e.Abort = true;
      this.worker.ReportProgress(e.Percent, (object) this.pbTransfer);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(LoanCenterClient.sw, TraceLevel.Verbose, nameof (LoanCenterClient), "Cancelling");
      this.worker.CancelAsync();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.pbTransfer = new ProgressBar();
      this.lblTransfer = new Label();
      this.pbOverall = new ProgressBar();
      this.lblOverall = new Label();
      this.worker = new BackgroundWorker();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(312, 90);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.pbTransfer.Location = new Point(12, 64);
      this.pbTransfer.Name = "pbTransfer";
      this.pbTransfer.Size = new Size(374, 16);
      this.pbTransfer.TabIndex = 3;
      this.lblTransfer.AutoSize = true;
      this.lblTransfer.Location = new Point(12, 48);
      this.lblTransfer.Name = "lblTransfer";
      this.lblTransfer.Size = new Size(105, 14);
      this.lblTransfer.TabIndex = 2;
      this.lblTransfer.Text = "Transfer Progress...";
      this.pbOverall.Location = new Point(12, 28);
      this.pbOverall.Name = "pbOverall";
      this.pbOverall.Size = new Size(374, 16);
      this.pbOverall.TabIndex = 1;
      this.lblOverall.AutoSize = true;
      this.lblOverall.Location = new Point(12, 12);
      this.lblOverall.Name = "lblOverall";
      this.lblOverall.Size = new Size(97, 14);
      this.lblOverall.TabIndex = 0;
      this.lblOverall.Text = "Overall Progress...";
      this.worker.WorkerReportsProgress = true;
      this.worker.WorkerSupportsCancellation = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(398, 121);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pbOverall);
      this.Controls.Add((Control) this.lblOverall);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.pbTransfer);
      this.Controls.Add((Control) this.lblTransfer);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanCenterClient);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Center";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
