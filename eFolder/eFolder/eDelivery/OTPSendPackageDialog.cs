// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.OTPSendPackageDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.eDelivery.ePass;
using EllieMae.EMLite.eFolder.eDelivery.HelperMethods;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.RemotingServices;
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
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class OTPSendPackageDialog : SendPackageDialog
  {
    private IContainer components;

    public OTPSendPackageDialog(
      eDeliveryMessageType packageType,
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList)
      : base(packageType, loanDataMgr, coversheetFile, signList, neededList, pdfList)
    {
      this.InitializeComponent();
      this.SetUI();
    }

    public OTPSendPackageDialog(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      string[] pdfList,
      HtmlEmailTemplateType emailType)
      : base(loanDataMgr, coversheetFile, signList, neededList, pdfList, emailType)
    {
      this.InitializeComponent();
      this.SetUI();
    }

    public OTPSendPackageDialog(eDeliveryMessage msg)
      : base(msg)
    {
      this.InitializeComponent();
      this.SetUI();
    }

    protected override void addRecipient(eDeliveryRecipient recipient)
    {
      if (recipient == null)
        return;
      if (this.contacts != null)
      {
        ContactDetails contactDetails = recipient.RecipientType == eDeliveryEntityType.Borrower.ToString("g") || recipient.RecipientType == eDeliveryEntityType.Coborrower.ToString("g") ? this.contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == recipient.BorrowerId)) : (!(recipient.RecipientType.ToUpper() == eDeliveryEntityType.NonBorrowingOwner.ToString("g").ToUpper()) ? this.contacts.Find((Predicate<ContactDetails>) (x => x.contactType.ToUpper() == recipient.RecipientType.ToUpper() && x.name == (!string.IsNullOrEmpty(recipient.UnparsedName) ? recipient.UnparsedName : recipient.FirstName + " " + recipient.LastName) && x.email.ToUpper() == recipient.EmailAddress.ToUpper())) : this.contacts.Find((Predicate<ContactDetails>) (x => x.borrowerId == recipient.BorrowerId)));
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
          deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientType.ToUpper() == recipient.RecipientType.ToUpper() && x.UnparsedName.Trim() == (!string.IsNullOrEmpty(recipient.UnparsedName) ? recipient.UnparsedName.Trim() : (recipient.FirstName + " " + recipient.LastName).Trim()) && x.EmailAddress.ToUpper() == recipient.EmailAddress.ToUpper()));
        recipient.RecipientId = deliveryRecipient == null ? (recipient.RecipientId == null ? Guid.NewGuid().ToString() : recipient.RecipientId) : deliveryRecipient.RecipientId;
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
      recipientControl.Tag = (object) recipient.NboIndex;
      this.pnlRecipients.Controls.Add((Control) recipientControl);
    }

    protected override void initTitle()
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

    protected override void setWindowDisplay(DocuSignSigningDialog form)
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

    protected override void hideFulfillmentSection()
    {
      this.gcMessage.Width = this.gcFulfillment.Right - this.gcMessage.Left;
      this.gcSigning.Width = this.gcMessage.Width;
      this.gcFulfillment.Visible = false;
      this.Width = 605;
    }

    protected override bool GetSelectRecipients()
    {
      return this.pnlRecipients.Controls.OfType<OTPRecipientControl>().Any<OTPRecipientControl>((Func<OTPRecipientControl, bool>) (ctrl => ctrl.cbSelect.Checked));
    }

    protected override List<ContactDetails> GetLoanContacts() => base.GetLoanContacts();

    protected override void validateRecipients()
    {
      string htmlBodyText = this.htmlEmailControl.HtmlBodyText;
      string text = this.cboSubject.Text;
      int num = 0;
      foreach (Control control in (ArrangedElementCollection) this.pnlRecipients.Controls)
      {
        if (control != null && control.GetType() == typeof (OTPRecipientControl))
        {
          OTPRecipientControl recipientcontrol = control as OTPRecipientControl;
          if (recipientcontrol.cbSelect.Checked)
          {
            if (this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name)) == null)
            {
              eDeliveryRecipient deliveryRecipient = this.deletedRecipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name));
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
            if (string.IsNullOrEmpty(recipientcontrol.ddPhoneNumber.Text))
              this.invalidList.Add(recipientcontrol.cbSelect.Text + "'s phone number is required.");
            else if (!this.IsValidPhoneNumber(this.RemovePhoneNumberPrefix(recipientcontrol.ddPhoneNumber.Text)) || !this.MatchPhoneFormat(this.RemovePhoneNumberPrefix(recipientcontrol.ddPhoneNumber.Text)))
              this.invalidList.Add(recipientcontrol.cbSelect.Text + "'s phone number must be a 10 - digit phone number with XXXXXXXXXX / (XXX)XXX-XXXX / (XXX)-XXX-XXXX / XXX-XXX-XXXX format.");
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
        }
      }
    }

    protected override void saveRecipients()
    {
      foreach (Control control in (ArrangedElementCollection) this.pnlRecipients.Controls)
      {
        if (control != null && control.GetType() == typeof (OTPRecipientControl))
        {
          OTPRecipientControl recipientcontrol = control as OTPRecipientControl;
          if (recipientcontrol.cbSelect.Checked)
          {
            Dictionary<string, string> contactDetails = new Dictionary<string, string>();
            if (!recipientcontrol.txtName.ReadOnly)
              contactDetails.Add("Name", recipientcontrol.txtName.Text);
            if (!recipientcontrol.txtEmail.ReadOnly)
              contactDetails.Add("Email", recipientcontrol.txtEmail.Text);
            if (recipientcontrol.cbSelect.Text == "Non-Borrowing Owner" && (!recipientcontrol.txtName.ReadOnly || !recipientcontrol.txtEmail.ReadOnly))
              contactDetails.Add("NboIndex", recipientcontrol.Tag.ToString());
            eDeliveryRecipient deliveryRecipient = this.Recipients.Find((Predicate<eDeliveryRecipient>) (x => x.RecipientId == recipientcontrol.Name));
            if (!string.IsNullOrEmpty(recipientcontrol.ddPhoneNumber.Text))
            {
              string str = recipientcontrol.ddPhoneNumber.Items.Count <= 0 || string.IsNullOrEmpty(recipientcontrol.ddPhoneNumber.SelectedValue?.ToString()) || !(recipientcontrol.ddPhoneNumber.SelectedValue.ToString().ToLower() == "2") ? "Mobile" : "Home";
              deliveryRecipient.PhoneNumber = this.RemovePhoneNumberPrefix(recipientcontrol.ddPhoneNumber.Text);
              deliveryRecipient.PhoneType = (PhoneType) Enum.Parse(typeof (PhoneType), str);
            }
            deliveryRecipient.PartyId = deliveryRecipient.RecipientId;
            if (contactDetails.Count > 0)
            {
              contactDetails.Add("ContactType", recipientcontrol.cbSelect.Text);
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

    protected override void SetSelectedRecipients(EmailListDialog dialog)
    {
      FileContacts fileContact = SendPackageFactory.CreateFileContact(this.loan);
      foreach (string selectedContact in dialog.selectedContacts)
      {
        char[] chArray = new char[1]{ ';' };
        string[] strArray = selectedContact.Split(chArray);
        eDeliveryRecipient recipient;
        if (strArray[2] == "Borrower")
        {
          if (strArray[6] == this.loan.CurrentBorrowerPair.Borrower.Id)
          {
            recipient = this.populateBorrower();
          }
          else
          {
            recipient = new eDeliveryRecipient("1", eDeliveryEntityType.Borrower);
            recipient.UnparsedName = strArray[0];
            recipient.EmailAddress = strArray[1];
            recipient.RecipientType = strArray[2];
            recipient.BorrowerId = strArray[6];
            recipient.CellPhoneNumber = strArray[3];
            recipient.HomePhoneNumber = strArray[4];
            recipient.WorkPhoneNumber = strArray[5];
          }
        }
        else if (strArray[2] == "Coborrower")
        {
          if (strArray[6] == this.loan.CurrentBorrowerPair.CoBorrower.Id)
          {
            recipient = this.populateCoborrower();
          }
          else
          {
            recipient = new eDeliveryRecipient("2", eDeliveryEntityType.Coborrower);
            recipient.UnparsedName = strArray[0];
            recipient.EmailAddress = strArray[1];
            recipient.RecipientType = strArray[2];
            recipient.BorrowerId = strArray[6];
            recipient.CellPhoneNumber = strArray[3];
            recipient.HomePhoneNumber = strArray[4];
            recipient.WorkPhoneNumber = strArray[5];
          }
        }
        else
        {
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
        recipient.CellPhoneNumber = nonBorrowingOwner.CellPhoneNumber;
        recipient.HomePhoneNumber = nonBorrowingOwner.HomePhoneNumber;
        recipient.WorkPhoneNumber = nonBorrowingOwner.BusinessPhoneNumber;
        recipient.NboIndex = nonBorrowingOwner.NboIndex;
        ++num;
        this.addRecipient(recipient);
      }
    }

    protected override eDeliveryRecipient populateBorrower()
    {
      eDeliveryRecipient deliveryRecipient = base.populateBorrower();
      if (deliveryRecipient == null)
        return (eDeliveryRecipient) null;
      deliveryRecipient.CellPhoneNumber = this.loan.GetSimpleField("1490");
      deliveryRecipient.HomePhoneNumber = this.loan.GetSimpleField("66");
      deliveryRecipient.WorkPhoneNumber = this.loan.GetSimpleField("FE0117");
      return deliveryRecipient;
    }

    protected override eDeliveryRecipient populateCoborrower()
    {
      eDeliveryRecipient deliveryRecipient = base.populateCoborrower();
      if (deliveryRecipient == null)
        return (eDeliveryRecipient) null;
      deliveryRecipient.CellPhoneNumber = this.loan.GetSimpleField("1480");
      deliveryRecipient.HomePhoneNumber = this.loan.GetSimpleField("98");
      deliveryRecipient.WorkPhoneNumber = this.loan.GetSimpleField("FE0217");
      return deliveryRecipient;
    }

    protected override eDeliveryRecipient populateOriginator(string originatorUserID)
    {
      eDeliveryRecipient deliveryRecipient = base.populateOriginator(originatorUserID);
      this.loan.GetPairIndex(this.loan.PairId);
      UserInfo user = Session.OrganizationManager.GetUser(originatorUserID);
      if (!(user != (UserInfo) null) || deliveryRecipient == null)
        return (eDeliveryRecipient) null;
      deliveryRecipient.CellPhoneNumber = user.CellPhone;
      deliveryRecipient.WorkPhoneNumber = user.Phone;
      return deliveryRecipient;
    }

    protected override List<eDeliveryRecipient> populateNonBorrowingOwner()
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
        deliveryRecipient.CellPhoneNumber = nonBorrowingOwner.CellPhoneNumber;
        deliveryRecipient.HomePhoneNumber = nonBorrowingOwner.HomePhoneNumber;
        deliveryRecipient.WorkPhoneNumber = nonBorrowingOwner.BusinessPhoneNumber;
        ++num;
        deliveryRecipientList.Add(deliveryRecipient);
      }
      return deliveryRecipientList;
    }

    private void SetUI()
    {
      this.lblAuthorizationCode.Text = "Phone Number";
      this.lblAuthorizationCode.TextAlign = ContentAlignment.TopRight;
      this.lblAuthorizationCode.Location = new Point(450, 11);
      this.btnAddRecipient.Location = new Point(520, 12);
      if (this.packageType == eDeliveryMessageType.InitialDisclosures)
      {
        string str = (string) null;
        if (Modules.IsModuleAvailableForUser(EncompassModule.Fulfillment, false))
          str = Session.ConfigurationManager.GetCompanySetting("Fulfillment", "ServiceEnabled");
        if (str == "Y")
        {
          this.Width += 90;
          this.Height += 10;
        }
        else
          this.Width = 650;
      }
      else if (this.packageType == eDeliveryMessageType.RequestDocuments || this.packageType == eDeliveryMessageType.SendDocuments)
        this.Width = 650;
      this.pnlRecipients.Width = 612;
      this.panel6.Width = this.pnlRecipients.Width;
      this.panel2.Width = 620;
      this.gcSigning.Width = this.gcMessage.Width;
    }

    protected override string AddMessageForThirdPartyUsers(string confirmationMessage)
    {
      if (this.Recipients.Any<eDeliveryRecipient>((Func<eDeliveryRecipient, bool>) (x => x.EntityType == eDeliveryEntityType.Other)) && !this.ThirdpartyUsersAuthenticated())
        confirmationMessage = confirmationMessage + "\n\n" + "The authentication code(s) for 3rd party signer(s) can be viewed in the packages tab of the eFolder";
      return confirmationMessage;
    }

    private bool ThirdpartyUsersAuthenticated()
    {
      Task<List<RecipientDetails>> recipientDetails = new EBSServiceClient().GetRecipientDetails(this.loan.GetField("GUID"));
      Task.WaitAll((Task) recipientDetails);
      List<RecipientDetails> result = recipientDetails.Result;
      return result != null && result.Count > 0 && !result.Where<RecipientDetails>((Func<RecipientDetails, bool>) (x => x.contactType.ToLower() != "borrower" && x.contactType.ToLower() != "nonborrowingowner")).Any<RecipientDetails>((Func<RecipientDetails, bool>) (y => !y.isAuthenticated));
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
      return phoneNumber.Trim().Where<char>((Func<char, bool>) (x => char.IsDigit(x))).Count<char>() == 10;
    }

    private bool MatchPhoneFormat(string phoneNumber)
    {
      return (phoneNumber.Length <= 1 || !phoneNumber.StartsWith("(") && !phoneNumber.StartsWith(")") || !(phoneNumber[1].ToString() == "(") && !(phoneNumber[1].ToString() == ")")) && (!phoneNumber.Contains("(") || phoneNumber.Contains(")")) && (!phoneNumber.Contains(")") || phoneNumber.Contains("(")) && new Regex("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", RegexOptions.IgnoreCase | RegexOptions.Compiled).IsMatch(phoneNumber);
    }

    private string RemovePhoneNumberPrefix(string phoneNumber)
    {
      return phoneNumber?.Replace("M: ", string.Empty).Replace("H: ", string.Empty).Replace("W: ", string.Empty).Trim();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1012, 652);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (OTPSendPackageDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
