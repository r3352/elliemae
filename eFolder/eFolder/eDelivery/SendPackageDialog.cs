// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.SendPackageDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.eDelivery.ePass;
using EllieMae.EMLite.eFolder.eDelivery.HelperMethods;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class SendPackageDialog : Form
  {
    private const string SIGNING_OPTION_ELECTRONIC = "eSign (electronically sign and return)";
    private const string SIGNING_OPTION_ELECTRONIC_AND_WET = "eSign + Wet Sign (for wet sign only documents)";
    private const string SIGNING_OPTION_WET = "Wet Sign (print, sign, and fax)";
    private const string SIGNING_OPTION_NONE = "No Signature Required";
    private LoanDataMgr loanDataMgr;
    protected LoanData loan;
    private string coversheetFile;
    private DocumentLog[] signList;
    private DocumentLog[] neededList;
    private string[] pdfList;
    private List<DocumentAttachment> attachmentList;
    protected List<eDeliveryRecipient> Recipients;
    protected List<eDeliveryRecipient> deletedRecipients;
    private List<string> requiredRecipients;
    private bool noOriginatorCancel;
    private string packageID;
    private string comments;
    private bool isWetSign;
    protected HtmlEmailTemplateType htmlEmailTemplate;
    protected EmailManager.EmailType emailType;
    protected eDeliveryMessageType packageType;
    private eDeliveryMessage msg;
    protected List<ContactDetails> contacts;
    protected List<string> invalidList = new List<string>();
    private EDisclosureSignOrderSetup signOrderSetup;
    private IContainer components;
    private Label lblRequired;
    private Button btnCancel;
    private Button btnSend;
    private Panel panel1;
    private Label lblSenderEmail;
    private Label lblSenderName;
    private Label lblSenderType;
    protected GroupContainer gcFulfillment;
    private CheckBox chkFulfillment;
    private Panel pnlFulfillment;
    private DateTimePicker dateFulfillBy;
    private Label lblFulfillment;
    private Label lblGfeDate;
    private Label lblFulfillBy;
    private Label lblMailFrom;
    private TextBox txtFromName;
    private Label lblFromName;
    private TextBox txtGfeDate;
    private TextBox txtFromAddress;
    private Label lblFromAddress1;
    private TextBox txtToBorrowerZip;
    private TextBox txtFromCity;
    private TextBox txtToBorrowerState;
    private Label lblFromAddress2;
    private Label lblToBorrowerPhone;
    private TextBox txtFromPhone;
    private TextBox txtToBorrowerPhone;
    private Label lblFromPhone;
    private Label lblToBorrowerAddress2;
    private TextBox txtFromState;
    private TextBox txtToBorrowerCity;
    private TextBox txtFromZip;
    private Label lblToBorrowerAddress;
    private Label lblMailToBorrower;
    private TextBox txtToBorrowerAddress;
    private TextBox txtToBorrowerName;
    private Label lblToBorrowerName;
    protected GroupContainer gcSigning;
    private ComboBox cboSigningOption;
    private Label lblSigningOption;
    protected GroupContainer gcMessage;
    protected ComboBox cboSubject;
    private Label lblSubject;
    private Button btnNotifyUsers;
    private Label lblNotifyUserCount;
    private DateTimePicker dateAcceptBy;
    private CheckBox chkAcceptBy;
    private CheckBox chkReadReceipt;
    private EMHelpLink emHelpLink;
    protected HtmlEmailControl htmlEmailControl;
    protected Panel panel2;
    protected Panel panel6;
    protected StandardIconButton btnAddRecipient;
    protected Label lblAuthorizationCode;
    private Label lblRecipientEmail;
    private Label lblRecipientName;
    private Label lblRecipientType;
    private TextBox txtSenderEmail;
    private TextBox txtSenderName;
    private ComboBox cmbSenderType;
    private Label label1;
    private Label label2;
    private Label label3;
    protected FlowLayoutPanel pnlRecipients;

    public SendPackageDialog(
      eDeliveryMessageType packageType,
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 67, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
      this.InitializeComponent();
      this.packageType = packageType;
      this.loanDataMgr = loanDataMgr;
      this.loan = loanDataMgr.LoanData;
      this.coversheetFile = coversheetFile;
      this.signList = signList;
      this.neededList = neededList;
      this.pdfList = pdfList;
      this.signOrderSetup = Session.ConfigurationManager.GetEDisclosureSignOrderSetup();
      Cursor.Current = Cursors.WaitCursor;
      this.initAttachmentList();
      PerformanceMeter.Current.AddCheckpoint("Initialize Attachments", 86, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
      this.initRecipients();
      PerformanceMeter.Current.AddCheckpoint("Initialize Signers", 88, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
      this.initNotifyCount();
      if (!this.noOriginatorCancel)
        this.initContents();
      Cursor.Current = Cursors.Default;
      PerformanceMeter.Current.AddCheckpoint("END", 95, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
    }

    public SendPackageDialog(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList,
      HtmlEmailTemplateType emailType)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.loan = loanDataMgr.LoanData;
      this.coversheetFile = coversheetFile;
      this.signList = signList;
      this.neededList = neededList;
      this.pdfList = pdfList;
      this.htmlEmailTemplate = emailType;
      this.signOrderSetup = Session.ConfigurationManager.GetEDisclosureSignOrderSetup();
      if (emailType == HtmlEmailTemplateType.ConsumerConnectPreClosing)
        this.packageType = eDeliveryMessageType.InitialDisclosures;
      Cursor.Current = Cursors.WaitCursor;
      this.initAttachmentList();
      this.initRecipients();
      this.initNotifyCount();
      if (!this.noOriginatorCancel)
        this.initContents();
      Cursor.Current = Cursors.Default;
    }

    public SendPackageDialog(eDeliveryMessage msg)
    {
      this.InitializeComponent();
      this.msg = msg;
      this.loanDataMgr = msg.LoanDataMgr;
      this.loan = this.loanDataMgr.LoanData;
      this.packageType = eDeliveryMessageType.SendDocuments;
      this.signOrderSetup = Session.ConfigurationManager.GetEDisclosureSignOrderSetup();
      Cursor.Current = Cursors.WaitCursor;
      this.initNotifyCount();
      this.initRecipients();
      if (!this.noOriginatorCancel)
        this.initContents();
      Cursor.Current = Cursors.Default;
    }

    public string OriginatorSignerUrl { get; protected set; }

    public bool NoOriginatorCancel => this.noOriginatorCancel;

    public string PackageID => this.packageID;

    public bool IsWetSign => this.isWetSign;

    public string Comments => this.comments;

    private void initAttachmentList()
    {
      Session.ConfigurationManager.GetDocumentTrackingSetup();
      this.loan.GetSimpleField("4004");
      this.attachmentList = new List<DocumentAttachment>();
      for (int index = 0; index < this.pdfList.Length; ++index)
      {
        using (PdfEditor pdfEditor = new PdfEditor(this.pdfList[index]))
        {
          DocumentAttachment documentAttachment = new DocumentAttachment(this.pdfList[index], this.signList[index]);
          documentAttachment.SignatureType = pdfEditor.SignatureType;
          documentAttachment.IntendedFor = pdfEditor.IntendedFor;
          documentAttachment.PageCount = pdfEditor.PageCount;
          if (documentAttachment.SignatureType == "eSignable")
            documentAttachment.SignatureFields = pdfEditor.SignatureFields;
          this.attachmentList.Add(documentAttachment);
        }
      }
      this.requiredRecipients = this.getRequiredRecipients();
    }

    private void initNotifyCount()
    {
      this.lblNotifyUserCount.Text = string.Format("({0} Users selected)", (object) new EmailNotificationClient().ActiveEmailCount(new List<Guid>()
      {
        new Guid(this.loan.GUID)
      }.ToArray()));
    }

    private void initRecipients()
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 257, nameof (initRecipients), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
      this.contacts = this.GetLoanContacts();
      PerformanceMeter.Current.AddCheckpoint("EBSClient.GetLoanContacts", 260, nameof (initRecipients), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
      if (this.packageType != eDeliveryMessageType.SendDocuments)
      {
        if (this.requiredRecipients.Contains(eDeliveryEntityType.Borrower.ToString("g")))
          this.addRecipient(this.populateBorrower());
        if (this.requiredRecipients.Contains(eDeliveryEntityType.Coborrower.ToString("g")))
          this.addRecipient(this.populateCoborrower());
        if (this.requiredRecipients.Contains(eDeliveryEntityType.Originator.ToString("g")))
        {
          string simpleField = this.loan.GetSimpleField("3239");
          if (simpleField == string.Empty)
          {
            PerformanceMeter.Current.AddCheckpoint("BEFORE Loan Originator Warning Dialog", 280, nameof (initRecipients), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
            DialogResult dialogResult = Utils.Dialog((IWin32Window) this, "There are documents included in this request that have Loan Originator signing points, but a Loan Originator is not currently assigned to the loan. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            PerformanceMeter.Current.AddCheckpoint("AFTER Loan Originator Warning Dialog", 284, nameof (initRecipients), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
            if (dialogResult == DialogResult.No)
            {
              this.noOriginatorCancel = true;
              return;
            }
          }
          else
            this.addRecipient(this.populateOriginator(simpleField));
        }
        if (this.requiredRecipients.Contains(eDeliveryEntityType.NonBorrowingOwner.ToString("g")))
          this.populateNonBorrowingOwner().ForEach((Action<eDeliveryRecipient>) (x => this.addRecipient(x)));
      }
      PerformanceMeter.Current.AddCheckpoint("END", 305, nameof (initRecipients), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
    }

    private void initContents()
    {
      this.initTitle();
      this.initHtmlControl();
      this.initFrom();
      this.initSubject();
      if (this.packageType == eDeliveryMessageType.RequestDocuments)
        this.hideFulfillmentSection();
      else if (this.packageType == eDeliveryMessageType.SendDocuments)
      {
        this.gcMessage.Height = this.gcSigning.Bottom - this.gcMessage.Top;
        this.gcFulfillment.Height = this.gcMessage.Height;
        this.gcSigning.Visible = false;
        this.Height = 716;
        this.hideFulfillmentSection();
      }
      if (this.packageType == eDeliveryMessageType.InitialDisclosures)
      {
        switch (this.loan.GetField("3969"))
        {
          case "RESPA 2010 GFE and HUD-1":
          case "Old GFE and HUD-1":
            this.lblGfeDate.Text = "GFE Application Date";
            break;
          case "RESPA-TILA 2015 LE and CD":
            this.lblGfeDate.Text = "LE Application Date";
            break;
        }
        this.txtGfeDate.Text = this.loan.GetSimpleField("3142");
        this.dateFulfillBy.Value = Utils.ParseDate((object) this.loan.GetField("3143"), DateTime.Today.AddDays(1.0));
        this.txtFromName.Text = this.loan.GetSimpleField("315");
        this.txtFromAddress.Text = this.loan.GetSimpleField("319");
        this.txtFromCity.Text = this.loan.GetSimpleField("313");
        this.txtFromState.Text = this.loan.GetSimpleField("321");
        this.txtFromZip.Text = this.loan.GetSimpleField("323");
        this.txtFromPhone.Text = this.loan.GetSimpleField("324");
        this.txtToBorrowerName.Text = this.loan.GetSimpleField("4000") + " " + this.loan.GetSimpleField("4002");
        this.txtToBorrowerPhone.Text = this.loan.GetSimpleField("66");
        if (this.loan.GetSimpleField("1416") != string.Empty)
        {
          this.txtToBorrowerAddress.Text = this.loan.GetSimpleField("1416");
          this.txtToBorrowerCity.Text = this.loan.GetSimpleField("1417");
          this.txtToBorrowerState.Text = this.loan.GetSimpleField("1418");
          this.txtToBorrowerZip.Text = this.loan.GetSimpleField("1419");
        }
        else
        {
          this.txtToBorrowerAddress.Text = this.loan.GetSimpleField("FR0104");
          this.txtToBorrowerCity.Text = this.loan.GetSimpleField("FR0106");
          this.txtToBorrowerState.Text = this.loan.GetSimpleField("FR0107");
          this.txtToBorrowerZip.Text = this.loan.GetSimpleField("FR0108");
        }
        string str = (string) null;
        if (Modules.IsModuleAvailableForUser(EncompassModule.Fulfillment, false))
          str = Session.ConfigurationManager.GetCompanySetting("Fulfillment", "ServiceEnabled");
        if (str != "Y")
          this.hideFulfillmentSection();
        else if (Session.ConfigurationManager.GetCompanySetting("Fulfillment", "AutoFulfillment") == "Y")
          this.chkFulfillment.Checked = true;
      }
      bool flag1 = false;
      bool flag2 = false;
      if (this.packageType != eDeliveryMessageType.SendDocuments)
      {
        this.dateAcceptBy.Value = this.packageType != eDeliveryMessageType.InitialDisclosures ? DateTime.Today.AddDays(2.0) : Utils.ParseDate((object) this.loan.GetField("3143"), DateTime.Today.AddDays(1.0));
        foreach (DocumentAttachment attachment in this.attachmentList)
        {
          switch (attachment.SignatureType)
          {
            case "eSignable":
              flag1 = true;
              continue;
            case "Wet Sign Only":
              flag2 = true;
              continue;
            default:
              continue;
          }
        }
        if (Session.ConfigurationManager.GetEDisclosureSetup().AllowESigning(this.loan) & flag1)
        {
          if (flag2)
            this.cboSigningOption.Items.Add((object) "eSign + Wet Sign (for wet sign only documents)");
          else
            this.cboSigningOption.Items.Add((object) "eSign (electronically sign and return)");
        }
        if (flag1 | flag2)
          this.cboSigningOption.Items.Add((object) "Wet Sign (print, sign, and fax)");
        else
          this.cboSigningOption.Items.Add((object) "No Signature Required");
        this.cboSigningOption.SelectedIndex = 0;
      }
      else
        this.dateAcceptBy.Value = DateTime.Today.AddDays(2.0);
    }

    protected virtual void initTitle()
    {
      if (this.packageType == eDeliveryMessageType.InitialDisclosures)
      {
        if (this.htmlEmailTemplate != HtmlEmailTemplateType.ConsumerConnectPreClosing)
          this.htmlEmailTemplate = HtmlEmailTemplateType.ConsumerConnectInitialDisclosures;
        if (this.htmlEmailTemplate == HtmlEmailTemplateType.ConsumerConnectPreClosing)
        {
          this.Text = "Send Pre-Closing Documents";
          this.emailType = EmailManager.EmailType.PreClosing;
        }
        else
        {
          this.Text = "Send eDisclosures";
          this.emailType = EmailManager.EmailType.Disclosures;
        }
      }
      else if (this.packageType == eDeliveryMessageType.RequestDocuments)
      {
        this.htmlEmailTemplate = HtmlEmailTemplateType.ConsumerConnectRequestDocuments;
        this.Text = "Send Request";
        this.emailType = EmailManager.EmailType.Request;
      }
      else
      {
        if (this.packageType != eDeliveryMessageType.SendDocuments)
          return;
        this.htmlEmailTemplate = HtmlEmailTemplateType.ConsumerConnectSendDocuments;
        this.emailType = EmailManager.EmailType.Send;
      }
    }

    private void initHtmlControl()
    {
      this.htmlEmailControl.LoadText(string.Empty, false);
      this.htmlEmailControl.ShowFieldButton = false;
      this.htmlEmailControl.AllowPersonalImages = true;
    }

    private void initFrom()
    {
      for (int index = 1; index <= 3; ++index)
      {
        string text = (string) null;
        string userid = (string) null;
        string email = (string) null;
        string name = (string) null;
        switch (index)
        {
          case 1:
            EmailFrom.GetFromUser(this.loanDataMgr, "Current User", ref userid, ref email, ref name);
            text = "Current User";
            break;
          case 2:
            EmailFrom.GetFromUser(this.loanDataMgr, "File Starter", ref userid, ref email, ref name);
            text = "File Starter";
            break;
          case 3:
            EmailFrom.GetFromUser(this.loanDataMgr, "Loan Officer", ref userid, ref email, ref name);
            text = "Loan Officer";
            break;
        }
        if (!string.IsNullOrEmpty(userid))
          this.cmbSenderType.Items.Add((object) new FieldOption(text, text));
      }
      if (this.cmbSenderType.Items.Count <= 0)
        return;
      this.cmbSenderType.SelectedIndex = 0;
    }

    private void initSubject()
    {
      HtmlEmailTemplate[] htmlEmailTemplates = Session.ConfigurationManager.GetHtmlEmailTemplates((string) null, this.htmlEmailTemplate);
      if (htmlEmailTemplates != null)
        this.cboSubject.Items.AddRange((object[]) htmlEmailTemplates);
      string strB = Session.GetPrivateProfileString("CCEmailTemplates", this.emailType.ToString());
      if (string.IsNullOrEmpty(strB))
        strB = Session.ConfigurationManager.GetCompanySetting("DefaultCCEmailTemplates", this.emailType.ToString());
      foreach (HtmlEmailTemplate htmlEmailTemplate in this.cboSubject.Items)
      {
        if (string.Compare(htmlEmailTemplate.Guid, strB, true) == 0)
          this.cboSubject.SelectedItem = (object) htmlEmailTemplate;
      }
      if (this.cboSubject.SelectedItem != null || this.cboSubject.Items.Count <= 0)
        return;
      this.cboSubject.SelectedIndex = 0;
    }

    private eDeliveryMessage createMessage()
    {
      eDeliveryMessage message = this.msg != null ? this.msg : SendPackageFactory.CreateEDeliveryMessage(this.loanDataMgr, this.packageType);
      message.FromEmail = this.txtSenderEmail.Text;
      message.FromName = this.txtSenderName.Text;
      message.To = string.Empty;
      bool flag = this.Recipients != null;
      if (flag)
      {
        foreach (eDeliveryRecipient recipient in this.Recipients)
          message.To = !string.IsNullOrEmpty(message.To) ? message.To + ";" + recipient.EmailAddress : recipient.EmailAddress;
      }
      message.Subject = this.cboSubject.Text.Trim();
      message.Body = this.htmlEmailControl.Html;
      message.ReadReceipt = this.chkReadReceipt.Checked;
      message.Recipients = flag ? this.Recipients.ToArray() : (eDeliveryRecipient[]) null;
      message.NotificationDate = !this.chkAcceptBy.Checked ? DateTime.MinValue.Date : this.dateAcceptBy.Value;
      message.ElectronicSignature = this.cboSigningOption.Text.Equals("eSign (electronically sign and return)") || this.cboSigningOption.Text.Equals("eSign + Wet Sign (for wet sign only documents)");
      message.ClearRecipients();
      if (flag)
      {
        foreach (eDeliveryRecipient recipient in this.Recipients)
          message.AddRecipient(recipient);
      }
      if (this.chkFulfillment.Checked)
      {
        message.FulfillmentEnabled = true;
        message.FulfillmentSchedDate = this.dateFulfillBy.Value;
        message.FulfillmentFromName = this.txtFromName.Text;
        message.FulfillmentFromAddress = this.txtFromAddress.Text;
        message.FulfillmentFromCity = this.txtFromCity.Text;
        message.FulfillmentFromState = this.txtFromState.Text;
        message.FulfillmentFromZip = this.txtFromZip.Text;
        message.FulfillmentFromPhone = this.txtFromPhone.Text;
        message.FulfillmentToName = this.txtToBorrowerName.Text;
        message.FulfillmentToAddress = this.txtToBorrowerAddress.Text;
        message.FulfillmentToCity = this.txtToBorrowerCity.Text;
        message.FulfillmentToState = this.txtToBorrowerState.Text;
        message.FulfillmentToZip = this.txtToBorrowerZip.Text;
        message.FulfillmentToPhone = this.txtToBorrowerPhone.Text;
      }
      else
        message.FulfillmentEnabled = false;
      if (this.packageType != eDeliveryMessageType.SendDocuments)
      {
        message.ClearAttachments();
        message.AddCoversheet(this.coversheetFile);
      }
      if (this.attachmentList != null)
      {
        foreach (DocumentAttachment attachment in this.attachmentList)
          message.AddAttachment((eDeliveryAttachment) attachment);
      }
      if (this.neededList != null && this.neededList.Length != 0)
        message.AddNeeded(this.neededList);
      return message;
    }

    private bool validateMessage(eDeliveryMessage msg)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 691, nameof (validateMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
      if (msg.Subject == string.Empty)
        this.invalidList.Add("Subject is required.");
      if (this.chkAcceptBy.Checked && msg.NotificationDate.Date != DateTime.MinValue.Date && msg.NotificationDate.Date <= DateTime.Today)
        this.invalidList.Add("Notification date for Borrower accessing the package must be a future date");
      if (msg.FulfillmentEnabled)
      {
        foreach (eDeliveryAttachment attachment in msg.Attachments)
        {
          using (PdfEditor pdfEditor = new PdfEditor(attachment.Filepath))
          {
            if (!pdfEditor.ContainsLetterOnly)
            {
              this.invalidList.Add("Fulfillment service is not allowed for document sets with a mixture of both Letter and Legal sized pages.");
              break;
            }
          }
        }
        if (msg.FulfillmentSchedDate.Date <= DateTime.Today)
          this.invalidList.Add("Scheduled Fulfillment Date must be after today's date.");
        if (msg.FulfillmentFromName == string.Empty || msg.FulfillmentFromAddress == string.Empty || msg.FulfillmentFromCity == string.Empty || msg.FulfillmentFromState == string.Empty || msg.FulfillmentFromZip == string.Empty || msg.FulfillmentFromPhone == string.Empty)
          this.invalidList.Add("All of the 'From' info under fulfillment is required.");
        if (msg.FulfillmentFromName.Contains("\""))
          this.invalidList.Add("Fulfillment 'From Name' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentFromAddress.Contains("\""))
          this.invalidList.Add("Fulfillment 'From Address' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentFromCity.Contains("\""))
          this.invalidList.Add("Fulfillment 'From City' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentFromState.Contains("\""))
          this.invalidList.Add("Fulfillment 'From State' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentFromZip.Contains("\""))
          this.invalidList.Add("Fulfillment 'From Zip' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentFromPhone.Contains("\""))
          this.invalidList.Add("Fulfillment 'From Phone' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentFromAddress.Length > 50)
          this.invalidList.Add("Fulfillment 'From Street Address' cannot be more than 50 characters");
        string pattern1 = "^\\d{5}(?:[-\\s]\\d{4})?$";
        string pattern2 = "^[A-Za-z]{2}$";
        if (!Regex.Match(msg.FulfillmentFromZip, pattern1).Success)
          this.invalidList.Add("Fulfillment 'From Zip' is invalid. Zip code format should be  5 digits or five digits + four digits");
        else if (msg.FulfillmentFromZip.Contains("00000"))
          this.invalidList.Add("Fulfillment 'From Zip' is invalid.");
        if (!Regex.Match(msg.FulfillmentFromState, pattern2).Success)
          this.invalidList.Add("Fulfillment 'From State' is invalid. State should have 2 characters.");
        if (DateTime.Today.AddDays(120.0) < msg.FulfillmentSchedDate)
          this.invalidList.Add("Scheduled fulfillment date cannot be more than 120 days from today's date");
        if (msg.FulfillmentFromPhone.Length < 12)
          this.invalidList.Add("The 'From' phone number must be a 10-digit phone number.");
        if (msg.FulfillmentToName == string.Empty || msg.FulfillmentToAddress == string.Empty || msg.FulfillmentToCity == string.Empty || msg.FulfillmentToState == string.Empty || msg.FulfillmentToZip == string.Empty || msg.FulfillmentToPhone == string.Empty)
          this.invalidList.Add("All of the 'Borrower' info under fulfillment is required.");
        if (msg.FulfillmentToName.Contains("\""))
          this.invalidList.Add("Fulfillment 'Borrower Name' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentToAddress.Contains("\""))
          this.invalidList.Add("Fulfillment 'Borrower Address' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentToCity.Contains("\""))
          this.invalidList.Add("Fulfillment 'Borrower City' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentToState.Contains("\""))
          this.invalidList.Add("Fulfillment 'Borrower State' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentToZip.Contains("\""))
          this.invalidList.Add("Fulfillment 'Borrower Zip' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentToPhone.Contains("\""))
          this.invalidList.Add("Fulfillment 'Borrower Phone' cannot contain double quotes. (Single quotes are acceptable.)");
        if (msg.FulfillmentToAddress.Length > 50)
          this.invalidList.Add("Fulfillment 'Borrower Street Address' cannot be more than 50 characters");
        if (!Regex.Match(msg.FulfillmentToZip, pattern1).Success)
          this.invalidList.Add("Fulfillment 'Borrower Zip' is invalid. Zip code format should be  5 digits or five digits + four digits");
        else if (msg.FulfillmentToZip.Contains("00000"))
          this.invalidList.Add("Fulfillment 'Borrower Zip' is invalid.");
        if (!Regex.Match(msg.FulfillmentToState, pattern2).Success)
          this.invalidList.Add("Fulfillment 'Borrower State' is invalid. State should have 2 characters.");
        if (msg.FulfillmentToPhone.Length < 12)
          this.invalidList.Add("The Fufillment 'Borrower Phone Number' must be a 10-digit phone number.");
      }
      if (msg.Recipients.Length == 0)
        this.invalidList.Add("You cannot use the '" + this.cboSigningOption.Text + "' signing option because none of the selected documents have eSignature points.");
      if (msg.ElectronicSignature)
      {
        List<string> stringList1 = new List<string>();
        foreach (eDeliveryRecipient recipient in msg.Recipients)
        {
          string empty = string.Empty;
          string str1;
          if (recipient.FirstName != null && recipient.LastName != null)
          {
            str1 = recipient.FirstName.ToLower();
            if (recipient.MiddleName != string.Empty)
              str1 = str1 + " " + recipient.MiddleName.ToLower();
            if (recipient.LastName != string.Empty)
              str1 = str1 + " " + recipient.LastName.ToLower();
            if (recipient.SuffixName != string.Empty)
              str1 = str1 + " " + recipient.SuffixName.ToLower();
          }
          else
            str1 = recipient.UnparsedName.ToLower();
          string str2 = str1 + ":" + recipient.EmailAddress;
          if (!stringList1.Contains(str2))
            stringList1.Add(str2);
        }
        if (stringList1.Count != msg.Recipients.Length)
          this.invalidList.Add("You cannot use the '" + this.cboSigningOption.Text + "' signing option because two of your signers have the same name and email address.");
        List<string> stringList2 = new List<string>();
        if (string.IsNullOrEmpty(this.loan.GetSimpleField("3239")))
          stringList2.Add(eDeliveryEntityType.Originator.ToString("g"));
        if (string.IsNullOrEmpty(this.loan.GetSimpleField("4004")) && string.IsNullOrEmpty(this.loan.GetSimpleField("4006")) && string.IsNullOrEmpty(this.loan.GetSimpleField("1268")))
          stringList2.Add(eDeliveryEntityType.Coborrower.ToString("g"));
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        foreach (DocumentAttachment attachment in this.attachmentList)
        {
          if (attachment.SignatureType.ToLower() != "esignable")
          {
            flag5 = true;
            break;
          }
          if (attachment.SignatureFields != null)
          {
            foreach (eDeliveryRecipient recipient in msg.Recipients)
            {
              if (!flag1 && ((IEnumerable<string>) attachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(eDeliveryEntityType.Borrower.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null && this.requiredRecipients.Any<string>((Func<string, bool>) (s => s.Equals(eDeliveryEntityType.Borrower.ToString("g"), StringComparison.InvariantCultureIgnoreCase))))
                flag1 = true;
              if (!flag2 && ((IEnumerable<string>) attachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(eDeliveryEntityType.Coborrower.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null && this.requiredRecipients.Any<string>((Func<string, bool>) (s => s.Equals(eDeliveryEntityType.Coborrower.ToString("g"), StringComparison.InvariantCultureIgnoreCase))))
                flag2 = true;
              if (!flag3 && ((IEnumerable<string>) attachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(eDeliveryEntityType.Originator.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null && this.requiredRecipients.Any<string>((Func<string, bool>) (s => s.Equals(eDeliveryEntityType.Originator.ToString("g"), StringComparison.InvariantCultureIgnoreCase))))
                flag3 = true;
              if (!flag4 && ((IEnumerable<string>) attachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(eDeliveryEntityType.Other.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null && this.requiredRecipients.Any<string>((Func<string, bool>) (s => s.Equals(eDeliveryEntityType.Other.ToString("g"), StringComparison.InvariantCultureIgnoreCase))))
                flag4 = true;
            }
          }
        }
        if (!flag5)
        {
          foreach (eDeliveryRecipient recipient in msg.Recipients)
          {
            if (string.Compare(recipient.EntityType.ToString("g"), eDeliveryEntityType.Borrower.ToString("g"), true) == 0 && !flag1)
              this.invalidList.Add("There are no eSignable documents for the Borrower. Clear the check box for the Borrower before sending the request.");
            if (string.Compare(recipient.EntityType.ToString("g"), eDeliveryEntityType.Coborrower.ToString("g"), true) == 0 && !flag2)
              this.invalidList.Add("There are no eSignable documents for the Coborrower. Clear the check box for the Coborrower before sending the request.");
            if (string.Compare(recipient.EntityType.ToString("g"), eDeliveryEntityType.Originator.ToString("g"), true) == 0 && !flag3)
              this.invalidList.Add("There are no eSignable documents for the Originator. Clear the check box for the Originator before sending the request.");
            if (string.Compare(recipient.EntityType.ToString("g"), eDeliveryEntityType.Other.ToString("g"), true) == 0 && !flag4)
              this.invalidList.Add("There are no eSignable documents for the Other. Clear the check box for the Other before sending the request.");
          }
        }
      }
      if (this.invalidList.Count > 0)
      {
        string str = string.Empty;
        for (int index = 0; index < this.invalidList.Count; ++index)
          str = str + "\n\n(" + Convert.ToString(index + 1) + ") " + this.invalidList[index];
        PerformanceMeter.Current.AddCheckpoint("BEFORE Errors Dialog", 984, nameof (validateMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        int num = (int) Utils.Dialog((IWin32Window) this, "You must address the following issues before you can send the package:" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Errors Dialog", 987, nameof (validateMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        return false;
      }
      PerformanceMeter.Current.AddCheckpoint("END", 991, nameof (validateMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
      return true;
    }

    private void addComments(eDeliveryMessage msg)
    {
      string str1 = msg.ReadReceipt ? "Yes" : "No";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("From: " + msg.FromEmail);
      if (this.Recipients.Any<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.EntityType == eDeliveryEntityType.Borrower)))
        stringBuilder.AppendLine("Borrower: " + this.Recipients.FirstOrDefault<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.EntityType == eDeliveryEntityType.Borrower)).EmailAddress);
      if (this.Recipients.Any<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.EntityType == eDeliveryEntityType.Coborrower)))
        stringBuilder.AppendLine("Co-Borrower: " + this.Recipients.FirstOrDefault<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.EntityType == eDeliveryEntityType.Coborrower)).EmailAddress);
      if (this.Recipients.Any<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.EntityType == eDeliveryEntityType.NonBorrowingOwner)))
        stringBuilder.AppendLine("Non-Borrowing Owner: " + string.Join(", ", this.Recipients.FindAll((Predicate<eDeliveryRecipient>) (x => x.EntityType == eDeliveryEntityType.NonBorrowingOwner)).Select<eDeliveryRecipient, string>((Func<eDeliveryRecipient, string>) (y => y.EmailAddress)).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str)))));
      stringBuilder.AppendLine("Subject: " + msg.Subject);
      stringBuilder.AppendLine("Read Receipt: " + str1);
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(msg.Body);
      this.comments = stringBuilder.ToString();
    }

    private async void btnSend_Click(object sender, EventArgs e)
    {
      SendPackageDialog owner = this;
      using (PerformanceMeter.StartNew("SndPkgDlgSndBtnClk", "DOCS SendPackageDialog.SendButtonClick", 1032, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1034, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        owner.invalidList.Clear();
        if ((owner.cboSigningOption.Text.Equals("eSign (electronically sign and return)") || owner.cboSigningOption.Text.Equals("eSign + Wet Sign (for wet sign only documents)")) && (owner.requiredRecipients == null || owner.requiredRecipients.Count == 0))
        {
          string text = "The documents in this package have no borrower or co-borrower eSignature points. Add eSignature points to the documents or convert the package into a wet signing package. ";
          PerformanceMeter.Current.AddCheckpoint("Before EXIT ERROR Dialog", 1043, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
          int num = (int) Utils.Dialog((IWin32Window) owner, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          PerformanceMeter.Current.AddCheckpoint("Before EXIT ERROR Dialog", 1045, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
          PerformanceMeter.Current.AddNote("EXIT ERROR DIALOG - " + text);
          return;
        }
        if (!owner.GetSelectRecipients())
        {
          string text = "Please select recipient(s)";
          PerformanceMeter.Current.AddCheckpoint("Before EXIT ERROR Dialog", 1057, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
          int num = (int) Utils.Dialog((IWin32Window) owner, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          PerformanceMeter.Current.AddCheckpoint("Before EXIT ERROR Dialog", 1059, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
          PerformanceMeter.Current.AddNote("EXIT ERROR DIALOG - " + text);
          return;
        }
        owner.validateSenderContactDetails();
        owner.validateRecipients();
        if (!owner.validateMessage(owner.msg = owner.createMessage()) || !owner.checkSecondaryUse())
          return;
        string confirmationMessage = owner.checkAutoSigners(owner.msg);
        if (confirmationMessage == null)
          return;
        owner.saveSenderContactDetails();
        owner.saveRecipients();
        PerformanceMeter.Current.AddCheckpoint("Save Borrower Details", 1089, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        if (owner.loanDataMgr.SessionObjects.StartupInfo.OtpSupport)
        {
          if (!owner.loanDataMgr.Save())
          {
            int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Unable to Send because the loan could not be saved.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          PerformanceMeter.Current.AddCheckpoint("Save Loan", 1099, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        }
        SendPackageDialog sendPackageDialog = owner;
        using (eDeliveryClient client = SendPackageFactory.CreateEDeliveryClient(owner.loanDataMgr))
        {
          object obj = await client.ShowDialogAsync();
          client.cancellationTokenSource = new CancellationTokenSource();
          client.cancellationToken = client.cancellationTokenSource.Token;
          await Task.Run((Action) (() => client.SendMessage(sendPackageDialog.msg)), client.cancellationToken);
          if (string.IsNullOrWhiteSpace(client.packageID))
            return;
          owner.packageID = client.packageID;
          owner.OriginatorSignerUrl = client.OriginatorSignerUrl;
        }
        PerformanceMeter.Current.AddCheckpoint("Send Package", 1120, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        owner.msg.UpdatePackageIdInDocs(owner.packageID);
        owner.addComments(owner.msg);
        HtmlEmailTemplate selectedItem = owner.cboSubject.SelectedItem as HtmlEmailTemplate;
        if (selectedItem != (HtmlEmailTemplate) null)
          Session.WritePrivateProfileString("CCEmailTemplates", owner.emailType.ToString(), selectedItem.Guid);
        owner.msg.CreateLogEntry();
        if (owner.packageType != eDeliveryMessageType.SendDocuments && owner.shouldLODocuSignEmbeddedSigning(owner.msg, owner.OriginatorSignerUrl))
        {
          DocuSignSigningDialog form = new DocuSignSigningDialog(owner.OriginatorSignerUrl);
          owner.setWindowDisplay(form);
          PerformanceMeter.Current.AddCheckpoint("BEFORE browser.ShowDialog()", 1142, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
          int num = (int) form.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER browser.ShowDialog()", 1144, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
          if (owner.signOrderSetup != null && owner.signOrderSetup.States.Count > 0 && owner.packageType == eDeliveryMessageType.InitialDisclosures)
          {
            bool boolean = Convert.ToBoolean(owner.signOrderSetup.SignOrderEnabled);
            bool flag = false;
            owner.signOrderSetup.States.TryGetValue(owner.loanDataMgr.LoanData.GetField("14").ToUpper(), out flag);
            if (boolean & flag)
              confirmationMessage = "An email message has been sent to the Originator.";
          }
          PerformanceMeter.Current.AddCheckpoint("LO eSigning", 1160, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        }
        if (owner.packageType != eDeliveryMessageType.SendDocuments)
        {
          EBSServiceClient ebsServiceClient = new EBSServiceClient();
          EDeliveryConsentTrackingResponse result = new EDeliveryRestClient(owner.loanDataMgr).GetLoanLevelConsentTracking().Result;
          DateTime now;
          if (owner.Recipients.Exists((Predicate<eDeliveryRecipient>) (x => x.RecipientType == eDeliveryEntityType.Borrower.ToString("g"))) && string.IsNullOrEmpty(owner.loan.GetField("4108").Replace("//", string.Empty)))
          {
            string name = string.Join(" ", ((IEnumerable<string>) new string[4]
            {
              owner.loan.GetSimpleField("4000"),
              owner.loan.GetSimpleField("4001"),
              owner.loan.GetSimpleField("4002"),
              owner.loan.GetSimpleField("4003")
            }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
            string simpleField = owner.loan.GetSimpleField("1240");
            if (owner.isConsentPending(result, name, simpleField))
            {
              LoanData loan = owner.loan;
              now = DateTime.Now;
              string val = now.ToString("MM/dd/yyyy");
              loan.SetField("4108", val);
            }
          }
          if (owner.Recipients.Exists((Predicate<eDeliveryRecipient>) (x => x.RecipientType == eDeliveryEntityType.Coborrower.ToString("g"))) && string.IsNullOrEmpty(owner.loan.GetField("4109").Replace("//", string.Empty)))
          {
            string name = string.Join(" ", ((IEnumerable<string>) new string[4]
            {
              owner.loan.GetSimpleField("4004"),
              owner.loan.GetSimpleField("4005"),
              owner.loan.GetSimpleField("4006"),
              owner.loan.GetSimpleField("4007")
            }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
            string simpleField = owner.loan.GetSimpleField("1268");
            if (string.IsNullOrEmpty(simpleField))
              simpleField = owner.loan.GetSimpleField("1240");
            if (owner.isConsentPending(result, name, simpleField))
            {
              LoanData loan = owner.loan;
              now = DateTime.Now;
              string val = now.ToString("MM/dd/yyyy");
              loan.SetField("4109", val);
            }
          }
          if (owner.Recipients.Any<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.RecipientType == eDeliveryEntityType.NonBorrowingOwner.ToString("g"))))
          {
            foreach (eDeliveryRecipient nbo in owner.Recipients.FindAll((Predicate<eDeliveryRecipient>) (x => x.RecipientType == eDeliveryEntityType.NonBorrowingOwner.ToString("g"))))
            {
              if (string.IsNullOrEmpty(owner.loan.GetField("NBOC" + (object) nbo.NboIndex + "21").Replace("//", string.Empty)) && owner.isConsentPending(result, string.Empty, string.Empty, nbo))
              {
                LoanData loan = owner.loan;
                string id = "NBOC" + (nbo.NboIndex <= 99 ? nbo.NboIndex.ToString("00") : nbo.NboIndex.ToString("000")) + "21";
                now = DateTime.Now;
                string val = now.ToString("MM/dd/yyyy");
                loan.SetField(id, val);
              }
            }
          }
          PerformanceMeter.Current.AddCheckpoint("Got Consent Data", 1219, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        }
        owner.isWetSign = owner.cboSigningOption.SelectedItem != null && owner.cboSigningOption.SelectedItem.Equals((object) "Wet Sign (print, sign, and fax)");
        PerformanceMeter.Current.AddCheckpoint("Before Confirmation Dialog", 1229, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        confirmationMessage = owner.AddMessageForThirdPartyUsers(confirmationMessage);
        int num1 = (int) Utils.Dialog((IWin32Window) owner, confirmationMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        PerformanceMeter.Current.AddCheckpoint("After Confirmation Dialog", 1232, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        owner.DialogResult = DialogResult.OK;
        PerformanceMeter.Current.AddCheckpoint("END", 1235, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\SendPackageDialog.cs");
        confirmationMessage = (string) null;
      }
    }

    private bool isConsentPending(
      EDeliveryConsentTrackingResponse consentResponse,
      string name,
      string email,
      eDeliveryRecipient nbo = null)
    {
      bool flag = true;
      foreach (EDeliveryConsentDetail consentTrackingDetail in consentResponse.consentTrackingDetails)
      {
        EDeliveryConsentOutput edeliveryConsentOutput = nbo == null ? consentTrackingDetail.consentOutput.OrderByDescending<EDeliveryConsentOutput, DateTime>((Func<EDeliveryConsentOutput, DateTime>) (k => k.date)).FirstOrDefault<EDeliveryConsentOutput>() : consentTrackingDetail.consentOutput.ToList<EDeliveryConsentOutput>().FindAll((Predicate<EDeliveryConsentOutput>) (x => x.RecipientId == nbo.RecipientId)).OrderByDescending<EDeliveryConsentOutput, DateTime>((Func<EDeliveryConsentOutput, DateTime>) (k => k.date)).FirstOrDefault<EDeliveryConsentOutput>();
        if (edeliveryConsentOutput != null && !string.IsNullOrEmpty(edeliveryConsentOutput.fullName) && !string.IsNullOrEmpty(edeliveryConsentOutput.email) && edeliveryConsentOutput.fullName.Trim().ToLower() == name.Trim().ToLower() && edeliveryConsentOutput.email.Trim().ToLower() == email.Trim().ToLower() && edeliveryConsentOutput.status != null && edeliveryConsentOutput.status.ToLower() == "accepted")
          flag = false;
      }
      return flag;
    }

    protected virtual void setWindowDisplay(DocuSignSigningDialog form)
    {
      Screen screen = Screen.FromControl((Control) this);
      DocuSignSigningDialog signSigningDialog = form;
      Rectangle workingArea = screen.WorkingArea;
      int width = workingArea.Width;
      workingArea = screen.WorkingArea;
      int height = workingArea.Height;
      Size size = new Size(width, height);
      signSigningDialog.Size = size;
      form.Location = screen.WorkingArea.Location;
    }

    protected virtual bool GetSelectRecipients()
    {
      return this.pnlRecipients.Controls.OfType<RecipientControl>().Any<RecipientControl>((Func<RecipientControl, bool>) (ctrl => ctrl.cbSelect.Checked));
    }

    protected virtual List<ContactDetails> GetLoanContacts()
    {
      Task<List<ContactDetails>> loanContacts = new EBSServiceClient().GetLoanContacts(this.loan.GetField("GUID"));
      Task.WaitAll((Task) loanContacts);
      return loanContacts.Result;
    }

    private bool shouldLODocuSignEmbeddedSigning(eDeliveryMessage msg, string originatorSignerUrl)
    {
      if (!originatorSignerUrl.Contains("docusign"))
        return false;
      eDeliveryRecipient deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.EntityType == eDeliveryEntityType.Originator));
      if (deliveryRecipient == null)
        return false;
      foreach (eDeliveryRecipient recipient in msg.Recipients)
      {
        if (recipient == deliveryRecipient && Session.UserID.ToLower() == recipient.UserID.ToLower())
          return recipient.AutoSign;
      }
      return false;
    }

    private string checkAutoSigners(eDeliveryMessage msg)
    {
      string str1 = string.Join(", ", ((IEnumerable<eDeliveryRecipient>) msg.Recipients).Select<eDeliveryRecipient, string>((Func<eDeliveryRecipient, string>) (x => x.RecipientType)).Distinct<string>());
      if (this.packageType == eDeliveryMessageType.SendDocuments)
        return string.Format("Documents have been sent to the {0}.", (object) str1);
      eDeliveryRecipient deliveryRecipient = ((IEnumerable<eDeliveryRecipient>) msg.Recipients).ToList<eDeliveryRecipient>().Find((Predicate<eDeliveryRecipient>) (x => x.EntityType == eDeliveryEntityType.Originator));
      if (deliveryRecipient != null)
      {
        foreach (eDeliveryRecipient recipient in msg.Recipients)
        {
          if (recipient == deliveryRecipient)
          {
            string str2 = string.Empty;
            foreach (eDeliveryAttachment attachment in msg.Attachments)
            {
              if (attachment is DocumentAttachment && recipient.RequiresSigning((DocumentAttachment) attachment))
                str2 = str2 + attachment.Title + Environment.NewLine;
            }
            if (Session.UserID.ToLower() == recipient.UserID.ToLower())
            {
              switch (Utils.Dialog((IWin32Window) this, "This package includes one or more documents that you must eSign before you can retrieve the borrower-signed documents. It is strongly recommended that you sign prior to the borrowers. Click \"Yes\" to apply your eSignature to the documents now. Click \"No\" to receive an email with a link to a secure website where you can eSign later." + Environment.NewLine + Environment.NewLine + "Originator eSignable Documents:" + Environment.NewLine + Environment.NewLine + str2, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
              {
                case DialogResult.Cancel:
                  return (string) null;
                case DialogResult.Yes:
                  recipient.AutoSign = true;
                  continue;
                case DialogResult.No:
                  recipient.AutoSign = false;
                  bool flag1 = false;
                  if (this.signOrderSetup != null && this.signOrderSetup.States.Count > 0)
                  {
                    bool boolean = Convert.ToBoolean(this.signOrderSetup.SignOrderEnabled);
                    bool flag2 = false;
                    this.signOrderSetup.States.TryGetValue(this.loanDataMgr.LoanData.GetField("14").ToUpper(), out flag2);
                    if (boolean & flag2)
                      flag1 = true;
                  }
                  if (flag1)
                    return "An email message has been sent to the Originator.";
                  return this.packageType == eDeliveryMessageType.InitialDisclosures ? string.Format("The disclosure package has been sent to the {0}.", (object) str1) : string.Format("The request package has been sent to the {0}.", (object) str1);
                default:
                  continue;
              }
            }
            else
            {
              if (Utils.Dialog((IWin32Window) this, "This package includes one or more documents that require the Originator's eSignature. The originator, " + recipient.UnparsedName + ", will be notified via email." + Environment.NewLine + Environment.NewLine + "Originator eSignable Documents:" + Environment.NewLine + Environment.NewLine + str2, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return (string) null;
              return this.packageType == eDeliveryMessageType.InitialDisclosures ? string.Format("The disclosure package has been sent to the {0}.", (object) str1) : string.Format("The request package has been sent to the {0}.", (object) str1);
            }
          }
        }
      }
      return this.packageType == eDeliveryMessageType.InitialDisclosures ? string.Format("The disclosure package has been sent to the {0}.", (object) str1) : string.Format("The request package has been sent to the {0}.", (object) str1);
    }

    private bool checkSecondaryUse()
    {
      if (this.packageType != eDeliveryMessageType.SendDocuments || !LoanServiceManager.ShowEmailPrompt)
        return true;
      bool flag1 = false;
      foreach (DocumentAttachment documentAttachment in ((IEnumerable<eDeliveryAttachment>) this.msg.Attachments).ToList<eDeliveryAttachment>().FindAll((Predicate<eDeliveryAttachment>) (x => x is DocumentAttachment)))
      {
        if (documentAttachment.Document.Title == "Credit Report")
          flag1 = true;
      }
      if (!flag1)
        return true;
      LoanData loanData = this.msg.LoanDataMgr.LoanData;
      BorrowerPair currentBorrowerPair = loanData.CurrentBorrowerPair;
      bool flag2 = false;
      foreach (BorrowerPair borrowerPair in loanData.GetBorrowerPairs())
      {
        loanData.SetBorrowerPair(borrowerPair);
        string field1 = loanData.GetField("1240");
        string field2 = loanData.GetField("1268");
        string to = this.msg.To;
        char[] chArray = new char[1]{ ';' };
        foreach (string str in to.Split(chArray))
        {
          if (string.Compare(str.Trim(), field1.Trim(), true) != 0 && string.Compare(str.Trim(), field2.Trim(), true) != 0)
            flag2 = true;
        }
      }
      loanData.SetBorrowerPair(currentBorrowerPair);
      if (!flag2)
        return true;
      DialogResult dialogResult = DialogResult.None;
      using (SecondaryUseDialog secondaryUseDialog = new SecondaryUseDialog())
        dialogResult = secondaryUseDialog.ShowDialog((IWin32Window) this);
      return dialogResult.Equals((object) DialogResult.Yes);
    }

    private void cboSigningOption_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool eSigning = this.cboSigningOption.Text.Equals("eSign (electronically sign and return)") || this.cboSigningOption.Text.Equals("eSign + Wet Sign (for wet sign only documents)");
      this.refreshRecipients(eSigning);
      this.btnAddRecipient.Visible = !eSigning;
      if (eSigning)
        return;
      this.removeRecipient(eDeliveryEntityType.Originator.ToString("g"));
      if (this.packageType == eDeliveryMessageType.SendDocuments)
        return;
      this.addRecipient(this.populateBorrower());
      this.addRecipient(this.populateCoborrower());
      this.populateNonBorrowingOwner().ForEach((Action<eDeliveryRecipient>) (x => this.addRecipient(x)));
    }

    private void chkFulfillment_CheckedChanged(object sender, EventArgs e)
    {
      this.pnlFulfillment.Enabled = this.chkFulfillment.Checked;
    }

    private void chkAcceptBy_CheckedChanged(object sender, EventArgs e)
    {
      this.dateAcceptBy.Enabled = this.chkAcceptBy.Checked;
    }

    private void formatFieldValue(object sender, FieldFormat format)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void txtFromState_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.STATE);
    }

    private void txtFromZip_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.ZIPCODE);
    }

    private void txtFromPhone_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.PHONE);
    }

    private void txtToState_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.STATE);
    }

    private void txtToZip_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.ZIPCODE);
    }

    private void txtToPhone_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.PHONE);
    }

    private void cboSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
      HtmlEmailTemplate selectedItem1 = this.cboSubject.SelectedItem as HtmlEmailTemplate;
      if (selectedItem1 == (HtmlEmailTemplate) null)
        return;
      UserInfo userInfo = (UserInfo) null;
      if (this.cmbSenderType.SelectedItem is FieldOption selectedItem2)
        userInfo = !(selectedItem2.Value == "Current User") ? Session.OrganizationManager.GetUser(selectedItem2.ReportingDatabaseValue) : Session.UserInfo;
      HtmlFieldMerge htmlFieldMerge = new HtmlFieldMerge(selectedItem1.Html);
      if (this.packageType != eDeliveryMessageType.SendDocuments)
      {
        List<DocumentAttachment> documentAttachmentList1 = new List<DocumentAttachment>();
        List<DocumentAttachment> documentAttachmentList2 = new List<DocumentAttachment>();
        foreach (DocumentAttachment attachment in this.attachmentList)
        {
          if (attachment.SignatureType != "Informational")
            documentAttachmentList1.Add(attachment);
          else
            documentAttachmentList2.Add(attachment);
        }
        string str1 = string.Empty;
        if (documentAttachmentList1.Count > 0)
        {
          str1 = "Please sign and return the following documents:";
          foreach (DocumentAttachment documentAttachment in documentAttachmentList1)
            str1 = str1 + Environment.NewLine + "* " + documentAttachment.Title;
        }
        string str2 = string.Empty;
        if (this.neededList != null && this.neededList.Length != 0)
        {
          str2 = "Please send the following documents:";
          foreach (DocumentLog needed in this.neededList)
            str2 = str2 + Environment.NewLine + "* " + needed.Title;
        }
        string str3 = string.Empty;
        if (documentAttachmentList2.Count > 0)
        {
          str3 = "Please review the following documents (no need to return):";
          foreach (DocumentAttachment documentAttachment in documentAttachmentList2)
            str3 = str3 + Environment.NewLine + "* " + documentAttachment.Title;
        }
        htmlFieldMerge.InformationalList = str3;
        htmlFieldMerge.SignAndReturnList = str1;
        htmlFieldMerge.NeededList = str2;
      }
      string html = htmlFieldMerge.MergeContent(this.loanDataMgr.LoanData, userInfo);
      if (string.IsNullOrEmpty(html))
        return;
      this.htmlEmailControl.LoadHtml(html, false);
    }

    private void btnNotifyUsers_Click(object sender, EventArgs e)
    {
      LoanDisplayInfo loanDisplayInfo = new LoanDisplayInfo()
      {
        LoanGuid = new Guid(this.loan.GUID)
      };
      using (NotifyUsersDialog notifyUsersDialog = new NotifyUsersDialog(new List<LoanDisplayInfo>()
      {
        loanDisplayInfo
      }))
      {
        int num = (int) notifyUsersDialog.ShowDialog((IWin32Window) this);
        if (!notifyUsersDialog.IsModified)
          return;
        this.initNotifyCount();
      }
    }

    protected virtual void hideFulfillmentSection()
    {
      this.gcMessage.Width = this.gcFulfillment.Right - this.gcMessage.Left;
      this.gcSigning.Width = this.gcMessage.Width;
      this.gcFulfillment.Visible = false;
      this.Width = 605;
    }

    private void btnAddRecipient_Click(object sender, EventArgs e)
    {
      using (EmailListDialog emailListDialog = SendPackageFactory.CreateEmailListDialog(this.loanDataMgr, this.contacts, this.packageType == eDeliveryMessageType.SendDocuments))
      {
        DialogResult dialogResult = emailListDialog.ShowDialog((IWin32Window) this);
        if (emailListDialog.contacts.Exists((Predicate<string>) (x => ((IEnumerable<string>) x.Split(';')).Last<string>() == "1")))
          this.contacts = this.GetLoanContacts();
        if (dialogResult == DialogResult.Cancel)
          return;
        this.SetSelectedRecipients(emailListDialog);
      }
    }

    private void cmbSenderType_SelectedIndexChanged(object sender, EventArgs e)
    {
      string userType = (this.cmbSenderType.SelectedItem as FieldOption).Value;
      this.txtSenderEmail.ReadOnly = true;
      this.txtSenderName.ReadOnly = true;
      this.txtSenderEmail.Text = string.Empty;
      this.txtSenderName.Text = string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      EmailFrom.GetFromUser(this.loanDataMgr, userType, ref empty1, ref empty2, ref empty3);
      if (!string.IsNullOrEmpty(empty2))
        this.txtSenderEmail.Text = empty2;
      else
        this.txtSenderEmail.ReadOnly = false;
      if (!string.IsNullOrEmpty(empty3))
        this.txtSenderName.Text = empty3;
      else
        this.txtSenderName.ReadOnly = false;
    }

    protected virtual void addRecipient(eDeliveryRecipient recipient)
    {
      if (recipient == null)
        return;
      if (this.contacts != null)
      {
        ContactDetails contactDetails = recipient.RecipientType == eDeliveryEntityType.Borrower.ToString("g") || recipient.RecipientType == eDeliveryEntityType.Coborrower.ToString("g") ? this.contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == recipient.BorrowerId)) : (!(recipient.RecipientType.ToUpper() == eDeliveryEntityType.NonBorrowingOwner.ToString("g").ToUpper()) ? this.contacts.Find((Predicate<ContactDetails>) (x => x.contactType.ToUpper() == recipient.RecipientType.ToUpper() && x.name == (!string.IsNullOrEmpty(recipient.UnparsedName) ? recipient.UnparsedName : recipient.FirstName + " " + recipient.LastName) && x.email.ToUpper() == recipient.EmailAddress.ToUpper())) : this.contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == recipient.BorrowerId)));
        if (contactDetails != null)
        {
          recipient.AuthCode = contactDetails.authCode;
          recipient.RecipientId = contactDetails.recipientId;
        }
        else if (recipient.RecipientId == null)
        {
          eDeliveryRecipient deliveryRecipient = (eDeliveryRecipient) null;
          if (this.Recipients != null)
            deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientType.ToUpper() == recipient.RecipientType.ToUpper() && x.UnparsedName == (!string.IsNullOrEmpty(recipient.UnparsedName) ? recipient.UnparsedName : recipient.FirstName + " " + recipient.LastName) && x.EmailAddress.ToUpper() == recipient.EmailAddress.ToUpper()));
          recipient.RecipientId = deliveryRecipient == null ? (recipient.RecipientId == null ? Guid.NewGuid().ToString() : recipient.RecipientId) : deliveryRecipient.RecipientId;
        }
      }
      else
      {
        eDeliveryRecipient deliveryRecipient = (eDeliveryRecipient) null;
        if (this.Recipients != null)
          deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientType.ToUpper() == recipient.RecipientType.ToUpper() && x.UnparsedName.Trim() == (!string.IsNullOrEmpty(recipient.UnparsedName) ? recipient.UnparsedName.Trim() : (recipient.FirstName + " " + recipient.LastName).Trim()) && x.EmailAddress.ToUpper() == recipient.EmailAddress.ToUpper()));
        recipient.RecipientId = deliveryRecipient == null ? (recipient.RecipientId == null ? Guid.NewGuid().ToString() : recipient.RecipientId) : deliveryRecipient.RecipientId;
      }
      if (this.Recipients == null)
        this.Recipients = new List<eDeliveryRecipient>();
      if (!this.Recipients.Exists((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipient.RecipientId)))
        this.Recipients.Add(recipient);
      if (((IEnumerable<Control>) this.pnlRecipients.Controls.Find(recipient.RecipientId, true)).FirstOrDefault<Control>() != null)
        return;
      RecipientControl recipientControl = new RecipientControl();
      recipientControl.Name = recipient.RecipientId;
      if (!string.IsNullOrEmpty(recipient.UnparsedName))
      {
        recipientControl.txtName.Text = recipient.UnparsedName;
        recipientControl.txtName.ReadOnly = true;
      }
      else if (!string.IsNullOrEmpty(recipient.FirstName) || !string.IsNullOrEmpty(recipient.LastName))
      {
        recipientControl.txtName.Text = recipient.FirstName + " " + recipient.LastName;
        recipientControl.txtName.ReadOnly = true;
      }
      else
        recipientControl.txtName.ReadOnly = false;
      if (!string.IsNullOrEmpty(recipient.EmailAddress))
      {
        recipientControl.txtEmail.Text = recipient.EmailAddress;
        recipientControl.txtEmail.ReadOnly = true;
      }
      else
        recipientControl.txtEmail.ReadOnly = false;
      if (recipient.RecipientType == eDeliveryEntityType.Originator.ToString("g"))
        recipientControl.txtAuthenticationCode.Visible = false;
      else if (!string.IsNullOrEmpty(recipient.AuthCode))
      {
        recipientControl.txtAuthenticationCode.Text = recipient.AuthCode;
        recipientControl.txtAuthenticationCode.ReadOnly = true;
      }
      else
        recipientControl.txtAuthenticationCode.ReadOnly = false;
      recipientControl.cbSelect.Text = recipient.RecipientType == eDeliveryEntityType.NonBorrowingOwner.ToString("g") ? "Non-Borrowing Owner" : recipient.RecipientType;
      recipientControl.Tag = (object) recipient.NboIndex;
      this.pnlRecipients.Controls.Add((Control) recipientControl);
    }

    private void addRecipient(string recipientType)
    {
      if (this.deletedRecipients != null)
      {
        foreach (eDeliveryRecipient deliveryRecipient in this.deletedRecipients.FindAll((Predicate<eDeliveryRecipient>) (x => x.RecipientType == recipientType)))
        {
          eDeliveryRecipient deletedRecipient = deliveryRecipient;
          if (this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == deletedRecipient.RecipientId)) == null)
            this.Recipients.Add(deletedRecipient);
          this.addRecipient(deletedRecipient);
        }
      }
      else
      {
        foreach (eDeliveryRecipient recipient in this.Recipients.FindAll((Predicate<eDeliveryRecipient>) (x => x.RecipientType == recipientType)))
          this.addRecipient(recipient);
      }
    }

    protected virtual int removeRecipient(string recipientType)
    {
      int num = 0;
      if (this.Recipients != null)
      {
        foreach (eDeliveryRecipient deliveryRecipient in this.Recipients.FindAll((Predicate<eDeliveryRecipient>) (x => x.RecipientType == recipientType)))
        {
          Control control = ((IEnumerable<Control>) this.pnlRecipients.Controls.Find(deliveryRecipient.RecipientId, true)).FirstOrDefault<Control>();
          if (control != null)
          {
            this.pnlRecipients.Controls.Remove(control);
            if (this.deletedRecipients == null)
              this.deletedRecipients = new List<eDeliveryRecipient>();
            this.deletedRecipients.Add(deliveryRecipient);
            this.Recipients.Remove(deliveryRecipient);
            ++num;
          }
        }
      }
      return num;
    }

    private void refreshRecipients(bool eSigning)
    {
      if (eSigning)
      {
        foreach (string requiredRecipient in this.requiredRecipients)
          this.addRecipient(requiredRecipient);
        if (this.Recipients != null && this.Recipients.Count != 0)
        {
          for (int index = this.Recipients.Count - 1; index >= 0; --index)
          {
            if (!this.requiredRecipients.Contains(this.Recipients[index].RecipientType))
            {
              int num = this.removeRecipient(this.Recipients[index].RecipientType);
              index -= num <= 1 ? 0 : num - 1;
            }
          }
        }
        if (this.Recipients != null && this.Recipients.Count != 0)
          return;
        this.pnlRecipients.Controls.Clear();
      }
      else
      {
        if (this.deletedRecipients == null)
          return;
        foreach (eDeliveryRecipient recipient in this.deletedRecipients.FindAll((Predicate<eDeliveryRecipient>) (x => x.Ischecked)))
          this.addRecipient(recipient);
      }
    }

    private List<string> getRequiredRecipients()
    {
      this.msg = SendPackageFactory.CreateEDeliveryMessage(this.loanDataMgr, this.packageType);
      List<string> requiredRecipients = new List<string>();
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      foreach (DocumentAttachment attachment in this.attachmentList)
      {
        if (this.msg.checkSigningpointExists(attachment, eDeliveryEntityType.Borrower.ToString("g")) && !flag1)
        {
          flag1 = true;
          requiredRecipients.Add(eDeliveryEntityType.Borrower.ToString("g"));
        }
        if (this.msg.checkSigningpointExists(attachment, eDeliveryEntityType.Coborrower.ToString("g")) && !flag2)
        {
          flag2 = true;
          requiredRecipients.Add(eDeliveryEntityType.Coborrower.ToString("g"));
        }
        if (this.msg.checkSigningpointExists(attachment, eDeliveryEntityType.NonBorrowingOwner.ToString("g")) && !flag3)
        {
          flag3 = true;
          requiredRecipients.Add(eDeliveryEntityType.NonBorrowingOwner.ToString("g"));
        }
        if (flag1 & flag2 & flag3)
          break;
      }
      foreach (DocumentAttachment documentAttachment in this.attachmentList.FindAll((Predicate<DocumentAttachment>) (x => x.SignatureFields != null)))
      {
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(eDeliveryEntityType.Borrower.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null)
          requiredRecipients.Add(eDeliveryEntityType.Borrower.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(eDeliveryEntityType.Coborrower.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null)
          requiredRecipients.Add(eDeliveryEntityType.Coborrower.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(eDeliveryEntityType.NonBorrowingOwner.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null)
          requiredRecipients.Add(eDeliveryEntityType.NonBorrowingOwner.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(eDeliveryEntityType.Originator.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null)
          requiredRecipients.Add(eDeliveryEntityType.Originator.ToString("g"));
      }
      return requiredRecipients;
    }

    protected virtual eDeliveryRecipient populateBorrower()
    {
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      eDeliveryRecipient deliveryRecipient = new eDeliveryRecipient("1", eDeliveryEntityType.Borrower)
      {
        FirstName = this.loan.GetSimpleField("4000"),
        MiddleName = this.loan.GetSimpleField("4001"),
        LastName = this.loan.GetSimpleField("4002"),
        SuffixName = this.loan.GetSimpleField("4003")
      };
      deliveryRecipient.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
      {
        deliveryRecipient.FirstName,
        deliveryRecipient.MiddleName,
        deliveryRecipient.LastName,
        deliveryRecipient.SuffixName
      }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      deliveryRecipient.EmailAddress = this.loan.GetSimpleField("1240");
      deliveryRecipient.SignatureField = "BorrowerSignature_" + pairIndex.ToString();
      deliveryRecipient.InitialsField = "BorrowerInitials_" + pairIndex.ToString();
      deliveryRecipient.CheckboxField = "BorrowerCheckbox_" + pairIndex.ToString();
      deliveryRecipient.RecipientType = "Borrower";
      deliveryRecipient.BorrowerId = Session.LoanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id;
      return deliveryRecipient;
    }

    protected virtual eDeliveryRecipient populateCoborrower()
    {
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      string simpleField1 = this.loan.GetSimpleField("4004");
      string simpleField2 = this.loan.GetSimpleField("1268");
      if (string.IsNullOrEmpty(simpleField1) && string.IsNullOrEmpty(simpleField2))
        return (eDeliveryRecipient) null;
      eDeliveryRecipient deliveryRecipient = new eDeliveryRecipient("2", eDeliveryEntityType.Coborrower)
      {
        FirstName = simpleField1,
        MiddleName = this.loan.GetSimpleField("4005"),
        LastName = this.loan.GetSimpleField("4006"),
        SuffixName = this.loan.GetSimpleField("4007")
      };
      deliveryRecipient.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
      {
        deliveryRecipient.FirstName,
        deliveryRecipient.MiddleName,
        deliveryRecipient.LastName,
        deliveryRecipient.SuffixName
      }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      deliveryRecipient.EmailAddress = simpleField2;
      deliveryRecipient.SignatureField = "CoborrowerSignature_" + pairIndex.ToString();
      deliveryRecipient.InitialsField = "CoborrowerInitials_" + pairIndex.ToString();
      deliveryRecipient.CheckboxField = "CoborrowerCheckbox_" + pairIndex.ToString();
      deliveryRecipient.RecipientType = "Coborrower";
      deliveryRecipient.BorrowerId = Session.LoanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id;
      return deliveryRecipient;
    }

    protected virtual eDeliveryRecipient populateOriginator(string originatorUserID)
    {
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      UserInfo user = Session.OrganizationManager.GetUser(originatorUserID);
      if (!(user != (UserInfo) null))
        return (eDeliveryRecipient) null;
      if (this.loan.GetSimpleField("1612") == string.Empty)
      {
        string fullName = user.FullName;
      }
      eDeliveryRecipient deliveryRecipient = new eDeliveryRecipient("3", eDeliveryEntityType.Originator)
      {
        FirstName = user.FirstName,
        MiddleName = user.MiddleName,
        SuffixName = user.SuffixName,
        LastName = user.LastName
      };
      deliveryRecipient.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
      {
        deliveryRecipient.FirstName,
        deliveryRecipient.MiddleName,
        deliveryRecipient.LastName,
        deliveryRecipient.SuffixName
      }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      deliveryRecipient.EmailAddress = user.Email;
      deliveryRecipient.UserID = originatorUserID;
      deliveryRecipient.RecipientId = originatorUserID;
      deliveryRecipient.SignatureField = "OriginatorSignature_" + pairIndex.ToString();
      deliveryRecipient.InitialsField = "OriginatorInitials_" + pairIndex.ToString();
      deliveryRecipient.RecipientType = "Originator";
      return deliveryRecipient;
    }

    protected virtual List<eDeliveryRecipient> populateNonBorrowingOwner()
    {
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      List<eDeliveryRecipient> deliveryRecipientList = new List<eDeliveryRecipient>();
      List<NonBorrowingOwner> byBorrowerPairId = this.loan.GetNboByBorrowerPairId(this.loan.GetBorrowerPairs()[pairIndex].Id);
      int num = 0;
      foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
      {
        eDeliveryRecipient deliveryRecipient = new eDeliveryRecipient("4", eDeliveryEntityType.NonBorrowingOwner)
        {
          FirstName = nonBorrowingOwner.FirstName,
          MiddleName = nonBorrowingOwner.MiddleName,
          LastName = nonBorrowingOwner.LastName,
          SuffixName = nonBorrowingOwner.SuffixName
        };
        deliveryRecipient.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          deliveryRecipient.FirstName,
          deliveryRecipient.MiddleName,
          deliveryRecipient.LastName,
          deliveryRecipient.SuffixName
        }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
        deliveryRecipient.EmailAddress = nonBorrowingOwner.EmailAddress;
        deliveryRecipient.SignatureField = "NonBorrowingOwnerSignature_" + pairIndex.ToString() + "_" + num.ToString();
        deliveryRecipient.InitialsField = "NonBorrowingOwnerInitials_" + pairIndex.ToString() + "_" + num.ToString();
        deliveryRecipient.CheckboxField = "NonBorrowingOwnerCheckbox_" + pairIndex.ToString() + "_" + num.ToString();
        deliveryRecipient.RecipientType = eDeliveryEntityType.NonBorrowingOwner.ToString("g");
        deliveryRecipient.BorrowerId = nonBorrowingOwner.NBOID;
        deliveryRecipient.NboIndex = nonBorrowingOwner.NboIndex;
        ++num;
        deliveryRecipientList.Add(deliveryRecipient);
      }
      return deliveryRecipientList;
    }

    protected virtual void saveRecipients()
    {
      foreach (Control control in (ArrangedElementCollection) this.pnlRecipients.Controls)
      {
        if (control != null && control.GetType() == typeof (RecipientControl))
        {
          RecipientControl recipientcontrol = control as RecipientControl;
          if (recipientcontrol.cbSelect.Checked)
          {
            Dictionary<string, string> contactDetails = new Dictionary<string, string>();
            if (!recipientcontrol.txtName.ReadOnly)
              contactDetails.Add("Name", recipientcontrol.txtName.Text);
            if (!recipientcontrol.txtEmail.ReadOnly)
              contactDetails.Add("Email", recipientcontrol.txtEmail.Text);
            if (recipientcontrol.cbSelect.Text == "Non-Borrowing Owner" && (!recipientcontrol.txtName.ReadOnly || !recipientcontrol.txtEmail.ReadOnly))
              contactDetails.Add("NboIndex", recipientcontrol.Tag.ToString());
            if (contactDetails.Count > 0)
            {
              contactDetails.Add("ContactType", recipientcontrol.cbSelect.Text);
              eDeliveryRecipient deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name));
              if (contactDetails.ContainsKey("Email"))
                deliveryRecipient.EmailAddress = contactDetails["Email"];
              if (contactDetails.ContainsKey("Name"))
                deliveryRecipient.UnparsedName = contactDetails["Name"];
              contactDetails.Add("BorrowerId", deliveryRecipient.BorrowerId);
              SendPackageFactory.CreateFileContact(this.loan).UpdateContactDetails(contactDetails);
            }
          }
        }
      }
    }

    private void saveSenderContactDetails()
    {
      Dictionary<string, string> contactDetails = new Dictionary<string, string>();
      if (!this.txtSenderName.ReadOnly)
        contactDetails.Add("Name", this.txtSenderName.Text);
      if (!this.txtSenderEmail.ReadOnly)
        contactDetails.Add("Email", this.txtSenderEmail.Text);
      if (contactDetails.Count <= 0)
        return;
      contactDetails.Add("SenderType", (this.cmbSenderType.SelectedItem as FieldOption).Value);
      EmailFrom.SaveFromUser(this.loanDataMgr, contactDetails);
    }

    protected virtual void validateRecipients()
    {
      string htmlBodyText = this.htmlEmailControl.HtmlBodyText;
      string text = this.cboSubject.Text;
      Dictionary<string, string> source = new Dictionary<string, string>();
      int num = 0;
      foreach (Control control in (ArrangedElementCollection) this.pnlRecipients.Controls)
      {
        if (control != null && control.GetType() == typeof (RecipientControl))
        {
          RecipientControl recipientcontrol = control as RecipientControl;
          if (recipientcontrol.cbSelect.Checked)
          {
            eDeliveryRecipient deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name));
            if (deliveryRecipient == null)
            {
              deliveryRecipient = this.deletedRecipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name));
              this.Recipients.Add(deliveryRecipient);
              this.deletedRecipients.Remove(deliveryRecipient);
              deliveryRecipient.Ischecked = true;
            }
            if (!recipientcontrol.txtName.ReadOnly && string.IsNullOrWhiteSpace(recipientcontrol.txtName.Text))
              this.invalidList.Add(recipientcontrol.cbSelect.Text + "'s Name is Required");
            if (!recipientcontrol.txtEmail.ReadOnly && string.IsNullOrWhiteSpace(recipientcontrol.txtEmail.Text))
              this.invalidList.Add(recipientcontrol.cbSelect.Text + "'s Email is Required");
            else if (!recipientcontrol.txtEmail.ReadOnly && !Utils.ValidateEmail(recipientcontrol.txtEmail.Text))
              this.invalidList.Add("Enter valid value for " + recipientcontrol.cbSelect.Text + "'s Email");
            if (!recipientcontrol.txtAuthenticationCode.ReadOnly && string.IsNullOrWhiteSpace(recipientcontrol.txtAuthenticationCode.Text))
              this.invalidList.Add(recipientcontrol.cbSelect.Text + "'s Authentication Code is Required");
            else if (!recipientcontrol.txtAuthenticationCode.ReadOnly && !this.ValidateAuthenticationCode(recipientcontrol.txtAuthenticationCode.Text))
              this.invalidList.Add("Enter valid value for " + recipientcontrol.cbSelect.Text + "'s Authentication Code.Minimum 4 and Maximum 10 numbers required. ");
            else if (!recipientcontrol.txtAuthenticationCode.ReadOnly)
            {
              deliveryRecipient.AuthCode = recipientcontrol.txtAuthenticationCode.Text;
              source.Add(recipientcontrol.cbSelect.Text + num.ToString(), deliveryRecipient.AuthCode);
            }
            ++num;
          }
          else
          {
            eDeliveryRecipient deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name));
            if (deliveryRecipient != null)
            {
              if (this.deletedRecipients == null)
                this.deletedRecipients = new List<eDeliveryRecipient>();
              this.deletedRecipients.Add(deliveryRecipient);
              this.Recipients.Remove(deliveryRecipient);
              deliveryRecipient.Ischecked = false;
            }
          }
          if (htmlBodyText != null && (recipientcontrol.txtAuthenticationCode.ReadOnly || recipientcontrol.cbSelect.Checked) && !string.IsNullOrEmpty(recipientcontrol.txtAuthenticationCode.Text) && htmlBodyText.Contains(recipientcontrol.txtAuthenticationCode.Text))
            this.invalidList.Add("Message may not contain the " + recipientcontrol.cbSelect.Text + " Authorization Code used for signing.");
          if (text != null && (recipientcontrol.txtAuthenticationCode.ReadOnly || recipientcontrol.cbSelect.Checked) && !string.IsNullOrEmpty(recipientcontrol.txtAuthenticationCode.Text) && text.Contains(recipientcontrol.txtAuthenticationCode.Text))
            this.invalidList.Add("Subject may not contain the " + recipientcontrol.cbSelect.Text + " Authorization Code used for signing.");
        }
      }
      if (source.GroupBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (x => x.Value)).Where<IGrouping<string, KeyValuePair<string, string>>>((Func<IGrouping<string, KeyValuePair<string, string>>, bool>) (x => x.Count<KeyValuePair<string, string>>() > 1)).Count<IGrouping<string, KeyValuePair<string, string>>>() <= 0)
        return;
      this.invalidList.Add("Authorization Code must be unique for each Recipient. Please enter unique Authentication Code.");
    }

    protected virtual void SetSelectedRecipients(EmailListDialog dialog)
    {
      foreach (string selectedContact in dialog.selectedContacts)
      {
        char[] chArray = new char[1]{ ';' };
        string[] strArray = selectedContact.Split(chArray);
        eDeliveryRecipient recipient;
        if (strArray[2] == "Borrower")
        {
          if (strArray[3] == this.loan.CurrentBorrowerPair.Borrower.Id)
          {
            recipient = this.populateBorrower();
          }
          else
          {
            recipient = new eDeliveryRecipient("1", eDeliveryEntityType.Borrower);
            recipient.UnparsedName = strArray[0];
            recipient.EmailAddress = strArray[1];
            recipient.RecipientType = strArray[2];
            recipient.BorrowerId = strArray[3];
          }
        }
        else if (strArray[2] == "Coborrower")
        {
          if (strArray[3] == this.loan.CurrentBorrowerPair.CoBorrower.Id)
          {
            recipient = this.populateCoborrower();
          }
          else
          {
            recipient = new eDeliveryRecipient("2", eDeliveryEntityType.Coborrower);
            recipient.UnparsedName = strArray[0];
            recipient.EmailAddress = strArray[1];
            recipient.RecipientType = strArray[2];
            recipient.BorrowerId = strArray[3];
          }
        }
        else
        {
          recipient = new eDeliveryRecipient("0", eDeliveryEntityType.Other);
          recipient.UnparsedName = strArray[0];
          recipient.EmailAddress = strArray[1];
          recipient.RecipientType = strArray[2];
        }
        this.addRecipient(recipient);
      }
      int num = 0;
      foreach (NonBorrowingOwner nonBorrowingOwner in dialog.selectedNbo)
      {
        eDeliveryRecipient recipient = new eDeliveryRecipient("4", eDeliveryEntityType.NonBorrowingOwner)
        {
          FirstName = nonBorrowingOwner.FirstName,
          MiddleName = nonBorrowingOwner.MiddleName,
          LastName = nonBorrowingOwner.LastName,
          SuffixName = nonBorrowingOwner.SuffixName
        };
        recipient.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          recipient.FirstName,
          recipient.MiddleName,
          recipient.LastName,
          recipient.SuffixName
        }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
        recipient.EmailAddress = nonBorrowingOwner.EmailAddress;
        recipient.RecipientType = eDeliveryEntityType.NonBorrowingOwner.ToString("g");
        recipient.BorrowerId = nonBorrowingOwner.NBOID;
        recipient.NboIndex = nonBorrowingOwner.NboIndex;
        ++num;
        this.addRecipient(recipient);
      }
    }

    private bool ValidateAuthenticationCode(string authenticationCode)
    {
      return authenticationCode.Length >= 4 && authenticationCode.Length <= 10 && int.TryParse(authenticationCode, out int _) && authenticationCode.All<char>(new Func<char, bool>(char.IsDigit));
    }

    private void validateSenderContactDetails()
    {
      if (!this.txtSenderName.ReadOnly && string.IsNullOrWhiteSpace(this.txtSenderName.Text))
        this.invalidList.Add("Sender's Name is Required");
      if (!this.txtSenderEmail.ReadOnly && string.IsNullOrWhiteSpace(this.txtSenderEmail.Text))
      {
        this.invalidList.Add("Sender's Email is Required");
      }
      else
      {
        if (this.txtSenderEmail.ReadOnly || Utils.ValidateEmail(this.txtSenderEmail.Text))
          return;
        this.invalidList.Add("Enter valid value for Sender's Email");
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    protected virtual string AddMessageForThirdPartyUsers(string confirmationMessage)
    {
      return confirmationMessage;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblRequired = new Label();
      this.btnCancel = new Button();
      this.btnSend = new Button();
      this.gcSigning = new GroupContainer();
      this.cboSigningOption = new ComboBox();
      this.lblSigningOption = new Label();
      this.gcFulfillment = new GroupContainer();
      this.emHelpLink = new EMHelpLink();
      this.chkFulfillment = new CheckBox();
      this.pnlFulfillment = new Panel();
      this.dateFulfillBy = new DateTimePicker();
      this.lblFulfillment = new Label();
      this.lblGfeDate = new Label();
      this.lblFulfillBy = new Label();
      this.lblMailFrom = new Label();
      this.txtFromName = new TextBox();
      this.lblFromName = new Label();
      this.txtGfeDate = new TextBox();
      this.txtFromAddress = new TextBox();
      this.lblFromAddress1 = new Label();
      this.txtToBorrowerZip = new TextBox();
      this.txtFromCity = new TextBox();
      this.txtToBorrowerState = new TextBox();
      this.lblFromAddress2 = new Label();
      this.lblToBorrowerPhone = new Label();
      this.txtFromPhone = new TextBox();
      this.txtToBorrowerPhone = new TextBox();
      this.lblFromPhone = new Label();
      this.lblToBorrowerAddress2 = new Label();
      this.txtFromState = new TextBox();
      this.txtToBorrowerCity = new TextBox();
      this.txtFromZip = new TextBox();
      this.lblToBorrowerAddress = new Label();
      this.lblMailToBorrower = new Label();
      this.txtToBorrowerAddress = new TextBox();
      this.txtToBorrowerName = new TextBox();
      this.lblToBorrowerName = new Label();
      this.panel1 = new Panel();
      this.lblSenderEmail = new Label();
      this.lblSenderName = new Label();
      this.lblSenderType = new Label();
      this.gcMessage = new GroupContainer();
      this.htmlEmailControl = new HtmlEmailControl();
      this.btnNotifyUsers = new Button();
      this.lblNotifyUserCount = new Label();
      this.dateAcceptBy = new DateTimePicker();
      this.chkAcceptBy = new CheckBox();
      this.chkReadReceipt = new CheckBox();
      this.cboSubject = new ComboBox();
      this.panel2 = new Panel();
      this.panel6 = new Panel();
      this.btnAddRecipient = new StandardIconButton();
      this.lblAuthorizationCode = new Label();
      this.lblRecipientEmail = new Label();
      this.lblRecipientName = new Label();
      this.lblRecipientType = new Label();
      this.txtSenderEmail = new TextBox();
      this.txtSenderName = new TextBox();
      this.cmbSenderType = new ComboBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.lblSubject = new Label();
      this.pnlRecipients = new FlowLayoutPanel();
      this.gcSigning.SuspendLayout();
      this.gcFulfillment.SuspendLayout();
      this.pnlFulfillment.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gcMessage.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel6.SuspendLayout();
      ((ISupportInitialize) this.btnAddRecipient).BeginInit();
      this.SuspendLayout();
      this.lblRequired.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblRequired.AutoSize = true;
      this.lblRequired.ForeColor = Color.Black;
      this.lblRequired.Location = new Point(8, 715);
      this.lblRequired.Name = "lblRequired";
      this.lblRequired.Size = new Size(88, 14);
      this.lblRequired.TabIndex = 6;
      this.lblRequired.Text = "* Required Fields";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(911, 630);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSend.Location = new Point(828, 630);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 22);
      this.btnSend.TabIndex = 7;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.gcSigning.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.gcSigning.Controls.Add((Control) this.cboSigningOption);
      this.gcSigning.Controls.Add((Control) this.lblSigningOption);
      this.gcSigning.HeaderForeColor = SystemColors.ControlText;
      this.gcSigning.Location = new Point(2, 558);
      this.gcSigning.Name = "gcSigning";
      this.gcSigning.Size = new Size(575, 64);
      this.gcSigning.TabIndex = 10;
      this.gcSigning.Text = "Borrower Signing Options";
      this.cboSigningOption.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSigningOption.FormattingEnabled = true;
      this.cboSigningOption.Location = new Point(258, 32);
      this.cboSigningOption.Name = "cboSigningOption";
      this.cboSigningOption.Size = new Size(302, 22);
      this.cboSigningOption.TabIndex = 1;
      this.cboSigningOption.SelectedIndexChanged += new EventHandler(this.cboSigningOption_SelectedIndexChanged);
      this.lblSigningOption.AutoSize = true;
      this.lblSigningOption.Location = new Point(8, 32);
      this.lblSigningOption.Name = "lblSigningOption";
      this.lblSigningOption.Size = new Size(133, 14);
      this.lblSigningOption.TabIndex = 0;
      this.lblSigningOption.Text = "* Borrower Signing Option";
      this.gcFulfillment.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.gcFulfillment.Controls.Add((Control) this.emHelpLink);
      this.gcFulfillment.Controls.Add((Control) this.chkFulfillment);
      this.gcFulfillment.Controls.Add((Control) this.pnlFulfillment);
      this.gcFulfillment.HeaderForeColor = SystemColors.ControlText;
      this.gcFulfillment.Location = new Point(582, 2);
      this.gcFulfillment.Name = "gcFulfillment";
      this.gcFulfillment.Size = new Size(426, 620);
      this.gcFulfillment.TabIndex = 9;
      this.gcFulfillment.Text = "Fulfillment";
      this.emHelpLink.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink.BackColor = Color.Transparent;
      this.emHelpLink.Cursor = Cursors.Hand;
      this.emHelpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink.HelpTag = "Disclosure Fulfillment";
      this.emHelpLink.Location = new Point(399, 5);
      this.emHelpLink.Name = "emHelpLink";
      this.emHelpLink.Size = new Size(19, 19);
      this.emHelpLink.TabIndex = 11;
      this.chkFulfillment.AutoSize = true;
      this.chkFulfillment.Location = new Point(12, 36);
      this.chkFulfillment.Name = "chkFulfillment";
      this.chkFulfillment.Size = new Size(254, 18);
      this.chkFulfillment.TabIndex = 1;
      this.chkFulfillment.Text = "Schedule a Fulfillment Service for this package.";
      this.chkFulfillment.UseVisualStyleBackColor = true;
      this.chkFulfillment.CheckedChanged += new EventHandler(this.chkFulfillment_CheckedChanged);
      this.pnlFulfillment.Controls.Add((Control) this.dateFulfillBy);
      this.pnlFulfillment.Controls.Add((Control) this.lblFulfillment);
      this.pnlFulfillment.Controls.Add((Control) this.lblGfeDate);
      this.pnlFulfillment.Controls.Add((Control) this.lblFulfillBy);
      this.pnlFulfillment.Controls.Add((Control) this.lblMailFrom);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromName);
      this.pnlFulfillment.Controls.Add((Control) this.lblFromName);
      this.pnlFulfillment.Controls.Add((Control) this.txtGfeDate);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromAddress);
      this.pnlFulfillment.Controls.Add((Control) this.lblFromAddress1);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerZip);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromCity);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerState);
      this.pnlFulfillment.Controls.Add((Control) this.lblFromAddress2);
      this.pnlFulfillment.Controls.Add((Control) this.lblToBorrowerPhone);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromPhone);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerPhone);
      this.pnlFulfillment.Controls.Add((Control) this.lblFromPhone);
      this.pnlFulfillment.Controls.Add((Control) this.lblToBorrowerAddress2);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromState);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerCity);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromZip);
      this.pnlFulfillment.Controls.Add((Control) this.lblToBorrowerAddress);
      this.pnlFulfillment.Controls.Add((Control) this.lblMailToBorrower);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerAddress);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerName);
      this.pnlFulfillment.Controls.Add((Control) this.lblToBorrowerName);
      this.pnlFulfillment.Dock = DockStyle.Bottom;
      this.pnlFulfillment.Enabled = false;
      this.pnlFulfillment.Location = new Point(1, 62);
      this.pnlFulfillment.Name = "pnlFulfillment";
      this.pnlFulfillment.Size = new Size(424, 557);
      this.pnlFulfillment.TabIndex = 2;
      this.dateFulfillBy.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateFulfillBy.CalendarTitleForeColor = Color.White;
      this.dateFulfillBy.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateFulfillBy.CustomFormat = "MM/dd/yyyy";
      this.dateFulfillBy.Format = DateTimePickerFormat.Custom;
      this.dateFulfillBy.Location = new Point(221, 44);
      this.dateFulfillBy.Name = "dateFulfillBy";
      this.dateFulfillBy.Size = new Size(100, 20);
      this.dateFulfillBy.TabIndex = 2;
      this.lblFulfillment.Location = new Point(12, 72);
      this.lblFulfillment.Name = "lblFulfillment";
      this.lblFulfillment.Size = new Size(368, 43);
      this.lblFulfillment.TabIndex = 3;
      this.lblFulfillment.Text = "If the borrower(s) does not access the package by the Scheduled Fulfillment Date, a hard copy of the eDisclosure package will be shipped to the following address.";
      this.lblGfeDate.AutoSize = true;
      this.lblGfeDate.Location = new Point(30, 20);
      this.lblGfeDate.Name = "lblGfeDate";
      this.lblGfeDate.Size = new Size(107, 14);
      this.lblGfeDate.TabIndex = 0;
      this.lblGfeDate.Text = "GFE Application Date";
      this.lblGfeDate.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFulfillBy.AutoSize = true;
      this.lblFulfillBy.Location = new Point(30, 48);
      this.lblFulfillBy.Name = "lblFulfillBy";
      this.lblFulfillBy.Size = new Size(140, 14);
      this.lblFulfillBy.TabIndex = 1;
      this.lblFulfillBy.Text = "* Scheduled Fulfillment Date";
      this.lblFulfillBy.TextAlign = ContentAlignment.MiddleLeft;
      this.lblMailFrom.AutoSize = true;
      this.lblMailFrom.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMailFrom.Location = new Point(12, 128);
      this.lblMailFrom.Name = "lblMailFrom";
      this.lblMailFrom.Size = new Size(36, 14);
      this.lblMailFrom.TabIndex = 4;
      this.lblMailFrom.Text = "From";
      this.txtFromName.Location = new Point((int) sbyte.MaxValue, 148);
      this.txtFromName.MaxLength = 50;
      this.txtFromName.Name = "txtFromName";
      this.txtFromName.Size = new Size(276, 20);
      this.txtFromName.TabIndex = 6;
      this.lblFromName.AutoSize = true;
      this.lblFromName.Location = new Point(12, 151);
      this.lblFromName.Name = "lblFromName";
      this.lblFromName.Size = new Size(41, 14);
      this.lblFromName.TabIndex = 5;
      this.lblFromName.Text = "* Name";
      this.lblFromName.TextAlign = ContentAlignment.MiddleLeft;
      this.txtGfeDate.Location = new Point(219, 20);
      this.txtGfeDate.MaxLength = 10;
      this.txtGfeDate.Name = "txtGfeDate";
      this.txtGfeDate.ReadOnly = true;
      this.txtGfeDate.Size = new Size(100, 20);
      this.txtGfeDate.TabIndex = 1;
      this.txtFromAddress.Location = new Point((int) sbyte.MaxValue, 172);
      this.txtFromAddress.MaxLength = 50;
      this.txtFromAddress.Name = "txtFromAddress";
      this.txtFromAddress.Size = new Size(276, 20);
      this.txtFromAddress.TabIndex = 8;
      this.lblFromAddress1.AutoSize = true;
      this.lblFromAddress1.Location = new Point(12, 175);
      this.lblFromAddress1.Name = "lblFromAddress1";
      this.lblFromAddress1.Size = new Size(87, 14);
      this.lblFromAddress1.TabIndex = 7;
      this.lblFromAddress1.Text = "* Street Address";
      this.lblFromAddress1.TextAlign = ContentAlignment.MiddleLeft;
      this.txtToBorrowerZip.Location = new Point(323, 320);
      this.txtToBorrowerZip.MaxLength = 10;
      this.txtToBorrowerZip.Name = "txtToBorrowerZip";
      this.txtToBorrowerZip.Size = new Size(80, 20);
      this.txtToBorrowerZip.TabIndex = 23;
      this.txtFromCity.Location = new Point((int) sbyte.MaxValue, 196);
      this.txtFromCity.MaxLength = 50;
      this.txtFromCity.Name = "txtFromCity";
      this.txtFromCity.Size = new Size(152, 20);
      this.txtFromCity.TabIndex = 10;
      this.txtToBorrowerState.Location = new Point(283, 320);
      this.txtToBorrowerState.MaxLength = 2;
      this.txtToBorrowerState.Name = "txtToBorrowerState";
      this.txtToBorrowerState.Size = new Size(36, 20);
      this.txtToBorrowerState.TabIndex = 22;
      this.lblFromAddress2.AutoSize = true;
      this.lblFromAddress2.Location = new Point(12, 199);
      this.lblFromAddress2.Name = "lblFromAddress2";
      this.lblFromAddress2.Size = new Size(83, 14);
      this.lblFromAddress2.TabIndex = 9;
      this.lblFromAddress2.Text = "* City, State, Zip";
      this.lblFromAddress2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblToBorrowerPhone.AutoSize = true;
      this.lblToBorrowerPhone.Location = new Point(12, 347);
      this.lblToBorrowerPhone.Name = "lblToBorrowerPhone";
      this.lblToBorrowerPhone.Size = new Size(44, 14);
      this.lblToBorrowerPhone.TabIndex = 24;
      this.lblToBorrowerPhone.Text = "* Phone";
      this.lblToBorrowerPhone.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFromPhone.Location = new Point((int) sbyte.MaxValue, 220);
      this.txtFromPhone.MaxLength = 20;
      this.txtFromPhone.Name = "txtFromPhone";
      this.txtFromPhone.Size = new Size(152, 20);
      this.txtFromPhone.TabIndex = 14;
      this.txtToBorrowerPhone.Location = new Point((int) sbyte.MaxValue, 344);
      this.txtToBorrowerPhone.MaxLength = 20;
      this.txtToBorrowerPhone.Name = "txtToBorrowerPhone";
      this.txtToBorrowerPhone.Size = new Size(152, 20);
      this.txtToBorrowerPhone.TabIndex = 25;
      this.lblFromPhone.AutoSize = true;
      this.lblFromPhone.Location = new Point(12, 223);
      this.lblFromPhone.Name = "lblFromPhone";
      this.lblFromPhone.Size = new Size(44, 14);
      this.lblFromPhone.TabIndex = 13;
      this.lblFromPhone.Text = "* Phone";
      this.lblFromPhone.TextAlign = ContentAlignment.MiddleLeft;
      this.lblToBorrowerAddress2.AutoSize = true;
      this.lblToBorrowerAddress2.Location = new Point(12, 323);
      this.lblToBorrowerAddress2.Name = "lblToBorrowerAddress2";
      this.lblToBorrowerAddress2.Size = new Size(83, 14);
      this.lblToBorrowerAddress2.TabIndex = 20;
      this.lblToBorrowerAddress2.Text = "* City, State, Zip";
      this.lblToBorrowerAddress2.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFromState.Location = new Point(283, 196);
      this.txtFromState.MaxLength = 2;
      this.txtFromState.Name = "txtFromState";
      this.txtFromState.Size = new Size(36, 20);
      this.txtFromState.TabIndex = 11;
      this.txtToBorrowerCity.Location = new Point((int) sbyte.MaxValue, 320);
      this.txtToBorrowerCity.MaxLength = 50;
      this.txtToBorrowerCity.Name = "txtToBorrowerCity";
      this.txtToBorrowerCity.Size = new Size(152, 20);
      this.txtToBorrowerCity.TabIndex = 21;
      this.txtFromZip.Location = new Point(323, 196);
      this.txtFromZip.MaxLength = 10;
      this.txtFromZip.Name = "txtFromZip";
      this.txtFromZip.Size = new Size(80, 20);
      this.txtFromZip.TabIndex = 12;
      this.lblToBorrowerAddress.AutoSize = true;
      this.lblToBorrowerAddress.Location = new Point(12, 299);
      this.lblToBorrowerAddress.Name = "lblToBorrowerAddress";
      this.lblToBorrowerAddress.Size = new Size(87, 14);
      this.lblToBorrowerAddress.TabIndex = 18;
      this.lblToBorrowerAddress.Text = "* Street Address";
      this.lblToBorrowerAddress.TextAlign = ContentAlignment.MiddleLeft;
      this.lblMailToBorrower.AutoSize = true;
      this.lblMailToBorrower.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMailToBorrower.Location = new Point(12, 252);
      this.lblMailToBorrower.Name = "lblMailToBorrower";
      this.lblMailToBorrower.Size = new Size(76, 14);
      this.lblMailToBorrower.TabIndex = 15;
      this.lblMailToBorrower.Text = "To Borrower";
      this.txtToBorrowerAddress.Location = new Point((int) sbyte.MaxValue, 296);
      this.txtToBorrowerAddress.MaxLength = 50;
      this.txtToBorrowerAddress.Name = "txtToBorrowerAddress";
      this.txtToBorrowerAddress.Size = new Size(276, 20);
      this.txtToBorrowerAddress.TabIndex = 19;
      this.txtToBorrowerName.Location = new Point((int) sbyte.MaxValue, 272);
      this.txtToBorrowerName.MaxLength = 50;
      this.txtToBorrowerName.Name = "txtToBorrowerName";
      this.txtToBorrowerName.Size = new Size(276, 20);
      this.txtToBorrowerName.TabIndex = 17;
      this.lblToBorrowerName.AutoSize = true;
      this.lblToBorrowerName.Location = new Point(12, 275);
      this.lblToBorrowerName.Name = "lblToBorrowerName";
      this.lblToBorrowerName.Size = new Size(41, 14);
      this.lblToBorrowerName.TabIndex = 16;
      this.lblToBorrowerName.Text = "* Name";
      this.lblToBorrowerName.TextAlign = ContentAlignment.MiddleLeft;
      this.panel1.Controls.Add((Control) this.lblSenderEmail);
      this.panel1.Controls.Add((Control) this.lblSenderName);
      this.panel1.Controls.Add((Control) this.lblSenderType);
      this.panel1.Location = new Point(11, 29);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(562, 37);
      this.panel1.TabIndex = 27;
      this.lblSenderEmail.AutoSize = true;
      this.lblSenderEmail.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSenderEmail.Location = new Point(304, 9);
      this.lblSenderEmail.Name = "lblSenderEmail";
      this.lblSenderEmail.Size = new Size(37, 13);
      this.lblSenderEmail.TabIndex = 44;
      this.lblSenderEmail.Text = "Email";
      this.lblSenderEmail.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSenderName.AutoSize = true;
      this.lblSenderName.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSenderName.Location = new Point(167, 11);
      this.lblSenderName.Name = "lblSenderName";
      this.lblSenderName.Size = new Size(39, 13);
      this.lblSenderName.TabIndex = 43;
      this.lblSenderName.Text = "Name";
      this.lblSenderName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSenderType.AutoSize = true;
      this.lblSenderType.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSenderType.Location = new Point(8, 11);
      this.lblSenderType.Name = "lblSenderType";
      this.lblSenderType.Size = new Size(79, 13);
      this.lblSenderType.TabIndex = 42;
      this.lblSenderType.Text = "Sender Type";
      this.lblSenderType.TextAlign = ContentAlignment.MiddleCenter;
      this.gcMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcMessage.Controls.Add((Control) this.htmlEmailControl);
      this.gcMessage.Controls.Add((Control) this.btnNotifyUsers);
      this.gcMessage.Controls.Add((Control) this.lblNotifyUserCount);
      this.gcMessage.Controls.Add((Control) this.dateAcceptBy);
      this.gcMessage.Controls.Add((Control) this.chkAcceptBy);
      this.gcMessage.Controls.Add((Control) this.chkReadReceipt);
      this.gcMessage.Controls.Add((Control) this.cboSubject);
      this.gcMessage.Controls.Add((Control) this.panel2);
      this.gcMessage.Controls.Add((Control) this.lblSubject);
      this.gcMessage.Controls.Add((Control) this.pnlRecipients);
      this.gcMessage.HeaderForeColor = SystemColors.ControlText;
      this.gcMessage.Location = new Point(2, 2);
      this.gcMessage.Name = "gcMessage";
      this.gcMessage.Size = new Size(575, 553);
      this.gcMessage.TabIndex = 1;
      this.gcMessage.Text = "Message";
      this.htmlEmailControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(5, 295);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(565, 200);
      this.htmlEmailControl.TabIndex = 12;
      this.btnNotifyUsers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNotifyUsers.Location = new Point(11, 500);
      this.btnNotifyUsers.Name = "btnNotifyUsers";
      this.btnNotifyUsers.Size = new Size(129, 23);
      this.btnNotifyUsers.TabIndex = 53;
      this.btnNotifyUsers.Text = "Notify Additional Users";
      this.btnNotifyUsers.UseVisualStyleBackColor = true;
      this.btnNotifyUsers.Click += new EventHandler(this.btnNotifyUsers_Click);
      this.lblNotifyUserCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblNotifyUserCount.AutoSize = true;
      this.lblNotifyUserCount.Location = new Point(10, 528);
      this.lblNotifyUserCount.Name = "lblNotifyUserCount";
      this.lblNotifyUserCount.Size = new Size(105, 14);
      this.lblNotifyUserCount.TabIndex = 50;
      this.lblNotifyUserCount.Text = "({0} Users selected)";
      this.dateAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.dateAcceptBy.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateAcceptBy.CalendarTitleForeColor = Color.White;
      this.dateAcceptBy.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateAcceptBy.CustomFormat = "MM/dd/yyyy";
      this.dateAcceptBy.Enabled = false;
      this.dateAcceptBy.Format = DateTimePickerFormat.Custom;
      this.dateAcceptBy.Location = new Point(467, 526);
      this.dateAcceptBy.Name = "dateAcceptBy";
      this.dateAcceptBy.Size = new Size(102, 20);
      this.dateAcceptBy.TabIndex = 52;
      this.chkAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAcceptBy.Location = new Point(162, 526);
      this.chkAcceptBy.Name = "chkAcceptBy";
      this.chkAcceptBy.Size = new Size(328, 20);
      this.chkAcceptBy.TabIndex = 51;
      this.chkAcceptBy.Text = "Notify me when borrower does not access by";
      this.chkAcceptBy.UseVisualStyleBackColor = true;
      this.chkAcceptBy.CheckedChanged += new EventHandler(this.chkAcceptBy_CheckedChanged);
      this.chkReadReceipt.AutoSize = true;
      this.chkReadReceipt.Checked = true;
      this.chkReadReceipt.CheckState = CheckState.Checked;
      this.chkReadReceipt.Location = new Point(162, 502);
      this.chkReadReceipt.Name = "chkReadReceipt";
      this.chkReadReceipt.Size = new Size(261, 18);
      this.chkReadReceipt.TabIndex = 49;
      this.chkReadReceipt.Text = "Notify me when borrower receives the package.";
      this.chkReadReceipt.UseVisualStyleBackColor = true;
      this.cboSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSubject.FormattingEnabled = true;
      this.cboSubject.Location = new Point(112, 265);
      this.cboSubject.Name = "cboSubject";
      this.cboSubject.Size = new Size(426, 22);
      this.cboSubject.Sorted = true;
      this.cboSubject.TabIndex = 12;
      this.cboSubject.SelectedIndexChanged += new EventHandler(this.cboSubject_SelectedIndexChanged);
      this.panel2.Controls.Add((Control) this.panel6);
      this.panel2.Controls.Add((Control) this.txtSenderEmail);
      this.panel2.Controls.Add((Control) this.txtSenderName);
      this.panel2.Controls.Add((Control) this.cmbSenderType);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Location = new Point(2, 28);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(568, 115);
      this.panel2.TabIndex = 45;
      this.panel6.Controls.Add((Control) this.btnAddRecipient);
      this.panel6.Controls.Add((Control) this.lblAuthorizationCode);
      this.panel6.Controls.Add((Control) this.lblRecipientEmail);
      this.panel6.Controls.Add((Control) this.lblRecipientName);
      this.panel6.Controls.Add((Control) this.lblRecipientType);
      this.panel6.Location = new Point(4, 64);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(560, 48);
      this.panel6.TabIndex = 52;
      this.btnAddRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddRecipient.BackColor = Color.Transparent;
      this.btnAddRecipient.Location = new Point(526, 12);
      this.btnAddRecipient.Margin = new Padding(4, 3, 0, 3);
      this.btnAddRecipient.MouseDownImage = (Image) null;
      this.btnAddRecipient.Name = "btnAddRecipient";
      this.btnAddRecipient.Size = new Size(21, 16);
      this.btnAddRecipient.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddRecipient.TabIndex = 49;
      this.btnAddRecipient.TabStop = false;
      this.btnAddRecipient.Click += new EventHandler(this.btnAddRecipient_Click);
      this.lblAuthorizationCode.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAuthorizationCode.Location = new Point(422, 6);
      this.lblAuthorizationCode.Name = "lblAuthorizationCode";
      this.lblAuthorizationCode.Size = new Size(105, 39);
      this.lblAuthorizationCode.TabIndex = 48;
      this.lblAuthorizationCode.Text = "Authorization Code";
      this.lblAuthorizationCode.TextAlign = ContentAlignment.MiddleCenter;
      this.lblRecipientEmail.AutoSize = true;
      this.lblRecipientEmail.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientEmail.Location = new Point(300, 11);
      this.lblRecipientEmail.Name = "lblRecipientEmail";
      this.lblRecipientEmail.Size = new Size(37, 13);
      this.lblRecipientEmail.TabIndex = 44;
      this.lblRecipientEmail.Text = "Email";
      this.lblRecipientEmail.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRecipientName.AutoSize = true;
      this.lblRecipientName.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientName.Location = new Point(170, 11);
      this.lblRecipientName.Name = "lblRecipientName";
      this.lblRecipientName.Size = new Size(39, 13);
      this.lblRecipientName.TabIndex = 43;
      this.lblRecipientName.Text = "Name";
      this.lblRecipientName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRecipientType.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientType.Location = new Point(26, 7);
      this.lblRecipientType.Name = "lblRecipientType";
      this.lblRecipientType.Size = new Size(85, 35);
      this.lblRecipientType.TabIndex = 42;
      this.lblRecipientType.Text = "Recipient Type";
      this.lblRecipientType.TextAlign = ContentAlignment.MiddleCenter;
      this.txtSenderEmail.Location = new Point(258, 34);
      this.txtSenderEmail.Name = "txtSenderEmail";
      this.txtSenderEmail.ReadOnly = true;
      this.txtSenderEmail.Size = new Size(153, 20);
      this.txtSenderEmail.TabIndex = 51;
      this.txtSenderName.Location = new Point(143, 34);
      this.txtSenderName.Name = "txtSenderName";
      this.txtSenderName.ReadOnly = true;
      this.txtSenderName.Size = new Size(100, 20);
      this.txtSenderName.TabIndex = 50;
      this.cmbSenderType.FormattingEnabled = true;
      this.cmbSenderType.Location = new Point(8, 33);
      this.cmbSenderType.Name = "cmbSenderType";
      this.cmbSenderType.Size = new Size(121, 22);
      this.cmbSenderType.TabIndex = 49;
      this.cmbSenderType.SelectedIndexChanged += new EventHandler(this.cmbSenderType_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(292, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(37, 13);
      this.label1.TabIndex = 44;
      this.label1.Text = "Email";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(162, 11);
      this.label2.Name = "label2";
      this.label2.Size = new Size(39, 13);
      this.label2.TabIndex = 43;
      this.label2.Text = "Name";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(17, 10);
      this.label3.Name = "label3";
      this.label3.Size = new Size(79, 13);
      this.label3.TabIndex = 42;
      this.label3.Text = "Sender Type";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(32, 268);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(50, 14);
      this.lblSubject.TabIndex = 11;
      this.lblSubject.Text = "* Subject";
      this.lblSubject.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlRecipients.AutoScroll = true;
      this.pnlRecipients.Location = new Point(3, 146);
      this.pnlRecipients.Name = "pnlRecipients";
      this.pnlRecipients.Size = new Size(567, 112);
      this.pnlRecipients.TabIndex = 48;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1012, 658);
      this.Controls.Add((Control) this.gcSigning);
      this.Controls.Add((Control) this.gcFulfillment);
      this.Controls.Add((Control) this.lblRequired);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSend);
      this.Controls.Add((Control) this.gcMessage);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SendPackageDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Send";
      this.gcSigning.ResumeLayout(false);
      this.gcSigning.PerformLayout();
      this.gcFulfillment.ResumeLayout(false);
      this.gcFulfillment.PerformLayout();
      this.pnlFulfillment.ResumeLayout(false);
      this.pnlFulfillment.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.gcMessage.ResumeLayout(false);
      this.gcMessage.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      ((ISupportInitialize) this.btnAddRecipient).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
