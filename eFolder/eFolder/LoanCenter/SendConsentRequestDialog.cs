// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.SendConsentRequestDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.ConsentServiceController;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class SendConsentRequestDialog : Form
  {
    private const string className = "SendConsentRequestDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private LoanData loan;
    private string packageID;
    private string comments;
    private IContainer components;
    private Button btnCancel;
    private Button btnSend;
    private GroupContainer gcMessage;
    private CheckBox chkReadReceipt;
    private Label lblSubject;
    private CheckBox chkAcceptBy;
    private DateTimePicker dateAcceptBy;
    private HtmlEmailControl htmlEmailControl;
    private ComboBox cboSubject;
    private Label lblNotifyUserCount;
    private Button btnNotifyUsers;
    private Label label1;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private FlowLayoutPanel pnlBorrowers;
    private EMHelpLink helpLink;
    private BorrowerPairConsentControl borrowerPairConsentControl1;
    private Label label6;
    private Label label9;
    private Label label10;
    private TextBox txtSenderEmail;
    private TextBox txtSenderName;
    private Label label7;
    private Label label8;
    private ComboBox cboSenderType;
    private Label lblSenderType;

    public SendConsentRequestDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      Tracing.Log(SendConsentRequestDialog.sw, TraceLevel.Verbose, nameof (SendConsentRequestDialog), "Initializing form.");
      this.loanDataMgr = loanDataMgr;
      this.loan = loanDataMgr.LoanData;
      Cursor.Current = Cursors.WaitCursor;
      this.initBorrowers();
      this.initNotifyCount();
      this.initContents();
      Cursor.Current = Cursors.Default;
      Tracing.Log(SendConsentRequestDialog.sw, TraceLevel.Verbose, nameof (SendConsentRequestDialog), "Form initialized.");
    }

    public string PackageID => this.packageID;

    public string Comments => this.comments;

    private void initFrom()
    {
      if (this.cboSenderType.Items.Count > 0)
        this.cboSenderType.Items.Clear();
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

    private DateTime? parseDate(DateTime? consentDate, System.TimeZoneInfo timeZoneInfo)
    {
      if (!consentDate.HasValue)
        return new DateTime?(DateTime.MinValue);
      DateTime dateTime = consentDate.Value;
      if (timeZoneInfo != null)
        return DateTimeKind.Utc == dateTime.Kind ? new DateTime?(System.TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo)) : new DateTime?(System.TimeZoneInfo.ConvertTime(dateTime, Utils.GetTimeZoneInfo("PST"), timeZoneInfo));
      string timeZoneCode = this.loan.GetField("LE1.XG9") == "" ? this.loan.GetField("LE1.X9") : this.loan.GetField("LE1.XG9");
      return new DateTime?(System.TimeZoneInfo.ConvertTime(dateTime, Utils.GetTimeZoneInfo("PST"), Utils.GetTimeZoneInfo(timeZoneCode)));
    }

    private void initBorrowers()
    {
      Tracing.Log(SendConsentRequestDialog.sw, TraceLevel.Verbose, nameof (SendConsentRequestDialog), "Initializing borrowers...");
      while (this.pnlBorrowers.Controls.Count > 0)
      {
        Control control = this.pnlBorrowers.Controls[0];
        this.pnlBorrowers.Controls.Remove(control);
        control.Dispose();
      }
      UserConsentDataGetResponseUserConsentDataGetResponseBody[] dataGetResponse1 = new ConsentServiceClient().UserConsentDataGet(this.loan.GUID).UserConsentDataGetResponse1;
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
          string str1 = this.loan.GetSimpleField("4000") + this.loan.GetSimpleField("4002");
          string str2 = this.loan.GetSimpleField("4004") + this.loan.GetSimpleField("4006");
          BorrowerPairConsentControl pairConsentControl = new BorrowerPairConsentControl();
          pairConsentControl.BorrowerPairId = borrowerPair.Id;
          pairConsentControl.BorrowerName = borrowerPair.Borrower.ToString();
          pairConsentControl.CoBorrowerName = borrowerPair.CoBorrower.ToString();
          pairConsentControl.BorrowerEmail = this.loan.GetSimpleField("1240");
          pairConsentControl.CoBorrowerEmail = this.loan.GetSimpleField("1268");
          if (num == 0)
            pairConsentControl.Margin = new Padding(0);
          else
            pairConsentControl.Margin = new Padding(0, 5, 0, 0);
          System.TimeZoneInfo timeZoneInfo = (System.TimeZoneInfo) null;
          foreach (DisclosureTracking2015Log disclosureTracking2015Log in ((IEnumerable<DisclosureTracking2015Log>) this.loan.GetLogList().GetAllDisclosureTracking2015Log(false)).ToArray<DisclosureTracking2015Log>())
          {
            if (disclosureTracking2015Log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && disclosureTracking2015Log.IsDisclosed)
            {
              timeZoneInfo = disclosureTracking2015Log.TimeZoneInfo;
              break;
            }
          }
          int? packageId;
          DateTime dateTime;
          foreach (UserConsentDataGetResponseUserConsentDataGetResponseBody dataGetResponseBody in dataGetResponse1)
          {
            packageId = dataGetResponseBody.PackageId;
            if (!packageId.HasValue)
            {
              string str3 = dataGetResponseBody.User.UserFirstName + dataGetResponseBody.User.UserLastName;
              bool flag1 = str3.Trim().ToLower() == str1.Trim().ToLower() && dataGetResponseBody.User.UserEmail.Trim().ToLower() == pairConsentControl.BorrowerEmail.Trim().ToLower();
              bool flag2 = !string.IsNullOrEmpty(pairConsentControl.CoBorrowerEmail) ? str3.Trim().ToLower() == str2.Trim().ToLower() && dataGetResponseBody.User.UserEmail.Trim().ToLower() == pairConsentControl.CoBorrowerEmail.Trim().ToLower() : str3.Trim().ToLower() == str2.Trim().ToLower() && dataGetResponseBody.User.UserEmail.Trim().ToLower() == pairConsentControl.BorrowerEmail.Trim().ToLower();
              if (flag1 | flag2)
              {
                if (dataGetResponseBody.DateConsentRequest.HasValue)
                {
                  dateTime = dataGetResponseBody.DateConsentRequest.Value;
                  if (dateTime.Year != 1900)
                  {
                    if (flag1)
                      pairConsentControl.BorrowerConsentSent = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.DateConsentRequest, timeZoneInfo));
                    if (flag2)
                      pairConsentControl.CoBorrowerConsentSent = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.DateConsentRequest, timeZoneInfo));
                  }
                }
                if (dataGetResponseBody.ConsentDate.HasValue)
                {
                  dateTime = dataGetResponseBody.ConsentDate.Value;
                  if (dateTime.Year != 1900 && dataGetResponseBody.ConsentStatus.HasValue)
                  {
                    if (dataGetResponseBody.ConsentStatus.Value)
                    {
                      if (flag1)
                      {
                        pairConsentControl.BorrowerAccepted = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.ConsentDate, timeZoneInfo));
                        pairConsentControl.BorrowerEnabled = false;
                      }
                      if (flag2)
                      {
                        pairConsentControl.CoBorrowerAccepted = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.ConsentDate, timeZoneInfo));
                        pairConsentControl.CoBorrowerEnabled = false;
                      }
                    }
                    else
                    {
                      if (flag1)
                        pairConsentControl.BorrowerRejected = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.ConsentDate, timeZoneInfo));
                      if (flag2)
                        pairConsentControl.CoBorrowerRejected = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.ConsentDate, timeZoneInfo));
                    }
                  }
                }
              }
            }
          }
          pairConsentControl.BorrowerSelectedChanged += new EventHandler(this.refreshSendButton);
          this.pnlBorrowers.Controls.Add((Control) pairConsentControl);
          ++num;
          foreach (NonBorrowingOwner nonBorrowingOwner in this.loanDataMgr.LoanData.GetNboByBorrowerPairId(borrowerPair.Id))
          {
            if (!string.IsNullOrWhiteSpace(nonBorrowingOwner.LastName) && !string.IsNullOrWhiteSpace(nonBorrowingOwner.FirstName) && !string.IsNullOrWhiteSpace(nonBorrowingOwner.EmailAddress))
            {
              string str4 = string.Join(" ", ((IEnumerable<string>) new string[3]
              {
                nonBorrowingOwner.FirstName,
                nonBorrowingOwner.MiddleName,
                nonBorrowingOwner.LastName
              }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str))));
              NboConsentControl nboConsentControl = new NboConsentControl();
              nboConsentControl.NboEnabled = true;
              nboConsentControl.BorrowerPairId = nonBorrowingOwner.VestingBorrowerPairId;
              nboConsentControl.NboName = str4;
              nboConsentControl.NboEmail = nonBorrowingOwner.EmailAddress;
              nboConsentControl.NboChecked = false;
              nboConsentControl.Margin = new Padding(0);
              nboConsentControl.DataTag = nonBorrowingOwner;
              foreach (UserConsentDataGetResponseUserConsentDataGetResponseBody dataGetResponseBody in dataGetResponse1)
              {
                packageId = dataGetResponseBody.PackageId;
                if (!packageId.HasValue)
                {
                  string str5 = dataGetResponseBody.User.UserFirstName + dataGetResponseBody.User.UserLastName;
                  string str6 = nonBorrowingOwner.FirstName + nonBorrowingOwner.LastName;
                  bool flag = str5.Trim().ToLower() == str6.Trim().ToLower() && dataGetResponseBody.User.UserEmail.Trim().ToLower() == nboConsentControl.NboEmail.Trim().ToLower();
                  if (flag)
                  {
                    if (dataGetResponseBody.DateConsentRequest.HasValue)
                    {
                      dateTime = dataGetResponseBody.DateConsentRequest.Value;
                      if (dateTime.Year != 1900 && flag)
                        nboConsentControl.NboConsentSent = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.DateConsentRequest, timeZoneInfo));
                    }
                    if (dataGetResponseBody.ConsentDate.HasValue)
                    {
                      dateTime = dataGetResponseBody.ConsentDate.Value;
                      if (dateTime.Year != 1900 && dataGetResponseBody.ConsentStatus.HasValue)
                      {
                        if (dataGetResponseBody.ConsentStatus.Value)
                        {
                          if (flag)
                          {
                            nboConsentControl.NboAccepted = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.ConsentDate, timeZoneInfo));
                            nboConsentControl.NboEnabled = false;
                          }
                        }
                        else if (flag)
                          nboConsentControl.NboRejected = string.Format("{0:MM/dd/yyyy}", (object) this.parseDate(dataGetResponseBody.ConsentDate, timeZoneInfo));
                      }
                    }
                  }
                }
              }
              nboConsentControl.NboSelectedChanged += new EventHandler(this.refreshSendButton);
              this.pnlBorrowers.Controls.Add((Control) nboConsentControl);
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
      Tracing.Log(SendConsentRequestDialog.sw, TraceLevel.Verbose, nameof (SendConsentRequestDialog), "InitBorrowers complete.");
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

    private void initSubject()
    {
      HtmlEmailTemplate[] htmlEmailTemplates = Session.ConfigurationManager.GetHtmlEmailTemplates((string) null, HtmlEmailTemplateType.LoanLevelConsent);
      if (htmlEmailTemplates != null)
        this.cboSubject.Items.AddRange((object[]) htmlEmailTemplates);
      string strB = Session.GetPrivateProfileString("EmailTemplates", "Consent");
      if (string.IsNullOrEmpty(strB))
        strB = Session.SessionObjects.GetCompanySettingFromCache("DefaultConsentEmailTemplates", "Consent");
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
        if (control is BorrowerPairConsentControl pairConsentControl)
        {
          if (pairConsentControl.BorrowerChecked || pairConsentControl.CoBorrowerChecked)
          {
            this.btnSend.Enabled = true;
            break;
          }
        }
        else if (control is NboConsentControl nboConsentControl && nboConsentControl.NboChecked)
        {
          this.btnSend.Enabled = true;
          break;
        }
      }
    }

    private LoanCenterMessage createMessage(
      string borrEmail,
      string coBorrEmail,
      List<EncompassContactEmail> nboEmailList)
    {
      LoanCenterMessage message = new LoanCenterMessage(this.loanDataMgr, LoanCenterMessageType.Consent);
      message.ReplyTo = this.txtSenderEmail.Text;
      message.To = string.Empty;
      message.CC = string.Empty;
      if (string.IsNullOrEmpty(borrEmail))
      {
        if (!string.IsNullOrEmpty(coBorrEmail))
          message.CC = coBorrEmail;
      }
      else
      {
        message.To = borrEmail;
        if (!string.IsNullOrEmpty(coBorrEmail))
          message.CC = coBorrEmail;
      }
      message.NBOSendEmails = new List<EncompassContactEmail>();
      if (nboEmailList != null && nboEmailList.Count > 0)
      {
        foreach (EncompassContactEmail nboEmail in nboEmailList)
        {
          if (string.IsNullOrEmpty(nboEmail.email))
            nboEmail.email = message.To;
          message.NBOSendEmails.Add(nboEmail);
        }
      }
      message.Subject = this.cboSubject.Text.Trim();
      message.Body = this.htmlEmailControl.Html;
      message.ReadReceipt = this.chkReadReceipt.Checked;
      if (this.chkAcceptBy.Checked)
        message.NotificationDate = this.dateAcceptBy.Value;
      message.ElectronicSignature = false;
      return message;
    }

    private bool validateMessage(LoanCenterMessage msg)
    {
      List<string> stringList = new List<string>();
      if (msg.ReplyTo == string.Empty)
        stringList.Add("The 'From' email address must be filled in.");
      else if (!Utils.ValidateEmail(msg.ReplyTo))
        stringList.Add("Invalid 'From' email address.");
      if (msg.To == string.Empty && msg.CC == string.Empty && msg.NBOSendEmails.Count == 0)
        stringList.Add("The To, CC or NBO email address must be filled in.");
      else if (!string.IsNullOrEmpty(msg.To) && !Utils.ValidateEmail(msg.To))
        stringList.Add("Invalid 'To' email address.");
      if (msg.To.Contains(",") || msg.To.Contains(";"))
        stringList.Add("The 'To' email address may not have more than one recipient.");
      if (!string.IsNullOrEmpty(msg.CC) && !Utils.ValidateEmail(msg.CC))
        stringList.Add("Invalid 'CC' email address.");
      if (msg.CC.Contains(",") || msg.CC.Contains(";"))
        stringList.Add("The 'CC' email address may not have more than one recipient.");
      if (msg.Subject == string.Empty)
        stringList.Add("Subject is required.");
      if (msg.NotificationDate.Date != DateTime.MinValue.Date && msg.NotificationDate.Date < DateTime.Today)
        stringList.Add("Borrower Accept By Date must be greater than or equal to today's date.");
      foreach (EncompassContactEmail nboSendEmail in msg.NBOSendEmails)
      {
        if (string.IsNullOrEmpty(nboSendEmail.email))
          stringList.Add("Non Borrowing owner email address must be filled in.");
      }
      if (stringList.Count <= 0)
        return true;
      string str = string.Empty;
      for (int index = 0; index < stringList.Count; ++index)
        str = str + "\n\n(" + Convert.ToString(index + 1) + ") " + stringList[index];
      int num = (int) Utils.Dialog((IWin32Window) this, "You must address the following issues before you can send the package:" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private bool sendMessage(LoanCenterMessage msg)
    {
      using (LoanCenterClient loanCenterClient = new LoanCenterClient())
      {
        this.packageID = loanCenterClient.SendMessage(msg);
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
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      foreach (Control control1 in (ArrangedElementCollection) this.pnlBorrowers.Controls)
      {
        if (control1 is BorrowerPairConsentControl pairConsentControl)
        {
          this.loan.SetBorrowerPair(this.loan.GetPairIndex(pairConsentControl.BorrowerPairId));
          string borrEmail = pairConsentControl.BorrowerChecked ? pairConsentControl.BorrowerEmail : string.Empty;
          string coBorrEmail = pairConsentControl.CoBorrowerChecked ? pairConsentControl.CoBorrowerEmail : string.Empty;
          if (pairConsentControl.CoBorrowerChecked && string.IsNullOrEmpty(coBorrEmail))
            coBorrEmail = pairConsentControl.BorrowerEmail;
          List<EncompassContactEmail> nboEmailList = new List<EncompassContactEmail>();
          foreach (Control control2 in (ArrangedElementCollection) this.pnlBorrowers.Controls)
          {
            if (control2 is NboConsentControl nboConsentControl && nboConsentControl.NboChecked && nboConsentControl.DataTag.VestingBorrowerPairId == pairConsentControl.BorrowerPairId)
              nboEmailList.Add(new EncompassContactEmail()
              {
                encompasscontactGuid = nboConsentControl.DataTag.NBOID,
                email = nboConsentControl.NboEmail.ToLower().Trim()
              });
          }
          if (!string.IsNullOrEmpty(borrEmail) || !string.IsNullOrEmpty(coBorrEmail) || nboEmailList.Count > 0)
          {
            LoanCenterMessage message = this.createMessage(borrEmail, coBorrEmail, nboEmailList);
            if (!this.validateMessage(message) || !this.sendMessage(message))
              return;
            message.CreateLogEntry();
          }
        }
        this.loan.SetBorrowerPair(pairIndex);
      }
      HtmlEmailTemplate selectedItem = this.cboSubject.SelectedItem as HtmlEmailTemplate;
      if (selectedItem != (HtmlEmailTemplate) null)
        Session.WritePrivateProfileString("EmailTemplates", "Consent", selectedItem.Guid);
      this.DialogResult = DialogResult.OK;
    }

    private void chkAcceptBy_CheckedChanged(object sender, EventArgs e)
    {
      this.dateAcceptBy.Enabled = this.chkAcceptBy.Checked;
    }

    private void btnFrom_Click(object sender, EventArgs e)
    {
      string str = (string) null;
      using (EmailListDialog emailListDialog = new EmailListDialog(this.loan))
      {
        if (emailListDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        str = emailListDialog.EmailList;
      }
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
      this.pnlBorrowers = new FlowLayoutPanel();
      this.borrowerPairConsentControl1 = new BorrowerPairConsentControl();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label6 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.dateAcceptBy = new DateTimePicker();
      this.lblNotifyUserCount = new Label();
      this.btnNotifyUsers = new Button();
      this.cboSubject = new ComboBox();
      this.htmlEmailControl = new HtmlEmailControl();
      this.chkAcceptBy = new CheckBox();
      this.chkReadReceipt = new CheckBox();
      this.lblSubject = new Label();
      this.helpLink = new EMHelpLink();
      this.txtSenderEmail = new TextBox();
      this.txtSenderName = new TextBox();
      this.label7 = new Label();
      this.label8 = new Label();
      this.cboSenderType = new ComboBox();
      this.lblSenderType = new Label();
      this.gcMessage.SuspendLayout();
      this.pnlBorrowers.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(535, 604);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSend.Enabled = false;
      this.btnSend.Location = new Point(452, 604);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 22);
      this.btnSend.TabIndex = 3;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.gcMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcMessage.Controls.Add((Control) this.txtSenderEmail);
      this.gcMessage.Controls.Add((Control) this.txtSenderName);
      this.gcMessage.Controls.Add((Control) this.label7);
      this.gcMessage.Controls.Add((Control) this.label8);
      this.gcMessage.Controls.Add((Control) this.cboSenderType);
      this.gcMessage.Controls.Add((Control) this.lblSenderType);
      this.gcMessage.Controls.Add((Control) this.pnlBorrowers);
      this.gcMessage.Controls.Add((Control) this.label5);
      this.gcMessage.Controls.Add((Control) this.label4);
      this.gcMessage.Controls.Add((Control) this.label6);
      this.gcMessage.Controls.Add((Control) this.label9);
      this.gcMessage.Controls.Add((Control) this.label10);
      this.gcMessage.Controls.Add((Control) this.label3);
      this.gcMessage.Controls.Add((Control) this.label2);
      this.gcMessage.Controls.Add((Control) this.label1);
      this.gcMessage.Controls.Add((Control) this.dateAcceptBy);
      this.gcMessage.Controls.Add((Control) this.lblNotifyUserCount);
      this.gcMessage.Controls.Add((Control) this.btnNotifyUsers);
      this.gcMessage.Controls.Add((Control) this.cboSubject);
      this.gcMessage.Controls.Add((Control) this.htmlEmailControl);
      this.gcMessage.Controls.Add((Control) this.chkAcceptBy);
      this.gcMessage.Controls.Add((Control) this.chkReadReceipt);
      this.gcMessage.Controls.Add((Control) this.lblSubject);
      this.gcMessage.HeaderForeColor = SystemColors.ControlText;
      this.gcMessage.Location = new Point(8, 23);
      this.gcMessage.Name = "gcMessage";
      this.gcMessage.Size = new Size(601, 576);
      this.gcMessage.TabIndex = 0;
      this.gcMessage.Text = "Message";
      this.pnlBorrowers.AutoScroll = true;
      this.pnlBorrowers.Controls.Add((Control) this.borrowerPairConsentControl1);
      this.pnlBorrowers.Location = new Point(11, 108);
      this.pnlBorrowers.Name = "pnlBorrowers";
      this.pnlBorrowers.Size = new Size(586, 51);
      this.pnlBorrowers.TabIndex = 21;
      this.borrowerPairConsentControl1.AutoScroll = true;
      this.borrowerPairConsentControl1.BackColor = SystemColors.Control;
      this.borrowerPairConsentControl1.BorrowerAccepted = "xxx";
      this.borrowerPairConsentControl1.BorrowerChecked = false;
      this.borrowerPairConsentControl1.BorrowerConsentSent = "xxx";
      this.borrowerPairConsentControl1.BorrowerEmail = "xxx";
      this.borrowerPairConsentControl1.BorrowerEnabled = true;
      this.borrowerPairConsentControl1.BorrowerName = "xxx";
      this.borrowerPairConsentControl1.BorrowerPairId = (string) null;
      this.borrowerPairConsentControl1.BorrowerRejected = "xxx";
      this.borrowerPairConsentControl1.CoBorrowerAccepted = "";
      this.borrowerPairConsentControl1.CoBorrowerChecked = false;
      this.borrowerPairConsentControl1.CoBorrowerConsentSent = "xxx";
      this.borrowerPairConsentControl1.CoBorrowerEmail = "xxx";
      this.borrowerPairConsentControl1.CoBorrowerEnabled = false;
      this.borrowerPairConsentControl1.CoBorrowerName = "";
      this.borrowerPairConsentControl1.CoBorrowerRejected = "";
      this.borrowerPairConsentControl1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.borrowerPairConsentControl1.Location = new Point(0, 0);
      this.borrowerPairConsentControl1.Margin = new Padding(0);
      this.borrowerPairConsentControl1.Name = "borrowerPairConsentControl1";
      this.borrowerPairConsentControl1.Size = new Size(555, 48);
      this.borrowerPairConsentControl1.TabIndex = 0;
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(488, 90);
      this.label5.Name = "label5";
      this.label5.Size = new Size(55, 14);
      this.label5.TabIndex = 20;
      this.label5.Text = "Rejected";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(416, 90);
      this.label4.Name = "label4";
      this.label4.Size = new Size(59, 14);
      this.label4.TabIndex = 19;
      this.label4.Text = "Accepted";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(392, 71);
      this.label6.Name = "label6";
      this.label6.Size = new Size(110, 14);
      this.label6.TabIndex = 18;
      this.label6.Text = "eConsent Request";
      this.label6.TextAlign = ContentAlignment.MiddleLeft;
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(14, 88);
      this.label9.Name = "label9";
      this.label9.Size = new Size(33, 14);
      this.label9.TabIndex = 63;
      this.label9.Text = "Type";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(14, 72);
      this.label10.Name = "label10";
      this.label10.Size = new Size(58, 14);
      this.label10.TabIndex = 62;
      this.label10.Text = "Recipient";
      this.label10.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(345, 90);
      this.label3.Name = "label3";
      this.label3.Size = new Size(32, 14);
      this.label3.TabIndex = 18;
      this.label3.Text = "Sent";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(242, 90);
      this.label2.Name = "label2";
      this.label2.Size = new Size(36, 14);
      this.label2.TabIndex = 17;
      this.label2.Text = "Email";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(154, 90);
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
      this.dateAcceptBy.Location = new Point(489, 545);
      this.dateAcceptBy.Name = "dateAcceptBy";
      this.dateAcceptBy.Size = new Size(100, 20);
      this.dateAcceptBy.TabIndex = 13;
      this.dateAcceptBy.Visible = false;
      this.lblNotifyUserCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblNotifyUserCount.AutoSize = true;
      this.lblNotifyUserCount.Location = new Point(14, 552);
      this.lblNotifyUserCount.Name = "lblNotifyUserCount";
      this.lblNotifyUserCount.Size = new Size(105, 14);
      this.lblNotifyUserCount.TabIndex = 8;
      this.lblNotifyUserCount.Text = "({0} Users selected)";
      this.btnNotifyUsers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNotifyUsers.Location = new Point(12, 528);
      this.btnNotifyUsers.Name = "btnNotifyUsers";
      this.btnNotifyUsers.Size = new Size(129, 22);
      this.btnNotifyUsers.TabIndex = 5;
      this.btnNotifyUsers.Text = "Notify Additional Users";
      this.btnNotifyUsers.Click += new EventHandler(this.btnNotifyUsers_Click);
      this.cboSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSubject.FormattingEnabled = true;
      this.cboSubject.Location = new Point(88, 163);
      this.cboSubject.Name = "cboSubject";
      this.cboSubject.Size = new Size(500, 22);
      this.cboSubject.Sorted = true;
      this.cboSubject.TabIndex = 9;
      this.cboSubject.SelectedIndexChanged += new EventHandler(this.cboSubject_SelectedIndexChanged);
      this.htmlEmailControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(11, 191);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(577, 331);
      this.htmlEmailControl.TabIndex = 10;
      this.chkAcceptBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAcceptBy.Location = new Point(236, 546);
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
      this.chkReadReceipt.Location = new Point(330, 532);
      this.chkReadReceipt.Name = "chkReadReceipt";
      this.chkReadReceipt.Size = new Size(261, 18);
      this.chkReadReceipt.TabIndex = 11;
      this.chkReadReceipt.Text = "Notify me when borrower receives the package.";
      this.chkReadReceipt.UseVisualStyleBackColor = true;
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(8, 167);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(50, 14);
      this.lblSubject.TabIndex = 8;
      this.lblSubject.Text = "* Subject";
      this.lblSubject.TextAlign = ContentAlignment.MiddleLeft;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Send Consent Request";
      this.helpLink.Location = new Point(12, 604);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 5;
      this.helpLink.TabStop = false;
      this.txtSenderEmail.Location = new Point(410, 49);
      this.txtSenderEmail.Name = "txtSenderEmail";
      this.txtSenderEmail.ReadOnly = true;
      this.txtSenderEmail.Size = new Size(175, 20);
      this.txtSenderEmail.TabIndex = 61;
      this.txtSenderName.Location = new Point(225, 49);
      this.txtSenderName.Name = "txtSenderName";
      this.txtSenderName.ReadOnly = true;
      this.txtSenderName.Size = new Size(175, 20);
      this.txtSenderName.TabIndex = 60;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(407, 33);
      this.label7.Name = "label7";
      this.label7.Size = new Size(37, 13);
      this.label7.TabIndex = 59;
      this.label7.Text = "Email";
      this.label7.TextAlign = ContentAlignment.MiddleLeft;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(222, 33);
      this.label8.Name = "label8";
      this.label8.Size = new Size(39, 13);
      this.label8.TabIndex = 58;
      this.label8.Text = "Name";
      this.label8.TextAlign = ContentAlignment.MiddleLeft;
      this.cboSenderType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSenderType.FormattingEnabled = true;
      this.cboSenderType.Location = new Point(12, 49);
      this.cboSenderType.Name = "cboSenderType";
      this.cboSenderType.Size = new Size(203, 22);
      this.cboSenderType.TabIndex = 57;
      this.cboSenderType.SelectedIndexChanged += new EventHandler(this.cboSenderType_SelectedIndexChanged);
      this.lblSenderType.AutoSize = true;
      this.lblSenderType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblSenderType.Location = new Point(13, 35);
      this.lblSenderType.Name = "lblSenderType";
      this.lblSenderType.Size = new Size(76, 14);
      this.lblSenderType.TabIndex = 56;
      this.lblSenderType.Text = "Sender Type";
      this.lblSenderType.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScroll = true;
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(619, 634);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.gcMessage);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSend);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SendConsentRequestDialog);
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
