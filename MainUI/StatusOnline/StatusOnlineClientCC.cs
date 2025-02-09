// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.StatusOnline.StatusOnlineClientCC
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.StatusOnline
{
  public class StatusOnlineClientCC : Form
  {
    protected const string className = "StatusOnlineClient";
    protected static readonly string sw = Tracing.SwStatusOnline;
    protected LoanDataMgr loanDataMgr;
    protected string packageGUID = string.Empty;
    private List<StatusOnlineTrigger> automaticEmailTriggers;
    private IContainer components;
    private Label lblProgress;
    private BackgroundWorker worker1;
    private BackgroundWorker worker2;

    public StatusOnlineClientCC(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
    }

    public void SendAutomaticEmails(List<StatusOnlineTrigger> automaticEmailTriggers)
    {
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Initializing Status Online Automatic Emails");
      this.lblProgress.Text = "Sending automatic emails...";
      this.automaticEmailTriggers = automaticEmailTriggers;
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Starting Thread");
      this.worker2.RunWorkerAsync();
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Showing Dialog");
      this.ShowDialog((IWin32Window) Form.ActiveForm);
    }

    private void worker2_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Thread Started");
      try
      {
        Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Automatic email count: " + (object) this.automaticEmailTriggers.Count);
        foreach (StatusOnlineTrigger automaticEmailTrigger in this.automaticEmailTriggers)
        {
          Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Sending automatic emails");
          this.sendStatusTriggerEmail(automaticEmailTrigger);
        }
      }
      catch (WebException ex)
      {
        if (ex.Status == WebExceptionStatus.RequestCanceled)
          Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Transfer Cancelled");
        else
          throw;
      }
    }

    private void worker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        MetricsFactory.IncrementErrorCounter(e.Error, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClientCC.cs", nameof (worker2_RunWorkerCompleted), 99);
        Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Error, "StatusOnlineClient", e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when sending status online automatic emails:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Closing Send Automatic Email Window");
      this.DialogResult = DialogResult.OK;
    }

    public StatusOnlineLoanSetup updatePublishedTriggers(
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

    private void sendStatusTriggerEmail(StatusOnlineTrigger trigger)
    {
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Inside Send Status Trigger Email");
      if (string.IsNullOrEmpty(trigger.EmailTemplate))
        return;
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Trigger Email Template found");
      string userid = string.Empty;
      string email = string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string name = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, trigger.EmailFromType, trigger.OwnerID, out userid, out email, out name);
      if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(email))
        return;
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "From user id: " + userid + " From Email: " + email);
      string str = this.sendNotifications(trigger);
      Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Send notifications for automated status online. result: " + str);
    }

    protected string getTriggerEmailBody(StatusOnlineTrigger trigger, string fromuserid)
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
      bool useHTMLBody)
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
        useHTMLBody = useHTMLBody
      };
    }

    public string ValidateRecipientInfo(RecipientInfo recipient)
    {
      string str = string.Empty;
      List<string> stringList = new List<string>();
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

    protected virtual string sendNotifications(StatusOnlineTrigger trigger)
    {
      List<ContactDetails> contactDetailsList1 = new List<ContactDetails>();
      bool flag = false;
      string str1 = string.Empty;
      string empty1 = string.Empty;
      string userid = string.Empty;
      string name = string.Empty;
      string email = string.Empty;
      string fromName = string.Empty;
      string str2 = string.Empty;
      string simpleField = this.loanDataMgr.LoanData.GetSimpleField("3239");
      if (simpleField != string.Empty)
      {
        UserInfo user = Session.OrganizationManager.GetUser(simpleField);
        if (user != (UserInfo) null)
        {
          fromName = this.loanDataMgr.LoanData.GetSimpleField("1612");
          if (fromName == string.Empty)
            fromName = user.FullName;
          if (string.IsNullOrEmpty(fromName))
            fromName = user.FirstName + " " + user.LastName;
          str2 = user.Email;
        }
      }
      if (string.IsNullOrEmpty(str2))
        return "No originator email address.";
      EBSServiceClient ebsServiceClient = new EBSServiceClient();
      Task<List<ContactDetails>> loanContacts = ebsServiceClient.GetLoanContacts(this.loanDataMgr.LoanData.GetField("GUID"));
      Task.WaitAll((Task) loanContacts);
      List<ContactDetails> contactDetailsList2 = loanContacts.Result == null ? new List<ContactDetails>() : loanContacts.Result;
      ContactDetails existingContact = (ContactDetails) null;
      ContactDetails contactDetails1 = (ContactDetails) null;
      Random random = new Random(DateTime.Now.Millisecond);
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      foreach (string emailRecipient in trigger.EmailRecipients)
      {
        existingContact = (ContactDetails) null;
        string userName = string.Empty;
        string field = this.loanDataMgr.LoanData.GetField(emailRecipient);
        if (!string.IsNullOrEmpty(field))
        {
          switch (emailRecipient)
          {
            case "1240":
              if (contactDetailsList2 != null)
                existingContact = contactDetailsList2.Find((Predicate<ContactDetails>) (x => x.borrowerId == this.loanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id));
              userName = this.loanDataMgr.LoanData.GetSimpleField("1868");
              if (string.IsNullOrEmpty(userName))
                userName = string.Join(" ", ((IEnumerable<string>) new string[4]
                {
                  this.loanDataMgr.LoanData.GetSimpleField("4000"),
                  this.loanDataMgr.LoanData.GetSimpleField("4001"),
                  this.loanDataMgr.LoanData.GetSimpleField("4002"),
                  this.loanDataMgr.LoanData.GetSimpleField("4003")
                }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
              contactDetails1 = this.GetContactForNotification(existingContact, userName, field, this.loanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id);
              break;
            case "1268":
              if (contactDetailsList2 != null)
                existingContact = contactDetailsList2.Find((Predicate<ContactDetails>) (x => x.borrowerId == this.loanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id));
              userName = this.loanDataMgr.LoanData.GetSimpleField("1873");
              if (string.IsNullOrEmpty(userName))
                userName = string.Join(" ", ((IEnumerable<string>) new string[4]
                {
                  this.loanDataMgr.LoanData.GetSimpleField("4004"),
                  this.loanDataMgr.LoanData.GetSimpleField("4005"),
                  this.loanDataMgr.LoanData.GetSimpleField("4006"),
                  this.loanDataMgr.LoanData.GetSimpleField("4007")
                }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
              contactDetails1 = this.GetContactForNotification(existingContact, userName, field, this.loanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id);
              break;
            case "VEND.X141":
              if (contactDetailsList2 != null)
                existingContact = contactDetailsList2.Find((Predicate<ContactDetails>) (x => x.contactType.ToUpper() == "Buyer's Agent".ToUpper()));
              if (string.IsNullOrEmpty(userName))
                userName = this.loanDataMgr.LoanData.GetField("VEND.X139");
              contactDetails1 = this.GetContactForNotification(existingContact, userName, field, (string) null, "Buyer's Agent");
              break;
          }
          if (!string.IsNullOrEmpty(userName) && contactDetails1 != null && existingContact == null)
          {
            contactDetailsList1.Add(contactDetails1);
            contactDetailsList2.Add(contactDetails1);
          }
        }
      }
      if (((IEnumerable<string>) trigger.EmailRecipients).Contains<string>("NBO"))
      {
        List<NonBorrowingOwner> byBorrowerPairId = this.loanDataMgr.LoanData.GetNboByBorrowerPairId(this.loanDataMgr.LoanData.CurrentBorrowerPair.Id);
        int num = 0;
        foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
        {
          NonBorrowingOwner nbo = nonBorrowingOwner;
          if (contactDetailsList2 != null)
            existingContact = contactDetailsList2.Find((Predicate<ContactDetails>) (x => x.borrowerId == nbo.NBOID));
          string str3;
          if (existingContact == null || string.IsNullOrEmpty(existingContact.name))
            str3 = string.Join(" ", ((IEnumerable<string>) new string[4]
            {
              nbo.FirstName,
              nbo.MiddleName,
              nbo.LastName,
              nbo.SuffixName
            }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
          else
            str3 = existingContact.name;
          string userName = str3;
          ContactDetails contactForNotification = this.GetContactForNotification(existingContact, userName, nbo.EmailAddress, nbo.NBOID, eDeliveryEntityType.NonBorrowingOwner.ToString("g"));
          if (existingContact == null && contactForNotification != null && !string.IsNullOrEmpty(contactForNotification.name) && !string.IsNullOrEmpty(contactForNotification.email))
          {
            contactDetailsList2.Add(contactForNotification);
            contactDetailsList1.Add(contactForNotification);
          }
          ++num;
        }
      }
      GetRecipientURLRequest request = new GetRecipientURLRequest();
      request.loanId = this.loanDataMgr.LoanData.GetField("GUID").Replace("{", string.Empty).Replace("}", string.Empty);
      request.siteId = !string.IsNullOrEmpty(this.loanDataMgr.LoanData.GetField("ConsumerConnectSiteID")) ? this.loanDataMgr.LoanData.GetField("ConsumerConnectSiteID") : "1234567890";
      List<Contact> contactList = new List<Contact>();
      foreach (ContactDetails contactDetails2 in contactDetailsList2)
        contactList.Add(new Contact()
        {
          contactType = contactDetails2.contactType,
          name = contactDetails2.name,
          email = contactDetails2.email,
          authCode = contactDetails2.authCode,
          authType = contactDetails2.authType,
          borrowerId = contactDetails2.borrowerId,
          recipientId = contactDetails2.recipientId == null ? Guid.NewGuid().ToString() : contactDetails2.recipientId
        });
      request.contacts = contactList.ToArray();
      Task<GetRecipientURLResponse> recipientUrl = ebsServiceClient.GetRecipientURL(request);
      Task.WaitAll((Task) recipientUrl);
      GetRecipientURLResponse result = recipientUrl.Result;
      foreach (ContactDetails contactDetails3 in contactDetailsList1)
      {
        if (string.IsNullOrEmpty(userid))
          StatusOnlineManager.GetTriggerSender(this.loanDataMgr, trigger.EmailFromType, trigger.OwnerID, out userid, out email, out name);
        string companySetting = Session.ConfigurationManager.GetCompanySetting("DefaultCCEmailTemplates", EmailManager.EmailType.AuthenticationCode.ToString());
        string authCodeEmailBody = this.getLOAuthCodeEmailBody(trigger, companySetting, userid, contactDetails3.name, contactDetails3.authCode);
        RecipientInfo recipientInfo = this.CreateRecipientInfo(this.packageGUID, trigger.Guid, email, fromName, str2, trigger.Name, authCodeEmailBody, true);
        recipientInfo.subject = this.loanDataMgr.LoanData.GetSimpleField("4002") + " loan: Authentication Code for Automated Status Online Message";
        if (this.ValidateRecipientInfo(recipientInfo) == string.Empty)
        {
          sendNotificationRequest notificationRequest = this.getSendNotificationRequest(recipientInfo.subject, userid, name, email, str2, HtmlFieldMerge.MergeDynamicConsumerConnectContent(authCodeEmailBody, string.Empty, contactDetails3.name));
          do
          {
            str1 = string.Empty;
            try
            {
              Task.WaitAll(ebsServiceClient.SendNotification(notificationRequest));
              break;
            }
            catch (SoapException ex)
            {
              MetricsFactory.IncrementErrorCounter((Exception) ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClientCC.cs", nameof (sendNotifications), 607);
              str1 = ex.Detail.InnerText;
            }
            catch (Exception ex)
            {
              MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClientCC.cs", nameof (sendNotifications), 612);
              str1 = ex.Message;
            }
            if (str1 != string.Empty)
            {
              DialogResult dialogResult = Utils.Dialog((IWin32Window) null, "The following error occurred when trying to send the loan status email:\n\n" + str1 + ". Do you wish to retry sending the email?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
              if (dialogResult == DialogResult.No)
                flag = false;
              if (dialogResult == DialogResult.Yes)
                flag = true;
            }
          }
          while (flag);
          if (str1 == string.Empty)
            this.createLogEntry(recipientInfo);
        }
      }
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, trigger.EmailFromType, trigger.OwnerID, out userid, out email, out name);
      string str4 = this.getTriggerEmailBody(trigger, userid);
      foreach (BorrowerPair borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
      {
        this.loanDataMgr.LoanData.SetBorrowerPair(borrowerPair);
        foreach (string emailRecipient in trigger.EmailRecipients)
        {
          existingContact = (ContactDetails) null;
          string field = this.loanDataMgr.LoanData.GetField(emailRecipient);
          if (!string.IsNullOrEmpty(field))
          {
            switch (emailRecipient)
            {
              case "1240":
                if (contactDetailsList2 != null)
                {
                  existingContact = contactDetailsList2.Find((Predicate<ContactDetails>) (x => x.borrowerId == this.loanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id));
                  break;
                }
                break;
              case "1268":
                if (contactDetailsList2 != null)
                {
                  existingContact = contactDetailsList2.Find((Predicate<ContactDetails>) (x => x.borrowerId == this.loanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id));
                  break;
                }
                break;
              case "VEND.X141":
                if (contactDetailsList2 != null)
                {
                  existingContact = contactDetailsList2.Find((Predicate<ContactDetails>) (x => x.contactType.ToUpper() == "Buyer's Agent".ToUpper()));
                  break;
                }
                break;
            }
            if (existingContact != null)
            {
              HtmlEmailTemplate htmlEmailTemplate = Session.ConfigurationManager.GetHtmlEmailTemplate(trigger.EmailTemplateOwner, trigger.EmailTemplate);
              RecipientInfo recipientInfo = this.CreateRecipientInfo(this.packageGUID, trigger.Guid, email, name, field, htmlEmailTemplate.Subject, str4, true);
              str4 = this.getTriggerEmailBody(trigger, userid);
              string str5 = this.ValidateRecipientInfo(recipientInfo);
              Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Validate Recipient result: " + str5);
              if (str5 == string.Empty)
              {
                Laturl laturl = ((IEnumerable<Laturl>) result.latUrls).ToList<Laturl>().Find((Predicate<Laturl>) (x => x.recipientId == existingContact.recipientId));
                if (laturl != null)
                  str4 = HtmlFieldMerge.MergeDynamicConsumerConnectContent(str4, laturl.url, existingContact.name);
                sendNotificationRequest notificationRequest = this.getSendNotificationRequest(recipientInfo.subject, userid, name, email, field, str4);
                do
                {
                  str1 = string.Empty;
                  try
                  {
                    Task.WaitAll(ebsServiceClient.SendNotification(notificationRequest));
                    break;
                  }
                  catch (SoapException ex)
                  {
                    MetricsFactory.IncrementErrorCounter((Exception) ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClientCC.cs", nameof (sendNotifications), 710);
                    str1 = ex.Detail.InnerText;
                  }
                  catch (Exception ex)
                  {
                    MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClientCC.cs", nameof (sendNotifications), 715);
                    str1 = ex.Message;
                  }
                  if (str1 != string.Empty)
                  {
                    DialogResult dialogResult = Utils.Dialog((IWin32Window) null, "The following error occurred when trying to send the loan status email:\n\n" + str1 + ". Do you wish to retry sending the email?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                    if (dialogResult == DialogResult.No)
                      flag = false;
                    if (dialogResult == DialogResult.Yes)
                      flag = true;
                  }
                }
                while (flag);
                if (str1 == string.Empty)
                  this.createLogEntry(recipientInfo);
              }
            }
          }
        }
        if (((IEnumerable<string>) trigger.EmailRecipients).Contains<string>("NBO"))
        {
          List<NonBorrowingOwner> byBorrowerPairId = this.loanDataMgr.LoanData.GetNboByBorrowerPairId(this.loanDataMgr.LoanData.CurrentBorrowerPair.Id);
          int num = 0;
          foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
          {
            NonBorrowingOwner nbo = nonBorrowingOwner;
            if (contactDetailsList2 != null)
              existingContact = contactDetailsList2.Find((Predicate<ContactDetails>) (x => x.borrowerId == nbo.NBOID));
            if (existingContact != null)
            {
              RecipientInfo recipientInfo = this.CreateRecipientInfo(this.packageGUID, trigger.Guid, email, name, existingContact.email, trigger.Name, str4, true);
              str4 = this.getTriggerEmailBody(trigger, userid);
              string str6 = this.ValidateRecipientInfo(recipientInfo);
              Tracing.Log(StatusOnlineClientCC.sw, TraceLevel.Verbose, "StatusOnlineClient", "Validate Recipient result: " + str6);
              if (str6 == string.Empty)
              {
                Laturl laturl = ((IEnumerable<Laturl>) result.latUrls).ToList<Laturl>().Find((Predicate<Laturl>) (x => x.recipientId == existingContact.recipientId));
                if (laturl != null)
                  str4 = HtmlFieldMerge.MergeDynamicConsumerConnectContent(str4, laturl.url, existingContact.name);
                sendNotificationRequest notificationRequest = this.getSendNotificationRequest(recipientInfo.subject, userid, name, email, existingContact.email, str4);
                do
                {
                  str1 = string.Empty;
                  try
                  {
                    Task.WaitAll(ebsServiceClient.SendNotification(notificationRequest));
                    break;
                  }
                  catch (SoapException ex)
                  {
                    MetricsFactory.IncrementErrorCounter((Exception) ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClientCC.cs", nameof (sendNotifications), 790);
                    str1 = ex.Detail.InnerText;
                  }
                  catch (Exception ex)
                  {
                    MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\StatusOnline\\StatusOnlineClientCC.cs", nameof (sendNotifications), 795);
                    str1 = ex.Message;
                  }
                  if (str1 != string.Empty)
                  {
                    DialogResult dialogResult = Utils.Dialog((IWin32Window) null, "The following error occurred when trying to send the loan status email:\n\n" + str1 + ". Do you wish to retry sending the email?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                    if (dialogResult == DialogResult.No)
                      flag = false;
                    if (dialogResult == DialogResult.Yes)
                      flag = true;
                  }
                }
                while (flag);
                if (str1 == string.Empty)
                  this.createLogEntry(recipientInfo);
              }
            }
            ++num;
          }
        }
      }
      return str1;
    }

    private ContactDetails GetContactForNotification(
      ContactDetails existingContact,
      string userName,
      string userEmail,
      string borrowerId,
      string contactType = "Borrower")
    {
      ContactDetails contactForNotification = existingContact != null ? existingContact : new ContactDetails();
      contactForNotification.name = userName;
      contactForNotification.email = userEmail;
      if (string.IsNullOrEmpty(contactForNotification.authCode))
      {
        string str = new Random(DateTime.Now.Millisecond).Next(1, int.MaxValue).ToString();
        contactForNotification.authType = "AuthCode";
        contactForNotification.authCode = str;
      }
      if (existingContact == null)
      {
        contactForNotification.recipientId = Guid.NewGuid().ToString();
        contactForNotification.contactType = contactType;
        contactForNotification.borrowerId = borrowerId;
      }
      return contactForNotification;
    }

    protected sendNotificationRequest getSendNotificationRequest(
      string subject,
      string fromuserid,
      string fromName,
      string fromEmail,
      string toEmail,
      string htmlBody)
    {
      return new sendNotificationRequest()
      {
        loanGuid = new Guid(this.loanDataMgr.LoanData.GUID.Replace("{", string.Empty).Replace("}", string.Empty)),
        contentType = "HTML",
        subject = subject,
        createdBy = fromuserid,
        createdDate = DateTime.Now.ToString(),
        senderFullName = fromName,
        replyTo = fromEmail,
        emails = new string[1]{ toEmail },
        body = htmlBody
      };
    }

    private string getLOAuthCodeEmailBody(
      StatusOnlineTrigger trigger,
      string emailTemplateGUID,
      string fromuserid,
      string forUserName,
      string authCode)
    {
      string authCodeEmailBody = string.Empty;
      if (!string.IsNullOrEmpty(emailTemplateGUID))
      {
        HtmlEmailTemplate htmlEmailTemplate = Session.ConfigurationManager.GetHtmlEmailTemplate(trigger.EmailTemplateOwner, emailTemplateGUID);
        if (htmlEmailTemplate != (HtmlEmailTemplate) null)
        {
          UserInfo userInfo = trigger.EmailFromType != TriggerEmailFromType.CurrentUser ? Session.OrganizationManager.GetUser(fromuserid) : Session.UserInfo;
          authCodeEmailBody = new HtmlFieldMerge(htmlEmailTemplate.Html)
          {
            AuthenticationUser = forUserName,
            AuthenticationCode = authCode
          }.MergeContent(this.loanDataMgr.LoanData, userInfo);
        }
      }
      return authCodeEmailBody;
    }

    protected void createLogEntry(RecipientInfo recipient)
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
