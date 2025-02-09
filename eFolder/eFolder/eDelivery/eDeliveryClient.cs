// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.eDeliveryClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.eFolder.eDelivery.MediaServer;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class eDeliveryClient : Form
  {
    private const string className = "eDeliveryClient";
    private static readonly string sw = Tracing.SwEFolder;
    public string packageID;
    public bool sentNotifications;
    public DialogResult result;
    public CancellationTokenSource cancellationTokenSource;
    public CancellationToken cancellationToken;
    private int requestCount;
    private int requestIndex;
    private int percentProgress;
    private const string ErrorMessage = "Your connection to the Encompass Server was lost and the electronic document package was not sent. Please try again.";
    private IContainer components;
    private Button btnCancel;
    private ProgressBar pbOverall;
    private Label lblOverall;

    private void VerifyEncompassServerConnection()
    {
      if (!(Session.ServerRealTime != DateTime.MinValue))
      {
        Tracing.Log(eDeliveryClient.sw, TraceLevel.Error, nameof (eDeliveryClient), "Lost connection to encompass server.");
        throw new EncompassServerConnectionException("Your connection to the Encompass Server was lost and the electronic document package was not sent. Please try again.");
      }
    }

    public eDeliveryClient() => this.InitializeComponent();

    public string OriginatorSignerUrl { get; private set; }

    public Task<object> ShowDialogAsync()
    {
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      EventHandler onShown = (EventHandler) null;
      onShown = (EventHandler) ((sender, e) =>
      {
        this.Shown -= onShown;
        tcs.TrySetResult((object) null);
      });
      this.Shown += onShown;
      int num;
      SynchronizationContext.Current.Post((SendOrPostCallback) (_ => num = (int) this.ShowDialog((IWin32Window) this.Owner)), (object) null);
      return tcs.Task;
    }

    public void SendMessage(eDeliveryMessage msg)
    {
      using (PerformanceMeter.StartNew("eDlvryClntSndMsg", "DOCS eDeliveryClient Worker Thread SendMessage", 85, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN - Thread Started", 88, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
        Tracing.Log(eDeliveryClient.sw, TraceLevel.Verbose, nameof (eDeliveryClient), "Thread Started");
        try
        {
          Request request = msg.CreateRequest();
          PerformanceMeter.Current.AddCheckpoint("CreateRequest", 94, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
          this.requestCount = msg.Attachments.Length + 4;
          this.requestIndex = 0;
          this.percentProgress = ++this.requestIndex * 100 / this.requestCount;
          this.ProgressChanged(this.percentProgress);
          try
          {
            Tracing.Log(eDeliveryClient.sw, TraceLevel.Verbose, nameof (eDeliveryClient), "GetRecipientURL");
            this.GetRecipientURL(request);
            msg.UpdateEmailBodyRequest(request);
            this.percentProgress = ++this.requestIndex * 100 / this.requestCount;
            this.ProgressChanged(this.percentProgress);
            if (this.cancellationToken.IsCancellationRequested)
            {
              PerformanceMeter.Current.AddCheckpoint("EXIT - Cancel Requested", 119, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
            }
            else
            {
              this.VerifyEncompassServerConnection();
              PerformanceMeter.Current.AddCheckpoint("BEFORE WaitAll UploadAttachments", (int) sbyte.MaxValue, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
              Task.WaitAll(this.UploadAttachments(request));
              PerformanceMeter.Current.AddCheckpoint("AFTER WaitAll UploadAttachments", 129, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
              if (this.cancellationToken.IsCancellationRequested)
              {
                PerformanceMeter.Current.AddCheckpoint("EXIT - Cancel Requested", 134, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
              }
              else
              {
                this.VerifyEncompassServerConnection();
                using (StreamWriter text = System.IO.File.CreateText(Path.GetTempPath() + "LastEDMRequest.json"))
                  text.Write(JsonConvert.SerializeObject((object) request));
                this.ChangeCancelButtonVisibility(false);
                EDeliveryServiceClient edeliveryServiceClient = new EDeliveryServiceClient();
                Task<string> package = edeliveryServiceClient.CreatePackage(request);
                PerformanceMeter.Current.AddCheckpoint("BEFORE WaitAll CreatePackage task", 153, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
                Task.WaitAll((Task) package);
                PerformanceMeter.Current.AddCheckpoint("AFTER WaitAll CreatePackage task", 155, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
                this.packageID = package.Result;
                if (string.IsNullOrWhiteSpace(this.packageID))
                {
                  PerformanceMeter.Current.AddCheckpoint("EXIT THROW Unknown Package Identifier", 162, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
                  throw new Exception("Unknown Package Identifier");
                }
                this.percentProgress = ++this.requestIndex * 100 / this.requestCount;
                this.ProgressChanged(this.percentProgress);
                if (this.cancellationToken.IsCancellationRequested)
                {
                  PerformanceMeter.Current.AddCheckpoint("EXIT - Cancel Requested", 173, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
                }
                else
                {
                  this.OriginatorSignerUrl = string.Empty;
                  if (!msg.ElectronicSignature || !((IEnumerable<eDeliveryRecipient>) msg.Recipients).Any<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.AutoSign)))
                    return;
                  Task<string> originatorSignerUrl = edeliveryServiceClient.GenerateEnvelopeOriginatorSignerURL(request);
                  PerformanceMeter.Current.AddCheckpoint("BEFORE WaitAll GenerateEnvelopeOriginatorSignerURL task", 181, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
                  Task.WaitAll((Task) originatorSignerUrl);
                  PerformanceMeter.Current.AddCheckpoint("AFTER WaitAll GenerateEnvelopeOriginatorSignerURL task", 183, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
                  this.OriginatorSignerUrl = originatorSignerUrl.Result;
                }
              }
            }
          }
          catch (Exception ex)
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT THROW Exception - " + ex.Message, 189, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
            throw ex;
          }
        }
        catch (WebException ex)
        {
          if (ex.Status == WebExceptionStatus.RequestCanceled)
          {
            Tracing.Log(eDeliveryClient.sw, TraceLevel.Verbose, nameof (eDeliveryClient), "Transfer Cancelled");
          }
          else
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT THROW WebException - " + ex.Message, 200, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
            throw;
          }
        }
        finally
        {
          PerformanceMeter.Current.AddCheckpoint("END", 206, nameof (SendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
        }
      }
    }

    public void SendNotofications(List<sendNotificationRequest> requests)
    {
      Tracing.Log(eDeliveryClient.sw, TraceLevel.Verbose, nameof (eDeliveryClient), "Thread Started");
      try
      {
        this.requestCount = requests.Count;
        this.requestIndex = 0;
        this.sentNotifications = false;
        try
        {
          EBSServiceClient ebsServiceClient = new EBSServiceClient();
          Tracing.Log(eDeliveryClient.sw, TraceLevel.Verbose, nameof (eDeliveryClient), nameof (SendNotofications));
          foreach (sendNotificationRequest request in requests)
          {
            if (this.cancellationToken.IsCancellationRequested)
              return;
            this.VerifyEncompassServerConnection();
            Task.WaitAll(ebsServiceClient.SendNotification(request));
            this.percentProgress = ++this.requestIndex * 100 / this.requestCount;
            this.ProgressChanged(this.percentProgress);
          }
          this.sentNotifications = true;
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }
      catch (WebException ex)
      {
        if (ex.Status == WebExceptionStatus.RequestCanceled)
          Tracing.Log(eDeliveryClient.sw, TraceLevel.Verbose, nameof (eDeliveryClient), "Transfer Cancelled");
        else
          throw;
      }
    }

    public async Task UploadAttachments(Request request)
    {
      eDeliveryClient eDeliveryClient1 = this;
      using (PerformanceMeter.StartNew("eDlvryClntUplAtmtTsk", "eDeliveryClient.UploadAttachment Async Task", 271, nameof (UploadAttachments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs"))
      {
        try
        {
          PerformanceMeter.Current.AddCheckpoint("BEGIN", 275, nameof (UploadAttachments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
          Task<string> mediaServerUrl = new EDeliveryServiceClient().GenerateMediaServerURL(request);
          Task.WaitAll((Task) mediaServerUrl);
          string mediaServerURL = mediaServerUrl.Result;
          eDeliveryClient eDeliveryClient2 = eDeliveryClient1;
          eDeliveryClient eDeliveryClient3 = eDeliveryClient1;
          int num1 = eDeliveryClient1.requestIndex + 1;
          int num2 = num1;
          eDeliveryClient3.requestIndex = num2;
          int num3 = num1 * 100 / eDeliveryClient1.requestCount;
          eDeliveryClient2.percentProgress = num3;
          eDeliveryClient1.ProgressChanged(eDeliveryClient1.percentProgress);
          MediaServerClient MediaServerClient = new MediaServerClient();
          PerformanceMeter.Current.AddCheckpoint("Before Upload, documents.Count: " + (object) request.documents.Count, 290, nameof (UploadAttachments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
          foreach (Document document1 in request.documents)
          {
            if (!string.IsNullOrEmpty(document1.filePath))
            {
              using (PerformanceMeter.Current.BeginOperation("Upload Attachement MediaServerClient.SaveVaultFile"))
              {
                if (eDeliveryClient1.cancellationToken.IsCancellationRequested)
                {
                  PerformanceMeter.Current.AddCheckpoint("EXIT Cancel Requested", 301, nameof (UploadAttachments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
                  return;
                }
                eDeliveryClient1.VerifyEncompassServerConnection();
                Tracing.Log(eDeliveryClient.sw, TraceLevel.Verbose, nameof (eDeliveryClient), "UploadAttachment");
                Document document = document1;
                document.mediaId = await MediaServerClient.SaveVaultFile(mediaServerURL, ((IEnumerable<char>) Path.GetInvalidFileNameChars()).Aggregate<char, string>(document1.title, (Func<string, char, string>) ((current, c) => current.Replace(c.ToString(), " "))), document1.filePath);
                document = (Document) null;
                eDeliveryClient eDeliveryClient4 = eDeliveryClient1;
                eDeliveryClient eDeliveryClient5 = eDeliveryClient1;
                int num4 = eDeliveryClient1.requestIndex + 1;
                int num5 = num4;
                eDeliveryClient5.requestIndex = num5;
                int num6 = num4 * 100 / eDeliveryClient1.requestCount;
                eDeliveryClient4.percentProgress = num6;
                eDeliveryClient1.ProgressChanged(eDeliveryClient1.percentProgress);
              }
            }
          }
          mediaServerURL = (string) null;
          MediaServerClient = (MediaServerClient) null;
        }
        finally
        {
          PerformanceMeter.Current.AddCheckpoint("END", 320, nameof (UploadAttachments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryClient.cs");
        }
      }
    }

    public virtual void GetRecipientURL(Request request)
    {
      EBSServiceClient ebsServiceClient = new EBSServiceClient();
      GetRecipientURLRequest request1 = new GetRecipientURLRequest();
      request1.loanId = request.applicationGroupId;
      request1.siteId = request.consumerConnectSiteID;
      List<Contact> contactList = new List<Contact>();
      foreach (Recipient recipient in request.recipients.FindAll((Predicate<Recipient>) (x => x.role != eDeliveryEntityType.Originator.ToString("g"))))
      {
        Contact contact = new Contact();
        contactList.Add(contact);
        contact.contactType = recipient.role.ToUpper().Contains("BORROWER") ? "Borrower" : recipient.role;
        contact.borrowerId = recipient.borrowerId;
        contact.authType = "AuthCode";
        contact.authCode = recipient.authCode;
        contact.name = recipient.fullName;
        contact.email = recipient.email;
        contact.recipientId = recipient.recipientId == null ? Guid.NewGuid().ToString() : recipient.recipientId;
      }
      Recipient recipient1 = request.recipients.FirstOrDefault<Recipient>((Func<Recipient, bool>) (x => x.role == eDeliveryEntityType.Originator.ToString("g")));
      if (recipient1 != null)
      {
        Contact contact = new Contact();
        contactList.Add(contact);
        contact.contactType = "EncompassUser";
        contact.userId = recipient1.userId;
      }
      request1.contacts = contactList.ToArray();
      Task<GetRecipientURLResponse> recipientUrl = ebsServiceClient.GetRecipientURL(request1);
      Task.WaitAll((Task) recipientUrl);
      foreach (Laturl laturl1 in ((IEnumerable<Laturl>) recipientUrl.Result.latUrls).ToList<Laturl>())
      {
        Laturl laturl = laturl1;
        Recipient recipient2 = laturl.recipientId != null ? request.recipients.Find((Predicate<Recipient>) (x => x.recipientId == laturl.recipientId)) : request.recipients.Find((Predicate<Recipient>) (x => x.userId == laturl.userId));
        if (recipient2 != null)
        {
          recipient2.url = laturl.url;
          recipient2.id = recipient2.userId == null ? recipient2.recipientId : recipient2.userId;
        }
      }
      if (request.notViewed != null)
      {
        List<string> stringList = new List<string>();
        foreach (Recipient recipient3 in request.recipients.FindAll((Predicate<Recipient>) (x => x.role != eDeliveryEntityType.Originator.ToString("g"))))
          stringList.Add(recipient3.id);
        request.notViewed.recipientIds = stringList.ToArray();
      }
      if (request.fulfillment == null || request.fulfillment.to == null)
        return;
      List<string> stringList1 = new List<string>();
      foreach (Recipient recipient4 in request.recipients.FindAll((Predicate<Recipient>) (x => x.role == eDeliveryEntityType.Borrower.ToString("g") || x.role == eDeliveryEntityType.Coborrower.ToString("g"))))
        stringList1.Add(recipient4.id);
      request.fulfillment.to.recipientIds = stringList1.ToArray();
    }

    public void ProgressChanged(int ProgressPercentage)
    {
      if (this.pbOverall.InvokeRequired)
        this.pbOverall.Invoke((Delegate) (() => this.pbOverall.Value = ProgressPercentage));
      else
        this.pbOverall.Value = ProgressPercentage;
    }

    private void ChangeCancelButtonVisibility(bool visibility)
    {
      if (this.btnCancel.InvokeRequired)
        this.btnCancel.Invoke((Delegate) (() => this.btnCancel.Visible = visibility));
      else
        this.btnCancel.Visible = visibility;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Tracing.Log(eDeliveryClient.sw, TraceLevel.Verbose, nameof (eDeliveryClient), "Cancelling");
      this.cancellationTokenSource.Cancel();
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
      this.pbOverall = new ProgressBar();
      this.lblOverall = new Label();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(312, 66);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.pbOverall.Location = new Point(12, 28);
      this.pbOverall.Name = "pbOverall";
      this.pbOverall.Size = new Size(374, 16);
      this.pbOverall.TabIndex = 1;
      this.lblOverall.AutoSize = true;
      this.lblOverall.Location = new Point(12, 12);
      this.lblOverall.Name = "lblOverall";
      this.lblOverall.Size = new Size(126, 16);
      this.lblOverall.TabIndex = 0;
      this.lblOverall.Text = "Overall Progress...";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(398, 95);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pbOverall);
      this.Controls.Add((Control) this.lblOverall);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (eDeliveryClient);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Sending Package ....";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
