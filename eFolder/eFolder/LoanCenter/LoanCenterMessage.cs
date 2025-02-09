// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.LoanCenterMessage
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
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class LoanCenterMessage
  {
    private LoanDataMgr loanDataMgr;
    private LoanCenterMessageType msgType;
    private string to;
    private string cc;
    private List<EncompassContactEmail> nboSendEmails;
    private string replyTo;
    private string subject;
    private string body;
    private bool readReceipt;
    private List<LoanCenterAttachment> attachmentList;
    private List<DocumentLog> neededList;
    private List<LoanCenterSigner> signerList;
    private bool electronicSignature;
    private bool useBranchAddress;
    private string consentModel;
    private bool answerQuestions;
    private XmlDocument xmlDoc;
    private EDisclosureSignOrderSetup signOrderSetup;
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
    private const string SigningFields = "fields";
    private const string Type = "type";
    private const string SigningFieldName = "name";
    private const string SignersType = "signers";
    private readonly string PdfMetadata;
    private static readonly string sw = Tracing.SwEFolder;
    private string className = nameof (LoanCenterMessage);

    public LoanCenterMessage(LoanDataMgr loanDataMgr, LoanCenterMessageType msgType)
    {
      this.loanDataMgr = loanDataMgr;
      this.msgType = msgType;
      this.replyTo = Session.UserInfo.Email;
      this.signOrderSetup = Session.ConfigurationManager.GetEDisclosureSignOrderSetup();
      this.attachmentList = new List<LoanCenterAttachment>();
      this.neededList = new List<DocumentLog>();
      this.signerList = new List<LoanCenterSigner>();
      try
      {
        this.PdfMetadata = ConfigurationManager.AppSettings["PDFJsonMetadata"].ToString();
      }
      catch (Exception ex)
      {
        this.PdfMetadata = "EMFormFields";
      }
    }

    public LoanDataMgr LoanDataMgr => this.loanDataMgr;

    public LoanCenterMessageType MsgType => this.msgType;

    public bool ElectronicSignature
    {
      get => this.electronicSignature;
      set => this.electronicSignature = value;
    }

    public bool AnswerQuestions
    {
      get => this.answerQuestions;
      set => this.answerQuestions = value;
    }

    public string ReplyTo
    {
      get => this.replyTo;
      set => this.replyTo = value;
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

    public List<EncompassContactEmail> NBOSendEmails
    {
      get => this.nboSendEmails;
      set => this.nboSendEmails = value;
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

    public LoanCenterAttachment[] Attachments => this.attachmentList.ToArray();

    public LoanCenterSigner[] Signers => this.signerList.ToArray();

    public bool UseSameWebCenter
    {
      get => this.loanDataMgr.SystemConfiguration.WebCenterSettings.UseSameWebcenter;
    }

    public bool UseBranchAddress
    {
      get => this.loanDataMgr.ConfigurationManager.GetEDisclosureSetup().UseBranchAddress;
      set => this.useBranchAddress = value;
    }

    public string ConsentModel
    {
      get => this.loanDataMgr.ConfigurationManager.GetEDisclosureSetup().ConsentModelType;
      set => this.consentModel = value;
    }

    public void AddSigner(LoanCenterSigner signer)
    {
      if (this.signerList.Contains(signer))
        return;
      this.signerList.Add(signer);
    }

    public void AddAttachment(LoanCenterAttachment attachment)
    {
      if (this.attachmentList.Contains(attachment))
        return;
      this.attachmentList.Add(attachment);
    }

    public LoanCenterAttachment AddCoversheet(string filepath)
    {
      LoanCenterAttachment attachment = new LoanCenterAttachment(filepath, "COVERSHEET");
      this.AddAttachment(attachment);
      return attachment;
    }

    public DocumentAttachment AddDocument(string filepath, DocumentLog doc)
    {
      DocumentAttachment attachment = new DocumentAttachment(filepath, doc);
      this.AddAttachment((LoanCenterAttachment) attachment);
      return attachment;
    }

    public SDTAttachment AddDocuments(string filepath, DocumentLog[] docList)
    {
      SDTAttachment attachment = new SDTAttachment(filepath, docList);
      this.AddAttachment((LoanCenterAttachment) attachment);
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
      this.AddAttachment((LoanCenterAttachment) attachment);
      return attachment;
    }

    public void AddNeeded(DocumentLog[] docList)
    {
      this.neededList.AddRange((IEnumerable<DocumentLog>) docList);
    }

    public bool CheckRequirements()
    {
      long num1 = 0;
      foreach (LoanCenterAttachment attachment in this.Attachments)
        num1 += attachment.Filesize;
      if (num1 > 31400000L)
      {
        PerformanceMeter.Current.AddCheckpoint("BEFORE Message Over Size ShowDialog()", 376, nameof (CheckRequirements), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
        int num2 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The message you are attempting to send is larger than the maximum size allowed. Please send the documents in multiple, smaller sets.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        PerformanceMeter.Current.AddCheckpoint("EXIT FALSE AFTER Message Over Size ShowDialog()", 381, nameof (CheckRequirements), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
        return false;
      }
      string simpleField1 = this.loanDataMgr.LoanData.GetSimpleField("11");
      string simpleField2 = this.loanDataMgr.LoanData.GetSimpleField("FR0104");
      if (simpleField1.Contains(" ") || simpleField2.Contains(" "))
        return true;
      PerformanceMeter.Current.AddCheckpoint("BEFORE Borrower Address ShowDialog()", 391, nameof (CheckRequirements), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
      int num3 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "In order to use this service, you need to type in the Borrower's Present Address or the Property Address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      PerformanceMeter.Current.AddCheckpoint("EXIT FALSE AFTER Borrower Address ShowDialog()", 396, nameof (CheckRequirements), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
      return false;
    }

    public void CreateLogEntry()
    {
      HtmlEmailLog htmlEmailLog1 = new HtmlEmailLog(Session.UserInfo.FullName + " (" + Session.UserID + ")");
      switch (this.msgType)
      {
        case LoanCenterMessageType.RequestDocuments:
          htmlEmailLog1.Description = "Documents requested";
          break;
        case LoanCenterMessageType.SendDocuments:
          htmlEmailLog1.Description = "Documents sent";
          break;
        case LoanCenterMessageType.SecureFormTransfer:
          htmlEmailLog1.Description = "Secure form transfer";
          break;
        case LoanCenterMessageType.Consent:
          htmlEmailLog1.Description = "Consent requested";
          break;
      }
      string str1 = this.readReceipt ? "Yes" : "No";
      htmlEmailLog1.Sender = this.replyTo;
      htmlEmailLog1.Recipient = this.to;
      if (!string.IsNullOrEmpty(this.cc))
      {
        HtmlEmailLog htmlEmailLog2 = htmlEmailLog1;
        htmlEmailLog2.Recipient = htmlEmailLog2.Recipient + "; " + this.cc;
      }
      if (this.nboSendEmails != null && this.nboSendEmails.Count > 0)
      {
        HtmlEmailLog htmlEmailLog3 = htmlEmailLog1;
        htmlEmailLog3.Recipient = htmlEmailLog3.Recipient + "; " + string.Join("; ", ((IEnumerable<string>) this.nboSendEmails.Select<EncompassContactEmail, string>((Func<EncompassContactEmail, string>) (x => x.email)).ToArray<string>()).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      }
      htmlEmailLog1.Subject = this.subject;
      htmlEmailLog1.Body = this.body;
      htmlEmailLog1.ReadReceipt = this.readReceipt;
      List<string> stringList = new List<string>();
      foreach (LoanCenterAttachment attachment in this.attachmentList)
      {
        switch (attachment)
        {
          case DocumentAttachment _:
            DocumentAttachment documentAttachment = (DocumentAttachment) attachment;
            stringList.Add(documentAttachment.Document.Title);
            string details1 = (string) null;
            switch (this.msgType)
            {
              case LoanCenterMessageType.RequestDocuments:
                details1 = "Doc request sent";
                break;
              case LoanCenterMessageType.SendDocuments:
                details1 = "Doc sent";
                break;
            }
            if (details1 != null)
            {
              this.loanDataMgr.LoanHistory.TrackChange((LogRecordBase) documentAttachment.Document, details1, (LogRecordBase) htmlEmailLog1);
              continue;
            }
            continue;
          case SDTAttachment _:
            foreach (DocumentLog document in ((SDTAttachment) attachment).Documents)
            {
              stringList.Add(document.Title);
              string details2 = (string) null;
              switch (this.msgType)
              {
                case LoanCenterMessageType.RequestDocuments:
                  details2 = "Doc request sent";
                  break;
                case LoanCenterMessageType.SendDocuments:
                  details2 = "Doc sent";
                  break;
              }
              if (details2 != null)
                this.loanDataMgr.LoanHistory.TrackChange((LogRecordBase) document, details2, (LogRecordBase) htmlEmailLog1);
            }
            continue;
          case SFTAttachment _:
            SFTAttachment sftAttachment = (SFTAttachment) attachment;
            stringList.AddRange((IEnumerable<string>) sftAttachment.Forms);
            continue;
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

    private void setNodeText(string xpath, string val)
    {
      if (val == null || val == string.Empty)
        return;
      XmlElement xmlElement = this.xmlDoc.DocumentElement;
      string str1 = xpath;
      char[] chArray = new char[1]{ '/' };
      foreach (string xpath1 in str1.Split(chArray))
      {
        if (xpath1.StartsWith("@"))
        {
          xmlElement.SetAttribute(xpath1.Substring(1), val);
          return;
        }
        if (xpath1 == "!CDATA")
        {
          xmlElement.AppendChild((XmlNode) this.xmlDoc.CreateCDataSection(val));
          return;
        }
        string name1 = xpath1;
        if (name1.EndsWith("]"))
          name1 = name1.Substring(0, name1.IndexOf("["));
        XmlNode xmlNode = xmlElement.SelectSingleNode(xpath1);
        while (xmlNode == null)
        {
          xmlNode = xmlElement.AppendChild((XmlNode) this.xmlDoc.CreateElement(name1));
          if (xpath1.IndexOf("[@") > 0)
          {
            string str2 = xpath1.Substring(xpath1.IndexOf("[@") + 2);
            string str3 = str2.Substring(0, str2.LastIndexOf("]"));
            string name2 = str3.Substring(0, str3.IndexOf("=")).Trim();
            string str4 = str3.Substring(str3.IndexOf("\"") + 1);
            string str5 = str4.Substring(0, str4.LastIndexOf("\""));
            ((XmlElement) xmlNode).SetAttribute(name2, str5);
          }
          else
            xmlNode = xmlElement.SelectSingleNode(xpath1);
        }
        xmlElement = (XmlElement) xmlNode;
      }
      xmlElement.InnerText = val;
    }

    private void setNodeField(string xpath, string val)
    {
      this.setNodeText(xpath, this.LoanDataMgr.LoanData.GetField(val));
    }

    public string ToXml()
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 606, nameof (ToXml), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
      LoanData loanData = this.loanDataMgr.LoanData;
      CompanyInfo companyInfo = Session.CompanyInfo;
      UserInfo userInfo = Session.UserInfo;
      string empty = string.Empty;
      OrgInfo orgInfo = Session.OrganizationManager.GetFirstAvaliableOrganization(Session.UserInfo.OrgId);
      if (orgInfo == null)
      {
        orgInfo = new OrgInfo();
        orgInfo.CompanyName = companyInfo.Name;
        orgInfo.CompanyAddress.Street1 = companyInfo.Address;
        orgInfo.CompanyAddress.City = companyInfo.City;
        orgInfo.CompanyAddress.State = companyInfo.State;
        orgInfo.CompanyAddress.Zip = companyInfo.Zip;
        orgInfo.CompanyPhone = companyInfo.Phone;
        orgInfo.CompanyFax = companyInfo.Fax;
      }
      string val1 = (string) null;
      switch (this.msgType)
      {
        case LoanCenterMessageType.RequestDocuments:
          val1 = "Request2";
          break;
        case LoanCenterMessageType.SendDocuments:
          val1 = "Send";
          break;
        case LoanCenterMessageType.InitialDisclosures:
          val1 = "Disclosures";
          break;
        case LoanCenterMessageType.SecureFormTransfer:
          val1 = "SFT";
          break;
        case LoanCenterMessageType.Consent:
          val1 = "Consent";
          break;
      }
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml("<DOCUMENTSUBMISSION/>");
      this.setNodeText("@Type", val1);
      this.setNodeText("@ESignatures", this.ElectronicSignature ? "Y" : "N");
      this.setNodeText("@UseSameWebCenter", this.UseSameWebCenter ? "Y" : "N");
      this.setNodeText("@Version", VersionInformation.CurrentVersion.DisplayVersion);
      this.setNodeText("@UseBranchAddress", this.UseBranchAddress ? "Y" : "N");
      this.setNodeText("@ConsentModel", this.ConsentModel);
      this.setNodeText("@IsDocuSignClientVersion", "Y");
      if (this.NotificationDate.Date != DateTime.MinValue.Date)
        this.setNodeText("@NotificationDate", this.NotificationDate.ToString("MM/dd/yyyy"));
      if (this.msgType == LoanCenterMessageType.InitialDisclosures)
        this.setNodeText("@OriginatorFulfillmentFee", Session.ConfigurationManager.GetCompanySetting("eDisclosures", "FulfillmentFee"));
      this.setNodeText("FROM/@ClientID", companyInfo.ClientID);
      this.setNodeText("FROM/@UserID", userInfo.Userid);
      this.setNodeText("FROM/@UserPassword", EpassLogin.LoginPassword);
      this.setNodeText("FROM/COMPANY/@Name", orgInfo.CompanyName);
      this.setNodeText("FROM/COMPANY/@StreetAddress1", orgInfo.CompanyAddress.Street1);
      this.setNodeText("FROM/COMPANY/@StreetAddress2", orgInfo.CompanyAddress.Street2);
      this.setNodeText("FROM/COMPANY/@City", orgInfo.CompanyAddress.City);
      this.setNodeText("FROM/COMPANY/@State", orgInfo.CompanyAddress.State);
      this.setNodeText("FROM/COMPANY/@PostalCode", orgInfo.CompanyAddress.Zip);
      this.setNodeText("FROM/COMPANY/@Phone", orgInfo.CompanyPhone);
      this.setNodeText("FROM/COMPANY/@Fax", orgInfo.CompanyFax);
      this.setNodeText("FROM/USER/@Name", userInfo.FullName);
      this.setNodeText("FROM/USER/@Phone", userInfo.Phone);
      this.setNodeText("FROM/USER/@Fax", userInfo.Fax);
      this.setNodeText("FROM/USER/@Email", userInfo.Email);
      this.setNodeField("LOAN/@LoanGUID", "GUID");
      this.setNodeField("LOAN/@LoanNumber", "364");
      this.setNodeText("LOAN/@BorrowerPair", loanData.PairId);
      this.setNodeField("LOAN/@WebsiteID", "WEBSITEID");
      this.setNodeField("LOAN/@ApplicationDate", "745");
      this.setNodeField("LOAN/BORROWER/@FirstName", "4000");
      this.setNodeField("LOAN/BORROWER/@LastName", "4002");
      this.setNodeField("LOAN/BORROWER/@Name", "1868");
      this.setNodeField("LOAN/BORROWER/@StreetAddress", "FR0104");
      this.setNodeField("LOAN/BORROWER/@City", "FR0106");
      this.setNodeField("LOAN/BORROWER/@State", "FR0107");
      this.setNodeField("LOAN/BORROWER/@PostalCode", "FR0108");
      this.setNodeField("LOAN/BORROWER/@HomePhone", "66");
      this.setNodeField("LOAN/BORROWER/@WorkPhone", "FE0117");
      this.setNodeField("LOAN/BORROWER/@CellPhone", "1490");
      this.setNodeField("LOAN/BORROWER/@Email", "1240");
      List<NonBorrowingOwner> byBorrowerPairId = this.loanDataMgr.LoanData.GetNboByBorrowerPairId(loanData.PairId);
      if (byBorrowerPairId.Count > 0)
      {
        foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
        {
          string str = "LOAN/ENCOMPASSCONTACTS/ENCOMPASSCONTACT[@EncompassContactGUID=\"" + nonBorrowingOwner.NBOID + "\"]/";
          this.setNodeText(str + "@Name", nonBorrowingOwner.FirstName + " " + nonBorrowingOwner.MiddleName + " " + nonBorrowingOwner.LastName);
          this.setNodeText(str + "@FirstName", nonBorrowingOwner.FirstName);
          this.setNodeText(str + "@LastName", nonBorrowingOwner.LastName);
          this.setNodeText(str + "@Email", nonBorrowingOwner.EmailAddress);
        }
      }
      if (loanData.GetSimpleField("4004") != string.Empty)
      {
        this.setNodeField("LOAN/COBORROWER/@FirstName", "4004");
        this.setNodeField("LOAN/COBORROWER/@LastName", "4006");
        this.setNodeField("LOAN/COBORROWER/@Name", "1873");
        this.setNodeField("LOAN/COBORROWER/@StreetAddress", "FR0204");
        this.setNodeField("LOAN/COBORROWER/@City", "FR0206");
        this.setNodeField("LOAN/COBORROWER/@State", "FR0207");
        this.setNodeField("LOAN/COBORROWER/@PostalCode", "FR0208");
        this.setNodeField("LOAN/COBORROWER/@HomePhone", "98");
        this.setNodeField("LOAN/COBORROWER/@WorkPhone", "FE0217");
        this.setNodeField("LOAN/COBORROWER/@CellPhone", "1480");
        this.setNodeField("LOAN/COBORROWER/@Email", "1268");
      }
      this.setNodeField("LOAN/PROPERTY/@StreetAddress", "11");
      this.setNodeField("LOAN/PROPERTY/@City", "12");
      this.setNodeField("LOAN/PROPERTY/@State", "14");
      this.setNodeField("LOAN/PROPERTY/@PostalCode", "15");
      this.setNodeField("LOAN/PROPERTY/@AppraisedValue", "356");
      this.setNodeField("LOAN/MORTGAGE/@MortgageType", "1172");
      this.setNodeField("LOAN/MORTGAGE/@LoanAmortizationType", "608");
      this.setNodeField("LOAN/MORTGAGE/@LoanPurpose", "19");
      this.setNodeField("LOAN/MORTGAGE/@InterestRatePercent", "3");
      this.setNodeField("LOAN/MORTGAGE/@LoanAmount", "1109");
      this.setNodeField("LOAN/ORIGINATOR/@UserID", "3239");
      this.setNodeField("LOAN/ORIGINATOR/@Name", "1612");
      this.setNodeField("LOAN/ORIGINATOR/@StreetAddress", "319");
      this.setNodeField("LOAN/ORIGINATOR/@City", "313");
      this.setNodeField("LOAN/ORIGINATOR/@State", "321");
      this.setNodeField("LOAN/ORIGINATOR/@PostalCode", "323");
      this.setNodeField("LOAN/ORIGINATOR/@Phone", "1823");
      this.setNodeField("LOAN/ORIGINATOR/@Fax", "326");
      this.setNodeField("LOAN/OFFICER/@UserID", "LOID");
      this.setNodeField("LOAN/OFFICER/@Name", "317");
      this.setNodeField("LOAN/OFFICER/@Title", "4508");
      this.setNodeField("LOAN/OFFICER/@StreetAddress", "319");
      this.setNodeField("LOAN/OFFICER/@City", "313");
      this.setNodeField("LOAN/OFFICER/@State", "321");
      this.setNodeField("LOAN/OFFICER/@PostalCode", "323");
      this.setNodeField("LOAN/OFFICER/@Phone", "1406");
      this.setNodeField("LOAN/OFFICER/@Fax", "2411");
      this.setNodeField("LOAN/OFFICER/@Email", "1407");
      this.setNodeField("LOAN/PROCESSOR/@UserID", "LPID");
      this.setNodeField("LOAN/PROCESSOR/@Name", "362");
      this.setNodeField("LOAN/PROCESSOR/@Title", "4509");
      this.setNodeField("LOAN/PROCESSOR/@StreetAddress", "319");
      this.setNodeField("LOAN/PROCESSOR/@City", "313");
      this.setNodeField("LOAN/PROCESSOR/@State", "321");
      this.setNodeField("LOAN/PROCESSOR/@PostalCode", "323");
      this.setNodeField("LOAN/PROCESSOR/@Phone", "1408");
      this.setNodeField("LOAN/PROCESSOR/@Fax", "2412");
      this.setNodeField("LOAN/PROCESSOR/@Email", "1409");
      this.setNodeField("LOAN/CLOSER/@UserID", "CLID");
      this.setNodeField("LOAN/CLOSER/@Name", "1855");
      this.setNodeField("LOAN/CLOSER/@StreetAddress", "319");
      this.setNodeField("LOAN/CLOSER/@City", "313");
      this.setNodeField("LOAN/CLOSER/@State", "321");
      this.setNodeField("LOAN/CLOSER/@PostalCode", "323");
      this.setNodeField("LOAN/CLOSER/@Phone", "1856");
      this.setNodeField("LOAN/CLOSER/@Fax", "2413");
      this.setNodeField("LOAN/CLOSER/@Email", "1857");
      this.setNodeText("EMAIL/@To", this.To);
      this.setNodeText("EMAIL/@Cc", this.CC);
      if (this.NBOSendEmails != null)
      {
        foreach (EncompassContactEmail nboSendEmail in this.NBOSendEmails)
          this.setNodeText("EMAIL/ENCOMPASSCONTACTREQUESTEMAILS/ENCOMPASSCONTACTREQUESTEMAIL[@EncompassContactGUID=\"" + nboSendEmail.encompasscontactGuid + "\"]/" + "@Email", nboSendEmail.email);
        PerformanceMeter.Current.AddCheckpoint("NBOSendEmails", 809, nameof (ToXml), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
      }
      this.setNodeText("EMAIL/@ReplyTo", this.ReplyTo);
      this.setNodeText("EMAIL/@ReplyTo", this.ReplyTo);
      this.setNodeText("EMAIL/@ReadReceipt", this.ReadReceipt ? "Y" : "N");
      this.setNodeText("EMAIL/@HtmlEmail", "Y");
      this.setNodeText("EMAIL/SUBJECT", this.Subject);
      this.setNodeText("EMAIL/MESSAGE/!CDATA", this.Body);
      PerformanceMeter.Current.AddCheckpoint("Initial Nodes", 818, nameof (ToXml), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
      if (this.FulfillmentEnabled)
      {
        this.setNodeText("FULFILLMENT/@ScheduledDate", this.FulfillmentSchedDate.ToString("MM/dd/yyyy"));
        this.setNodeText("FULFILLMENT/FROM/@Name", this.FulfillmentFromName);
        this.setNodeText("FULFILLMENT/FROM/@StreetAddress", this.FulfillmentFromAddress);
        this.setNodeText("FULFILLMENT/FROM/@City", this.FulfillmentFromCity);
        this.setNodeText("FULFILLMENT/FROM/@State", this.FulfillmentFromState);
        this.setNodeText("FULFILLMENT/FROM/@PostalCode", this.FulfillmentFromZip);
        this.setNodeText("FULFILLMENT/FROM/@Phone", this.FulfillmentFromPhone);
        this.setNodeText("FULFILLMENT/TO/@Name", this.FulfillmentToName);
        this.setNodeText("FULFILLMENT/TO/@StreetAddress", this.FulfillmentToAddress);
        this.setNodeText("FULFILLMENT/TO/@City", this.FulfillmentToCity);
        this.setNodeText("FULFILLMENT/TO/@State", this.FulfillmentToState);
        this.setNodeText("FULFILLMENT/TO/@PostalCode", this.FulfillmentToZip);
        this.setNodeText("FULFILLMENT/TO/@Phone", this.FulfillmentToPhone);
      }
      int num1 = 0;
      foreach (LoanCenterSigner signer in this.signerList)
      {
        ++num1;
        string str1 = "SIGNERS/SIGNER[" + num1.ToString() + "]/";
        string val2 = (string) null;
        switch (signer.EntityType)
        {
          case LoanCenterEntityType.Borrower:
            val2 = "BORROWER";
            break;
          case LoanCenterEntityType.Coborrower:
            val2 = "COBORROWER";
            break;
          case LoanCenterEntityType.Originator:
            val2 = "ORIGINATOR";
            break;
          case LoanCenterEntityType.NonBorrowingOwner:
            val2 = "NONBORROWINGOWNER";
            break;
        }
        this.setNodeText(str1 + "@SignerID", signer.SignerID);
        this.setNodeText(str1 + "@Type", val2);
        this.setNodeText(str1 + "@FirstName", signer.FirstName);
        this.setNodeText(str1 + "@MiddleName", signer.MiddleName);
        this.setNodeText(str1 + "@LastName", signer.LastName);
        this.setNodeText(str1 + "@SuffixName", signer.SuffixName);
        this.setNodeText(str1 + "@Name", signer.UnparsedName);
        this.setNodeText(str1 + "@Email", signer.EmailAddress);
        this.setNodeText(str1 + "@UserID", signer.UserID);
        this.setNodeText(str1 + "@AutoSign", signer.AutoSign ? "Y" : "N");
        if (signer.EntityType == LoanCenterEntityType.Originator)
        {
          this.setNodeText(str1 + "@SignFirst", "N");
          if (this.signOrderSetup != null && this.signOrderSetup.States.Count > 0)
          {
            bool boolean = Convert.ToBoolean(this.signOrderSetup.SignOrderEnabled);
            bool flag = false;
            this.signOrderSetup.States.TryGetValue(this.LoanDataMgr.LoanData.GetField("14").ToUpper(), out flag);
            if (this.msgType == LoanCenterMessageType.InitialDisclosures & boolean & flag)
              this.setNodeText(str1 + "@SignFirst", "Y");
          }
        }
        if (signer.EntityType == LoanCenterEntityType.Borrower || signer.EntityType == LoanCenterEntityType.Coborrower || signer.EntityType == LoanCenterEntityType.NonBorrowingOwner)
        {
          string str2 = str1 + "SECURITY_FIELDS/";
          this.setNodeText(str2 + "@Type", this.answerQuestions ? "Questions" : "Password");
          this.setNodeText(str2 + "@ESignPassword", signer.AuthCode);
          if (this.answerQuestions)
          {
            int num2 = 0;
            foreach (LoanCenterSecurityQuestion securityQuestion in signer.SecurityQuestions)
            {
              ++num2;
              string str3 = str2 + "FIELD[" + num2.ToString() + "]/";
              this.setNodeText(str3 + "@ID", securityQuestion.FieldID);
              this.setNodeText(str3 + "@Value", securityQuestion.FieldValue);
            }
          }
        }
      }
      PerformanceMeter.Current.AddCheckpoint("Signer Nodes", 923, nameof (ToXml), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
      int num3 = 0;
      int num4 = 0;
      foreach (LoanCenterAttachment attachment in this.attachmentList)
      {
        switch (attachment)
        {
          case DocumentAttachment _:
            using (PerformanceMeter.Current.BeginOperation("LoanCenterMessage.ToXml.DocumentAttachment"))
            {
              ++num3;
              string docNode = "DOCUMENTS/DOCUMENT[" + num3.ToString() + "]/";
              DocumentAttachment docAttachment = (DocumentAttachment) attachment;
              this.setNodeText(docNode + "@ID", docAttachment.ID);
              this.setNodeText(docNode + "@Name", docAttachment.Title);
              this.setNodeText(docNode + "@PageCount", docAttachment.PageCount.ToString());
              if (docAttachment.SignatureType == "eSignable" && !this.ElectronicSignature)
                this.setNodeText(docNode + "@SignatureType", "");
              else
                this.setNodeText(docNode + "@SignatureType", docAttachment.SignatureType);
              if (docAttachment.SignatureFields != null)
              {
                Tracing.Log(LoanCenterMessage.sw, TraceLevel.Verbose, this.className, "Checking if JSON Metadata is attached");
                if (docAttachment.PdfReader == null)
                  docAttachment.PdfReader = new PdfReader(attachment.Filepath);
                if (docAttachment.PdfReader.Info != null && docAttachment.PdfReader.Info.ContainsKey(this.PdfMetadata))
                {
                  Tracing.Log(LoanCenterMessage.sw, TraceLevel.Info, this.className, "JSON Metadata is attached");
                  this.ParseJsonMetadataAndAppendSignNodes(docAttachment.PdfReader.Info[this.PdfMetadata].ToString(), docNode);
                  continue;
                }
                Tracing.Log(LoanCenterMessage.sw, TraceLevel.Info, this.className, "JSON Metadata is not attached");
                this.GetSigningPointFromAttachment(docAttachment, docNode);
                continue;
              }
              continue;
            }
          case SDTAttachment _:
            using (PerformanceMeter.Current.BeginOperation("LoanCenterMessage.ToXml.SDTAttachment"))
            {
              ++num4;
              string str4 = "DOCUMENTS/MERGED_DOCUMENT[" + num4.ToString() + "]/";
              SDTAttachment sdtAttachment = (SDTAttachment) attachment;
              this.setNodeText(str4 + "@ID", sdtAttachment.ID);
              this.setNodeText(str4 + "@Name", sdtAttachment.Title);
              int num5 = 0;
              foreach (DocumentLog document in sdtAttachment.Documents)
              {
                ++num5;
                string str5 = str4 + "MERGED_DOCUMENT_ENTITY[" + num5.ToString() + "]/";
                this.setNodeText(str5 + "@ID", document.Guid);
                this.setNodeText(str5 + "@Name", document.Title);
              }
              continue;
            }
          case SFTAttachment _:
            using (PerformanceMeter.Current.BeginOperation("LoanCenterMessage.ToXml.SFTAttachment"))
            {
              ++num4;
              string str = "DOCUMENTS/MERGED_DOCUMENT[" + num4.ToString() + "]/";
              SFTAttachment sftAttachment = (SFTAttachment) attachment;
              this.setNodeText(str + "@ID", sftAttachment.ID);
              this.setNodeText(str + "@Name", sftAttachment.Title);
              int num6 = 0;
              foreach (string form in sftAttachment.Forms)
              {
                ++num6;
                this.setNodeText(str + "MERGED_DOCUMENT_ENTITY[" + num6.ToString() + "]/" + "@Name", form);
              }
              continue;
            }
          default:
            using (PerformanceMeter.Current.BeginOperation("LoanCenterMessage.ToXml.OtherAttachment"))
            {
              ++num3;
              string str = "DOCUMENTS/DOCUMENT[" + num3.ToString() + "]/";
              this.setNodeText(str + "@ID", attachment.ID);
              this.setNodeText(str + "@Name", attachment.Title);
              continue;
            }
        }
      }
      PerformanceMeter.Current.AddCheckpoint("Attachments Loop", 1046, nameof (ToXml), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
      int num7 = 0;
      foreach (DocumentLog needed in this.neededList)
      {
        ++num7;
        string str = "DOCUMENTS/NEEDED_DOCUMENT[" + num7.ToString() + "]/";
        this.setNodeText(str + "@ID", needed.Guid);
        this.setNodeText(str + "@Name", needed.Title);
      }
      string outerXml = this.xmlDoc.OuterXml;
      PerformanceMeter.Current.AddCheckpoint("END", 1064, nameof (ToXml), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\LoanCenterMessage.cs");
      return outerXml;
    }

    private void ParseJsonMetadataAndAppendSignNodes(string metadataValue, string docNode)
    {
      Tracing.Log(LoanCenterMessage.sw, TraceLevel.Info, this.className, "Entering ParseJsonMetadataAndAppendSignNodes method");
      IEnumerable<JToken> source1;
      try
      {
        source1 = JObject.Parse(metadataValue)["signers"].Children().AsEnumerable<JToken>();
      }
      catch (JsonReaderException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      if (source1.Count<JToken>() <= 0)
        return;
      foreach (LoanCenterSigner signer1 in this.signerList)
      {
        LoanCenterSigner signer = signer1;
        string str = docNode + "SIGNATURES/SIGNATURE[@SignerID=\"" + signer.SignerID + "\"]/";
        JToken jtoken1 = source1.FirstOrDefault<JToken>((Func<JToken, bool>) (role => role[(object) "type"].ToString() == signer.EntityType.ToString().ToLower()));
        if (jtoken1 != null)
        {
          JEnumerable<JToken> source2 = jtoken1[(object) "fields"].Children();
          JToken jtoken2 = source2.FirstOrDefault<JToken>((Func<JToken, bool>) (field => string.Compare(field[(object) "type"].ToString(), SignatureFieldType.Signature.ToString(), true) == 0 || string.Compare(field[(object) "type"].ToString(), SignatureFieldType.SignAndDate.ToString(), true) == 0));
          if (jtoken2 != null)
            this.setNodeText(str + "@FullSignatureFieldName", jtoken2[(object) "name"].ToString());
          JToken jtoken3 = source2.FirstOrDefault<JToken>((Func<JToken, bool>) (field => string.Compare(field[(object) "type"].ToString(), SignatureFieldType.Initials.ToString(), true) == 0));
          if (jtoken3 != null)
            this.setNodeText(str + "@InitialsFieldName", jtoken3[(object) "name"].ToString());
          JToken jtoken4 = source2.FirstOrDefault<JToken>((Func<JToken, bool>) (field => string.Compare(field[(object) "type"].ToString(), SignatureFieldType.Checkbox.ToString(), true) == 0));
          if (jtoken4 != null)
            this.setNodeText(str + "@CheckboxFieldName", jtoken4[(object) "name"].ToString());
        }
      }
    }

    private void GetSigningPointFromAttachment(DocumentAttachment docAttachment, string docNode)
    {
      foreach (LoanCenterSigner signer in this.signerList)
      {
        string str = docNode + "SIGNATURES/SIGNATURE[@SignerID=\"" + signer.SignerID + "\"]/";
        if (Array.IndexOf<string>(docAttachment.SignatureFields, signer.SignatureField) >= 0)
          this.setNodeText(str + "@FullSignatureFieldName", signer.SignatureField);
        if (Array.IndexOf<string>(docAttachment.SignatureFields, signer.InitialsField) >= 0)
          this.setNodeText(str + "@InitialsFieldName", signer.InitialsField);
        if (Array.IndexOf<string>(docAttachment.SignatureFields, signer.CheckboxField) >= 0)
          this.setNodeText(str + "@CheckboxFieldName", signer.CheckboxField);
      }
    }

    public bool LoadSignersFromJSON(DocumentAttachment attachment, List<LoanCenterSigner> signers)
    {
      if (attachment.PdfReader == null)
        attachment.PdfReader = new PdfReader(attachment.Filepath);
      if (attachment.PdfReader.Info == null || !attachment.PdfReader.Info.ContainsKey(this.PdfMetadata))
        return false;
      string json = attachment.PdfReader.Info[this.PdfMetadata].ToString();
      try
      {
        IEnumerable<JToken> source = JObject.Parse(json)[nameof (signers)].Children().AsEnumerable<JToken>();
        if (source.Count<JToken>() > 0)
        {
          foreach (LoanCenterSigner signer1 in signers)
          {
            LoanCenterSigner signer = signer1;
            if (signer != null)
            {
              if (signer.EntityType != LoanCenterEntityType.NonBorrowingOwner)
              {
                JToken jtoken = source.FirstOrDefault<JToken>((Func<JToken, bool>) (field => field[(object) "type"].ToString() == signer.EntityType.ToString().ToLower()));
                if (jtoken != null)
                {
                  if (jtoken[(object) "fields"].Children().FirstOrDefault<JToken>((Func<JToken, bool>) (field => field[(object) "type"].ToString() == SignatureFieldType.Signature.ToString() || field[(object) "type"].ToString() == SignatureFieldType.SignAndDate.ToString())) != null)
                    this.AddSigner(signer);
                  else if (jtoken[(object) "fields"].Children().FirstOrDefault<JToken>((Func<JToken, bool>) (field => field[(object) "type"].ToString() == SignatureFieldType.Signature.ToString())) != null)
                    this.AddSigner(signer);
                }
              }
              else
              {
                JToken jtoken = source.FirstOrDefault<JToken>((Func<JToken, bool>) (field => field[(object) "type"].ToString().ToLower() == signer.EntityType.ToString().ToLower() && field[(object) "id"].ToString() == signer.SignerID));
                if (jtoken != null)
                {
                  if (jtoken[(object) "fields"].Children().FirstOrDefault<JToken>((Func<JToken, bool>) (field => field[(object) "type"].ToString() == SignatureFieldType.Signature.ToString() || field[(object) "type"].ToString() == SignatureFieldType.SignAndDate.ToString())) != null)
                    this.AddSigner(signer);
                  else if (jtoken[(object) "fields"].Children().FirstOrDefault<JToken>((Func<JToken, bool>) (field => field[(object) "type"].ToString() == SignatureFieldType.Signature.ToString())) != null)
                    this.AddSigner(signer);
                }
              }
            }
          }
        }
      }
      catch (JsonReaderException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return true;
    }
  }
}
