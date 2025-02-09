// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.StatusOnline.OTPSendEmailDialogCC
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.eFolder.eDelivery.ePass;
using EllieMae.EMLite.eFolder.eDelivery.HelperMethods;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.StatusOnline
{
  public class OTPSendEmailDialogCC : Form
  {
    private LoanDataMgr loanDataMgr;
    private LoanData loan;
    private StatusOnlineTrigger statusTrigger;
    private List<eDeliveryRecipient> Recipients;
    private List<ContactDetails> _contacts;
    private List<EllieMae.EMLite.eFolder.eDelivery.Party> parties;
    private List<PartyResponse> partiesResponse;
    private string packageGUID;
    private const string CURRENT_USER = "Current User";
    private const string FILE_STARTER = "File Starter";
    private const string LOAN_OFFICER = "Loan Officer";
    private const string OWNER = "Other";
    private IContainer components;
    private Button btnSend;
    private Button btnSkip;
    private GroupContainer gcMessage;
    private Label label9;
    private Label label8;
    private TextBox txtSenderEmail;
    private TextBox txtSenderName;
    private Label label1;
    private Label label2;
    private FlowLayoutPanel pnlRecipients;
    private Label label13;
    private Label label14;
    private ComboBox cboSubject;
    private ComboBox cboSenderType;
    private HtmlEmailControl htmlEmailControl;
    private Label label15;
    private Label label16;
    private Label label11;
    private Label label12;
    private RecipientControl recipientControl1;
    private StandardIconButton btnAddRecipient;

    public OTPSendEmailDialogCC(LoanDataMgr loanDataMgr, StatusOnlineTrigger statusTrigger)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.loan = loanDataMgr.LoanData;
      this.statusTrigger = statusTrigger;
      this.initContents();
    }

    public OTPSendEmailDialogCC(
      LoanDataMgr loanDataMgr,
      string packageGUID,
      StatusOnlineTrigger statusTrigger)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.loan = loanDataMgr.LoanData;
      this.packageGUID = packageGUID;
      this.statusTrigger = statusTrigger;
      this.initContents();
    }

    private void initContents()
    {
      this.initHtmlControl();
      this.initSender();
      this.initRecipients();
      this.initSelectedEmailTemplate();
    }

    private void initHtmlControl()
    {
      this.htmlEmailControl.LoadText(string.Empty, false);
      this.htmlEmailControl.ShowFieldButton = false;
      this.htmlEmailControl.AllowPersonalImages = Session.UserInfo.PersonalStatusOnline;
    }

    private void initSender()
    {
      string userid1 = string.Empty;
      string email1 = string.Empty;
      string name1 = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, TriggerEmailFromType.CurrentUser, this.statusTrigger.OwnerID, out userid1, out email1, out name1);
      if (!string.IsNullOrEmpty(userid1) && !string.IsNullOrEmpty(email1))
        this.cboSenderType.Items.Add((object) new FieldOption("Current User", "Current User", userid1));
      string userid2 = string.Empty;
      string email2 = string.Empty;
      string name2 = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, TriggerEmailFromType.FileStarter, this.statusTrigger.OwnerID, out userid2, out email2, out name2);
      this.cboSenderType.Items.Add((object) new FieldOption("File Starter", "File Starter", userid2));
      string userid3 = string.Empty;
      string email3 = string.Empty;
      string name3 = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, TriggerEmailFromType.LoanOfficer, this.statusTrigger.OwnerID, out userid3, out email3, out name3);
      if (!string.IsNullOrEmpty(userid3) && !string.IsNullOrEmpty(email3))
        this.cboSenderType.Items.Add((object) new FieldOption("Loan Officer", "Loan Officer", userid3));
      this.cboSenderType.SelectedIndex = this.getSenderSelection();
      this.setSenderInfo();
    }

    private int getSenderSelection()
    {
      if (this.cboSenderType.Items.Count == 0)
        return -1;
      for (int index = 0; index < this.cboSenderType.Items.Count; ++index)
      {
        FieldOption fieldOption = (FieldOption) this.cboSenderType.Items[index];
        string str = string.Empty;
        switch (this.statusTrigger.EmailFromType)
        {
          case TriggerEmailFromType.CurrentUser:
            str = "Current User";
            break;
          case TriggerEmailFromType.LoanOfficer:
            str = "Loan Officer";
            break;
          case TriggerEmailFromType.FileStarter:
            str = "File Starter";
            break;
          case TriggerEmailFromType.Owner:
            str = "Other";
            break;
        }
        if (fieldOption.Value == str)
          return index;
      }
      return 0;
    }

    private void initRecipients()
    {
      FileContacts fileContact = SendPackageFactory.CreateFileContact(this.loan);
      Task<List<ContactDetails>> loanContacts = new EBSServiceClient().GetLoanContacts(this.loan.GetField("GUID"));
      Task.WaitAll((Task) loanContacts);
      this._contacts = loanContacts.Result == null ? new List<ContactDetails>() : loanContacts.Result;
      while (this.pnlRecipients.Controls.Count > 0)
      {
        Control control = this.pnlRecipients.Controls[0];
        this.pnlRecipients.Controls.Remove(control);
        control.Dispose();
      }
      int pairIndex = this.loan.GetPairIndex(this.loan.PairId);
      if (((IEnumerable<string>) this.statusTrigger.EmailRecipients).Contains<string>("1240"))
        this.addRecipient(new eDeliveryRecipient("1", eDeliveryEntityType.Borrower)
        {
          BorrowerId = Session.LoanDataMgr.LoanData.CurrentBorrowerPair.Borrower.Id,
          FirstName = this.loan.GetSimpleField("4000"),
          MiddleName = this.loan.GetSimpleField("4001"),
          LastName = this.loan.GetSimpleField("4002"),
          SuffixName = this.loan.GetSimpleField("4003"),
          UnparsedName = this.loan.GetSimpleField("1868"),
          EmailAddress = this.loan.GetSimpleField("1240"),
          SignatureField = "BorrowerSignature_" + pairIndex.ToString(),
          InitialsField = "BorrowerInitials_" + pairIndex.ToString(),
          CheckboxField = "BorrowerCheckbox_" + pairIndex.ToString(),
          RecipientType = "Borrower",
          CellPhoneNumber = this.loan.GetSimpleField("1490"),
          HomePhoneNumber = this.loan.GetSimpleField("66"),
          WorkPhoneNumber = this.loan.GetSimpleField("FE0117")
        });
      if (((IEnumerable<string>) this.statusTrigger.EmailRecipients).Contains<string>("1268"))
      {
        string simpleField1 = this.loan.GetSimpleField("4004");
        string simpleField2 = this.loan.GetSimpleField("1268");
        this.addRecipient(new eDeliveryRecipient("2", eDeliveryEntityType.Coborrower)
        {
          BorrowerId = Session.LoanDataMgr.LoanData.CurrentBorrowerPair.CoBorrower.Id,
          FirstName = simpleField1,
          MiddleName = this.loan.GetSimpleField("4005"),
          LastName = this.loan.GetSimpleField("4006"),
          SuffixName = this.loan.GetSimpleField("4007"),
          UnparsedName = this.loan.GetSimpleField("1873"),
          EmailAddress = simpleField2,
          SignatureField = "CoborrowerSignature_" + pairIndex.ToString(),
          InitialsField = "CoborrowerInitials_" + pairIndex.ToString(),
          CheckboxField = "CoborrowerCheckbox_" + pairIndex.ToString(),
          RecipientType = "Coborrower",
          CellPhoneNumber = this.loan.GetSimpleField("1480"),
          HomePhoneNumber = this.loan.GetSimpleField("98"),
          WorkPhoneNumber = this.loan.GetSimpleField("FE0217")
        });
      }
      if (((IEnumerable<string>) this.statusTrigger.EmailRecipients).Contains<string>("VEND.X141"))
      {
        ContactDetails contactDetails1 = this._contacts.Find((Predicate<ContactDetails>) (x => x.contactType == "Buyer's Agent"));
        eDeliveryRecipient recipient = new eDeliveryRecipient("3", eDeliveryEntityType.Other);
        if (contactDetails1 == null || string.IsNullOrEmpty(contactDetails1.name) || string.IsNullOrEmpty(contactDetails1.email))
        {
          recipient.UnparsedName = this.loan.GetField("VEND.X139");
          recipient.EmailAddress = this.loan.GetField("VEND.X141");
        }
        else
        {
          recipient.UnparsedName = contactDetails1.name;
          recipient.EmailAddress = contactDetails1.email;
          recipient.RecipientId = contactDetails1.recipientId;
        }
        string[] strArray = Utils.SplitName(recipient.UnparsedName);
        recipient.FirstName = strArray[0];
        recipient.LastName = strArray[1];
        recipient.RecipientType = "Buyer's Agent";
        Dictionary<string, string> contactDetails2 = fileContact.GetContactDetails(recipient.RecipientType);
        if (contactDetails2 != null && contactDetails2.ContainsKey("CellPhoneNumber") && !string.IsNullOrEmpty(contactDetails2["CellPhoneNumber"]))
          recipient.CellPhoneNumber = contactDetails2["CellPhoneNumber"];
        if (contactDetails2 != null && contactDetails2.ContainsKey("HomePhoneNumber") && !string.IsNullOrEmpty(contactDetails2["HomePhoneNumber"]))
          recipient.HomePhoneNumber = contactDetails2["HomePhoneNumber"];
        if (contactDetails2 != null && contactDetails2.ContainsKey("WorkPhoneNumber") && !string.IsNullOrEmpty(contactDetails2["WorkPhoneNumber"]))
          recipient.WorkPhoneNumber = contactDetails2["WorkPhoneNumber"];
        this.addRecipient(recipient);
      }
      if (!((IEnumerable<string>) this.statusTrigger.EmailRecipients).Contains<string>("NBO"))
        return;
      List<NonBorrowingOwner> byBorrowerPairId = this.loan.GetNboByBorrowerPairId(this.loan.GetBorrowerPairs()[pairIndex].Id);
      int num = 0;
      foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
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
        recipient.CellPhoneNumber = nonBorrowingOwner.CellPhoneNumber;
        recipient.HomePhoneNumber = nonBorrowingOwner.HomePhoneNumber;
        recipient.WorkPhoneNumber = nonBorrowingOwner.BusinessPhoneNumber;
        recipient.NboIndex = nonBorrowingOwner.NboIndex;
        ++num;
        this.addRecipient(recipient);
      }
    }

    private void addRecipient(eDeliveryRecipient recipient)
    {
      if (recipient != null)
      {
        if (this._contacts != null)
        {
          ContactDetails contactDetails = recipient.RecipientType == eDeliveryEntityType.Borrower.ToString("g") || recipient.RecipientType == eDeliveryEntityType.Coborrower.ToString("g") ? this._contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == recipient.BorrowerId)) : (!(recipient.RecipientType.ToUpper() == eDeliveryEntityType.NonBorrowingOwner.ToString("g").ToUpper()) ? this._contacts.Find((Predicate<ContactDetails>) (x => x.contactType.ToUpper() == recipient.RecipientType.ToUpper() && x.name == (!string.IsNullOrEmpty(recipient.UnparsedName) ? recipient.UnparsedName : recipient.FirstName + " " + recipient.LastName) && x.email.ToUpper() == recipient.EmailAddress.ToUpper())) : this._contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == recipient.BorrowerId)));
          if (contactDetails != null)
            recipient.RecipientId = contactDetails.recipientId;
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
            deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientType.ToUpper() == recipient.RecipientType.ToUpper() && x.UnparsedName == (!string.IsNullOrEmpty(recipient.UnparsedName) ? recipient.UnparsedName : recipient.FirstName + " " + recipient.LastName) && x.EmailAddress.ToUpper() == recipient.EmailAddress.ToUpper()));
          recipient.RecipientId = deliveryRecipient == null ? (recipient.RecipientId == null ? Guid.NewGuid().ToString() : recipient.RecipientId) : deliveryRecipient.RecipientId;
        }
      }
      if (this.Recipients == null)
        this.Recipients = new List<eDeliveryRecipient>();
      if (!this.Recipients.Exists((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipient.RecipientId)))
        this.Recipients.Add(recipient);
      if (((IEnumerable<Control>) this.pnlRecipients.Controls.Find(recipient.RecipientId, true)).FirstOrDefault<Control>() != null)
        return;
      OTPRecipientControl recipientControl = new OTPRecipientControl();
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
      SortedDictionary<int, string> dataSource = new SortedDictionary<int, string>();
      dataSource.Add(1000, "----Enter new number----");
      if (!string.IsNullOrEmpty(recipient.CellPhoneNumber) && this.IsValidPhoneNumber(recipient.CellPhoneNumber))
        dataSource.Add(1, "M: " + recipient.CellPhoneNumber);
      if (!string.IsNullOrEmpty(recipient.HomePhoneNumber) && this.IsValidPhoneNumber(recipient.HomePhoneNumber))
        dataSource.Add(2, "H: " + recipient.HomePhoneNumber);
      if (!string.IsNullOrEmpty(recipient.WorkPhoneNumber) && this.IsValidPhoneNumber(recipient.WorkPhoneNumber))
        dataSource.Add(3, "W: " + recipient.WorkPhoneNumber);
      if (dataSource.Count > 1)
      {
        recipientControl.ddPhoneNumber.DataSource = (object) new BindingSource((object) dataSource, (string) null);
        recipientControl.ddPhoneNumber.DisplayMember = "Value";
        recipientControl.ddPhoneNumber.ValueMember = "Key";
        recipientControl.ddPhoneNumber.DropDownStyle = ComboBoxStyle.DropDownList;
        recipientControl.ddPhoneNumber.BackColor = Color.White;
      }
      else
        recipientControl.ddPhoneNumber.DropDownStyle = ComboBoxStyle.DropDown;
      recipientControl.cbSelect.Text = recipient.RecipientType == eDeliveryEntityType.NonBorrowingOwner.ToString("g") ? "Non-Borrowing Owner" : recipient.RecipientType;
      recipientControl.cbSelect.Checked = true;
      recipientControl.Tag = (object) recipient.NboIndex;
      this.pnlRecipients.Controls.Add((Control) recipientControl);
    }

    private void initSelectedEmailTemplate()
    {
      string emailTemplate = this.statusTrigger.EmailTemplate;
      if (string.IsNullOrEmpty(emailTemplate))
        return;
      HtmlEmailTemplate htmlEmailTemplate = Session.ConfigurationManager.GetHtmlEmailTemplate(this.statusTrigger.EmailTemplateOwner, emailTemplate);
      if (!(htmlEmailTemplate != (HtmlEmailTemplate) null) || this.cboSubject.Items.Contains((object) htmlEmailTemplate))
        return;
      this.cboSubject.SelectedIndex = this.cboSubject.Items.Add((object) htmlEmailTemplate);
      this.populateHtmlEmail();
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
      return phoneNumber.Trim().Where<char>((Func<char, bool>) (x => char.IsDigit(x))).Count<char>() == 10;
    }

    private bool MatchPhoneFormat(string phoneNumber)
    {
      return (phoneNumber.Length <= 1 || !phoneNumber.StartsWith("(") && !phoneNumber.StartsWith(")") || !(phoneNumber[1].ToString() == "(") && !(phoneNumber[1].ToString() == ")")) && (!phoneNumber.Contains("(") || phoneNumber.Contains(")")) && (!phoneNumber.Contains(")") || phoneNumber.Contains("(")) && new Regex("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", RegexOptions.IgnoreCase | RegexOptions.Compiled).IsMatch(phoneNumber);
    }

    private void populateHtmlEmail()
    {
      UserInfo userInfo = (UserInfo) null;
      if ((object) (this.cboSubject.SelectedItem as HtmlEmailTemplate) == null)
        return;
      HtmlEmailTemplate selectedItem1 = (HtmlEmailTemplate) this.cboSubject.SelectedItem;
      if (this.cboSenderType.SelectedItem != null)
      {
        FieldOption selectedItem2 = (FieldOption) this.cboSenderType.SelectedItem;
        if (selectedItem2 != null)
          userInfo = !(selectedItem2.Value == "Current User") ? Session.OrganizationManager.GetUser(selectedItem2.ReportingDatabaseValue) : Session.UserInfo;
      }
      string html = new HtmlFieldMerge(selectedItem1.Html).MergeContent(this.loanDataMgr.LoanData, userInfo);
      if (string.IsNullOrEmpty(html))
        return;
      this.htmlEmailControl.LoadHtml(html, false);
    }

    private void setSenderInfo()
    {
      string str = (this.cboSenderType.SelectedItem as FieldOption).Value;
      TriggerEmailFromType from = TriggerEmailFromType.CurrentUser;
      switch (str)
      {
        case "Current User":
          from = TriggerEmailFromType.CurrentUser;
          break;
        case "File Starter":
          from = TriggerEmailFromType.FileStarter;
          break;
        case "Loan Officer":
          from = TriggerEmailFromType.LoanOfficer;
          break;
      }
      this.txtSenderEmail.ReadOnly = true;
      this.txtSenderName.ReadOnly = true;
      this.txtSenderEmail.Text = string.Empty;
      this.txtSenderName.Text = string.Empty;
      string userid = string.Empty;
      string email = string.Empty;
      string name = string.Empty;
      StatusOnlineManager.GetTriggerSender(this.loanDataMgr, from, this.statusTrigger.OwnerID, out userid, out email, out name);
      if (!string.IsNullOrEmpty(email))
        this.txtSenderEmail.Text = email;
      else
        this.txtSenderEmail.ReadOnly = false;
      if (!string.IsNullOrEmpty(name))
        this.txtSenderName.Text = name;
      else
        this.txtSenderName.ReadOnly = false;
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
      contactDetails.Add("SenderType", (this.cboSenderType.SelectedItem as FieldOption).Value);
      this.saveFromUser(this.loanDataMgr, contactDetails);
    }

    private void saveFromUser(LoanDataMgr loanDataMgr, Dictionary<string, string> contactDetails)
    {
      switch (contactDetails["SenderType"])
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
          string field = loanDataMgr.LoanData.GetField("LOID");
          if (string.IsNullOrEmpty(field))
            break;
          UserInfo user = Session.OrganizationManager.GetUser(field);
          if (!(user != (UserInfo) null))
            break;
          if (contactDetails.ContainsKey("Email"))
            user.Email = contactDetails["Email"];
          if (contactDetails.ContainsKey("Name"))
          {
            string[] source = contactDetails["Name"].Split(' ');
            string str = string.Empty;
            if (((IEnumerable<string>) source).Count<string>() > 1)
              str = ((IEnumerable<string>) source).ToList<string>().LastOrDefault<string>();
            user.FirstName = contactDetails["Name"].Replace(" " + str, string.Empty);
            if (!string.IsNullOrEmpty(str))
              user.LastName = str;
          }
          Session.OrganizationManager.UpdateUser(user);
          break;
      }
    }

    private string saveRecipients()
    {
      string str1 = string.Empty;
      List<string> stringList = new List<string>();
      this.parties = new List<EllieMae.EMLite.eFolder.eDelivery.Party>();
      this.partiesResponse = new List<PartyResponse>();
      string htmlBodyText = this.htmlEmailControl.HtmlBodyText;
      foreach (Control control in (ArrangedElementCollection) this.pnlRecipients.Controls)
      {
        if (control != null && control.GetType() == typeof (OTPRecipientControl))
        {
          OTPRecipientControl recipientcontrol = control as OTPRecipientControl;
          if (recipientcontrol.cbSelect.Checked)
          {
            if (string.IsNullOrWhiteSpace(recipientcontrol.txtName.Text))
              stringList.Add(recipientcontrol.cbSelect.Text + " Name cannot be blank.");
            if (string.IsNullOrWhiteSpace(recipientcontrol.txtEmail.Text))
              stringList.Add(recipientcontrol.cbSelect.Text + " Email cannot be blank.");
            else if (!Utils.ValidateEmail(recipientcontrol.txtEmail.Text))
              stringList.Add(recipientcontrol.cbSelect.Text + " Email is invalid.");
            if (string.IsNullOrWhiteSpace(recipientcontrol.ddPhoneNumber.Text))
              stringList.Add(recipientcontrol.cbSelect.Text + "'s phone number is required.");
            else if (!this.IsValidPhoneNumber(this.RemovePhoneNumberPrefix(recipientcontrol.ddPhoneNumber.Text)) || !this.MatchPhoneFormat(this.RemovePhoneNumberPrefix(recipientcontrol.ddPhoneNumber.Text)))
              stringList.Add(recipientcontrol.cbSelect.Text + "'s phone number must be a 10 - digit phone number with XXXXXXXXXX / (XXX)XXX-XXXX / (XXX)-XXX-XXXX / XXX-XXX-XXXX format.");
            if (stringList.Count <= 0)
            {
              Dictionary<string, string> contactDetails = new Dictionary<string, string>();
              if (!recipientcontrol.txtName.ReadOnly)
                contactDetails.Add("Name", recipientcontrol.txtName.Text);
              if (!recipientcontrol.txtEmail.ReadOnly)
                contactDetails.Add("Email", recipientcontrol.txtEmail.Text);
              if (recipientcontrol.cbSelect.Text == "Non-Borrowing Owner" && (!recipientcontrol.txtName.ReadOnly || !recipientcontrol.txtEmail.ReadOnly))
                contactDetails.Add("NboIndex", recipientcontrol.Tag.ToString());
              eDeliveryRecipient recipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name));
              if (!string.IsNullOrEmpty(recipientcontrol.ddPhoneNumber.Text))
              {
                string str2 = recipientcontrol.ddPhoneNumber.Items.Count <= 0 || string.IsNullOrEmpty(recipientcontrol.ddPhoneNumber.SelectedValue?.ToString()) || !(recipientcontrol.ddPhoneNumber.SelectedValue.ToString().ToLower() == "2") ? "Mobile" : "Home";
                recipient.PhoneNumber = this.RemovePhoneNumberPrefix(recipientcontrol.ddPhoneNumber.Text);
                recipient.PhoneType = (EllieMae.EMLite.eFolder.eDelivery.PhoneType) Enum.Parse(typeof (EllieMae.EMLite.eFolder.eDelivery.PhoneType), str2);
              }
              recipient.PartyId = recipient.RecipientId;
              if (contactDetails.Count > 0)
              {
                contactDetails.Add("ContactType", recipientcontrol.cbSelect.Text);
                if (contactDetails.ContainsKey("Email"))
                  recipient.EmailAddress = contactDetails["Email"];
                if (contactDetails.ContainsKey("Name"))
                  recipient.UnparsedName = contactDetails["Name"];
                contactDetails.Add("BorrowerId", recipient.BorrowerId);
                SendPackageFactory.CreateFileContact(this.loan).UpdateContactDetails(contactDetails);
              }
              this.parties.Add(this.CreatePartyForDMOSRequest(recipient));
            }
          }
          else
            this.Recipients.Remove(this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name)));
        }
      }
      if (stringList.Count > 0)
      {
        for (int index = 0; index < stringList.Count; ++index)
          str1 = str1 + "\n\n(" + Convert.ToString(index + 1) + ") " + stringList[index];
        return str1;
      }
      if (!this.loanDataMgr.Save())
        return "Unable to save the loan.";
      if (this.parties.Count > 0)
      {
        Task<GetDMOSRecipientURLResponse> recipientUrl = new DMOSServiceClient().GetRecipientURL(new GetDMOSRecipientURLRequest()
        {
          loanGuid = this.loanDataMgr.LoanData.GetField("GUID").Replace("{", string.Empty).Replace("}", string.Empty),
          parties = this.parties.ToArray(),
          caller = new Caller()
          {
            name = "Encompass",
            version = VersionInformation.CurrentVersion.DisplayVersion,
            clientId = Session.CompanyInfo.ClientID
          }
        });
        Task.WaitAll((Task) recipientUrl);
        GetDMOSRecipientURLResponse result = recipientUrl.Result;
        this.loanDataMgr.refreshLoanFromServer();
        if (result != null && result.parties != null)
          this.partiesResponse = ((IEnumerable<PartyResponse>) result.parties).ToList<PartyResponse>();
      }
      return str1;
    }

    private EllieMae.EMLite.eFolder.eDelivery.Party CreatePartyForDMOSRequest(
      eDeliveryRecipient recipient)
    {
      EllieMae.EMLite.eFolder.eDelivery.Party party = new EllieMae.EMLite.eFolder.eDelivery.Party();
      party.id = recipient.PartyId;
      party.contact = new DMOSContact()
      {
        emailAddress = recipient.EmailAddress,
        phoneNumber = recipient.PhoneNumber,
        phoneType = recipient.PhoneType.ToString()
      };
      party.fullName = recipient.UnparsedName;
      DMOSRequestHelper.SetPartyEntityType(party, recipient.RecipientType, recipient.BorrowerId, recipient.UserID);
      return party;
    }

    private void cboSenderType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setSenderInfo();
    }

    private void btnTo_Click(object sender, EventArgs e)
    {
      FileContacts fileContact = SendPackageFactory.CreateFileContact(this.loan);
      using (OTPEmailListDailog otpEmailListDailog = new OTPEmailListDailog(this.loanDataMgr, this._contacts, true))
      {
        DialogResult dialogResult = otpEmailListDailog.ShowDialog((IWin32Window) this);
        if (otpEmailListDailog.contacts.Exists((Predicate<string>) (x => ((IEnumerable<string>) x.Split(';')).Last<string>() == "1")))
        {
          Task<List<ContactDetails>> loanContacts = new EBSServiceClient().GetLoanContacts(this.loan.GetField("GUID"));
          Task.WaitAll((Task) loanContacts);
          this._contacts = loanContacts.Result;
        }
        if (dialogResult == DialogResult.Cancel)
          return;
        foreach (string selectedContact in otpEmailListDailog.selectedContacts)
        {
          char[] chArray = new char[1]{ ';' };
          string[] strArray = selectedContact.Split(chArray);
          eDeliveryRecipient recipient;
          if (strArray[2] == "Borrower")
          {
            recipient = new eDeliveryRecipient("1", eDeliveryEntityType.Borrower)
            {
              BorrowerId = strArray[3]
            };
            recipient.BorrowerId = strArray[6];
            recipient.CellPhoneNumber = strArray[3];
            recipient.HomePhoneNumber = strArray[4];
            recipient.WorkPhoneNumber = strArray[5];
          }
          else if (strArray[2] == "Coborrower")
          {
            recipient = new eDeliveryRecipient("2", eDeliveryEntityType.Coborrower)
            {
              BorrowerId = strArray[3]
            };
            recipient.BorrowerId = strArray[6];
            recipient.CellPhoneNumber = strArray[3];
            recipient.HomePhoneNumber = strArray[4];
            recipient.WorkPhoneNumber = strArray[5];
          }
          else
            recipient = new eDeliveryRecipient("0", eDeliveryEntityType.Other);
          recipient.UnparsedName = strArray[0];
          recipient.EmailAddress = strArray[1];
          recipient.RecipientType = strArray[2];
          Dictionary<string, string> contactDetails = fileContact.GetContactDetails(recipient.RecipientType);
          if (contactDetails != null && contactDetails.ContainsKey("CellPhoneNumber") && !string.IsNullOrEmpty(contactDetails["CellPhoneNumber"]))
            recipient.CellPhoneNumber = contactDetails["CellPhoneNumber"];
          if (contactDetails != null && contactDetails.ContainsKey("HomePhoneNumber") && !string.IsNullOrEmpty(contactDetails["HomePhoneNumber"]))
            recipient.HomePhoneNumber = contactDetails["HomePhoneNumber"];
          if (contactDetails != null && contactDetails.ContainsKey("WorkPhoneNumber") && !string.IsNullOrEmpty(contactDetails["WorkPhoneNumber"]))
            recipient.WorkPhoneNumber = contactDetails["WorkPhoneNumber"];
          this.addRecipient(recipient);
        }
        int num = 0;
        foreach (NonBorrowingOwner nonBorrowingOwner in otpEmailListDailog.selectedNbo)
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
          recipient.CellPhoneNumber = nonBorrowingOwner.CellPhoneNumber;
          recipient.HomePhoneNumber = nonBorrowingOwner.HomePhoneNumber;
          recipient.WorkPhoneNumber = nonBorrowingOwner.BusinessPhoneNumber;
          recipient.NboIndex = nonBorrowingOwner.NboIndex;
          ++num;
          this.addRecipient(recipient);
        }
      }
    }

    private void cboSubject_DropDown(object sender, EventArgs e)
    {
      foreach (HtmlEmailTemplate htmlEmailTemplate in Session.ConfigurationManager.GetHtmlEmailTemplates((string) null, HtmlEmailTemplateType.ConsumerConnectStatusOnline))
      {
        if (!this.cboSubject.Items.Contains((object) htmlEmailTemplate))
          this.cboSubject.Items.Add((object) htmlEmailTemplate);
      }
      FieldOption selectedItem = (FieldOption) this.cboSenderType.SelectedItem;
      if (selectedItem != null)
      {
        string reportingDatabaseValue = selectedItem.ReportingDatabaseValue;
        if (!string.IsNullOrEmpty(reportingDatabaseValue))
        {
          foreach (HtmlEmailTemplate htmlEmailTemplate in Session.ConfigurationManager.GetHtmlEmailTemplates(reportingDatabaseValue, HtmlEmailTemplateType.ConsumerConnectStatusOnline))
          {
            if (!this.cboSubject.Items.Contains((object) htmlEmailTemplate))
              this.cboSubject.Items.Add((object) htmlEmailTemplate);
          }
        }
      }
      this.cboSubject.DropDown -= new EventHandler(this.cboSubject_DropDown);
    }

    private void cboSubject_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.populateHtmlEmail();
    }

    private void cboFrom_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.populateHtmlEmail();
      this.cboSubject.DropDown -= new EventHandler(this.cboSubject_DropDown);
    }

    private void btnSkip_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnSend_Click(object sender, EventArgs e)
    {
      string empty1 = string.Empty;
      string text;
      if (!string.IsNullOrEmpty(text = this.validateSenderContactDetails()))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.saveSenderContactDetails();
        string str1 = this.saveRecipients();
        if (str1 != string.Empty)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You must address the following issues before you can send the email:" + str1, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          string str2 = string.Empty;
          Dictionary<string, string> dictionary = new Dictionary<string, string>();
          int num3 = 0;
          foreach (Control control in (ArrangedElementCollection) this.pnlRecipients.Controls)
          {
            if (control is OTPRecipientControl recipientControl)
            {
              if (recipientControl.cbSelect.Checked)
              {
                str2 = str2 + recipientControl.txtEmail.Text.Trim() + ";";
                dictionary.Add(recipientControl.Name, recipientControl.cbSelect.Text + ";" + recipientControl.txtEmail.Text + ";" + recipientControl.txtName.Text);
              }
              ++num3;
            }
          }
          if (string.IsNullOrEmpty(str2))
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one valid recipient before you can send the email.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            string str3 = str2.Substring(0, str2.Length - 1);
            EBSServiceClient ebsServiceClient = new EBSServiceClient();
            sendNotificationRequest request = new sendNotificationRequest()
            {
              loanGuid = new Guid(this.loan.GUID.Replace("{", string.Empty).Replace("}", string.Empty)),
              contentType = "HTML",
              createdBy = this.txtSenderName.Text,
              createdDate = DateTime.Now.ToString(),
              senderFullName = this.txtSenderName.Text,
              replyTo = this.txtSenderEmail.Text
            };
            foreach (EllieMae.EMLite.eFolder.eDelivery.Party party in this.parties)
            {
              EllieMae.EMLite.eFolder.eDelivery.Party item = party;
              if (item.loanEntity.entityType.IndexOf("Borrower", StringComparison.OrdinalIgnoreCase) > -1 || item.loanEntity.entityType.IndexOf("Coborrower", StringComparison.OrdinalIgnoreCase) > -1)
              {
                string empty2 = string.Empty;
                request.emails = new string[1]
                {
                  item.contact.emailAddress
                };
                string fullName = item.fullName;
                request.subject = fullName + ":" + this.cboSubject.Text;
                request.body = HtmlFieldMerge.MergeDynamicConsumerConnectContent(this.htmlEmailControl.Html, this.partiesResponse.Where<PartyResponse>((Func<PartyResponse, bool>) (x => x.id == item.id)).FirstOrDefault<PartyResponse>()?.url, fullName);
                try
                {
                  Task task = ebsServiceClient.SendNotification(request);
                  Task.WaitAll(task);
                  if (task.Status == TaskStatus.RanToCompletion)
                    this.createLogEntry(new RecipientInfo()
                    {
                      from = request.replyTo,
                      to = request.emails[0],
                      subject = request.subject,
                      body = request.body
                    });
                }
                catch
                {
                }
              }
            }
            IEnumerable<KeyValuePair<string, string>> second = dictionary.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Value.Contains("Borrower") || x.Value.Contains("Coborrower")));
            foreach (KeyValuePair<string, string> keyValuePair in dictionary.Except<KeyValuePair<string, string>>(second))
            {
              KeyValuePair<string, string> email = keyValuePair;
              foreach (PartyResponse partyResponse in this.partiesResponse.FindAll((Predicate<PartyResponse>) (x => x.id == email.Key)))
              {
                string[] strArray = email.Value.Split(';');
                request.emails = new string[1]
                {
                  strArray[1]
                };
                request.subject = strArray[2] + ": " + this.cboSubject.Text;
                request.body = HtmlFieldMerge.MergeDynamicConsumerConnectContent(this.htmlEmailControl.Html, partyResponse.url, strArray[2]);
                Task task = ebsServiceClient.SendNotification(request);
                Task.WaitAll(task);
                if (task.Status == TaskStatus.RanToCompletion)
                  this.createLogEntry(new RecipientInfo()
                  {
                    from = request.replyTo,
                    to = request.emails[0],
                    subject = request.subject,
                    body = request.body
                  });
              }
            }
            EmailFrom.addEmailToNotificationSettings(new Guid(this.loan.GUID), this.txtSenderEmail.Text);
            int num5 = (int) Utils.Dialog((IWin32Window) this, this.GetMessageForThirdPartyUsers("An email message has been sent to " + str3), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.DialogResult = DialogResult.OK;
          }
        }
      }
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

    private string RemovePhoneNumberPrefix(string phoneNumber)
    {
      return phoneNumber?.Replace("M: ", string.Empty).Replace("H: ", string.Empty).Replace("W: ", string.Empty).Trim();
    }

    private string GetMessageForThirdPartyUsers(string confirmationMessage)
    {
      if (this.Recipients.Any<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.EntityType == eDeliveryEntityType.Other)) && !this.ThirdpartyUsersAuthenticated())
        confirmationMessage = confirmationMessage + "\n\n" + "The authentication code(s) for 3rd party signer(s) can be viewed in the packages tab of the eFolder.";
      return confirmationMessage;
    }

    private bool ThirdpartyUsersAuthenticated()
    {
      Task<List<RecipientDetails>> recipientDetails = new EBSServiceClient().GetRecipientDetails(this.loan.GetField("GUID"));
      Task.WaitAll((Task) recipientDetails);
      List<RecipientDetails> result = recipientDetails.Result;
      return result != null && result.Count > 0 && !result.Where<RecipientDetails>((Func<RecipientDetails, bool>) (x => x.contactType.ToLower() != "borrower" && x.contactType.ToLower() != "nonborrowingowner")).Any<RecipientDetails>((Func<RecipientDetails, bool>) (y => !y.isAuthenticated));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnSend = new Button();
      this.btnSkip = new Button();
      this.gcMessage = new GroupContainer();
      this.btnAddRecipient = new StandardIconButton();
      this.label11 = new Label();
      this.label12 = new Label();
      this.label9 = new Label();
      this.label8 = new Label();
      this.txtSenderEmail = new TextBox();
      this.txtSenderName = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.pnlRecipients = new FlowLayoutPanel();
      this.recipientControl1 = new RecipientControl();
      this.label13 = new Label();
      this.label14 = new Label();
      this.cboSubject = new ComboBox();
      this.cboSenderType = new ComboBox();
      this.htmlEmailControl = new HtmlEmailControl();
      this.label15 = new Label();
      this.label16 = new Label();
      this.gcMessage.SuspendLayout();
      ((ISupportInitialize) this.btnAddRecipient).BeginInit();
      this.pnlRecipients.SuspendLayout();
      this.SuspendLayout();
      this.btnSend.Location = new Point(519, 580);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 23);
      this.btnSend.TabIndex = 8;
      this.btnSend.Text = "Send";
      this.btnSend.UseVisualStyleBackColor = true;
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.btnSkip.Location = new Point(606, 580);
      this.btnSkip.Name = "btnSkip";
      this.btnSkip.Size = new Size(89, 23);
      this.btnSkip.TabIndex = 9;
      this.btnSkip.Text = "Skip This Email";
      this.btnSkip.UseVisualStyleBackColor = true;
      this.btnSkip.Click += new EventHandler(this.btnSkip_Click);
      this.gcMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcMessage.Controls.Add((Control) this.btnAddRecipient);
      this.gcMessage.Controls.Add((Control) this.label11);
      this.gcMessage.Controls.Add((Control) this.label12);
      this.gcMessage.Controls.Add((Control) this.label9);
      this.gcMessage.Controls.Add((Control) this.label8);
      this.gcMessage.Controls.Add((Control) this.txtSenderEmail);
      this.gcMessage.Controls.Add((Control) this.txtSenderName);
      this.gcMessage.Controls.Add((Control) this.label1);
      this.gcMessage.Controls.Add((Control) this.label2);
      this.gcMessage.Controls.Add((Control) this.pnlRecipients);
      this.gcMessage.Controls.Add((Control) this.label13);
      this.gcMessage.Controls.Add((Control) this.label14);
      this.gcMessage.Controls.Add((Control) this.cboSubject);
      this.gcMessage.Controls.Add((Control) this.cboSenderType);
      this.gcMessage.Controls.Add((Control) this.htmlEmailControl);
      this.gcMessage.Controls.Add((Control) this.label15);
      this.gcMessage.Controls.Add((Control) this.label16);
      this.gcMessage.HeaderForeColor = SystemColors.ControlText;
      this.gcMessage.Location = new Point(12, 12);
      this.gcMessage.Name = "gcMessage";
      this.gcMessage.Size = new Size(683, 559);
      this.gcMessage.TabIndex = 61;
      this.gcMessage.Text = "Message";
      this.btnAddRecipient.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddRecipient.BackColor = Color.Transparent;
      this.btnAddRecipient.Location = new Point(640, 95);
      this.btnAddRecipient.Margin = new Padding(4, 3, 0, 3);
      this.btnAddRecipient.MouseDownImage = (Image) null;
      this.btnAddRecipient.Name = "btnAddRecipient";
      this.btnAddRecipient.Size = new Size(21, 16);
      this.btnAddRecipient.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddRecipient.TabIndex = 61;
      this.btnAddRecipient.TabStop = false;
      this.btnAddRecipient.Click += new EventHandler(this.btnTo_Click);
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(464, 100);
      this.label11.Name = "label11";
      this.label11.Size = new Size(51, 14);
      this.label11.TabIndex = 60;
      this.label11.Text = "Number";
      this.label11.TextAlign = ContentAlignment.MiddleLeft;
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(464, 84);
      this.label12.Name = "label12";
      this.label12.Size = new Size(42, 14);
      this.label12.TabIndex = 59;
      this.label12.Text = "Phone";
      this.label12.TextAlign = ContentAlignment.MiddleLeft;
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(14, 100);
      this.label9.Name = "label9";
      this.label9.Size = new Size(33, 14);
      this.label9.TabIndex = 57;
      this.label9.Text = "Type";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(14, 84);
      this.label8.Name = "label8";
      this.label8.Size = new Size(58, 14);
      this.label8.TabIndex = 56;
      this.label8.Text = "Recipient";
      this.label8.TextAlign = ContentAlignment.MiddleLeft;
      this.txtSenderEmail.Location = new Point(392, 50);
      this.txtSenderEmail.Name = "txtSenderEmail";
      this.txtSenderEmail.ReadOnly = true;
      this.txtSenderEmail.Size = new Size(278, 20);
      this.txtSenderEmail.TabIndex = 55;
      this.txtSenderName.Location = new Point(165, 50);
      this.txtSenderName.Name = "txtSenderName";
      this.txtSenderName.ReadOnly = true;
      this.txtSenderName.Size = new Size(215, 20);
      this.txtSenderName.TabIndex = 54;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(389, 34);
      this.label1.Name = "label1";
      this.label1.Size = new Size(37, 13);
      this.label1.TabIndex = 53;
      this.label1.Text = "Email";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(162, 34);
      this.label2.Name = "label2";
      this.label2.Size = new Size(39, 13);
      this.label2.TabIndex = 52;
      this.label2.Text = "Name";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlRecipients.AutoScroll = true;
      this.pnlRecipients.Controls.Add((Control) this.recipientControl1);
      this.pnlRecipients.Location = new Point(13, 117);
      this.pnlRecipients.Name = "pnlRecipients";
      this.pnlRecipients.Size = new Size(657, 122);
      this.pnlRecipients.TabIndex = 21;
      this.recipientControl1.AutoScroll = true;
      this.recipientControl1.BackColor = Color.WhiteSmoke;
      this.recipientControl1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.recipientControl1.Location = new Point(2, 2);
      this.recipientControl1.Margin = new Padding(2);
      this.recipientControl1.Name = "recipientControl1";
      this.recipientControl1.Size = new Size(646, 27);
      this.recipientControl1.TabIndex = 0;
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(298, 100);
      this.label13.Name = "label13";
      this.label13.Size = new Size(36, 14);
      this.label13.TabIndex = 17;
      this.label13.Text = "Email";
      this.label13.TextAlign = ContentAlignment.MiddleLeft;
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(162, 100);
      this.label14.Name = "label14";
      this.label14.Size = new Size(38, 14);
      this.label14.TabIndex = 16;
      this.label14.Text = "Name";
      this.label14.TextAlign = ContentAlignment.MiddleLeft;
      this.cboSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSubject.FormattingEnabled = true;
      this.cboSubject.Location = new Point(88, 250);
      this.cboSubject.Name = "cboSubject";
      this.cboSubject.Size = new Size(582, 22);
      this.cboSubject.Sorted = true;
      this.cboSubject.TabIndex = 9;
      this.cboSenderType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboSenderType.FormattingEnabled = true;
      this.cboSenderType.Location = new Point(13, 50);
      this.cboSenderType.Name = "cboSenderType";
      this.cboSenderType.Size = new Size(146, 22);
      this.cboSenderType.TabIndex = 1;
      this.cboSenderType.SelectedIndexChanged += new EventHandler(this.cboSenderType_SelectedIndexChanged);
      this.htmlEmailControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.htmlEmailControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailControl.Location = new Point(11, 278);
      this.htmlEmailControl.Name = "htmlEmailControl";
      this.htmlEmailControl.Size = new Size(659, 270);
      this.htmlEmailControl.TabIndex = 10;
      this.label15.AutoSize = true;
      this.label15.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label15.Location = new Point(14, 36);
      this.label15.Name = "label15";
      this.label15.Size = new Size(76, 14);
      this.label15.TabIndex = 0;
      this.label15.Text = "Sender Type";
      this.label15.TextAlign = ContentAlignment.MiddleLeft;
      this.label16.AutoSize = true;
      this.label16.Location = new Point(8, 254);
      this.label16.Name = "label16";
      this.label16.Size = new Size(50, 14);
      this.label16.TabIndex = 8;
      this.label16.Text = "* Subject";
      this.label16.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(710, 615);
      this.Controls.Add((Control) this.gcMessage);
      this.Controls.Add((Control) this.btnSkip);
      this.Controls.Add((Control) this.btnSend);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (OTPSendEmailDialogCC);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Send Notification Email";
      this.gcMessage.ResumeLayout(false);
      this.gcMessage.PerformLayout();
      ((ISupportInitialize) this.btnAddRecipient).EndInit();
      this.pnlRecipients.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
