// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.eDeliveryMessage
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.eDelivery.Model;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class eDeliveryMessage
  {
    private LoanDataMgr loanDataMgr;
    private eDeliveryMessageType msgType;
    private string to;
    private string cc;
    private string fromEmail;
    private string fromName;
    private string subject;
    private string body;
    private bool readReceipt;
    private List<eDeliveryAttachment> attachmentList;
    private List<DocumentLog> neededList;
    private List<eDeliveryRecipient> recipients;
    private bool electronicSignature;
    private EDisclosureSignOrderSetup signOrderSetup;
    private const string className = "NotifyUsersDialog";
    private static readonly string sw = Tracing.SwEFolder;
    public bool FulfillmentEnabled;
    public DateTime FulfillmentSchedDate = DateTime.MinValue;
    public DateTime NotificationDate = DateTime.MinValue;
    public string FulfillmentFromName = string.Empty;
    public string FulfillmentFromAddress = string.Empty;
    public string FulfillmentFromCity = string.Empty;
    public string FulfillmentFromState = string.Empty;
    public string FulfillmentFromZip = string.Empty;
    public string FulfillmentFromPhone = string.Empty;
    public string FulfillmentToName = string.Empty;
    public string FulfillmentToAddress = string.Empty;
    public string FulfillmentToCity = string.Empty;
    public string FulfillmentToState = string.Empty;
    public string FulfillmentToZip = string.Empty;
    public string FulfillmentToPhone = string.Empty;
    public string OrigintorEmailTemplate = "<html><head><title>Email - Encompass User - Electronic Loan Document Request</title><style>.header {color:#EB9103; font-weight:bold; font-style:italic; font-family:Arial; font-size: 12pt;}.normal {color:000; font-weight:normal; font-family:Arial; font-size:10pt;}.normalsm {color:000; font-weight:normal; font-family:Arial; font-size:7pt;}A:link {color:#0000FF;font-weight:normal;font-family:Arial;font-size:10pt;text-decoration: underline;}A:visited {color:#0000FF;font-weight:normal;font-family:Arial;font-size:10pt;text-decoration: underline;}A:active {color:#005595;font-weight:normal;font-family:Arial;font-size:10pt;text-decoration: underline;}A:hover {color: #EB9103;font-weight:normal;font-family:Arial;font-size:10pt;text-decoration: underline;}.linkSm {color:#005595;font-weight:normal;font-family:Arial;font-size:8pt;}A.linkSm:link {color:#005595;font-weight:normal;font-family:Arial;font-size:8pt;text-decoration: underline;}A.linkSm:visited {color:#005595;font-weight:normal;font-family:Arial;font-size:8pt;text-decoration: underline;}A.linkSm:active {color:#005595;font-weight:normal;font-family:Arial;font-size:8pt;text-decoration: underline;}A.linkSm:hover {color: #EB9103;font-weight:normal;font-family:Arial;font-size:8pt;text-decoration: underline;}.copyright {color:#999999;font-weight:normal;font-family:Arial;font-size:8pt;}A.copyright:link {color:#999999;font-weight:normal;font-family:Arial;font-size:8pt;text-decoration: underline;}A.copyright:visited {color:#999999;font-weight:normal;font-family:Arial;font-size:8pt;text-decoration: underline;}A.copyright:active {color:#999999;font-weight:normal;font-family:Arial;font-size:8pt;text-decoration: underline;}A.copyright:hover {color: #EB9103;font-weight:normal;font-family:Arial;font-size:8pt;text-decoration: underline;}</style></head><body topmargin=\"0\" leftmargin=\"0\" marginwidth=\"0\" marginheight=\"0\"><table width = \"600\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td width = \"14\"> &nbsp;</td><td width = \"586\"><br><div class=\"normal\">\t\t\t\t\t\t Borrower: <b>[[BorFullName]]</b><br>\t\t\t\t\t\t Property Address: <b>[[PropertyAddress]]</b><br>\t\t\t\t\t\t [[PackageType]] Sent Date/Time: <b>[[SentDate]]</b><br></div></td></tr><tr><td width = \"14\"> &nbsp;</td><td colspan = \"2\"><hr size=\"1\" noshade color = \"#999999\"></td></tr><tr><td width=\"14\">&nbsp;</td><td width = \"586\"><div class=\"normal\"><br>Dear [[User]],<br><br>This email has been sent to notify you that your signature is required on the package sent on [[SentDate]]. It is recommended that you sign the documents as soon as possible.The documents cannot be retrieved to the eFolder until both you and the borrowers sign them.<br/><br>If you have already completed signing, please ignore this email.</br><br>[[Documents]]\t\t\t\t\t\t<a href = \"[[URL]]\" target=\"_blank\">Click here to sign the documents</a>.<br/><br/>If you experience problems opening the link, copy and paste the URL below into your Web browser.<br/>URL: [[URL]]<br><br/>If you have any questions, please contact your system administrator.<br><br>Sincerely,<br>Encompass eFolder Team<br> Ellie Mae, Inc.</div></td></tr></table></body></html>";
    public readonly string PdfMetadata;

    public eDeliveryMessage(LoanDataMgr loanDataMgr, eDeliveryMessageType msgType)
    {
      this.loanDataMgr = loanDataMgr;
      this.msgType = msgType;
      this.FromEmail = Session.UserInfo.Email;
      this.signOrderSetup = Session.ConfigurationManager.GetEDisclosureSignOrderSetup();
      this.attachmentList = new List<eDeliveryAttachment>();
      this.neededList = new List<DocumentLog>();
      this.recipients = new List<eDeliveryRecipient>();
      try
      {
        this.PdfMetadata = ConfigurationManager.AppSettings["PDFJsonMetadata"].ToString();
      }
      catch (Exception ex)
      {
        this.PdfMetadata = "EMFormFields";
        Tracing.Log(eDeliveryMessage.sw, "NotifyUsersDialog", TraceLevel.Error, ex.ToString());
      }
    }

    public LoanDataMgr LoanDataMgr => this.loanDataMgr;

    public eDeliveryMessageType MsgType => this.msgType;

    public bool ElectronicSignature
    {
      get => this.electronicSignature;
      set => this.electronicSignature = value;
    }

    public string FromEmail
    {
      get => this.fromEmail;
      set => this.fromEmail = value;
    }

    public string FromName
    {
      get => this.fromName;
      set => this.fromName = value;
    }

    public string To
    {
      get => this.to;
      set => this.to = value;
    }

    public string CC
    {
      get => this.cc;
      set => this.cc = value;
    }

    public string Subject
    {
      get => this.subject;
      set => this.subject = value;
    }

    public string Body
    {
      get => this.body;
      set => this.body = value;
    }

    public bool ReadReceipt
    {
      get => this.readReceipt;
      set => this.readReceipt = value;
    }

    public eDeliveryAttachment[] Attachments => this.attachmentList.ToArray();

    public eDeliveryRecipient[] Recipients
    {
      get => this.recipients.ToArray();
      set
      {
      }
    }

    public void ClearRecipients() => this.recipients.Clear();

    public void AddRecipient(eDeliveryRecipient recipient)
    {
      if (this.recipients.Contains(recipient))
        return;
      this.recipients.Add(recipient);
    }

    public void ClearAttachments() => this.attachmentList.Clear();

    public void AddAttachment(eDeliveryAttachment attachment)
    {
      if (this.attachmentList.Contains(attachment))
        return;
      this.attachmentList.Add(attachment);
    }

    public eDeliveryAttachment AddCoversheet(string filepath)
    {
      eDeliveryAttachment attachment = new eDeliveryAttachment(filepath, "COVERSHEET");
      this.AddAttachment(attachment);
      return attachment;
    }

    public DocumentAttachment AddDocument(string filepath, DocumentLog doc)
    {
      DocumentAttachment attachment = new DocumentAttachment(filepath, doc);
      this.AddAttachment((eDeliveryAttachment) attachment);
      return attachment;
    }

    public SDTAttachment AddDocuments(string filepath, DocumentLog[] docList)
    {
      SDTAttachment attachment = new SDTAttachment(filepath, docList);
      this.AddAttachment((eDeliveryAttachment) attachment);
      return attachment;
    }

    public SFTAttachment AddForms(string filepath, FormItemInfo[] formItemList)
    {
      List<string> stringList = new List<string>();
      foreach (FormItemInfo formItem in formItemList)
        stringList.Add(formItem.GroupName);
      return this.AddForms(filepath, stringList.ToArray());
    }

    public SFTAttachment AddForms(string filepath, string[] formList)
    {
      SFTAttachment attachment = new SFTAttachment(filepath, formList);
      this.AddAttachment((eDeliveryAttachment) attachment);
      return attachment;
    }

    public void AddNeeded(DocumentLog[] docList)
    {
      foreach (DocumentLog doc in docList)
      {
        if (!this.neededList.Contains(doc))
          this.neededList.AddRange((IEnumerable<DocumentLog>) docList);
      }
    }

    public bool CheckRequirements()
    {
      string simpleField1 = this.loanDataMgr.LoanData.GetSimpleField("11");
      string simpleField2 = this.loanDataMgr.LoanData.GetSimpleField("FR0104");
      if (simpleField1.Contains(" ") || simpleField2.Contains(" "))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "In order to use this service, you need to type in the Borrower's Present Address or the Property Address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    public void CreateLogEntry()
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 362, nameof (CreateLogEntry), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryMessage.cs");
        HtmlEmailLog htmlEmailLog1 = new HtmlEmailLog(Session.UserInfo.FullName + " (" + Session.UserID + ")");
        PerformanceMeter.Current.AddCheckpoint("new HtmlEmailLog", 366, nameof (CreateLogEntry), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryMessage.cs");
        switch (this.msgType)
        {
          case eDeliveryMessageType.RequestDocuments:
            htmlEmailLog1.Description = "Documents requested";
            break;
          case eDeliveryMessageType.SendDocuments:
            htmlEmailLog1.Description = "Documents sent";
            break;
          case eDeliveryMessageType.SecureFormTransfer:
            htmlEmailLog1.Description = "Secure form transfer";
            break;
          default:
            htmlEmailLog1.Description = "Documents requested";
            break;
        }
        string str = this.readReceipt ? "Yes" : "No";
        htmlEmailLog1.Sender = this.FromEmail;
        htmlEmailLog1.Recipient = this.to;
        if (!string.IsNullOrEmpty(this.cc))
        {
          HtmlEmailLog htmlEmailLog2 = htmlEmailLog1;
          htmlEmailLog2.Recipient = htmlEmailLog2.Recipient + "; " + this.cc;
        }
        htmlEmailLog1.Subject = this.subject;
        htmlEmailLog1.Body = this.body;
        htmlEmailLog1.ReadReceipt = this.readReceipt;
        List<string> stringList = new List<string>();
        foreach (eDeliveryAttachment attachment in this.attachmentList)
        {
          switch (attachment)
          {
            case DocumentAttachment _:
              using (PerformanceMeter.Current.BeginOperation("eDeliveryMessage.CreateLogEntry.DocumentAttachment"))
              {
                DocumentAttachment documentAttachment = (DocumentAttachment) attachment;
                stringList.Add(documentAttachment.Document.Title);
                string details = (string) null;
                switch (this.msgType)
                {
                  case eDeliveryMessageType.RequestDocuments:
                    details = "Doc request sent";
                    break;
                  case eDeliveryMessageType.SendDocuments:
                    details = "Doc sent";
                    break;
                }
                if (details != null)
                {
                  this.loanDataMgr.LoanHistory.TrackChange((LogRecordBase) documentAttachment.Document, details, (LogRecordBase) htmlEmailLog1);
                  continue;
                }
                continue;
              }
            case SDTAttachment _:
              using (PerformanceMeter.Current.BeginOperation("eDeliveryMessage.CreateLogEntry.SDTAttachment"))
              {
                foreach (DocumentLog document in ((SDTAttachment) attachment).Documents)
                {
                  stringList.Add(document.Title);
                  string details = (string) null;
                  switch (this.msgType)
                  {
                    case eDeliveryMessageType.RequestDocuments:
                      details = "Doc request sent";
                      break;
                    case eDeliveryMessageType.SendDocuments:
                      details = "Doc sent";
                      break;
                  }
                  if (details != null)
                    this.loanDataMgr.LoanHistory.TrackChange((LogRecordBase) document, details, (LogRecordBase) htmlEmailLog1);
                }
                continue;
              }
            case SFTAttachment _:
              using (PerformanceMeter.Current.BeginOperation("eDeliveryMessage.CreateLogEntry.SFTAttachment"))
              {
                SFTAttachment sftAttachment = (SFTAttachment) attachment;
                stringList.AddRange((IEnumerable<string>) sftAttachment.Forms);
                continue;
              }
            default:
              continue;
          }
        }
        foreach (DocumentLog needed in this.neededList)
        {
          stringList.Add(needed.Title);
          this.loanDataMgr.LoanHistory.TrackChange((LogRecordBase) needed, "Doc request sent", (LogRecordBase) htmlEmailLog1);
        }
        htmlEmailLog1.Documents = stringList.ToArray();
        this.loanDataMgr.AddOperationLog((LogRecordBase) htmlEmailLog1);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 476, nameof (CreateLogEntry), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryMessage.cs");
      }
    }

    public Request CreateRequest()
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 487, nameof (CreateRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryMessage.cs");
        LoanData loanData = this.loanDataMgr.LoanData;
        CompanyInfo companyInfo = Session.CompanyInfo;
        UserInfo userInfo = Session.UserInfo;
        Request requestObject = this.GetRequestObject();
        Caller caller = new Caller();
        caller.name = "Encompass";
        caller.version = VersionInformation.CurrentVersion.DisplayVersion;
        caller.clientId = companyInfo.ClientID;
        requestObject.caller = caller;
        requestObject.applicationGroupId = this.LoanDataMgr.LoanData.GUID.Replace("{", string.Empty).Replace("}", string.Empty);
        requestObject.consumerConnectSiteID = !string.IsNullOrEmpty(this.LoanDataMgr.LoanData.GetField("ConsumerConnectSiteID")) ? this.LoanDataMgr.LoanData.GetField("ConsumerConnectSiteID") : "1234567890";
        requestObject.from = new User();
        requestObject.from.email = string.IsNullOrEmpty(this.FromEmail) ? this.FromEmail : this.FromEmail.Trim();
        requestObject.from.name = this.FromName;
        requestObject.notifyWhenViewed = this.ReadReceipt;
        requestObject.custom = new Dictionary<string, string>();
        requestObject.custom.Add("packageGroupNumber", this.LoanDataMgr.LoanData.LoanNumber);
        requestObject.custom.Add("clientId", caller.clientId);
        requestObject.custom.Add("SynchronizationId", Guid.NewGuid().ToString());
        requestObject.custom.Add("primaryRecipientName", this.LoanDataMgr.LoanData.CurrentBorrowerPair.Borrower.FirstName + " " + this.LoanDataMgr.LoanData.CurrentBorrowerPair.Borrower.LastName);
        if (this.Recipients.Length != 0)
          requestObject.recipients = new List<Recipient>();
        string empty = string.Empty;
        PerformanceMeter.Current.AddCheckpoint("BEFORE Recipients loop, Count: " + (object) this.recipients.Count, 522, nameof (CreateRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryMessage.cs");
        foreach (eDeliveryRecipient recipient in this.recipients)
        {
          Recipient recipientObject = this.GetRecipientObject();
          requestObject.recipients.Add(recipientObject);
          this.SetOTPRecipientDetails(recipientObject, recipient);
          recipientObject.borrowerId = recipient.BorrowerId;
          recipientObject.recipientId = recipient.RecipientId;
          recipientObject.nboIndex = recipient.NboIndex == 0 ? (string) null : recipient.NboIndex.ToString();
          if (recipient.EntityType == eDeliveryEntityType.Originator)
          {
            recipientObject.sendESignatureNotification = "Encompass";
            recipientObject.userId = recipient.UserID;
          }
          recipientObject.createdEmail = new Email();
          recipientObject.createdEmail.content = this.Body;
          recipientObject.role = recipient.RecipientType;
          recipientObject.fullName = !string.IsNullOrEmpty(recipient.UnparsedName) ? recipient.UnparsedName : recipient.FirstName + " " + recipient.LastName;
          recipientObject.createdEmail.subject = recipientObject.fullName + ": " + this.Subject;
          recipientObject.email = string.IsNullOrEmpty(recipient.EmailAddress) ? recipient.EmailAddress : recipient.EmailAddress.Trim();
          recipientObject.routingOrder = 1;
          if (this.recipients.Any<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.EntityType == eDeliveryEntityType.Originator)) && this.signOrderSetup != null && this.signOrderSetup.States.Count > 0 && this.MsgType == eDeliveryMessageType.InitialDisclosures)
          {
            bool boolean = Convert.ToBoolean(this.signOrderSetup.SignOrderEnabled);
            bool flag = false;
            this.signOrderSetup.States.TryGetValue(this.LoanDataMgr.LoanData.GetField("14").ToUpper(), out flag);
            if (boolean & flag)
              recipientObject.routingOrder = recipient.EntityType != eDeliveryEntityType.Originator ? 2 : 1;
          }
          recipientObject.authCode = recipient.AuthCode;
          int documentCounter = 0;
          List<Signingpoint> signingpointList = new List<Signingpoint>();
          foreach (eDeliveryAttachment attachment in this.attachmentList.FindAll((Predicate<eDeliveryAttachment>) (x => x.Title != "COVERSHEET")))
          {
            using (PerformanceMeter.Current.BeginOperation("eDeliveryMessage.CreateRequest.AttachmentLoop"))
            {
              if (recipientObject.documents == null)
                recipientObject.documents = new List<Signingpoint>();
              Signingpoint signingpoint = new Signingpoint();
              signingpoint.id = attachment.ID;
              if (recipient.EntityType != eDeliveryEntityType.Originator || recipient.EntityType == eDeliveryEntityType.Originator && this.IsEsignableDocument(attachment))
                recipientObject.documents.Add(signingpoint);
              if (attachment is DocumentAttachment)
              {
                if (this.electronicSignature)
                {
                  DocumentAttachment docAttachment = (DocumentAttachment) attachment;
                  if (docAttachment.SignatureType == "eSignable")
                  {
                    int height;
                    try
                    {
                      if (docAttachment.PdfReader == null)
                        docAttachment.PdfReader = new PdfReader(attachment.Filepath);
                      height = (int) docAttachment.PdfReader.GetPageSize(docAttachment.PageCount).Height;
                    }
                    catch (Exception ex)
                    {
                      Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in reading PDF data : " + ex.ToString());
                      PerformanceMeter.Current.AddCheckpoint("EXIT Null Exception reading PDF data", 609, nameof (CreateRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryMessage.cs");
                      return (Request) null;
                    }
                    ++documentCounter;
                    if (docAttachment.PdfReader.Info != null && docAttachment.PdfReader.Info.ContainsKey(this.PdfMetadata))
                    {
                      Signingpoint jsonMetadata = this.ParseJsonMetadata(docAttachment.PdfReader.Info[this.PdfMetadata].ToString(), signingpoint, recipientObject, documentCounter, height);
                      if (jsonMetadata.fields == null)
                        signingpointList.Add(jsonMetadata);
                    }
                    else
                    {
                      Signingpoint pointFromAttachment = this.GetSigningPointFromAttachment(docAttachment, recipient, signingpoint, height);
                      if (pointFromAttachment.fields == null)
                        signingpointList.Add(pointFromAttachment);
                    }
                  }
                }
              }
            }
          }
          if (signingpointList.Count > 0)
          {
            foreach (Signingpoint signingpoint in signingpointList)
            {
              if (recipientObject.documents.Contains(signingpoint))
                recipientObject.documents.Remove(signingpoint);
            }
          }
          if (this.neededList.Count > 0)
          {
            if (requestObject.documents == null)
              requestObject.documents = new List<Document>();
            if (recipientObject.documents == null)
              recipientObject.documents = new List<Signingpoint>();
            foreach (DocumentLog needed in this.neededList)
            {
              Document document = new Document();
              document.id = needed.Guid;
              document.title = needed.Title;
              document.type = "Needed";
              if (requestObject.documents.All<Document>((Func<Document, bool>) (d => d.id != document.id)))
                requestObject.documents.Add(document);
              if (recipient.EntityType != eDeliveryEntityType.Originator)
                recipientObject.documents.Add(new Signingpoint()
                {
                  id = document.id
                });
            }
          }
        }
        PerformanceMeter.Current.AddCheckpoint("AFTER Recipients loop, Count: " + (object) this.recipients.Count, 672, nameof (CreateRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryMessage.cs");
        DateTime dateTime;
        if (this.FulfillmentEnabled)
        {
          requestObject.fulfillment = new Fulfillment();
          Fulfillment fulfillment = requestObject.fulfillment;
          dateTime = this.FulfillmentSchedDate.ToUniversalTime();
          string str = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
          fulfillment.scheduledDate = str;
          requestObject.fulfillment.from = new User();
          requestObject.fulfillment.from.name = this.FulfillmentFromName;
          requestObject.fulfillment.from.street = this.FulfillmentFromAddress;
          requestObject.fulfillment.from.city = this.FulfillmentFromCity;
          requestObject.fulfillment.from.state = this.FulfillmentFromState;
          requestObject.fulfillment.from.zipCode = this.FulfillmentFromZip;
          requestObject.fulfillment.from.phone = this.FulfillmentFromPhone;
          requestObject.fulfillment.to = new User();
          requestObject.fulfillment.to.name = this.FulfillmentToName;
          requestObject.fulfillment.to.street = this.FulfillmentToAddress;
          requestObject.fulfillment.to.city = this.FulfillmentToCity;
          requestObject.fulfillment.to.state = this.FulfillmentToState;
          requestObject.fulfillment.to.zipCode = this.FulfillmentToZip;
          requestObject.fulfillment.to.phone = this.FulfillmentToPhone;
        }
        DateTime date1 = this.NotificationDate.Date;
        dateTime = DateTime.MinValue;
        DateTime date2 = dateTime.Date;
        if (date1 != date2)
        {
          requestObject.notViewed = new NotViewed();
          NotViewed notViewed = requestObject.notViewed;
          dateTime = this.NotificationDate.ToUniversalTime();
          string str = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
          notViewed.date = str;
        }
        if (this.attachmentList.Count > 0 && requestObject.documents == null)
          requestObject.documents = new List<Document>();
        foreach (eDeliveryAttachment attachment in this.attachmentList)
        {
          switch (attachment)
          {
            case DocumentAttachment _:
              using (PerformanceMeter.Current.BeginOperation("eDeliveryMessage.CreateRequest.DocumentAttachment"))
              {
                DocumentAttachment documentAttachment = (DocumentAttachment) attachment;
                Document document = new Document();
                document.id = documentAttachment.ID;
                document.title = documentAttachment.Title;
                document.pages = documentAttachment.PageCount;
                document.size = Convert.ToInt32(documentAttachment.Filesize);
                document.filePath = attachment.Filepath;
                switch (documentAttachment.SignatureType)
                {
                  case "eSignable":
                    document.type = this.electronicSignature ? "ESign" : "WetSign";
                    break;
                  case "Wet Sign Only":
                    document.type = "WetSign";
                    break;
                  case "Informational":
                    document.type = "Information";
                    break;
                  default:
                    document.type = "Information";
                    break;
                }
                requestObject.documents.Add(document);
                continue;
              }
            case SDTAttachment _:
              using (PerformanceMeter.Current.BeginOperation("eDeliveryMessage.CreateRequest.SDTAttachment"))
              {
                requestObject.documents.Add(new Document()
                {
                  id = attachment.ID,
                  title = attachment.Title,
                  pages = new PdfReader(attachment.Filepath).NumberOfPages,
                  size = Convert.ToInt32(attachment.Filesize),
                  filePath = attachment.Filepath,
                  type = "Information"
                });
                continue;
              }
            case SFTAttachment _:
              using (PerformanceMeter.Current.BeginOperation("eDeliveryMessage.CreateRequest.SFTAttachment"))
              {
                requestObject.documents.Add(new Document()
                {
                  id = attachment.ID,
                  title = attachment.Title,
                  pages = new PdfReader(attachment.Filepath).NumberOfPages,
                  size = Convert.ToInt32(attachment.Filesize),
                  filePath = attachment.Filepath,
                  type = "Information"
                });
                continue;
              }
            default:
              if (attachment.Title == "COVERSHEET")
              {
                using (PerformanceMeter.Current.BeginOperation("eDeliveryMessage.CreateRequest.SFTAttachment"))
                {
                  requestObject.documents.Add(new Document()
                  {
                    id = attachment.ID,
                    title = attachment.Title,
                    pages = 1,
                    size = Convert.ToInt32(attachment.Filesize),
                    type = "CoverSheet",
                    filePath = attachment.Filepath
                  });
                  continue;
                }
              }
              else
                continue;
          }
        }
        return requestObject;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 798, nameof (CreateRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\eDeliveryMessage.cs");
      }
    }

    public Signingpoint ParseJsonMetadata(
      string PDFJsonMetadataValue,
      Signingpoint signingpoint,
      Recipient recipient,
      int documentCounter,
      int pageHeight)
    {
      IEnumerable<JToken> jtokens1;
      try
      {
        JObject jobject = JObject.Parse(PDFJsonMetadataValue);
        if (jobject["signers"] == null)
        {
          Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Could not found signers node in Json data.");
          return (Signingpoint) null;
        }
        jtokens1 = jobject["signers"].Children().AsEnumerable<JToken>();
      }
      catch (JsonReaderException ex)
      {
        Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data : " + ex.ToString());
        return (Signingpoint) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data : " + ex.ToString());
        return (Signingpoint) null;
      }
      List<Recipient> recipientList = new List<Recipient>();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (JToken jtoken1 in jtokens1)
      {
        Recipient recipient1 = new Recipient();
        recipient1.role = jtoken1[(object) "type"].ToString();
        if (string.Compare(recipient1.role, recipient.role, true) == 0 && (recipient1.role != "nonborrowingowner" || recipient.borrowerId == jtoken1[(object) "id"].ToString()))
        {
          IEnumerable<JToken> jtokens2 = (IEnumerable<JToken>) new List<JToken>();
          IEnumerable<JToken> jtokens3 = jtoken1[(object) "fields"].Children().AsEnumerable<JToken>();
          try
          {
            if (signingpoint.fields == null)
              signingpoint.fields = new List<SignatureField>();
            foreach (JToken jtoken2 in jtokens3)
            {
              SignatureField signatureField = jtoken2.ToObject<SignatureField>();
              if (signatureField == null)
              {
                Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Error in converting JSON signingField. JSON object : " + jtoken2.ToString());
              }
              else
              {
                signatureField.Name = string.Join("_", ((IEnumerable<string>) new string[2]
                {
                  signatureField.Name,
                  recipient.nboIndex
                }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
                if (signatureField.Id == null)
                {
                  signatureField.Id = Guid.NewGuid().ToString() + "_" + documentCounter.ToString();
                  dictionary.Add(signatureField.Id, signatureField.Id);
                }
                else
                {
                  dictionary.Add(signatureField.Id, signatureField.Id + "_" + documentCounter.ToString());
                  signatureField.Id = dictionary[signatureField.Id];
                }
                SignatureFieldType? type = signatureField.Type;
                SignatureFieldType signatureFieldType1 = SignatureFieldType.CheckGroup;
                if (!(type.GetValueOrDefault() == signatureFieldType1 & type.HasValue))
                {
                  type = signatureField.Type;
                  SignatureFieldType signatureFieldType2 = SignatureFieldType.RadioGroup;
                  if (!(type.GetValueOrDefault() == signatureFieldType2 & type.HasValue))
                    goto label_29;
                }
                if (signatureField.ItemFields != null)
                {
                  foreach (SignatureFieldItem itemField in signatureField.ItemFields)
                  {
                    if (itemField.Id == null)
                    {
                      itemField.Id = Guid.NewGuid().ToString() + "_" + documentCounter.ToString();
                      dictionary.Add(itemField.Id, itemField.Id);
                    }
                    else
                    {
                      dictionary.Add(itemField.Id, itemField.Id + "_" + documentCounter.ToString());
                      itemField.Id = dictionary[itemField.Id];
                    }
                  }
                }
label_29:
                signingpoint.fields.Add(signatureField);
              }
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data for SigningField : " + ex.ToString());
            return (Signingpoint) null;
          }
        }
      }
      if (signingpoint.fields != null)
      {
        foreach (SignatureField signatureField in signingpoint.fields.Where<SignatureField>((Func<SignatureField, bool>) (f => f.ConditionalField != null)))
        {
          if (signatureField.ConditionalField.Id != null)
            signatureField.ConditionalField.Id = dictionary.ContainsKey(signatureField.ConditionalField.Id) ? dictionary[signatureField.ConditionalField.Id] : Guid.NewGuid().ToString() + "_" + documentCounter.ToString();
        }
      }
      return signingpoint;
    }

    public bool checkSigningpointExists(DocumentAttachment docAttachment, string recipientType)
    {
      if (docAttachment.PdfReader == null)
        docAttachment.PdfReader = new PdfReader(docAttachment.Filepath);
      if (docAttachment.PdfReader.Info == null || !docAttachment.PdfReader.Info.ContainsKey(this.PdfMetadata))
        return false;
      string json = docAttachment.PdfReader.Info[this.PdfMetadata].ToString();
      try
      {
        JObject jobject = JObject.Parse(json);
        if (jobject["signers"] == null)
        {
          Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Could not found signers node in Json data.");
          return false;
        }
        return jobject["signers"].Children().AsEnumerable<JToken>().Any<JToken>((Func<JToken, bool>) (x => x[(object) "type"].ToString().ToLower() == recipientType.ToLower()));
      }
      catch (JsonReaderException ex)
      {
        Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data : " + ex.ToString());
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data : " + ex.ToString());
        return false;
      }
    }

    public bool checkSigningpointExists(EllieMae.EMLite.eFolder.LoanCenter.DocumentAttachment docAttachment, string recipientType)
    {
      if (docAttachment.PdfReader == null)
        docAttachment.PdfReader = new PdfReader(docAttachment.Filepath);
      if (docAttachment.PdfReader.Info == null || !docAttachment.PdfReader.Info.ContainsKey(this.PdfMetadata))
        return false;
      string json = docAttachment.PdfReader.Info[this.PdfMetadata].ToString();
      try
      {
        JObject jobject = JObject.Parse(json);
        if (jobject["signers"] == null)
        {
          Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Could not found signers node in Json data.");
          return false;
        }
        return jobject["signers"].Children().AsEnumerable<JToken>().Any<JToken>((Func<JToken, bool>) (x => x[(object) "type"].ToString().ToLower() == recipientType.ToLower()));
      }
      catch (JsonReaderException ex)
      {
        Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data : " + ex.ToString());
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data : " + ex.ToString());
        return false;
      }
    }

    public List<string> GetEncompassContactsFromPDFJsonMetadata(string PDFJsonMetadataValue)
    {
      IEnumerable<JToken> jtokens;
      try
      {
        JObject jobject = JObject.Parse(PDFJsonMetadataValue);
        if (jobject["signers"] == null)
        {
          Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Could not found signers node in Json data.");
          return (List<string>) null;
        }
        jtokens = jobject["signers"].Children().AsEnumerable<JToken>();
      }
      catch (JsonReaderException ex)
      {
        Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data : " + ex.ToString());
        return (List<string>) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(eDeliveryMessage.sw, TraceLevel.Error, "NotifyUsersDialog", "Exception in parsing Json data : " + ex.ToString());
        return (List<string>) null;
      }
      List<string> fromPdfJsonMetadata = new List<string>();
      foreach (JToken jtoken in jtokens)
      {
        if (jtoken[(object) "type"].ToString().Contains("nonborrowingowner"))
        {
          string str = jtoken[(object) "id"].ToString();
          if (string.IsNullOrEmpty(str))
            throw new Exception("Exception in parsing json data: Non borrowing owner id is empty or null");
          fromPdfJsonMetadata.Add(str);
        }
      }
      return fromPdfJsonMetadata;
    }

    public Signingpoint GetSigningPointFromAttachment(
      DocumentAttachment docAttachment,
      eDeliveryRecipient eDeliveryrecipient,
      Signingpoint signingpoint,
      int pageHeight)
    {
      bool flag = false;
      AcroFields.Item fieldItem = docAttachment.PdfReader.AcroFields.GetFieldItem(eDeliveryrecipient.SignatureField);
      TextField tx = new TextField((PdfWriter) null, (Rectangle) null, (string) null);
      if (fieldItem != null)
      {
        PdfDictionary merged = fieldItem.GetMerged(0);
        docAttachment.PdfReader.AcroFields.DecodeGenericDictionary(merged, (BaseField) tx);
        flag = tx.Font != null;
      }
      new Signingpoint().fields = new List<SignatureField>();
      int? nullable;
      if (docAttachment.SignatureFields != null && eDeliveryrecipient.SignatureField != null && Array.IndexOf<string>(docAttachment.SignatureFields, eDeliveryrecipient.SignatureField) >= 0 && docAttachment.PdfReader.AcroFields.Fields.ContainsKey(eDeliveryrecipient.SignatureField))
      {
        for (int index = 0; index < docAttachment.PdfReader.AcroFields.Fields[eDeliveryrecipient.SignatureField].Size; ++index)
        {
          SignatureField field = new SignatureField();
          field.Name = eDeliveryrecipient.SignatureField;
          field.Type = new SignatureFieldType?(SignatureFieldType.SignAndDate);
          IList<AcroFields.FieldPosition> fieldPositions = docAttachment.PdfReader.AcroFields.GetFieldPositions(eDeliveryrecipient.SignatureField);
          if (fieldPositions != null && fieldPositions.Count > 0)
            field = this.GetFormFieldSingingDataInformation(field, eDeliveryrecipient.SignatureField, fieldPositions.ElementAt<AcroFields.FieldPosition>(index), pageHeight);
          if (signingpoint.fields == null)
            signingpoint.fields = new List<SignatureField>();
          signingpoint.fields.Add(field);
          if (flag)
          {
            Array array = tx.Font.FullFontName.GetValue(0) as Array;
            if ((int) tx.FontSize == 2)
            {
              field.Scale = new Decimal?(0.5M);
              SignatureField signatureField1 = field;
              nullable = signatureField1.Left;
              signatureField1.Left = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 2) : new int?();
              if (field.Name.StartsWith("CoborrowerSignature_"))
              {
                SignatureField signatureField2 = field;
                nullable = signatureField2.Bottom;
                signatureField2.Bottom = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 9) : new int?();
                SignatureField signatureField3 = field;
                nullable = signatureField3.Top;
                signatureField3.Top = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 9) : new int?();
                SignatureField signatureField4 = field;
                nullable = signatureField4.Right;
                signatureField4.Right = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 78) : new int?();
              }
              else
              {
                SignatureField signatureField5 = field;
                nullable = signatureField5.Bottom;
                signatureField5.Bottom = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 12) : new int?();
                SignatureField signatureField6 = field;
                nullable = signatureField6.Top;
                signatureField6.Top = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 13) : new int?();
                SignatureField signatureField7 = field;
                nullable = signatureField7.Right;
                signatureField7.Right = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 78) : new int?();
              }
            }
          }
        }
      }
      if (docAttachment.SignatureFields != null && eDeliveryrecipient.InitialsField != null && Array.IndexOf<string>(docAttachment.SignatureFields, eDeliveryrecipient.InitialsField) >= 0)
      {
        for (int index = 0; index < docAttachment.PdfReader.AcroFields.Fields[eDeliveryrecipient.InitialsField].Size; ++index)
        {
          SignatureField field = new SignatureField();
          field.Name = eDeliveryrecipient.InitialsField;
          field.Type = new SignatureFieldType?(SignatureFieldType.Initials);
          IList<AcroFields.FieldPosition> fieldPositions = docAttachment.PdfReader.AcroFields.GetFieldPositions(eDeliveryrecipient.InitialsField);
          if (fieldPositions != null && fieldPositions.Count > 0)
            field = this.GetFormFieldSingingDataInformation(field, eDeliveryrecipient.InitialsField, fieldPositions.ElementAt<AcroFields.FieldPosition>(index), pageHeight);
          if (signingpoint.fields == null)
            signingpoint.fields = new List<SignatureField>();
          signingpoint.fields.Add(field);
        }
      }
      if (docAttachment.SignatureFields != null && eDeliveryrecipient.CheckboxField != null && Array.IndexOf<string>(docAttachment.SignatureFields, eDeliveryrecipient.CheckboxField) >= 0)
      {
        for (int index = 0; index < docAttachment.PdfReader.AcroFields.Fields[eDeliveryrecipient.CheckboxField].Size; ++index)
        {
          SignatureField field = new SignatureField();
          field.Name = eDeliveryrecipient.CheckboxField;
          field.Type = new SignatureFieldType?(SignatureFieldType.Checkbox);
          IList<AcroFields.FieldPosition> fieldPositions = docAttachment.PdfReader.AcroFields.GetFieldPositions(eDeliveryrecipient.CheckboxField);
          if (fieldPositions != null && fieldPositions.Count > 0)
            field = this.GetFormFieldSingingDataInformation(field, eDeliveryrecipient.CheckboxField, fieldPositions.ElementAt<AcroFields.FieldPosition>(index), pageHeight);
          if (flag)
          {
            SignatureField signatureField8 = field;
            nullable = signatureField8.Bottom;
            signatureField8.Bottom = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 14) : new int?();
            SignatureField signatureField9 = field;
            nullable = signatureField9.Top;
            signatureField9.Top = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + 14) : new int?();
          }
          if (signingpoint.fields == null)
            signingpoint.fields = new List<SignatureField>();
          signingpoint.fields.Add(field);
        }
      }
      return signingpoint;
    }

    public SignatureField GetFormFieldSingingDataInformation(
      SignatureField field,
      string controlName,
      AcroFields.FieldPosition formFieldPosition,
      int pageHeight)
    {
      field.Page = new int?(formFieldPosition.page);
      Rectangle position = formFieldPosition.position;
      field.Left = new int?((int) position.Left);
      field.Right = new int?((int) position.Right);
      field.Top = new int?(pageHeight - 13 - (int) position.Top);
      field.Bottom = new int?(pageHeight - 13 - (int) position.Bottom);
      return field;
    }

    public SignatureFieldItem GetSingingDataInformation(
      SignatureFieldItem field,
      string controlName,
      AcroFields.FieldPosition formFieldPosition,
      int pageHeight)
    {
      field.Page = new int?(formFieldPosition.page);
      Rectangle position = formFieldPosition.position;
      field.Left = new int?((int) position.Left);
      field.Right = new int?((int) position.Right);
      field.Top = new int?((int) position.Top);
      field.Bottom = new int?((int) position.Bottom);
      return field;
    }

    public void UpdateEmailBodyRequest(Request request)
    {
      LoanData loanData = this.loanDataMgr.LoanData;
      string empty1 = string.Empty;
      foreach (Recipient recipient in request.recipients.FindAll((Predicate<Recipient>) (x => x.role != eDeliveryEntityType.Originator.ToString("g"))))
      {
        string url = this.addParameterToUrl(recipient.url, "packageId", "[[PackageId]]");
        recipient.createdEmail.content = HtmlFieldMerge.MergeDynamicConsumerConnectContent(recipient.createdEmail.content, url, recipient.fullName);
      }
      Recipient recipient1 = request.recipients.Find((Predicate<Recipient>) (x => x.role == eDeliveryEntityType.Originator.ToString("g")));
      if (recipient1 == null)
        return;
      string origintorEmailTemplate = this.OrigintorEmailTemplate;
      UserInfo user = Session.OrganizationManager.GetUser(recipient1.userId);
      if (user != (UserInfo) null && string.Equals(user.UserType, "External", StringComparison.OrdinalIgnoreCase))
      {
        using (BinaryObject systemSettings = Session.ConfigurationManager.GetSystemSettings("Originator-External.htm"))
        {
          if (systemSettings != null)
            origintorEmailTemplate = systemSettings.ToString();
        }
      }
      eDeliveryRecipient deliveryRecipient = this.recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientType == eDeliveryEntityType.Borrower.ToString("g")));
      string str1 = origintorEmailTemplate;
      string str2;
      if (deliveryRecipient != null)
        str2 = str1.Replace("[[BorFullName]]", !string.IsNullOrEmpty(deliveryRecipient.UnparsedName) ? deliveryRecipient.UnparsedName : deliveryRecipient.FirstName + " " + deliveryRecipient.LastName);
      else
        str2 = str1.Replace("[[BorFullName]]", string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          loanData.GetSimpleField("4000"),
          loanData.GetSimpleField("4001"),
          loanData.GetSimpleField("4002"),
          loanData.GetSimpleField("4003")
        }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str)))));
      string str3 = str2.Replace("[[URL]]", this.addParameterToUrl(recipient1.url, "packageId", "[[PackageId]]")).Replace("[[SentDate]]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
      string newValue1 = string.Empty;
      foreach (Signingpoint signingpoint in recipient1.documents.FindAll((Predicate<Signingpoint>) (x => x.fields != null)))
      {
        Signingpoint document = signingpoint;
        newValue1 = newValue1 + request.documents.Find((Predicate<Document>) (x => x.id == document.id)).title + Environment.NewLine + "<br>";
      }
      string str4 = str3.Replace("[[Documents]]", newValue1).Replace("[[PropertyAddress]]", loanData.GetSimpleField("11")).Replace("[[User]]", this.recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientType == eDeliveryEntityType.Originator.ToString("g"))).UnparsedName);
      string empty2 = string.Empty;
      string newValue2;
      switch (this.MsgType)
      {
        case eDeliveryMessageType.RequestDocuments:
          newValue2 = "Doc Request";
          break;
        case eDeliveryMessageType.InitialDisclosures:
          newValue2 = "eDisclosure package";
          break;
        default:
          newValue2 = "Documents";
          break;
      }
      string str5 = str4.Replace("[[PackageType]]", newValue2);
      recipient1.createdEmail.content = str5;
    }

    public void UpdatePackageIdInDocs(string packageId)
    {
      foreach (eDeliveryAttachment attachment in this.attachmentList)
      {
        if (attachment is DocumentAttachment)
          ((DocumentAttachment) attachment).SetPackageId(packageId);
      }
      foreach (DocumentLog needed in this.neededList)
        needed.PackageId = packageId;
    }

    protected virtual void SetOTPRecipientDetails(
      Recipient recipient,
      eDeliveryRecipient eDeliveryRecipient)
    {
    }

    protected virtual Request GetRequestObject() => new Request();

    protected virtual Recipient GetRecipientObject() => new Recipient();

    private bool IsEsignableDocument(eDeliveryAttachment attachment)
    {
      return attachment is DocumentAttachment && ((DocumentAttachment) attachment).SignatureType == "eSignable";
    }

    private string addParameterToUrl(string url, string paramName, string paramValue)
    {
      string str = url.Contains("?") ? "&" : "?";
      url = url + str + paramName + "=" + paramValue;
      return url;
    }
  }
}
