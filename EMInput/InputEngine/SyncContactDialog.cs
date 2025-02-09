// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SyncContactDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SyncContactDialog : Form
  {
    private static string sw = Tracing.SwContact;
    private LoanData _loan;
    private bool isPrimaryPair;
    private List<ContactCustomField> contactCustomFields;
    private System.ComponentModel.Container components;
    private ListView listViewBor;
    private RadioButton rbtnCreateBor;
    private RadioButton rbtnUpdateBor;
    private RadioButton rbtnUpdateCoBor;
    private RadioButton rbtnCreateCoBor;
    private ListView listViewCoBor;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private Button btnNext;
    private Button btnCancel;
    private ColumnHeader columnHeader6;
    private ColumnHeader columnHeader7;
    private ColumnHeader columnHeader8;
    private ColumnHeader columnHeader9;
    private ColumnHeader columnHeader10;
    private Panel panel1;
    private CheckBox chkBoxUpdateAddress;
    private Panel panel2;

    public SyncContactDialog(LoanData loan)
    {
      this.InitializeComponent();
      this._loan = loan;
      string field1 = this._loan.GetField("19");
      string field2 = this._loan.GetField("1811");
      if (this._loan.GetField("2821") == "Y" && field1 == "Purchase" && field2 == "PrimaryResidence")
      {
        this.chkBoxUpdateAddress.Checked = false;
        this.chkBoxUpdateAddress.Enabled = true;
      }
      else
      {
        this.chkBoxUpdateAddress.Checked = false;
        this.chkBoxUpdateAddress.Enabled = false;
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
      this.listViewBor = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.rbtnCreateBor = new RadioButton();
      this.rbtnUpdateBor = new RadioButton();
      this.rbtnUpdateCoBor = new RadioButton();
      this.rbtnCreateCoBor = new RadioButton();
      this.listViewCoBor = new ListView();
      this.columnHeader6 = new ColumnHeader();
      this.columnHeader7 = new ColumnHeader();
      this.columnHeader8 = new ColumnHeader();
      this.columnHeader9 = new ColumnHeader();
      this.columnHeader10 = new ColumnHeader();
      this.btnNext = new Button();
      this.btnCancel = new Button();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.chkBoxUpdateAddress = new CheckBox();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.listViewBor.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5
      });
      this.listViewBor.FullRowSelect = true;
      this.listViewBor.GridLines = true;
      this.listViewBor.HideSelection = false;
      this.listViewBor.Location = new Point(8, 52);
      this.listViewBor.MultiSelect = false;
      this.listViewBor.Name = "listViewBor";
      this.listViewBor.Size = new Size(584, 112);
      this.listViewBor.TabIndex = 0;
      this.listViewBor.UseCompatibleStateImageBehavior = false;
      this.listViewBor.View = View.Details;
      this.columnHeader1.Text = "Last Name";
      this.columnHeader1.Width = 119;
      this.columnHeader2.Text = "First Name";
      this.columnHeader2.Width = 95;
      this.columnHeader3.Text = "Company";
      this.columnHeader3.Width = 123;
      this.columnHeader4.Text = "Home Phone";
      this.columnHeader4.Width = 103;
      this.columnHeader5.Text = "Personal Email";
      this.columnHeader5.Width = 147;
      this.rbtnCreateBor.Location = new Point(8, 4);
      this.rbtnCreateBor.Name = "rbtnCreateBor";
      this.rbtnCreateBor.Size = new Size(580, 24);
      this.rbtnCreateBor.TabIndex = 1;
      this.rbtnCreateBor.Text = "Create a new contact record for borrower.";
      this.rbtnUpdateBor.Location = new Point(8, 28);
      this.rbtnUpdateBor.Name = "rbtnUpdateBor";
      this.rbtnUpdateBor.Size = new Size(580, 24);
      this.rbtnUpdateBor.TabIndex = 2;
      this.rbtnUpdateBor.Text = "Select a borrower contact from the following matching records to update.";
      this.rbtnUpdateCoBor.Location = new Point(8, 28);
      this.rbtnUpdateCoBor.Name = "rbtnUpdateCoBor";
      this.rbtnUpdateCoBor.Size = new Size(580, 24);
      this.rbtnUpdateCoBor.TabIndex = 5;
      this.rbtnUpdateCoBor.Text = "Select a coborrower contact from the following matching records to update.";
      this.rbtnCreateCoBor.Location = new Point(8, 4);
      this.rbtnCreateCoBor.Name = "rbtnCreateCoBor";
      this.rbtnCreateCoBor.Size = new Size(580, 24);
      this.rbtnCreateCoBor.TabIndex = 4;
      this.rbtnCreateCoBor.Text = "Create a new contact record for coborrower.";
      this.listViewCoBor.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader6,
        this.columnHeader7,
        this.columnHeader8,
        this.columnHeader9,
        this.columnHeader10
      });
      this.listViewCoBor.FullRowSelect = true;
      this.listViewCoBor.GridLines = true;
      this.listViewCoBor.HideSelection = false;
      this.listViewCoBor.Location = new Point(8, 52);
      this.listViewCoBor.MultiSelect = false;
      this.listViewCoBor.Name = "listViewCoBor";
      this.listViewCoBor.Size = new Size(584, 112);
      this.listViewCoBor.TabIndex = 3;
      this.listViewCoBor.UseCompatibleStateImageBehavior = false;
      this.listViewCoBor.View = View.Details;
      this.columnHeader6.Text = "Last Name";
      this.columnHeader6.Width = 119;
      this.columnHeader7.Text = "First Name";
      this.columnHeader7.Width = 95;
      this.columnHeader8.Text = "Company";
      this.columnHeader8.Width = 123;
      this.columnHeader9.Text = "Home Phone";
      this.columnHeader9.Width = 103;
      this.columnHeader10.Text = "Personal Email";
      this.columnHeader10.Width = 147;
      this.btnNext.Location = new Point(444, 372);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new Size(75, 23);
      this.btnNext.TabIndex = 6;
      this.btnNext.Text = "Update";
      this.btnNext.Click += new EventHandler(this.btnNext_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(532, 372);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.panel1.BorderStyle = BorderStyle.Fixed3D;
      this.panel1.Controls.Add((Control) this.listViewBor);
      this.panel1.Controls.Add((Control) this.rbtnCreateBor);
      this.panel1.Controls.Add((Control) this.rbtnUpdateBor);
      this.panel1.Location = new Point(8, 4);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(604, 172);
      this.panel1.TabIndex = 8;
      this.panel2.BorderStyle = BorderStyle.Fixed3D;
      this.panel2.Controls.Add((Control) this.listViewCoBor);
      this.panel2.Controls.Add((Control) this.rbtnUpdateCoBor);
      this.panel2.Controls.Add((Control) this.rbtnCreateCoBor);
      this.panel2.Location = new Point(8, 188);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(604, 172);
      this.panel2.TabIndex = 9;
      this.chkBoxUpdateAddress.Location = new Point(8, 366);
      this.chkBoxUpdateAddress.Name = "chkBoxUpdateAddress";
      this.chkBoxUpdateAddress.Size = new Size(376, 20);
      this.chkBoxUpdateAddress.TabIndex = 14;
      this.chkBoxUpdateAddress.Text = "Update the contact's home address with the subject property address.";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(622, 403);
      this.Controls.Add((Control) this.chkBoxUpdateAddress);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnNext);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SyncContactDialog);
      this.ShowInTaskbar = false;
      this.Text = "Update Contact";
      this.Load += new EventHandler(this.SyncContactDialog_Load);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public ListViewItem addContactToBorListView(BorrowerInfo contact)
    {
      ListViewItem borListView = new ListViewItem(new string[5]
      {
        contact.LastName,
        contact.FirstName,
        contact.EmployerName,
        contact.HomePhone,
        contact.PersonalEmail
      });
      borListView.Tag = (object) contact;
      this.listViewBor.Items.Add(borListView);
      return borListView;
    }

    public ListViewItem addContactToCoborListView(BorrowerInfo contact)
    {
      ListViewItem coborListView = new ListViewItem(new string[5]
      {
        contact.LastName,
        contact.FirstName,
        contact.EmployerName,
        contact.HomePhone,
        contact.PersonalEmail
      });
      coborListView.Tag = (object) contact;
      this.listViewCoBor.Items.Add(coborListView);
      return coborListView;
    }

    private void ListMatchingContacts()
    {
      if (this._loan == null)
        return;
      if (this._loan.CurrentBorrowerPair.Borrower.Id == this._loan.GetBorrowerPairs()[0].Borrower.Id)
        this.isPrimaryPair = true;
      string str1 = this._loan.GetField("LOID").Trim();
      if (str1 == string.Empty)
        return;
      string str2 = this._loan.GetField("37");
      string str3 = this._loan.GetField("36");
      string str4 = this._loan.GetField("69");
      string str5 = this._loan.GetField("68");
      bool flag1 = false;
      bool flag2 = false;
      if (str2 == string.Empty && str3 == string.Empty)
        flag1 = true;
      if (str4 == string.Empty && str5 == string.Empty)
        flag2 = true;
      BorrowerInfo[] borrowerInfoArray1 = (BorrowerInfo[]) null;
      if (!flag1)
      {
        if (str2.Length > 50)
          str2 = str2.Substring(0, 50);
        if (str3.Length > 50)
          str3 = str3.Substring(0, 50);
        borrowerInfoArray1 = Session.ContactManager.QueryAllBorrowers(new QueryCriterion[3]
        {
          (QueryCriterion) new StringValueCriterion("Contact.LastName", str2, StringMatchType.StartsWith),
          (QueryCriterion) new StringValueCriterion("Contact.FirstName", str3, StringMatchType.StartsWith),
          (QueryCriterion) new StringValueCriterion("Contact.OwnerID", str1)
        }, RelatedLoanMatchType.None);
      }
      BorrowerInfo[] borrowerInfoArray2 = (BorrowerInfo[]) null;
      if (!flag2)
      {
        if (str4.Length > 50)
          str4 = str4.Substring(0, 50);
        if (str5.Length > 50)
          str5 = str5.Substring(0, 50);
        borrowerInfoArray2 = Session.ContactManager.QueryAllBorrowers(new QueryCriterion[3]
        {
          (QueryCriterion) new StringValueCriterion("Contact.LastName", str4, StringMatchType.StartsWith),
          (QueryCriterion) new StringValueCriterion("Contact.FirstName", str5, StringMatchType.StartsWith),
          (QueryCriterion) new StringValueCriterion("Contact.OwnerID", str1)
        }, RelatedLoanMatchType.None);
      }
      if (flag1)
      {
        this.rbtnUpdateBor.Checked = false;
        this.rbtnUpdateBor.Enabled = false;
        this.rbtnCreateBor.Checked = false;
        this.rbtnUpdateBor.Enabled = false;
        this.listViewBor.Enabled = false;
      }
      else if (borrowerInfoArray1 != null && borrowerInfoArray1.Length != 0)
      {
        this.rbtnUpdateBor.Checked = true;
        this.rbtnUpdateBor.Enabled = true;
        this.listViewBor.Enabled = true;
        foreach (BorrowerInfo contact in borrowerInfoArray1)
          this.addContactToBorListView(contact);
        this.listViewBor.Items[0].Selected = true;
      }
      else
      {
        this.rbtnCreateBor.Checked = true;
        this.rbtnUpdateBor.Enabled = false;
        this.listViewBor.Enabled = false;
      }
      if (flag2)
      {
        this.rbtnUpdateCoBor.Checked = false;
        this.rbtnUpdateCoBor.Enabled = false;
        this.rbtnCreateCoBor.Checked = false;
        this.rbtnCreateCoBor.Enabled = false;
        this.listViewCoBor.Enabled = false;
      }
      else if (borrowerInfoArray2 != null && borrowerInfoArray2.Length != 0)
      {
        this.rbtnUpdateCoBor.Checked = true;
        this.rbtnUpdateCoBor.Enabled = true;
        this.listViewCoBor.Enabled = true;
        foreach (BorrowerInfo contact in borrowerInfoArray2)
          this.addContactToCoborListView(contact);
        this.listViewCoBor.Items[0].Selected = true;
      }
      else
      {
        this.rbtnCreateCoBor.Checked = true;
        this.rbtnUpdateCoBor.Enabled = false;
        this.listViewCoBor.Enabled = false;
      }
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Borrower_CreateNew))
      {
        if (this.rbtnCreateBor.Enabled)
        {
          this.rbtnCreateBor.Enabled = false;
          this.rbtnCreateBor.Checked = false;
        }
        if (this.rbtnCreateCoBor.Enabled)
        {
          this.rbtnCreateCoBor.Enabled = false;
          this.rbtnCreateCoBor.Checked = false;
        }
      }
      if (!this.rbtnCreateBor.Enabled && !this.rbtnCreateCoBor.Enabled && !this.rbtnUpdateBor.Enabled && !this.rbtnUpdateCoBor.Enabled)
        this.btnNext.Enabled = false;
      else
        this.btnNext.Enabled = true;
    }

    private BorrowerInfo UpdateBorContact()
    {
      if (this.listViewBor.SelectedItems.Count != 1)
        return (BorrowerInfo) null;
      BorrowerInfo tag = (BorrowerInfo) this.listViewBor.SelectedItems[0].Tag;
      this.GetBorrowerInfoFromLoan(this._loan, tag);
      tag.PrimaryContact = this.isPrimaryPair;
      Session.ContactManager.UpdateBorrower(tag);
      return tag;
    }

    private BorrowerInfo CreateBorContact()
    {
      BorrowerInfo borrowerInfoFromLoan = this.CreateBorrowerInfoFromLoan(this._loan);
      borrowerInfoFromLoan.OwnerID = this._loan.GetField("LOID").Trim();
      borrowerInfoFromLoan.AccessLevel = ContactAccess.Private;
      borrowerInfoFromLoan.PrimaryContact = this.isPrimaryPair;
      int borrower = Session.ContactManager.CreateBorrower(borrowerInfoFromLoan, DateTime.Now, ContactSource.Entered);
      borrowerInfoFromLoan.ContactID = borrower;
      return borrowerInfoFromLoan;
    }

    private BorrowerInfo CreateBorrowerInfoFromLoan(LoanData loan)
    {
      BorrowerInfo contact = new BorrowerInfo();
      this.GetBorrowerInfoFromLoan(loan, contact);
      return contact;
    }

    private void GetBorrowerInfoFromLoan(LoanData loan, BorrowerInfo contact)
    {
      contact.FirstName = loan.GetField("36");
      contact.LastName = loan.GetField("37");
      if (Session.LoanData.GetField("2821") != "Y")
      {
        contact.HomeAddress.Street1 = loan.GetField("FR0104");
        contact.HomeAddress.City = loan.GetField("FR0106");
        contact.HomeAddress.State = loan.GetField("FR0107");
        contact.HomeAddress.Zip = loan.GetField("FR0108");
      }
      else if (this.chkBoxUpdateAddress.Checked)
      {
        contact.HomeAddress.Street1 = loan.GetField("11");
        contact.HomeAddress.Street2 = string.Empty;
        contact.HomeAddress.City = loan.GetField("12");
        contact.HomeAddress.State = loan.GetField("14");
        contact.HomeAddress.Zip = loan.GetField("15");
      }
      contact.HomePhone = loan.GetField("66");
      contact.MobilePhone = loan.GetField("1490");
      contact.BizEmail = loan.GetField("1178");
      contact.PersonalEmail = loan.GetField("1240");
      contact.FaxNumber = loan.GetField("1188");
      contact.BizAddress.Street1 = loan.GetField("FE0104");
      contact.BizAddress.City = loan.GetField("FE0105");
      contact.BizAddress.State = loan.GetField("FE0106");
      contact.BizAddress.Zip = loan.GetField("FE0107");
      contact.EmployerName = loan.GetField("FE0102");
      contact.JobTitle = loan.GetField("FE0110");
      contact.WorkPhone = loan.GetField("FE0117");
      contact.SSN = loan.GetField("65");
      contact.Referral = loan.GetField("1822");
      try
      {
        Decimal num = Convert.ToDecimal(loan.GetField("910"));
        contact.Income = num * 12M;
      }
      catch
      {
      }
      try
      {
        string field = loan.GetField("1402");
        DateTime dateTime = Convert.ToDateTime(field);
        if (dateTime > Utils.DbMinSmallDate)
        {
          if (dateTime < Utils.DbMaxSmallDate)
          {
            if (field.IndexOf(dateTime.Year.ToString()) == -1)
            {
              int year1 = dateTime.Year;
              DateTime today = DateTime.Today;
              int year2 = today.Year;
              if (year1 >= year2)
              {
                int num = dateTime.Year - 100;
                today = DateTime.Today;
                int year3 = today.Year;
                if (num < year3)
                  dateTime = dateTime.AddYears(-100);
              }
            }
            contact.Birthdate = dateTime;
          }
        }
      }
      catch
      {
      }
      string field1 = loan.GetField("52");
      string field2 = loan.GetField("84");
      if (field1 == "Married")
      {
        contact.Married = true;
        if (!(field2 == "Married"))
          return;
        contact.SpouseName = loan.GetField("68") + " " + loan.GetField("69");
      }
      else
        contact.Married = false;
    }

    private BorrowerInfo UpdateCoborContact()
    {
      if (this.listViewCoBor.SelectedItems.Count != 1)
        return (BorrowerInfo) null;
      BorrowerInfo tag = (BorrowerInfo) this.listViewCoBor.SelectedItems[0].Tag;
      this.GetCoborrowerInfoFromLoan(this._loan, tag);
      tag.PrimaryContact = false;
      Session.ContactManager.UpdateBorrower(tag);
      return tag;
    }

    private BorrowerInfo CreateCoborContact()
    {
      BorrowerInfo coborrowerInfoFromLoan = this.CreateCoborrowerInfoFromLoan(this._loan);
      coborrowerInfoFromLoan.OwnerID = this._loan.GetField("LOID").Trim();
      coborrowerInfoFromLoan.AccessLevel = ContactAccess.Private;
      coborrowerInfoFromLoan.PrimaryContact = false;
      int borrower = Session.ContactManager.CreateBorrower(coborrowerInfoFromLoan, DateTime.Now, ContactSource.Entered);
      coborrowerInfoFromLoan.ContactID = borrower;
      return coborrowerInfoFromLoan;
    }

    private BorrowerInfo CreateCoborrowerInfoFromLoan(LoanData loan)
    {
      BorrowerInfo contact = new BorrowerInfo();
      this.GetCoborrowerInfoFromLoan(loan, contact);
      return contact;
    }

    private void GetCoborrowerInfoFromLoan(LoanData loan, BorrowerInfo contact)
    {
      contact.FirstName = loan.GetField("68");
      contact.LastName = loan.GetField("69");
      if (Session.LoanData.GetField("2821") != "Y")
      {
        contact.HomeAddress.Street1 = loan.GetField("FR0204");
        contact.HomeAddress.City = loan.GetField("FR0206");
        contact.HomeAddress.State = loan.GetField("FR0207");
        contact.HomeAddress.Zip = loan.GetField("FR0208");
      }
      else if (this.chkBoxUpdateAddress.Checked)
      {
        contact.HomeAddress.Street1 = loan.GetField("11");
        contact.HomeAddress.Street2 = string.Empty;
        contact.HomeAddress.City = loan.GetField("12");
        contact.HomeAddress.State = loan.GetField("14");
        contact.HomeAddress.Zip = loan.GetField("15");
      }
      contact.HomePhone = loan.GetField("98");
      contact.MobilePhone = loan.GetField("1480");
      contact.BizEmail = loan.GetField("1179");
      contact.PersonalEmail = loan.GetField("1268");
      contact.FaxNumber = loan.GetField("1241");
      contact.BizAddress.Street1 = loan.GetField("FE0204");
      contact.BizAddress.City = loan.GetField("FE0205");
      contact.BizAddress.State = loan.GetField("FE0206");
      contact.BizAddress.Zip = loan.GetField("FE0207");
      contact.EmployerName = loan.GetField("FE0202");
      contact.JobTitle = loan.GetField("FE0210");
      contact.WorkPhone = loan.GetField("FE0217");
      contact.SSN = loan.GetField("97");
      contact.Referral = loan.GetField("1822");
      try
      {
        Decimal num = Convert.ToDecimal(loan.GetField("911"));
        contact.Income = num * 12M;
      }
      catch
      {
      }
      try
      {
        string field = loan.GetField("1403");
        DateTime dateTime = Convert.ToDateTime(field);
        if (dateTime > Utils.DbMinSmallDate)
        {
          if (dateTime < Utils.DbMaxSmallDate)
          {
            if (field.IndexOf(dateTime.Year.ToString()) == -1)
            {
              int year1 = dateTime.Year;
              DateTime today = DateTime.Today;
              int year2 = today.Year;
              if (year1 >= year2)
              {
                int num = dateTime.Year - 100;
                today = DateTime.Today;
                int year3 = today.Year;
                if (num < year3)
                  dateTime = dateTime.AddYears(-100);
              }
            }
            contact.Birthdate = dateTime;
          }
        }
      }
      catch
      {
      }
      string field1 = loan.GetField("52");
      if (loan.GetField("84") == "Married")
      {
        contact.Married = true;
        if (!(field1 == "Married"))
          return;
        contact.SpouseName = loan.GetField("36") + " " + loan.GetField("37");
      }
      else
        contact.Married = false;
    }

    private void SyncContactDialog_Load(object sender, EventArgs e) => this.ListMatchingContacts();

    private void btnNext_Click(object sender, EventArgs e)
    {
      if (this.rbtnCreateBor.Checked)
      {
        BorrowerInfo borContact = this.CreateBorContact();
        if (borContact != null)
          this.updateContactCustomFields(borContact);
      }
      if (this.rbtnUpdateBor.Checked)
      {
        BorrowerInfo contact = this.UpdateBorContact();
        if (contact != null)
          this.updateContactCustomFields(contact);
      }
      if (this.rbtnCreateCoBor.Checked)
      {
        BorrowerInfo coborContact = this.CreateCoborContact();
        if (coborContact != null)
          this.updateContactCustomFields(coborContact);
      }
      if (this.rbtnUpdateCoBor.Checked)
      {
        BorrowerInfo contact = this.UpdateCoborContact();
        if (contact != null)
          this.updateContactCustomFields(contact);
      }
      int num = (int) MessageBox.Show((IWin32Window) this, "Borrower and Coborrower data in loan file is synchronized with Contact database.", "Synchronization Complete", MessageBoxButtons.OK);
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void updateContactCustomFields(BorrowerInfo contact)
    {
      Tracing.Log(SyncContactDialog.sw, TraceLevel.Verbose, "Custom Field Mapping", string.Format("Entering Input.InputEngine.SyncContactDialog.updateContactCustomFields(contact.FirstName='{0}', contact.LastName='{1}')", (object) contact.FirstName, (object) contact.LastName));
      if (this.contactCustomFields == null)
      {
        this.contactCustomFields = new List<ContactCustomField>();
        CustomFieldMappingCollection mappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(Session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.Borrower, true));
        if (mappingCollection.Count == 0)
          return;
        foreach (CustomFieldMapping customFieldMapping in (CollectionBase) mappingCollection)
        {
          string fieldValue = (string) null;
          try
          {
            fieldValue = Session.LoanData.GetField(customFieldMapping.LoanFieldId);
            Tracing.Log(SyncContactDialog.sw, TraceLevel.Verbose, "Custom Field Mapping", string.Format("CustomFieldMapping: CategoryId='{0}', FieldNumber='{1}', FieldFormat='{2}', LoanFieldId='{3}', FieldValue='{4}'", (object) customFieldMapping.CategoryId, (object) customFieldMapping.FieldNumber, (object) customFieldMapping.FieldFormat, (object) customFieldMapping.LoanFieldId, (object) fieldValue));
          }
          catch (Exception ex)
          {
            Tracing.Log(SyncContactDialog.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Loan Field ID '{2}', Value '{1}' to Business Contact 'Custom Field {0}' failed.", (object) customFieldMapping.FieldNumber.ToString(), fieldValue == null ? (object) "UNKNOWN" : (object) fieldValue, (object) customFieldMapping.LoanFieldId));
            fieldValue = (string) null;
          }
          if (fieldValue != null)
            this.contactCustomFields.Add(new ContactCustomField(contact.ContactID, customFieldMapping.FieldNumber, contact.OwnerID, fieldValue));
        }
      }
      if (this.contactCustomFields.Count == 0)
        return;
      foreach (ContactCustomField contactCustomField in this.contactCustomFields)
      {
        contactCustomField.ContactID = contact.ContactID;
        contactCustomField.OwnerID = contact.OwnerID;
      }
      Session.ContactManager.UpdateCustomFieldsForContact(contact.ContactID, ContactType.Borrower, this.contactCustomFields.ToArray());
    }
  }
}
