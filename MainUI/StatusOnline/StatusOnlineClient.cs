// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.StatusOnline.StatusOnlineClient
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.StatusOnline
{
  public class StatusOnlineClient : Form
  {
    private const string className = "StatusOnlineClient";
    private static readonly string sw = Tracing.SwStatusOnline;
    private LoanDataMgr loanDataMgr;
    private string packageGUID;
    private StatusOnlineLoanSetup statusOnlineLoanSetup;
    private List<StatusOnlineTrigger> publishTriggers;
    private bool showPrompt;
    private List<StatusOnlineTrigger> automaticEmailTriggers;
    private IContainer components;
    private Label lblProgress;
    private BackgroundWorker worker1;
    private BackgroundWorker worker2;

    public StatusOnlineClient(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
    }

    public string Publish(
      StatusOnlineLoanSetup statusOnlineLoanSetup,
      List<StatusOnlineTrigger> publishTriggers,
      bool showPrompt)
    {
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Initializing Status Online Publish");
      this.packageGUID = (string) null;
      this.statusOnlineLoanSetup = statusOnlineLoanSetup;
      this.publishTriggers = publishTriggers;
      this.showPrompt = showPrompt;
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Starting Thread");
      this.worker1.RunWorkerAsync();
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Showing Dialog");
      if (this.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
        return (string) null;
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Package guid: " + this.packageGUID);
      return this.packageGUID;
    }

    public void SendAutomaticEmails(
      string packageGUID,
      List<StatusOnlineTrigger> automaticEmailTriggers)
    {
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Initializing Status Online Automatic Emails");
      this.lblProgress.Text = "Sending automatic emails...";
      this.packageGUID = packageGUID;
      this.automaticEmailTriggers = automaticEmailTriggers;
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Starting Thread");
      this.worker2.RunWorkerAsync();
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Showing Dialog");
      this.ShowDialog((IWin32Window) Form.ActiveForm);
    }

    private void worker1_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Thread Started");
      try
      {
        Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Building publish xml");
        string onlinePublishXml = StatusOnlineClient.getStatusOnlinePublishXml(this.loanDataMgr, this.publishTriggers);
        Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Registering status online");
        string packageGUID = this.registerStatusOnline();
        if (string.IsNullOrEmpty(packageGUID))
          return;
        Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Uploading status file");
        if (this.uploadStatus(packageGUID, onlinePublishXml) != string.Empty)
          return;
        Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Updating published triggers");
        this.statusOnlineLoanSetup = StatusOnlineClient.updatePublishedTriggers(this.publishTriggers, this.showPrompt);
        Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Setting package identifier: " + packageGUID);
        this.packageGUID = packageGUID;
      }
      catch (WebException ex)
      {
        if (ex.Status == WebExceptionStatus.RequestCanceled)
          Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Transfer Cancelled");
        else
          throw;
      }
    }

    private void worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        MetricsFactory.IncrementErrorCounter(e.Error, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClient.cs", nameof (worker1_RunWorkerCompleted), 141);
        Tracing.Log(StatusOnlineClient.sw, TraceLevel.Error, nameof (StatusOnlineClient), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to publish the package:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Closing Publish Window");
      if (this.packageGUID != null)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void worker2_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Thread Started");
      try
      {
        Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Automatic email count: " + (object) this.automaticEmailTriggers.Count);
        foreach (StatusOnlineTrigger automaticEmailTrigger in this.automaticEmailTriggers)
        {
          Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Sending automatic emails");
          this.sendStatusTriggerEmail(automaticEmailTrigger);
        }
      }
      catch (WebException ex)
      {
        if (ex.Status == WebExceptionStatus.RequestCanceled)
          Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Transfer Cancelled");
        else
          throw;
      }
    }

    private void worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        MetricsFactory.IncrementErrorCounter(e.Error, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClient.cs", nameof (worker2_RunWorkerCompleted), 189);
        Tracing.Log(StatusOnlineClient.sw, TraceLevel.Error, nameof (StatusOnlineClient), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when sending status online automatic emails:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Closing Send Automatic Email Window");
      this.DialogResult = DialogResult.OK;
    }

    private static StatusOnlineLoanSetup updatePublishedTriggers(
      List<StatusOnlineTrigger> publishTriggers,
      bool showPrompt)
    {
      LoanIdentity loanIdentity = Session.LoanManager.GetLoanIdentity(Session.LoanData.GUID);
      StatusOnlineLoanSetup statusOnlineLoanSetup = Session.LoanManager.SaveStatusOnlineTriggers(loanIdentity, publishTriggers.ToArray());
      if (statusOnlineLoanSetup.ShowPrompt != showPrompt)
        Session.LoanManager.SetStatusOnlinePrompt(loanIdentity, showPrompt);
      return statusOnlineLoanSetup;
    }

    private static string getStatusOnlinePublishXml(
      LoanDataMgr loanDataMgr,
      List<StatusOnlineTrigger> publishTriggers)
    {
      return new StatusOnlineMessage(loanDataMgr, publishTriggers[0].PortalType).ToStatusXml(publishTriggers);
    }

    private string registerStatusOnline()
    {
      bool flag = false;
      string empty = string.Empty;
      string str1 = string.Empty;
      do
      {
        empty = string.Empty;
        string str2;
        try
        {
          str1 = this.register();
          break;
        }
        catch (SoapException ex)
        {
          MetricsFactory.IncrementErrorCounter((Exception) ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClient.cs", nameof (registerStatusOnline), 253);
          str2 = ex.Detail.InnerText;
        }
        catch (Exception ex)
        {
          MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClient.cs", nameof (registerStatusOnline), 258);
          str2 = ex.Message;
        }
        if (str2 != string.Empty)
        {
          DialogResult dialogResult = Utils.Dialog((IWin32Window) this, "The following error occurred when trying to publish the loan status :\n\n" + str2 + ". Do you wish to retry publishing?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
          if (dialogResult == DialogResult.No)
            flag = false;
          if (dialogResult == DialogResult.Yes)
            flag = true;
        }
      }
      while (flag);
      return str1;
    }

    private string register()
    {
      string empty = string.Empty;
      StatusOnlineMessage statusOnlineMessage = new StatusOnlineMessage(this.loanDataMgr, this.publishTriggers[0].PortalType);
      if (!statusOnlineMessage.CheckRequirements())
        return string.Empty;
      string registerXml = statusOnlineMessage.ToRegisterXml();
      using (StreamWriter text = System.IO.File.CreateText(Path.GetTempPath() + "LastStatusOnlineRequest.xml"))
        text.Write(registerXml);
      using (ePackageWse ePackageWse = new ePackageWse(Session.SessionObjects?.StartupInfo?.ServiceUrls?.EPackageServiceUrl))
        return ePackageWse.RegisterStatusOnlineRequest(registerXml);
    }

    private string uploadStatus(string packageGUID, string publishXml)
    {
      bool flag = false;
      string empty = string.Empty;
      string str;
      do
      {
        str = string.Empty;
        try
        {
          this.upload(packageGUID, publishXml);
          break;
        }
        catch (SoapException ex)
        {
          MetricsFactory.IncrementErrorCounter((Exception) ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClient.cs", nameof (uploadStatus), 313);
          str = ex.Detail.InnerText;
        }
        catch (Exception ex)
        {
          MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClient.cs", nameof (uploadStatus), 318);
          str = ex.Message;
        }
        if (str != string.Empty)
        {
          DialogResult dialogResult = Utils.Dialog((IWin32Window) null, "The following error occurred when trying to publish the loan status :\n\n" + str + ". Do you wish to retry publishing?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
          if (dialogResult == DialogResult.No)
            flag = false;
          if (dialogResult == DialogResult.Yes)
            flag = true;
        }
      }
      while (flag);
      return str;
    }

    private void upload(string packageGUID, string publishXML)
    {
      try
      {
        Guid guid = new Guid(packageGUID);
      }
      catch
      {
        throw new Exception("The given package identifier is invalid");
      }
      using (ePackageWse ePackageWse = new ePackageWse(Session.SessionObjects?.StartupInfo?.ServiceUrls?.EPackageServiceUrl))
        ePackageWse.UploadStatus(packageGUID, publishXML);
    }

    private void sendStatusTriggerEmail(StatusOnlineTrigger trigger)
    {
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Inside Send Status Trigger Email");
      if (string.IsNullOrEmpty(trigger.EmailTemplate))
        return;
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Trigger Email Template found");
      string userid = string.Empty;
      string email = string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string name = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, trigger.EmailFromType, trigger.OwnerID, out userid, out email, out name);
      if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(email))
        return;
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "From user id: " + userid + " From Email: " + email);
      string triggerRecipients = StatusOnlineManager.GetTriggerRecipients(this.loanDataMgr, trigger.EmailRecipients, true);
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "To email: " + triggerRecipients);
      string triggerEmailBody = this.getTriggerEmailBody(trigger, userid);
      HtmlEmailTemplate htmlEmailTemplate = Session.ConfigurationManager.GetHtmlEmailTemplate(trigger.EmailTemplateOwner, trigger.EmailTemplate);
      RecipientInfo recipientInfo = this.CreateRecipientInfo(this.packageGUID, trigger.Guid, email, name, triggerRecipients, htmlEmailTemplate.Subject, triggerEmailBody, true, StatusOnlineManager.IsTPOConnectLoan(this.loanDataMgr));
      string str1 = this.ValidateRecipientInfo(recipientInfo);
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Validate Recipient result: " + str1);
      if (!(str1 == string.Empty))
        return;
      string str2 = this.SendRecipientEmail(recipientInfo);
      Tracing.Log(StatusOnlineClient.sw, TraceLevel.Verbose, nameof (StatusOnlineClient), "Send Recipient email result: " + str2);
    }

    private string getTriggerEmailBody(StatusOnlineTrigger trigger, string fromuserid)
    {
      string triggerEmailBody = string.Empty;
      string emailTemplate = trigger.EmailTemplate;
      if (!string.IsNullOrEmpty(emailTemplate))
      {
        HtmlEmailTemplate htmlEmailTemplate = Session.ConfigurationManager.GetHtmlEmailTemplate(trigger.EmailTemplateOwner, emailTemplate);
        if (htmlEmailTemplate != (HtmlEmailTemplate) null)
        {
          UserInfo userInfo = trigger.EmailFromType != TriggerEmailFromType.CurrentUser ? Session.OrganizationManager.GetUser(fromuserid) : Session.UserInfo;
          triggerEmailBody = new HtmlFieldMerge(htmlEmailTemplate.Html).MergeContent(this.loanDataMgr.LoanData, userInfo);
        }
      }
      return triggerEmailBody;
    }

    public RecipientInfo CreateRecipientInfo(
      string packageGUID,
      string triggerGUID,
      string from,
      string fromName,
      string to,
      string subject,
      string body,
      bool useHTMLBody,
      bool isTPOConnectLoan)
    {
      return new RecipientInfo()
      {
        packageGUID = packageGUID,
        triggerGUID = triggerGUID,
        from = from,
        fromName = fromName,
        to = to,
        subject = subject,
        body = body,
        useHTMLBody = useHTMLBody,
        isTPOConnectLoan = isTPOConnectLoan
      };
    }

    public string ValidateRecipientInfo(RecipientInfo recipient)
    {
      string str = string.Empty;
      List<string> stringList = new List<string>();
      try
      {
        Guid guid = new Guid(recipient.packageGUID);
      }
      catch
      {
        stringList.Add("The given package identifier is invalid.");
      }
      if (recipient.from == string.Empty)
        stringList.Add("The 'From' email address must be filled in.");
      else if (!Utils.ValidateEmail(recipient.from))
        stringList.Add("Invalid 'From' email address.");
      if (recipient.to == string.Empty)
        stringList.Add("The 'To' email address must be filled in.");
      else if (!Utils.ValidateEmail(recipient.to))
        stringList.Add("Invalid 'To' email address.");
      if (recipient.subject == string.Empty)
        stringList.Add("Subject is required.");
      if (stringList.Count > 0)
      {
        for (int index = 0; index < stringList.Count; ++index)
          str = str + "\n\n(" + Convert.ToString(index + 1) + ") " + stringList[index];
      }
      return str;
    }

    public string SendRecipientEmail(RecipientInfo recipient)
    {
      bool flag = false;
      string empty = string.Empty;
      string str;
      do
      {
        str = string.Empty;
        try
        {
          this.sendRecipientEmail(recipient);
          break;
        }
        catch (SoapException ex)
        {
          MetricsFactory.IncrementErrorCounter((Exception) ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClient.cs", nameof (SendRecipientEmail), 512);
          str = ex.Detail.InnerText;
        }
        catch (Exception ex)
        {
          MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClient.cs", nameof (SendRecipientEmail), 517);
          str = ex.Message;
        }
        if (str != string.Empty)
        {
          DialogResult dialogResult = Utils.Dialog((IWin32Window) null, "The following error occurred when trying to send the loan status email:\n\n" + str + ". Do you wish to retry sending the email?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
          if (dialogResult == DialogResult.No)
            flag = false;
          if (dialogResult == DialogResult.Yes)
            flag = true;
        }
      }
      while (flag);
      if (str == string.Empty)
        this.createLogEntry(recipient);
      return str;
    }

    private void createLogEntry(RecipientInfo recipient)
    {
      Session.LoanDataMgr.AddOperationLog((LogRecordBase) new HtmlEmailLog(Session.UserInfo.FullName + " (" + Session.UserID + ")")
      {
        Description = "Status Online Update",
        Sender = recipient.from,
        Recipient = recipient.to,
        Subject = recipient.subject,
        Body = recipient.body
      });
    }

    private void sendRecipientEmail(RecipientInfo recipient)
    {
      CompanyInfo companyInfo = Session.CompanyInfo;
      UserInfo userInfo = Session.UserInfo;
      SenderInfo senderInfo = new SenderInfo();
      senderInfo.clientid = companyInfo.ClientID;
      senderInfo.userid = userInfo.Userid;
      senderInfo.userPassword = EpassLogin.LoginPassword;
      using (ePackageWse ePackageWse = new ePackageWse(Session.SessionObjects?.StartupInfo?.ServiceUrls?.EPackageServiceUrl))
        ePackageWse.SendRecipientEmail(recipient, senderInfo);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblProgress = new Label();
      this.worker1 = new BackgroundWorker();
      this.worker2 = new BackgroundWorker();
      this.SuspendLayout();
      this.lblProgress.AutoSize = true;
      this.lblProgress.Location = new Point(12, 17);
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new Size(67, 14);
      this.lblProgress.TabIndex = 0;
      this.lblProgress.Text = "Publishing ...";
      this.worker1.DoWork += new DoWorkEventHandler(this.worker1_DoWork);
      this.worker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker1_RunWorkerCompleted);
      this.worker2.DoWork += new DoWorkEventHandler(this.worker2_DoWork);
      this.worker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker2_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(303, 47);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblProgress);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "StatusOnline";
      this.Text = "Status Online";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
