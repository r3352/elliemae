// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorSearchContactInfoPage
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BorSearchContactInfoPage : ContactSearchPage
  {
    private Label label13;
    private Label label12;
    private Label label11;
    private Label lblContactOwner;
    private DateTimePicker dateTimePickerBirth2;
    private Label lblAndAnni;
    private Label lblAndBirth;
    private ComboBox cmbBoxAnni;
    private ComboBox cmbBoxBirth;
    private DateTimePicker dateTimePickerBirth1;
    private Label label9;
    private Label label10;
    private DateTimePicker dateTimePickerAnni2;
    private DateTimePicker dateTimePickerAnni1;
    private System.ComponentModel.Container components;
    private string _ContactLabel = string.Empty;
    private ComboBox cmbBoxStatus;
    private TextBox txtBoxLastName;
    private ComboBox cmbBoxContactType;
    private Label label1;
    private ComboBox cmbBoxAccessLevel;
    private Label lblAccessLevel;
    private TextBox txtBoxContactOwner;
    private Button btnContactOwner;
    private TextBox txtBoxFirstName;
    private ComboBox cmbBoxBirthIsOptions;
    private ComboBox cmbBoxAnniIsOptions;
    private ComboBox cmbBoxStatusIs;
    private ContactSearchContext searchContext;
    private Sessions.Session session;

    public BorSearchContactInfoPage(Sessions.Session session, ContactSearchContext searchContext)
    {
      this.session = session;
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
      this.cmbBoxStatus = new ComboBox();
      this.label13 = new Label();
      this.txtBoxLastName = new TextBox();
      this.label12 = new Label();
      this.txtBoxFirstName = new TextBox();
      this.label11 = new Label();
      this.lblContactOwner = new Label();
      this.dateTimePickerBirth2 = new DateTimePicker();
      this.lblAndAnni = new Label();
      this.lblAndBirth = new Label();
      this.cmbBoxAnni = new ComboBox();
      this.cmbBoxBirth = new ComboBox();
      this.dateTimePickerBirth1 = new DateTimePicker();
      this.label9 = new Label();
      this.label10 = new Label();
      this.dateTimePickerAnni2 = new DateTimePicker();
      this.dateTimePickerAnni1 = new DateTimePicker();
      this.cmbBoxContactType = new ComboBox();
      this.label1 = new Label();
      this.cmbBoxAccessLevel = new ComboBox();
      this.lblAccessLevel = new Label();
      this.txtBoxContactOwner = new TextBox();
      this.btnContactOwner = new Button();
      this.cmbBoxBirthIsOptions = new ComboBox();
      this.cmbBoxAnniIsOptions = new ComboBox();
      this.cmbBoxStatusIs = new ComboBox();
      this.SuspendLayout();
      this.cmbBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxStatus.Location = new Point(220, 140);
      this.cmbBoxStatus.Name = "cmbBoxStatus";
      this.cmbBoxStatus.Size = new Size(204, 21);
      this.cmbBoxStatus.TabIndex = 10;
      this.label13.Location = new Point(32, 144);
      this.label13.Name = "label13";
      this.label13.Size = new Size(80, 16);
      this.label13.TabIndex = 9;
      this.label13.Text = "Status";
      this.txtBoxLastName.Location = new Point(220, 108);
      this.txtBoxLastName.MaxLength = 50;
      this.txtBoxLastName.Name = "txtBoxLastName";
      this.txtBoxLastName.Size = new Size(204, 20);
      this.txtBoxLastName.TabIndex = 8;
      this.label12.Location = new Point(32, 112);
      this.label12.Name = "label12";
      this.label12.Size = new Size(80, 16);
      this.label12.TabIndex = 7;
      this.label12.Text = "Last Name";
      this.txtBoxFirstName.Location = new Point(220, 76);
      this.txtBoxFirstName.MaxLength = 50;
      this.txtBoxFirstName.Name = "txtBoxFirstName";
      this.txtBoxFirstName.Size = new Size(204, 20);
      this.txtBoxFirstName.TabIndex = 6;
      this.label11.Location = new Point(32, 80);
      this.label11.Name = "label11";
      this.label11.Size = new Size(80, 16);
      this.label11.TabIndex = 5;
      this.label11.Text = "First Name";
      this.lblContactOwner.Location = new Point(32, 48);
      this.lblContactOwner.Name = "lblContactOwner";
      this.lblContactOwner.Size = new Size(80, 16);
      this.lblContactOwner.TabIndex = 2;
      this.lblContactOwner.Text = "Contact Owner";
      this.dateTimePickerBirth2.CustomFormat = "' '";
      this.dateTimePickerBirth2.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerBirth2.Location = new Point(340, 208);
      this.dateTimePickerBirth2.Name = "dateTimePickerBirth2";
      this.dateTimePickerBirth2.Size = new Size(84, 20);
      this.dateTimePickerBirth2.TabIndex = 17;
      this.dateTimePickerBirth2.CloseUp += new EventHandler(this.dateTimePickerBirth2_CloseUp);
      this.lblAndAnni.Location = new Point(312, 240);
      this.lblAndAnni.Name = "lblAndAnni";
      this.lblAndAnni.Size = new Size(24, 16);
      this.lblAndAnni.TabIndex = 21;
      this.lblAndAnni.Text = "and";
      this.lblAndBirth.Location = new Point(312, 212);
      this.lblAndBirth.Name = "lblAndBirth";
      this.lblAndBirth.Size = new Size(24, 16);
      this.lblAndBirth.TabIndex = 16;
      this.lblAndBirth.Text = "and";
      this.cmbBoxAnni.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxAnni.Location = new Point(108, 236);
      this.cmbBoxAnni.Name = "cmbBoxAnni";
      this.cmbBoxAnni.Size = new Size(84, 21);
      this.cmbBoxAnni.TabIndex = 19;
      this.cmbBoxAnni.SelectedIndexChanged += new EventHandler(this.cmbBoxAnni_SelectedIndexChanged);
      this.cmbBoxBirth.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxBirth.Location = new Point(108, 208);
      this.cmbBoxBirth.Name = "cmbBoxBirth";
      this.cmbBoxBirth.Size = new Size(84, 21);
      this.cmbBoxBirth.TabIndex = 14;
      this.cmbBoxBirth.SelectedIndexChanged += new EventHandler(this.cmbBoxBirth_SelectedIndexChanged);
      this.dateTimePickerBirth1.CustomFormat = "' '";
      this.dateTimePickerBirth1.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerBirth1.Location = new Point(220, 208);
      this.dateTimePickerBirth1.Name = "dateTimePickerBirth1";
      this.dateTimePickerBirth1.Size = new Size(84, 20);
      this.dateTimePickerBirth1.TabIndex = 15;
      this.dateTimePickerBirth1.CloseUp += new EventHandler(this.dateTimePickerBirth1_CloseUp);
      this.label9.Location = new Point(32, 212);
      this.label9.Name = "label9";
      this.label9.Size = new Size(80, 16);
      this.label9.TabIndex = 13;
      this.label9.Text = "Birthday";
      this.label10.Location = new Point(32, 240);
      this.label10.Name = "label10";
      this.label10.Size = new Size(80, 16);
      this.label10.TabIndex = 18;
      this.label10.Text = "Anniversary";
      this.dateTimePickerAnni2.CustomFormat = "' '";
      this.dateTimePickerAnni2.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerAnni2.Location = new Point(340, 236);
      this.dateTimePickerAnni2.Name = "dateTimePickerAnni2";
      this.dateTimePickerAnni2.Size = new Size(84, 20);
      this.dateTimePickerAnni2.TabIndex = 22;
      this.dateTimePickerAnni2.CloseUp += new EventHandler(this.dateTimePickerAnni2_CloseUp);
      this.dateTimePickerAnni1.CustomFormat = "' '";
      this.dateTimePickerAnni1.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerAnni1.Location = new Point(220, 236);
      this.dateTimePickerAnni1.Name = "dateTimePickerAnni1";
      this.dateTimePickerAnni1.Size = new Size(84, 20);
      this.dateTimePickerAnni1.TabIndex = 20;
      this.dateTimePickerAnni1.CloseUp += new EventHandler(this.dateTimePickerAnni1_CloseUp);
      this.cmbBoxContactType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxContactType.Location = new Point(220, 172);
      this.cmbBoxContactType.Name = "cmbBoxContactType";
      this.cmbBoxContactType.Size = new Size(204, 21);
      this.cmbBoxContactType.TabIndex = 12;
      this.label1.Location = new Point(32, 176);
      this.label1.Name = "label1";
      this.label1.Size = new Size(80, 16);
      this.label1.TabIndex = 11;
      this.label1.Text = "Contact Type";
      this.cmbBoxAccessLevel.Location = new Point(220, 12);
      this.cmbBoxAccessLevel.Name = "cmbBoxAccessLevel";
      this.cmbBoxAccessLevel.Size = new Size(204, 21);
      this.cmbBoxAccessLevel.TabIndex = 1;
      this.lblAccessLevel.Location = new Point(32, 16);
      this.lblAccessLevel.Name = "lblAccessLevel";
      this.lblAccessLevel.Size = new Size(92, 16);
      this.lblAccessLevel.TabIndex = 0;
      this.lblAccessLevel.Text = "Access Level";
      this.txtBoxContactOwner.Location = new Point(220, 44);
      this.txtBoxContactOwner.Name = "txtBoxContactOwner";
      this.txtBoxContactOwner.ReadOnly = true;
      this.txtBoxContactOwner.Size = new Size(164, 20);
      this.txtBoxContactOwner.TabIndex = 3;
      this.btnContactOwner.Location = new Point(388, 44);
      this.btnContactOwner.Name = "btnContactOwner";
      this.btnContactOwner.Size = new Size(36, 23);
      this.btnContactOwner.TabIndex = 4;
      this.btnContactOwner.Text = "...";
      this.btnContactOwner.Click += new EventHandler(this.btnContactOwner_Click);
      this.cmbBoxBirthIsOptions.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxBirthIsOptions.Location = new Point(220, 200);
      this.cmbBoxBirthIsOptions.Name = "cmbBoxBirthIsOptions";
      this.cmbBoxBirthIsOptions.Size = new Size(112, 21);
      this.cmbBoxBirthIsOptions.TabIndex = 23;
      this.cmbBoxBirthIsOptions.SelectedIndexChanged += new EventHandler(this.cmbBoxBirthIsOptions_SelectedIndexChanged);
      this.cmbBoxAnniIsOptions.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxAnniIsOptions.Location = new Point(220, 244);
      this.cmbBoxAnniIsOptions.Name = "cmbBoxAnniIsOptions";
      this.cmbBoxAnniIsOptions.Size = new Size(112, 21);
      this.cmbBoxAnniIsOptions.TabIndex = 24;
      this.cmbBoxAnniIsOptions.SelectedIndexChanged += new EventHandler(this.cmbBoxAnniIsOptions_SelectedIndexChanged);
      this.cmbBoxStatusIs.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxStatusIs.Location = new Point(108, 140);
      this.cmbBoxStatusIs.Name = "cmbBoxStatusIs";
      this.cmbBoxStatusIs.Size = new Size(84, 21);
      this.cmbBoxStatusIs.TabIndex = 25;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(456, 280);
      this.Controls.Add((Control) this.cmbBoxStatusIs);
      this.Controls.Add((Control) this.cmbBoxAnniIsOptions);
      this.Controls.Add((Control) this.cmbBoxBirthIsOptions);
      this.Controls.Add((Control) this.btnContactOwner);
      this.Controls.Add((Control) this.txtBoxContactOwner);
      this.Controls.Add((Control) this.cmbBoxAccessLevel);
      this.Controls.Add((Control) this.lblAccessLevel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cmbBoxContactType);
      this.Controls.Add((Control) this.cmbBoxStatus);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.txtBoxLastName);
      this.Controls.Add((Control) this.txtBoxFirstName);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.lblContactOwner);
      this.Controls.Add((Control) this.dateTimePickerBirth2);
      this.Controls.Add((Control) this.lblAndAnni);
      this.Controls.Add((Control) this.lblAndBirth);
      this.Controls.Add((Control) this.cmbBoxAnni);
      this.Controls.Add((Control) this.cmbBoxBirth);
      this.Controls.Add((Control) this.dateTimePickerBirth1);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.dateTimePickerAnni2);
      this.Controls.Add((Control) this.dateTimePickerAnni1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BorSearchContactInfoPage);
      this.Text = nameof (BorSearchContactInfoPage);
      this.ResumeLayout(false);
      this.PerformLayout();
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
          case "Contact.Anniversary":
            this.cmbBoxAnni.Text = contactQueryItem.Condition;
            this.dateTimePickerAnni1.CustomFormat = "MM'/'dd";
            this.dateTimePickerAnni1.Value = Utils.ParseDate((object) contactQueryItem.Value1);
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.dateTimePickerAnni2.CustomFormat = "MM'/'dd";
              this.dateTimePickerAnni2.Value = Utils.ParseDate((object) contactQueryItem.Value2);
              break;
            }
            break;
          case "Contact.Birthdate":
            this.cmbBoxBirth.Text = contactQueryItem.Condition;
            this.dateTimePickerBirth1.CustomFormat = "MM'/'dd";
            this.dateTimePickerBirth1.Value = Utils.ParseDate((object) contactQueryItem.Value1);
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.dateTimePickerBirth2.CustomFormat = "MM'/'dd";
              this.dateTimePickerBirth2.Value = Utils.ParseDate((object) contactQueryItem.Value2);
              break;
            }
            break;
          case "Contact.ContactType":
            this.cmbBoxContactType.Text = contactQueryItem.Value1;
            break;
          case "Contact.FirstName":
            this.txtBoxFirstName.Text = contactQueryItem.Value1;
            break;
          case "Contact.LastName":
            this.txtBoxLastName.Text = contactQueryItem.Value1;
            break;
          case "Contact.OwnerID":
            UserInfo user = this.session.OrganizationManager.GetUser(contactQueryItem.Value1);
            if (user == (UserInfo) null)
            {
              this.txtBoxContactOwner.Text = "";
              this.txtBoxContactOwner.Tag = (object) "";
              break;
            }
            this.txtBoxContactOwner.Text = user.FullName + " (" + user.Userid + ")";
            this.txtBoxContactOwner.Tag = (object) user.Userid;
            break;
          case "Contact.Status":
            this.cmbBoxStatus.Text = contactQueryItem.Value1;
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
      this.txtBoxContactOwner.Tag = (object) "";
      this._ContactLabel = "borrowers";
      BorrowerStatus borrowerStatus = this.session.ContactManager.GetBorrowerStatus();
      this.cmbBoxStatusIs.Items.Clear();
      this.cmbBoxStatusIs.Items.AddRange(BoolConditionEnumUtil.GetDisplayNames());
      this.cmbBoxStatusIs.SelectedIndex = 0;
      this.cmbBoxStatus.Items.Clear();
      this.cmbBoxStatus.Items.Add((object) new BorrowerStatusItem("", -1));
      this.cmbBoxStatus.Items.AddRange((object[]) borrowerStatus.Items);
      this.txtBoxFirstName.Text = string.Empty;
      this.txtBoxLastName.Text = string.Empty;
      this.txtBoxContactOwner.Text = "";
      this.txtBoxContactOwner.Tag = (object) "";
      this.cmbBoxContactType.Items.Clear();
      this.cmbBoxContactType.Items.AddRange(BorrowerTypeEnumUtil.GetDisplayNames());
      this.cmbBoxContactType.SelectedIndex = 0;
      this.cmbBoxBirthIsOptions.Items.Clear();
      this.cmbBoxBirthIsOptions.Items.AddRange(MonthWeekEnumUtil.GetDisplayNames());
      this.cmbBoxBirthIsOptions.SelectedIndex = 0;
      this.cmbBoxBirthIsOptions.Location = this.dateTimePickerBirth1.Location;
      this.cmbBoxBirth.Items.Clear();
      this.cmbBoxBirth.Items.AddRange(DateConditionEnumUtil.GetDisplayNames());
      this.cmbBoxBirth.SelectedIndex = 0;
      this.lblAndBirth.Visible = false;
      this.dateTimePickerBirth2.Enabled = false;
      this.dateTimePickerBirth2.Visible = false;
      this.dateTimePickerBirth1.Visible = false;
      this.cmbBoxAnniIsOptions.Items.Clear();
      this.cmbBoxAnniIsOptions.Items.AddRange(MonthWeekEnumUtil.GetDisplayNames());
      this.cmbBoxAnniIsOptions.SelectedIndex = 0;
      this.cmbBoxAnniIsOptions.Location = this.dateTimePickerAnni1.Location;
      this.cmbBoxAnni.Items.Clear();
      this.cmbBoxAnni.Items.AddRange(DateConditionEnumUtil.GetDisplayNames());
      this.cmbBoxAnni.SelectedIndex = 0;
      this.lblAndAnni.Visible = false;
      this.dateTimePickerAnni2.Enabled = false;
      this.dateTimePickerAnni2.Visible = false;
      this.dateTimePickerAnni1.Visible = false;
      this.cmbBoxAccessLevel.Items.Clear();
      this.cmbBoxAccessLevel.Items.AddRange((object[]) new string[3]
      {
        string.Empty,
        "Public",
        "Personal"
      });
      this.cmbBoxAccessLevel.SelectedIndex = 0;
      this.cmbBoxAccessLevel.Enabled = true;
      this.initForCtx();
    }

    public override void Reset()
    {
      this.cmbBoxAccessLevel.SelectedIndex = 0;
      this.cmbBoxAccessLevel.Enabled = true;
      this.txtBoxContactOwner.Text = "";
      this.txtBoxContactOwner.Tag = (object) "";
      this.txtBoxFirstName.Text = string.Empty;
      this.txtBoxLastName.Text = string.Empty;
      this.cmbBoxStatusIs.SelectedIndex = 0;
      this.cmbBoxStatus.SelectedIndex = 0;
      this.cmbBoxContactType.SelectedIndex = 0;
      this.cmbBoxBirth.SelectedIndex = 0;
      this.dateTimePickerBirth1.CustomFormat = "' '";
      this.dateTimePickerBirth1.Value = DateTime.Now;
      this.dateTimePickerBirth2.CustomFormat = "' '";
      this.dateTimePickerBirth2.Value = DateTime.Now;
      this.cmbBoxAnni.SelectedIndex = 0;
      this.dateTimePickerAnni1.CustomFormat = "' '";
      this.dateTimePickerAnni1.Value = DateTime.Now;
      this.dateTimePickerAnni2.CustomFormat = "' '";
      this.dateTimePickerAnni2.Value = DateTime.Now;
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
          switch (defaultCriterion.FieldName)
          {
            case "Contact.Birthdate":
              this.initializeDateField(this.cmbBoxBirth, this.dateTimePickerBirth1, this.dateTimePickerBirth2, (DateValueCriterion) defaultCriterion, "MM'/'dd");
              continue;
            case "Contact.Anniversary":
              this.initializeDateField(this.cmbBoxAnni, this.dateTimePickerAnni1, this.dateTimePickerAnni2, (DateValueCriterion) defaultCriterion, "MM'/'dd");
              continue;
            default:
              ++num;
              continue;
          }
        }
      }
    }

    public override bool ValidateUserInput() => true;

    public override ContactQuery GetSearchCriteria()
    {
      this._StaticQuery.Items = new ContactQueryItem[0];
      this.GetAccessLevelQuery();
      this.GetFirstNameQuery();
      this.GetLastNameQuery();
      this.GetStatusQuery();
      this.GetBirthQuery("Birthday Anniversary");
      this.GetAnniQuery("Birthday Anniversary");
      this.GetOwnerQuery();
      this.GetContactTypeQuery();
      return this._StaticQuery;
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

    private void GetStatusQuery()
    {
      if (this.cmbBoxStatus.Text.Trim() == string.Empty)
        return;
      ContactQueryItem contactQueryItem = new ContactQueryItem();
      contactQueryItem.FieldDisplayName = "Status";
      contactQueryItem.FieldName = "Contact.Status";
      contactQueryItem.GroupName = "ContactInfo";
      BoolCondition boolCondition = BoolConditionEnumUtil.NameToValue(this.cmbBoxStatusIs.SelectedItem.ToString());
      contactQueryItem.Condition = boolCondition == BoolCondition.Is ? "Is" : "Is Not";
      contactQueryItem.Value1 = this.cmbBoxStatus.Text.Trim();
      contactQueryItem.Value2 = string.Empty;
      contactQueryItem.ValueType = "System.String";
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) contactQueryItem);
    }

    private void GetContactTypeQuery()
    {
      if (this.cmbBoxContactType.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Contact Type",
        FieldName = "Contact.ContactType",
        GroupName = "ContactInfo",
        Condition = "Is",
        Value1 = this.cmbBoxContactType.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetBirthQuery(string levelOneName)
    {
      if (this.cmbBoxBirth.Text == "Is")
      {
        if (this.cmbBoxBirthIsOptions.Text == "" || this.cmbBoxBirthIsOptions.Text == "Exact Date" && this.dateTimePickerBirth2.Text.Trim() == string.Empty)
          return;
      }
      else if (this.dateTimePickerBirth1.Text.Trim() == string.Empty)
        return;
      if (this.cmbBoxBirth.Text == "Between" && this.dateTimePickerBirth2.Text.Trim() == string.Empty)
        return;
      ContactQueryItem contactQueryItem = new ContactQueryItem();
      contactQueryItem.FieldDisplayName = "Birthday";
      contactQueryItem.FieldName = "Contact.Birthdate";
      contactQueryItem.GroupName = "ContactInfo";
      contactQueryItem.Condition = this.cmbBoxBirth.Text;
      if (this.cmbBoxBirth.Text == "Is")
      {
        contactQueryItem.Value1 = this.cmbBoxBirthIsOptions.Text.Trim();
        contactQueryItem.Value2 = this.dateTimePickerBirth2.Text.Trim();
      }
      else
      {
        contactQueryItem.Value1 = this.dateTimePickerBirth1.Text.Trim();
        contactQueryItem.Value2 = this.dateTimePickerBirth2.Text.Trim();
      }
      contactQueryItem.ValueType = "System.DateTime";
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) contactQueryItem);
    }

    private void GetAnniQuery(string levelOneName)
    {
      if (this.cmbBoxAnni.Text == "Is")
      {
        if (this.cmbBoxAnniIsOptions.Text == "" || this.cmbBoxAnniIsOptions.Text == "Exact Date" && this.dateTimePickerAnni2.Text.Trim() == string.Empty)
          return;
      }
      else if (this.dateTimePickerAnni1.Text.Trim() == string.Empty)
        return;
      if (this.cmbBoxAnni.Text == "Between" && this.dateTimePickerAnni2.Text.Trim() == string.Empty)
        return;
      ContactQueryItem contactQueryItem = new ContactQueryItem();
      contactQueryItem.FieldDisplayName = "Anniversary";
      contactQueryItem.FieldName = "Contact.Anniversary";
      contactQueryItem.GroupName = "ContactInfo";
      contactQueryItem.Condition = this.cmbBoxAnni.Text;
      if (this.cmbBoxAnni.Text == "Is")
      {
        contactQueryItem.Value1 = this.cmbBoxAnniIsOptions.Text.Trim();
        contactQueryItem.Value2 = this.dateTimePickerAnni2.Text.Trim();
      }
      else
      {
        contactQueryItem.Value1 = this.dateTimePickerAnni1.Text.Trim();
        contactQueryItem.Value2 = this.dateTimePickerAnni2.Text.Trim();
      }
      contactQueryItem.ValueType = "System.DateTime";
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) contactQueryItem);
    }

    private void GetOwnerQuery()
    {
      if (this.txtBoxContactOwner.Text == null || this.txtBoxContactOwner.Text == "")
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Contact Owner",
        FieldName = "Contact.OwnerID",
        GroupName = "ContactInfo",
        Condition = "Is",
        Value1 = (!(this.txtBoxContactOwner.Text == "") ? (string) this.txtBoxContactOwner.Tag : ""),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void dateTimePickerBirth1_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerBirth1.CustomFormat = "MM'/'dd";
    }

    private void dateTimePickerBirth2_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerBirth2.CustomFormat = "MM'/'dd";
    }

    private void dateTimePickerAnni1_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerAnni1.CustomFormat = "MM'/'dd";
    }

    private void dateTimePickerAnni2_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerAnni2.CustomFormat = "MM'/'dd";
    }

    private void btnContactOwner_Click(object sender, EventArgs e)
    {
      using (ContactAssignment contactAssignment = new ContactAssignment(this.session, AclFeature.Cnt_Borrower_CreateNew, ""))
      {
        if (contactAssignment.ShowDialog() != DialogResult.OK)
          return;
        this.txtBoxContactOwner.Text = contactAssignment.SelectedUser.FullName + " (" + contactAssignment.SelectedUser.Userid + ")";
        this.txtBoxContactOwner.Tag = (object) contactAssignment.SelectedUser.Userid;
      }
    }

    private void cmbBoxBirth_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxBirth.Text == "Is")
      {
        this.cmbBoxBirthIsOptions.Visible = true;
        this.cmbBoxBirthIsOptions.SelectedIndex = 0;
        this.lblAndBirth.Visible = false;
        this.dateTimePickerBirth1.Visible = false;
        this.dateTimePickerBirth2.Enabled = false;
        this.dateTimePickerBirth2.Visible = false;
      }
      else if (this.cmbBoxBirth.Text == "Between")
      {
        this.cmbBoxBirthIsOptions.Visible = false;
        this.dateTimePickerBirth1.Visible = true;
        this.lblAndBirth.Visible = true;
        this.dateTimePickerBirth2.Enabled = true;
        this.dateTimePickerBirth2.Visible = true;
      }
      else
      {
        this.cmbBoxBirthIsOptions.Visible = false;
        this.dateTimePickerBirth1.Visible = true;
        this.lblAndBirth.Visible = false;
        this.dateTimePickerBirth2.Enabled = false;
        this.dateTimePickerBirth2.Visible = false;
      }
    }

    private void cmbBoxAnni_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxAnni.Text == "Is")
      {
        this.cmbBoxAnniIsOptions.Visible = true;
        this.cmbBoxAnniIsOptions.SelectedIndex = 0;
        this.lblAndAnni.Visible = false;
        this.dateTimePickerAnni1.Visible = false;
        this.dateTimePickerAnni2.Enabled = false;
        this.dateTimePickerAnni2.Visible = false;
      }
      else if (this.cmbBoxAnni.Text == "Between")
      {
        this.cmbBoxAnniIsOptions.Visible = false;
        this.dateTimePickerAnni1.Visible = true;
        this.lblAndAnni.Visible = true;
        this.dateTimePickerAnni2.Enabled = true;
        this.dateTimePickerAnni2.Visible = true;
      }
      else
      {
        this.cmbBoxAnniIsOptions.Visible = false;
        this.dateTimePickerAnni1.Visible = true;
        this.lblAndAnni.Visible = false;
        this.dateTimePickerAnni2.Enabled = false;
        this.dateTimePickerAnni2.Visible = false;
      }
    }

    private void cmbBoxBirthIsOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxBirthIsOptions.Text == "Exact Date")
      {
        this.dateTimePickerBirth2.Enabled = true;
        this.dateTimePickerBirth2.Visible = true;
      }
      else
      {
        this.dateTimePickerBirth2.Enabled = false;
        this.dateTimePickerBirth2.Visible = false;
      }
    }

    private void cmbBoxAnniIsOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxAnniIsOptions.Text == "Exact Date")
      {
        this.dateTimePickerAnni2.Enabled = true;
        this.dateTimePickerAnni2.Visible = true;
      }
      else
      {
        this.dateTimePickerAnni2.Enabled = false;
        this.dateTimePickerAnni2.Visible = false;
      }
    }
  }
}
