// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizSearchContactInfoPage
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizSearchContactInfoPage : ContactSearchPage
  {
    private Label label12;
    private Label label11;
    private Label lblContactOwner;
    private System.ComponentModel.Container components;
    private string _ContactLabel = string.Empty;
    private TextBox txtBoxLastName;
    private Label label1;
    private Label label3;
    private Label label4;
    private TextBox txtBoxCompany;
    private ComboBox cmbBoxCategory;
    private ComboBox cmbBoxAccessLevel;
    private TextBox txtBoxTitle;
    private TextBox txtBoxLicenseNumber;
    private Label lblAccessLevel;
    private TextBox txtBoxFirstName;
    private ContactSearchContext searchContext;

    public BizSearchContactInfoPage(ContactSearchContext searchContext)
    {
      this.InitializeComponent();
      this.searchContext = searchContext;
      this.Init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtBoxLastName = new TextBox();
      this.label12 = new Label();
      this.txtBoxFirstName = new TextBox();
      this.label11 = new Label();
      this.lblContactOwner = new Label();
      this.txtBoxCompany = new TextBox();
      this.label1 = new Label();
      this.cmbBoxCategory = new ComboBox();
      this.lblAccessLevel = new Label();
      this.cmbBoxAccessLevel = new ComboBox();
      this.txtBoxTitle = new TextBox();
      this.label3 = new Label();
      this.txtBoxLicenseNumber = new TextBox();
      this.label4 = new Label();
      this.SuspendLayout();
      this.txtBoxLastName.Location = new Point(188, 120);
      this.txtBoxLastName.MaxLength = 50;
      this.txtBoxLastName.Name = "txtBoxLastName";
      this.txtBoxLastName.Size = new Size(204, 20);
      this.txtBoxLastName.TabIndex = 7;
      this.txtBoxLastName.Text = "";
      this.label12.Location = new Point(60, 124);
      this.label12.Name = "label12";
      this.label12.Size = new Size(92, 16);
      this.label12.TabIndex = 6;
      this.label12.Text = "Last Name";
      this.txtBoxFirstName.Location = new Point(188, 92);
      this.txtBoxFirstName.MaxLength = 50;
      this.txtBoxFirstName.Name = "txtBoxFirstName";
      this.txtBoxFirstName.Size = new Size(204, 20);
      this.txtBoxFirstName.TabIndex = 5;
      this.txtBoxFirstName.Text = "";
      this.label11.Location = new Point(60, 96);
      this.label11.Name = "label11";
      this.label11.Size = new Size(92, 16);
      this.label11.TabIndex = 4;
      this.label11.Text = "First Name";
      this.lblContactOwner.Location = new Point(60, 64);
      this.lblContactOwner.Name = "lblContactOwner";
      this.lblContactOwner.Size = new Size(92, 16);
      this.lblContactOwner.TabIndex = 2;
      this.lblContactOwner.Text = "Category";
      this.txtBoxCompany.Location = new Point(188, 148);
      this.txtBoxCompany.MaxLength = 64;
      this.txtBoxCompany.Name = "txtBoxCompany";
      this.txtBoxCompany.Size = new Size(204, 20);
      this.txtBoxCompany.TabIndex = 9;
      this.txtBoxCompany.Text = "";
      this.label1.Location = new Point(60, 152);
      this.label1.Name = "label1";
      this.label1.Size = new Size(92, 16);
      this.label1.TabIndex = 8;
      this.label1.Text = "Company";
      this.cmbBoxCategory.Location = new Point(188, 60);
      this.cmbBoxCategory.Name = "cmbBoxCategory";
      this.cmbBoxCategory.Size = new Size(204, 21);
      this.cmbBoxCategory.Sorted = true;
      this.cmbBoxCategory.TabIndex = 3;
      this.lblAccessLevel.Location = new Point(60, 32);
      this.lblAccessLevel.Name = "lblAccessLevel";
      this.lblAccessLevel.Size = new Size(92, 16);
      this.lblAccessLevel.TabIndex = 0;
      this.lblAccessLevel.Text = "Access Level";
      this.cmbBoxAccessLevel.Location = new Point(188, 28);
      this.cmbBoxAccessLevel.Name = "cmbBoxAccessLevel";
      this.cmbBoxAccessLevel.Size = new Size(204, 21);
      this.cmbBoxAccessLevel.TabIndex = 1;
      this.txtBoxTitle.Location = new Point(188, 176);
      this.txtBoxTitle.MaxLength = 50;
      this.txtBoxTitle.Name = "txtBoxTitle";
      this.txtBoxTitle.Size = new Size(204, 20);
      this.txtBoxTitle.TabIndex = 11;
      this.txtBoxTitle.Text = "";
      this.label3.Location = new Point(60, 180);
      this.label3.Name = "label3";
      this.label3.Size = new Size(92, 16);
      this.label3.TabIndex = 10;
      this.label3.Text = "Title";
      this.txtBoxLicenseNumber.Location = new Point(188, 204);
      this.txtBoxLicenseNumber.MaxLength = 50;
      this.txtBoxLicenseNumber.Name = "txtBoxLicenseNumber";
      this.txtBoxLicenseNumber.Size = new Size(204, 20);
      this.txtBoxLicenseNumber.TabIndex = 13;
      this.txtBoxLicenseNumber.Text = "";
      this.label4.Location = new Point(60, 208);
      this.label4.Name = "label4";
      this.label4.Size = new Size(92, 16);
      this.label4.TabIndex = 12;
      this.label4.Text = "License Number";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(456, 280);
      this.Controls.Add((Control) this.txtBoxLicenseNumber);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtBoxTitle);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.cmbBoxAccessLevel);
      this.Controls.Add((Control) this.lblAccessLevel);
      this.Controls.Add((Control) this.cmbBoxCategory);
      this.Controls.Add((Control) this.txtBoxCompany);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtBoxLastName);
      this.Controls.Add((Control) this.txtBoxFirstName);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.lblContactOwner);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BizSearchContactInfoPage);
      this.Text = "BorSearchContactInfoPage";
      this.ResumeLayout(false);
    }

    protected override void loadQuery()
    {
      this.Reset();
      foreach (ContactQueryItem contactQueryItem in this._StaticQuery.Items)
      {
        switch (contactQueryItem.FieldName)
        {
          case "Contact.AccessLevel":
            if (contactQueryItem.Value1 == "Private")
            {
              this.cmbBoxAccessLevel.Text = "Personal";
              break;
            }
            this.cmbBoxAccessLevel.Text = contactQueryItem.Value1;
            break;
          case "Contact.CategoryID":
            Hashtable categoryIdToNameTable = new BizCategoryUtil(Session.SessionObjects).GetCategoryIdToNameTable();
            if (!categoryIdToNameTable.Contains((object) int.Parse(contactQueryItem.Value1)))
              return;
            this.cmbBoxCategory.Text = (string) categoryIdToNameTable[(object) int.Parse(contactQueryItem.Value1)];
            break;
          case "Contact.CompanyName":
            this.txtBoxCompany.Text = contactQueryItem.Value1;
            break;
          case "Contact.FirstName":
            this.txtBoxFirstName.Text = contactQueryItem.Value1;
            break;
          case "Contact.JobTitle":
            this.txtBoxTitle.Text = contactQueryItem.Value1;
            break;
          case "Contact.LastName":
            this.txtBoxLastName.Text = contactQueryItem.Value1;
            break;
          case "Contact.LicenseNumber":
            this.txtBoxLicenseNumber.Text = contactQueryItem.Value1;
            break;
        }
      }
    }

    private void initForCampaignCtx()
    {
      this.txtBoxFirstName.Enabled = false;
      this.txtBoxLastName.Enabled = false;
    }

    private void initForCtx()
    {
      switch (this.searchContext)
      {
        case ContactSearchContext.CampaignAdd:
        case ContactSearchContext.CampaignDelete:
          this.initForCampaignCtx();
          break;
      }
    }

    private void Init()
    {
      this._ContactLabel = "business contacts";
      this.txtBoxFirstName.Text = string.Empty;
      this.txtBoxLastName.Text = string.Empty;
      this.txtBoxLicenseNumber.Text = string.Empty;
      this.txtBoxTitle.Text = string.Empty;
      this.txtBoxCompany.Text = string.Empty;
      BizCategory[] bizCategories = Session.ContactManager.GetBizCategories();
      this.cmbBoxCategory.Items.Clear();
      this.cmbBoxCategory.Items.Add((object) string.Empty);
      this.cmbBoxCategory.Items.AddRange((object[]) bizCategories);
      this.cmbBoxAccessLevel.Items.Clear();
      this.cmbBoxAccessLevel.Items.AddRange((object[]) new string[3]
      {
        string.Empty,
        "Public",
        "Personal"
      });
      this.initForCtx();
    }

    public override void Reset()
    {
      this.cmbBoxAccessLevel.SelectedIndex = 0;
      this.cmbBoxCategory.SelectedIndex = 0;
      this.txtBoxFirstName.Text = string.Empty;
      this.txtBoxLastName.Text = string.Empty;
      this.txtBoxCompany.Text = string.Empty;
      this.txtBoxLicenseNumber.Text = string.Empty;
      this.txtBoxTitle.Text = string.Empty;
    }

    public override void LoadQuery(
      QueryCriterion[] defaultCriteria,
      RelatedLoanMatchType defaultMatchType)
    {
      int num = 0;
      for (int index = 0; index < defaultCriteria.Length; ++index)
      {
        if (defaultCriteria[index] is FieldValueCriterion defaultCriterion)
        {
          string fieldName = defaultCriterion.FieldName;
          ++num;
        }
      }
    }

    public override bool ValidateUserInput() => true;

    public override ContactQuery GetSearchCriteria()
    {
      this._StaticQuery.Items = new ContactQueryItem[0];
      this.GetCategoryQuery();
      this.GetAccessLevelQuery();
      this.GetFirstNameQuery();
      this.GetLastNameQuery();
      this.GetCompanyQuery();
      this.GetJobTitleQuery();
      this.GetLicenseNumberQuery();
      return this._StaticQuery;
    }

    private void GetCategoryQuery()
    {
      if (this.cmbBoxCategory.Text.Trim() == string.Empty)
        return;
      Hashtable categoryNameToIdTable = new BizCategoryUtil(Session.SessionObjects).GetCategoryNameToIdTable();
      if (!categoryNameToIdTable.Contains((object) this.cmbBoxCategory.Text.Trim()))
        return;
      int num = (int) categoryNameToIdTable[(object) this.cmbBoxCategory.Text.Trim()];
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Category",
        FieldName = "Contact.CategoryID",
        GroupName = "ContactInfo",
        Condition = "Is",
        Value1 = num.ToString(),
        Value2 = string.Empty,
        ValueType = "System.Int32"
      });
    }

    private void GetAccessLevelQuery()
    {
      if (this.cmbBoxAccessLevel.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Access Level",
        FieldName = "Contact.AccessLevel",
        GroupName = "ContactInfo",
        Condition = "Is",
        Value1 = (!(this.cmbBoxAccessLevel.Text.Trim() == "Personal") ? this.cmbBoxAccessLevel.Text.Trim() : "Private"),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetFirstNameQuery()
    {
      if (this.txtBoxFirstName.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "First Name",
        FieldName = "Contact.FirstName",
        GroupName = "ContactInfo",
        Condition = "Starts with",
        Value1 = this.txtBoxFirstName.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetLastNameQuery()
    {
      if (this.txtBoxLastName.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Last Name",
        FieldName = "Contact.LastName",
        GroupName = "ContactInfo",
        Condition = "Starts with",
        Value1 = this.txtBoxLastName.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetCompanyQuery()
    {
      if (this.txtBoxCompany.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Company",
        FieldName = "Contact.CompanyName",
        GroupName = "ContactInfo",
        Condition = "Starts with",
        Value1 = this.txtBoxCompany.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetJobTitleQuery()
    {
      if (this.txtBoxTitle.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Job Title",
        FieldName = "Contact.JobTitle",
        GroupName = "ContactInfo",
        Condition = "Starts with",
        Value1 = this.txtBoxTitle.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetLicenseNumberQuery()
    {
      if (this.txtBoxLicenseNumber.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "License Number",
        FieldName = "Contact.LicenseNumber",
        GroupName = "ContactInfo",
        Condition = "Starts with",
        Value1 = this.txtBoxLicenseNumber.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }
  }
}
