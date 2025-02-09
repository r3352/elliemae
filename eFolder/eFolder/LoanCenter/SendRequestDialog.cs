// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.SendRequestDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
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
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class SendRequestDialog : Form
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
    private string packageID;
    private string comments;
    private bool eSign;
    private bool wetSign;
    private List<LoanCenterSigner> signers;
    private List<LoanCenterSigner> deletedSigners;
    private List<string> requiredSigners;
    private List<string> invalidList = new List<string>();
    private IContainer components;
    private Button btnCancel;
    private Button btnSend;
    private GroupContainer gcMessage;
    private CheckBox chkReadReceipt;
    private Label lblSubject;
    private CheckBox chkAcceptBy;
    private Label lblRequired;
    private DateTimePicker dateAcceptBy;
    private HtmlEmailControl htmlEmailControl;
    private ComboBox cboSubject;
    private Label lblNotifyUserCount;
    private Button btnNotifyUsers;
    private GroupContainer gcSigning;
    private Label lblAuthentication;
    private ComboBox cboAuthentication;
    private ComboBox cboSigningOption;
    private Label lblSigningOption;
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
    private FlowLayoutPanel pnlRecipients;

    public SendRequestDialog(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.loan = loanDataMgr.LoanData;
      this.coversheetFile = coversheetFile;
      this.signList = signList;
      this.neededList = neededList;
      this.pdfList = pdfList;
      Cursor.Current = Cursors.WaitCursor;
      this.initAttachmentList();
      this.initSigners();
      this.initNotifyCount();
      if (!this.noOriginatorCancel)
        this.initContents();
      Cursor.Current = Cursors.Default;
    }

    public string OriginatorSignerUrl { get; private set; }

    public bool NoOriginatorCancel => this.noOriginatorCancel;

    public string PackageID => this.packageID;

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
      this.initHtmlControl();
      this.initFrom();
      this.initSubject();
      this.dateAcceptBy.Value = DateTime.Today.AddDays(2.0);
      foreach (DocumentAttachment attachment in this.attachmentList)
      {
        switch (attachment.SignatureType)
        {
          case "eSignable":
            this.eSign = true;
            continue;
          case "Wet Sign Only":
            this.wetSign = true;
            continue;
          default:
            continue;
        }
      }
      if (Session.ConfigurationManager.GetEDisclosureSetup().AllowESigning(this.loan) && this.eSign)
      {
        if (this.wetSign)
          this.cboSigningOption.Items.Add((object) "eSign + Wet Sign (for wet sign only documents)");
        else
          this.cboSigningOption.Items.Add((object) "eSign (electronically sign and return)");
      }
      if (this.eSign || this.wetSign)
        this.cboSigningOption.Items.Add((object) "Wet Sign (print, sign, and fax)");
      else
        this.cboSigningOption.Items.Add((object) "No Signature Required");
      this.initSigningOptionsList(false);
      this.cboAuthentication.Items.Add((object) "Answer security questions");
      this.cboAuthentication.Items.Add((object) "Authorization Code");
      this.cboAuthentication.SelectedIndex = 1;
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
        EmailFromType emailFromType = EmailFromType.CurrentUser;
        switch (index)
        {
          case 1:
            EmailFrom.GetFromUser(this.loanDataMgr, EmailFromType.CurrentUser, ref userid, ref email, ref name);
            text = "Current User";
            emailFromType = EmailFromType.CurrentUser;
            break;
          case 2:
            EmailFrom.GetFromUser(this.loanDataMgr, EmailFromType.FileStarter, ref userid, ref email, ref name);
            text = "File Starter";
            emailFromType = EmailFromType.FileStarter;
            break;
          case 3:
            EmailFrom.GetFromUser(this.loanDataMgr, EmailFromType.LoanOfficer, ref userid, ref email, ref name);
            text = "Loan Officer";
            emailFromType = EmailFromType.LoanOfficer;
            break;
        }
        if (!string.IsNullOrEmpty(userid))
          this.cmbSenderType.Items.Add((object) new FieldOption(text, emailFromType.ToString()));
      }
      if (this.cmbSenderType.Items.Count <= 0)
        return;
      this.cmbSenderType.SelectedIndex = 0;
    }

    private void initSubject()
    {
      HtmlEmailTemplate[] htmlEmailTemplates = Session.ConfigurationManager.GetHtmlEmailTemplates((string) null, HtmlEmailTemplateType.RequestDocuments);
      if (htmlEmailTemplates != null)
        this.cboSubject.Items.AddRange((object[]) htmlEmailTemplates);
      string strB = Session.GetPrivateProfileString("EmailTemplates", "Request");
      if (string.IsNullOrEmpty(strB))
        strB = Session.ConfigurationManager.GetCompanySetting("DefaultEmailTemplates", "Request");
      foreach (HtmlEmailTemplate htmlEmailTemplate in this.cboSubject.Items)
      {
        if (string.Compare(htmlEmailTemplate.Guid, strB, true) == 0)
          this.cboSubject.SelectedItem = (object) htmlEmailTemplate;
      }
      if (this.cboSubject.SelectedItem != null || this.cboSubject.Items.Count <= 0)
        return;
      this.cboSubject.SelectedIndex = 0;
    }

    private void initSigningOptionsList(bool other)
    {
      this.cboSigningOption.Items.Clear();
      if (!other && Session.ConfigurationManager.GetEDisclosureSetup().AllowESigning(this.loan) && this.eSign)
      {
        if (this.wetSign)
          this.cboSigningOption.Items.Add((object) "eSign + Wet Sign (for wet sign only documents)");
        else
          this.cboSigningOption.Items.Add((object) "eSign (electronically sign and return)");
      }
      if (this.eSign || this.wetSign)
        this.cboSigningOption.Items.Add((object) "Wet Sign (print, sign, and fax)");
      else
        this.cboSigningOption.Items.Add((object) "No Signature Required");
      this.cboSigningOption.SelectedIndex = 0;
    }

    private LoanCenterMessage createMessage()
    {
      LoanCenterMessage message = new LoanCenterMessage(this.loanDataMgr, LoanCenterMessageType.RequestDocuments);
      message.ReplyTo = this.txtSenderEmail.Text.Trim();
      message.To = string.Empty;
      message.CC = string.Empty;
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
      message.AddCoversheet(this.coversheetFile);
      foreach (LoanCenterAttachment attachment in this.attachmentList)
        message.AddAttachment(attachment);
      if (this.neededList != null && this.neededList.Length != 0)
        message.AddNeeded(this.neededList);
      return message;
    }

    private bool validateMessage(LoanCenterMessage msg)
    {
      if (msg.ReplyTo == string.Empty)
        this.invalidList.Add("The 'From' email address must be filled in.");
      else if (!Utils.ValidateEmail(msg.ReplyTo))
        this.invalidList.Add("Invalid 'From' email address.");
      if (msg.NBOSendEmails != null)
      {
        foreach (EncompassContactEmail nboSendEmail in msg.NBOSendEmails)
        {
          if (string.IsNullOrEmpty(nboSendEmail.email))
            this.invalidList.Add("Non Borrowing Owner email must be filled in");
          else if (!Utils.ValidateEmail(nboSendEmail.email))
            this.invalidList.Add("Invalid 'Non Borrowing Owner' email address");
        }
      }
      if (msg.Subject == string.Empty)
        this.invalidList.Add("Subject is required.");
      if (msg.NotificationDate.Date != DateTime.MinValue.Date && msg.NotificationDate.Date < DateTime.Today)
        this.invalidList.Add("Borrower Accept By Date must be greater than or equal to today's date.");
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
      using (LoanCenterClient loanCenterClient = new LoanCenterClient())
      {
        this.packageID = loanCenterClient.SendMessage(msg);
        this.OriginatorSignerUrl = loanCenterClient.OriginatorSignerUrl;
        if (this.packageID == null)
          return false;
      }
      string str = msg.ReadReceipt ? "Yes" : "No";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("From: " + msg.ReplyTo);
      stringBuilder.AppendLine("To: " + msg.To);
      if (msg.CC != string.Empty)
        stringBuilder.AppendLine("Cc: " + msg.CC);
      stringBuilder.AppendLine("Subject: " + msg.Subject);
      stringBuilder.AppendLine("Read Receipt: " + str);
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(msg.Body);
      this.comments = stringBuilder.ToString();
      return true;
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      this.invalidList.Clear();
      if ((this.cboSigningOption.Text.Equals("eSign (electronically sign and return)") || this.cboSigningOption.Text.Equals("eSign + Wet Sign (for wet sign only documents)")) && (this.requiredSigners == null || this.requiredSigners.Count == 0))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The documents in this package have no borrower or co-borrower eSignature points. Add eSignature points to the documents or convert the package into a wet signing package. ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (!this.pnlRecipients.Controls.OfType<RecipientControl>().Any<RecipientControl>((Func<RecipientControl, bool>) (ctrl => ctrl.cbSelect.Checked)))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select recipient(s)", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.validateSenderContactDetails();
        this.validateSigners();
        LoanCenterMessage message = this.createMessage();
        if (!this.validateMessage(message))
          return;
        string text = this.checkAutoSigners(message);
        if (text == null)
          return;
        this.saveSenderContactDetails();
        this.saveSigners();
        if (!this.sendMessage(message))
          return;
        message.CreateLogEntry();
        HtmlEmailTemplate selectedItem = this.cboSubject.SelectedItem as HtmlEmailTemplate;
        if (selectedItem != (HtmlEmailTemplate) null)
          Session.WritePrivateProfileString("EmailTemplates", "Request", selectedItem.Guid);
        if (this.shouldLODocuSignEmbeddedSigning(message, this.OriginatorSignerUrl))
        {
          DocuSignSigningDialog form = new DocuSignSigningDialog(this.OriginatorSignerUrl);
          this.setWindowDisplay(form);
          int num3 = (int) form.ShowDialog((IWin32Window) Form.ActiveForm);
        }
        int num4 = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.DialogResult = DialogResult.OK;
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
          string str = string.Empty;
          foreach (LoanCenterAttachment attachment in msg.Attachments)
          {
            if (attachment is DocumentAttachment && signer.RequiresSigning((DocumentAttachment) attachment))
              str = str + attachment.Title + Environment.NewLine;
          }
          if (string.Compare(Session.UserID, signer.UserID, StringComparison.OrdinalIgnoreCase) == 0)
          {
            switch (Utils.Dialog((IWin32Window) this, "This package includes one or more documents that you must eSign before you can retrieve the borrower-signed documents. It is strongly recommended that you sign prior to the borrowers. Click \"Yes\" to eSign the documents now.  Click \"No\" to eSign later.  You will receive an email with a link to a secure website where you can access and eSign the documents in this package." + Environment.NewLine + Environment.NewLine + "Originator eSignable Documents:" + Environment.NewLine + Environment.NewLine + str, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
              case DialogResult.Cancel:
                return (string) null;
              case DialogResult.Yes:
                signer.AutoSign = true;
                continue;
              case DialogResult.No:
                return "The request package has been sent to the borrower.";
              default:
                continue;
            }
          }
          else
            return Utils.Dialog((IWin32Window) this, "This package includes one or more documents that require the Originator's eSignature. The originator, " + signer.UnparsedName + ", will be notified via email." + Environment.NewLine + Environment.NewLine + "Originator eSignable Documents:" + Environment.NewLine + Environment.NewLine + str, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK ? "An email message has been sent to the Originator." : (string) null;
        }
      }
      return "The request package has been sent to the borrower.";
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

    private void chkAcceptBy_CheckedChanged(object sender, EventArgs e)
    {
      this.dateAcceptBy.Enabled = this.chkAcceptBy.Checked;
    }

    private void cboSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
      HtmlEmailTemplate selectedItem1 = this.cboSubject.SelectedItem as HtmlEmailTemplate;
      if (selectedItem1 == (HtmlEmailTemplate) null)
        return;
      UserInfo userInfo = (UserInfo) null;
      if (this.cmbSenderType.SelectedItem is FieldOption selectedItem2)
        userInfo = selectedItem2.Text == "Current User" || selectedItem2.Value == "Current User" ? Session.UserInfo : Session.OrganizationManager.GetUser(selectedItem2.ReportingDatabaseValue);
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
      string html = new HtmlFieldMerge(selectedItem1.Html)
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
      EmailFromType userType = (EmailFromType) Enum.Parse(typeof (EmailFromType), (this.cmbSenderType.SelectedItem as FieldOption).Value);
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
        loanCenterSigner.RecipientType = eDeliveryEntityType.NonBorrowingOwner.ToString("g");
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
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      eDeliveryMessage eDeliveryMessage = new eDeliveryMessage(this.loanDataMgr, eDeliveryMessageType.RequestDocuments);
      foreach (DocumentAttachment attachment in this.attachmentList)
      {
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
        if (eDeliveryMessage.checkSigningpointExists(attachment, LoanCenterEntityType.NonBorrowingOwner.ToString("g")) && !flag3)
        {
          flag3 = true;
          requiredSigners.Add(LoanCenterEntityType.NonBorrowingOwner.ToString("g"));
        }
        if (flag1 & flag2 & flag3)
          break;
      }
      foreach (DocumentAttachment documentAttachment in this.attachmentList.FindAll((Predicate<DocumentAttachment>) (x => x.SignatureFields != null)))
      {
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(LoanCenterEntityType.Borrower.ToString("g")))) != null)
          requiredSigners.Add(LoanCenterEntityType.Borrower.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(LoanCenterEntityType.Coborrower.ToString("g")))) != null)
          requiredSigners.Add(LoanCenterEntityType.Coborrower.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(LoanCenterEntityType.NonBorrowingOwner.ToString("g")))) != null)
          requiredSigners.Add(LoanCenterEntityType.NonBorrowingOwner.ToString("g"));
        if (((IEnumerable<string>) documentAttachment.SignatureFields).ToList<string>().Find((Predicate<string>) (x => x.StartsWith(LoanCenterEntityType.Originator.ToString("g")))) != null)
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
          signer.RecipientType = eDeliveryEntityType.NonBorrowingOwner.ToString("g");
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
      this.gcMessage = new GroupContainer();
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
      this.dateAcceptBy = new DateTimePicker();
      this.lblNotifyUserCount = new Label();
      this.btnNotifyUsers = new Button();
      this.cboSubject = new ComboBox();
      this.htmlEmailControl = new HtmlEmailControl();
      this.chkAcceptBy = new CheckBox();
      this.chkReadReceipt = new CheckBox();
      this.lblSubject = new Label();
      this.lblRequired = new Label();
      this.gcSigning = new GroupContainer();
      this.lblAuthentication = new Label();
      this.cboAuthentication = new ComboBox();
      this.cboSigningOption = new ComboBox();
      this.lblSigningOption = new Label();
      this.gcMessage.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel6.SuspendLayout();
      ((ISupportInitialize) this.btnAddRecipient).BeginInit();
      this.gcSigning.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(532, 630);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.btnSend.Location = new Point(449, 630);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 22);
      this.btnSend.TabIndex = 3;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.gcMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.gcMessage.Controls.Add((Control) this.panel2);
      this.gcMessage.Controls.Add((Control) this.pnlRecipients);
      this.gcMessage.Controls.Add((Control) this.dateAcceptBy);
      this.gcMessage.Controls.Add((Control) this.lblNotifyUserCount);
      this.gcMessage.Controls.Add((Control) this.btnNotifyUsers);
      this.gcMessage.Controls.Add((Control) this.cboSubject);
      this.gcMessage.Controls.Add((Control) this.htmlEmailControl);
      this.gcMessage.Controls.Add((Control) this.chkAcceptBy);
      this.gcMessage.Controls.Add((Control) this.chkReadReceipt);
      this.gcMessage.Controls.Add((Control) this.lblSubject);
      this.gcMessage.HeaderForeColor = SystemColors.ControlText;
      this.gcMessage.Location = new Point(8, 93);
      this.gcMessage.Name = "gcMessage";
      this.gcMessage.Size = new Size(601, 532);
      this.gcMessage.TabIndex = 0;
      this.gcMessage.Text = "Message";
      this.panel2.Controls.Add((Control) this.panel6);
      this.panel2.Controls.Add((Control) this.txtSenderEmail);
      this.panel2.Controls.Add((Control) this.txtSenderName);
      this.panel2.Controls.Add((Control) this.cmbSenderType);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Location = new Point(11, 29);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(568, 103);
      this.panel2.TabIndex = 49;
      this.panel6.Controls.Add((Control) this.btnAddRecipient);
      this.panel6.Controls.Add((Control) this.lblAuthorizationCode);
      this.panel6.Controls.Add((Control) this.lblRecipientEmail);
      this.panel6.Controls.Add((Control) this.lblRecipientName);
      this.panel6.Controls.Add((Control) this.lblRecipientType);
      this.panel6.Location = new Point(4, 64);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(560, 36);
      this.panel6.TabIndex = 52;
      this.btnAddRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddRecipient.BackColor = Color.Transparent;
      this.btnAddRecipient.Location = new Point(535, 12);
      this.btnAddRecipient.Margin = new Padding(4, 3, 0, 3);
      this.btnAddRecipient.MouseDownImage = (Image) null;
      this.btnAddRecipient.Name = "btnAddRecipient";
      this.btnAddRecipient.Size = new Size(21, 16);
      this.btnAddRecipient.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddRecipient.TabIndex = 49;
      this.btnAddRecipient.TabStop = false;
      this.btnAddRecipient.Click += new EventHandler(this.btnAddRecipient_Click);
      this.lblAuthorizationCode.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAuthorizationCode.Location = new Point(420, 11);
      this.lblAuthorizationCode.Name = "lblAuthorizationCode";
      this.lblAuthorizationCode.Size = new Size(115, 13);
      this.lblAuthorizationCode.TabIndex = 48;
      this.lblAuthorizationCode.Text = "Authorization Code";
      this.lblAuthorizationCode.TextAlign = ContentAlignment.MiddleCenter;
      this.lblRecipientEmail.AutoSize = true;
      this.lblRecipientEmail.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientEmail.Location = new Point(298, 11);
      this.lblRecipientEmail.Name = "lblRecipientEmail";
      this.lblRecipientEmail.Size = new Size(37, 13);
      this.lblRecipientEmail.TabIndex = 44;
      this.lblRecipientEmail.Text = "Email";
      this.lblRecipientEmail.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRecipientName.AutoSize = true;
      this.lblRecipientName.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientName.Location = new Point(168, 11);
      this.lblRecipientName.Name = "lblRecipientName";
      this.lblRecipientName.Size = new Size(39, 13);
      this.lblRecipientName.TabIndex = 43;
      this.lblRecipientName.Text = "Name";
      this.lblRecipientName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRecipientType.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRecipientType.Location = new Point(13, 0);
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
      this.pnlRecipients.Location = new Point(12, 133);
      this.pnlRecipients.Name = "pnlRecipients";
      this.pnlRecipients.Size = new Size(567, 118);
      this.pnlRecipients.TabIndex = 50;
      this.dateAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.dateAcceptBy.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateAcceptBy.CalendarTitleForeColor = Color.White;
      this.dateAcceptBy.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateAcceptBy.CustomFormat = "MM/dd/yyyy";
      this.dateAcceptBy.Enabled = false;
      this.dateAcceptBy.Format = DateTimePickerFormat.Custom;
      this.dateAcceptBy.Location = new Point(488, 507);
      this.dateAcceptBy.Name = "dateAcceptBy";
      this.dateAcceptBy.Size = new Size(100, 20);
      this.dateAcceptBy.TabIndex = 13;
      this.lblNotifyUserCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblNotifyUserCount.AutoSize = true;
      this.lblNotifyUserCount.Location = new Point(14, 514);
      this.lblNotifyUserCount.Name = "lblNotifyUserCount";
      this.lblNotifyUserCount.Size = new Size(105, 14);
      this.lblNotifyUserCount.TabIndex = 8;
      this.lblNotifyUserCount.Text = "({0} Users selected)";
      this.btnNotifyUsers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNotifyUsers.Location = new Point(12, 488);
      this.btnNotifyUsers.Name = "btnNotifyUsers";
      this.btnNotifyUsers.Size = new Size(129, 22);
      this.btnNotifyUsers.TabIndex = 5;
      this.btnNotifyUsers.Text = "Notify Additional Users";
      this.btnNotifyUsers.Click += new EventHandler(this.btnNotifyUsers_Click);
      this.cboSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSubject.FormattingEnabled = true;
      this.cboSubject.Location = new Point(88, 265);
      this.cboSubject.Name = "cboSubject";
      this.cboSubject.Size = new Size(500, 22);
      this.cboSubject.Sorted = true;
      this.cboSubject.TabIndex = 9;
      this.cboSubject.SelectedIndexChanged += new EventHandler(this.cboSubject_SelectedIndexChanged);
      this.htmlEmailControl.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(11, 295);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(577, 187);
      this.htmlEmailControl.TabIndex = 10;
      this.chkAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAcceptBy.Location = new Point(236, 509);
      this.chkAcceptBy.Name = "chkAcceptBy";
      this.chkAcceptBy.Size = new Size(252, 19);
      this.chkAcceptBy.TabIndex = 12;
      this.chkAcceptBy.Text = "Notify me when borrower does not access by";
      this.chkAcceptBy.UseVisualStyleBackColor = true;
      this.chkAcceptBy.CheckedChanged += new EventHandler(this.chkAcceptBy_CheckedChanged);
      this.chkReadReceipt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkReadReceipt.AutoSize = true;
      this.chkReadReceipt.Checked = true;
      this.chkReadReceipt.CheckState = CheckState.Checked;
      this.chkReadReceipt.Location = new Point(236, 489);
      this.chkReadReceipt.Name = "chkReadReceipt";
      this.chkReadReceipt.Size = new Size(261, 18);
      this.chkReadReceipt.TabIndex = 11;
      this.chkReadReceipt.Text = "Notify me when borrower receives the package.";
      this.chkReadReceipt.UseVisualStyleBackColor = true;
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(8, 268);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(50, 14);
      this.lblSubject.TabIndex = 8;
      this.lblSubject.Text = "* Subject";
      this.lblSubject.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRequired.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.lblRequired.AutoSize = true;
      this.lblRequired.ForeColor = Color.Black;
      this.lblRequired.Location = new Point(5, 630);
      this.lblRequired.Name = "lblRequired";
      this.lblRequired.Size = new Size(88, 14);
      this.lblRequired.TabIndex = 2;
      this.lblRequired.Text = "* Required Fields";
      this.gcSigning.Anchor = AnchorStyles.Top | AnchorStyles.Left;
      this.gcSigning.Controls.Add((Control) this.lblAuthentication);
      this.gcSigning.Controls.Add((Control) this.cboAuthentication);
      this.gcSigning.Controls.Add((Control) this.cboSigningOption);
      this.gcSigning.Controls.Add((Control) this.lblSigningOption);
      this.gcSigning.HeaderForeColor = SystemColors.ControlText;
      this.gcSigning.Location = new Point(8, 6);
      this.gcSigning.Name = "gcSigning";
      this.gcSigning.Size = new Size(601, 83);
      this.gcSigning.TabIndex = 5;
      this.gcSigning.Text = "Borrower Signing Options";
      this.lblAuthentication.AutoSize = true;
      this.lblAuthentication.Location = new Point(8, 58);
      this.lblAuthentication.Name = "lblAuthentication";
      this.lblAuthentication.Size = new Size(170, 14);
      this.lblAuthentication.TabIndex = 2;
      this.lblAuthentication.Text = "* Borrower Authentication Method";
      this.cboAuthentication.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAuthentication.FormattingEnabled = true;
      this.cboAuthentication.Location = new Point(194, 55);
      this.cboAuthentication.Name = "cboAuthentication";
      this.cboAuthentication.Size = new Size(267, 22);
      this.cboAuthentication.TabIndex = 3;
      this.cboAuthentication.SelectedIndexChanged += new EventHandler(this.cboAuthentication_SelectedIndexChanged);
      this.cboSigningOption.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSigningOption.FormattingEnabled = true;
      this.cboSigningOption.Location = new Point(194, 29);
      this.cboSigningOption.Name = "cboSigningOption";
      this.cboSigningOption.Size = new Size(267, 22);
      this.cboSigningOption.TabIndex = 1;
      this.cboSigningOption.SelectedIndexChanged += new EventHandler(this.cboSigningOption_SelectedIndexChanged);
      this.lblSigningOption.AutoSize = true;
      this.lblSigningOption.Location = new Point(8, 32);
      this.lblSigningOption.Name = "lblSigningOption";
      this.lblSigningOption.Size = new Size(133, 14);
      this.lblSigningOption.TabIndex = 0;
      this.lblSigningOption.Text = "* Borrower Signing Option";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(619, 655);
      this.Controls.Add((Control) this.gcSigning);
      this.Controls.Add((Control) this.lblRequired);
      this.Controls.Add((Control) this.gcMessage);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSend);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SendRequestDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Send Request";
      this.gcMessage.ResumeLayout(false);
      this.gcMessage.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      ((ISupportInitialize) this.btnAddRecipient).EndInit();
      this.gcSigning.ResumeLayout(false);
      this.gcSigning.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
