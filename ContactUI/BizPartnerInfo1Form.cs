// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizPartnerInfo1Form
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizPartnerInfo1Form : Form, IBindingForm
  {
    private bool changed;
    private bool intermidiateData;
    private bool deleteBackKey;
    private bool initialLoad = true;
    private bool refreshWOSave;
    private ContactType sourceCT = ContactType.PublicBiz;
    private BizPartnerInfo contactInfo;
    private CustomFieldsDefinition customFieldsDefinition;
    private CustomFieldValueCollection customFieldValues;
    private GroupContainer gcPersonalInfo;
    private GroupContainer gcBusinessInfo;
    private GroupContainer gcBizCatAddFields;
    private const bool CLEARTEXT = true;
    private const bool HIDECATEGORY = true;
    private bool populatingControls;
    private bool hasCategoryFields;
    private Panel pnlMiddle;
    private TextBox txtBoxBizState;
    private TextBox txtBoxBizZip;
    private Label lblBizZip;
    private Label lblBizState;
    private TextBox txtBoxBizCity;
    private TextBox txtBoxBizAddress2;
    private Label lblBizCity;
    private Label lblBizAddress2;
    private Label lblBizAddress1;
    private TextBox txtBoxBizAddress1;
    private Label lblWebUrl;
    private TextBox txtBoxBizWebUrl;
    private TextBox txtBoxJobTitle;
    private Label lblTitle;
    private TextBox txtBoxFees;
    private Label lblFees;
    private Label lblFieldDescription09;
    private ComboBox cboFieldValue09;
    private Label lblFieldDescription08;
    private ComboBox cboFieldValue08;
    private Label lblFieldDescription07;
    private Label lblFieldDescription06;
    private ComboBox cboFieldValue07;
    private ComboBox cboFieldValue06;
    private Label lblFieldDescription05;
    private Label lblFieldDescription04;
    private Label lblFieldDescription03;
    private ComboBox cboFieldValue05;
    private ComboBox cboFieldValue04;
    private ComboBox cboFieldValue03;
    private ComboBox cboFieldValue02;
    private ComboBox cboFieldValue01;
    private Label lblFieldDescription02;
    private Label lblFieldDescription01;
    private Label lblCategoryTitle;
    private Label[] lblFieldDescriptions = new Label[11];
    private ComboBox[] cboFieldValues = new ComboBox[11];
    private CheckBox chkBoxAccess;
    private StandardIconButton siBtnWorkEmail;
    private StandardIconButton siBtnHomeEmail;
    private StandardIconButton siBtnFaxNumber;
    private StandardIconButton siBtnCellPhone;
    private StandardIconButton siBtnWorkPhone;
    private StandardIconButton siBtnHomePhone;
    private int curDisplayedCatID = -1;
    private Label lblFieldDescription10;
    private ComboBox cboFieldValue10;
    private Label lblFieldDescription11;
    private ComboBox cboFieldValue11;
    private Panel panel1;
    private TextBox txtBoxPersonalInfoLicAuthNAme;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label3;
    private Label lblLicenseNum;
    private TextBox txtBoxLicenseNumber;
    private Label label2;
    private Label label4;
    private Label label8;
    private TextBox txtBoxBizAuthName;
    private Label label10;
    private TextBox txtBoxPersonalInfoLicNumber;
    private Label label9;
    private DatePicker dpBizLicIssueDate;
    private DatePicker dpPersonalInfoLicIssueDate;
    private ComboBox comboBoxBizInfoLicState;
    private ComboBox comboBoxPersonalInfoLicState;
    private ComboBox comboBoxBizInfoLicAuthType;
    private ComboBox comboBoxPersonalInfoLicAuthType;
    private bool isReadOnly;
    private BizCategoryUtil catUtil;
    private BizPartnerTabForm tabForm;
    private IContainer components;
    private TextBox txtBoxLastName;
    private Label lblLastName;
    private Label lblFirstName;
    private TextBox txtBoxFirstName;
    private Label lblCategory;
    private Label lblCompany;
    private TextBox txtBoxCompanyName;
    private CheckBox chkBoxNoSpam;
    private ComboBox cmbBoxCategoryID;
    private Label lblWorkEmail;
    private Label lblFaxNumber;
    private Label lblCellPhone;
    private Label lblWorkPhone;
    private Label lblHomeEmail;
    private Label lblHomePhone;
    private TextBox txtBoxWorkPhone;
    private TextBox txtBoxBizEmail;
    private TextBox txtBoxMobilePhone;
    private TextBox txtBoxFaxNumber;
    private TextBox txtBoxPersonalEmail;
    private TextBox txtBoxHomePhone;
    private TextBox txtBoxSalutation;
    private Label label1;
    private ToolTip toolTip;

    public event EventHandler SummaryChanged;

    public event BizPartnerInfo1Form.CategoryChanged CategoryChangedEvent;

    public bool IsReadOnly
    {
      get => this.isReadOnly;
      set
      {
        this.isReadOnly = value;
        if (this.isReadOnly)
          this.disableControls();
        else
          this.enableControls();
      }
    }

    public bool isDirty() => this.changed;

    public BizPartnerInfo1Form(BizPartnerTabForm tabForm, ContactType source)
    {
      this.InitializeComponent();
      this.tabForm = tabForm;
      this.sourceCT = source;
      this.hasCategoryFields = false;
      this.initializeControls();
      this.Init();
    }

    private void initializeControls()
    {
      this.lblFieldDescriptions[0] = this.lblFieldDescription01;
      this.lblFieldDescriptions[1] = this.lblFieldDescription02;
      this.lblFieldDescriptions[2] = this.lblFieldDescription03;
      this.lblFieldDescriptions[3] = this.lblFieldDescription04;
      this.lblFieldDescriptions[4] = this.lblFieldDescription05;
      this.lblFieldDescriptions[5] = this.lblFieldDescription06;
      this.lblFieldDescriptions[6] = this.lblFieldDescription07;
      this.lblFieldDescriptions[7] = this.lblFieldDescription08;
      this.lblFieldDescriptions[8] = this.lblFieldDescription09;
      this.lblFieldDescriptions[9] = this.lblFieldDescription10;
      this.lblFieldDescriptions[10] = this.lblFieldDescription11;
      this.cboFieldValues[0] = this.cboFieldValue01;
      this.cboFieldValues[1] = this.cboFieldValue02;
      this.cboFieldValues[2] = this.cboFieldValue03;
      this.cboFieldValues[3] = this.cboFieldValue04;
      this.cboFieldValues[4] = this.cboFieldValue05;
      this.cboFieldValues[5] = this.cboFieldValue06;
      this.cboFieldValues[6] = this.cboFieldValue07;
      this.cboFieldValues[7] = this.cboFieldValue08;
      this.cboFieldValues[8] = this.cboFieldValue09;
      this.cboFieldValues[9] = this.cboFieldValue10;
      this.cboFieldValues[10] = this.cboFieldValue11;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public int CurrentContactID
    {
      get => this.contactInfo != null ? this.contactInfo.ContactID : -1;
      set
      {
        if (this.CurrentContactID == value)
          return;
        this.contactInfo = (BizPartnerInfo) null;
        this.customFieldValues = (CustomFieldValueCollection) null;
        this.customFieldsDefinition = (CustomFieldsDefinition) null;
        this.customFieldValues = (CustomFieldValueCollection) null;
        this.setControlState(this.isReadOnly || -1 == this.CurrentContactID, true, true);
        this.disableControls();
        if (value < 0)
          return;
        this.contactInfo = Session.ContactManager.GetBizPartner(value);
        if (this.contactInfo == null)
          throw new ObjectNotFoundException("Unable to retrieve business contact.", ObjectType.Contact, (object) value);
        this.loadForm();
      }
    }

    public object CurrentContact
    {
      get => (object) this.contactInfo;
      set
      {
        if (this.CurrentContact == value)
          return;
        this.contactInfo = (BizPartnerInfo) null;
        this.customFieldValues = (CustomFieldValueCollection) null;
        this.customFieldsDefinition = (CustomFieldsDefinition) null;
        this.customFieldValues = (CustomFieldValueCollection) null;
        this.setControlState(this.isReadOnly || -1 == this.CurrentContactID, true, true);
        this.disableControls();
        if (value == null)
          return;
        this.contactInfo = (BizPartnerInfo) value;
        this.loadForm();
      }
    }

    public BizPartnerInfo ResetBizPartnerInfo
    {
      set => this.contactInfo.OwnerID = value.OwnerID;
    }

    public bool SaveChanges()
    {
      if (!this.changed || this.contactInfo == null)
        return false;
      if (this.txtBoxPersonalEmail.Text.Trim() != string.Empty && !Utils.ValidateEmail(this.txtBoxPersonalEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid email address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxPersonalEmail.Focus();
        return false;
      }
      if (this.txtBoxBizEmail.Text.Trim() != string.Empty && !Utils.ValidateEmail(this.txtBoxBizEmail.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid email address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxBizEmail.Focus();
        return false;
      }
      this.contactInfo = Session.ContactManager.GetBizPartner(this.CurrentContactID);
      this.contactInfo.CategoryID = this.getSelectedCategory();
      this.contactInfo.FaxNumber = this.txtBoxFaxNumber.Text;
      this.contactInfo.HomePhone = this.txtBoxHomePhone.Text;
      this.contactInfo.MobilePhone = this.txtBoxMobilePhone.Text;
      this.contactInfo.PersonalEmail = this.txtBoxPersonalEmail.Text;
      this.contactInfo.BizEmail = this.txtBoxBizEmail.Text;
      this.contactInfo.WorkPhone = this.txtBoxWorkPhone.Text;
      this.contactInfo.LastName = this.txtBoxLastName.Text;
      this.contactInfo.Salutation = this.txtBoxSalutation.Text;
      this.contactInfo.FirstName = this.txtBoxFirstName.Text;
      this.contactInfo.NoSpam = this.chkBoxNoSpam.Checked;
      this.contactInfo.CompanyName = this.txtBoxCompanyName.Text;
      this.contactInfo.AccessLevel = !this.chkBoxAccess.Checked ? ContactAccess.Private : ContactAccess.Public;
      this.contactInfo.BizAddress.State = this.txtBoxBizState.Text;
      this.contactInfo.BizAddress.Zip = this.txtBoxBizZip.Text;
      this.contactInfo.BizAddress.City = this.txtBoxBizCity.Text;
      this.contactInfo.BizAddress.Street2 = this.txtBoxBizAddress2.Text;
      this.contactInfo.BizAddress.Street1 = this.txtBoxBizAddress1.Text;
      this.contactInfo.BizWebUrl = this.txtBoxBizWebUrl.Text;
      this.contactInfo.Fees = Utils.ParseInt((object) this.txtBoxFees.Text);
      this.contactInfo.JobTitle = this.txtBoxJobTitle.Text;
      this.contactInfo.LicenseNumber = this.txtBoxLicenseNumber.Text;
      this.contactInfo.PersonalInfoLicense = new BizContactLicenseInfo(this.txtBoxPersonalInfoLicNumber.Text, this.txtBoxPersonalInfoLicAuthNAme.Text, this.comboBoxPersonalInfoLicAuthType.Text, this.comboBoxPersonalInfoLicState.Text, this.dpPersonalInfoLicIssueDate.Value);
      this.contactInfo.BizContactLicense = new BizContactLicenseInfo(this.txtBoxLicenseNumber.Text, this.txtBoxBizAuthName.Text, this.comboBoxBizInfoLicAuthType.Text, this.comboBoxBizInfoLicState.Text, this.dpBizLicIssueDate.Value);
      try
      {
        Session.ContactManager.UpdateBizPartner(this.contactInfo);
        if (this.hasCategoryFields)
          this.saveCategoryFields();
      }
      catch (Exception ex)
      {
        throw new ObjectNotFoundException("Unable to update business contact '" + this.contactInfo.LastName + ", " + this.contactInfo.FirstName + "' (ContactID:" + (object) this.contactInfo.ContactID + ").", ObjectType.Contact, (object) this.contactInfo.ContactID);
      }
      this.changed = false;
      return true;
    }

    private bool saveCategoryFields()
    {
      for (int index = 0; index < this.customFieldsDefinition.CustomFieldDefinitions.Count; ++index)
      {
        string str = this.cboFieldValues[index].Text.Trim();
        int fieldId = ((CustomFieldDefinition) this.cboFieldValues[index].Tag).FieldId;
        if (this.customFieldValues.Contains(fieldId))
        {
          CustomFieldValue customFieldValue = this.customFieldValues.Find(fieldId);
          if (string.Empty != str)
            customFieldValue.FieldValue = str;
          else
            customFieldValue.Delete();
        }
        else if (string.Empty != str)
        {
          FieldFormat fieldFormat = ((CustomFieldDefinition) this.cboFieldValues[index].Tag).FieldFormat;
          CustomFieldValue customFieldValue = CustomFieldValue.NewCustomFieldValue(fieldId, this.CurrentContactID, fieldFormat);
          customFieldValue.FieldValue = str;
          this.customFieldValues.Add(customFieldValue);
        }
      }
      if (this.customFieldValues != null)
      {
        if (this.customFieldValues.IsDirty)
        {
          try
          {
            this.customFieldValues = this.customFieldValues.Save();
          }
          catch (Exception ex)
          {
            throw new ObjectNotFoundException("Unable to update category custom fields for business contact '" + this.contactInfo.LastName + ", " + this.contactInfo.FirstName + "'.", ObjectType.Contact, (object) this.contactInfo.ContactID);
          }
          this.loadForm();
          return true;
        }
      }
      return false;
    }

    private void cboFieldValue_Changed(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.changed = true;
      ComboBox cboFieldValue = (ComboBox) sender;
      FieldFormat fieldFormat = ((CustomFieldDefinition) cboFieldValue.Tag).FieldFormat;
      this.formatFieldValue(cboFieldValue, fieldFormat);
      if (this.SummaryChanged == null)
        return;
      this.SummaryChanged((object) null, (EventArgs) null);
    }

    private void formatFieldValue(ComboBox cboFieldValue, FieldFormat fieldFormat)
    {
      if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
        if (FieldFormat.SSN == fieldFormat)
          return;
      }
      bool needsUpdate = false;
      int newCursorPos = 0;
      string str = Utils.FormatInput(cboFieldValue.Text, fieldFormat, ref needsUpdate, cboFieldValue.SelectionStart, ref newCursorPos);
      if (!needsUpdate)
        return;
      cboFieldValue.Text = str;
      cboFieldValue.SelectionStart = newCursorPos;
    }

    private void txtBoxBizZip_Leave(object sender, EventArgs e)
    {
      if (this.txtBoxBizZip.Text == "")
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(this.txtBoxBizZip.Text, ZipCodeUtils.GetMultipleZipInfoAt(this.txtBoxBizZip.Text));
      if (zipCodeInfo == null)
        return;
      this.txtBoxBizCity.Text = zipCodeInfo.City;
      this.txtBoxBizState.Text = zipCodeInfo.State;
    }

    private void cboFieldValue_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.populatingControls || Keys.Back != e.KeyCode && Keys.Delete != e.KeyCode)
        return;
      this.deleteBackKey = true;
    }

    private void loadForm()
    {
      this.initialLoad = true;
      this.loadCategoryFields(this.contactInfo.CategoryID);
      if (!this.IsReadOnly)
      {
        this.enableControls();
        this.setControlState(false, false, false);
      }
      this.comboBoxBizInfoLicState.Items.Clear();
      this.comboBoxPersonalInfoLicState.Items.Clear();
      this.comboBoxBizInfoLicState.Items.AddRange((object[]) Utils.GetStates());
      this.comboBoxPersonalInfoLicState.Items.AddRange((object[]) Utils.GetStates());
      this.setSelectedCategory(this.contactInfo.CategoryID);
      this.txtBoxFaxNumber.Text = this.contactInfo.FaxNumber;
      this.txtBoxHomePhone.Text = this.contactInfo.HomePhone;
      this.txtBoxMobilePhone.Text = this.contactInfo.MobilePhone;
      this.txtBoxPersonalEmail.Text = this.contactInfo.PersonalEmail;
      this.txtBoxBizEmail.Text = this.contactInfo.BizEmail;
      this.txtBoxWorkPhone.Text = this.contactInfo.WorkPhone;
      this.txtBoxLastName.Text = this.contactInfo.LastName;
      this.txtBoxSalutation.Text = this.contactInfo.Salutation;
      this.txtBoxFirstName.Text = this.contactInfo.FirstName;
      this.chkBoxNoSpam.Checked = this.contactInfo.NoSpam;
      this.txtBoxCompanyName.Text = this.contactInfo.CompanyName;
      this.chkBoxAccess.Checked = this.contactInfo.AccessLevel == ContactAccess.Public;
      this.txtBoxBizState.Text = this.contactInfo.BizAddress.State;
      this.txtBoxBizZip.Text = this.contactInfo.BizAddress.Zip;
      this.txtBoxBizCity.Text = this.contactInfo.BizAddress.City;
      this.txtBoxBizAddress2.Text = this.contactInfo.BizAddress.Street2;
      this.txtBoxBizAddress1.Text = this.contactInfo.BizAddress.Street1;
      this.txtBoxBizWebUrl.Text = this.contactInfo.BizWebUrl;
      this.txtBoxFees.Text = this.contactInfo.Fees >= 0 ? this.contactInfo.Fees.ToString() : "";
      this.txtBoxJobTitle.Text = this.contactInfo.JobTitle;
      this.txtBoxLicenseNumber.Text = this.contactInfo.LicenseNumber;
      this.populateCategoryFields();
      this.txtBoxPersonalInfoLicAuthNAme.Text = this.contactInfo.PersonalInfoLicense.LicenseAuthName;
      this.comboBoxPersonalInfoLicState.Text = this.contactInfo.PersonalInfoLicense.LicenseStateCode;
      this.comboBoxPersonalInfoLicAuthType.Text = this.contactInfo.PersonalInfoLicense.LicenseAuthType;
      this.dpPersonalInfoLicIssueDate.Value = !(this.contactInfo.PersonalInfoLicense.LicenseIssueDate != DateTime.MinValue) ? new DateTime() : this.contactInfo.PersonalInfoLicense.LicenseIssueDate;
      this.txtBoxPersonalInfoLicNumber.Text = this.contactInfo.PersonalInfoLicense.LicenseNumber;
      this.txtBoxBizAuthName.Text = this.contactInfo.BizContactLicense.LicenseAuthName;
      this.comboBoxBizInfoLicState.Text = this.contactInfo.BizContactLicense.LicenseStateCode;
      this.comboBoxBizInfoLicAuthType.Text = this.contactInfo.BizContactLicense.LicenseAuthType;
      this.dpBizLicIssueDate.Value = !(this.contactInfo.BizContactLicense.LicenseIssueDate != DateTime.MinValue) ? new DateTime() : this.contactInfo.BizContactLicense.LicenseIssueDate;
      this.siBtnHomePhone.Enabled = true;
      this.siBtnWorkPhone.Enabled = true;
      this.siBtnCellPhone.Enabled = true;
      this.siBtnFaxNumber.Enabled = true;
      this.siBtnHomeEmail.Enabled = true;
      this.siBtnWorkEmail.Enabled = true;
      this.changed = false;
      this.initialLoad = false;
    }

    private void loadCategoryFields(int categoryID)
    {
      this.customFieldsDefinition = CustomFieldsDefinition.GetCustomFieldsDefinition(Session.SessionObjects, CustomFieldsType.BizCategoryStandard, categoryID);
      if (0 < this.customFieldsDefinition.CustomFieldDefinitions.Count)
      {
        this.hasCategoryFields = true;
        this.customFieldValues = this.contactInfo == null ? (CustomFieldValueCollection) null : CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(this.contactInfo.ContactID, categoryID));
        this.lblCategoryTitle.Text = new BizCategoryUtil(Session.SessionObjects).CategoryIdToName(categoryID);
        if (this.lblCategoryTitle.Text.ToLower() == "lender" || this.lblCategoryTitle.Text.ToLower() == "investor")
          this.lblCategoryTitle.Text += " Mortgagee Clause";
        else
          this.lblCategoryTitle.Text += " category fields:";
      }
      else
        this.lblCategoryTitle.Text = "There are no additional fields for this category.";
    }

    private void fitLabelText(Label label, string text)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        if (Utils.FitLabelText(graphics, label, text))
          this.toolTip.SetToolTip((Control) label, string.Empty);
        else
          this.toolTip.SetToolTip((Control) label, Utils.FitToolTipText(graphics, label.Font, 400f, text));
      }
    }

    private void populateCategoryFields()
    {
      int index = 0;
      foreach (CustomFieldDefinition customFieldDefinition in (CollectionBase) this.customFieldsDefinition.CustomFieldDefinitions)
      {
        this.fitLabelText(this.lblFieldDescriptions[index], customFieldDefinition.FieldDescription);
        this.lblFieldDescriptions[index].Visible = true;
        if (FieldFormat.DROPDOWN == customFieldDefinition.FieldFormat)
        {
          this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.DropDown;
          this.cboFieldValues[index].Items.Clear();
          this.cboFieldValues[index].Items.AddRange((object[]) customFieldDefinition.CustomFieldOptions.GetOptionValues());
        }
        else if (FieldFormat.DROPDOWNLIST == customFieldDefinition.FieldFormat)
        {
          this.cboFieldValues[index].DropDownStyle = ComboBoxStyle.DropDownList;
          this.cboFieldValues[index].Items.Clear();
          this.cboFieldValues[index].Items.Add((object) "");
          this.cboFieldValues[index].Items.AddRange((object[]) customFieldDefinition.CustomFieldOptions.GetOptionValues());
        }
        else if (FieldFormat.ZIPCODE == customFieldDefinition.FieldFormat && FieldFormat.ZIPCODE == customFieldDefinition.FieldFormat)
        {
          this.cboFieldValues[index].Leave -= new EventHandler(this.categoryFieldZip_Leave);
          this.cboFieldValues[index].Leave += new EventHandler(this.categoryFieldZip_Leave);
        }
        if (this.customFieldValues != null && this.customFieldValues.Contains(customFieldDefinition.FieldId))
          this.cboFieldValues[index].Text = this.customFieldValues.Find(customFieldDefinition.FieldId).FieldValue;
        this.cboFieldValues[index].Tag = (object) customFieldDefinition;
        this.toolTip.SetToolTip((Control) this.cboFieldValues[index], FieldFormatEnumUtil.ValueToName(customFieldDefinition.FieldFormat));
        this.cboFieldValues[index].Visible = true;
        if (customFieldDefinition.FieldDescription == "Investor Code")
          this.cboFieldValues[index].MaxLength = 64;
        ++index;
      }
    }

    private void categoryFieldZip_Leave(object sender, EventArgs e)
    {
      ComboBox comboBox1 = (ComboBox) sender;
      CustomFieldDefinition tag = (CustomFieldDefinition) comboBox1.Tag;
      if (tag == null || tag.FieldFormat != FieldFormat.ZIPCODE)
        return;
      ComboBox comboBox2 = (ComboBox) null;
      ComboBox comboBox3 = (ComboBox) null;
      for (int index = 0; index < this.cboFieldValues.Length; ++index)
      {
        if (((CustomFieldDefinition) this.cboFieldValues[index].Tag).FieldFormat == FieldFormat.STATE)
        {
          comboBox3 = this.cboFieldValues[index];
          if (index - 1 >= 0)
          {
            comboBox2 = this.cboFieldValues[index - 1];
            break;
          }
          break;
        }
      }
      if (comboBox1.Text.Length < 5)
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(comboBox1.Text.Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(comboBox1.Text.Substring(0, 5)));
      if (zipCodeInfo == null)
        return;
      if (comboBox3 != null)
        comboBox3.Text = zipCodeInfo.State;
      if (comboBox2 == null)
        return;
      comboBox2.Text = zipCodeInfo.City;
    }

    private void clearForm()
    {
      this.cmbBoxCategoryID.SelectedIndex = -1;
      this.curDisplayedCatID = -1;
      this.txtBoxFaxNumber.Text = "";
      this.txtBoxHomePhone.Text = "";
      this.txtBoxMobilePhone.Text = "";
      this.txtBoxPersonalEmail.Text = "";
      this.txtBoxBizEmail.Text = "";
      this.txtBoxWorkPhone.Text = "";
      this.txtBoxLastName.Text = "";
      this.txtBoxSalutation.Text = "";
      this.txtBoxFirstName.Text = "";
      this.chkBoxNoSpam.Checked = false;
      this.chkBoxAccess.Checked = false;
      this.txtBoxCompanyName.Text = "";
      this.txtBoxPersonalInfoLicAuthNAme.Text = "";
      this.comboBoxPersonalInfoLicState.SelectedIndex = -1;
      this.comboBoxPersonalInfoLicAuthType.SelectedIndex = -1;
      this.dpBizLicIssueDate.Value = new DateTime();
      this.txtBoxPersonalInfoLicNumber.Text = "";
      this.comboBoxBizInfoLicAuthType.SelectedIndex = -1;
      this.txtBoxBizAuthName.Text = "";
      this.dpPersonalInfoLicIssueDate.Value = new DateTime();
      this.comboBoxBizInfoLicState.SelectedIndex = -1;
      this.changed = false;
    }

    public void disableForm()
    {
      this.disableControlsOnly();
      this.setControlState(true, false, false);
    }

    private void disableControlsOnly()
    {
      this.cmbBoxCategoryID.Enabled = false;
      this.txtBoxFaxNumber.ReadOnly = true;
      this.txtBoxHomePhone.ReadOnly = true;
      this.txtBoxMobilePhone.ReadOnly = true;
      this.txtBoxPersonalEmail.ReadOnly = true;
      this.txtBoxBizEmail.ReadOnly = true;
      this.txtBoxWorkPhone.ReadOnly = true;
      this.txtBoxLastName.ReadOnly = true;
      this.txtBoxSalutation.ReadOnly = true;
      this.txtBoxFirstName.ReadOnly = true;
      this.chkBoxNoSpam.Enabled = false;
      this.chkBoxAccess.Enabled = false;
      this.txtBoxCompanyName.ReadOnly = true;
      this.siBtnHomePhone.Enabled = false;
      this.siBtnWorkPhone.Enabled = false;
      this.siBtnCellPhone.Enabled = false;
      this.siBtnFaxNumber.Enabled = false;
      this.siBtnHomeEmail.Enabled = false;
      this.siBtnWorkEmail.Enabled = false;
      this.comboBoxBizInfoLicAuthType.Enabled = false;
      this.txtBoxBizAuthName.ReadOnly = true;
      this.comboBoxBizInfoLicState.Enabled = false;
      this.txtBoxPersonalInfoLicNumber.ReadOnly = true;
      this.txtBoxPersonalInfoLicAuthNAme.ReadOnly = true;
      this.comboBoxPersonalInfoLicState.Enabled = false;
      this.comboBoxPersonalInfoLicAuthType.Enabled = false;
      this.dpBizLicIssueDate.Enabled = false;
      this.dpPersonalInfoLicIssueDate.Enabled = false;
    }

    private void setControlState(bool readOnly, bool clearText, bool hideCategory)
    {
      this.SuspendLayout();
      this.txtBoxBizState.ReadOnly = readOnly;
      this.txtBoxBizZip.ReadOnly = readOnly;
      this.txtBoxBizCity.ReadOnly = readOnly;
      this.txtBoxBizAddress2.ReadOnly = readOnly;
      this.txtBoxBizAddress1.ReadOnly = readOnly;
      this.txtBoxBizWebUrl.ReadOnly = readOnly;
      this.txtBoxFees.ReadOnly = readOnly;
      this.txtBoxJobTitle.ReadOnly = readOnly;
      this.txtBoxLicenseNumber.ReadOnly = readOnly;
      if (clearText)
      {
        this.txtBoxBizState.Text = string.Empty;
        this.txtBoxBizZip.Text = string.Empty;
        this.txtBoxBizCity.Text = string.Empty;
        this.txtBoxBizAddress2.Text = string.Empty;
        this.txtBoxBizAddress1.Text = string.Empty;
        this.txtBoxBizWebUrl.Text = string.Empty;
        this.txtBoxFees.Text = string.Empty;
        this.txtBoxJobTitle.Text = string.Empty;
        this.txtBoxLicenseNumber.Text = string.Empty;
      }
      this.setControlStateCategoryFields(readOnly, clearText, hideCategory);
      this.ResumeLayout();
    }

    private void setControlStateCategoryFields(bool readOnly, bool clearText, bool hideCategory)
    {
      if (clearText)
        this.lblCategoryTitle.Text = "Category standard fields:";
      for (int index = 0; index < this.cboFieldValues.Length; ++index)
      {
        this.cboFieldValues[index].Enabled = !readOnly;
        if (clearText)
          this.cboFieldValues[index].Text = string.Empty;
        if (hideCategory)
        {
          this.cboFieldValues[index].Visible = false;
          this.lblFieldDescriptions[index].Visible = false;
        }
      }
    }

    private void disableControls()
    {
      this.initialLoad = true;
      this.clearForm();
      this.disableControlsOnly();
      this.setControlState(true, true, false);
      this.initialLoad = false;
    }

    private void enableControls()
    {
      this.cmbBoxCategoryID.Enabled = true;
      this.chkBoxNoSpam.Enabled = true;
      this.txtBoxFaxNumber.ReadOnly = false;
      this.txtBoxHomePhone.ReadOnly = false;
      this.txtBoxMobilePhone.ReadOnly = false;
      this.txtBoxPersonalEmail.ReadOnly = false;
      this.txtBoxBizEmail.ReadOnly = false;
      this.txtBoxWorkPhone.ReadOnly = false;
      this.txtBoxLastName.ReadOnly = false;
      this.txtBoxSalutation.ReadOnly = false;
      this.txtBoxFirstName.ReadOnly = false;
      this.txtBoxCompanyName.ReadOnly = false;
      this.chkBoxAccess.Enabled = true;
      this.comboBoxBizInfoLicAuthType.Enabled = true;
      this.txtBoxBizAuthName.ReadOnly = false;
      this.comboBoxBizInfoLicState.Enabled = true;
      this.txtBoxPersonalInfoLicNumber.ReadOnly = false;
      this.txtBoxPersonalInfoLicAuthNAme.ReadOnly = false;
      this.comboBoxPersonalInfoLicState.Enabled = true;
      this.comboBoxPersonalInfoLicAuthType.Enabled = true;
      this.dpBizLicIssueDate.Enabled = true;
      this.dpPersonalInfoLicIssueDate.Enabled = true;
    }

    private void setSelectedCategory(int categoryId)
    {
      for (int index = 0; index < this.cmbBoxCategoryID.Items.Count; ++index)
      {
        if (((BizCategory) this.cmbBoxCategoryID.Items[index]).CategoryID == categoryId)
        {
          this.cmbBoxCategoryID.SelectedIndex = index;
          this.curDisplayedCatID = categoryId;
          return;
        }
      }
      this.curDisplayedCatID = -1;
      this.cmbBoxCategoryID.SelectedIndex = -1;
    }

    private int getSelectedCategory()
    {
      return this.cmbBoxCategoryID.SelectedIndex < 0 ? -1 : ((BizCategory) this.cmbBoxCategoryID.SelectedItem).CategoryID;
    }

    private void Init()
    {
      this.catUtil = new BizCategoryUtil(Session.SessionObjects);
      BizCategory[] bizCategories = Session.ContactManager.GetBizCategories();
      this.cmbBoxCategoryID.Items.Clear();
      this.cmbBoxCategoryID.Items.AddRange((object[]) bizCategories);
      this.CurrentContactID = -1;
      string caption1 = "Open the Contact Notes and send an email";
      this.toolTip.SetToolTip((Control) this.siBtnHomeEmail, caption1);
      this.toolTip.SetToolTip((Control) this.siBtnWorkEmail, caption1);
      string caption2 = "Open the Contact Notes";
      this.toolTip.SetToolTip((Control) this.siBtnHomePhone, caption2);
      this.toolTip.SetToolTip((Control) this.siBtnWorkPhone, caption2);
      this.toolTip.SetToolTip((Control) this.siBtnCellPhone, caption2);
      this.toolTip.SetToolTip((Control) this.siBtnFaxNumber, caption2);
    }

    public void SetFocusOnDefaultField() => this.txtBoxFirstName.Focus();

    public void RefreshData()
    {
      BizCategory[] bizCategories = Session.ContactManager.GetBizCategories();
      object selectedItem = this.cmbBoxCategoryID.SelectedItem;
      this.cmbBoxCategoryID.Items.Clear();
      this.cmbBoxCategoryID.Items.AddRange((object[]) bizCategories);
      try
      {
        if (selectedItem == null)
          return;
        this.refreshWOSave = true;
        this.cmbBoxCategoryID.SelectedItem = selectedItem;
        this.refreshWOSave = false;
      }
      catch
      {
      }
    }

    public void RefreshContactList() => this.tabForm.RefreshContactList();

    private void BizPartnerInfoForm_SizeChanged(object sender, EventArgs e)
    {
      this.gcPersonalInfo.Size = this.gcBusinessInfo.Size = this.Width <= 1010 ? new Size(334, this.Height) : new Size((this.Width - 8) / 3, this.Height);
    }

    public void summaryFieldChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.formatText(sender, e);
      if (this.intermidiateData)
        return;
      if (sender is ComboBox && this.cmbBoxCategoryID == (ComboBox) sender)
      {
        int categoryId = -1;
        if (this.contactInfo != null && !this.validateCategoryChange(out categoryId))
          return;
        if (categoryId > -1)
        {
          this.setControlStateCategoryFields(this.isReadOnly || -1 == this.CurrentContactID, true, true);
          this.loadCategoryFields(categoryId);
          this.populateCategoryFields();
          if (this.CategoryChangedEvent != null)
            this.CategoryChangedEvent(categoryId);
        }
      }
      if (this.refreshWOSave)
        return;
      this.changed = true;
      if (this.SummaryChanged == null)
        return;
      this.SummaryChanged((object) null, (EventArgs) null);
    }

    private bool validateCategoryChange(out int categoryId)
    {
      categoryId = this.catUtil.CategoryNameToId(this.cmbBoxCategoryID.Text);
      if (-1 == categoryId)
      {
        this.catUtil = new BizCategoryUtil(Session.SessionObjects);
        categoryId = this.catUtil.CategoryNameToId(this.cmbBoxCategoryID.Text);
      }
      if (categoryId == this.curDisplayedCatID)
        return false;
      this.customFieldValues = CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(this.contactInfo.ContactID, this.contactInfo.CategoryID));
      if (0 < this.customFieldValues.Count && DialogResult.No == Utils.Dialog((IWin32Window) this, "Changing the category for this contact will result in the loss of category custom field values. Do you wish to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
      {
        this.cmbBoxCategoryID.SelectedIndexChanged -= new EventHandler(this.summaryFieldChanged);
        this.setSelectedCategory(this.curDisplayedCatID);
        this.cmbBoxCategoryID.SelectedIndexChanged += new EventHandler(this.summaryFieldChanged);
        return false;
      }
      this.curDisplayedCatID = categoryId;
      return true;
    }

    public void formatText(object sender, EventArgs e)
    {
      if (this.intermidiateData)
        this.intermidiateData = false;
      else if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        FieldFormat dataFormat;
        if (sender == this.txtBoxHomePhone || sender == this.txtBoxFaxNumber || sender == this.txtBoxWorkPhone || sender == this.txtBoxMobilePhone)
          dataFormat = FieldFormat.PHONE;
        else if (sender == this.txtBoxBizZip)
        {
          dataFormat = FieldFormat.ZIPCODE;
        }
        else
        {
          if (sender != this.txtBoxBizState)
            return;
          dataFormat = FieldFormat.STATE;
        }
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        int newCursorPos = 0;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate, textBox.SelectionStart, ref newCursorPos);
        if (!needsUpdate)
          return;
        this.intermidiateData = true;
        textBox.Text = str;
        textBox.SelectionStart = newCursorPos;
      }
    }

    private void feeChanged(object sender, EventArgs e)
    {
      this.changed = true;
      if (this.txtBoxFees.Text == "")
        return;
      try
      {
        if (int.Parse(this.txtBoxFees.Text) >= 0)
          return;
      }
      catch
      {
      }
      int num = (int) Utils.Dialog((IWin32Window) this, "The Fees for this contact must be a whole number greater than or equal to zero.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      if (this.contactInfo != null)
        this.txtBoxFees.Text = this.contactInfo.Fees >= 0 ? this.contactInfo.Fees.ToString() : "";
      else
        this.txtBoxFees.Text = "";
      this.summaryFieldChanged((object) null, (EventArgs) null);
    }

    private void txtBoxFees_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void txtBoxBizState_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      textBox.Text = textBox.Text.ToUpper();
    }

    private void controlChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.formatText(sender, e);
      this.changed = true;
    }

    private void txtBoxKeyDown(object sender, KeyEventArgs e)
    {
      if (this.initialLoad || e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    private bool checkPrivateRight()
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return true;
      ContactGroupInfo[] groupsForContact = Session.ContactGroupManager.GetContactGroupsForContact(this.sourceCT, this.CurrentContactID);
      if ((groupsForContact == null || groupsForContact.Length == 0) && Session.UserInfo.IsTopLevelAdministrator())
        return true;
      BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
      foreach (ContactGroupInfo contactGroupInfo in groupsForContact)
      {
        bool flag = false;
        foreach (BizGroupRef bizGroupRef in contactGroupRefs)
        {
          if (bizGroupRef.BizGroupID == contactGroupInfo.GroupId)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }

    private void picBoxHomeEmail_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      SystemUtil.ShellExecute("mailto:" + this.contactInfo.PersonalEmail);
      ContactUtils.addEmailHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.BizPartner, newNote);
    }

    private void picBoxWorkEmail_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      SystemUtil.ShellExecute("mailto:" + this.contactInfo.BizEmail);
      ContactUtils.addEmailHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.BizPartner, newNote);
    }

    private void picBoxHomePhone_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      ContactUtils.addCallHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.BizPartner, newNote);
    }

    private void picBoxWorkPhone_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      ContactUtils.addCallHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.BizPartner, newNote);
    }

    private void picBoxCellPhone_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      ContactUtils.addCallHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.BizPartner, newNote);
    }

    private void picBoxFaxNumber_Click(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      this.tabForm.ActivateNotesTab();
      int newNote = this.tabForm.ContactNotesForm.CreateNewNote();
      if (this.contactInfo == null)
        return;
      ContactUtils.addFaxHistory(new int[1]
      {
        this.contactInfo.ContactID
      }, ContactType.BizPartner, newNote);
    }

    private void picBoxEmail_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (BizPartnerInfo1Form)).GetObject("picBoxEmailOver.Image");
    }

    private void picBoxEmail_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (BizPartnerInfo1Form)).GetObject("picBoxHomeEmail.Image");
    }

    private void picBoxPhone_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (BizPartnerInfo1Form)).GetObject("picBoxPhoneOver.Image");
    }

    private void picBoxPhone_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (BizPartnerInfo1Form)).GetObject("picBoxHomePhone.Image");
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.toolTip = new ToolTip(this.components);
      this.pnlMiddle = new Panel();
      this.gcBusinessInfo = new GroupContainer();
      this.comboBoxBizInfoLicAuthType = new ComboBox();
      this.comboBoxBizInfoLicState = new ComboBox();
      this.dpBizLicIssueDate = new DatePicker();
      this.txtBoxBizAuthName = new TextBox();
      this.label10 = new Label();
      this.lblLicenseNum = new Label();
      this.txtBoxLicenseNumber = new TextBox();
      this.label2 = new Label();
      this.label4 = new Label();
      this.label8 = new Label();
      this.txtBoxJobTitle = new TextBox();
      this.lblTitle = new Label();
      this.txtBoxFees = new TextBox();
      this.lblFees = new Label();
      this.txtBoxBizState = new TextBox();
      this.txtBoxBizZip = new TextBox();
      this.lblBizZip = new Label();
      this.lblBizState = new Label();
      this.txtBoxBizCity = new TextBox();
      this.txtBoxBizAddress2 = new TextBox();
      this.lblBizCity = new Label();
      this.lblBizAddress2 = new Label();
      this.lblBizAddress1 = new Label();
      this.txtBoxBizAddress1 = new TextBox();
      this.lblWebUrl = new Label();
      this.txtBoxBizWebUrl = new TextBox();
      this.lblCategory = new Label();
      this.cmbBoxCategoryID = new ComboBox();
      this.lblCompany = new Label();
      this.txtBoxCompanyName = new TextBox();
      this.gcBizCatAddFields = new GroupContainer();
      this.panel1 = new Panel();
      this.lblCategoryTitle = new Label();
      this.lblFieldDescription11 = new Label();
      this.cboFieldValue11 = new ComboBox();
      this.lblFieldDescription01 = new Label();
      this.lblFieldDescription02 = new Label();
      this.cboFieldValue10 = new ComboBox();
      this.cboFieldValue01 = new ComboBox();
      this.lblFieldDescription10 = new Label();
      this.cboFieldValue02 = new ComboBox();
      this.lblFieldDescription09 = new Label();
      this.cboFieldValue03 = new ComboBox();
      this.cboFieldValue09 = new ComboBox();
      this.cboFieldValue04 = new ComboBox();
      this.lblFieldDescription08 = new Label();
      this.cboFieldValue05 = new ComboBox();
      this.cboFieldValue08 = new ComboBox();
      this.lblFieldDescription03 = new Label();
      this.lblFieldDescription07 = new Label();
      this.lblFieldDescription04 = new Label();
      this.lblFieldDescription06 = new Label();
      this.lblFieldDescription05 = new Label();
      this.cboFieldValue07 = new ComboBox();
      this.cboFieldValue06 = new ComboBox();
      this.gcPersonalInfo = new GroupContainer();
      this.comboBoxPersonalInfoLicAuthType = new ComboBox();
      this.comboBoxPersonalInfoLicState = new ComboBox();
      this.dpPersonalInfoLicIssueDate = new DatePicker();
      this.txtBoxPersonalInfoLicNumber = new TextBox();
      this.label9 = new Label();
      this.txtBoxPersonalInfoLicAuthNAme = new TextBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label3 = new Label();
      this.siBtnWorkEmail = new StandardIconButton();
      this.siBtnHomeEmail = new StandardIconButton();
      this.siBtnFaxNumber = new StandardIconButton();
      this.siBtnCellPhone = new StandardIconButton();
      this.siBtnWorkPhone = new StandardIconButton();
      this.siBtnHomePhone = new StandardIconButton();
      this.chkBoxAccess = new CheckBox();
      this.lblFirstName = new Label();
      this.txtBoxPersonalEmail = new TextBox();
      this.txtBoxSalutation = new TextBox();
      this.txtBoxFaxNumber = new TextBox();
      this.lblHomeEmail = new Label();
      this.label1 = new Label();
      this.lblFaxNumber = new Label();
      this.txtBoxBizEmail = new TextBox();
      this.lblWorkEmail = new Label();
      this.lblCellPhone = new Label();
      this.txtBoxWorkPhone = new TextBox();
      this.txtBoxMobilePhone = new TextBox();
      this.lblWorkPhone = new Label();
      this.chkBoxNoSpam = new CheckBox();
      this.txtBoxHomePhone = new TextBox();
      this.lblHomePhone = new Label();
      this.txtBoxLastName = new TextBox();
      this.txtBoxFirstName = new TextBox();
      this.lblLastName = new Label();
      this.pnlMiddle.SuspendLayout();
      this.gcBusinessInfo.SuspendLayout();
      this.gcBizCatAddFields.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gcPersonalInfo.SuspendLayout();
      ((ISupportInitialize) this.siBtnWorkEmail).BeginInit();
      ((ISupportInitialize) this.siBtnHomeEmail).BeginInit();
      ((ISupportInitialize) this.siBtnFaxNumber).BeginInit();
      ((ISupportInitialize) this.siBtnCellPhone).BeginInit();
      ((ISupportInitialize) this.siBtnWorkPhone).BeginInit();
      ((ISupportInitialize) this.siBtnHomePhone).BeginInit();
      this.SuspendLayout();
      this.toolTip.AutomaticDelay = 0;
      this.pnlMiddle.Controls.Add((Control) this.gcBusinessInfo);
      this.pnlMiddle.Dock = DockStyle.Fill;
      this.pnlMiddle.Location = new Point(335, 1);
      this.pnlMiddle.Name = "pnlMiddle";
      this.pnlMiddle.Padding = new Padding(4, 0, 4, 0);
      this.pnlMiddle.Size = new Size(340, 378);
      this.pnlMiddle.TabIndex = 5;
      this.gcBusinessInfo.Controls.Add((Control) this.comboBoxBizInfoLicAuthType);
      this.gcBusinessInfo.Controls.Add((Control) this.comboBoxBizInfoLicState);
      this.gcBusinessInfo.Controls.Add((Control) this.dpBizLicIssueDate);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizAuthName);
      this.gcBusinessInfo.Controls.Add((Control) this.label10);
      this.gcBusinessInfo.Controls.Add((Control) this.lblLicenseNum);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxLicenseNumber);
      this.gcBusinessInfo.Controls.Add((Control) this.label2);
      this.gcBusinessInfo.Controls.Add((Control) this.label4);
      this.gcBusinessInfo.Controls.Add((Control) this.label8);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxJobTitle);
      this.gcBusinessInfo.Controls.Add((Control) this.lblTitle);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxFees);
      this.gcBusinessInfo.Controls.Add((Control) this.lblFees);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizState);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizZip);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizZip);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizState);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizCity);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizAddress2);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizCity);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizAddress2);
      this.gcBusinessInfo.Controls.Add((Control) this.lblBizAddress1);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizAddress1);
      this.gcBusinessInfo.Controls.Add((Control) this.lblWebUrl);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxBizWebUrl);
      this.gcBusinessInfo.Controls.Add((Control) this.lblCategory);
      this.gcBusinessInfo.Controls.Add((Control) this.cmbBoxCategoryID);
      this.gcBusinessInfo.Controls.Add((Control) this.lblCompany);
      this.gcBusinessInfo.Controls.Add((Control) this.txtBoxCompanyName);
      this.gcBusinessInfo.Dock = DockStyle.Fill;
      this.gcBusinessInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcBusinessInfo.Location = new Point(4, 0);
      this.gcBusinessInfo.Name = "gcBusinessInfo";
      this.gcBusinessInfo.Size = new Size(332, 378);
      this.gcBusinessInfo.TabIndex = 3;
      this.gcBusinessInfo.Text = "Business Information";
      this.comboBoxBizInfoLicAuthType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxBizInfoLicAuthType.FormattingEnabled = true;
      this.comboBoxBizInfoLicAuthType.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "Private",
        (object) "Public Federal",
        (object) "Public Local",
        (object) "Public State"
      });
      this.comboBoxBizInfoLicAuthType.Location = new Point(153, 280);
      this.comboBoxBizInfoLicAuthType.Name = "comboBoxBizInfoLicAuthType";
      this.comboBoxBizInfoLicAuthType.Size = new Size(160, 22);
      this.comboBoxBizInfoLicAuthType.TabIndex = 94;
      this.comboBoxBizInfoLicAuthType.SelectedIndexChanged += new EventHandler(this.summaryFieldChanged);
      this.comboBoxBizInfoLicState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxBizInfoLicState.FormattingEnabled = true;
      this.comboBoxBizInfoLicState.Location = new Point(153, 303);
      this.comboBoxBizInfoLicState.Name = "comboBoxBizInfoLicState";
      this.comboBoxBizInfoLicState.Size = new Size(121, 22);
      this.comboBoxBizInfoLicState.TabIndex = 66;
      this.comboBoxBizInfoLicState.SelectedIndexChanged += new EventHandler(this.summaryFieldChanged);
      this.dpBizLicIssueDate.BackColor = SystemColors.Window;
      this.dpBizLicIssueDate.Location = new Point(153, 326);
      this.dpBizLicIssueDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpBizLicIssueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpBizLicIssueDate.Name = "dpBizLicIssueDate";
      this.dpBizLicIssueDate.Size = new Size(123, 22);
      this.dpBizLicIssueDate.TabIndex = 68;
      this.dpBizLicIssueDate.Tag = (object) "763";
      this.dpBizLicIssueDate.ToolTip = "";
      this.dpBizLicIssueDate.Value = new DateTime(0L);
      this.dpBizLicIssueDate.ValueChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizAuthName.Location = new Point(153, 259);
      this.txtBoxBizAuthName.MaxLength = 50;
      this.txtBoxBizAuthName.Name = "txtBoxBizAuthName";
      this.txtBoxBizAuthName.Size = new Size(164, 20);
      this.txtBoxBizAuthName.TabIndex = 62;
      this.txtBoxBizAuthName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(10, 262);
      this.label10.Name = "label10";
      this.label10.Size = new Size(137, 14);
      this.label10.TabIndex = 93;
      this.label10.Text = "Lic. Issuing Authority Name";
      this.lblLicenseNum.Location = new Point(10, 240);
      this.lblLicenseNum.Name = "lblLicenseNum";
      this.lblLicenseNum.Size = new Size(108, 15);
      this.lblLicenseNum.TabIndex = 91;
      this.lblLicenseNum.Text = "Company License #";
      this.txtBoxLicenseNumber.Location = new Point(153, 237);
      this.txtBoxLicenseNumber.MaxLength = 50;
      this.txtBoxLicenseNumber.Name = "txtBoxLicenseNumber";
      this.txtBoxLicenseNumber.Size = new Size(164, 20);
      this.txtBoxLicenseNumber.TabIndex = 60;
      this.txtBoxLicenseNumber.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 329);
      this.label2.Name = "label2";
      this.label2.Size = new Size(78, 14);
      this.label2.TabIndex = 86;
      this.label2.Text = "Lic. Issue Date";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 307);
      this.label4.Name = "label4";
      this.label4.Size = new Size(126, 14);
      this.label4.TabIndex = 85;
      this.label4.Text = "Lic. Authority State Code";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 285);
      this.label8.Name = "label8";
      this.label8.Size = new Size(96, 14);
      this.label8.TabIndex = 84;
      this.label8.Text = "Lic. Authority Type";
      this.txtBoxJobTitle.Location = new Point(153, 215);
      this.txtBoxJobTitle.MaxLength = 50;
      this.txtBoxJobTitle.Name = "txtBoxJobTitle";
      this.txtBoxJobTitle.Size = new Size(164, 20);
      this.txtBoxJobTitle.TabIndex = 58;
      this.txtBoxJobTitle.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblTitle.Location = new Point(10, 218);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(60, 15);
      this.lblTitle.TabIndex = 24;
      this.lblTitle.Text = "Job Title";
      this.txtBoxFees.Location = new Point(153, 171);
      this.txtBoxFees.MaxLength = 8;
      this.txtBoxFees.Name = "txtBoxFees";
      this.txtBoxFees.Size = new Size(164, 20);
      this.txtBoxFees.TabIndex = 54;
      this.txtBoxFees.TextChanged += new EventHandler(this.feeChanged);
      this.lblFees.Location = new Point(10, 174);
      this.lblFees.Name = "lblFees";
      this.lblFees.Size = new Size(60, 15);
      this.lblFees.TabIndex = 28;
      this.lblFees.Text = "Fees";
      this.txtBoxBizState.CharacterCasing = CharacterCasing.Upper;
      this.txtBoxBizState.Location = new Point(153, 148);
      this.txtBoxBizState.MaxLength = 2;
      this.txtBoxBizState.Name = "txtBoxBizState";
      this.txtBoxBizState.Size = new Size(28, 20);
      this.txtBoxBizState.TabIndex = 50;
      this.txtBoxBizState.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizState.Leave += new EventHandler(this.txtBoxBizState_Leave);
      this.txtBoxBizZip.Location = new Point(219, 148);
      this.txtBoxBizZip.MaxLength = 20;
      this.txtBoxBizZip.Name = "txtBoxBizZip";
      this.txtBoxBizZip.Size = new Size(82, 20);
      this.txtBoxBizZip.TabIndex = 52;
      this.txtBoxBizZip.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizZip.Leave += new EventHandler(this.txtBoxBizZip_Leave);
      this.lblBizZip.BackColor = Color.Transparent;
      this.lblBizZip.Location = new Point(190, 151);
      this.lblBizZip.Name = "lblBizZip";
      this.lblBizZip.RightToLeft = RightToLeft.No;
      this.lblBizZip.Size = new Size(23, 15);
      this.lblBizZip.TabIndex = 20;
      this.lblBizZip.Text = "Zip";
      this.lblBizState.BackColor = Color.Transparent;
      this.lblBizState.Location = new Point(10, 151);
      this.lblBizState.Name = "lblBizState";
      this.lblBizState.RightToLeft = RightToLeft.No;
      this.lblBizState.Size = new Size(33, 15);
      this.lblBizState.TabIndex = 18;
      this.lblBizState.Text = "State";
      this.txtBoxBizCity.Location = new Point(153, 126);
      this.txtBoxBizCity.MaxLength = 50;
      this.txtBoxBizCity.Name = "txtBoxBizCity";
      this.txtBoxBizCity.Size = new Size(164, 20);
      this.txtBoxBizCity.TabIndex = 48;
      this.txtBoxBizCity.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizAddress2.Location = new Point(153, 104);
      this.txtBoxBizAddress2.MaxLength = 50;
      this.txtBoxBizAddress2.Name = "txtBoxBizAddress2";
      this.txtBoxBizAddress2.Size = new Size(164, 20);
      this.txtBoxBizAddress2.TabIndex = 46;
      this.txtBoxBizAddress2.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblBizCity.Location = new Point(10, 129);
      this.lblBizCity.Name = "lblBizCity";
      this.lblBizCity.RightToLeft = RightToLeft.No;
      this.lblBizCity.Size = new Size(25, 15);
      this.lblBizCity.TabIndex = 16;
      this.lblBizCity.Text = "City";
      this.lblBizAddress2.Location = new Point(10, 108);
      this.lblBizAddress2.Name = "lblBizAddress2";
      this.lblBizAddress2.Size = new Size(62, 16);
      this.lblBizAddress2.TabIndex = 14;
      this.lblBizAddress2.Text = "Address 2";
      this.lblBizAddress1.Location = new Point(10, 85);
      this.lblBizAddress1.Name = "lblBizAddress1";
      this.lblBizAddress1.Size = new Size(62, 20);
      this.lblBizAddress1.TabIndex = 12;
      this.lblBizAddress1.Text = "Address 1";
      this.txtBoxBizAddress1.Location = new Point(153, 82);
      this.txtBoxBizAddress1.MaxLength = (int) byte.MaxValue;
      this.txtBoxBizAddress1.Name = "txtBoxBizAddress1";
      this.txtBoxBizAddress1.Size = new Size(164, 20);
      this.txtBoxBizAddress1.TabIndex = 44;
      this.txtBoxBizAddress1.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblWebUrl.Location = new Point(10, 196);
      this.lblWebUrl.Name = "lblWebUrl";
      this.lblWebUrl.Size = new Size(60, 15);
      this.lblWebUrl.TabIndex = 22;
      this.lblWebUrl.Text = "Web URL";
      this.txtBoxBizWebUrl.Location = new Point(153, 193);
      this.txtBoxBizWebUrl.MaxLength = 50;
      this.txtBoxBizWebUrl.Name = "txtBoxBizWebUrl";
      this.txtBoxBizWebUrl.Size = new Size(164, 20);
      this.txtBoxBizWebUrl.TabIndex = 56;
      this.txtBoxBizWebUrl.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxBizWebUrl.Leave += new EventHandler(this.txtBoxBizWebUrl_Leave);
      this.lblCategory.Location = new Point(10, 38);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(60, 15);
      this.lblCategory.TabIndex = 0;
      this.lblCategory.Text = "Category";
      this.cmbBoxCategoryID.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCategoryID.Location = new Point(153, 36);
      this.cmbBoxCategoryID.Name = "cmbBoxCategoryID";
      this.cmbBoxCategoryID.Size = new Size(164, 22);
      this.cmbBoxCategoryID.Sorted = true;
      this.cmbBoxCategoryID.TabIndex = 40;
      this.cmbBoxCategoryID.SelectedIndexChanged += new EventHandler(this.summaryFieldChanged);
      this.lblCompany.Location = new Point(10, 63);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(60, 15);
      this.lblCompany.TabIndex = 2;
      this.lblCompany.Text = "Company";
      this.txtBoxCompanyName.Location = new Point(153, 60);
      this.txtBoxCompanyName.MaxLength = 64;
      this.txtBoxCompanyName.Name = "txtBoxCompanyName";
      this.txtBoxCompanyName.Size = new Size(164, 20);
      this.txtBoxCompanyName.TabIndex = 42;
      this.txtBoxCompanyName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.gcBizCatAddFields.Controls.Add((Control) this.panel1);
      this.gcBizCatAddFields.Dock = DockStyle.Right;
      this.gcBizCatAddFields.HeaderForeColor = SystemColors.ControlText;
      this.gcBizCatAddFields.Location = new Point(675, 1);
      this.gcBizCatAddFields.Name = "gcBizCatAddFields";
      this.gcBizCatAddFields.Size = new Size(334, 378);
      this.gcBizCatAddFields.TabIndex = 4;
      this.gcBizCatAddFields.Text = "Business Category Additional Fields";
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add((Control) this.lblCategoryTitle);
      this.panel1.Controls.Add((Control) this.lblFieldDescription11);
      this.panel1.Controls.Add((Control) this.cboFieldValue11);
      this.panel1.Controls.Add((Control) this.lblFieldDescription01);
      this.panel1.Controls.Add((Control) this.lblFieldDescription02);
      this.panel1.Controls.Add((Control) this.cboFieldValue10);
      this.panel1.Controls.Add((Control) this.cboFieldValue01);
      this.panel1.Controls.Add((Control) this.lblFieldDescription10);
      this.panel1.Controls.Add((Control) this.cboFieldValue02);
      this.panel1.Controls.Add((Control) this.lblFieldDescription09);
      this.panel1.Controls.Add((Control) this.cboFieldValue03);
      this.panel1.Controls.Add((Control) this.cboFieldValue09);
      this.panel1.Controls.Add((Control) this.cboFieldValue04);
      this.panel1.Controls.Add((Control) this.lblFieldDescription08);
      this.panel1.Controls.Add((Control) this.cboFieldValue05);
      this.panel1.Controls.Add((Control) this.cboFieldValue08);
      this.panel1.Controls.Add((Control) this.lblFieldDescription03);
      this.panel1.Controls.Add((Control) this.lblFieldDescription07);
      this.panel1.Controls.Add((Control) this.lblFieldDescription04);
      this.panel1.Controls.Add((Control) this.lblFieldDescription06);
      this.panel1.Controls.Add((Control) this.lblFieldDescription05);
      this.panel1.Controls.Add((Control) this.cboFieldValue07);
      this.panel1.Controls.Add((Control) this.cboFieldValue06);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(332, 351);
      this.panel1.TabIndex = 51;
      this.lblCategoryTitle.AutoSize = true;
      this.lblCategoryTitle.Font = new Font("Arial", 11f, FontStyle.Bold, GraphicsUnit.Pixel, (byte) 0);
      this.lblCategoryTitle.Location = new Point(7, 5);
      this.lblCategoryTitle.Name = "lblCategoryTitle";
      this.lblCategoryTitle.Size = new Size(146, 14);
      this.lblCategoryTitle.TabIndex = 28;
      this.lblCategoryTitle.Text = "Category standard fields:";
      this.lblFieldDescription11.Location = new Point(11, 274);
      this.lblFieldDescription11.Name = "lblFieldDescription11";
      this.lblFieldDescription11.Size = new Size(128, 15);
      this.lblFieldDescription11.TabIndex = 49;
      this.lblFieldDescription11.Text = "Field Description 11";
      this.cboFieldValue11.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue11.Location = new Point(151, 270);
      this.cboFieldValue11.Name = "cboFieldValue11";
      this.cboFieldValue11.Size = new Size(104, 22);
      this.cboFieldValue11.TabIndex = 50;
      this.cboFieldValue11.Tag = (object) "5";
      this.cboFieldValue11.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue11.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue11.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.lblFieldDescription01.Location = new Point(9, 24);
      this.lblFieldDescription01.Name = "lblFieldDescription01";
      this.lblFieldDescription01.Size = new Size(128, 15);
      this.lblFieldDescription01.TabIndex = 29;
      this.lblFieldDescription01.Text = "Field Description 1";
      this.lblFieldDescription02.Location = new Point(9, 48);
      this.lblFieldDescription02.Name = "lblFieldDescription02";
      this.lblFieldDescription02.Size = new Size(128, 15);
      this.lblFieldDescription02.TabIndex = 31;
      this.lblFieldDescription02.Text = "Field Description 2";
      this.cboFieldValue10.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue10.Location = new Point(151, 245);
      this.cboFieldValue10.Name = "cboFieldValue10";
      this.cboFieldValue10.Size = new Size(104, 22);
      this.cboFieldValue10.TabIndex = 48;
      this.cboFieldValue10.Tag = (object) "5";
      this.cboFieldValue10.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue10.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue10.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.cboFieldValue01.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue01.Location = new Point(151, 20);
      this.cboFieldValue01.Name = "cboFieldValue01";
      this.cboFieldValue01.Size = new Size(104, 22);
      this.cboFieldValue01.TabIndex = 30;
      this.cboFieldValue01.Tag = (object) "1";
      this.cboFieldValue01.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue01.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue01.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.lblFieldDescription10.Location = new Point(10, 250);
      this.lblFieldDescription10.Name = "lblFieldDescription10";
      this.lblFieldDescription10.Size = new Size(128, 15);
      this.lblFieldDescription10.TabIndex = 47;
      this.lblFieldDescription10.Text = "Field Description 10";
      this.cboFieldValue02.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue02.Location = new Point(151, 45);
      this.cboFieldValue02.Name = "cboFieldValue02";
      this.cboFieldValue02.Size = new Size(104, 22);
      this.cboFieldValue02.TabIndex = 32;
      this.cboFieldValue02.Tag = (object) "2";
      this.cboFieldValue02.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue02.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue02.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.lblFieldDescription09.Location = new Point(9, 225);
      this.lblFieldDescription09.Name = "lblFieldDescription09";
      this.lblFieldDescription09.Size = new Size(128, 15);
      this.lblFieldDescription09.TabIndex = 46;
      this.lblFieldDescription09.Text = "Field Description 9";
      this.cboFieldValue03.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue03.Location = new Point(151, 70);
      this.cboFieldValue03.Name = "cboFieldValue03";
      this.cboFieldValue03.Size = new Size(104, 22);
      this.cboFieldValue03.TabIndex = 34;
      this.cboFieldValue03.Tag = (object) "3";
      this.cboFieldValue03.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue03.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue03.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.cboFieldValue09.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue09.Location = new Point(151, 220);
      this.cboFieldValue09.Name = "cboFieldValue09";
      this.cboFieldValue09.Size = new Size(104, 22);
      this.cboFieldValue09.TabIndex = 45;
      this.cboFieldValue09.Tag = (object) "5";
      this.cboFieldValue09.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue09.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue09.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.cboFieldValue04.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue04.Location = new Point(151, 95);
      this.cboFieldValue04.Name = "cboFieldValue04";
      this.cboFieldValue04.Size = new Size(104, 22);
      this.cboFieldValue04.TabIndex = 36;
      this.cboFieldValue04.Tag = (object) "4";
      this.cboFieldValue04.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue04.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue04.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.lblFieldDescription08.Location = new Point(9, 198);
      this.lblFieldDescription08.Name = "lblFieldDescription08";
      this.lblFieldDescription08.Size = new Size(128, 15);
      this.lblFieldDescription08.TabIndex = 43;
      this.lblFieldDescription08.Text = "Field Description 8";
      this.cboFieldValue05.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue05.Location = new Point(151, 120);
      this.cboFieldValue05.Name = "cboFieldValue05";
      this.cboFieldValue05.Size = new Size(104, 22);
      this.cboFieldValue05.TabIndex = 38;
      this.cboFieldValue05.Tag = (object) "5";
      this.cboFieldValue05.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue05.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue05.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.cboFieldValue08.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue08.Location = new Point(151, 195);
      this.cboFieldValue08.Name = "cboFieldValue08";
      this.cboFieldValue08.Size = new Size(104, 22);
      this.cboFieldValue08.TabIndex = 44;
      this.cboFieldValue08.Tag = (object) "5";
      this.cboFieldValue08.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue08.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue08.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.lblFieldDescription03.Location = new Point(9, 74);
      this.lblFieldDescription03.Name = "lblFieldDescription03";
      this.lblFieldDescription03.Size = new Size(128, 15);
      this.lblFieldDescription03.TabIndex = 33;
      this.lblFieldDescription03.Text = "Field Description 3";
      this.lblFieldDescription07.Location = new Point(9, 173);
      this.lblFieldDescription07.Name = "lblFieldDescription07";
      this.lblFieldDescription07.Size = new Size(128, 15);
      this.lblFieldDescription07.TabIndex = 41;
      this.lblFieldDescription07.Text = "Field Description 7";
      this.lblFieldDescription04.Location = new Point(9, 98);
      this.lblFieldDescription04.Name = "lblFieldDescription04";
      this.lblFieldDescription04.Size = new Size(128, 15);
      this.lblFieldDescription04.TabIndex = 35;
      this.lblFieldDescription04.Text = "Field Description 4";
      this.lblFieldDescription06.Location = new Point(9, 148);
      this.lblFieldDescription06.Name = "lblFieldDescription06";
      this.lblFieldDescription06.Size = new Size(128, 15);
      this.lblFieldDescription06.TabIndex = 39;
      this.lblFieldDescription06.Text = "Field Description 6";
      this.lblFieldDescription05.Location = new Point(9, 123);
      this.lblFieldDescription05.Name = "lblFieldDescription05";
      this.lblFieldDescription05.Size = new Size(128, 15);
      this.lblFieldDescription05.TabIndex = 37;
      this.lblFieldDescription05.Text = "Field Description 5";
      this.cboFieldValue07.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue07.Location = new Point(151, 170);
      this.cboFieldValue07.Name = "cboFieldValue07";
      this.cboFieldValue07.Size = new Size(104, 22);
      this.cboFieldValue07.TabIndex = 42;
      this.cboFieldValue07.Tag = (object) "5";
      this.cboFieldValue07.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue07.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue07.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.cboFieldValue06.DropDownStyle = ComboBoxStyle.Simple;
      this.cboFieldValue06.Location = new Point(151, 145);
      this.cboFieldValue06.Name = "cboFieldValue06";
      this.cboFieldValue06.Size = new Size(104, 22);
      this.cboFieldValue06.TabIndex = 40;
      this.cboFieldValue06.Tag = (object) "4";
      this.cboFieldValue06.SelectedIndexChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue06.TextChanged += new EventHandler(this.cboFieldValue_Changed);
      this.cboFieldValue06.KeyDown += new KeyEventHandler(this.cboFieldValue_KeyDown);
      this.gcPersonalInfo.Controls.Add((Control) this.comboBoxPersonalInfoLicAuthType);
      this.gcPersonalInfo.Controls.Add((Control) this.comboBoxPersonalInfoLicState);
      this.gcPersonalInfo.Controls.Add((Control) this.dpPersonalInfoLicIssueDate);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxPersonalInfoLicNumber);
      this.gcPersonalInfo.Controls.Add((Control) this.label9);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxPersonalInfoLicAuthNAme);
      this.gcPersonalInfo.Controls.Add((Control) this.label7);
      this.gcPersonalInfo.Controls.Add((Control) this.label6);
      this.gcPersonalInfo.Controls.Add((Control) this.label5);
      this.gcPersonalInfo.Controls.Add((Control) this.label3);
      this.gcPersonalInfo.Controls.Add((Control) this.siBtnWorkEmail);
      this.gcPersonalInfo.Controls.Add((Control) this.siBtnHomeEmail);
      this.gcPersonalInfo.Controls.Add((Control) this.siBtnFaxNumber);
      this.gcPersonalInfo.Controls.Add((Control) this.siBtnCellPhone);
      this.gcPersonalInfo.Controls.Add((Control) this.siBtnWorkPhone);
      this.gcPersonalInfo.Controls.Add((Control) this.siBtnHomePhone);
      this.gcPersonalInfo.Controls.Add((Control) this.chkBoxAccess);
      this.gcPersonalInfo.Controls.Add((Control) this.lblFirstName);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxPersonalEmail);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxSalutation);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxFaxNumber);
      this.gcPersonalInfo.Controls.Add((Control) this.lblHomeEmail);
      this.gcPersonalInfo.Controls.Add((Control) this.label1);
      this.gcPersonalInfo.Controls.Add((Control) this.lblFaxNumber);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxBizEmail);
      this.gcPersonalInfo.Controls.Add((Control) this.lblWorkEmail);
      this.gcPersonalInfo.Controls.Add((Control) this.lblCellPhone);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxWorkPhone);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxMobilePhone);
      this.gcPersonalInfo.Controls.Add((Control) this.lblWorkPhone);
      this.gcPersonalInfo.Controls.Add((Control) this.chkBoxNoSpam);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxHomePhone);
      this.gcPersonalInfo.Controls.Add((Control) this.lblHomePhone);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxLastName);
      this.gcPersonalInfo.Controls.Add((Control) this.txtBoxFirstName);
      this.gcPersonalInfo.Controls.Add((Control) this.lblLastName);
      this.gcPersonalInfo.Dock = DockStyle.Left;
      this.gcPersonalInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcPersonalInfo.Location = new Point(1, 1);
      this.gcPersonalInfo.Name = "gcPersonalInfo";
      this.gcPersonalInfo.Size = new Size(334, 378);
      this.gcPersonalInfo.TabIndex = 2;
      this.gcPersonalInfo.Text = "Personal Information";
      this.comboBoxPersonalInfoLicAuthType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxPersonalInfoLicAuthType.FormattingEnabled = true;
      this.comboBoxPersonalInfoLicAuthType.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "Private",
        (object) "Public Federal",
        (object) "Public Local",
        (object) "Public State"
      });
      this.comboBoxPersonalInfoLicAuthType.Location = new Point(151, 275);
      this.comboBoxPersonalInfoLicAuthType.Name = "comboBoxPersonalInfoLicAuthType";
      this.comboBoxPersonalInfoLicAuthType.Size = new Size(160, 22);
      this.comboBoxPersonalInfoLicAuthType.TabIndex = 85;
      this.comboBoxPersonalInfoLicAuthType.SelectedIndexChanged += new EventHandler(this.summaryFieldChanged);
      this.comboBoxPersonalInfoLicState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxPersonalInfoLicState.FormattingEnabled = true;
      this.comboBoxPersonalInfoLicState.Location = new Point(151, 298);
      this.comboBoxPersonalInfoLicState.Name = "comboBoxPersonalInfoLicState";
      this.comboBoxPersonalInfoLicState.Size = new Size(121, 22);
      this.comboBoxPersonalInfoLicState.TabIndex = 25;
      this.comboBoxPersonalInfoLicState.SelectedIndexChanged += new EventHandler(this.summaryFieldChanged);
      this.dpPersonalInfoLicIssueDate.BackColor = SystemColors.Window;
      this.dpPersonalInfoLicIssueDate.Location = new Point(151, 321);
      this.dpPersonalInfoLicIssueDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpPersonalInfoLicIssueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpPersonalInfoLicIssueDate.Name = "dpPersonalInfoLicIssueDate";
      this.dpPersonalInfoLicIssueDate.Size = new Size(123, 22);
      this.dpPersonalInfoLicIssueDate.TabIndex = 27;
      this.dpPersonalInfoLicIssueDate.Tag = (object) "763";
      this.dpPersonalInfoLicIssueDate.ToolTip = "";
      this.dpPersonalInfoLicIssueDate.Value = new DateTime(0L);
      this.dpPersonalInfoLicIssueDate.ValueChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxPersonalInfoLicNumber.Location = new Point(151, 233);
      this.txtBoxPersonalInfoLicNumber.MaxLength = 50;
      this.txtBoxPersonalInfoLicNumber.Name = "txtBoxPersonalInfoLicNumber";
      this.txtBoxPersonalInfoLicNumber.Size = new Size(173, 20);
      this.txtBoxPersonalInfoLicNumber.TabIndex = 19;
      this.txtBoxPersonalInfoLicNumber.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(10, 236);
      this.label9.Name = "label9";
      this.label9.Size = new Size(94, 14);
      this.label9.TabIndex = 84;
      this.label9.Text = "Contact License #";
      this.txtBoxPersonalInfoLicAuthNAme.Location = new Point(151, 254);
      this.txtBoxPersonalInfoLicAuthNAme.MaxLength = 50;
      this.txtBoxPersonalInfoLicAuthNAme.Name = "txtBoxPersonalInfoLicAuthNAme";
      this.txtBoxPersonalInfoLicAuthNAme.Size = new Size(173, 20);
      this.txtBoxPersonalInfoLicAuthNAme.TabIndex = 21;
      this.txtBoxPersonalInfoLicAuthNAme.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 324);
      this.label7.Name = "label7";
      this.label7.Size = new Size(78, 14);
      this.label7.TabIndex = 78;
      this.label7.Text = "Lic. Issue Date";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 302);
      this.label6.Name = "label6";
      this.label6.Size = new Size(126, 14);
      this.label6.TabIndex = 77;
      this.label6.Text = "Lic. Authority State Code";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 280);
      this.label5.Name = "label5";
      this.label5.Size = new Size(96, 14);
      this.label5.TabIndex = 76;
      this.label5.Text = "Lic. Authority Type";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 257);
      this.label3.Name = "label3";
      this.label3.Size = new Size(137, 14);
      this.label3.TabIndex = 75;
      this.label3.Text = "Lic. Issuing Authority Name";
      this.siBtnWorkEmail.BackColor = Color.Transparent;
      this.siBtnWorkEmail.Enabled = false;
      this.siBtnWorkEmail.Location = new Point(309, 212);
      this.siBtnWorkEmail.MouseDownImage = (Image) null;
      this.siBtnWorkEmail.Name = "siBtnWorkEmail";
      this.siBtnWorkEmail.Size = new Size(16, 16);
      this.siBtnWorkEmail.StandardButtonType = StandardIconButton.ButtonType.EmailButton;
      this.siBtnWorkEmail.TabIndex = 51;
      this.siBtnWorkEmail.TabStop = false;
      this.siBtnWorkEmail.Click += new EventHandler(this.picBoxWorkEmail_Click);
      this.siBtnHomeEmail.BackColor = Color.Transparent;
      this.siBtnHomeEmail.Enabled = false;
      this.siBtnHomeEmail.Location = new Point(309, 192);
      this.siBtnHomeEmail.MouseDownImage = (Image) null;
      this.siBtnHomeEmail.Name = "siBtnHomeEmail";
      this.siBtnHomeEmail.Size = new Size(16, 16);
      this.siBtnHomeEmail.StandardButtonType = StandardIconButton.ButtonType.EmailButton;
      this.siBtnHomeEmail.TabIndex = 50;
      this.siBtnHomeEmail.TabStop = false;
      this.siBtnHomeEmail.Click += new EventHandler(this.picBoxHomeEmail_Click);
      this.siBtnFaxNumber.BackColor = Color.Transparent;
      this.siBtnFaxNumber.Enabled = false;
      this.siBtnFaxNumber.Location = new Point(308, 170);
      this.siBtnFaxNumber.MouseDownImage = (Image) null;
      this.siBtnFaxNumber.Name = "siBtnFaxNumber";
      this.siBtnFaxNumber.Size = new Size(16, 16);
      this.siBtnFaxNumber.StandardButtonType = StandardIconButton.ButtonType.FaxPhoneButton;
      this.siBtnFaxNumber.TabIndex = 49;
      this.siBtnFaxNumber.TabStop = false;
      this.siBtnFaxNumber.Click += new EventHandler(this.picBoxFaxNumber_Click);
      this.siBtnCellPhone.BackColor = Color.Transparent;
      this.siBtnCellPhone.Enabled = false;
      this.siBtnCellPhone.Location = new Point(308, 148);
      this.siBtnCellPhone.MouseDownImage = (Image) null;
      this.siBtnCellPhone.Name = "siBtnCellPhone";
      this.siBtnCellPhone.Size = new Size(16, 16);
      this.siBtnCellPhone.StandardButtonType = StandardIconButton.ButtonType.CellPhoneButton;
      this.siBtnCellPhone.TabIndex = 48;
      this.siBtnCellPhone.TabStop = false;
      this.siBtnCellPhone.Click += new EventHandler(this.picBoxCellPhone_Click);
      this.siBtnWorkPhone.BackColor = Color.Transparent;
      this.siBtnWorkPhone.Enabled = false;
      this.siBtnWorkPhone.Location = new Point(308, 126);
      this.siBtnWorkPhone.MouseDownImage = (Image) null;
      this.siBtnWorkPhone.Name = "siBtnWorkPhone";
      this.siBtnWorkPhone.Size = new Size(16, 16);
      this.siBtnWorkPhone.StandardButtonType = StandardIconButton.ButtonType.PhoneButton;
      this.siBtnWorkPhone.TabIndex = 47;
      this.siBtnWorkPhone.TabStop = false;
      this.siBtnWorkPhone.Click += new EventHandler(this.picBoxWorkPhone_Click);
      this.siBtnHomePhone.BackColor = Color.Transparent;
      this.siBtnHomePhone.Enabled = false;
      this.siBtnHomePhone.Location = new Point(308, 104);
      this.siBtnHomePhone.MouseDownImage = (Image) null;
      this.siBtnHomePhone.Name = "siBtnHomePhone";
      this.siBtnHomePhone.Size = new Size(16, 16);
      this.siBtnHomePhone.StandardButtonType = StandardIconButton.ButtonType.PhoneButton;
      this.siBtnHomePhone.TabIndex = 46;
      this.siBtnHomePhone.TabStop = false;
      this.siBtnHomePhone.Click += new EventHandler(this.picBoxHomePhone_Click);
      this.chkBoxAccess.Location = new Point(241, 346);
      this.chkBoxAccess.Name = "chkBoxAccess";
      this.chkBoxAccess.Size = new Size(57, 20);
      this.chkBoxAccess.TabIndex = 32;
      this.chkBoxAccess.Text = "Public";
      this.chkBoxAccess.CheckedChanged += new EventHandler(this.chkBoxAccess_CheckedChanged);
      this.lblFirstName.Location = new Point(10, 38);
      this.lblFirstName.Name = "lblFirstName";
      this.lblFirstName.Size = new Size(61, 15);
      this.lblFirstName.TabIndex = 0;
      this.lblFirstName.Text = "First Name";
      this.txtBoxPersonalEmail.Location = new Point(151, 190);
      this.txtBoxPersonalEmail.MaxLength = 50;
      this.txtBoxPersonalEmail.Name = "txtBoxPersonalEmail";
      this.txtBoxPersonalEmail.Size = new Size(152, 20);
      this.txtBoxPersonalEmail.TabIndex = 15;
      this.txtBoxPersonalEmail.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxSalutation.Location = new Point(151, 80);
      this.txtBoxSalutation.MaxLength = 84;
      this.txtBoxSalutation.Name = "txtBoxSalutation";
      this.txtBoxSalutation.Size = new Size(173, 20);
      this.txtBoxSalutation.TabIndex = 5;
      this.txtBoxSalutation.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxFaxNumber.Location = new Point(151, 168);
      this.txtBoxFaxNumber.MaxLength = 30;
      this.txtBoxFaxNumber.Name = "txtBoxFaxNumber";
      this.txtBoxFaxNumber.Size = new Size(152, 20);
      this.txtBoxFaxNumber.TabIndex = 13;
      this.txtBoxFaxNumber.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblHomeEmail.Location = new Point(10, 193);
      this.lblHomeEmail.Name = "lblHomeEmail";
      this.lblHomeEmail.Size = new Size(72, 15);
      this.lblHomeEmail.TabIndex = 14;
      this.lblHomeEmail.Text = "Home Email";
      this.label1.Location = new Point(10, 82);
      this.label1.Name = "label1";
      this.label1.Size = new Size(72, 15);
      this.label1.TabIndex = 4;
      this.label1.Text = "Salutation";
      this.lblFaxNumber.Location = new Point(10, 171);
      this.lblFaxNumber.Name = "lblFaxNumber";
      this.lblFaxNumber.Size = new Size(76, 15);
      this.lblFaxNumber.TabIndex = 12;
      this.lblFaxNumber.Text = "Fax Number";
      this.txtBoxBizEmail.Location = new Point(151, 212);
      this.txtBoxBizEmail.MaxLength = 50;
      this.txtBoxBizEmail.Name = "txtBoxBizEmail";
      this.txtBoxBizEmail.Size = new Size(152, 20);
      this.txtBoxBizEmail.TabIndex = 17;
      this.txtBoxBizEmail.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblWorkEmail.Location = new Point(10, 215);
      this.lblWorkEmail.Name = "lblWorkEmail";
      this.lblWorkEmail.Size = new Size(61, 16);
      this.lblWorkEmail.TabIndex = 16;
      this.lblWorkEmail.Text = "Work Email";
      this.lblCellPhone.Location = new Point(10, 149);
      this.lblCellPhone.Name = "lblCellPhone";
      this.lblCellPhone.Size = new Size(76, 15);
      this.lblCellPhone.TabIndex = 10;
      this.lblCellPhone.Text = "Cell Phone";
      this.txtBoxWorkPhone.Location = new Point(151, 124);
      this.txtBoxWorkPhone.MaxLength = 30;
      this.txtBoxWorkPhone.Name = "txtBoxWorkPhone";
      this.txtBoxWorkPhone.Size = new Size(152, 20);
      this.txtBoxWorkPhone.TabIndex = 9;
      this.txtBoxWorkPhone.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxMobilePhone.Location = new Point(151, 146);
      this.txtBoxMobilePhone.MaxLength = 30;
      this.txtBoxMobilePhone.Name = "txtBoxMobilePhone";
      this.txtBoxMobilePhone.Size = new Size(152, 20);
      this.txtBoxMobilePhone.TabIndex = 11;
      this.txtBoxMobilePhone.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblWorkPhone.Location = new Point(10, (int) sbyte.MaxValue);
      this.lblWorkPhone.Name = "lblWorkPhone";
      this.lblWorkPhone.Size = new Size(72, 14);
      this.lblWorkPhone.TabIndex = 8;
      this.lblWorkPhone.Text = "Work Phone";
      this.chkBoxNoSpam.Location = new Point(149, 346);
      this.chkBoxNoSpam.Name = "chkBoxNoSpam";
      this.chkBoxNoSpam.Size = new Size(86, 20);
      this.chkBoxNoSpam.TabIndex = 30;
      this.chkBoxNoSpam.Text = "Do Not Email";
      this.chkBoxNoSpam.Click += new EventHandler(this.summaryFieldChanged);
      this.txtBoxHomePhone.Location = new Point(151, 102);
      this.txtBoxHomePhone.MaxLength = 30;
      this.txtBoxHomePhone.Name = "txtBoxHomePhone";
      this.txtBoxHomePhone.Size = new Size(152, 20);
      this.txtBoxHomePhone.TabIndex = 7;
      this.txtBoxHomePhone.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblHomePhone.Location = new Point(10, 105);
      this.lblHomePhone.Name = "lblHomePhone";
      this.lblHomePhone.Size = new Size(72, 16);
      this.lblHomePhone.TabIndex = 6;
      this.lblHomePhone.Text = "Home Phone";
      this.txtBoxLastName.Location = new Point(151, 58);
      this.txtBoxLastName.MaxLength = 50;
      this.txtBoxLastName.Name = "txtBoxLastName";
      this.txtBoxLastName.Size = new Size(173, 20);
      this.txtBoxLastName.TabIndex = 3;
      this.txtBoxLastName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxFirstName.Location = new Point(151, 36);
      this.txtBoxFirstName.MaxLength = 50;
      this.txtBoxFirstName.Name = "txtBoxFirstName";
      this.txtBoxFirstName.Size = new Size(173, 20);
      this.txtBoxFirstName.TabIndex = 1;
      this.txtBoxFirstName.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.lblLastName.Location = new Point(10, 61);
      this.lblLastName.Name = "lblLastName";
      this.lblLastName.Size = new Size(72, 15);
      this.lblLastName.TabIndex = 2;
      this.lblLastName.Text = "Last Name";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(1010, 380);
      this.Controls.Add((Control) this.pnlMiddle);
      this.Controls.Add((Control) this.gcBizCatAddFields);
      this.Controls.Add((Control) this.gcPersonalInfo);
      this.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BizPartnerInfo1Form);
      this.Padding = new Padding(1);
      this.Text = "BizPartnerInfoForm";
      this.SizeChanged += new EventHandler(this.BizPartnerInfoForm_SizeChanged);
      this.pnlMiddle.ResumeLayout(false);
      this.gcBusinessInfo.ResumeLayout(false);
      this.gcBusinessInfo.PerformLayout();
      this.gcBizCatAddFields.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.gcPersonalInfo.ResumeLayout(false);
      this.gcPersonalInfo.PerformLayout();
      ((ISupportInitialize) this.siBtnWorkEmail).EndInit();
      ((ISupportInitialize) this.siBtnHomeEmail).EndInit();
      ((ISupportInitialize) this.siBtnFaxNumber).EndInit();
      ((ISupportInitialize) this.siBtnCellPhone).EndInit();
      ((ISupportInitialize) this.siBtnWorkPhone).EndInit();
      ((ISupportInitialize) this.siBtnHomePhone).EndInit();
      this.ResumeLayout(false);
    }

    private void chkBoxAccess_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      if (!this.chkBoxAccess.Checked)
      {
        if (!this.checkPrivateRight())
        {
          if (Session.EncompassEdition == EncompassEdition.Banker)
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "You do not have edit right to one or more groups that this contact belongs to.  Please contact the Admin for help", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "You do not have access to manage public business contacts.  Please contact the Admin for help", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          this.chkBoxAccess.CheckedChanged -= new EventHandler(this.chkBoxAccess_CheckedChanged);
          this.chkBoxAccess.Checked = true;
          this.chkBoxAccess.CheckedChanged += new EventHandler(this.chkBoxAccess_CheckedChanged);
        }
        else if (DialogResult.OK == Utils.Dialog((IWin32Window) this, "You are about to make this public business contact your own private business contact.  Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
        {
          this.contactInfo.OwnerID = Session.UserID;
          this.contactInfo.AccessLevel = ContactAccess.Private;
          Session.ContactManager.UpdateBizPartner(this.contactInfo);
          ContactGroupInfo[] groupsForContact = Session.ContactGroupManager.GetContactGroupsForContact(ContactType.PublicBiz, this.contactInfo.ContactID);
          if (groupsForContact != null && groupsForContact.Length != 0)
          {
            foreach (ContactGroupInfo groupInfo in groupsForContact)
            {
              groupInfo.DeletedContactIds = new int[1]
              {
                this.contactInfo.ContactID
              };
              Session.ContactGroupManager.SaveContactGroup(groupInfo);
            }
          }
          this.changed = true;
          this.tabForm.TriggerSave();
        }
        else
        {
          this.chkBoxAccess.CheckedChanged -= new EventHandler(this.chkBoxAccess_CheckedChanged);
          this.chkBoxAccess.Checked = true;
          this.chkBoxAccess.CheckedChanged += new EventHandler(this.chkBoxAccess_CheckedChanged);
        }
      }
      else if (Session.EncompassEdition == EncompassEdition.Banker)
      {
        ContactGroupSelectionDlg groupSelectionDlg = new ContactGroupSelectionDlg(ContactType.PublicBiz, true);
        if (groupSelectionDlg.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        {
          this.chkBoxAccess.CheckedChanged -= new EventHandler(this.chkBoxAccess_CheckedChanged);
          this.chkBoxAccess.Checked = false;
          this.chkBoxAccess.CheckedChanged += new EventHandler(this.chkBoxAccess_CheckedChanged);
        }
        else
        {
          ContactGroupInfo[] selectedGroups = groupSelectionDlg.SelectedGroups;
          if (selectedGroups == null || selectedGroups.Length == 0)
            return;
          for (int index = 0; index < selectedGroups.Length; ++index)
          {
            selectedGroups[index].AddedContactIds = new int[1]
            {
              this.CurrentContactID
            };
            Session.ContactGroupManager.SaveContactGroup(selectedGroups[index]);
          }
          this.contactInfo.OwnerID = "";
          this.changed = true;
          this.tabForm.TriggerSave();
        }
      }
      else
      {
        ContactGroupInfo[] bizContactGroups = Session.ContactGroupManager.GetPublicBizContactGroups();
        if (bizContactGroups != null && bizContactGroups.Length == 0)
          return;
        for (int index = 0; index < bizContactGroups.Length; ++index)
        {
          bizContactGroups[index].AddedContactIds = new int[1]
          {
            this.CurrentContactID
          };
          Session.ContactGroupManager.SaveContactGroup(bizContactGroups[index]);
        }
        this.contactInfo.OwnerID = "";
        this.changed = true;
        this.tabForm.TriggerSave();
      }
    }

    public void MakePublic()
    {
      this.chkBoxAccess.CheckedChanged -= new EventHandler(this.chkBoxAccess_CheckedChanged);
      this.chkBoxAccess.Checked = true;
      this.chkBoxAccess.CheckedChanged += new EventHandler(this.chkBoxAccess_CheckedChanged);
    }

    private void txtBoxBizWebUrl_Leave(object sender, EventArgs e)
    {
      string text = ((Control) sender).Text;
      if (text == "" || SystemUtil.IsValidURL(text))
        return;
      int num = (int) MessageBox.Show("Invalid Web URL.", "Web URL", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void PersonalInfocalenderBtnIssueDate_DateSelected(
      object sender,
      CalendarPopupEventArgs e)
    {
    }

    private void BizCalenderIssueDate_DateSelected(object sender, CalendarPopupEventArgs e)
    {
    }

    public delegate void CategoryChanged(int categoryID);
  }
}
