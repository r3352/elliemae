// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.SendConsentRequestDialogCC
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.eFolder.eDelivery.ePass;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class SendConsentRequestDialogCC : Form
  {
    private const string className = "SendConsentRequestDialogCC";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private LoanData loan;
    private string packageID;
    private string comments;
    private List<ContactDetails> _contacts;
    private List<Contact> contacts;
    private IContainer components;
    private Button btnCancel;
    private Button btnSend;
    private GroupContainer gcMessage;
    private CheckBox chkReadReceipt;
    private Label lblSenderType;
    private Label lblSubject;
    private CheckBox chkAcceptBy;
    private DateTimePicker dateAcceptBy;
    private HtmlEmailControl htmlEmailControl;
    private ComboBox cboSubject;
    private ComboBox cboSenderType;
    private Label lblNotifyUserCount;
    private Button btnNotifyUsers;
    private Label label1;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private FlowLayoutPanel pnlBorrowers;
    private EMHelpLink helpLink;
    private TextBox txtSenderEmail;
    private TextBox txtSenderName;
    private Label label6;
    private Label label7;
    private BorrowerPairConsentControlCC borrowerPairConsentControlCC1;
    private Label label8;
    private Label label10;
    private Label label9;
    private Label label11;
    private Label label12;
    private ContactConsentControlCC contactConsentControlCC1;

    public SendConsentRequestDialogCC(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      Tracing.Log(SendConsentRequestDialogCC.sw, TraceLevel.Verbose, nameof (SendConsentRequestDialogCC), "Initializing form.");
      this.loanDataMgr = loanDataMgr;
      this.loan = loanDataMgr.LoanData;
      Cursor.Current = Cursors.WaitCursor;
      this.initBorrowers();
      this.initNotifyCount();
      this.initContents();
      Cursor.Current = Cursors.Default;
      Tracing.Log(SendConsentRequestDialogCC.sw, TraceLevel.Verbose, nameof (SendConsentRequestDialogCC), "Form initialized.");
    }

    public string PackageID => this.packageID;

    public string Comments => this.comments;

    private void initBorrowers()
    {
      Tracing.Log(SendConsentRequestDialogCC.sw, TraceLevel.Verbose, nameof (SendConsentRequestDialogCC), "Initializing borrowers...");
      Task<List<ContactDetails>> loanContacts = new EBSServiceClient().GetLoanContacts(this.loan.GetField("GUID"));
      Task.WaitAll((Task) loanContacts);
      this._contacts = loanContacts.Result == null ? new List<ContactDetails>() : loanContacts.Result;
      while (this.pnlBorrowers.Controls.Count > 0)
      {
        Control control = this.pnlBorrowers.Controls[0];
        this.pnlBorrowers.Controls.Remove(control);
        control.Dispose();
      }
      EDeliveryConsentTrackingResponse result = new EDeliveryRestClient(this.loanDataMgr).GetLoanLevelConsentTracking().Result;
      int pairIndex1 = this.loan.GetPairIndex(this.loan.PairId);
      int pairIndex2 = -1;
      BorrowerPair[] borrowerPairs = this.loanDataMgr.LoanData.GetBorrowerPairs();
      int num = 0;
      try
      {
        foreach (BorrowerPair borrowerPair in borrowerPairs)
        {
          pairIndex2 = this.loan.GetPairIndex(borrowerPair.Id);
          this.loan.SetBorrowerPair(pairIndex2);
          string str1 = string.Join(" ", ((IEnumerable<string>) new string[4]
          {
            this.loan.GetSimpleField("4000"),
            this.loan.GetSimpleField("4001"),
            this.loan.GetSimpleField("4002"),
            this.loan.GetSimpleField("4003")
          }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
          string str2 = string.Join(" ", ((IEnumerable<string>) new string[4]
          {
            this.loan.GetSimpleField("4004"),
            this.loan.GetSimpleField("4005"),
            this.loan.GetSimpleField("4006"),
            this.loan.GetSimpleField("4007")
          }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
          string simpleField1 = this.loan.GetSimpleField("1268");
          bool flag1 = !string.IsNullOrEmpty(str2) || !string.IsNullOrEmpty(simpleField1);
          BorrowerPairConsentControlCC consentControlCc1 = new BorrowerPairConsentControlCC();
          string str3 = string.Empty;
          string str4 = string.Empty;
          if (this._contacts != null)
          {
            ContactDetails contactDetails1 = this._contacts.Find((Predicate<ContactDetails>) (x => x.contactType.ToUpper() == "Borrower".ToUpper() && x.borrowerId == this.loan.CurrentBorrowerPair.Borrower.Id));
            if (contactDetails1 != null)
              str3 = contactDetails1.authCode;
            if (flag1)
            {
              ContactDetails contactDetails2 = this._contacts.Find((Predicate<ContactDetails>) (x => x.contactType.ToUpper() == "Borrower".ToUpper() && x.borrowerId == this.loan.CurrentBorrowerPair.CoBorrower.Id));
              if (contactDetails2 != null)
                str4 = contactDetails2.authCode;
            }
          }
          consentControlCc1.BorrowerEnabled = true;
          consentControlCc1.BorrowerPairId = borrowerPair.Id;
          consentControlCc1.BorrowerName = str1;
          consentControlCc1.BorrowerEmail = this.loan.GetSimpleField("1240");
          consentControlCc1.BorrowerChecked = true;
          consentControlCc1.BorrowerAuthenticationCode = str3;
          if (num == 0)
            consentControlCc1.Margin = new Padding(0);
          else
            consentControlCc1.Margin = new Padding(0, 5, 0, 0);
          if (flag1)
          {
            consentControlCc1.CoBorrowerEnabled = true;
            consentControlCc1.CoBorrowerName = str2;
            consentControlCc1.CoBorrowerEmail = simpleField1;
            consentControlCc1.CoBorrowerChecked = true;
            consentControlCc1.CoBorrowerAuthenticationCode = str4;
          }
          string simpleField2 = this.loan.GetSimpleField("4108");
          string simpleField3 = this.loan.GetSimpleField("4109");
          if (!string.IsNullOrEmpty(simpleField2))
            consentControlCc1.BorrowerConsentSent = simpleField2;
          if (!string.IsNullOrEmpty(simpleField3))
            consentControlCc1.CoBorrowerConsentSent = simpleField3;
          if (result != null)
          {
            foreach (EDeliveryConsentDetail consentTrackingDetail in result.consentTrackingDetails)
            {
              EDeliveryConsentOutput edeliveryConsentOutput = consentTrackingDetail.consentOutput.OrderByDescending<EDeliveryConsentOutput, DateTime>((Func<EDeliveryConsentOutput, DateTime>) (k => k.date)).FirstOrDefault<EDeliveryConsentOutput>();
              bool flag2 = !string.IsNullOrEmpty(edeliveryConsentOutput.fullName) && !string.IsNullOrEmpty(edeliveryConsentOutput.email) && edeliveryConsentOutput.fullName.Trim().ToLower() == str1.Trim().ToLower() && edeliveryConsentOutput.email.Trim().ToLower() == consentControlCc1.BorrowerEmail.Trim().ToLower();
              bool flag3 = false;
              if (flag1)
                flag3 = !string.IsNullOrEmpty(consentControlCc1.CoBorrowerEmail) ? !string.IsNullOrEmpty(edeliveryConsentOutput.fullName) && !string.IsNullOrEmpty(edeliveryConsentOutput.email) && edeliveryConsentOutput.fullName.Trim().ToLower() == str2.Trim().ToLower() && edeliveryConsentOutput.email.Trim().ToLower() == consentControlCc1.CoBorrowerEmail.Trim().ToLower() : !string.IsNullOrEmpty(edeliveryConsentOutput.fullName) && !string.IsNullOrEmpty(edeliveryConsentOutput.email) && edeliveryConsentOutput.fullName.Trim().ToLower() == str2.Trim().ToLower() && edeliveryConsentOutput.email.Trim().ToLower() == consentControlCc1.BorrowerEmail.Trim().ToLower();
              if (flag2 | flag3)
              {
                if (flag2)
                  consentControlCc1.BorrowerConsentSent = simpleField2;
                if (flag3)
                  consentControlCc1.CoBorrowerConsentSent = simpleField3;
                if (edeliveryConsentOutput.status != null)
                {
                  if (edeliveryConsentOutput.status.ToLower() == "accepted")
                  {
                    if (flag2)
                    {
                      consentControlCc1.BorrowerAccepted = Utils.convertUTC_To_PST(edeliveryConsentOutput.date).ToString("MM/dd/yyyy");
                      consentControlCc1.BorrowerEnabled = false;
                    }
                    if (flag3)
                    {
                      consentControlCc1.CoBorrowerAccepted = Utils.convertUTC_To_PST(edeliveryConsentOutput.date).ToString("MM/dd/yyyy");
                      consentControlCc1.CoBorrowerEnabled = false;
                    }
                  }
                  else if (edeliveryConsentOutput.status.ToLower() == "declined")
                  {
                    if (flag2)
                      consentControlCc1.BorrowerRejected = Utils.convertUTC_To_PST(edeliveryConsentOutput.date).ToString("MM/dd/yyyy");
                    if (flag3)
                      consentControlCc1.CoBorrowerRejected = Utils.convertUTC_To_PST(edeliveryConsentOutput.date).ToString("MM/dd/yyyy");
                  }
                }
              }
            }
          }
          consentControlCc1.BorrowerSelectedChanged += new EventHandler(this.refreshSendButton);
          this.pnlBorrowers.Controls.Add((Control) consentControlCc1);
          ++num;
          List<NonBorrowingOwner> byBorrowerPairId = this.loanDataMgr.LoanData.GetNboByBorrowerPairId(borrowerPair.Id);
          consentControlCc1.NboContacts = byBorrowerPairId;
          foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
          {
            NonBorrowingOwner nbo = nonBorrowingOwner;
            string str5 = string.Join(" ", ((IEnumerable<string>) new string[4]
            {
              nbo.FirstName,
              nbo.MiddleName,
              nbo.LastName,
              nbo.SuffixName
            }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
            if (!string.IsNullOrWhiteSpace(str5) || !string.IsNullOrWhiteSpace(nbo.EmailAddress))
            {
              string str6 = string.Empty;
              if (this._contacts != null)
              {
                ContactDetails contactDetails = this._contacts.Find((Predicate<ContactDetails>) (x => x.contactType.ToUpper() == "NonBorrowingOwner".ToUpper() && x.borrowerId == nbo.NBOID));
                if (contactDetails != null)
                  str6 = contactDetails.authCode;
              }
              ContactConsentControlCC consentControlCc2 = new ContactConsentControlCC();
              consentControlCc2.ContactType = "Non-Borrowing Owner";
              consentControlCc2.ContactEnabled = true;
              consentControlCc2.BorrowerPairId = nbo.VestingBorrowerPairId;
              consentControlCc2.ContactName = str5;
              consentControlCc2.ContactEmail = nbo.EmailAddress;
              consentControlCc2.ContactChecked = true;
              consentControlCc2.ContactAuthenticationCode = str6;
              consentControlCc2.Margin = new Padding(0);
              consentControlCc2.DataTag = (object) nbo;
              string str7 = this.loan.GetField("NBOC" + (nbo.NboIndex <= 99 ? nbo.NboIndex.ToString("00") : nbo.NboIndex.ToString("000")) + "21").Replace("//", string.Empty);
              if (!string.IsNullOrEmpty(str7))
                consentControlCc2.ContactConsentSent = str7;
              if (result != null)
              {
                foreach (EDeliveryConsentDetail consentTrackingDetail in result.consentTrackingDetails)
                {
                  EDeliveryConsentOutput edeliveryConsentOutput = consentTrackingDetail.consentOutput.OrderByDescending<EDeliveryConsentOutput, DateTime>((Func<EDeliveryConsentOutput, DateTime>) (k => k.date)).FirstOrDefault<EDeliveryConsentOutput>();
                  bool flag4 = !string.IsNullOrEmpty(edeliveryConsentOutput.fullName) && !string.IsNullOrEmpty(edeliveryConsentOutput.email) && edeliveryConsentOutput.fullName.Trim().ToLower() == str5.Trim().ToLower() && edeliveryConsentOutput.email.Trim().ToLower() == nbo.EmailAddress.Trim().ToLower();
                  if (flag4)
                  {
                    consentControlCc2.ContactConsentSent = str7;
                    if (edeliveryConsentOutput.status != null)
                    {
                      if (edeliveryConsentOutput.status.ToLower() == "accepted")
                      {
                        if (flag4)
                        {
                          consentControlCc2.ContactAccepted = Utils.convertUTC_To_PST(edeliveryConsentOutput.date).ToString("MM/dd/yyyy");
                          consentControlCc2.ContactEnabled = false;
                        }
                      }
                      else if (edeliveryConsentOutput.status.ToLower() == "declined" && flag4)
                        consentControlCc2.ContactRejected = Utils.convertUTC_To_PST(edeliveryConsentOutput.date).ToString("MM/dd/yyyy");
                    }
                  }
                }
              }
              consentControlCc2.BorrowerSelectedChanged += new EventHandler(this.refreshSendButton);
              this.pnlBorrowers.Controls.Add((Control) consentControlCc2);
              ++num;
            }
          }
        }
      }
      finally
      {
        if (pairIndex1 != pairIndex2)
          this.loan.SetBorrowerPair(pairIndex1);
      }
      if (num > 1)
      {
        int height = this.pnlBorrowers.Height;
        this.pnlBorrowers.Height *= 2;
        this.lblSubject.Top += height;
        this.cboSubject.Top += height;
        this.htmlEmailControl.Top += height;
        this.htmlEmailControl.Height -= height;
      }
      this.btnSend.Enabled = false;
      foreach (Control control in (ArrangedElementCollection) this.pnlBorrowers.Controls)
      {
        if (control.GetType() == typeof (BorrowerPairConsentControlCC))
        {
          if (((BorrowerPairConsentControlCC) control).BorrowerEnabled || ((BorrowerPairConsentControlCC) control).CoBorrowerEnabled)
          {
            this.btnSend.Enabled = true;
            break;
          }
        }
        else if (control.GetType() == typeof (ContactConsentControlCC) && ((ContactConsentControlCC) control).ContactEnabled)
        {
          this.btnSend.Enabled = true;
          break;
        }
      }
      Tracing.Log(SendConsentRequestDialogCC.sw, TraceLevel.Verbose, nameof (SendConsentRequestDialogCC), "InitBorrowers complete.");
    }

    private void initNotifyCount()
    {
      this.lblNotifyUserCount.Text = string.Format("({0} Users selected)", (object) new EmailNotificationClient().ActiveEmailCount(new List<Guid>()
      {
        new Guid(this.loan.GUID)
      }.ToArray()));
    }

    private void initContents()
    {
      this.initHtmlControl();
      this.initFrom();
      this.initSubject();
      this.dateAcceptBy.Value = DateTime.Today.AddDays(2.0);
    }

    private void initHtmlControl()
    {
      this.htmlEmailControl.LoadText(string.Empty, false);
      this.htmlEmailControl.ShowFieldButton = false;
      this.htmlEmailControl.AllowPersonalImages = true;
    }

    private void initFrom()
    {
      this.addFromUser(EmailFromType.CurrentUser, "Current User");
      this.addFromUser(EmailFromType.FileStarter, "File Starter");
      this.addFromUser(EmailFromType.LoanOfficer, "Loan Officer");
      if (this.cboSenderType.Items.Count > 0)
        this.cboSenderType.SelectedIndex = 0;
      this.setSenderInfo();
    }

    private void addFromUser(EmailFromType fromType, string userType)
    {
      if (fromType == EmailFromType.FileStarter)
      {
        this.cboSenderType.Items.Add((object) new FieldOption(userType, userType));
      }
      else
      {
        string userid = (string) null;
        string email = (string) null;
        string name = (string) null;
        EmailFrom.GetFromUser(this.loanDataMgr, fromType, ref userid, ref email, ref name);
        if (string.IsNullOrEmpty(userid))
          return;
        this.cboSenderType.Items.Add((object) new FieldOption(userType, userType));
      }
    }

    private void initSubject()
    {
      HtmlEmailTemplate[] htmlEmailTemplates = Session.ConfigurationManager.GetHtmlEmailTemplates((string) null, HtmlEmailTemplateType.ConsumerConnectLoanLevelConsent);
      if (htmlEmailTemplates != null)
        this.cboSubject.Items.AddRange((object[]) htmlEmailTemplates);
      string strB = Session.GetPrivateProfileString("CCEmailTemplates", EmailManager.EmailType.Consent.ToString());
      if (string.IsNullOrEmpty(strB))
        strB = Session.ConfigurationManager.GetCompanySetting("DefaultCCEmailTemplates", EmailManager.EmailType.Consent.ToString());
      foreach (HtmlEmailTemplate htmlEmailTemplate in this.cboSubject.Items)
      {
        if (string.Compare(htmlEmailTemplate.Guid, strB, true) == 0)
          this.cboSubject.SelectedItem = (object) htmlEmailTemplate;
      }
      if (this.cboSubject.SelectedItem != null || this.cboSubject.Items.Count <= 0)
        return;
      this.cboSubject.SelectedIndex = 0;
    }

    private void refreshSendButton(object sender, EventArgs e)
    {
      this.btnSend.Enabled = false;
      foreach (Control control in (ArrangedElementCollection) this.pnlBorrowers.Controls)
      {
        if (control is BorrowerPairConsentControlCC consentControlCc2)
        {
          if (consentControlCc2.BorrowerChecked || consentControlCc2.CoBorrowerChecked)
          {
            this.btnSend.Enabled = true;
            break;
          }
        }
        else if (control is ContactConsentControlCC consentControlCc1 && consentControlCc1.ContactChecked)
        {
          this.btnSend.Enabled = true;
          break;
        }
      }
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      string empty = string.Empty;
      string text;
      if (!string.IsNullOrEmpty(text = this.validateSenderContactDetails()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.validateRecipients())
          return;
        this.saveSenderContactDetails();
        int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
        this.saveRecipients();
        HtmlEmailLog rec = new HtmlEmailLog(Session.UserInfo.FullName + " (" + Session.UserID + ")")
        {
          Description = "Consent requested",
          Sender = this.txtSenderEmail.Text,
          Subject = this.cboSubject.Text,
          ReadReceipt = this.chkReadReceipt.Checked
        };
        string str1 = string.Empty;
        for (int index = this.pnlBorrowers.Controls.Count - 1; index >= 0; --index)
        {
          switch (this.pnlBorrowers.Controls[index])
          {
            case BorrowerPairConsentControlCC consentControlCc1:
              if (consentControlCc1.BorrowerEnabled || consentControlCc1.CoBorrowerEnabled)
              {
                EBSServiceClient ebsServiceClient = new EBSServiceClient();
                this.loan.SetBorrowerPair(this.loan.GetPairIndex(consentControlCc1.BorrowerPairId));
                string str2 = consentControlCc1.BorrowerChecked ? consentControlCc1.BorrowerEmail : string.Empty;
                string str3 = consentControlCc1.CoBorrowerChecked ? consentControlCc1.CoBorrowerEmail : string.Empty;
                if (consentControlCc1.CoBorrowerChecked && string.IsNullOrEmpty(str3))
                  str3 = consentControlCc1.BorrowerEmail;
                sendNotificationRequest request = new sendNotificationRequest()
                {
                  loanGuid = new Guid(this.loan.GUID.Replace("{", string.Empty).Replace("}", string.Empty)),
                  contentType = "HTML",
                  createdBy = this.txtSenderName.Text,
                  createdDate = DateTime.Now.ToString(),
                  senderFullName = this.txtSenderName.Text,
                  replyTo = this.txtSenderEmail.Text
                };
                Contact contact1 = this.contacts.Find((Predicate<Contact>) (x => x.borrowerId == Session.LoanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id));
                if (consentControlCc1.BorrowerChecked && consentControlCc1.BorrowerEnabled && contact1 != null)
                {
                  request.subject = consentControlCc1.BorrowerName + ": " + this.cboSubject.Text;
                  request.body = HtmlFieldMerge.MergeDynamicConsumerConnectContent(this.htmlEmailControl.Html, contact1.url, consentControlCc1.BorrowerName);
                  request.emails = new string[1]{ str2 };
                  Task.WaitAll(ebsServiceClient.SendNotification(request));
                  if (consentControlCc1.BorrowerEnabled && consentControlCc1.BorrowerChecked && string.IsNullOrEmpty(this.loan.GetField("4108").Replace("//", string.Empty)))
                    this.loan.SetField("4108", DateTime.Now.ToString("MM/dd/yyyy"));
                  rec.Body = request.body;
                  rec.Recipient = str2;
                }
                Contact contact2 = this.contacts.Find((Predicate<Contact>) (x => x.borrowerId == Session.LoanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id));
                if (consentControlCc1.CoBorrowerChecked && consentControlCc1.CoBorrowerEnabled && contact2 != null)
                {
                  request.subject = consentControlCc1.CoBorrowerName + ": " + this.cboSubject.Text;
                  request.body = HtmlFieldMerge.MergeDynamicConsumerConnectContent(this.htmlEmailControl.Html, contact2.url, consentControlCc1.CoBorrowerName);
                  request.emails = new string[1]{ str3 };
                  Task.WaitAll(ebsServiceClient.SendNotification(request));
                  if (consentControlCc1.CoBorrowerEnabled && consentControlCc1.CoBorrowerChecked && string.IsNullOrEmpty(this.loan.GetField("4109").Replace("//", string.Empty)))
                    this.loan.SetField("4109", DateTime.Now.ToString("MM/dd/yyyy"));
                  if (string.IsNullOrEmpty(rec.Recipient))
                  {
                    rec.Recipient = str3;
                    rec.Body = request.body;
                  }
                  else
                  {
                    HtmlEmailLog htmlEmailLog = rec;
                    htmlEmailLog.Recipient = htmlEmailLog.Recipient + ";" + str3;
                  }
                }
              }
              if (!string.IsNullOrEmpty(str1))
              {
                if (string.IsNullOrEmpty(rec.Recipient))
                {
                  rec.Recipient = str1;
                }
                else
                {
                  HtmlEmailLog htmlEmailLog = rec;
                  htmlEmailLog.Recipient = htmlEmailLog.Recipient + ";" + str1;
                }
              }
              if (!string.IsNullOrEmpty(rec.Recipient))
                this.loanDataMgr.AddOperationLog((LogRecordBase) rec);
              str1 = string.Empty;
              break;
            case ContactConsentControlCC consentControlCc2:
              if (consentControlCc2.ContactEnabled)
              {
                EBSServiceClient ebsServiceClient = new EBSServiceClient();
                string contactEmail = consentControlCc2.ContactEmail;
                NonBorrowingOwner nonBorrowingOwner = (NonBorrowingOwner) consentControlCc2.DataTag;
                sendNotificationRequest request = new sendNotificationRequest()
                {
                  loanGuid = new Guid(this.loan.GUID.Replace("{", string.Empty).Replace("}", string.Empty)),
                  contentType = "HTML",
                  createdBy = this.txtSenderName.Text,
                  createdDate = DateTime.Now.ToString(),
                  senderFullName = this.txtSenderName.Text,
                  replyTo = this.txtSenderEmail.Text
                };
                Contact contact = this.contacts.Find((Predicate<Contact>) (x => x.borrowerId == nonBorrowingOwner.NBOID));
                if (consentControlCc2.ContactChecked && consentControlCc2.ContactEnabled && contact != null)
                {
                  request.subject = consentControlCc2.ContactName + ": " + this.cboSubject.Text;
                  request.body = HtmlFieldMerge.MergeDynamicConsumerConnectContent(this.htmlEmailControl.Html, contact.url, consentControlCc2.ContactName);
                  request.emails = new string[1]
                  {
                    contactEmail
                  };
                  Task.WaitAll(ebsServiceClient.SendNotification(request));
                  string id = "NBOC" + (nonBorrowingOwner.NboIndex <= 99 ? nonBorrowingOwner.NboIndex.ToString("00") : nonBorrowingOwner.NboIndex.ToString("000")) + "21";
                  if (consentControlCc2.ContactEnabled && consentControlCc2.ContactChecked && string.IsNullOrEmpty(this.loan.GetField(id).Replace("//", string.Empty)))
                    this.loan.SetField(id, DateTime.Now.ToString("MM/dd/yyyy"));
                  if (string.IsNullOrEmpty(str1))
                  {
                    str1 = contactEmail;
                    rec.Body = request.body;
                    break;
                  }
                  str1 = str1 + ";" + contactEmail;
                  break;
                }
                break;
              }
              break;
          }
        }
        if (pairIndex != this.loan.GetPairIndex(this.loan.PairId))
          this.loan.SetBorrowerPair(pairIndex);
        HtmlEmailTemplate selectedItem = this.cboSubject.SelectedItem as HtmlEmailTemplate;
        if (selectedItem != (HtmlEmailTemplate) null)
          Session.WritePrivateProfileString("CCEmailTemplates", EmailManager.EmailType.Consent.ToString(), selectedItem.Guid);
        EllieMae.EMLite.eFolder.eDelivery.EmailFrom.addEmailToNotificationSettings(new Guid(this.loan.GUID), this.txtSenderEmail.Text);
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool validateAuthenticationCode(string authenticationCode)
    {
      return authenticationCode.Length >= 4 && authenticationCode.Length <= 10 && int.TryParse(authenticationCode, out int _) && authenticationCode.All<char>(new Func<char, bool>(char.IsDigit));
    }

    private string validateSenderContactDetails()
    {
      if (!this.txtSenderName.ReadOnly && string.IsNullOrWhiteSpace(this.txtSenderName.Text))
        return "Please Enter value for Sender's Name";
      if (!this.txtSenderEmail.ReadOnly && string.IsNullOrWhiteSpace(this.txtSenderEmail.Text))
        return "Please Enter value for Sender's Email";
      return !this.txtSenderEmail.ReadOnly && !Utils.ValidateEmail(this.txtSenderEmail.Text) ? "Please Enter valid value for Sender's Email" : string.Empty;
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
      contactDetails.Add("ContactType", (this.cboSenderType.SelectedItem as FieldOption).Value);
      this.saveFromUser(this.loanDataMgr, contactDetails);
    }

    private void saveFromUser(LoanDataMgr loanDataMgr, Dictionary<string, string> contactDetails)
    {
      switch (contactDetails["ContactType"])
      {
        case "Current User":
          if (contactDetails.ContainsKey("Email"))
            Session.UserInfo.Email = contactDetails["Email"];
          if (contactDetails.ContainsKey("Name"))
          {
            string[] source = contactDetails["Name"].Split(' ');
            string str = string.Empty;
            if (((IEnumerable<string>) source).Count<string>() > 1)
              str = ((IEnumerable<string>) source).ToList<string>().LastOrDefault<string>();
            Session.UserInfo.FirstName = contactDetails["Name"].Replace(" " + str, string.Empty);
            if (!string.IsNullOrEmpty(str))
              Session.UserInfo.LastName = str;
          }
          Session.User.UpdatePersonalInfo(Session.UserInfo.FirstName, Session.UserInfo.LastName, Session.UserInfo.Email, Session.UserInfo.Phone, Session.UserInfo.CellPhone, Session.UserInfo.Fax);
          break;
        case "File Starter":
          foreach (MilestoneLog allMilestone in loanDataMgr.LoanData.GetLogList().GetAllMilestones())
          {
            if (allMilestone.Stage == "Started")
            {
              string loanAssociateId = allMilestone.LoanAssociateID;
              string email = allMilestone.LoanAssociateEmail;
              string fullName = allMilestone.LoanAssociateName;
              string loanAssociateTitle = allMilestone.LoanAssociateTitle;
              string loanAssociatePhone = allMilestone.LoanAssociatePhone;
              string associateCellPhone = allMilestone.LoanAssociateCellPhone;
              string loanAssociateFax = allMilestone.LoanAssociateFax;
              if (contactDetails.ContainsKey("Name"))
                fullName = contactDetails["Name"];
              if (contactDetails.ContainsKey("Email"))
                email = contactDetails["Email"];
              allMilestone.SetLoanAssociate(loanAssociateId, fullName, email, loanAssociatePhone, associateCellPhone, loanAssociateFax, loanAssociateTitle);
            }
          }
          break;
        case "Loan Officer":
          new FileContacts(this.loan).UpdateContactDetails(contactDetails);
          break;
      }
    }

    private void saveRecipients()
    {
      this.contacts = new List<Contact>();
      Guid guid;
      foreach (Control control in (ArrangedElementCollection) this.pnlBorrowers.Controls)
      {
        if (control != null && control.GetType() == typeof (BorrowerPairConsentControlCC))
        {
          BorrowerPairConsentControlCC consentControlCc = control as BorrowerPairConsentControlCC;
          this.loan.SetBorrowerPair(this.loan.GetPairIndex(consentControlCc.BorrowerPairId));
          if (consentControlCc.BorrowerEnabled && consentControlCc.BorrowerChecked)
          {
            Dictionary<string, string> contactDetails1 = new Dictionary<string, string>();
            if (consentControlCc.BorrowerNameUpdated)
              contactDetails1.Add("Name", consentControlCc.BorrowerName);
            if (consentControlCc.BorrowerEmailUpdated)
              contactDetails1.Add("Email", consentControlCc.BorrowerEmail);
            if (contactDetails1.Count > 0)
            {
              contactDetails1.Add("ContactType", "Borrower");
              new FileContacts(this.loan).UpdateContactDetails(contactDetails1);
            }
            ContactDetails contactDetails2 = this._contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == Session.LoanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id));
            List<Contact> contacts = this.contacts;
            string id = this.loan.CurrentBorrowerPair.Borrower.Id;
            string authenticationCode = consentControlCc.BorrowerAuthenticationCode;
            string recipientId;
            if (contactDetails2 != null)
            {
              recipientId = contactDetails2.recipientId;
            }
            else
            {
              guid = Guid.NewGuid();
              recipientId = guid.ToString();
            }
            string borrowerName = consentControlCc.BorrowerName;
            string borrowerEmail = consentControlCc.BorrowerEmail;
            Contact ebsContact = this.createEBSContact(id, authenticationCode, recipientId, borrowerName, borrowerEmail, "Borrower");
            contacts.Add(ebsContact);
          }
          if (consentControlCc.CoBorrowerEnabled && consentControlCc.CoBorrowerChecked)
          {
            Dictionary<string, string> contactDetails3 = new Dictionary<string, string>();
            if (consentControlCc.CoBorrowerNameUpdated)
              contactDetails3.Add("Name", consentControlCc.CoBorrowerName);
            if (consentControlCc.CoBorrowerEmailUpdated)
              contactDetails3.Add("Email", consentControlCc.CoBorrowerEmail);
            if (contactDetails3.Count > 0)
            {
              contactDetails3.Add("ContactType", "Coborrower");
              new FileContacts(this.loan).UpdateContactDetails(contactDetails3);
            }
            ContactDetails contactDetails4 = this._contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == Session.LoanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id));
            List<Contact> contacts = this.contacts;
            string id = this.loan.CurrentBorrowerPair.CoBorrower.Id;
            string authenticationCode = consentControlCc.CoBorrowerAuthenticationCode;
            string recipientId;
            if (contactDetails4 != null)
            {
              recipientId = contactDetails4.recipientId;
            }
            else
            {
              guid = Guid.NewGuid();
              recipientId = guid.ToString();
            }
            string coBorrowerName = consentControlCc.CoBorrowerName;
            string coBorrowerEmail = consentControlCc.CoBorrowerEmail;
            Contact ebsContact = this.createEBSContact(id, authenticationCode, recipientId, coBorrowerName, coBorrowerEmail, "Borrower");
            contacts.Add(ebsContact);
          }
        }
        else if (control != null && control is ContactConsentControlCC)
        {
          ContactConsentControlCC recipientcontrol = control as ContactConsentControlCC;
          if (recipientcontrol.ContactEnabled && recipientcontrol.ContactChecked)
          {
            Dictionary<string, string> contactDetails5 = new Dictionary<string, string>();
            if (recipientcontrol.ContactNameUpdated)
              contactDetails5.Add("Name", recipientcontrol.ContactName);
            if (recipientcontrol.ContactEmailUpdated)
              contactDetails5.Add("Email", recipientcontrol.ContactEmail);
            if (recipientcontrol.ContactNameUpdated || recipientcontrol.ContactEmailUpdated)
              contactDetails5.Add("NboIndex", ((NonBorrowingOwner) recipientcontrol.DataTag).NboIndex.ToString());
            if (contactDetails5.Count > 0)
            {
              contactDetails5.Add("ContactType", "Non-Borrowing Owner");
              new FileContacts(this.loan).UpdateContactDetails(contactDetails5);
            }
            ContactDetails contactDetails6 = this._contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == ((NonBorrowingOwner) recipientcontrol.DataTag).NBOID));
            List<Contact> contacts = this.contacts;
            string nboid = ((NonBorrowingOwner) recipientcontrol.DataTag).NBOID;
            string authenticationCode = recipientcontrol.ContactAuthenticationCode;
            string recipientId;
            if (contactDetails6 != null)
            {
              recipientId = contactDetails6.recipientId;
            }
            else
            {
              guid = Guid.NewGuid();
              recipientId = guid.ToString();
            }
            string contactName = recipientcontrol.ContactName;
            string contactEmail = recipientcontrol.ContactEmail;
            Contact ebsContact = this.createEBSContact(nboid, authenticationCode, recipientId, contactName, contactEmail, "NonBorrowingOwner");
            contacts.Add(ebsContact);
          }
        }
      }
      if (this.contacts.Count <= 0)
        return;
      Task<GetRecipientURLResponse> recipientUrl = new EBSServiceClient().GetRecipientURL(new GetRecipientURLRequest()
      {
        loanId = this.loanDataMgr.LoanData.GetField("GUID").Replace("{", string.Empty).Replace("}", string.Empty),
        siteId = !string.IsNullOrEmpty(this.loanDataMgr.LoanData.GetField("ConsumerConnectSiteID")) ? this.loanDataMgr.LoanData.GetField("ConsumerConnectSiteID") : "1234567890",
        contacts = this.contacts.ToArray()
      });
      Task.WaitAll((Task) recipientUrl);
      foreach (Laturl laturl1 in ((IEnumerable<Laturl>) recipientUrl.Result.latUrls).ToList<Laturl>())
      {
        Laturl laturl = laturl1;
        Contact contact = this.contacts.Find((Predicate<Contact>) (x => x.recipientId == laturl.recipientId));
        if (contact != null)
          contact.url = laturl.url;
      }
    }

    private bool validateRecipients()
    {
      Dictionary<string, string> source = new Dictionary<string, string>();
      int num1 = 0;
      foreach (Control control in (ArrangedElementCollection) this.pnlBorrowers.Controls)
      {
        if (control != null && control.GetType() == typeof (BorrowerPairConsentControlCC))
        {
          BorrowerPairConsentControlCC recipientcontrol = control as BorrowerPairConsentControlCC;
          int pairIndex = this.loan.GetPairIndex(recipientcontrol.BorrowerPairId);
          this.loan.SetBorrowerPair(pairIndex);
          string str1 = recipientcontrol.BorrowerChecked ? recipientcontrol.BorrowerEmail : string.Empty;
          string str2 = recipientcontrol.CoBorrowerChecked ? recipientcontrol.CoBorrowerEmail : string.Empty;
          string str3 = recipientcontrol.BorrowerChecked ? recipientcontrol.BorrowerAuthenticationCode : string.Empty;
          string str4 = recipientcontrol.CoBorrowerChecked ? recipientcontrol.CoBorrowerAuthenticationCode : string.Empty;
          if (recipientcontrol.BorrowerEnabled && recipientcontrol.BorrowerChecked)
          {
            if (string.IsNullOrEmpty(recipientcontrol.BorrowerName.Trim()))
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "Borrower's name is required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (string.IsNullOrEmpty(recipientcontrol.BorrowerEmail.Trim()))
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "Borrower's email is required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (string.IsNullOrWhiteSpace(recipientcontrol.BorrowerAuthenticationCode))
            {
              int num4 = (int) Utils.Dialog((IWin32Window) this, "Borrower Recipient must have an Authentication Code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (!this.validateAuthenticationCode(recipientcontrol.BorrowerAuthenticationCode))
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this, "Borrower's Authentication Code must have at least 4, and no more than 10, numbers.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (recipientcontrol.BorrAuthenticationEnabled)
              source.Add("Borrower" + pairIndex.ToString(), recipientcontrol.BorrowerAuthenticationCode);
          }
          if (recipientcontrol.CoBorrowerEnabled && recipientcontrol.CoBorrowerChecked)
          {
            if (string.IsNullOrEmpty(recipientcontrol.CoBorrowerName.Trim()))
            {
              int num6 = (int) Utils.Dialog((IWin32Window) this, "Co-Borrower's name is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (string.IsNullOrEmpty(str2))
            {
              int num7 = (int) Utils.Dialog((IWin32Window) this, "Co-Borrower's email is required (or on checked Borrower).", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (string.IsNullOrWhiteSpace(recipientcontrol.CoBorrowerAuthenticationCode))
            {
              int num8 = (int) Utils.Dialog((IWin32Window) this, "Co-Borrower's Recipient must have an Authentication Code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (!this.validateAuthenticationCode(recipientcontrol.CoBorrowerAuthenticationCode))
            {
              int num9 = (int) Utils.Dialog((IWin32Window) this, "Co-Borrower's Authentication Code must have at least 4, and no more than 10, numbers.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (source.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (c => c.Value == recipientcontrol.CoBorrowerAuthenticationCode)))
            {
              int num10 = (int) Utils.Dialog((IWin32Window) this, "Authorization Code must be unique for each Recipient. Please enter unique Authentication Code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
            if (recipientcontrol.CoBorrAuthenticationEnabled)
              source.Add("CoBorrower" + pairIndex.ToString(), recipientcontrol.CoBorrowerAuthenticationCode);
          }
          string htmlBodyText = this.htmlEmailControl.HtmlBodyText;
          if (htmlBodyText != null && (!recipientcontrol.BorrAuthenticationEnabled || recipientcontrol.BorrowerChecked) && !string.IsNullOrEmpty(recipientcontrol.BorrowerAuthenticationCode) && htmlBodyText.Contains(recipientcontrol.BorrowerAuthenticationCode))
          {
            int num11 = (int) Utils.Dialog((IWin32Window) this, "Message may not contain the borrower Authorization Code used for signing.");
            return false;
          }
          if (htmlBodyText != null && (!recipientcontrol.CoBorrAuthenticationEnabled || recipientcontrol.CoBorrowerChecked) && !string.IsNullOrEmpty(recipientcontrol.CoBorrowerAuthenticationCode) && htmlBodyText.Contains(recipientcontrol.CoBorrowerAuthenticationCode))
          {
            int num12 = (int) Utils.Dialog((IWin32Window) this, "Message may not contain the coborrower Authorization Code used for signing.");
            return false;
          }
          string text = this.cboSubject.Text;
          if (text != null && (!recipientcontrol.BorrAuthenticationEnabled || recipientcontrol.BorrowerChecked) && !string.IsNullOrEmpty(recipientcontrol.BorrowerAuthenticationCode) && text.Contains(recipientcontrol.BorrowerAuthenticationCode))
          {
            int num13 = (int) Utils.Dialog((IWin32Window) this, "Subject may not contain the borrower Authorization Code used for signing.");
            return false;
          }
          if (text != null && (!recipientcontrol.CoBorrAuthenticationEnabled || recipientcontrol.CoBorrowerChecked) && !string.IsNullOrEmpty(recipientcontrol.CoBorrowerAuthenticationCode) && text.Contains(recipientcontrol.CoBorrowerAuthenticationCode))
          {
            int num14 = (int) Utils.Dialog((IWin32Window) this, "Subject may not contain the coborrower Authorization Code used for signing.");
            return false;
          }
        }
        else if (control != null && control is ContactConsentControlCC)
        {
          ContactConsentControlCC recipientcontrol = control as ContactConsentControlCC;
          int pairIndex = this.loan.GetPairIndex(recipientcontrol.BorrowerPairId);
          string str5 = recipientcontrol.ContactChecked ? recipientcontrol.ContactEmail : string.Empty;
          string str6 = recipientcontrol.ContactChecked ? recipientcontrol.ContactAuthenticationCode : string.Empty;
          if (recipientcontrol.ContactEnabled)
          {
            if (recipientcontrol.ContactChecked)
            {
              if (string.IsNullOrEmpty(recipientcontrol.ContactName.Trim()))
              {
                int num15 = (int) Utils.Dialog((IWin32Window) this, "Non-Borrowing Owner’s name is required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (string.IsNullOrEmpty(recipientcontrol.ContactEmail.Trim()))
              {
                int num16 = (int) Utils.Dialog((IWin32Window) this, "Non-Borrowing Owner’s email is required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (string.IsNullOrWhiteSpace(recipientcontrol.ContactAuthenticationCode))
              {
                int num17 = (int) Utils.Dialog((IWin32Window) this, "Contact Recipient must have an Authentication Code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (!this.validateAuthenticationCode(recipientcontrol.ContactAuthenticationCode))
              {
                int num18 = (int) Utils.Dialog((IWin32Window) this, "Contact's Authentication Code must have at least 4, and no more than 10, numbers.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (source.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (c => c.Value == recipientcontrol.ContactAuthenticationCode)))
              {
                int num19 = (int) Utils.Dialog((IWin32Window) this, "Authorization Code must be unique for each Recipient. Please enter unique Authentication Code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (recipientcontrol.ContactAuthenticationEnabled)
                source.Add("NBO" + num1.ToString() + pairIndex.ToString(), recipientcontrol.ContactAuthenticationCode);
            }
            ++num1;
          }
          string htmlBodyText = this.htmlEmailControl.HtmlBodyText;
          if (htmlBodyText != null && (!recipientcontrol.ContactAuthenticationEnabled || recipientcontrol.ContactChecked) && !string.IsNullOrEmpty(recipientcontrol.ContactAuthenticationCode) && htmlBodyText.Contains(recipientcontrol.ContactAuthenticationCode))
          {
            int num20 = (int) Utils.Dialog((IWin32Window) this, "Message may not contain the borrower Authorization Code used for signing.");
            return false;
          }
          string text = this.cboSubject.Text;
          if (text != null && (!recipientcontrol.ContactAuthenticationEnabled || recipientcontrol.ContactChecked) && !string.IsNullOrEmpty(recipientcontrol.ContactAuthenticationCode) && text.Contains(recipientcontrol.ContactAuthenticationCode))
          {
            int num21 = (int) Utils.Dialog((IWin32Window) this, "Subject may not contain the borrower Authorization Code used for signing.");
            return false;
          }
        }
      }
      if (source.GroupBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (x => x.Value)).Where<IGrouping<string, KeyValuePair<string, string>>>((Func<IGrouping<string, KeyValuePair<string, string>>, bool>) (x => x.Count<KeyValuePair<string, string>>() > 1)).Count<IGrouping<string, KeyValuePair<string, string>>>() <= 0)
        return true;
      int num22 = (int) Utils.Dialog((IWin32Window) this, "Authorization Code must be unique for each Recipient. Please enter unique Authentication Code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private Contact createEBSContact(
      string id,
      string authCode,
      string recipientId,
      string name,
      string email,
      string role)
    {
      return new Contact()
      {
        borrowerId = id,
        contactType = role,
        authType = "AuthCode",
        authCode = authCode,
        recipientId = string.IsNullOrEmpty(recipientId) ? Guid.NewGuid().ToString() : recipientId,
        name = name,
        email = email
      };
    }

    private void chkAcceptBy_CheckedChanged(object sender, EventArgs e)
    {
      this.dateAcceptBy.Enabled = this.chkAcceptBy.Checked;
    }

    private void cboSenderType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setSenderInfo();
      this.cboSubject_SelectedIndexChanged(sender, e);
    }

    private void setSenderInfo()
    {
      string str = (this.cboSenderType.SelectedItem as FieldOption).Value;
      EmailFromType userType = EmailFromType.CurrentUser;
      switch (str)
      {
        case "Current User":
          userType = EmailFromType.CurrentUser;
          break;
        case "File Starter":
          userType = EmailFromType.FileStarter;
          break;
        case "Loan Officer":
          userType = EmailFromType.LoanOfficer;
          break;
      }
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

    private void cboSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
      HtmlEmailTemplate selectedItem1 = this.cboSubject.SelectedItem as HtmlEmailTemplate;
      if (selectedItem1 == (HtmlEmailTemplate) null)
        return;
      UserInfo userInfo = (UserInfo) null;
      if (this.cboSenderType.SelectedItem is FieldOption selectedItem2)
        userInfo = !(selectedItem2.Value == "Current User") ? Session.OrganizationManager.GetUser(selectedItem2.ReportingDatabaseValue) : Session.UserInfo;
      string html = new HtmlFieldMerge(selectedItem1.Html).MergeContent(this.loanDataMgr.LoanData, userInfo);
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

    private void SendConsentRequestDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
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
      this.label11 = new Label();
      this.label12 = new Label();
      this.label10 = new Label();
      this.label9 = new Label();
      this.label8 = new Label();
      this.txtSenderEmail = new TextBox();
      this.txtSenderName = new TextBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.pnlBorrowers = new FlowLayoutPanel();
      this.borrowerPairConsentControlCC1 = new BorrowerPairConsentControlCC();
      this.contactConsentControlCC1 = new ContactConsentControlCC();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.dateAcceptBy = new DateTimePicker();
      this.lblNotifyUserCount = new Label();
      this.btnNotifyUsers = new Button();
      this.cboSubject = new ComboBox();
      this.cboSenderType = new ComboBox();
      this.htmlEmailControl = new HtmlEmailControl();
      this.chkAcceptBy = new CheckBox();
      this.chkReadReceipt = new CheckBox();
      this.lblSenderType = new Label();
      this.lblSubject = new Label();
      this.helpLink = new EMHelpLink();
      this.gcMessage.SuspendLayout();
      this.pnlBorrowers.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(651, 627);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSend.Location = new Point(568, 627);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 22);
      this.btnSend.TabIndex = 3;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.gcMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcMessage.Controls.Add((Control) this.label11);
      this.gcMessage.Controls.Add((Control) this.label12);
      this.gcMessage.Controls.Add((Control) this.label10);
      this.gcMessage.Controls.Add((Control) this.label9);
      this.gcMessage.Controls.Add((Control) this.label8);
      this.gcMessage.Controls.Add((Control) this.txtSenderEmail);
      this.gcMessage.Controls.Add((Control) this.txtSenderName);
      this.gcMessage.Controls.Add((Control) this.label6);
      this.gcMessage.Controls.Add((Control) this.label7);
      this.gcMessage.Controls.Add((Control) this.pnlBorrowers);
      this.gcMessage.Controls.Add((Control) this.label5);
      this.gcMessage.Controls.Add((Control) this.label4);
      this.gcMessage.Controls.Add((Control) this.label3);
      this.gcMessage.Controls.Add((Control) this.label2);
      this.gcMessage.Controls.Add((Control) this.label1);
      this.gcMessage.Controls.Add((Control) this.dateAcceptBy);
      this.gcMessage.Controls.Add((Control) this.lblNotifyUserCount);
      this.gcMessage.Controls.Add((Control) this.btnNotifyUsers);
      this.gcMessage.Controls.Add((Control) this.cboSubject);
      this.gcMessage.Controls.Add((Control) this.cboSenderType);
      this.gcMessage.Controls.Add((Control) this.htmlEmailControl);
      this.gcMessage.Controls.Add((Control) this.chkAcceptBy);
      this.gcMessage.Controls.Add((Control) this.chkReadReceipt);
      this.gcMessage.Controls.Add((Control) this.lblSenderType);
      this.gcMessage.Controls.Add((Control) this.lblSubject);
      this.gcMessage.HeaderForeColor = SystemColors.ControlText;
      this.gcMessage.Location = new Point(8, 8);
      this.gcMessage.Margin = new Padding(0);
      this.gcMessage.Name = "gcMessage";
      this.gcMessage.Size = new Size(717, 599);
      this.gcMessage.TabIndex = 0;
      this.gcMessage.Text = "Message";
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(612, 91);
      this.label11.Name = "label11";
      this.label11.Size = new Size(36, 14);
      this.label11.TabIndex = 60;
      this.label11.Text = "Code";
      this.label11.TextAlign = ContentAlignment.MiddleLeft;
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(612, 75);
      this.label12.Name = "label12";
      this.label12.Size = new Size(81, 14);
      this.label12.TabIndex = 59;
      this.label12.Text = "Authorization";
      this.label12.TextAlign = ContentAlignment.MiddleLeft;
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(433, 73);
      this.label10.Name = "label10";
      this.label10.Size = new Size(110, 14);
      this.label10.TabIndex = 58;
      this.label10.Text = "eConsent Request";
      this.label10.TextAlign = ContentAlignment.MiddleLeft;
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(14, 91);
      this.label9.Name = "label9";
      this.label9.Size = new Size(33, 14);
      this.label9.TabIndex = 57;
      this.label9.Text = "Type";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(14, 75);
      this.label8.Name = "label8";
      this.label8.Size = new Size(58, 14);
      this.label8.TabIndex = 56;
      this.label8.Text = "Recipient";
      this.label8.TextAlign = ContentAlignment.MiddleLeft;
      this.txtSenderEmail.Location = new Point(530, 52);
      this.txtSenderEmail.Name = "txtSenderEmail";
      this.txtSenderEmail.ReadOnly = true;
      this.txtSenderEmail.Size = new Size(175, 20);
      this.txtSenderEmail.TabIndex = 55;
      this.txtSenderName.Location = new Point(322, 50);
      this.txtSenderName.Name = "txtSenderName";
      this.txtSenderName.ReadOnly = true;
      this.txtSenderName.Size = new Size(175, 20);
      this.txtSenderName.TabIndex = 54;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(527, 36);
      this.label6.Name = "label6";
      this.label6.Size = new Size(37, 13);
      this.label6.TabIndex = 53;
      this.label6.Text = "Email";
      this.label6.TextAlign = ContentAlignment.MiddleLeft;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(319, 34);
      this.label7.Name = "label7";
      this.label7.Size = new Size(39, 13);
      this.label7.TabIndex = 52;
      this.label7.Text = "Name";
      this.label7.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlBorrowers.AutoScroll = true;
      this.pnlBorrowers.Controls.Add((Control) this.borrowerPairConsentControlCC1);
      this.pnlBorrowers.Controls.Add((Control) this.contactConsentControlCC1);
      this.pnlBorrowers.Location = new Point(11, 110);
      this.pnlBorrowers.Name = "pnlBorrowers";
      this.pnlBorrowers.Size = new Size(702, 76);
      this.pnlBorrowers.TabIndex = 21;
      this.borrowerPairConsentControlCC1.AutoScroll = true;
      this.borrowerPairConsentControlCC1.BackColor = SystemColors.Control;
      this.borrowerPairConsentControlCC1.BorrowerAccepted = "XXXX";
      this.borrowerPairConsentControlCC1.BorrowerAuthenticationCode = "";
      this.borrowerPairConsentControlCC1.BorrowerChecked = false;
      this.borrowerPairConsentControlCC1.BorrowerConsentSent = "XXXX";
      this.borrowerPairConsentControlCC1.BorrowerEmail = "";
      this.borrowerPairConsentControlCC1.BorrowerEnabled = false;
      this.borrowerPairConsentControlCC1.BorrowerName = "";
      this.borrowerPairConsentControlCC1.BorrowerPairId = (string) null;
      this.borrowerPairConsentControlCC1.BorrowerRejected = "XXXX";
      this.borrowerPairConsentControlCC1.CoBorrowerAccepted = "";
      this.borrowerPairConsentControlCC1.CoBorrowerAuthenticationCode = "";
      this.borrowerPairConsentControlCC1.CoBorrowerChecked = false;
      this.borrowerPairConsentControlCC1.CoBorrowerConsentSent = "";
      this.borrowerPairConsentControlCC1.CoBorrowerEmail = "";
      this.borrowerPairConsentControlCC1.CoBorrowerEnabled = false;
      this.borrowerPairConsentControlCC1.CoBorrowerName = "";
      this.borrowerPairConsentControlCC1.CoBorrowerRejected = "";
      this.borrowerPairConsentControlCC1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.borrowerPairConsentControlCC1.Location = new Point(0, 0);
      this.borrowerPairConsentControlCC1.Margin = new Padding(0);
      this.borrowerPairConsentControlCC1.Name = "borrowerPairConsentControlCC1";
      this.borrowerPairConsentControlCC1.Size = new Size(674, 49);
      this.borrowerPairConsentControlCC1.TabIndex = 0;
      this.contactConsentControlCC1.AutoScroll = true;
      this.contactConsentControlCC1.BackColor = SystemColors.Control;
      this.contactConsentControlCC1.BorrowerPairId = (string) null;
      this.contactConsentControlCC1.ContactAccepted = "";
      this.contactConsentControlCC1.ContactAuthenticationCode = "";
      this.contactConsentControlCC1.ContactChecked = false;
      this.contactConsentControlCC1.ContactConsentSent = "";
      this.contactConsentControlCC1.ContactEmail = "";
      this.contactConsentControlCC1.ContactEnabled = false;
      this.contactConsentControlCC1.ContactName = "";
      this.contactConsentControlCC1.ContactRejected = "";
      this.contactConsentControlCC1.ContactType = "Non-Borrowing Owner";
      this.contactConsentControlCC1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.contactConsentControlCC1.Location = new Point(0, 49);
      this.contactConsentControlCC1.Margin = new Padding(0);
      this.contactConsentControlCC1.Name = "contactConsentControlCC1";
      this.contactConsentControlCC1.Size = new Size(674, 24);
      this.contactConsentControlCC1.TabIndex = 1;
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(533, 91);
      this.label5.Name = "label5";
      this.label5.Size = new Size(55, 14);
      this.label5.TabIndex = 20;
      this.label5.Text = "Rejected";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(462, 91);
      this.label4.Name = "label4";
      this.label4.Size = new Size(59, 14);
      this.label4.TabIndex = 19;
      this.label4.Text = "Accepted";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(381, 91);
      this.label3.Name = "label3";
      this.label3.Size = new Size(32, 14);
      this.label3.TabIndex = 18;
      this.label3.Text = "Sent";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(257, 91);
      this.label2.Name = "label2";
      this.label2.Size = new Size(36, 14);
      this.label2.TabIndex = 17;
      this.label2.Text = "Email";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(150, 91);
      this.label1.Name = "label1";
      this.label1.Size = new Size(38, 14);
      this.label1.TabIndex = 16;
      this.label1.Text = "Name";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.dateAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.dateAcceptBy.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateAcceptBy.CalendarTitleForeColor = Color.White;
      this.dateAcceptBy.CalendarTrailingForeColor = Color.FromArgb(156, 156, 156);
      this.dateAcceptBy.CustomFormat = "MM/dd/yyyy";
      this.dateAcceptBy.Enabled = false;
      this.dateAcceptBy.Format = DateTimePickerFormat.Custom;
      this.dateAcceptBy.Location = new Point(489, 570);
      this.dateAcceptBy.Name = "dateAcceptBy";
      this.dateAcceptBy.Size = new Size(100, 20);
      this.dateAcceptBy.TabIndex = 13;
      this.dateAcceptBy.Visible = false;
      this.lblNotifyUserCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblNotifyUserCount.AutoSize = true;
      this.lblNotifyUserCount.Location = new Point(14, 567);
      this.lblNotifyUserCount.Name = "lblNotifyUserCount";
      this.lblNotifyUserCount.Size = new Size(105, 14);
      this.lblNotifyUserCount.TabIndex = 8;
      this.lblNotifyUserCount.Text = "({0} Users selected)";
      this.btnNotifyUsers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNotifyUsers.Location = new Point(12, 533);
      this.btnNotifyUsers.Name = "btnNotifyUsers";
      this.btnNotifyUsers.Size = new Size(129, 22);
      this.btnNotifyUsers.TabIndex = 5;
      this.btnNotifyUsers.Text = "Notify Additional Users";
      this.btnNotifyUsers.Click += new EventHandler(this.btnNotifyUsers_Click);
      this.cboSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSubject.FormattingEnabled = true;
      this.cboSubject.Location = new Point(89, 199);
      this.cboSubject.Name = "cboSubject";
      this.cboSubject.Size = new Size(616, 22);
      this.cboSubject.Sorted = true;
      this.cboSubject.TabIndex = 9;
      this.cboSubject.SelectedIndexChanged += new EventHandler(this.cboSubject_SelectedIndexChanged);
      this.cboSenderType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSenderType.FormattingEnabled = true;
      this.cboSenderType.Location = new Point(13, 50);
      this.cboSenderType.Name = "cboSenderType";
      this.cboSenderType.Size = new Size(291, 22);
      this.cboSenderType.TabIndex = 1;
      this.cboSenderType.SelectedIndexChanged += new EventHandler(this.cboSenderType_SelectedIndexChanged);
      this.htmlEmailControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(11, 230);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(693, 267);
      this.htmlEmailControl.TabIndex = 10;
      this.chkAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAcceptBy.Location = new Point(236, 570);
      this.chkAcceptBy.Name = "chkAcceptBy";
      this.chkAcceptBy.Size = new Size(252, 19);
      this.chkAcceptBy.TabIndex = 12;
      this.chkAcceptBy.Text = "Notify me when borrower does not access by";
      this.chkAcceptBy.UseVisualStyleBackColor = true;
      this.chkAcceptBy.Visible = false;
      this.chkAcceptBy.CheckedChanged += new EventHandler(this.chkAcceptBy_CheckedChanged);
      this.chkReadReceipt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkReadReceipt.AutoSize = true;
      this.chkReadReceipt.Checked = true;
      this.chkReadReceipt.CheckState = CheckState.Checked;
      this.chkReadReceipt.Location = new Point(443, 537);
      this.chkReadReceipt.Name = "chkReadReceipt";
      this.chkReadReceipt.Size = new Size(261, 18);
      this.chkReadReceipt.TabIndex = 11;
      this.chkReadReceipt.Text = "Notify me when borrower receives the package.";
      this.chkReadReceipt.UseVisualStyleBackColor = true;
      this.lblSenderType.AutoSize = true;
      this.lblSenderType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSenderType.Location = new Point(14, 36);
      this.lblSenderType.Name = "lblSenderType";
      this.lblSenderType.Size = new Size(76, 14);
      this.lblSenderType.TabIndex = 0;
      this.lblSenderType.Text = "Sender Type";
      this.lblSenderType.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(9, 203);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(50, 14);
      this.lblSubject.TabIndex = 8;
      this.lblSubject.Text = "* Subject";
      this.lblSubject.TextAlign = ContentAlignment.MiddleLeft;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Send Consent Request";
      this.helpLink.Location = new Point(8, 631);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 5;
      this.helpLink.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(735, 660);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.gcMessage);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSend);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SendConsentRequestDialogCC);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Send Consent";
      this.KeyDown += new KeyEventHandler(this.SendConsentRequestDialog_KeyDown);
      this.gcMessage.ResumeLayout(false);
      this.gcMessage.PerformLayout();
      this.pnlBorrowers.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
