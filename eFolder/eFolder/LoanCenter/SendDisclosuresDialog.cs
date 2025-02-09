// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.SendDisclosuresDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.eFolder.eDelivery.ePass;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class SendDisclosuresDialog : Form
  {
    private const string SIGNING_OPTION_ELECTRONIC = "eSign (electronically sign and return)";
    private const string SIGNING_OPTION_ELECTRONIC_AND_WET = "eSign + Wet Sign (for wet sign only documents)";
    private const string SIGNING_OPTION_WET = "Wet Sign (print, sign, and fax)";
    private const string SIGNING_OPTION_NONE = "No Signature Required";
    private const string AUTHENTICATION_QUESTIONS = "Answer security questions";
    private const string AUTHENTICATION_PASSWORD = "Authorization Code";
    private LoanDataMgr loanDataMgr;
    private LoanData loan;
    private string coversheetFile;
    private DocumentLog[] signList;
    private DocumentLog[] neededList;
    private string[] pdfList;
    private List<DocumentAttachment> attachmentList;
    private bool noOriginatorCancel;
    private bool hasBorrowerDocs;
    private string packageID;
    private string comments;
    private bool isWetSign;
    private HtmlEmailTemplateType emailType = HtmlEmailTemplateType.InitialDisclosures;
    private string sessionEmailProfileName = "Disclosures";
    private List<LoanCenterSigner> signers;
    private List<LoanCenterSigner> deletedSigners;
    private List<string> requiredSigners;
    private List<string> invalidList = new List<string>();
    private EDisclosureSignOrderSetup signOrderSetup;
    private Dictionary<string, Tuple<string, string, string>> SenderData;
    private IContainer components;
    private Button btnCancel;
    private Button btnSend;
    private GroupContainer gcMessage;
    private CheckBox chkReadReceipt;
    private Label lblSubject;
    private GroupContainer gcSigning;
    private ComboBox cboSigningOption;
    private Label lblSigningOption;
    private GroupContainer gcFulfillment;
    private CheckBox chkFulfillment;
    private CheckBox chkAcceptBy;
    private EMHelpLink emHelpLink;
    private Label lblRequired;
    private DateTimePicker dateAcceptBy;
    private HtmlEmailControl htmlEmailControl;
    private ComboBox cboSubject;
    private Label lblNotifyUserCount;
    private Button btnNotifyUsers;
    private ComboBox cboAuthentication;
    private Label lblAuthentication;
    private FlowLayoutPanel pnlRecipients;
    private Panel pnlFulfillment;
    private Button btnCopyAddress;
    private TextBox txtToCoborrowerZip;
    private TextBox txtToCoborrowerState;
    private Label lblToCoborrowerPhone;
    private TextBox txtToCoborrowerPhone;
    private Label lblToCoborrowerAddress2;
    private TextBox txtToCoborrowerCity;
    private Label lblToCoborrowerAddress;
    private Label lblMailToCoborrower;
    private TextBox txtToCoborrowerAddress;
    private TextBox txtToCoborrowerName;
    private Label lblToCoborrowerName;
    private Label lblFulfillment;
    private TextBox txtToBorrowerZip;
    private TextBox txtToBorrowerState;
    private Label lblToBorrowerPhone;
    private TextBox txtToBorrowerPhone;
    private Label lblToBorrowerAddress2;
    private TextBox txtToBorrowerCity;
    private Label lblToBorrowerAddress;
    private Label lblMailToBorrower;
    private TextBox txtToBorrowerAddress;
    private TextBox txtToBorrowerName;
    private Label lblToBorrowerName;
    private Label lblMailFrom;
    private TextBox txtFromName;
    private Label lblFromName;
    private TextBox txtFromAddress;
    private Label lblFromAddress1;
    private TextBox txtFromCity;
    private Label lblFromAddress2;
    private TextBox txtFromPhone;
    private Label lblFromPhone;
    private TextBox txtFromState;
    private DateTimePicker dateFulfillBy;
    private Label lblGfeDate;
    private Label lblFulfillBy;
    private TextBox txtGfeDate;
    private TextBox txtFromZip;
    private Panel panel2;
    private Panel panel6;
    private StandardIconButton btnAddRecipient;
    private Label lblAuthorizationCode;
    private Label lblRecipientEmail;
    private Label lblRecipientName;
    private Label lblRecipientType;
    private TextBox txtSenderEmail;
    private TextBox txtSenderName;
    private ComboBox cmbSenderType;
    private Label label1;
    private Label label2;
    private Label label3;
    private Panel panelDlg;

    public SendDisclosuresDialog(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList)
      : this(loanDataMgr, coversheetFile, signList, neededList, pdfList, HtmlEmailTemplateType.InitialDisclosures)
    {
    }

    public SendDisclosuresDialog(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList,
      HtmlEmailTemplateType emailType)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 77, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.loan = loanDataMgr.LoanData;
      this.coversheetFile = coversheetFile;
      this.signList = signList;
      this.neededList = neededList;
      this.pdfList = pdfList;
      this.emailType = emailType;
      this.signOrderSetup = Session.ConfigurationManager.GetEDisclosureSignOrderSetup();
      Cursor.Current = Cursors.WaitCursor;
      this.initAttachmentList();
      this.initSigners();
      this.initNotifyCount();
      if (!this.noOriginatorCancel)
        this.initContents();
      Cursor.Current = Cursors.Default;
      PerformanceMeter.Current.AddCheckpoint("END", 105, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
    }

    public string OriginatorSignerUrl { get; private set; }

    public bool NoOriginatorCancel => this.noOriginatorCancel;

    public string PackageID => this.packageID;

    public bool IsWetSign => this.isWetSign;

    public string Comments => this.comments;

    private void initAttachmentList()
    {
      DocumentTrackingSetup documentTrackingSetup = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup;
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
          if (documentTrackingSetup.IgnoreIntendedFor || documentAttachment.IntendedFor == ForBorrowerType.All || documentAttachment.IntendedFor == ForBorrowerType.Borrower)
            this.hasBorrowerDocs = true;
          this.attachmentList.Add(documentAttachment);
        }
      }
      if (this.neededList != null && this.neededList.Length != 0)
        this.hasBorrowerDocs = true;
      this.requiredSigners = this.getRequiredSigners();
    }

    private void initNotifyCount()
    {
      this.lblNotifyUserCount.Text = string.Format("({0} Users selected)", (object) new EmailNotificationClient().ActiveEmailCount(new List<Guid>()
      {
        new Guid(this.loan.GUID)
      }.ToArray()));
    }

    private void initSigners()
    {
      this.addSigner(this.populateBorrower());
      this.addSigner(this.populateCoborrower());
      string simpleField1 = this.loan.GetSimpleField("3239");
      if (simpleField1 == string.Empty)
      {
        if (Utils.Dialog((IWin32Window) this, "There are documents included in this request that have Loan Originator signing points, but a Loan Originator is not currently assigned to the loan. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        {
          this.noOriginatorCancel = true;
          return;
        }
      }
      else
        this.addSigner(this.populateOriginator(simpleField1));
      this.populateNonBorrowingOwner().ForEach((Action<LoanCenterSigner>) (x => this.addSigner(x)));
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        using (ePackage ePackage = new ePackage(Session.SessionObjects?.StartupInfo?.ServiceUrls?.EPackageServiceUrl))
        {
          string securityFieldList = ePackage.GetSecurityFieldList();
          xmlDocument.LoadXml(securityFieldList);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to retrieve the list of security questions:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      LoanCenterSigner loanCenterSigner1 = this.signers.FirstOrDefault<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.EntityType == LoanCenterEntityType.Borrower));
      if (loanCenterSigner1 != null)
      {
        foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("/SECURITY_FIELDS/BORROWER/FIELD"))
        {
          string attribute = selectNode.GetAttribute("ID");
          string simpleField2 = this.loan.GetSimpleField(attribute);
          if (simpleField2 != string.Empty)
            loanCenterSigner1.AddSecurityQuestion(attribute, simpleField2);
        }
      }
      LoanCenterSigner loanCenterSigner2 = this.signers.FirstOrDefault<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.EntityType == LoanCenterEntityType.Coborrower));
      if (loanCenterSigner2 != null)
      {
        foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("/SECURITY_FIELDS/COBORROWER/FIELD"))
        {
          string attribute = selectNode.GetAttribute("ID");
          string simpleField3 = this.loan.GetSimpleField(attribute);
          if (simpleField3 != string.Empty)
            loanCenterSigner2.AddSecurityQuestion(attribute, simpleField3);
        }
      }
      if (!this.signers.Any<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.EntityType == LoanCenterEntityType.NonBorrowingOwner)))
        return;
      XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("/SECURITY_FIELDS/NONBORROWINGOWNER/FIELD");
      foreach (LoanCenterSigner loanCenterSigner3 in this.signers.FindAll((Predicate<LoanCenterSigner>) (x => x.EntityType == LoanCenterEntityType.NonBorrowingOwner)))
      {
        foreach (XmlElement xmlElement in xmlNodeList)
        {
          string attribute = xmlElement.GetAttribute("ID");
          string empty = string.Empty;
          string fieldValue = !attribute.Contains("TR") ? (!attribute.Contains("NBOC") ? this.loan.GetSimpleField(attribute) : this.loan.GetSimpleField("NBOC" + (loanCenterSigner3.NboIndex <= 99 ? loanCenterSigner3.NboIndex.ToString("00") : loanCenterSigner3.NboIndex.ToString("000")) + attribute.Substring(attribute.Length - 2))) : this.loan.GetSimpleField("TR" + (loanCenterSigner3.NBOVestingIndex <= 99 ? loanCenterSigner3.NBOVestingIndex.ToString("00") : loanCenterSigner3.NBOVestingIndex.ToString("000")) + attribute.Substring(attribute.Length - 2));
          if (fieldValue != string.Empty)
            loanCenterSigner3.AddSecurityQuestion(attribute, fieldValue);
        }
      }
    }

    private void initContents()
    {
      this.cboAuthentication.Items.Add((object) "Answer security questions");
      this.cboAuthentication.Items.Add((object) "Authorization Code");
      this.cboAuthentication.SelectedIndex = 1;
      this.initTitle();
      this.initHtmlControl();
      this.initFrom();
      this.initSubject();
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
      this.dateAcceptBy.Value = Utils.ParseDate((object) this.loan.GetField("3143"), DateTime.Today.AddDays(1.0));
      bool flag1 = false;
      bool flag2 = false;
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
      {
        this.gcMessage.Width = this.gcFulfillment.Right - this.gcMessage.Left;
        this.gcSigning.Width = this.gcMessage.Width;
        this.gcFulfillment.Visible = false;
        this.Width = 661;
      }
      else
      {
        if (!(Session.ConfigurationManager.GetCompanySetting("Fulfillment", "AutoFulfillment") == "Y"))
          return;
        this.chkFulfillment.Checked = true;
      }
    }

    private void initTitle()
    {
      if (this.emailType == HtmlEmailTemplateType.PreClosing)
      {
        this.Text = "Send Pre-Closing Documents";
        this.sessionEmailProfileName = "PreClosing";
      }
      else
      {
        this.Text = "Send eDisclosures";
        this.sessionEmailProfileName = "Disclosures";
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
      this.SenderData = new Dictionary<string, Tuple<string, string, string>>();
      this.cmbSenderType.Items.Clear();
      for (int index = 0; index <= 2; ++index)
      {
        string key = (string) null;
        string userid = (string) null;
        string email = (string) null;
        string name = (string) null;
        switch (index)
        {
          case 0:
            EmailFrom.GetFromUser(this.loanDataMgr, EmailFromType.CurrentUser, ref userid, ref email, ref name);
            key = "Current User";
            break;
          case 1:
            EmailFrom.GetFromUser(this.loanDataMgr, EmailFromType.FileStarter, ref userid, ref email, ref name);
            key = "File Starter";
            break;
          case 2:
            EmailFrom.GetFromUser(this.loanDataMgr, EmailFromType.LoanOfficer, ref userid, ref email, ref name);
            key = "Loan Officer";
            break;
        }
        if (!string.IsNullOrEmpty(userid))
        {
          this.cmbSenderType.Items.Add((object) key);
          this.SenderData[key] = new Tuple<string, string, string>(userid, email, name);
        }
      }
      if (this.cmbSenderType.Items.Count <= 0)
        return;
      this.cmbSenderType.SelectedIndex = 0;
    }

    private void initSubject()
    {
      HtmlEmailTemplate[] htmlEmailTemplates = Session.ConfigurationManager.GetHtmlEmailTemplates((string) null, this.emailType);
      if (htmlEmailTemplates != null)
        this.cboSubject.Items.AddRange((object[]) htmlEmailTemplates);
      string strB = Session.GetPrivateProfileString("EmailTemplates", this.sessionEmailProfileName);
      if (string.IsNullOrEmpty(strB))
        strB = Session.ConfigurationManager.GetCompanySetting("DefaultEmailTemplates", this.sessionEmailProfileName);
      foreach (HtmlEmailTemplate htmlEmailTemplate in this.cboSubject.Items)
      {
        if (string.Compare(htmlEmailTemplate.Guid, strB, true) == 0)
          this.cboSubject.SelectedItem = (object) htmlEmailTemplate;
      }
      if (this.cboSubject.SelectedItem != null || this.cboSubject.Items.Count <= 0)
        return;
      this.cboSubject.SelectedIndex = 0;
    }

    private void btnCopyAddress_Click(object sender, EventArgs e)
    {
      if (!this.hasBorrowerDocs)
        return;
      this.txtToCoborrowerAddress.Text = this.txtToBorrowerAddress.Text;
      this.txtToCoborrowerCity.Text = this.txtToBorrowerCity.Text;
      this.txtToCoborrowerState.Text = this.txtToBorrowerState.Text;
      this.txtToCoborrowerZip.Text = this.txtToBorrowerZip.Text;
      this.txtToCoborrowerPhone.Text = this.txtToBorrowerPhone.Text;
    }

    private LoanCenterMessage createMessage()
    {
      LoanCenterMessage message = new LoanCenterMessage(this.loanDataMgr, LoanCenterMessageType.InitialDisclosures)
      {
        ReplyTo = this.txtSenderEmail.Text,
        NBOSendEmails = new List<EncompassContactEmail>(),
        To = string.Empty,
        CC = string.Empty
      };
      message.NBOSendEmails = new List<EncompassContactEmail>();
      if (this.signers.Any<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.RecipientType.Trim().ToLower() == "borrower")))
        message.To = this.signers.FirstOrDefault<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.RecipientType.Trim().ToLower() == "borrower")).EmailAddress;
      if (this.signers.Any<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.RecipientType.Trim().ToLower() != "borrower" && x.RecipientType.Trim().ToLower() != "nonborrowingowner" && x.RecipientType.Trim().ToLower() != "originator")))
        message.CC = string.Join(";", this.signers.FindAll((Predicate<LoanCenterSigner>) (x => x.RecipientType.Trim().ToLower() != "borrower" && x.RecipientType.Trim().ToLower() != "nonborrowingowner" && x.RecipientType.Trim().ToLower() != "originator")).Select<LoanCenterSigner, string>((Func<LoanCenterSigner, string>) (y => y.EmailAddress)).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      foreach (LoanCenterSigner loanCenterSigner in this.signers.FindAll((Predicate<LoanCenterSigner>) (x => x.RecipientType.Trim().ToLower() == "nonborrowingowner")))
        message.NBOSendEmails.Add(new EncompassContactEmail()
        {
          email = loanCenterSigner.EmailAddress,
          encompasscontactGuid = loanCenterSigner.SignerID
        });
      message.Subject = this.cboSubject.Text.Trim();
      message.Body = this.htmlEmailControl.Html;
      message.ReadReceipt = this.chkReadReceipt.Checked;
      if (this.chkAcceptBy.Checked)
        message.NotificationDate = this.dateAcceptBy.Value;
      if (this.cboSigningOption.Text.Equals("eSign (electronically sign and return)") || this.cboSigningOption.Text.Equals("eSign + Wet Sign (for wet sign only documents)"))
      {
        message.ElectronicSignature = true;
        if (this.cboAuthentication.Text.Equals("Answer security questions"))
          message.AnswerQuestions = true;
        int num = 0;
        foreach (DocumentAttachment attachment in this.attachmentList)
        {
          ++num;
          if (!message.LoadSignersFromJSON(attachment, this.signers))
          {
            LoanCenterSigner signer1 = this.signers.FirstOrDefault<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.EntityType == LoanCenterEntityType.Borrower));
            if (signer1 != null && signer1.RequiresSigning(attachment))
              message.AddSigner(signer1);
            LoanCenterSigner signer2 = this.signers.FirstOrDefault<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.EntityType == LoanCenterEntityType.Coborrower));
            if (signer2 != null && signer2.RequiresSigning(attachment))
              message.AddSigner(signer2);
            LoanCenterSigner signer3 = this.signers.FirstOrDefault<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.EntityType == LoanCenterEntityType.Originator));
            if (signer3 != null && signer3.RequiresSigning(attachment))
              message.AddSigner(signer3);
            if (this.signers.Any<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.EntityType == LoanCenterEntityType.NonBorrowingOwner)) && new eDeliveryMessage(this.loanDataMgr, eDeliveryMessageType.RequestDocuments).checkSigningpointExists(attachment, LoanCenterEntityType.NonBorrowingOwner.ToString("g")))
            {
              foreach (LoanCenterSigner signer4 in this.signers.FindAll((Predicate<LoanCenterSigner>) (x => x.EntityType == LoanCenterEntityType.NonBorrowingOwner)))
                message.AddSigner(signer4);
            }
          }
        }
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
      message.AddCoversheet(this.coversheetFile);
      foreach (LoanCenterAttachment attachment in this.attachmentList)
        message.AddAttachment(attachment);
      if (this.neededList != null && this.neededList.Length != 0)
        message.AddNeeded(this.neededList);
      return message;
    }

    private bool validateMessage(LoanCenterMessage msg)
    {
      if (msg.Subject == string.Empty)
        this.invalidList.Add("Subject is required.");
      if (msg.NotificationDate.Date != DateTime.MinValue.Date && msg.NotificationDate.Date < DateTime.Today)
        this.invalidList.Add("Borrower Accept By Date must be greater than or equal to today's date.");
      if (msg.FulfillmentEnabled)
      {
        foreach (LoanCenterAttachment attachment in msg.Attachments)
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
        if (msg.FulfillmentToPhone.Length < 12)
          this.invalidList.Add("The Fufillment 'Borrower Phone Number' must be a 10-digit phone number.");
        if (msg.FulfillmentToAddress.Length > 50)
          this.invalidList.Add("Fulfillment 'Borrower Street Address' cannot be more than 50 characters");
        if (!Regex.Match(msg.FulfillmentToZip, pattern1).Success)
          this.invalidList.Add("Fulfillment 'Borrower Zip' is invalid. Zip code format should be  5 digits or five digits + four digits");
        else if (msg.FulfillmentToZip.Contains("00000"))
          this.invalidList.Add("Fulfillment 'Borrower Zip' is invalid.");
        if (!Regex.Match(msg.FulfillmentToState, pattern2).Success)
          this.invalidList.Add("Fulfillment 'Borrower State' is invalid. State should have 2 characters.");
        if (DateTime.Today.AddDays(120.0) < msg.FulfillmentSchedDate)
          this.invalidList.Add("Scheduled fulfillment date cannot be more than 120 days from today's date");
      }
      if (msg.ElectronicSignature)
      {
        List<string> stringList = new List<string>();
        foreach (LoanCenterSigner signer in msg.Signers)
        {
          string str1 = signer.UnparsedName.ToLower();
          if (signer.FirstName != null && signer.LastName != null)
          {
            str1 = signer.FirstName.ToLower();
            if (signer.MiddleName != string.Empty)
              str1 = str1 + " " + signer.MiddleName.ToLower();
            if (signer.LastName != string.Empty)
              str1 = str1 + " " + signer.LastName.ToLower();
            if (signer.SuffixName != string.Empty)
              str1 = str1 + " " + signer.SuffixName.ToLower();
          }
          string str2 = str1 + ":" + signer.EmailAddress;
          if (!stringList.Contains(str2))
            stringList.Add(str2);
        }
        if (stringList.Count != msg.Signers.Length)
          this.invalidList.Add("You cannot use the '" + this.cboSigningOption.Text + "' signing option because two of your signers have the same name and email address.");
        if (msg.Signers.Length == 0)
          this.invalidList.Add("You cannot use the '" + this.cboSigningOption.Text + "' signing option because none of the selected documents have eSignature points.");
        if (msg.AnswerQuestions)
        {
          bool flag = true;
          foreach (LoanCenterSigner signer in msg.Signers)
          {
            if ((signer.EntityType == LoanCenterEntityType.Borrower || signer.EntityType == LoanCenterEntityType.Coborrower || signer.EntityType == LoanCenterEntityType.NonBorrowingOwner) && signer.SecurityQuestions.Length < 5)
              flag = false;
          }
          if (!flag)
            this.invalidList.Add("You cannot use the '" + this.cboAuthentication.Text + "' authentication method because there is not enough information available in the loan file.");
        }
      }
      if (this.invalidList.Count <= 0)
        return true;
      string str = string.Empty;
      for (int index = 0; index < this.invalidList.Count; ++index)
        str = str + "\n\n(" + Convert.ToString(index + 1) + ") " + this.invalidList[index];
      int num = (int) Utils.Dialog((IWin32Window) this, "You must address the following issues before you can send the package:" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private bool sendMessage(LoanCenterMessage msg)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 993, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
      using (LoanCenterClient loanCenterClient = new LoanCenterClient())
      {
        this.packageID = loanCenterClient.SendMessage(msg);
        this.OriginatorSignerUrl = loanCenterClient.OriginatorSignerUrl;
        if (this.packageID == null)
          return false;
      }
      string str1 = msg.ReadReceipt ? "Yes" : "No";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("From: " + msg.ReplyTo);
      if (msg.To != string.Empty)
        stringBuilder.AppendLine("Borrower: " + msg.To);
      if (msg.CC != string.Empty)
        stringBuilder.AppendLine("Co-Borrower: " + msg.CC);
      if (this.signers.Any<LoanCenterSigner>((Func<LoanCenterSigner, bool>) (x => x.EntityType == LoanCenterEntityType.NonBorrowingOwner)))
        stringBuilder.AppendLine("Non-Borrowing Owner: " + string.Join(", ", this.signers.FindAll((Predicate<LoanCenterSigner>) (x => x.EntityType == LoanCenterEntityType.NonBorrowingOwner)).Select<LoanCenterSigner, string>((Func<LoanCenterSigner, string>) (y => y.EmailAddress)).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str)))));
      stringBuilder.AppendLine("Subject: " + msg.Subject);
      stringBuilder.AppendLine("Read Receipt: " + str1);
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(msg.Body);
      this.comments = stringBuilder.ToString();
      PerformanceMeter.Current.AddCheckpoint("END", 1024, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
      return true;
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 1033, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
      this.invalidList.Clear();
      if ((this.cboSigningOption.Text.Equals("eSign (electronically sign and return)") || this.cboSigningOption.Text.Equals("eSign + Wet Sign (for wet sign only documents)")) && (this.requiredSigners == null || this.requiredSigners.Count == 0))
      {
        PerformanceMeter.Current.AddCheckpoint("BEFORE no-borrower Warning Dialog", 1041, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
        int num = (int) Utils.Dialog((IWin32Window) this, "The documents in this package have no borrower or co-borrower eSignature points. Add eSignature points to the documents or convert the package into a wet signing package. ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        PerformanceMeter.Current.AddCheckpoint("EXIT AFTER no-borrower Warning Dialog", 1043, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
      }
      else if (!this.pnlRecipients.Controls.OfType<RecipientControl>().Any<RecipientControl>((Func<RecipientControl, bool>) (ctrl => ctrl.cbSelect.Checked)))
      {
        PerformanceMeter.Current.AddCheckpoint("BEFORE no-recipient Warning Dialog", 1053, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select recipient(s)", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        PerformanceMeter.Current.AddCheckpoint("EXIT AFTER no-recipient Warning Dialog", 1055, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
      }
      else
      {
        this.validateSenderContactDetails();
        this.validateSigners();
        LoanCenterMessage message = this.createMessage();
        if (!this.validateMessage(message))
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT not validateMessage", 1071, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
        }
        else
        {
          string text = this.checkAutoSigners(message);
          if (text == null)
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT null checkAutoSigners confirmationMessage", 1079, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
          }
          else
          {
            this.saveSenderContactDetails();
            this.saveSigners();
            PerformanceMeter.Current.AddCheckpoint("Save Borrower Details", 1089, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
            if (!this.sendMessage(message))
            {
              PerformanceMeter.Current.AddCheckpoint("EXIT if not sendMessage", 1094, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
            }
            else
            {
              PerformanceMeter.Current.AddCheckpoint("Send Package", 1098, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
              HtmlEmailTemplate selectedItem = this.cboSubject.SelectedItem as HtmlEmailTemplate;
              if (selectedItem != (HtmlEmailTemplate) null)
                Session.WritePrivateProfileString("EmailTemplates", this.sessionEmailProfileName, selectedItem.Guid);
              if (this.shouldLODocuSignEmbeddedSigning(message, this.OriginatorSignerUrl))
              {
                DocuSignSigningDialog form = new DocuSignSigningDialog(this.OriginatorSignerUrl);
                this.setWindowDisplay(form);
                PerformanceMeter.Current.AddCheckpoint("BEFORE browser.ShowDialog()", 1110, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
                int num = (int) form.ShowDialog((IWin32Window) Form.ActiveForm);
                PerformanceMeter.Current.AddCheckpoint("AFTER browser.ShowDialog()", 1112, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
                if (this.signOrderSetup != null && this.signOrderSetup.States.Count > 0)
                {
                  bool boolean = Convert.ToBoolean(this.signOrderSetup.SignOrderEnabled);
                  bool flag = false;
                  this.signOrderSetup.States.TryGetValue(this.loanDataMgr.LoanData.GetField("14").ToUpper(), out flag);
                  if (boolean & flag)
                    text = "An email message has been sent to the Originator.";
                }
                PerformanceMeter.Current.AddCheckpoint("LO eSigning", 1128, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
              }
              this.isWetSign = this.cboSigningOption.SelectedItem.Equals((object) "Wet Sign (print, sign, and fax)") || this.cboSigningOption.SelectedItem.Equals((object) "No Signature Required");
              PerformanceMeter.Current.AddCheckpoint("Before Confirmation Dialog", 1136, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
              int num1 = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              PerformanceMeter.Current.AddCheckpoint("After Confirmation Dialog", 1138, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
              this.DialogResult = DialogResult.OK;
              PerformanceMeter.Current.AddCheckpoint("END", 1141, nameof (btnSend_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\LoanCenter\\SendDisclosuresDialog.cs");
            }
          }
        }
      }
    }

    private void setWindowDisplay(DocuSignSigningDialog form)
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

    private bool shouldLODocuSignEmbeddedSigning(LoanCenterMessage msg, string originatorSignerUrl)
    {
      if (!originatorSignerUrl.Contains("docusign"))
        return false;
      foreach (LoanCenterSigner signer in msg.Signers)
      {
        if (signer.EntityType == LoanCenterEntityType.Originator && string.Compare(Session.UserID, signer.UserID, StringComparison.OrdinalIgnoreCase) == 0)
          return signer.AutoSign;
      }
      return false;
    }

    private string checkAutoSigners(LoanCenterMessage msg)
    {
      foreach (LoanCenterSigner signer in msg.Signers)
      {
        if (signer.EntityType == LoanCenterEntityType.Originator)
        {
          string str1 = string.Empty;
          foreach (LoanCenterAttachment attachment in msg.Attachments)
          {
            if (attachment is DocumentAttachment && signer.RequiresSigning((DocumentAttachment) attachment))
              str1 = str1 + attachment.Title + Environment.NewLine;
          }
          if (string.Compare(Session.UserID, signer.UserID, StringComparison.OrdinalIgnoreCase) == 0)
          {
            switch (Utils.Dialog((IWin32Window) this, "This package includes one or more documents that you must eSign before you can retrieve the borrower-signed documents. It is strongly recommended that you sign prior to the borrowers. Click \"Yes\" to apply your eSignature to the documents now. Click \"No\" to receive an email with a link to a secure website where you can eSign later." + Environment.NewLine + Environment.NewLine + "Originator eSignable Documents:" + Environment.NewLine + Environment.NewLine + str1, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
              case DialogResult.Cancel:
                return (string) null;
              case DialogResult.Yes:
                signer.AutoSign = true;
                continue;
              case DialogResult.No:
                string str2 = "The disclosure package has been sent to the borrower.";
                if (this.signOrderSetup != null && this.signOrderSetup.States.Count > 0)
                {
                  bool boolean = Convert.ToBoolean(this.signOrderSetup.SignOrderEnabled);
                  bool flag = false;
                  this.signOrderSetup.States.TryGetValue(this.loanDataMgr.LoanData.GetField("14").ToUpper(), out flag);
                  if (boolean & flag)
                    str2 = "An email message has been sent to the Originator.";
                }
                return str2;
              default:
                continue;
            }
          }
          else
            return Utils.Dialog((IWin32Window) this, "This package includes one or more documents that require the Originator's eSignature. The originator, " + signer.UnparsedName + ", will be notified via email." + Environment.NewLine + Environment.NewLine + "Originator eSignable Documents:" + Environment.NewLine + Environment.NewLine + str1, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK ? "An email message has been sent to the Originator." : (string) null;
        }
      }
      return "The disclosure package has been sent to the borrower.";
    }

    private void cboSigningOption_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool eSigning = this.cboSigningOption.Text.Equals("eSign (electronically sign and return)") || this.cboSigningOption.Text.Equals("eSign + Wet Sign (for wet sign only documents)");
      this.cboAuthentication.Text.Equals("Authorization Code");
      this.lblAuthentication.Visible = eSigning;
      this.cboAuthentication.Visible = eSigning;
      this.refreshRecipients(eSigning);
      this.btnAddRecipient.Visible = !eSigning;
      if (!eSigning)
      {
        this.removeSigner(LoanCenterEntityType.Originator.ToString("g"));
        this.addSigner(this.populateBorrower());
        this.addSigner(this.populateCoborrower());
        this.populateNonBorrowingOwner().ForEach(new Action<LoanCenterSigner>(this.addSigner));
      }
      this.enableAuthorizationCode();
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
      HtmlEmailTemplate selectedItem = this.cboSubject.SelectedItem as HtmlEmailTemplate;
      if (selectedItem == (HtmlEmailTemplate) null)
        return;
      UserInfo userInfo = (UserInfo) null;
      int selectedIndex = this.cmbSenderType.SelectedIndex;
      if (selectedIndex >= 0 && selectedIndex < 3)
      {
        switch (selectedIndex)
        {
          case 0:
            userInfo = Session.UserInfo;
            break;
          case 1:
            userInfo = Session.OrganizationManager.GetUser(this.cmbSenderType.Text);
            break;
          case 2:
            userInfo = Session.OrganizationManager.GetUser(this.cmbSenderType.Text);
            break;
        }
      }
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
      string html = new HtmlFieldMerge(selectedItem.Html)
      {
        InformationalList = str3,
        SignAndReturnList = str1,
        NeededList = str2
      }.MergeContent(this.loanDataMgr.LoanData, userInfo);
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

    private void cmbSenderType_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = this.cmbSenderType.SelectedIndex;
      if (selectedIndex < 0 || selectedIndex > 2)
        return;
      string text = this.cmbSenderType.Text;
      string str1 = this.SenderData[text].Item2;
      string str2 = this.SenderData[text].Item3;
      this.txtSenderName.Text = str2;
      this.txtSenderName.ReadOnly = !string.IsNullOrWhiteSpace(str2);
      this.txtSenderEmail.Text = str1;
      this.txtSenderEmail.ReadOnly = !string.IsNullOrWhiteSpace(str1);
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
      contactDetails.Add("SenderType", (this.cmbSenderType.SelectedItem as FieldOption).Text);
      EmailFrom.SaveFromUser(this.loanDataMgr, contactDetails);
    }

    private void saveSigners()
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
              LoanCenterSigner loanCenterSigner = this.signers.Find((Predicate<LoanCenterSigner>) (x => x.SignerID == recipientcontrol.Name));
              if (contactDetails.ContainsKey("Email"))
                loanCenterSigner.EmailAddress = contactDetails["Email"];
              if (contactDetails.ContainsKey("Name"))
                loanCenterSigner.UnparsedName = contactDetails["Name"];
              new FileContacts(this.loan).UpdateContactDetails(contactDetails);
            }
          }
        }
      }
    }

    private LoanCenterSigner populateBorrower()
    {
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      LoanCenterSigner loanCenterSigner = new LoanCenterSigner("1", LoanCenterEntityType.Borrower)
      {
        FirstName = this.loan.GetSimpleField("4000"),
        MiddleName = this.loan.GetSimpleField("4001"),
        LastName = this.loan.GetSimpleField("4002"),
        SuffixName = this.loan.GetSimpleField("4003")
      };
      loanCenterSigner.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
      {
        loanCenterSigner.FirstName,
        loanCenterSigner.MiddleName,
        loanCenterSigner.LastName,
        loanCenterSigner.SuffixName
      }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      loanCenterSigner.EmailAddress = this.loan.GetSimpleField("1240");
      loanCenterSigner.SignatureField = "BorrowerSignature_" + pairIndex.ToString();
      loanCenterSigner.InitialsField = "BorrowerInitials_" + pairIndex.ToString();
      loanCenterSigner.CheckboxField = "BorrowerCheckbox_" + pairIndex.ToString();
      loanCenterSigner.RecipientType = "Borrower";
      return loanCenterSigner;
    }

    private LoanCenterSigner populateCoborrower()
    {
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      string simpleField1 = this.loan.GetSimpleField("4004");
      string simpleField2 = this.loan.GetSimpleField("1268");
      if (string.IsNullOrEmpty(simpleField1) && string.IsNullOrEmpty(simpleField2))
        return (LoanCenterSigner) null;
      LoanCenterSigner loanCenterSigner = new LoanCenterSigner("2", LoanCenterEntityType.Coborrower)
      {
        FirstName = simpleField1,
        MiddleName = this.loan.GetSimpleField("4005"),
        LastName = this.loan.GetSimpleField("4006"),
        SuffixName = this.loan.GetSimpleField("4007")
      };
      loanCenterSigner.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
      {
        loanCenterSigner.FirstName,
        loanCenterSigner.MiddleName,
        loanCenterSigner.LastName,
        loanCenterSigner.SuffixName
      }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      loanCenterSigner.EmailAddress = simpleField2;
      loanCenterSigner.SignatureField = "CoborrowerSignature_" + pairIndex.ToString();
      loanCenterSigner.InitialsField = "CoborrowerInitials_" + pairIndex.ToString();
      loanCenterSigner.CheckboxField = "CoborrowerCheckbox_" + pairIndex.ToString();
      loanCenterSigner.RecipientType = "Coborrower";
      return loanCenterSigner;
    }

    private LoanCenterSigner populateOriginator(string originatorUserID)
    {
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      UserInfo user = Session.OrganizationManager.GetUser(originatorUserID);
      if (!(user != (UserInfo) null))
        return (LoanCenterSigner) null;
      if (this.loan.GetSimpleField("1612") == string.Empty)
      {
        string fullName = user.FullName;
      }
      LoanCenterSigner loanCenterSigner = new LoanCenterSigner("3", LoanCenterEntityType.Originator)
      {
        FirstName = user.FirstName,
        MiddleName = user.MiddleName,
        SuffixName = user.SuffixName,
        LastName = user.LastName
      };
      loanCenterSigner.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
      {
        loanCenterSigner.FirstName,
        loanCenterSigner.MiddleName,
        loanCenterSigner.LastName,
        loanCenterSigner.SuffixName
      }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
      loanCenterSigner.EmailAddress = user.Email;
      loanCenterSigner.UserID = originatorUserID;
      loanCenterSigner.SignatureField = "OriginatorSignature_" + pairIndex.ToString();
      loanCenterSigner.InitialsField = "OriginatorInitials_" + pairIndex.ToString();
      loanCenterSigner.RecipientType = "Originator";
      return loanCenterSigner;
    }

    private List<LoanCenterSigner> populateNonBorrowingOwner()
    {
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      List<LoanCenterSigner> loanCenterSignerList = new List<LoanCenterSigner>();
      List<NonBorrowingOwner> byBorrowerPairId = this.loan.GetNboByBorrowerPairId(this.loan.GetBorrowerPairs()[pairIndex].Id);
      int num = 0;
      foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
      {
        LoanCenterSigner loanCenterSigner = new LoanCenterSigner("4", LoanCenterEntityType.NonBorrowingOwner)
        {
          FirstName = nonBorrowingOwner.FirstName,
          MiddleName = nonBorrowingOwner.MiddleName,
          LastName = nonBorrowingOwner.LastName,
          SuffixName = nonBorrowingOwner.SuffixName
        };
        loanCenterSigner.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          loanCenterSigner.FirstName,
          loanCenterSigner.MiddleName,
          loanCenterSigner.LastName,
          loanCenterSigner.SuffixName
        }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
        loanCenterSigner.EmailAddress = nonBorrowingOwner.EmailAddress;
        loanCenterSigner.SignatureField = "NonBorrowingOwnerSignature_" + pairIndex.ToString() + "_" + num.ToString();
        loanCenterSigner.InitialsField = "NonBorrowingOwnerInitials_" + pairIndex.ToString() + "_" + num.ToString();
        loanCenterSigner.CheckboxField = "NonBorrowingOwnerCheckbox_" + pairIndex.ToString() + "_" + num.ToString();
        loanCenterSigner.RecipientType = LoanCenterEntityType.NonBorrowingOwner.ToString("g");
        loanCenterSigner.SignerID = nonBorrowingOwner.NBOID;
        loanCenterSigner.NboIndex = nonBorrowingOwner.NboIndex;
        loanCenterSigner.NBOVestingIndex = nonBorrowingOwner.NBOVestingIndex;
        ++num;
        loanCenterSignerList.Add(loanCenterSigner);
      }
      return loanCenterSignerList;
    }

    private void addSigner(LoanCenterSigner signer)
    {
      if (signer == null)
        return;
      if (this.signers == null)
        this.signers = new List<LoanCenterSigner>();
      if (!this.signers.Exists((Predicate<LoanCenterSigner>) (x =>
      {
        if (x.SignerID != "0" && x.SignerID == signer.SignerID)
          return true;
        return x.SignerID == "0" && x.RecipientType.ToLower() == signer.RecipientType.ToLower();
      })))
        this.signers.Add(signer);
      if (((IEnumerable<Control>) this.pnlRecipients.Controls.Find(signer.SignerID != "0" ? signer.SignerID : signer.RecipientType, true)).FirstOrDefault<Control>() != null)
        return;
      RecipientControl recipientControl = new RecipientControl();
      recipientControl.Name = signer.SignerID != "0" ? signer.SignerID : signer.RecipientType;
      if (!string.IsNullOrEmpty(signer.UnparsedName))
      {
        recipientControl.txtName.Text = signer.UnparsedName;
        recipientControl.txtName.ReadOnly = true;
      }
      else if (!string.IsNullOrEmpty(signer.FirstName) || !string.IsNullOrEmpty(signer.LastName))
      {
        recipientControl.txtName.Text = signer.FirstName + " " + signer.LastName;
        recipientControl.txtName.ReadOnly = true;
      }
      else
        recipientControl.txtName.ReadOnly = false;
      if (!string.IsNullOrEmpty(signer.EmailAddress))
      {
        recipientControl.txtEmail.Text = signer.EmailAddress;
        recipientControl.txtEmail.ReadOnly = true;
      }
      else
        recipientControl.txtEmail.ReadOnly = false;
      if (signer.EntityType == LoanCenterEntityType.Originator || signer.EntityType == LoanCenterEntityType.Unknown)
        recipientControl.txtAuthenticationCode.Visible = false;
      recipientControl.txtAuthenticationCode.ReadOnly = false;
      recipientControl.cbSelect.Text = signer.RecipientType == LoanCenterEntityType.NonBorrowingOwner.ToString("g") ? "Non-Borrowing Owner" : signer.RecipientType;
      recipientControl.Tag = (object) signer.NboIndex;
      this.pnlRecipients.Controls.Add((Control) recipientControl);
    }

    private void cboAuthentication_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.enableAuthorizationCode();
    }

    private void enableAuthorizationCode()
    {
      bool flag1 = this.cboAuthentication.Text.Equals("Authorization Code");
      bool flag2 = this.cboSigningOption.Text.Equals("eSign (electronically sign and return)") || this.cboSigningOption.Text.Equals("eSign + Wet Sign (for wet sign only documents)");
      this.lblAuthorizationCode.Visible = flag1 & flag2;
      foreach (Control control in (ArrangedElementCollection) this.pnlRecipients.Controls)
      {
        if (control != null && control.GetType() == typeof (RecipientControl))
        {
          RecipientControl recipientControl = control as RecipientControl;
          if (recipientControl.cbSelect.Text == "Originator")
            recipientControl.txtAuthenticationCode.Visible = false;
          else
            recipientControl.txtAuthenticationCode.Visible = flag1 & flag2;
        }
      }
    }

    private int removeSigner(string signerType)
    {
      int num = 0;
      foreach (LoanCenterSigner loanCenterSigner in this.signers.FindAll((Predicate<LoanCenterSigner>) (x => x.RecipientType == signerType)))
      {
        Control control = ((IEnumerable<Control>) this.pnlRecipients.Controls.Find(loanCenterSigner.SignerID != "0" ? loanCenterSigner.SignerID : loanCenterSigner.RecipientType, true)).FirstOrDefault<Control>();
        if (control != null)
        {
          this.pnlRecipients.Controls.Remove(control);
          if (this.deletedSigners == null)
            this.deletedSigners = new List<LoanCenterSigner>();
          this.deletedSigners.Add(loanCenterSigner);
          this.signers.Remove(loanCenterSigner);
          ++num;
        }
      }
      return num;
    }

    private void refreshRecipients(bool eSigning)
    {
      if (eSigning)
      {
        foreach (string requiredSigner in this.requiredSigners)
          this.addSigner(requiredSigner);
        if (this.signers != null && this.signers.Count != 0)
        {
          for (int index = this.signers.Count - 1; index >= 0; --index)
          {
            if (!this.requiredSigners.Contains(this.signers[index].RecipientType))
            {
              int num = this.removeSigner(this.signers[index].RecipientType);
              index -= num <= 1 ? 0 : num - 1;
            }
          }
        }
        if (this.signers != null && this.signers.Count != 0)
          return;
        this.pnlRecipients.Controls.Clear();
      }
      else
      {
        if (this.deletedSigners == null)
          return;
        foreach (LoanCenterSigner signer in this.deletedSigners.FindAll((Predicate<LoanCenterSigner>) (x => x.Ischecked)))
          this.addSigner(signer);
      }
    }

    private void addSigner(string signerType)
    {
      if (this.deletedSigners != null)
      {
        foreach (LoanCenterSigner loanCenterSigner in this.deletedSigners.FindAll((Predicate<LoanCenterSigner>) (x => x.RecipientType == signerType)))
        {
          LoanCenterSigner deletedRecipient = loanCenterSigner;
          if (this.signers.Find((Predicate<LoanCenterSigner>) (x =>
          {
            if (x.SignerID != "0" && x.SignerID == deletedRecipient.SignerID)
              return true;
            return x.SignerID == "0" && x.RecipientType.ToLower() == deletedRecipient.RecipientType.ToLower();
          })) == null)
            this.signers.Add(deletedRecipient);
          this.addSigner(deletedRecipient);
        }
      }
      else
      {
        foreach (LoanCenterSigner signer in this.signers.FindAll((Predicate<LoanCenterSigner>) (x => x.RecipientType == signerType)))
          this.addSigner(signer);
      }
    }

    private List<string> getRequiredSigners()
    {
      List<string> requiredSigners = new List<string>();
      eDeliveryMessage eDeliveryMessage = new eDeliveryMessage(this.loanDataMgr, eDeliveryMessageType.RequestDocuments);
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      foreach (DocumentAttachment attachment in this.attachmentList)
      {
        if (eDeliveryMessage.checkSigningpointExists(attachment, LoanCenterEntityType.NonBorrowingOwner.ToString("g")) && !flag3)
        {
          flag3 = true;
          requiredSigners.Add(LoanCenterEntityType.NonBorrowingOwner.ToString("g"));
        }
        if (eDeliveryMessage.checkSigningpointExists(attachment, LoanCenterEntityType.Borrower.ToString("g")) && !flag1)
        {
          flag1 = true;
          requiredSigners.Add(LoanCenterEntityType.Borrower.ToString("g"));
        }
        if (eDeliveryMessage.checkSigningpointExists(attachment, LoanCenterEntityType.Coborrower.ToString("g")) && !flag2)
        {
          flag2 = true;
          requiredSigners.Add(LoanCenterEntityType.Coborrower.ToString("g"));
        }
        if (flag1 & flag2 & flag3)
          break;
      }
      foreach (DocumentAttachment documentAttachment in this.attachmentList.FindAll((Predicate<DocumentAttachment>) (x => x.SignatureFields != null)))
      {
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(LoanCenterEntityType.Borrower.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null)
          requiredSigners.Add(LoanCenterEntityType.Borrower.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(LoanCenterEntityType.Coborrower.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null)
          requiredSigners.Add(LoanCenterEntityType.Coborrower.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(LoanCenterEntityType.NonBorrowingOwner.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null)
          requiredSigners.Add(LoanCenterEntityType.NonBorrowingOwner.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(LoanCenterEntityType.Originator.ToString("g"), StringComparison.InvariantCultureIgnoreCase))) != null)
          requiredSigners.Add(LoanCenterEntityType.Originator.ToString("g"));
      }
      return requiredSigners;
    }

    private void btnAddRecipient_Click(object sender, EventArgs e)
    {
      using (EllieMae.EMLite.ePass.EmailListDialog emailListDialog = new EllieMae.EMLite.ePass.EmailListDialog(this.loan))
      {
        if (emailListDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        foreach (string selectedContact in emailListDialog.selectedContacts)
        {
          char[] chArray = new char[1]{ ';' };
          string[] strArray = selectedContact.Split(chArray);
          LoanCenterSigner signer;
          if (strArray[2] == "Borrower")
            signer = this.populateBorrower();
          else if (strArray[2] == "Coborrower")
          {
            signer = this.populateCoborrower();
          }
          else
          {
            signer = new LoanCenterSigner("0", LoanCenterEntityType.Unknown);
            signer.UnparsedName = strArray[0];
            signer.EmailAddress = strArray[1];
            signer.RecipientType = strArray[2];
          }
          this.addSigner(signer);
        }
        int num = 0;
        foreach (NonBorrowingOwner nonBorrowingOwner in emailListDialog.selectedNbo)
        {
          LoanCenterSigner signer = new LoanCenterSigner("4", LoanCenterEntityType.NonBorrowingOwner)
          {
            FirstName = nonBorrowingOwner.FirstName,
            MiddleName = nonBorrowingOwner.MiddleName,
            LastName = nonBorrowingOwner.LastName,
            SuffixName = nonBorrowingOwner.SuffixName
          };
          signer.UnparsedName = string.Join(" ", ((IEnumerable<string>) new string[4]
          {
            signer.FirstName,
            signer.MiddleName,
            signer.LastName,
            signer.SuffixName
          }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
          signer.EmailAddress = nonBorrowingOwner.EmailAddress;
          signer.RecipientType = LoanCenterEntityType.NonBorrowingOwner.ToString("g");
          signer.SignerID = nonBorrowingOwner.NBOID;
          signer.NboIndex = nonBorrowingOwner.NboIndex;
          ++num;
          this.addSigner(signer);
        }
      }
    }

    private void validateSigners()
    {
      string htmlBodyText = this.htmlEmailControl.HtmlBodyText;
      foreach (Control control in (ArrangedElementCollection) this.pnlRecipients.Controls)
      {
        if (control != null && control.GetType() == typeof (RecipientControl))
        {
          RecipientControl recipientcontrol = control as RecipientControl;
          if (recipientcontrol.cbSelect.Checked)
          {
            LoanCenterSigner loanCenterSigner = this.signers.Find((Predicate<LoanCenterSigner>) (x => x.SignerID != null && x.SignerID == recipientcontrol.Name || x.RecipientType.ToLower() == recipientcontrol.Name.ToLower()));
            if (loanCenterSigner == null)
            {
              loanCenterSigner = this.deletedSigners.Find((Predicate<LoanCenterSigner>) (x =>
              {
                if (x.SignerID != "0" && x.SignerID == recipientcontrol.Name)
                  return true;
                return x.SignerID == "0" && x.RecipientType.ToLower() == recipientcontrol.Name.ToLower();
              }));
              this.signers.Add(loanCenterSigner);
              this.deletedSigners.Remove(loanCenterSigner);
              loanCenterSigner.Ischecked = true;
            }
            if (!recipientcontrol.txtName.ReadOnly && string.IsNullOrWhiteSpace(recipientcontrol.txtName.Text))
              this.invalidList.Add(recipientcontrol.cbSelect.Text + "'s Name is Required");
            if (!recipientcontrol.txtEmail.ReadOnly && string.IsNullOrWhiteSpace(recipientcontrol.txtEmail.Text))
              this.invalidList.Add(recipientcontrol.cbSelect.Text + "'s Email is Required");
            else if (!recipientcontrol.txtEmail.ReadOnly && !Utils.ValidateEmail(recipientcontrol.txtEmail.Text))
              this.invalidList.Add("Enter valid value for " + recipientcontrol.cbSelect.Text + "'s Email");
            else if (string.IsNullOrEmpty(loanCenterSigner.EmailAddress))
              loanCenterSigner.EmailAddress = recipientcontrol.txtEmail.Text;
            if (recipientcontrol.txtAuthenticationCode.Visible && string.IsNullOrWhiteSpace(recipientcontrol.txtAuthenticationCode.Text))
              this.invalidList.Add(recipientcontrol.cbSelect.Text + "'s Authentication Code is Required");
            else if (recipientcontrol.txtAuthenticationCode.Visible && !this.ValidateAuthenticationCode(recipientcontrol.txtAuthenticationCode.Text))
              this.invalidList.Add("Enter valid value for " + recipientcontrol.cbSelect.Text + "'s Authentication Code");
            else
              loanCenterSigner.AuthCode = recipientcontrol.txtAuthenticationCode.Text;
          }
          else
          {
            LoanCenterSigner loanCenterSigner = this.signers.Find((Predicate<LoanCenterSigner>) (x => x.SignerID != null && x.SignerID == recipientcontrol.Name || x.RecipientType.ToLower() == recipientcontrol.Name.ToLower()));
            if (loanCenterSigner != null)
            {
              if (this.deletedSigners == null)
                this.deletedSigners = new List<LoanCenterSigner>();
              this.deletedSigners.Add(loanCenterSigner);
              this.signers.Remove(loanCenterSigner);
              loanCenterSigner.Ischecked = false;
            }
          }
          if (htmlBodyText != null && (recipientcontrol.txtAuthenticationCode.ReadOnly || recipientcontrol.cbSelect.Checked) && !string.IsNullOrEmpty(recipientcontrol.txtAuthenticationCode.Text) && htmlBodyText.Contains(recipientcontrol.txtAuthenticationCode.Text))
            this.invalidList.Add("Message may not contain the " + recipientcontrol.cbSelect.Text + " Authorization Code used for signing.");
        }
      }
    }

    private bool ValidateAuthenticationCode(string authenticationCode)
    {
      return int.TryParse(authenticationCode, out int _) && authenticationCode.All<char>(new Func<char, bool>(char.IsDigit));
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnSend = new Button();
      this.gcFulfillment = new GroupContainer();
      this.chkFulfillment = new CheckBox();
      this.pnlFulfillment = new Panel();
      this.txtFromZip = new TextBox();
      this.dateFulfillBy = new DateTimePicker();
      this.lblGfeDate = new Label();
      this.lblFulfillBy = new Label();
      this.txtGfeDate = new TextBox();
      this.lblMailFrom = new Label();
      this.txtFromName = new TextBox();
      this.lblFromName = new Label();
      this.txtFromAddress = new TextBox();
      this.lblFromAddress1 = new Label();
      this.txtFromCity = new TextBox();
      this.lblFromAddress2 = new Label();
      this.txtFromPhone = new TextBox();
      this.lblFromPhone = new Label();
      this.txtFromState = new TextBox();
      this.btnCopyAddress = new Button();
      this.txtToCoborrowerZip = new TextBox();
      this.txtToCoborrowerState = new TextBox();
      this.lblToCoborrowerPhone = new Label();
      this.txtToCoborrowerPhone = new TextBox();
      this.lblToCoborrowerAddress2 = new Label();
      this.txtToCoborrowerCity = new TextBox();
      this.lblToCoborrowerAddress = new Label();
      this.lblMailToCoborrower = new Label();
      this.txtToCoborrowerAddress = new TextBox();
      this.txtToCoborrowerName = new TextBox();
      this.lblToCoborrowerName = new Label();
      this.lblFulfillment = new Label();
      this.txtToBorrowerZip = new TextBox();
      this.txtToBorrowerState = new TextBox();
      this.lblToBorrowerPhone = new Label();
      this.txtToBorrowerPhone = new TextBox();
      this.lblToBorrowerAddress2 = new Label();
      this.txtToBorrowerCity = new TextBox();
      this.lblToBorrowerAddress = new Label();
      this.lblMailToBorrower = new Label();
      this.txtToBorrowerAddress = new TextBox();
      this.txtToBorrowerName = new TextBox();
      this.lblToBorrowerName = new Label();
      this.gcSigning = new GroupContainer();
      this.cboAuthentication = new ComboBox();
      this.lblAuthentication = new Label();
      this.cboSigningOption = new ComboBox();
      this.lblSigningOption = new Label();
      this.gcMessage = new GroupContainer();
      this.emHelpLink = new EMHelpLink();
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
      this.pnlRecipients = new FlowLayoutPanel();
      this.panelDlg = new Panel();
      this.btnNotifyUsers = new Button();
      this.lblNotifyUserCount = new Label();
      this.cboSubject = new ComboBox();
      this.htmlEmailControl = new HtmlEmailControl();
      this.dateAcceptBy = new DateTimePicker();
      this.chkAcceptBy = new CheckBox();
      this.chkReadReceipt = new CheckBox();
      this.lblSubject = new Label();
      this.lblRequired = new Label();
      this.gcFulfillment.SuspendLayout();
      this.pnlFulfillment.SuspendLayout();
      this.gcSigning.SuspendLayout();
      this.gcMessage.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel6.SuspendLayout();
      ((ISupportInitialize) this.btnAddRecipient).BeginInit();
      this.pnlRecipients.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(965, 630);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 25);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSend.Location = new Point(882, 630);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 25);
      this.btnSend.TabIndex = 4;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.gcFulfillment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.gcFulfillment.Controls.Add((Control) this.chkFulfillment);
      this.gcFulfillment.Controls.Add((Control) this.pnlFulfillment);
      this.gcFulfillment.HeaderForeColor = SystemColors.ControlText;
      this.gcFulfillment.Location = new Point(620, 5);
      this.gcFulfillment.Name = "gcFulfillment";
      this.gcFulfillment.Size = new Size(419, 625);
      this.gcFulfillment.TabIndex = 2;
      this.gcFulfillment.Text = "Fulfillment";
      this.chkFulfillment.AutoSize = true;
      this.chkFulfillment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkFulfillment.Location = new Point(12, 30);
      this.chkFulfillment.Name = "chkFulfillment";
      this.chkFulfillment.Size = new Size(254, 18);
      this.chkFulfillment.TabIndex = 1;
      this.chkFulfillment.Text = "Schedule a Fulfillment Service for this package.";
      this.chkFulfillment.UseVisualStyleBackColor = true;
      this.chkFulfillment.CheckedChanged += new EventHandler(this.chkFulfillment_CheckedChanged);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromZip);
      this.pnlFulfillment.Controls.Add((Control) this.dateFulfillBy);
      this.pnlFulfillment.Controls.Add((Control) this.lblGfeDate);
      this.pnlFulfillment.Controls.Add((Control) this.lblFulfillBy);
      this.pnlFulfillment.Controls.Add((Control) this.txtGfeDate);
      this.pnlFulfillment.Controls.Add((Control) this.lblMailFrom);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromName);
      this.pnlFulfillment.Controls.Add((Control) this.lblFromName);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromAddress);
      this.pnlFulfillment.Controls.Add((Control) this.lblFromAddress1);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromCity);
      this.pnlFulfillment.Controls.Add((Control) this.lblFromAddress2);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromPhone);
      this.pnlFulfillment.Controls.Add((Control) this.lblFromPhone);
      this.pnlFulfillment.Controls.Add((Control) this.txtFromState);
      this.pnlFulfillment.Controls.Add((Control) this.btnCopyAddress);
      this.pnlFulfillment.Controls.Add((Control) this.txtToCoborrowerZip);
      this.pnlFulfillment.Controls.Add((Control) this.txtToCoborrowerState);
      this.pnlFulfillment.Controls.Add((Control) this.lblToCoborrowerPhone);
      this.pnlFulfillment.Controls.Add((Control) this.txtToCoborrowerPhone);
      this.pnlFulfillment.Controls.Add((Control) this.lblToCoborrowerAddress2);
      this.pnlFulfillment.Controls.Add((Control) this.txtToCoborrowerCity);
      this.pnlFulfillment.Controls.Add((Control) this.lblToCoborrowerAddress);
      this.pnlFulfillment.Controls.Add((Control) this.lblMailToCoborrower);
      this.pnlFulfillment.Controls.Add((Control) this.txtToCoborrowerAddress);
      this.pnlFulfillment.Controls.Add((Control) this.txtToCoborrowerName);
      this.pnlFulfillment.Controls.Add((Control) this.lblToCoborrowerName);
      this.pnlFulfillment.Controls.Add((Control) this.lblFulfillment);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerZip);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerState);
      this.pnlFulfillment.Controls.Add((Control) this.lblToBorrowerPhone);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerPhone);
      this.pnlFulfillment.Controls.Add((Control) this.lblToBorrowerAddress2);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerCity);
      this.pnlFulfillment.Controls.Add((Control) this.lblToBorrowerAddress);
      this.pnlFulfillment.Controls.Add((Control) this.lblMailToBorrower);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerAddress);
      this.pnlFulfillment.Controls.Add((Control) this.txtToBorrowerName);
      this.pnlFulfillment.Controls.Add((Control) this.lblToBorrowerName);
      this.pnlFulfillment.Dock = DockStyle.Bottom;
      this.pnlFulfillment.Enabled = false;
      this.pnlFulfillment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlFulfillment.Location = new Point(1, 50);
      this.pnlFulfillment.Name = "pnlFulfillment";
      this.pnlFulfillment.Size = new Size(417, 550);
      this.pnlFulfillment.MinimumSize = new Size(417, 550);
      this.pnlFulfillment.TabIndex = 2;
      this.txtFromZip.Location = new Point(323, 195);
      this.txtFromZip.MaxLength = 10;
      this.txtFromZip.Name = "txtFromZip";
      this.txtFromZip.Size = new Size(80, 20);
      this.txtFromZip.TabIndex = 52;
      this.dateFulfillBy.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateFulfillBy.CalendarTitleForeColor = Color.White;
      this.dateFulfillBy.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateFulfillBy.CustomFormat = "MM/dd/yyyy";
      this.dateFulfillBy.Format = DateTimePickerFormat.Custom;
      this.dateFulfillBy.Location = new Point(214, 40);
      this.dateFulfillBy.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.dateFulfillBy.Name = "dateFulfillBy";
      this.dateFulfillBy.Size = new Size(100, 20);
      this.dateFulfillBy.TabIndex = 51;
      this.lblGfeDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.lblGfeDate.AutoSize = true;
      this.lblGfeDate.Location = new Point(23, 16);
      this.lblGfeDate.Name = "lblGfeDate";
      this.lblGfeDate.Size = new Size(107, 14);
      this.lblGfeDate.TabIndex = 48;
      this.lblGfeDate.Text = "GFE Application Date";
      this.lblGfeDate.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFulfillBy.AutoSize = true;
      this.lblFulfillBy.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.lblFulfillBy.Location = new Point(23, 40);
      this.lblFulfillBy.Name = "lblFulfillBy";
      this.lblFulfillBy.Size = new Size(140, 14);
      this.lblFulfillBy.TabIndex = 49;
      this.lblFulfillBy.Text = "* Scheduled Fulfillment Date";
      this.lblFulfillBy.TextAlign = ContentAlignment.MiddleLeft;
      this.txtGfeDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtGfeDate.Location = new Point(214, 16);
      this.txtGfeDate.MaxLength = 10;
      this.txtGfeDate.Name = "txtGfeDate";
      this.txtGfeDate.ReadOnly = true;
      this.txtGfeDate.Size = new Size(100, 20);
      this.txtGfeDate.TabIndex = 50;
      this.lblMailFrom.AutoSize = true;
      this.lblMailFrom.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMailFrom.Location = new Point(13, 125);
      this.lblMailFrom.Name = "lblMailFrom";
      this.lblMailFrom.Size = new Size(36, 14);
      this.lblMailFrom.TabIndex = 38;
      this.lblMailFrom.Text = "From";
      this.txtFromName.Location = new Point(128, 145);
      this.txtFromName.MaxLength = 50;
      this.txtFromName.Name = "txtFromName";
      this.txtFromName.Size = new Size(276, 20);
      this.txtFromName.TabIndex = 40;
      this.lblFromName.AutoSize = true;
      this.lblFromName.Location = new Point(13, 148);
      this.lblFromName.Name = "lblFromName";
      this.lblFromName.Size = new Size(41, 14);
      this.lblFromName.TabIndex = 39;
      this.lblFromName.Text = "* Name";
      this.lblFromName.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFromAddress.Location = new Point(128, 170);
      this.txtFromAddress.MaxLength = 50;
      this.txtFromAddress.Name = "txtFromAddress";
      this.txtFromAddress.Size = new Size(276, 20);
      this.txtFromAddress.TabIndex = 42;
      this.lblFromAddress1.AutoSize = true;
      this.lblFromAddress1.Location = new Point(13, 173);
      this.lblFromAddress1.Name = "lblFromAddress1";
      this.lblFromAddress1.Size = new Size(87, 14);
      this.lblFromAddress1.TabIndex = 41;
      this.lblFromAddress1.Text = "* Street Address";
      this.lblFromAddress1.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFromCity.Location = new Point(128, 195);
      this.txtFromCity.MaxLength = 50;
      this.txtFromCity.Name = "txtFromCity";
      this.txtFromCity.Size = new Size(152, 20);
      this.txtFromCity.TabIndex = 44;
      this.lblFromAddress2.AutoSize = true;
      this.lblFromAddress2.Location = new Point(13, 198);
      this.lblFromAddress2.Name = "lblFromAddress2";
      this.lblFromAddress2.Size = new Size(83, 14);
      this.lblFromAddress2.TabIndex = 43;
      this.lblFromAddress2.Text = "* City, State, Zip";
      this.lblFromAddress2.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFromPhone.Location = new Point((int) sbyte.MaxValue, 220);
      this.txtFromPhone.MaxLength = 20;
      this.txtFromPhone.Name = "txtFromPhone";
      this.txtFromPhone.Size = new Size(152, 20);
      this.txtFromPhone.TabIndex = 47;
      this.lblFromPhone.AutoSize = true;
      this.lblFromPhone.Location = new Point(12, 223);
      this.lblFromPhone.Name = "lblFromPhone";
      this.lblFromPhone.Size = new Size(44, 14);
      this.lblFromPhone.TabIndex = 46;
      this.lblFromPhone.Text = "* Phone";
      this.lblFromPhone.TextAlign = ContentAlignment.MiddleLeft;
      this.txtFromState.Location = new Point(284, 195);
      this.txtFromState.MaxLength = 2;
      this.txtFromState.Name = "txtFromState";
      this.txtFromState.Size = new Size(36, 20);
      this.txtFromState.TabIndex = 45;
      this.btnCopyAddress.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCopyAddress.Location = new Point(231, 250);
      this.btnCopyAddress.Name = "btnCopyAddress";
      this.btnCopyAddress.Size = new Size(173, 22);
      this.btnCopyAddress.TabIndex = 37;
      this.btnCopyAddress.Text = "Same Address As Above";
      this.btnCopyAddress.Visible = false;
      this.btnCopyAddress.Click += new EventHandler(this.btnCopyAddress_Click);
      this.txtToCoborrowerZip.Enabled = false;
      this.txtToCoborrowerZip.Location = new Point(324, 499);
      this.txtToCoborrowerZip.MaxLength = 10;
      this.txtToCoborrowerZip.Name = "txtToCoborrowerZip";
      this.txtToCoborrowerZip.Size = new Size(80, 20);
      this.txtToCoborrowerZip.TabIndex = 34;
      this.txtToCoborrowerZip.Visible = false;
      this.txtToCoborrowerZip.KeyUp += new KeyEventHandler(this.txtToZip_KeyUp);
      this.txtToCoborrowerState.Enabled = false;
      this.txtToCoborrowerState.Location = new Point(284, 499);
      this.txtToCoborrowerState.MaxLength = 2;
      this.txtToCoborrowerState.Name = "txtToCoborrowerState";
      this.txtToCoborrowerState.Size = new Size(36, 20);
      this.txtToCoborrowerState.TabIndex = 33;
      this.txtToCoborrowerState.Visible = false;
      this.txtToCoborrowerState.KeyUp += new KeyEventHandler(this.txtToState_KeyUp);
      this.lblToCoborrowerPhone.AutoSize = true;
      this.lblToCoborrowerPhone.Location = new Point(13, 527);
      this.lblToCoborrowerPhone.Name = "lblToCoborrowerPhone";
      this.lblToCoborrowerPhone.Size = new Size(44, 14);
      this.lblToCoborrowerPhone.TabIndex = 35;
      this.lblToCoborrowerPhone.Text = "* Phone";
      this.lblToCoborrowerPhone.TextAlign = ContentAlignment.MiddleLeft;
      this.lblToCoborrowerPhone.Visible = false;
      this.txtToCoborrowerPhone.Enabled = false;
      this.txtToCoborrowerPhone.Location = new Point((int) sbyte.MaxValue, 524);
      this.txtToCoborrowerPhone.MaxLength = 20;
      this.txtToCoborrowerPhone.Name = "txtToCoborrowerPhone";
      this.txtToCoborrowerPhone.Size = new Size(152, 20);
      this.txtToCoborrowerPhone.TabIndex = 36;
      this.txtToCoborrowerPhone.Visible = false;
      this.txtToCoborrowerPhone.KeyUp += new KeyEventHandler(this.txtToPhone_KeyUp);
      this.lblToCoborrowerAddress2.AutoSize = true;
      this.lblToCoborrowerAddress2.Location = new Point(8, 502);
      this.lblToCoborrowerAddress2.Name = "lblToCoborrowerAddress2";
      this.lblToCoborrowerAddress2.Size = new Size(83, 14);
      this.lblToCoborrowerAddress2.TabIndex = 31;
      this.lblToCoborrowerAddress2.Text = "* City, State, Zip";
      this.lblToCoborrowerAddress2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblToCoborrowerAddress2.Visible = false;
      this.txtToCoborrowerCity.Enabled = false;
      this.txtToCoborrowerCity.Location = new Point(128, 499);
      this.txtToCoborrowerCity.MaxLength = 50;
      this.txtToCoborrowerCity.Name = "txtToCoborrowerCity";
      this.txtToCoborrowerCity.Size = new Size(152, 20);
      this.txtToCoborrowerCity.TabIndex = 32;
      this.txtToCoborrowerCity.Visible = false;
      this.lblToCoborrowerAddress.AutoSize = true;
      this.lblToCoborrowerAddress.Location = new Point(8, 473);
      this.lblToCoborrowerAddress.Name = "lblToCoborrowerAddress";
      this.lblToCoborrowerAddress.Size = new Size(87, 14);
      this.lblToCoborrowerAddress.TabIndex = 29;
      this.lblToCoborrowerAddress.Text = "* Street Address";
      this.lblToCoborrowerAddress.TextAlign = ContentAlignment.MiddleLeft;
      this.lblToCoborrowerAddress.Visible = false;
      this.lblMailToCoborrower.AutoSize = true;
      this.lblMailToCoborrower.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMailToCoborrower.Location = new Point(8, 427);
      this.lblMailToCoborrower.Name = "lblMailToCoborrower";
      this.lblMailToCoborrower.Size = new Size(95, 14);
      this.lblMailToCoborrower.TabIndex = 26;
      this.lblMailToCoborrower.Text = "To Co-Borrower";
      this.lblMailToCoborrower.Visible = false;
      this.txtToCoborrowerAddress.Enabled = false;
      this.txtToCoborrowerAddress.Location = new Point(128, 473);
      this.txtToCoborrowerAddress.MaxLength = 50;
      this.txtToCoborrowerAddress.Name = "txtToCoborrowerAddress";
      this.txtToCoborrowerAddress.Size = new Size(276, 20);
      this.txtToCoborrowerAddress.TabIndex = 30;
      this.txtToCoborrowerAddress.Visible = false;
      this.txtToCoborrowerName.Enabled = false;
      this.txtToCoborrowerName.Location = new Point(128, 447);
      this.txtToCoborrowerName.MaxLength = 50;
      this.txtToCoborrowerName.Name = "txtToCoborrowerName";
      this.txtToCoborrowerName.Size = new Size(276, 20);
      this.txtToCoborrowerName.TabIndex = 28;
      this.txtToCoborrowerName.Visible = false;
      this.lblToCoborrowerName.AutoSize = true;
      this.lblToCoborrowerName.Location = new Point(13, 450);
      this.lblToCoborrowerName.Name = "lblToCoborrowerName";
      this.lblToCoborrowerName.Size = new Size(41, 14);
      this.lblToCoborrowerName.TabIndex = 27;
      this.lblToCoborrowerName.Text = "* Name";
      this.lblToCoborrowerName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblToCoborrowerName.Visible = false;
      this.lblFulfillment.Location = new Point(8, 70);
      this.lblFulfillment.Name = "lblFulfillment";
      this.lblFulfillment.Size = new Size(368, 43);
      this.lblFulfillment.TabIndex = 3;
      this.lblFulfillment.Text = "If the borrower(s) does not access the package by the Scheduled Fulfillment Date, a hard copy of the eDisclosure package will be shipped to the following address.";
      this.txtToBorrowerZip.Location = new Point(323, 349);
      this.txtToBorrowerZip.MaxLength = 10;
      this.txtToBorrowerZip.Name = "txtToBorrowerZip";
      this.txtToBorrowerZip.Size = new Size(80, 20);
      this.txtToBorrowerZip.TabIndex = 23;
      this.txtToBorrowerZip.KeyUp += new KeyEventHandler(this.txtToZip_KeyUp);
      this.txtToBorrowerState.Location = new Point(283, 349);
      this.txtToBorrowerState.MaxLength = 2;
      this.txtToBorrowerState.Name = "txtToBorrowerState";
      this.txtToBorrowerState.Size = new Size(36, 20);
      this.txtToBorrowerState.TabIndex = 22;
      this.txtToBorrowerState.KeyUp += new KeyEventHandler(this.txtToState_KeyUp);
      this.lblToBorrowerPhone.AutoSize = true;
      this.lblToBorrowerPhone.Location = new Point(11, 378);
      this.lblToBorrowerPhone.Name = "lblToBorrowerPhone";
      this.lblToBorrowerPhone.Size = new Size(44, 14);
      this.lblToBorrowerPhone.TabIndex = 24;
      this.lblToBorrowerPhone.Text = "* Phone";
      this.lblToBorrowerPhone.TextAlign = ContentAlignment.MiddleLeft;
      this.txtToBorrowerPhone.Location = new Point(128, 374);
      this.txtToBorrowerPhone.MaxLength = 20;
      this.txtToBorrowerPhone.Name = "txtToBorrowerPhone";
      this.txtToBorrowerPhone.Size = new Size(152, 20);
      this.txtToBorrowerPhone.TabIndex = 25;
      this.txtToBorrowerPhone.KeyUp += new KeyEventHandler(this.txtToPhone_KeyUp);
      this.lblToBorrowerAddress2.AutoSize = true;
      this.lblToBorrowerAddress2.Location = new Point(8, 352);
      this.lblToBorrowerAddress2.Name = "lblToBorrowerAddress2";
      this.lblToBorrowerAddress2.Size = new Size(83, 14);
      this.lblToBorrowerAddress2.TabIndex = 20;
      this.lblToBorrowerAddress2.Text = "* City, State, Zip";
      this.lblToBorrowerAddress2.TextAlign = ContentAlignment.MiddleLeft;
      this.txtToBorrowerCity.Location = new Point((int) sbyte.MaxValue, 349);
      this.txtToBorrowerCity.MaxLength = 50;
      this.txtToBorrowerCity.Name = "txtToBorrowerCity";
      this.txtToBorrowerCity.Size = new Size(152, 20);
      this.txtToBorrowerCity.TabIndex = 21;
      this.lblToBorrowerAddress.AutoSize = true;
      this.lblToBorrowerAddress.Location = new Point(8, 331);
      this.lblToBorrowerAddress.Name = "lblToBorrowerAddress";
      this.lblToBorrowerAddress.Size = new Size(87, 14);
      this.lblToBorrowerAddress.TabIndex = 18;
      this.lblToBorrowerAddress.Text = "* Street Address";
      this.lblToBorrowerAddress.TextAlign = ContentAlignment.MiddleLeft;
      this.lblMailToBorrower.AutoSize = true;
      this.lblMailToBorrower.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMailToBorrower.Location = new Point(8, 270);
      this.lblMailToBorrower.Name = "lblMailToBorrower";
      this.lblMailToBorrower.Size = new Size(76, 14);
      this.lblMailToBorrower.TabIndex = 15;
      this.lblMailToBorrower.Text = "To Borrower";
      this.txtToBorrowerAddress.Location = new Point(128, 324);
      this.txtToBorrowerAddress.MaxLength = 50;
      this.txtToBorrowerAddress.Name = "txtToBorrowerAddress";
      this.txtToBorrowerAddress.Size = new Size(276, 20);
      this.txtToBorrowerAddress.TabIndex = 19;
      this.txtToBorrowerName.Location = new Point(128, 298);
      this.txtToBorrowerName.MaxLength = 50;
      this.txtToBorrowerName.Name = "txtToBorrowerName";
      this.txtToBorrowerName.Size = new Size(276, 20);
      this.txtToBorrowerName.TabIndex = 17;
      this.lblToBorrowerName.AutoSize = true;
      this.lblToBorrowerName.Location = new Point(11, 305);
      this.lblToBorrowerName.Name = "lblToBorrowerName";
      this.lblToBorrowerName.Size = new Size(41, 14);
      this.lblToBorrowerName.TabIndex = 16;
      this.lblToBorrowerName.Text = "* Name";
      this.lblToBorrowerName.TextAlign = ContentAlignment.MiddleLeft;
      this.gcSigning.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcSigning.Controls.Add((Control) this.cboAuthentication);
      this.gcSigning.Controls.Add((Control) this.lblAuthentication);
      this.gcSigning.Controls.Add((Control) this.cboSigningOption);
      this.gcSigning.Controls.Add((Control) this.lblSigningOption);
      this.gcSigning.HeaderForeColor = SystemColors.ControlText;
      this.gcSigning.Location = new Point(4, 30);
      this.gcSigning.Name = "gcSigning";
      this.gcSigning.Size = new Size(593, 92);
      this.gcSigning.TabIndex = 1;
      this.gcSigning.Text = "Borrower Signing Options";
      this.cboAuthentication.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAuthentication.FormattingEnabled = true;
      this.cboAuthentication.Location = new Point(236, 61);
      this.cboAuthentication.Name = "cboAuthentication";
      this.cboAuthentication.Size = new Size(302, 22);
      this.cboAuthentication.TabIndex = 1;
      this.cboAuthentication.SelectedIndexChanged += new EventHandler(this.cboSigningOption_SelectedIndexChanged);
      this.lblAuthentication.AutoSize = true;
      this.lblAuthentication.Location = new Point(8, 64);
      this.lblAuthentication.Name = "lblAuthentication";
      this.lblAuthentication.Size = new Size(170, 14);
      this.lblAuthentication.TabIndex = 0;
      this.lblAuthentication.Text = "* Borrower Authentication Method";
      this.cboSigningOption.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSigningOption.FormattingEnabled = true;
      this.cboSigningOption.Location = new Point(236, 33);
      this.cboSigningOption.Name = "cboSigningOption";
      this.cboSigningOption.Size = new Size(302, 22);
      this.cboSigningOption.TabIndex = 1;
      this.cboSigningOption.SelectedIndexChanged += new EventHandler(this.cboSigningOption_SelectedIndexChanged);
      this.lblSigningOption.AutoSize = true;
      this.lblSigningOption.Location = new Point(8, 36);
      this.lblSigningOption.Name = "lblSigningOption";
      this.lblSigningOption.Size = new Size(133, 14);
      this.lblSigningOption.TabIndex = 0;
      this.lblSigningOption.Text = "* Borrower Signing Option";
      this.gcMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcMessage.Controls.Add((Control) this.emHelpLink);
      this.gcMessage.Controls.Add((Control) this.panel2);
      this.gcMessage.Controls.Add((Control) this.pnlRecipients);
      this.gcMessage.Controls.Add((Control) this.gcSigning);
      this.gcMessage.Controls.Add((Control) this.btnNotifyUsers);
      this.gcMessage.Controls.Add((Control) this.lblNotifyUserCount);
      this.gcMessage.Controls.Add((Control) this.cboSubject);
      this.gcMessage.Controls.Add((Control) this.htmlEmailControl);
      this.gcMessage.Controls.Add((Control) this.dateAcceptBy);
      this.gcMessage.Controls.Add((Control) this.chkAcceptBy);
      this.gcMessage.Controls.Add((Control) this.chkReadReceipt);
      this.gcMessage.Controls.Add((Control) this.lblSubject);
      this.gcMessage.HeaderForeColor = SystemColors.ControlText;
      this.gcMessage.Location = new Point(5, 5);
      this.gcMessage.Name = "gcMessage";
      this.gcMessage.Size = new Size(609, 627);
      this.gcMessage.TabIndex = 0;
      this.gcMessage.Text = "Message";
      this.emHelpLink.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emHelpLink.BackColor = Color.Transparent;
      this.emHelpLink.Cursor = Cursors.Hand;
      this.emHelpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink.HelpTag = "Disclosure Fulfillment";
      this.emHelpLink.Location = new Point(1000, -161);
      this.emHelpLink.Name = "emHelpLink";
      this.emHelpLink.Size = new Size(19, 19);
      this.emHelpLink.TabIndex = 0;
      this.panel2.Controls.Add((Control) this.panel6);
      this.panel2.Controls.Add((Control) this.txtSenderEmail);
      this.panel2.Controls.Add((Control) this.txtSenderName);
      this.panel2.Controls.Add((Control) this.cmbSenderType);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Location = new Point(4, (int) sbyte.MaxValue);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(593, 115);
      this.panel2.TabIndex = 51;
      this.panel6.Controls.Add((Control) this.btnAddRecipient);
      this.panel6.Controls.Add((Control) this.lblAuthorizationCode);
      this.panel6.Controls.Add((Control) this.lblRecipientEmail);
      this.panel6.Controls.Add((Control) this.lblRecipientName);
      this.panel6.Controls.Add((Control) this.lblRecipientType);
      this.panel6.Location = new Point(4, 63);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(586, 49);
      this.panel6.TabIndex = 52;
      this.btnAddRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddRecipient.BackColor = Color.Transparent;
      this.btnAddRecipient.Location = new Point(552, 12);
      this.btnAddRecipient.Margin = new Padding(4, 3, 0, 3);
      this.btnAddRecipient.MouseDownImage = (Image) null;
      this.btnAddRecipient.Name = "btnAddRecipient";
      this.btnAddRecipient.Size = new Size(21, 16);
      this.btnAddRecipient.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddRecipient.TabIndex = 49;
      this.btnAddRecipient.TabStop = false;
      this.btnAddRecipient.Click += new EventHandler(this.btnAddRecipient_Click);
      this.lblAuthorizationCode.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAuthorizationCode.Location = new Point(436, 6);
      this.lblAuthorizationCode.Name = "lblAuthorizationCode";
      this.lblAuthorizationCode.Size = new Size(105, 39);
      this.lblAuthorizationCode.TabIndex = 48;
      this.lblAuthorizationCode.Text = "Authorization Code";
      this.lblAuthorizationCode.TextAlign = ContentAlignment.MiddleCenter;
      this.lblRecipientEmail.AutoSize = true;
      this.lblRecipientEmail.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientEmail.Location = new Point(322, 11);
      this.lblRecipientEmail.Name = "lblRecipientEmail";
      this.lblRecipientEmail.Size = new Size(37, 13);
      this.lblRecipientEmail.TabIndex = 44;
      this.lblRecipientEmail.Text = "Email";
      this.lblRecipientEmail.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRecipientName.AutoSize = true;
      this.lblRecipientName.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientName.Location = new Point(177, 11);
      this.lblRecipientName.Name = "lblRecipientName";
      this.lblRecipientName.Size = new Size(39, 13);
      this.lblRecipientName.TabIndex = 43;
      this.lblRecipientName.Text = "Name";
      this.lblRecipientName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRecipientType.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientType.Location = new Point(13, 7);
      this.lblRecipientType.Name = "lblRecipientType";
      this.lblRecipientType.Size = new Size(100, 35);
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
      this.pnlRecipients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlRecipients.AutoScroll = true;
      this.pnlRecipients.Location = new Point(5, 246);
      this.pnlRecipients.Name = "pnlRecipients";
      this.pnlRecipients.Size = new Size(592, 112);
      this.pnlRecipients.TabIndex = 52;
      this.panelDlg.AutoScroll = true;
      this.panelDlg.AutoSize = true;
      this.panelDlg.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.panelDlg.BackColor = Color.DarkRed;
      this.panelDlg.ForeColor = SystemColors.ControlLight;
      this.panelDlg.Location = new Point(3, 3);
      this.panelDlg.MinimumSize = new Size(1019, 678);
      this.panelDlg.Name = "panelDlg";
      this.panelDlg.Size = new Size(1019, 678);
      this.panelDlg.TabIndex = 1;
      this.btnNotifyUsers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNotifyUsers.Location = new Point(14, 587);
      this.btnNotifyUsers.Name = "btnNotifyUsers";
      this.btnNotifyUsers.Size = new Size(129, 23);
      this.btnNotifyUsers.TabIndex = 13;
      this.btnNotifyUsers.Text = "Notify Additional Users";
      this.btnNotifyUsers.UseVisualStyleBackColor = true;
      this.btnNotifyUsers.Click += new EventHandler(this.btnNotifyUsers_Click);
      this.lblNotifyUserCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblNotifyUserCount.AutoSize = true;
      this.lblNotifyUserCount.Location = new Point(13, 611);
      this.lblNotifyUserCount.Name = "lblNotifyUserCount";
      this.lblNotifyUserCount.Size = new Size(105, 14);
      this.lblNotifyUserCount.TabIndex = 10;
      this.lblNotifyUserCount.Text = "({0} Users selected)";
      this.cboSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSubject.FormattingEnabled = true;
      this.cboSubject.Location = new Point(88, 367);
      this.cboSubject.Name = "cboSubject";
      this.cboSubject.Size = new Size(509, 22);
      this.cboSubject.Sorted = true;
      this.cboSubject.TabIndex = 7;
      this.cboSubject.SelectedIndexChanged += new EventHandler(this.cboSubject_SelectedIndexChanged);
      this.htmlEmailControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(11, 397);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(586, 188);
      this.htmlEmailControl.TabIndex = 8;
      this.dateAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.dateAcceptBy.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateAcceptBy.CalendarTitleForeColor = Color.White;
      this.dateAcceptBy.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateAcceptBy.CustomFormat = "MM/dd/yyyy";
      this.dateAcceptBy.Enabled = false;
      this.dateAcceptBy.Format = DateTimePickerFormat.Custom;
      this.dateAcceptBy.Location = new Point(415, 603);
      this.dateAcceptBy.Name = "dateAcceptBy";
      this.dateAcceptBy.Size = new Size(100, 20);
      this.dateAcceptBy.TabIndex = 11;
      this.chkAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAcceptBy.Location = new Point(163, 607);
      this.chkAcceptBy.Name = "chkAcceptBy";
      this.chkAcceptBy.Size = new Size(251, 19);
      this.chkAcceptBy.TabIndex = 10;
      this.chkAcceptBy.Text = "Notify me when borrower does not access by";
      this.chkAcceptBy.UseVisualStyleBackColor = true;
      this.chkAcceptBy.CheckedChanged += new EventHandler(this.chkAcceptBy_CheckedChanged);
      this.chkReadReceipt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkReadReceipt.AutoSize = true;
      this.chkReadReceipt.Checked = true;
      this.chkReadReceipt.CheckState = CheckState.Checked;
      this.chkReadReceipt.Location = new Point(163, 587);
      this.chkReadReceipt.Name = "chkReadReceipt";
      this.chkReadReceipt.Size = new Size(261, 18);
      this.chkReadReceipt.TabIndex = 9;
      this.chkReadReceipt.Text = "Notify me when borrower receives the package.";
      this.chkReadReceipt.UseVisualStyleBackColor = true;
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(8, 371);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(50, 14);
      this.lblSubject.TabIndex = 6;
      this.lblSubject.Text = "* Subject";
      this.lblSubject.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRequired.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblRequired.AutoSize = true;
      this.lblRequired.ForeColor = Color.Black;
      this.lblRequired.Location = new Point(8, 635);
      this.lblRequired.Name = "lblRequired";
      this.lblRequired.Size = new Size(88, 14);
      this.lblRequired.TabIndex = 3;
      this.lblRequired.Text = "* Required Fields";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1046, 655);
      this.Controls.Add((Control) this.lblRequired);
      this.Controls.Add((Control) this.gcFulfillment);
      this.Controls.Add((Control) this.gcMessage);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSend);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SendDisclosuresDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Send eDisclosures";
      this.gcFulfillment.ResumeLayout(false);
      this.gcFulfillment.PerformLayout();
      this.pnlFulfillment.ResumeLayout(false);
      this.pnlFulfillment.PerformLayout();
      this.gcSigning.ResumeLayout(false);
      this.gcSigning.PerformLayout();
      this.gcMessage.ResumeLayout(false);
      this.gcMessage.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      ((ISupportInitialize) this.btnAddRecipient).EndInit();
      this.pnlRecipients.ResumeLayout(false);
      this.pnlRecipients.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
