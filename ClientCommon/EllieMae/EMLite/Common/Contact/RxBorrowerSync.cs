// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.RxBorrowerSync
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class RxBorrowerSync : Form
  {
    private BorrowerInfo borContact;
    private string firstName = "";
    private string lastName = "";
    private string middleName = "";
    private string suffix = "";
    private string ssn = "";
    private string hPhone = "";
    private string wPhone = "";
    private string cPhone = "";
    private Address hAddress;
    private DateTime birthday = DateTime.MinValue;
    private bool married;
    private bool isCoborrower;
    private string hEmail = "";
    private bool forceOptOutLogic;
    private bool twoWayCustomSyncOnly = true;
    private FeaturesAclManager aclMgr;
    private CustomFieldMappingCollection customFieldsMappings;
    private static string sw = Tracing.SwContact;
    private string wEmail = "";
    private string wEmployer = "";
    private Address wAddress;
    private string wJobTitle = "";
    private IContainer components;
    private Label label1;
    private GroupContainer gcMismatch;
    private ListView lvConflict;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private Label label2;
    private RadioButton rdoLoan;
    private RadioButton rdoContact;
    private Button btnUpdate;
    private Button btnNoUpdate;
    private FlowLayoutPanel flowLayoutPanel1;

    public RxBorrowerSync(bool isCoborrower, BorrowerInfo selectedContact)
      : this(isCoborrower, selectedContact, false, true)
    {
    }

    public RxBorrowerSync(
      bool isCoborrower,
      BorrowerInfo selectedContact,
      bool forceOptOutLogic,
      bool twoWayCustomSyncOnly)
    {
      this.InitializeComponent();
      this.borContact = selectedContact;
      this.twoWayCustomSyncOnly = twoWayCustomSyncOnly;
      this.isCoborrower = isCoborrower;
      this.forceOptOutLogic = forceOptOutLogic;
      this.customFieldsMappings = CustomFieldMappingCollection.GetCustomFieldMappingCollection(Session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.Borrower, twoWayCustomSyncOnly));
      this.initPageValue();
      this.enforceSecurity(selectedContact.OwnerID);
      this.DialogResult = DialogResult.Cancel;
    }

    private void enforceSecurity(string ownerID)
    {
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (this.forceOptOutLogic)
      {
        if (this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Contacts_Update))
        {
          this.btnNoUpdate.Visible = true;
        }
        else
        {
          this.btnNoUpdate.Visible = false;
          this.ControlBox = false;
        }
      }
      if (this.lvConflict.Items.Count == 0)
      {
        this.rdoContact.Enabled = false;
        this.rdoContact.Checked = false;
        this.rdoLoan.Enabled = false;
        this.rdoLoan.Checked = false;
      }
      else if (UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas) || ownerID == Session.UserID || Session.AclGroupManager.GetBorrowerContactAccessRight(Session.UserInfo, ownerID) == AclResourceAccess.ReadWrite)
      {
        this.rdoLoan.Enabled = true;
        if (!this.isEmptyLoan())
          return;
        this.rdoContact.Checked = true;
      }
      else
      {
        this.rdoLoan.Enabled = false;
        this.rdoContact.Checked = true;
      }
    }

    private bool isEmptyLoan()
    {
      if (this.firstName != "" || this.lastName != "" || this.middleName != "" || this.suffix != "" || this.ssn != "" || this.hPhone != "" || this.wPhone != "" || this.cPhone != "" || this.hEmail != "" || this.hAddress.ToString() != "" && this.hAddress.ToString().Trim() != "," || this.wEmail != "" || this.wEmployer != "" || this.wAddress.ToString() != "" && this.wAddress.ToString().Trim() != "," || this.wJobTitle != "")
        return false;
      return this.isCoborrower ? (!(Session.LoanData.GetField("1403").Trim() != "") || !(Session.LoanData.GetField("1403").Trim() != "//")) && !(Session.LoanData.GetField("84").Trim() != "") : (!(Session.LoanData.GetField("1402").Trim() != "") || !(Session.LoanData.GetField("1402").Trim() != "//")) && !(Session.LoanData.GetField("52").Trim() != "");
    }

    private void initPageValue()
    {
      LoanData loanData = Session.LoanData;
      if (this.isCoborrower)
      {
        this.firstName = loanData.GetField("4004").Trim();
        this.lastName = loanData.GetField("4006").Trim();
        this.middleName = loanData.GetField("4005").Trim();
        this.suffix = loanData.GetField("4007").Trim();
        this.ssn = loanData.GetField("97").Trim();
        this.hPhone = loanData.GetField("98").Trim();
        this.wPhone = loanData.GetField("FE0217").Trim();
        this.cPhone = loanData.GetField("1480").Trim();
        this.hEmail = loanData.GetField("1268").Trim();
        this.hAddress = new Address(loanData.GetField("FR0204").Trim(), "", loanData.GetField("FR0206").Trim(), loanData.GetField("FR0207").Trim(), loanData.GetField("FR0208").Trim());
        this.wEmail = loanData.GetField("1179").Trim();
        this.wEmployer = loanData.GetField("FE0202").Trim();
        this.wAddress = new Address(loanData.GetField("FE0204").Trim(), "", loanData.GetField("FE0205").Trim(), loanData.GetField("FE0206").Trim(), loanData.GetField("FE0207").Trim());
        this.wJobTitle = loanData.GetField("FE0210").Trim();
        if (loanData.GetField("1403").Trim() != "")
        {
          if (Utils.IsDate((object) loanData.GetField("1403").Trim()))
          {
            try
            {
              this.birthday = DateTime.Parse(loanData.GetField("1403").Trim());
            }
            catch (Exception ex)
            {
            }
          }
        }
        if (loanData.GetField("84").Trim() != "" && loanData.GetField("84").Trim() != "Unmarried")
          this.married = true;
      }
      else
      {
        this.firstName = loanData.GetField("4000").Trim();
        this.lastName = loanData.GetField("4002").Trim();
        this.middleName = loanData.GetField("4001").Trim();
        this.suffix = loanData.GetField("4003").Trim();
        this.ssn = loanData.GetField("65").Trim();
        this.hPhone = loanData.GetField("66").Trim();
        this.wPhone = loanData.GetField("FE0117").Trim();
        this.cPhone = loanData.GetField("1490").Trim();
        this.hEmail = loanData.GetField("1240").Trim();
        this.hAddress = new Address(loanData.GetField("FR0104").Trim(), "", loanData.GetField("FR0106").Trim(), loanData.GetField("FR0107").Trim(), loanData.GetField("FR0108").Trim());
        this.wEmail = loanData.GetField("1178").Trim();
        this.wEmployer = loanData.GetField("FE0102").Trim();
        this.wAddress = new Address(loanData.GetField("FE0104").Trim(), "", loanData.GetField("FE0105").Trim(), loanData.GetField("FE0106").Trim(), loanData.GetField("FE0107").Trim());
        this.wJobTitle = loanData.GetField("FE0110").Trim();
        if (loanData.GetField("1402").Trim() != "")
        {
          if (Utils.IsDate((object) loanData.GetField("1402").Trim()))
          {
            try
            {
              this.birthday = DateTime.Parse(loanData.GetField("1402").Trim());
            }
            catch (Exception ex)
            {
            }
          }
        }
        if (loanData.GetField("52").Trim() != "" && loanData.GetField("52").Trim() != "Unmarried")
          this.married = true;
      }
      if (this.firstName != this.borContact.FirstName.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "First Name",
          this.firstName,
          this.borContact.FirstName
        }));
      if (this.lastName != this.borContact.LastName.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Last Name",
          this.lastName,
          this.borContact.LastName
        }));
      if (this.middleName != this.borContact.MiddleName.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Middle Name",
          this.middleName,
          this.borContact.MiddleName
        }));
      if (this.suffix != this.borContact.SuffixName.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Suffix",
          this.suffix,
          this.borContact.SuffixName
        }));
      if (this.hPhone != this.borContact.HomePhone.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Home Phone",
          this.hPhone,
          this.borContact.HomePhone
        }));
      if (this.wPhone != this.borContact.WorkPhone.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Work Phone",
          this.wPhone,
          this.borContact.WorkPhone
        }));
      if (this.cPhone != this.borContact.MobilePhone.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Cell Phone",
          this.cPhone,
          this.borContact.MobilePhone
        }));
      if (this.hEmail != this.borContact.PersonalEmail.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Home Email",
          this.hEmail,
          this.borContact.PersonalEmail
        }));
      if (this.wEmail != this.borContact.BizEmail.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Work Email",
          this.wEmail,
          this.borContact.BizEmail
        }));
      if (this.wEmployer != this.borContact.EmployerName.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Employer",
          this.wEmployer,
          this.borContact.EmployerName
        }));
      if (this.wAddress.ToString(true) != this.borContact.BizAddress.ToString(false).Replace("  ", " "))
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Employer Address",
          this.wAddress.ToString(true),
          this.borContact.BizAddress.ToString(false).Replace("  ", " ")
        }));
      if (this.wJobTitle != this.borContact.JobTitle.Trim())
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Job Title",
          this.wJobTitle,
          this.borContact.JobTitle
        }));
      if (this.hAddress.ToString(true) != this.borContact.HomeAddress.ToString(false).Replace("  ", " "))
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Home Address",
          this.hAddress.ToString(true),
          this.borContact.HomeAddress.ToString(false).Replace("  ", " ")
        }));
      if (this.birthday != this.borContact.Birthdate)
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Birthday",
          this.birthday == DateTime.MinValue ? "" : this.birthday.ToShortDateString(),
          this.borContact.Birthdate == DateTime.MinValue ? "" : this.borContact.Birthdate.ToShortDateString()
        }));
      if (this.married != this.borContact.Married)
        this.lvConflict.Items.Add(new ListViewItem(new string[3]
        {
          "Married",
          this.married.ToString(),
          this.borContact.Married.ToString()
        }));
      ContactCustomFieldInfoCollection customFieldInfo = Session.ContactManager.GetCustomFieldInfo(EllieMae.EMLite.ContactUI.ContactType.Borrower);
      ContactCustomField[] fieldsForContact = Session.ContactManager.GetCustomFieldsForContact(this.borContact.ContactID, EllieMae.EMLite.ContactUI.ContactType.Borrower);
      foreach (CustomFieldMapping customFieldsMapping in (CollectionBase) this.customFieldsMappings)
      {
        string field = Session.LoanData.GetField(customFieldsMapping.LoanFieldId);
        string str1 = "";
        string str2 = customFieldsMapping.LoanFieldId;
        foreach (ContactCustomField contactCustomField in fieldsForContact)
        {
          if (contactCustomField.FieldID == customFieldsMapping.FieldNumber)
          {
            str1 = contactCustomField.FieldValue;
            break;
          }
        }
        foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
        {
          if (contactCustomFieldInfo.LoanFieldId == customFieldsMapping.LoanFieldId)
          {
            str2 = contactCustomFieldInfo.Label;
            break;
          }
        }
        if (field != str1)
          this.lvConflict.Items.Add(new ListViewItem(new string[3]
          {
            str2,
            field,
            str1
          }));
      }
      this.gcMismatch.Text = "Unmatching items(" + (object) this.lvConflict.Items.Count + ")";
    }

    public bool HasConflict => this.lvConflict.Items.Count > 0;

    public BorrowerInfo BorrowerObj => this.borContact;

    private void btnUpdate_Click(object sender, EventArgs e)
    {
      LoanData loanData = Session.LoanData;
      if (this.lvConflict.Items.Count > 0)
      {
        if (this.rdoContact.Checked)
        {
          string str1 = this.borContact.HomeAddress.Street1 + " " + this.borContact.HomeAddress.Street2;
          string str2 = this.borContact.BizAddress.Street1 + " " + this.borContact.BizAddress.Street2;
          if (this.isCoborrower)
          {
            loanData.SetField("4004", this.borContact.FirstName);
            loanData.SetField("4006", this.borContact.LastName);
            loanData.SetField("4005", this.borContact.MiddleName);
            loanData.SetField("4007", this.borContact.SuffixName);
            loanData.SetField("97", this.borContact.SSN);
            loanData.SetField("98", this.borContact.HomePhone);
            loanData.SetField("FE0217", this.borContact.WorkPhone);
            loanData.SetField("1480", this.borContact.MobilePhone);
            loanData.SetField("FR0204", str1.Trim());
            loanData.SetField("FR0206", this.borContact.HomeAddress.City);
            loanData.SetField("FR0207", this.borContact.HomeAddress.State);
            loanData.SetField("FR0208", this.borContact.HomeAddress.Zip);
            loanData.SetField("1403", this.borContact.BirthdateString);
            loanData.SetField("1268", this.borContact.PersonalEmail);
            loanData.SetField("84", this.borContact.Married ? "Married" : "Unmarried");
            loanData.SetField("1179", this.borContact.BizEmail);
            loanData.SetField("FE0202", this.borContact.EmployerName);
            loanData.SetField("FE0204", str2.Trim());
            loanData.SetField("FE0205", this.borContact.BizAddress.City);
            loanData.SetField("FE0206", this.borContact.BizAddress.State);
            loanData.SetField("FE0207", this.borContact.BizAddress.Zip);
            loanData.SetField("FE0210", this.borContact.JobTitle);
          }
          else
          {
            loanData.SetField("4000", this.borContact.FirstName);
            loanData.SetField("4002", this.borContact.LastName);
            loanData.SetField("4001", this.borContact.MiddleName);
            loanData.SetField("4003", this.borContact.SuffixName);
            loanData.SetField("65", this.borContact.SSN);
            loanData.SetField("66", this.borContact.HomePhone);
            loanData.SetField("FE0117", this.borContact.WorkPhone);
            loanData.SetField("1490", this.borContact.MobilePhone);
            loanData.SetField("FR0104", str1.Trim());
            loanData.SetField("FR0106", this.borContact.HomeAddress.City);
            loanData.SetField("FR0107", this.borContact.HomeAddress.State);
            loanData.SetField("FR0108", this.borContact.HomeAddress.Zip);
            loanData.SetField("1402", this.borContact.BirthdateString);
            loanData.SetField("1240", this.borContact.PersonalEmail);
            loanData.SetField("52", this.borContact.Married ? "Married" : "Unmarried");
            loanData.SetField("1178", this.borContact.BizEmail);
            loanData.SetField("FE0102", this.borContact.EmployerName);
            loanData.SetField("FE0104", str2.Trim());
            loanData.SetField("FE0105", this.borContact.BizAddress.City);
            loanData.SetField("FE0106", this.borContact.BizAddress.State);
            loanData.SetField("FE0107", this.borContact.BizAddress.Zip);
            loanData.SetField("FE0110", this.borContact.JobTitle);
          }
          ContactCustomField[] fieldsForContact = Session.ContactManager.GetCustomFieldsForContact(this.borContact.ContactID, EllieMae.EMLite.ContactUI.ContactType.Borrower);
          foreach (CustomFieldMapping customFieldsMapping in (CollectionBase) this.customFieldsMappings)
          {
            Session.LoanData.GetField(customFieldsMapping.LoanFieldId);
            foreach (ContactCustomField contactCustomField in fieldsForContact)
            {
              if (contactCustomField.FieldID == customFieldsMapping.FieldNumber)
              {
                Session.LoanData.SetField(customFieldsMapping.LoanFieldId, contactCustomField.FieldValue);
                break;
              }
            }
          }
        }
        else
        {
          if (this.isCoborrower)
          {
            this.borContact.FirstName = loanData.GetField("4004");
            this.borContact.LastName = loanData.GetField("4006");
            this.borContact.MiddleName = loanData.GetField("4005");
            this.borContact.SuffixName = loanData.GetField("4007");
            this.borContact.SSN = loanData.GetField("97");
            this.borContact.HomePhone = loanData.GetField("98");
            this.borContact.WorkPhone = loanData.GetField("FE0217");
            this.borContact.MobilePhone = loanData.GetField("1480");
            this.borContact.HomeAddress.Street1 = loanData.GetField("FR0204");
            this.borContact.HomeAddress.Street2 = "";
            this.borContact.HomeAddress.City = loanData.GetField("FR0206");
            this.borContact.HomeAddress.State = loanData.GetField("FR0207");
            this.borContact.HomeAddress.Zip = loanData.GetField("FR0208");
            this.borContact.PersonalEmail = loanData.GetField("1268");
            if (loanData.GetField("1403") != "" && Utils.IsDate((object) loanData.GetField("1403")))
              this.borContact.Birthdate = DateTime.Parse(loanData.GetField("1403"));
            this.borContact.Married = loanData.GetField("84") == "Married";
            this.borContact.BizEmail = loanData.GetField("1179");
            this.borContact.EmployerName = loanData.GetField("FE0202");
            this.borContact.BizAddress.Street1 = loanData.GetField("FE0204");
            this.borContact.BizAddress.Street2 = "";
            this.borContact.BizAddress.City = loanData.GetField("FE0205");
            this.borContact.BizAddress.State = loanData.GetField("FE0206");
            this.borContact.BizAddress.Zip = loanData.GetField("FE0207");
            this.borContact.JobTitle = loanData.GetField("FE0210");
          }
          else
          {
            this.borContact.FirstName = loanData.GetField("4000");
            this.borContact.LastName = loanData.GetField("4002");
            this.borContact.MiddleName = loanData.GetField("4001");
            this.borContact.SuffixName = loanData.GetField("4003");
            this.borContact.SSN = loanData.GetField("65");
            this.borContact.HomePhone = loanData.GetField("66");
            this.borContact.WorkPhone = loanData.GetField("FE0117");
            this.borContact.MobilePhone = loanData.GetField("1490");
            this.borContact.HomeAddress.Street1 = loanData.GetField("FR0104");
            this.borContact.HomeAddress.Street2 = "";
            this.borContact.HomeAddress.City = loanData.GetField("FR0106");
            this.borContact.HomeAddress.State = loanData.GetField("FR0107");
            this.borContact.HomeAddress.Zip = loanData.GetField("FR0108");
            this.borContact.PersonalEmail = loanData.GetField("1240");
            this.borContact.Birthdate = !(loanData.GetField("1402") != "") || !Utils.IsDate((object) loanData.GetField("1402")) ? DateTime.MinValue : DateTime.Parse(loanData.GetField("1402"));
            this.borContact.Married = loanData.GetField("52") == "Married";
            this.borContact.BizEmail = loanData.GetField("1178");
            this.borContact.EmployerName = loanData.GetField("FE0102");
            this.borContact.BizAddress.Street1 = loanData.GetField("FE0104");
            this.borContact.BizAddress.Street2 = "";
            this.borContact.BizAddress.City = loanData.GetField("FE0105");
            this.borContact.BizAddress.State = loanData.GetField("FE0106");
            this.borContact.BizAddress.Zip = loanData.GetField("FE0107");
            this.borContact.JobTitle = loanData.GetField("FE0110");
          }
          Session.ContactManager.UpdateBorrower(this.borContact);
          List<ContactCustomField> contactCustomFieldList = new List<ContactCustomField>();
          foreach (CustomFieldMapping customFieldsMapping in (CollectionBase) this.customFieldsMappings)
          {
            string fieldValue = (string) null;
            try
            {
              fieldValue = Session.LoanData.GetField(customFieldsMapping.LoanFieldId);
              Tracing.Log(RxBorrowerSync.sw, TraceLevel.Verbose, "Custom Field Mapping", string.Format("CustomFieldMapping: CategoryId='{0}', FieldNumber='{1}', FieldFormat='{2}', LoanFieldId='{3}', FieldValue='{4}'", (object) customFieldsMapping.CategoryId, (object) customFieldsMapping.FieldNumber, (object) customFieldsMapping.FieldFormat, (object) customFieldsMapping.LoanFieldId, (object) fieldValue));
            }
            catch (Exception ex)
            {
              Tracing.Log(RxBorrowerSync.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Loan Field ID '{2}', Value '{1}' to Business Contact 'Custom Field {0}' failed.", (object) customFieldsMapping.FieldNumber.ToString(), fieldValue == null ? (object) "UNKNOWN" : (object) fieldValue, (object) customFieldsMapping.LoanFieldId));
              fieldValue = (string) null;
            }
            if (fieldValue != null)
            {
              ContactCustomField contactCustomField = new ContactCustomField(this.borContact.ContactID, customFieldsMapping.FieldNumber, this.borContact.OwnerID, fieldValue);
              contactCustomFieldList.Add(contactCustomField);
            }
          }
          if (contactCustomFieldList.Count != 0)
          {
            foreach (ContactCustomField contactCustomField in contactCustomFieldList)
            {
              contactCustomField.ContactID = this.borContact.ContactID;
              contactCustomField.OwnerID = this.borContact.OwnerID;
            }
            Session.ContactManager.UpdateCustomFieldsForContact(this.borContact.ContactID, EllieMae.EMLite.ContactUI.ContactType.Borrower, contactCustomFieldList.ToArray());
          }
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private void RxBorrowerSync_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult == DialogResult.OK || this.btnNoUpdate.Visible)
        return;
      e.Cancel = true;
    }

    private void performCustomFieldSync()
    {
      Tracing.Log(RxBorrowerSync.sw, TraceLevel.Verbose, "Custom Field Mapping", string.Format("Entering ClientCommon.Contact.RxBorrowerSync.updateContactCustomFields(contact.FirstName='{0}', contact.LastName='{1}')", (object) this.borContact.FirstName, (object) this.borContact.LastName));
      List<ContactCustomField> contactCustomFieldList = new List<ContactCustomField>();
      CustomFieldMappingCollection mappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(Session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.Borrower, true));
      if (mappingCollection.Count == 0)
        return;
      foreach (CustomFieldMapping customFieldMapping in (CollectionBase) mappingCollection)
      {
        string fieldValue = (string) null;
        try
        {
          fieldValue = Session.LoanData.GetField(customFieldMapping.LoanFieldId);
          Tracing.Log(RxBorrowerSync.sw, TraceLevel.Verbose, "Custom Field Mapping", string.Format("CustomFieldMapping: CategoryId='{0}', FieldNumber='{1}', FieldFormat='{2}', LoanFieldId='{3}', FieldValue='{4}'", (object) customFieldMapping.CategoryId, (object) customFieldMapping.FieldNumber, (object) customFieldMapping.FieldFormat, (object) customFieldMapping.LoanFieldId, (object) fieldValue));
        }
        catch (Exception ex)
        {
          Tracing.Log(RxBorrowerSync.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Loan Field ID '{2}', Value '{1}' to Business Contact 'Custom Field {0}' failed.", (object) customFieldMapping.FieldNumber.ToString(), fieldValue == null ? (object) "UNKNOWN" : (object) fieldValue, (object) customFieldMapping.LoanFieldId));
          fieldValue = (string) null;
        }
        if (fieldValue != null)
        {
          ContactCustomField contactCustomField = new ContactCustomField(this.borContact.ContactID, customFieldMapping.FieldNumber, this.borContact.OwnerID, fieldValue);
          contactCustomFieldList.Add(contactCustomField);
        }
      }
      if (contactCustomFieldList.Count == 0)
        return;
      foreach (ContactCustomField contactCustomField in contactCustomFieldList)
      {
        contactCustomField.ContactID = this.borContact.ContactID;
        contactCustomField.OwnerID = this.borContact.OwnerID;
      }
      Session.ContactManager.UpdateCustomFieldsForContact(this.borContact.ContactID, EllieMae.EMLite.ContactUI.ContactType.Borrower, contactCustomFieldList.ToArray());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RxBorrowerSync));
      this.label1 = new Label();
      this.gcMismatch = new GroupContainer();
      this.lvConflict = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.label2 = new Label();
      this.rdoLoan = new RadioButton();
      this.rdoContact = new RadioButton();
      this.btnUpdate = new Button();
      this.btnNoUpdate = new Button();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.gcMismatch.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(464, 53);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.gcMismatch.Controls.Add((Control) this.lvConflict);
      this.gcMismatch.Location = new Point(12, 65);
      this.gcMismatch.Name = "gcMismatch";
      this.gcMismatch.Size = new Size(464, 136);
      this.gcMismatch.TabIndex = 1;
      this.gcMismatch.Text = "Unmatching items";
      this.lvConflict.BorderStyle = BorderStyle.None;
      this.lvConflict.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3
      });
      this.lvConflict.Dock = DockStyle.Fill;
      this.lvConflict.GridLines = true;
      this.lvConflict.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.lvConflict.Location = new Point(1, 26);
      this.lvConflict.Name = "lvConflict";
      this.lvConflict.Size = new Size(462, 109);
      this.lvConflict.TabIndex = 0;
      this.lvConflict.UseCompatibleStateImageBehavior = false;
      this.lvConflict.View = View.Details;
      this.columnHeader1.Text = "Field Name";
      this.columnHeader1.Width = 121;
      this.columnHeader2.Text = "Loan";
      this.columnHeader2.Width = 144;
      this.columnHeader3.Text = "Borrower Contacts";
      this.columnHeader3.Width = 180;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(94, 208);
      this.label2.Name = "label2";
      this.label2.Size = new Size(37, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Select";
      this.rdoLoan.AutoSize = true;
      this.rdoLoan.Checked = true;
      this.rdoLoan.Location = new Point(143, 206);
      this.rdoLoan.Name = "rdoLoan";
      this.rdoLoan.Size = new Size(75, 17);
      this.rdoLoan.TabIndex = 3;
      this.rdoLoan.TabStop = true;
      this.rdoLoan.Text = "Loan Data";
      this.rdoLoan.UseVisualStyleBackColor = true;
      this.rdoContact.AutoSize = true;
      this.rdoContact.Location = new Point(286, 205);
      this.rdoContact.Name = "rdoContact";
      this.rdoContact.Size = new Size(93, 17);
      this.rdoContact.TabIndex = 4;
      this.rdoContact.TabStop = true;
      this.rdoContact.Text = "Contacts Data";
      this.rdoContact.UseVisualStyleBackColor = true;
      this.btnUpdate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnUpdate.Location = new Point(12, 0);
      this.btnUpdate.Margin = new Padding(0);
      this.btnUpdate.Name = "btnUpdate";
      this.btnUpdate.Size = new Size(91, 22);
      this.btnUpdate.TabIndex = 5;
      this.btnUpdate.Text = "Synchronize";
      this.btnUpdate.UseVisualStyleBackColor = true;
      this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
      this.btnNoUpdate.DialogResult = DialogResult.Cancel;
      this.btnNoUpdate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnNoUpdate.Location = new Point(108, 0);
      this.btnNoUpdate.Margin = new Padding(5, 0, 0, 0);
      this.btnNoUpdate.Name = "btnNoUpdate";
      this.btnNoUpdate.Size = new Size(136, 22);
      this.btnNoUpdate.TabIndex = 6;
      this.btnNoUpdate.Text = "Keep unsynchronized";
      this.btnNoUpdate.UseVisualStyleBackColor = true;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNoUpdate);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnUpdate);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(232, 240);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new Padding(0, 0, 10, 0);
      this.flowLayoutPanel1.Size = new Size(254, 22);
      this.flowLayoutPanel1.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(485, 274);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.Controls.Add((Control) this.rdoContact);
      this.Controls.Add((Control) this.rdoLoan);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.gcMismatch);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RxBorrowerSync);
      this.Text = "  Data Match";
      this.FormClosing += new FormClosingEventHandler(this.RxBorrowerSync_FormClosing);
      this.gcMismatch.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
