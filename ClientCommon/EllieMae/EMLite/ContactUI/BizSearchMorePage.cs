// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizSearchMorePage
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizSearchMorePage : MoreSearchForm
  {
    private System.ComponentModel.Container components;
    private ContextMenu fieldMenu;
    private MenuItem menuItemDetails;
    private MenuItem menuItem19;
    private MenuItem menuItemNoEmail;
    private MenuItem menuItemBizAddress;
    private MenuItem menuItemBizAddress1;
    private MenuItem menuItemBizAddress2;
    private MenuItem menuItemBizAddressCity;
    private MenuItem menuItemBizAddressState;
    private MenuItem menuItemBizAddressZip;
    private MenuItem menuItemWebUrl;
    private MenuItem menuItemHomePhone;
    private MenuItem menuItemWorkPhone;
    private MenuItem menuItemMobilePhone;
    private MenuItem menuItemFaxNumber;
    private MenuItem menuItemWorkEmail;
    private MenuItem menuItemPersonalEmail;
    private MenuItem menuItemFees;
    private MenuItem menuItem9;
    private MenuItem menuItemCustomFields;
    private ContactSearchContext searchContext;

    public BizSearchMorePage(ContactSearchContext searchContext)
    {
      this.InitializeComponent();
      this.searchContext = searchContext;
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
      this.menuItemHomePhone = new MenuItem();
      this.menuItemWorkPhone = new MenuItem();
      this.menuItemMobilePhone = new MenuItem();
      this.menuItemFaxNumber = new MenuItem();
      this.menuItemWorkEmail = new MenuItem();
      this.menuItemPersonalEmail = new MenuItem();
      this.menuItem9 = new MenuItem();
      this.menuItemBizAddress = new MenuItem();
      this.menuItemBizAddress1 = new MenuItem();
      this.menuItemBizAddress2 = new MenuItem();
      this.menuItemBizAddressCity = new MenuItem();
      this.menuItemBizAddressState = new MenuItem();
      this.menuItemBizAddressZip = new MenuItem();
      this.menuItem19 = new MenuItem();
      this.menuItemWebUrl = new MenuItem();
      this.menuItemFees = new MenuItem();
      this.menuItemNoEmail = new MenuItem();
      this.menuItemCustomFields = new MenuItem();
      this.txtBoxContactFieldName.Name = "txtBoxContactFieldName";
      this.fieldMenu.MenuItems.AddRange(new MenuItem[2]
      {
        this.menuItemDetails,
        this.menuItemCustomFields
      });
      this.menuItemDetails.Index = 0;
      this.menuItemDetails.MenuItems.AddRange(new MenuItem[12]
      {
        this.menuItemHomePhone,
        this.menuItemWorkPhone,
        this.menuItemMobilePhone,
        this.menuItemFaxNumber,
        this.menuItemWorkEmail,
        this.menuItemPersonalEmail,
        this.menuItem9,
        this.menuItemBizAddress,
        this.menuItem19,
        this.menuItemWebUrl,
        this.menuItemFees,
        this.menuItemNoEmail
      });
      this.menuItemDetails.Text = "Contact Details";
      this.menuItemHomePhone.Index = 0;
      this.menuItemHomePhone.Text = "Home Phone";
      this.menuItemHomePhone.Click += new EventHandler(this.menuItemHomePhone_Click);
      this.menuItemWorkPhone.Index = 1;
      this.menuItemWorkPhone.Text = "Work Phone";
      this.menuItemWorkPhone.Click += new EventHandler(this.menuItemWorkPhone_Click);
      this.menuItemMobilePhone.Index = 2;
      this.menuItemMobilePhone.Text = "Mobile Phone";
      this.menuItemMobilePhone.Click += new EventHandler(this.menuItemCellPhone_Click);
      this.menuItemFaxNumber.Index = 3;
      this.menuItemFaxNumber.Text = "Fax Number";
      this.menuItemFaxNumber.Click += new EventHandler(this.menuItemFaxNumber_Click);
      this.menuItemWorkEmail.Index = 4;
      this.menuItemWorkEmail.Text = "Work Email";
      this.menuItemWorkEmail.Click += new EventHandler(this.menuItemWorkEmail_Click);
      this.menuItemPersonalEmail.Index = 5;
      this.menuItemPersonalEmail.Text = "Personal Email";
      this.menuItemPersonalEmail.Click += new EventHandler(this.menuItemPersonalEmail_Click);
      this.menuItem9.Index = 6;
      this.menuItem9.Text = "-";
      this.menuItemBizAddress.Index = 7;
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
      this.menuItem19.Index = 8;
      this.menuItem19.Text = "-";
      this.menuItemWebUrl.Index = 9;
      this.menuItemWebUrl.Text = "Web URL";
      this.menuItemWebUrl.Click += new EventHandler(this.menuItemWebUrl_Click);
      this.menuItemFees.Index = 10;
      this.menuItemFees.Text = "Fees";
      this.menuItemFees.Click += new EventHandler(this.menuItemFees_Click);
      this.menuItemNoEmail.Index = 11;
      this.menuItemNoEmail.Text = "Do not email";
      this.menuItemNoEmail.Click += new EventHandler(this.menuItemNoEmail_Click);
      this.menuItemCustomFields.Index = 1;
      this.menuItemCustomFields.Text = "Custom Fields";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(456, 334);
      this.Name = nameof (BizSearchMorePage);
    }

    private ContactCustomFieldInfoCollection getCustomFields()
    {
      return Session.ContactManager.GetCustomFieldInfo(ContactType.BizPartner);
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

    private void menuItemHomePhone_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Home Phone";
      this._DbColumnName = "Contact.HomePhone";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemWorkPhone_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Phone";
      this._DbColumnName = "Contact.WorkPhone";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemCellPhone_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Mobile Phone";
      this._DbColumnName = "Contact.MobilePhone";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemFaxNumber_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Fax Number";
      this._DbColumnName = "Contact.FaxNumber";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemPersonalEmail_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Personal Email";
      this._DbColumnName = "Contact.PersonalEmail";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemWorkEmail_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Work Email";
      this._DbColumnName = "Contact.BizEmail";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBizAddress1_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Business Address1";
      this._DbColumnName = "Contact.BizAddress1";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBizAddress2_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Business Address2";
      this._DbColumnName = "Contact.BizAddress2";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBizAddressCity_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Business Address City";
      this._DbColumnName = "Contact.BizCity";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemBizAddressState_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Business Address State";
      this._DbColumnName = "Contact.BizState";
      this._FieldFormat = FieldFormat.STATE;
    }

    private void menuItemBizAddressZip_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Business Address Zip";
      this._DbColumnName = "Contact.BizZip";
      this._FieldFormat = FieldFormat.ZIPCODE;
    }

    private void menuItemNoEmail_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Do not email";
      this._DbColumnName = "Contact.NoSpam";
      this._FieldFormat = FieldFormat.X;
      this.isFieldBool = true;
    }

    private void menuItemWebUrl_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Web Url";
      this._DbColumnName = "Contact.BizWebUrl";
      this._FieldFormat = FieldFormat.STRING;
    }

    private void menuItemFees_Click(object sender, EventArgs e)
    {
      this.txtBoxContactFieldName.Text = "Fees";
      this._DbColumnName = "Contact.Fees";
      this._FieldFormat = FieldFormat.INTEGER;
    }
  }
}
