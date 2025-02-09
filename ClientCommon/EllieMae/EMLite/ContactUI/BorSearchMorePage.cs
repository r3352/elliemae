// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorSearchMorePage
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BorSearchMorePage : MoreSearchForm
  {
    private System.ComponentModel.Container components;
    private ContextMenu fieldMenu;
    private MenuItem menuItemDetails;
    private MenuItem menuItemDetails_HomePhone;
    private MenuItem menuItemDetails_WorkPhone;
    private MenuItem menuItemDetails_CellPhone;
    private MenuItem menuItemDetails_FaxNumber;
    private MenuItem menuItemDetails_PersonalEmail;
    private MenuItem menuItemDetails_WorkEmail;
    private MenuItem menuItem11;
    private MenuItem menuItemHomeAddress;
    private MenuItem menuItemHomeAddress1;
    private MenuItem menuItemHomeAddress2;
    private MenuItem menuItemHomeAddressCity;
    private MenuItem menuItemHomeAddressState;
    private MenuItem menuItemHomeAddressZip;
    private MenuItem menuItem19;
    private MenuItem menuItemNoCall;
    private MenuItem menuItemNoEmail;
    private MenuItem menuItemNoFax;
    private MenuItem menuItemExtra;
    private MenuItem menuItemCompany;
    private MenuItem menuItemTitle;
    private MenuItem menuItemBizAddress;
    private MenuItem menuItemBizAddress1;
    private MenuItem menuItemBizAddress2;
    private MenuItem menuItemBizAddressCity;
    private MenuItem menuItemBizAddressState;
    private MenuItem menuItemBizAddressZip;
    private MenuItem menuItemWebUrl;
    private MenuItem menuItem31;
    private MenuItem menuItemReferral;
    private MenuItem menuItemAnnualIncome;
    private MenuItem menuItem35;
    private MenuItem menuItemMarried;
    private MenuItem menuItemSpouse;
    private MenuItem menuItemOpportunity;
    private MenuItem menuItemLoanAmount;
    private MenuItem menuItemLoanPurpose;
    private MenuItem menuItemLoanTerm;
    private MenuItem menuItemAmortization;
    private MenuItem menuItemDownPayment;
    private MenuItem menuItemMortgageBalance;
    private MenuItem menuItemMortgageRate;
    private MenuItem menuItemHousingPayment;
    private MenuItem menuItemNonHousingPayment;
    private MenuItem menuItem48;
    private MenuItem menuItemPropertyAddress;
    private MenuItem menuItemPropertyAddress1;
    private MenuItem menuItemPropertyAddressCity;
    private MenuItem menuItemPropertyAddressState;
    private MenuItem menuItemPropertyAddressZip;
    private MenuItem menuItemPropertyUse;
    private MenuItem menuItemPropertyType;
    private MenuItem menuItemPropertyValue;
    private MenuItem menuItemPurchaseDate;
    private MenuItem menuItem57;
    private MenuItem menuItemCreditRating;
    private MenuItem menuItemBankruptcy;
    private MenuItem menuItemEmployment;
    private MenuItem menuItemStatus;
    private MenuItem menuItem2;
    private MenuItem menuItemLoanPurposeOther;
    private MenuItem menuItemCustomFields;
    private MenuItem menuItemPrimaryContact;
    private MenuItem menuItemSSN;
    private MenuItem menuItem3;
    private ContactSearchContext searchContext;

    public BorSearchMorePage(ContactSearchContext searchContext)
    {
      this.InitializeComponent();
      this.searchContext = searchContext;
      this.init();
    }

    private void initForCtx()
    {
      ContactSearchContext searchContext = this.searchContext;
      if (searchContext == ContactSearchContext.Contacts)
        return;
      int num = (int) (searchContext - 1);
    }

    private void init()
    {
      this.initForCtx();
      this.loadMenuItems();
      this.setContextMenu(this.fieldMenu);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.fieldMenu = new ContextMenu();
      this.menuItemDetails = new MenuItem();
      this.menuItemDetails_HomePhone = new MenuItem();
      this.menuItemDetails_WorkPhone = new MenuItem();
      this.menuItemDetails_CellPhone = new MenuItem();
      this.menuItemDetails_FaxNumber = new MenuItem();
      this.menuItemDetails_PersonalEmail = new MenuItem();
      this.menuItemDetails_WorkEmail = new MenuItem();
      this.menuItem11 = new MenuItem();
      this.menuItemHomeAddress = new MenuItem();
      this.menuItemHomeAddress1 = new MenuItem();
      this.menuItemHomeAddress2 = new MenuItem();
      this.menuItemHomeAddressCity = new MenuItem();
      this.menuItemHomeAddressState = new MenuItem();
      this.menuItemHomeAddressZip = new MenuItem();
      this.menuItem19 = new MenuItem();
      this.menuItemNoCall = new MenuItem();
      this.menuItemNoEmail = new MenuItem();
      this.menuItemNoFax = new MenuItem();
      this.menuItemPrimaryContact = new MenuItem();
      this.menuItemExtra = new MenuItem();
      this.menuItemStatus = new MenuItem();
      this.menuItem2 = new MenuItem();
      this.menuItemCompany = new MenuItem();
      this.menuItemTitle = new MenuItem();
      this.menuItemBizAddress = new MenuItem();
      this.menuItemBizAddress1 = new MenuItem();
      this.menuItemBizAddress2 = new MenuItem();
      this.menuItemBizAddressCity = new MenuItem();
      this.menuItemBizAddressState = new MenuItem();
      this.menuItemBizAddressZip = new MenuItem();
      this.menuItemWebUrl = new MenuItem();
      this.menuItem31 = new MenuItem();
      this.menuItemReferral = new MenuItem();
      this.menuItemAnnualIncome = new MenuItem();
      this.menuItem35 = new MenuItem();
      this.menuItemMarried = new MenuItem();
      this.menuItemSpouse = new MenuItem();
      this.menuItemOpportunity = new MenuItem();
      this.menuItemLoanAmount = new MenuItem();
      this.menuItemLoanPurpose = new MenuItem();
      this.menuItemLoanPurposeOther = new MenuItem();
      this.menuItemLoanTerm = new MenuItem();
      this.menuItemAmortization = new MenuItem();
      this.menuItemDownPayment = new MenuItem();
      this.menuItemMortgageBalance = new MenuItem();
      this.menuItemMortgageRate = new MenuItem();
      this.menuItemHousingPayment = new MenuItem();
      this.menuItemNonHousingPayment = new MenuItem();
      this.menuItem48 = new MenuItem();
      this.menuItemPropertyAddress = new MenuItem();
      this.menuItemPropertyAddress1 = new MenuItem();
      this.menuItemPropertyAddressCity = new MenuItem();
      this.menuItemPropertyAddressState = new MenuItem();
      this.menuItemPropertyAddressZip = new MenuItem();
      this.menuItemPropertyUse = new MenuItem();
      this.menuItemPropertyType = new MenuItem();
      this.menuItemPropertyValue = new MenuItem();
      this.menuItemPurchaseDate = new MenuItem();
      this.menuItem57 = new MenuItem();
      this.menuItemCreditRating = new MenuItem();
      this.menuItemBankruptcy = new MenuItem();
      this.menuItemEmployment = new MenuItem();
      this.menuItemCustomFields = new MenuItem();
      this.menuItemSSN = new MenuItem();
      this.menuItem3 = new MenuItem();
      this.txtBoxContactFieldName.Name = "txtBoxContactFieldName";
      this.fieldMenu.MenuItems.AddRange(new MenuItem[4]
      {
        this.menuItemDetails,
        this.menuItemExtra,
        this.menuItemOpportunity,
        this.menuItemCustomFields
      });
      this.menuItemDetails.Index = 0;
      this.menuItemDetails.MenuItems.AddRange(new MenuItem[15]
      {
        this.menuItemSSN,
        this.menuItemPrimaryContact,
        this.menuItem3,
        this.menuItemDetails_HomePhone,
        this.menuItemDetails_WorkPhone,
        this.menuItemDetails_CellPhone,
        this.menuItemDetails_FaxNumber,
        this.menuItemDetails_PersonalEmail,
        this.menuItemDetails_WorkEmail,
        this.menuItem11,
        this.menuItemHomeAddress,
        this.menuItem19,
        this.menuItemNoCall,
        this.menuItemNoEmail,
        this.menuItemNoFax
      });
      this.menuItemDetails.Text = "Details";
      this.menuItemDetails_HomePhone.Index = 3;
      this.menuItemDetails_HomePhone.Text = "Home Phone";
      this.menuItemDetails_HomePhone.Click += new EventHandler(this.menuItemDetails_HomePhone_Click);
      this.menuItemDetails_WorkPhone.Index = 4;
      this.menuItemDetails_WorkPhone.Text = "Work Phone";
      this.menuItemDetails_WorkPhone.Click += new EventHandler(this.menuItemDetails_WorkPhone_Click);
      this.menuItemDetails_CellPhone.Index = 5;
      this.menuItemDetails_CellPhone.Text = "Cell Phone";
      this.menuItemDetails_CellPhone.Click += new EventHandler(this.menuItemDetails_CellPhone_Click);
      this.menuItemDetails_FaxNumber.Index = 6;
      this.menuItemDetails_FaxNumber.Text = "Fax Number";
      this.menuItemDetails_FaxNumber.Click += new EventHandler(this.menuItemDetails_FaxNumber_Click);
      this.menuItemDetails_PersonalEmail.Index = 7;
      this.menuItemDetails_PersonalEmail.Text = "Personal Email";
      this.menuItemDetails_PersonalEmail.Click += new EventHandler(this.menuItemDetails_PersonalEmail_Click);
      this.menuItemDetails_WorkEmail.Index = 8;
      this.menuItemDetails_WorkEmail.Text = "Work Email";
      this.menuItemDetails_WorkEmail.Click += new EventHandler(this.menuItemDetails_WorkEmail_Click);
      this.menuItem11.Index = 9;
      this.menuItem11.Text = "-";
      this.menuItemHomeAddress.Index = 10;
      this.menuItemHomeAddress.MenuItems.AddRange(new MenuItem[5]
      {
        this.menuItemHomeAddress1,
        this.menuItemHomeAddress2,
        this.menuItemHomeAddressCity,
        this.menuItemHomeAddressState,
        this.menuItemHomeAddressZip
      });
      this.menuItemHomeAddress.Text = "Home Address";
      this.menuItemHomeAddress1.Index = 0;
      this.menuItemHomeAddress1.Text = "Address1";
      this.menuItemHomeAddress1.Click += new EventHandler(this.menuItemHomeAddress1_Click);
      this.menuItemHomeAddress2.Index = 1;
      this.menuItemHomeAddress2.Text = "Address2";
      this.menuItemHomeAddress2.Click += new EventHandler(this.menuItemHomeAddress2_Click);
      this.menuItemHomeAddressCity.Index = 2;
      this.menuItemHomeAddressCity.Text = "City";
      this.menuItemHomeAddressCity.Click += new EventHandler(this.menuItemHomeAddressCity_Click);
      this.menuItemHomeAddressState.Index = 3;
      this.menuItemHomeAddressState.Text = "State";
      this.menuItemHomeAddressState.Click += new EventHandler(this.menuItemHomeAddressState_Click);
      this.menuItemHomeAddressZip.Index = 4;
      this.menuItemHomeAddressZip.Text = "Zip";
      this.menuItemHomeAddressZip.Click += new EventHandler(this.menuItemHomeAddressZip_Click);
      this.menuItem19.Index = 11;
      this.menuItem19.Text = "-";
      this.menuItemNoCall.Index = 12;
      this.menuItemNoCall.Text = "Do not call";
      this.menuItemNoCall.Click += new EventHandler(this.menuItemNoCall_Click);
      this.menuItemNoEmail.Index = 13;
      this.menuItemNoEmail.Text = "Do not email";
      this.menuItemNoEmail.Click += new EventHandler(this.menuItemNoEmail_Click);
      this.menuItemNoFax.Index = 14;
      this.menuItemNoFax.Text = "Do not fax";
      this.menuItemNoFax.Click += new EventHandler(this.menuItemNoFax_Click);
      this.menuItemPrimaryContact.Index = 1;
      this.menuItemPrimaryContact.Text = "Primary Contact";
      this.menuItemPrimaryContact.Click += new EventHandler(this.menuItemPrimaryContact_Click);
      this.menuItemExtra.Index = 1;
      this.menuItemExtra.MenuItems.AddRange(new MenuItem[12]
      {
        this.menuItemStatus,
        this.menuItem2,
        this.menuItemCompany,
        this.menuItemTitle,
        this.menuItemBizAddress,
        this.menuItemWebUrl,
        this.menuItem31,
        this.menuItemReferral,
        this.menuItemAnnualIncome,
        this.menuItem35,
        this.menuItemMarried,
        this.menuItemSpouse
      });
      this.menuItemExtra.Text = "Extra";
      this.menuItemStatus.Index = 0;
      this.menuItemStatus.Text = "Status";
      this.menuItemStatus.Click += new EventHandler(this.menuItemStatus_Click);
      this.menuItem2.Index = 1;
      this.menuItem2.Text = "-";
      this.menuItemCompany.Index = 2;
      this.menuItemCompany.Text = "Company";
      this.menuItemCompany.Click += new EventHandler(this.menuItemCompany_Click);
      this.menuItemTitle.Index = 3;
      this.menuItemTitle.Text = "Title";
      this.menuItemTitle.Click += new EventHandler(this.menuItemTitle_Click);
      this.menuItemBizAddress.Index = 4;
      this.menuItemBizAddress.MenuItems.AddRange(new MenuItem[5]
      {
        this.menuItemBizAddress1,
        this.menuItemBizAddress2,
        this.menuItemBizAddressCity,
        this.menuItemBizAddressState,
        this.menuItemBizAddressZip
      });
      this.menuItemBizAddress.Text = "Business Address";
      this.menuItemBizAddress1.Index = 0;
      this.menuItemBizAddress1.Text = "Address1";
      this.menuItemBizAddress1.Click += new EventHandler(this.menuItemBizAddress1_Click);
      this.menuItemBizAddress2.Index = 1;
      this.menuItemBizAddress2.Text = "Address2";
      this.menuItemBizAddress2.Click += new EventHandler(this.menuItemBizAddress2_Click);
      this.menuItemBizAddressCity.Index = 2;
      this.menuItemBizAddressCity.Text = "City";
      this.menuItemBizAddressCity.Click += new EventHandler(this.menuItemBizAddressCity_Click);
      this.menuItemBizAddressState.Index = 3;
      this.menuItemBizAddressState.Text = "State";
      this.menuItemBizAddressState.Click += new EventHandler(this.menuItemBizAddressState_Click);
      this.menuItemBizAddressZip.Index = 4;
      this.menuItemBizAddressZip.Text = "Zip";
      this.menuItemBizAddressZip.Click += new EventHandler(this.menuItemBizAddressZip_Click);
      this.menuItemWebUrl.Index = 5;
      this.menuItemWebUrl.Text = "Web URL";
      this.menuItemWebUrl.Click += new EventHandler(this.menuItemWebUrl_Click);
      this.menuItem31.Index = 6;
      this.menuItem31.Text = "-";
      this.menuItemReferral.Index = 7;
      this.menuItemReferral.Text = "Referral";
      this.menuItemReferral.Click += new EventHandler(this.menuItemReferral_Click);
      this.menuItemAnnualIncome.Index = 8;
      this.menuItemAnnualIncome.Text = "Annual Income";
      this.menuItemAnnualIncome.Click += new EventHandler(this.menuItemAnnualIncome_Click);
      this.menuItem35.Index = 9;
      this.menuItem35.Text = "-";
      this.menuItemMarried.Index = 10;
      this.menuItemMarried.Text = "Married";
      this.menuItemMarried.Click += new EventHandler(this.menuItemMarried_Click);
      this.menuItemSpouse.Index = 11;
      this.menuItemSpouse.Text = "Spouse ";
      this.menuItemSpouse.Click += new EventHandler(this.menuItemSpouse_Click);
      this.menuItemOpportunity.Index = 2;
      this.menuItemOpportunity.MenuItems.AddRange(new MenuItem[20]
      {
        this.menuItemLoanAmount,
        this.menuItemLoanPurpose,
        this.menuItemLoanPurposeOther,
        this.menuItemLoanTerm,
        this.menuItemAmortization,
        this.menuItemDownPayment,
        this.menuItemMortgageBalance,
        this.menuItemMortgageRate,
        this.menuItemHousingPayment,
        this.menuItemNonHousingPayment,
        this.menuItem48,
        this.menuItemPropertyAddress,
        this.menuItemPropertyUse,
        this.menuItemPropertyType,
        this.menuItemPropertyValue,
        this.menuItemPurchaseDate,
        this.menuItem57,
        this.menuItemCreditRating,
        this.menuItemBankruptcy,
        this.menuItemEmployment
      });
      this.menuItemOpportunity.Text = "Opportunity";
      this.menuItemLoanAmount.Index = 0;
      this.menuItemLoanAmount.Text = "Loan Amount";
      this.menuItemLoanAmount.Click += new EventHandler(this.menuItemLoanAmount_Click);
      this.menuItemLoanPurpose.Index = 1;
      this.menuItemLoanPurpose.Text = "Loan Purpose";
      this.menuItemLoanPurpose.Click += new EventHandler(this.menuItemLoanPurpose_Click);
      this.menuItemLoanPurposeOther.Index = 2;
      this.menuItemLoanPurposeOther.Text = "Loan Purpose -- Other";
      this.menuItemLoanPurposeOther.Click += new EventHandler(this.menuItemLoanPurposeOther_Click);
      this.menuItemLoanTerm.Index = 3;
      this.menuItemLoanTerm.Text = "Loan Term";
      this.menuItemLoanTerm.Click += new EventHandler(this.menuItemLoanTerm_Click);
      this.menuItemAmortization.Index = 4;
      this.menuItemAmortization.Text = "Amortization";
      this.menuItemAmortization.Click += new EventHandler(this.menuItemAmortization_Click);
      this.menuItemDownPayment.Index = 5;
      this.menuItemDownPayment.Text = "Available Down Payment";
      this.menuItemDownPayment.Click += new EventHandler(this.menuItemDownPayment_Click);
      this.menuItemMortgageBalance.Index = 6;
      this.menuItemMortgageBalance.Text = "Current Mortgage Balance";
      this.menuItemMortgageBalance.Click += new EventHandler(this.menuItemMortgageBalance_Click);
      this.menuItemMortgageRate.Index = 7;
      this.menuItemMortgageRate.Text = "Current Mortgage Rate";
      this.menuItemMortgageRate.Click += new EventHandler(this.menuItemMortgageRate_Click);
      this.menuItemHousingPayment.Index = 8;
      this.menuItemHousingPayment.Text = "Monthly Payment (housing)";
      this.menuItemHousingPayment.Click += new EventHandler(this.menuItemHousingPayment_Click);
      this.menuItemNonHousingPayment.Index = 9;
      this.menuItemNonHousingPayment.Text = "Monthly Payment (non-housing)";
      this.menuItemNonHousingPayment.Click += new EventHandler(this.menuItemNonHousingPayment_Click);
      this.menuItem48.Index = 10;
      this.menuItem48.Text = "-";
      this.menuItemPropertyAddress.Index = 11;
      this.menuItemPropertyAddress.MenuItems.AddRange(new MenuItem[4]
      {
        this.menuItemPropertyAddress1,
        this.menuItemPropertyAddressCity,
        this.menuItemPropertyAddressState,
        this.menuItemPropertyAddressZip
      });
      this.menuItemPropertyAddress.Text = "Property Address";
      this.menuItemPropertyAddress1.Index = 0;
      this.menuItemPropertyAddress1.Text = "Address";
      this.menuItemPropertyAddress1.Click += new EventHandler(this.menuItemPropertyAddress1_Click);
      this.menuItemPropertyAddressCity.Index = 1;
      this.menuItemPropertyAddressCity.Text = "City";
      this.menuItemPropertyAddressCity.Click += new EventHandler(this.menuItemPropertyAddressCity_Click);
      this.menuItemPropertyAddressState.Index = 2;
      this.menuItemPropertyAddressState.Text = "State";
      this.menuItemPropertyAddressState.Click += new EventHandler(this.menuItemPropertyAddressState_Click);
      this.menuItemPropertyAddressZip.Index = 3;
      this.menuItemPropertyAddressZip.Text = "Zip";
      this.menuItemPropertyAddressZip.Click += new EventHandler(this.menuItemPropertyAddressZip_Click);
      this.menuItemPropertyUse.Index = 12;
      this.menuItemPropertyUse.Text = "Property Use";
      this.menuItemPropertyUse.Click += new EventHandler(this.menuItemPropertyUse_Click);
      this.menuItemPropertyType.Index = 13;
      this.menuItemPropertyType.Text = "Property Type";
      this.menuItemPropertyType.Click += new EventHandler(this.menuItemPropertyType_Click);
      this.menuItemPropertyValue.Index = 14;
      this.menuItemPropertyValue.Text = "Property Value";
      this.menuItemPropertyValue.Click += new EventHandler(this.menuItemPropertyValue_Click);
      this.menuItemPurchaseDate.Index = 15;
      this.menuItemPurchaseDate.Text = "Purchase Date";
      this.menuItemPurchaseDate.Click += new EventHandler(this.menuItemPurchaseDate_Click);
      this.menuItem57.Index = 16;
      this.menuItem57.Text = "-";
      this.menuItemCreditRating.Index = 17;
      this.menuItemCreditRating.Text = "Credit Rating";
      this.menuItemCreditRating.Click += new EventHandler(this.menuItemCreditRating_Click);
      this.menuItemBankruptcy.Index = 18;
      this.menuItemBankruptcy.Text = "Bankruptcy";
      this.menuItemBankruptcy.Click += new EventHandler(this.menuItemBankruptcy_Click);
      this.menuItemEmployment.Index = 19;
      this.menuItemEmployment.Text = "Employment";
      this.menuItemEmployment.Click += new EventHandler(this.menuItemEmployment_Click);
      this.menuItemCustomFields.Index = 3;
      this.menuItemCustomFields.Text = "Custom Fields";
      this.menuItemSSN.Index = 0;
      this.menuItemSSN.Text = "SSN";
      this.menuItemSSN.Click += new EventHandler(this.menuItemSSN_Click);
      this.menuItem3.Index = 2;
      this.menuItem3.Text = "-";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(456, 334);
      this.Name = nameof (BorSearchMorePage);
    }

    private ContactCustomFieldInfoCollection getCustomFields()
    {
      return Session.ContactManager.GetCustomFieldInfo(ContactType.Borrower);
    }

    private void loadMenuItems()
    {
      ContactCustomFieldInfoCollection customFields = this.getCustomFields();
      ContactCustomFieldInfo[] items = customFields.Items;
      string[] strArray = new string[5]
      {
        customFields.Page1Name,
        customFields.Page2Name,
        customFields.Page3Name,
        customFields.Page4Name,
        customFields.Page5Name
      };
      Array.Sort<ContactCustomFieldInfo>(items);
      ArrayList[] arrayListArray = new ArrayList[5]
      {
        new ArrayList(),
        new ArrayList(),
        new ArrayList(),
        new ArrayList(),
        new ArrayList()
      };
      for (int index1 = 0; index1 < items.Length; ++index1)
      {
        int index2 = (int) Math.Floor((double) (items[index1].LabelID - 1) / 20.0);
        arrayListArray[index2].Add((object) items[index1]);
      }
      this._MenuItemToField.Clear();
      for (int index3 = 0; index3 < 5; ++index3)
      {
        if (arrayListArray[index3].Count > 0)
        {
          MenuItem menuItem = new MenuItem();
          menuItem.Text = strArray[index3];
          menuItem.Index = index3;
          this.menuItemCustomFields.MenuItems.Add(menuItem);
          ArrayList arrayList = arrayListArray[index3];
          for (int index4 = 0; index4 < arrayList.Count; ++index4)
          {
            MenuItem key = new MenuItem();
            key.Text = ((ContactCustomFieldInfo) arrayList[index4]).Label;
            key.Index = index4;
            key.Click += new EventHandler(this.menuItemField_Click);
            menuItem.MenuItems.Add(key);
            this._MenuItemToField.Add((object) key, arrayList[index4]);
          }
        }
      }
    }

    private void menuItemField_Click(object sender, EventArgs e)
    {
      this.isFieldBool = false;
      MenuItem key = (MenuItem) sender;
      ContactCustomFieldInfo contactCustomFieldInfo = (ContactCustomFieldInfo) this._MenuItemToField[(object) key];
      this.txtBoxContactFieldName.Text = key.Text;
      this._DbColumnName = "Custom." + contactCustomFieldInfo.Label;
      this._FieldFormat = contactCustomFieldInfo.FieldType;
      if (contactCustomFieldInfo.FieldType == FieldFormat.DROPDOWN)
        this.setControlsForDropDownField((object[]) contactCustomFieldInfo.FieldOptions);
      else if (contactCustomFieldInfo.FieldType == FieldFormat.DROPDOWNLIST)
        this.setControlsForDropDownListField((object[]) contactCustomFieldInfo.FieldOptions);
      else
        this.setControlsForNonDropDownField();
    }

    private void menuItemDetails_HomePhone_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Home Phone";
      this._DbColumnName = "Contact.HomePhone";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemDetails_WorkPhone_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Phone";
      this._DbColumnName = "Contact.WorkPhone";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemDetails_CellPhone_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Cell Phone";
      this._DbColumnName = "Contact.MobilePhone";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemDetails_FaxNumber_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Fax Number";
      this._DbColumnName = "Contact.FaxNumber";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemDetails_PersonalEmail_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Personal Email";
      this._DbColumnName = "Contact.PersonalEmail";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemDetails_WorkEmail_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Email";
      this._DbColumnName = "Contact.BizEmail";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemHomeAddress1_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Home Address1";
      this._DbColumnName = "Contact.HomeAddress1";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemHomeAddress2_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Home Address2";
      this._DbColumnName = "Contact.HomeAddress2";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemHomeAddressCity_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Home Address City";
      this._DbColumnName = "Contact.HomeCity";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemHomeAddressState_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Home Address State";
      this._DbColumnName = "Contact.HomeState";
      this._FieldFormat = FieldFormat.STATE;
    }

    private void menuItemHomeAddressZip_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Home Address Zip";
      this._DbColumnName = "Contact.HomeZip";
      this._FieldFormat = FieldFormat.ZIPCODE;
    }

    private void menuItemNoCall_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Do not call";
      this._DbColumnName = "Contact.NoCall";
      this._FieldFormat = FieldFormat.X;
      this.isFieldBool = true;
    }

    private void menuItemNoEmail_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Do not email";
      this._DbColumnName = "Contact.NoSpam";
      this._FieldFormat = FieldFormat.X;
      this.isFieldBool = true;
    }

    private void menuItemNoFax_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Do not fax";
      this._DbColumnName = "Contact.NoFax";
      this._FieldFormat = FieldFormat.X;
      this.isFieldBool = true;
    }

    private void menuItemStatus_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Status";
      this._DbColumnName = "Contact.Status";
      this._FieldFormat = FieldFormat.STRING;
      this.setControlsForDropDownField(new ArrayList((ICollection) Session.ContactManager.GetBorrowerStatus().Items).ToArray());
    }

    private void menuItemCompany_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Company";
      this._DbColumnName = "Contact.EmployerName";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemTitle_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Title";
      this._DbColumnName = "Contact.JobTitle";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBizAddress1_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Address1";
      this._DbColumnName = "Contact.BizAddress1";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBizAddress2_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Address2";
      this._DbColumnName = "Contact.BizAddress2";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBizAddressCity_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Address City";
      this._DbColumnName = "Contact.BizCity";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBizAddressState_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Address State";
      this._DbColumnName = "Contact.BizState";
      this._FieldFormat = FieldFormat.STATE;
    }

    private void menuItemBizAddressZip_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Address Zip";
      this._DbColumnName = "Contact.BizZip";
      this._FieldFormat = FieldFormat.ZIPCODE;
    }

    private void menuItemWebUrl_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Web Url";
      this._DbColumnName = "Contact.BizWebUrl";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemSSN_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "SSN";
      this._DbColumnName = "Contact.SSN";
      this._FieldFormat = FieldFormat.SSN;
    }

    private void menuItemReferral_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Referral";
      this._DbColumnName = "Contact.Referral";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemAnnualIncome_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Annual Income";
      this._DbColumnName = "Contact.Income";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemMarried_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Married";
      this._DbColumnName = "Contact.Married";
      this._FieldFormat = FieldFormat.X;
      this.isFieldBool = true;
    }

    private void menuItemSpouse_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Spouse";
      this._DbColumnName = "Contact.SpouseName";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemLoanAmount_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Loan Amount";
      this._DbColumnName = "Opportunity.LoanAmount";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemLoanPurpose_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Loan Purpose";
      this._DbColumnName = "Opportunity.Purpose";
      this._FieldFormat = FieldFormat.STRING;
      this.setControlsForDropDownListField(LoanPurposeEnumUtil.GetDisplayNames());
    }

    private void menuItemLoanTerm_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Loan Term";
      this._DbColumnName = "Opportunity.Term";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemAmortization_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Amortization";
      this._DbColumnName = "Opportunity.Amortization";
      this._FieldFormat = FieldFormat.STRING;
      this.setControlsForDropDownListField(AmortizationTypeEnumUtil.GetDisplayNames());
    }

    private void menuItemDownPayment_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Down Payment";
      this._DbColumnName = "Opportunity.DownPayment";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemMortgageBalance_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Mortgage Balance";
      this._DbColumnName = "Opportunity.MortgageBalance";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemMortgageRate_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Mortgage Rate";
      this._DbColumnName = "Opportunity.MortgageRate";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemHousingPayment_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Housing Payment";
      this._DbColumnName = "Opportunity.HousingPayment";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemNonHousingPayment_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Non-Housing Payment";
      this._DbColumnName = "Opportunity.NonHousingPayment";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemPropertyAddress1_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Property Address Street";
      this._DbColumnName = "Opportunity.PropertyAddress";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemPropertyAddressCity_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Property Address City";
      this._DbColumnName = "Opportunity.PropertyCity";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemPropertyAddressState_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Property Address State";
      this._DbColumnName = "Opportunity.PropertyState";
      this._FieldFormat = FieldFormat.STATE;
    }

    private void menuItemPropertyAddressZip_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Property Address Zip";
      this._DbColumnName = "Opportunity.PropertyZip";
      this._FieldFormat = FieldFormat.ZIPCODE;
    }

    private void menuItemPropertyUse_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Property Use";
      this._DbColumnName = "Opportunity.PropertyUse";
      this._FieldFormat = FieldFormat.STRING;
      this.setControlsForDropDownListField(PropertyUseEnumUtil.GetDisplayNames());
    }

    private void menuItemPropertyType_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Property Type";
      this._DbColumnName = "Opportunity.PropertyType";
      this._FieldFormat = FieldFormat.STRING;
      this.setControlsForDropDownListField(PropertyTypeEnumUtil.GetDisplayNames());
    }

    private void menuItemPropertyValue_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Property Value";
      this._DbColumnName = "Opportunity.PropertyValue";
      this._FieldFormat = FieldFormat.DECIMAL_4;
    }

    private void menuItemPurchaseDate_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Purchase Date";
      this._DbColumnName = "Opportunity.PurchaseDate";
      this._FieldFormat = FieldFormat.DATE;
    }

    private void menuItemCreditRating_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Credit Rating";
      this._DbColumnName = "Opportunity.CreditRating";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBankruptcy_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Bankruptcy";
      this._DbColumnName = "Opportunity.Bankruptcy";
      this._FieldFormat = FieldFormat.X;
      this.isFieldBool = true;
    }

    private void menuItemEmployment_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Employment";
      this._DbColumnName = "Opportunity.Employment";
      this._FieldFormat = FieldFormat.STRING;
      this.setControlsForDropDownListField(EmploymentStatusEnumUtil.GetDisplayNames());
    }

    private void menuItemLoanPurposeOther_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Loan Purpose - Other";
      this._DbColumnName = "Opportunity.PurposeOther";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemPrimaryContact_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Primary Contact";
      this._DbColumnName = "Contact.PrimaryContact";
      this._FieldFormat = FieldFormat.X;
      this.isFieldBool = true;
    }
  }
}
