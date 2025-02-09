// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactSearchMarketingPage
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
  public class ContactSearchMarketingPage : ContactSearchPage
  {
    private GroupBox groupBox1;
    private System.ComponentModel.Container components;
    private ComboBox cmbBoxActivity;
    private Label label6;
    private DateTimePicker dateTimePickerCommDate2;
    private DateTimePicker dateTimePickerCommDate1;
    private ComboBox cmbBoxCommDate;
    private TextBox txtBoxMergeDocument;
    private ComboBox cmbBoxMergeDocument;
    private ComboBox cmbBoxCommSubject;
    private TextBox txtBoxCommSubject;
    private ComboBox cmbBoxCommDetails;
    private TextBox txtBoxCommDetails;
    private Label lblAndCommDate;
    private ComboBox cmbBoxContactSourceCondition;
    private ComboBox cmbBoxContactSource;
    private ComboBox cmbBoxCommUserIDCondition;
    private string _ContactLabel = string.Empty;
    private TextBox txtBoxCommUserID;
    private Button btnCommUserID;
    private Label lblContactSource;
    private Label lblCommDetails;
    private Label lblCommSubject;
    private Label lblCommUserID;
    private Label lblMergeDocName;
    private Label lblCommDate;
    private ContactType _ContactType;
    private ContactSearchContext searchContext;
    private Sessions.Session session;

    public ContactSearchMarketingPage(
      Sessions.Session session,
      ContactType contactType,
      ContactSearchContext searchContext)
    {
      this.session = session;
      this.InitializeComponent();
      this._ContactType = contactType;
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
      this.groupBox1 = new GroupBox();
      this.btnCommUserID = new Button();
      this.txtBoxCommUserID = new TextBox();
      this.cmbBoxContactSource = new ComboBox();
      this.cmbBoxContactSourceCondition = new ComboBox();
      this.lblContactSource = new Label();
      this.cmbBoxCommDetails = new ComboBox();
      this.txtBoxCommDetails = new TextBox();
      this.lblCommDetails = new Label();
      this.cmbBoxCommSubject = new ComboBox();
      this.txtBoxCommSubject = new TextBox();
      this.lblCommSubject = new Label();
      this.cmbBoxCommUserIDCondition = new ComboBox();
      this.lblCommUserID = new Label();
      this.cmbBoxMergeDocument = new ComboBox();
      this.txtBoxMergeDocument = new TextBox();
      this.lblMergeDocName = new Label();
      this.dateTimePickerCommDate2 = new DateTimePicker();
      this.dateTimePickerCommDate1 = new DateTimePicker();
      this.lblAndCommDate = new Label();
      this.cmbBoxCommDate = new ComboBox();
      this.lblCommDate = new Label();
      this.cmbBoxActivity = new ComboBox();
      this.label6 = new Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.btnCommUserID);
      this.groupBox1.Controls.Add((Control) this.txtBoxCommUserID);
      this.groupBox1.Controls.Add((Control) this.cmbBoxContactSource);
      this.groupBox1.Controls.Add((Control) this.cmbBoxContactSourceCondition);
      this.groupBox1.Controls.Add((Control) this.lblContactSource);
      this.groupBox1.Controls.Add((Control) this.cmbBoxCommDetails);
      this.groupBox1.Controls.Add((Control) this.txtBoxCommDetails);
      this.groupBox1.Controls.Add((Control) this.lblCommDetails);
      this.groupBox1.Controls.Add((Control) this.cmbBoxCommSubject);
      this.groupBox1.Controls.Add((Control) this.txtBoxCommSubject);
      this.groupBox1.Controls.Add((Control) this.lblCommSubject);
      this.groupBox1.Controls.Add((Control) this.cmbBoxCommUserIDCondition);
      this.groupBox1.Controls.Add((Control) this.lblCommUserID);
      this.groupBox1.Controls.Add((Control) this.cmbBoxMergeDocument);
      this.groupBox1.Controls.Add((Control) this.txtBoxMergeDocument);
      this.groupBox1.Controls.Add((Control) this.lblMergeDocName);
      this.groupBox1.Controls.Add((Control) this.dateTimePickerCommDate2);
      this.groupBox1.Controls.Add((Control) this.dateTimePickerCommDate1);
      this.groupBox1.Controls.Add((Control) this.lblAndCommDate);
      this.groupBox1.Controls.Add((Control) this.cmbBoxCommDate);
      this.groupBox1.Controls.Add((Control) this.lblCommDate);
      this.groupBox1.Location = new Point(8, 40);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(440, 224);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Activity Info";
      this.btnCommUserID.Location = new Point(388, 88);
      this.btnCommUserID.Name = "btnCommUserID";
      this.btnCommUserID.Size = new Size(40, 23);
      this.btnCommUserID.TabIndex = 11;
      this.btnCommUserID.Text = "...";
      this.btnCommUserID.Click += new EventHandler(this.btnCommUserID_Click);
      this.txtBoxCommUserID.Location = new Point(224, 88);
      this.txtBoxCommUserID.Name = "txtBoxCommUserID";
      this.txtBoxCommUserID.ReadOnly = true;
      this.txtBoxCommUserID.Size = new Size(160, 20);
      this.txtBoxCommUserID.TabIndex = 10;
      this.txtBoxCommUserID.Text = "";
      this.cmbBoxContactSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxContactSource.Location = new Point(224, 188);
      this.cmbBoxContactSource.Name = "cmbBoxContactSource";
      this.cmbBoxContactSource.Size = new Size(204, 21);
      this.cmbBoxContactSource.TabIndex = 20;
      this.cmbBoxContactSourceCondition.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxContactSourceCondition.Location = new Point(116, 188);
      this.cmbBoxContactSourceCondition.Name = "cmbBoxContactSourceCondition";
      this.cmbBoxContactSourceCondition.Size = new Size(84, 21);
      this.cmbBoxContactSourceCondition.TabIndex = 19;
      this.lblContactSource.Location = new Point(12, 188);
      this.lblContactSource.Name = "lblContactSource";
      this.lblContactSource.Size = new Size(92, 28);
      this.lblContactSource.TabIndex = 18;
      this.lblContactSource.Text = "Contact Data Source";
      this.cmbBoxCommDetails.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCommDetails.Location = new Point(116, 156);
      this.cmbBoxCommDetails.Name = "cmbBoxCommDetails";
      this.cmbBoxCommDetails.Size = new Size(84, 21);
      this.cmbBoxCommDetails.TabIndex = 16;
      this.txtBoxCommDetails.Location = new Point(224, 156);
      this.txtBoxCommDetails.MaxLength = 200;
      this.txtBoxCommDetails.Name = "txtBoxCommDetails";
      this.txtBoxCommDetails.Size = new Size(204, 20);
      this.txtBoxCommDetails.TabIndex = 17;
      this.txtBoxCommDetails.Text = "";
      this.lblCommDetails.Location = new Point(12, 152);
      this.lblCommDetails.Name = "lblCommDetails";
      this.lblCommDetails.Size = new Size(92, 28);
      this.lblCommDetails.TabIndex = 15;
      this.lblCommDetails.Text = "Communication Details";
      this.cmbBoxCommSubject.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCommSubject.Location = new Point(116, 124);
      this.cmbBoxCommSubject.Name = "cmbBoxCommSubject";
      this.cmbBoxCommSubject.Size = new Size(84, 21);
      this.cmbBoxCommSubject.TabIndex = 13;
      this.txtBoxCommSubject.Location = new Point(224, 124);
      this.txtBoxCommSubject.MaxLength = 50;
      this.txtBoxCommSubject.Name = "txtBoxCommSubject";
      this.txtBoxCommSubject.Size = new Size(204, 20);
      this.txtBoxCommSubject.TabIndex = 14;
      this.txtBoxCommSubject.Text = "";
      this.lblCommSubject.Location = new Point(12, 120);
      this.lblCommSubject.Name = "lblCommSubject";
      this.lblCommSubject.Size = new Size(92, 28);
      this.lblCommSubject.TabIndex = 12;
      this.lblCommSubject.Text = "Communication Subject";
      this.cmbBoxCommUserIDCondition.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCommUserIDCondition.Location = new Point(116, 88);
      this.cmbBoxCommUserIDCondition.Name = "cmbBoxCommUserIDCondition";
      this.cmbBoxCommUserIDCondition.Size = new Size(84, 21);
      this.cmbBoxCommUserIDCondition.TabIndex = 9;
      this.cmbBoxCommUserIDCondition.SelectedIndexChanged += new EventHandler(this.cmbBoxCommUserIDCondition_SelectedIndexChanged);
      this.lblCommUserID.Location = new Point(12, 88);
      this.lblCommUserID.Name = "lblCommUserID";
      this.lblCommUserID.Size = new Size(92, 28);
      this.lblCommUserID.TabIndex = 8;
      this.lblCommUserID.Text = "Communicator User ID";
      this.cmbBoxMergeDocument.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxMergeDocument.Location = new Point(116, 24);
      this.cmbBoxMergeDocument.Name = "cmbBoxMergeDocument";
      this.cmbBoxMergeDocument.Size = new Size(84, 21);
      this.cmbBoxMergeDocument.TabIndex = 1;
      this.txtBoxMergeDocument.Location = new Point(224, 24);
      this.txtBoxMergeDocument.MaxLength = (int) byte.MaxValue;
      this.txtBoxMergeDocument.Name = "txtBoxMergeDocument";
      this.txtBoxMergeDocument.Size = new Size(204, 20);
      this.txtBoxMergeDocument.TabIndex = 2;
      this.txtBoxMergeDocument.Text = "";
      this.lblMergeDocName.Location = new Point(12, 24);
      this.lblMergeDocName.Name = "lblMergeDocName";
      this.lblMergeDocName.Size = new Size(92, 28);
      this.lblMergeDocName.TabIndex = 0;
      this.lblMergeDocName.Text = "Merge Document Name";
      this.dateTimePickerCommDate2.CustomFormat = "' '";
      this.dateTimePickerCommDate2.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerCommDate2.Location = new Point(344, 56);
      this.dateTimePickerCommDate2.Name = "dateTimePickerCommDate2";
      this.dateTimePickerCommDate2.Size = new Size(84, 20);
      this.dateTimePickerCommDate2.TabIndex = 7;
      this.dateTimePickerCommDate2.CloseUp += new EventHandler(this.dateTimePickerCommDate2_CloseUp);
      this.dateTimePickerCommDate1.CustomFormat = "' '";
      this.dateTimePickerCommDate1.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerCommDate1.Location = new Point(224, 56);
      this.dateTimePickerCommDate1.Name = "dateTimePickerCommDate1";
      this.dateTimePickerCommDate1.Size = new Size(84, 20);
      this.dateTimePickerCommDate1.TabIndex = 5;
      this.dateTimePickerCommDate1.CloseUp += new EventHandler(this.dateTimePickerCommDate1_CloseUp);
      this.lblAndCommDate.Location = new Point(316, 60);
      this.lblAndCommDate.Name = "lblAndCommDate";
      this.lblAndCommDate.Size = new Size(24, 16);
      this.lblAndCommDate.TabIndex = 6;
      this.lblAndCommDate.Text = "and";
      this.cmbBoxCommDate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCommDate.Location = new Point(116, 56);
      this.cmbBoxCommDate.Name = "cmbBoxCommDate";
      this.cmbBoxCommDate.Size = new Size(84, 21);
      this.cmbBoxCommDate.TabIndex = 4;
      this.cmbBoxCommDate.SelectedIndexChanged += new EventHandler(this.cmbBoxCommDate_SelectedIndexChanged);
      this.lblCommDate.Location = new Point(12, 56);
      this.lblCommDate.Name = "lblCommDate";
      this.lblCommDate.Size = new Size(92, 28);
      this.lblCommDate.TabIndex = 3;
      this.lblCommDate.Text = "Communication Date";
      this.cmbBoxActivity.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxActivity.Location = new Point(124, 12);
      this.cmbBoxActivity.Name = "cmbBoxActivity";
      this.cmbBoxActivity.Size = new Size(204, 21);
      this.cmbBoxActivity.TabIndex = 1;
      this.cmbBoxActivity.SelectedIndexChanged += new EventHandler(this.cmbBoxActivity_SelectedIndexChanged);
      this.label6.Location = new Point(28, 16);
      this.label6.Name = "label6";
      this.label6.Size = new Size(80, 16);
      this.label6.TabIndex = 0;
      this.label6.Text = "Activity";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(456, 280);
      this.Controls.Add((Control) this.cmbBoxActivity);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ContactSearchMarketingPage);
      this.Text = "BorSearchLoanInfoPage";
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void initForCampaignCtx()
    {
      this.cmbBoxCommSubject.Enabled = false;
      this.cmbBoxCommDetails.Enabled = false;
      this.txtBoxCommSubject.Enabled = false;
      this.txtBoxCommDetails.Enabled = false;
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
      this._ContactLabel = this._ContactType != ContactType.Borrower ? "business contacts" : "borrowers";
      this.cmbBoxActivity.Items.Clear();
      this.cmbBoxActivity.Items.AddRange(ContactMarketingActivityEnumUtil.GetDisplayNames());
      this.cmbBoxActivity.SelectedIndex = 0;
      this.cmbBoxCommDate.Items.Clear();
      this.cmbBoxCommDate.Items.AddRange(DateConditionEnumUtil.GetDisplayNames());
      this.cmbBoxCommDate.SelectedIndex = 0;
      this.lblAndCommDate.Visible = false;
      this.dateTimePickerCommDate2.Enabled = false;
      this.dateTimePickerCommDate2.Visible = false;
      this.cmbBoxCommUserIDCondition.Items.Clear();
      this.cmbBoxCommUserIDCondition.Items.AddRange(TextConditionEnumUtil.GetDisplayNames());
      this.cmbBoxCommUserIDCondition.SelectedIndex = 0;
      this.cmbBoxMergeDocument.Items.Clear();
      this.cmbBoxMergeDocument.Items.AddRange(TextConditionEnumUtil.GetDisplayNames());
      this.cmbBoxMergeDocument.SelectedIndex = 0;
      this.cmbBoxCommSubject.Items.Clear();
      this.cmbBoxCommSubject.Items.AddRange(TextConditionEnumUtil.GetDisplayNames());
      this.cmbBoxCommSubject.SelectedIndex = 0;
      this.cmbBoxCommDetails.Items.Clear();
      this.cmbBoxCommDetails.Items.AddRange(TextConditionEnumUtil.GetDisplayNames());
      this.cmbBoxCommDetails.SelectedIndex = 0;
      this.cmbBoxContactSourceCondition.Items.Clear();
      this.cmbBoxContactSourceCondition.Items.AddRange(BoolConditionEnumUtil.GetDisplayNames());
      this.cmbBoxContactSourceCondition.SelectedIndex = 0;
      this.cmbBoxContactSource.Items.Clear();
      this.cmbBoxContactSource.Items.Add((object) "");
      this.cmbBoxContactSource.Items.AddRange(ContactSourceEnumUtil.GetDisplayNames(this._ContactType));
      this.cmbBoxContactSource.SelectedIndex = 0;
      this.initForCtx();
    }

    public override void Reset()
    {
      this.cmbBoxActivity.SelectedIndex = 0;
      this.cmbBoxActivity_SelectedIndexChanged((object) null, (EventArgs) null);
      this.cmbBoxMergeDocument.SelectedIndex = 0;
      this.txtBoxMergeDocument.Text = "";
      this.cmbBoxCommDate.SelectedIndex = 0;
      this.cmbBoxCommDate_SelectedIndexChanged((object) null, (EventArgs) null);
      this.dateTimePickerCommDate1.CustomFormat = "' '";
      this.dateTimePickerCommDate1.Value = DateTime.Now;
      this.dateTimePickerCommDate2.CustomFormat = "' '";
      this.dateTimePickerCommDate2.Value = DateTime.Now;
      this.cmbBoxCommUserIDCondition.SelectedIndex = 0;
      this.cmbBoxCommUserIDCondition_SelectedIndexChanged((object) null, (EventArgs) null);
      this.cmbBoxCommSubject.SelectedIndex = 0;
      this.txtBoxCommSubject.Text = "";
      this.cmbBoxCommDetails.SelectedIndex = 0;
      this.txtBoxCommDetails.Text = "";
      this.cmbBoxContactSourceCondition.SelectedIndex = 0;
      this.cmbBoxContactSource.SelectedIndex = 0;
      this.txtBoxCommUserID.Text = "";
    }

    protected override void loadQuery()
    {
      this.Reset();
      foreach (ContactQueryItem contactQueryItem in this._StaticQuery.Items)
      {
        switch (contactQueryItem.FieldName)
        {
          case "History.ContactSource":
            this.cmbBoxContactSourceCondition.Text = contactQueryItem.Condition;
            this.cmbBoxContactSource.Text = contactQueryItem.Value1;
            break;
          case "History.EventType":
            if (contactQueryItem.Value1 == "Emailed" || contactQueryItem.Value1 == "Called" || contactQueryItem.Value1 == "Faxed")
            {
              this.cmbBoxActivity.Text = contactQueryItem.Value1.Substring(0, contactQueryItem.Value1.Length - 2);
              break;
            }
            this.cmbBoxActivity.Text = contactQueryItem.Value1;
            break;
          case "History.LetterName":
            this.cmbBoxMergeDocument.Text = contactQueryItem.Condition;
            this.txtBoxMergeDocument.Text = contactQueryItem.Value1;
            break;
          case "History.Sender":
            this.cmbBoxCommUserIDCondition.Text = contactQueryItem.Condition;
            this.txtBoxCommUserID.Text = contactQueryItem.Value1;
            break;
          case "History.Subject":
          case "RelatedNote.Subject":
            this.cmbBoxCommSubject.Text = contactQueryItem.Condition;
            this.txtBoxCommSubject.Text = contactQueryItem.Value1;
            break;
          case "History.TimeOfHistory":
            this.cmbBoxCommDate.Text = contactQueryItem.Condition;
            this.dateTimePickerCommDate1.CustomFormat = "MM'/'dd'/'yyyy";
            this.dateTimePickerCommDate1.Value = Utils.ParseDate((object) contactQueryItem.Value1);
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.dateTimePickerCommDate2.CustomFormat = "MM'/'dd'/'yyyy";
              this.dateTimePickerCommDate2.Value = Utils.ParseDate((object) contactQueryItem.Value2);
              break;
            }
            break;
          case "RelatedNote.Details":
            this.cmbBoxCommDetails.Text = contactQueryItem.Condition;
            this.txtBoxCommDetails.Text = contactQueryItem.Value1;
            break;
        }
      }
    }

    public override void LoadQuery(
      QueryCriterion[] defaultCriteria,
      RelatedLoanMatchType defaultMatchType)
    {
    }

    public override ContactQuery GetSearchCriteria()
    {
      this._StaticQuery.Items = new ContactQueryItem[0];
      this.GetCommDateQuery();
      this.GetDocNameQuery();
      this.GetCommUserIDQuery();
      this.GetCommSubject();
      this.GetCommDetails();
      this.GetContactSource();
      if (this._StaticQuery.Items.Length != 0)
        this.GetActivityQuery();
      return this._StaticQuery;
    }

    public override bool ValidateUserInput() => true;

    private void GetActivityQuery()
    {
      ContactQueryItem contactQueryItem = new ContactQueryItem();
      contactQueryItem.FieldDisplayName = "Marketing Activity";
      contactQueryItem.FieldName = "History.EventType";
      contactQueryItem.GroupName = "History";
      contactQueryItem.Condition = "Is";
      contactQueryItem.Value1 = this.cmbBoxActivity.Text.Trim();
      if (this.cmbBoxActivity.Text.Trim() == "Email" || this.cmbBoxActivity.Text.Trim() == "Call" || this.cmbBoxActivity.Text.Trim() == "Fax")
        contactQueryItem.Value1 += "ed";
      contactQueryItem.Value2 = string.Empty;
      contactQueryItem.ValueType = "System.String";
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) contactQueryItem);
    }

    private void GetCommDateQuery()
    {
      if (this.dateTimePickerCommDate1.Text.Trim() == string.Empty || this.cmbBoxCommDate.Text == "Between" && this.dateTimePickerCommDate2.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Communication Date",
        FieldName = "History.TimeOfHistory",
        GroupName = "History",
        Condition = this.cmbBoxCommDate.Text,
        Value1 = this.dateTimePickerCommDate1.Text.Trim(),
        Value2 = this.dateTimePickerCommDate2.Text.Trim(),
        ValueType = "System.DateTime"
      });
    }

    private void GetDocNameQuery()
    {
      if (this.txtBoxMergeDocument.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Merge Document Name",
        FieldName = "History.LetterName",
        GroupName = "History",
        Condition = this.cmbBoxMergeDocument.Text,
        Value1 = this.txtBoxMergeDocument.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetCommUserIDQuery()
    {
      if (this.txtBoxCommUserID.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Communicator User ID",
        FieldName = "History.Sender",
        GroupName = "History",
        Condition = this.cmbBoxCommUserIDCondition.Text,
        Value1 = this.txtBoxCommUserID.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetCommSubject()
    {
      if (this.txtBoxCommSubject.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Communication Subject",
        FieldName = (this.cmbBoxActivity.Text == "Email" || this.cmbBoxActivity.Text == "Fax" || this.cmbBoxActivity.Text == "Call" ? "RelatedNote.Subject" : "History.Subject"),
        GroupName = "History",
        Condition = this.cmbBoxCommSubject.Text,
        Value1 = this.txtBoxCommSubject.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetCommDetails()
    {
      if (this.txtBoxCommDetails.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Communication Details",
        FieldName = "RelatedNote.Details",
        GroupName = "History",
        Condition = this.cmbBoxCommDetails.Text,
        Value1 = this.txtBoxCommDetails.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetContactSource()
    {
      if (this.cmbBoxContactSource.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Contact Data Source",
        FieldName = "History.ContactSource",
        GroupName = "History",
        Condition = this.cmbBoxContactSourceCondition.Text,
        Value1 = this.cmbBoxContactSource.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void cmbBoxCommDate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxCommDate.Text == "Between")
      {
        this.lblAndCommDate.Visible = true;
        this.dateTimePickerCommDate2.Enabled = true;
        this.dateTimePickerCommDate2.Visible = true;
        this.dateTimePickerCommDate2.CustomFormat = "' '";
        this.dateTimePickerCommDate2.Value = DateTime.Now;
      }
      else
      {
        this.lblAndCommDate.Visible = false;
        this.dateTimePickerCommDate2.Enabled = false;
        this.dateTimePickerCommDate2.Visible = false;
        this.dateTimePickerCommDate2.CustomFormat = "' '";
        this.dateTimePickerCommDate2.Value = DateTime.Now;
      }
    }

    private void dateTimePickerCommDate1_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerCommDate1.CustomFormat = "MM'/'dd'/'yyyy";
    }

    private void dateTimePickerCommDate2_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerCommDate2.CustomFormat = "MM'/'dd'/'yyyy";
    }

    private void enableAll()
    {
      this.cmbBoxMergeDocument.Enabled = true;
      this.txtBoxMergeDocument.Enabled = true;
      this.cmbBoxCommSubject.Enabled = true;
      this.txtBoxCommSubject.Enabled = true;
      this.cmbBoxCommDetails.Enabled = true;
      this.txtBoxCommDetails.Enabled = true;
      this.cmbBoxCommUserIDCondition.Enabled = true;
      this.txtBoxCommUserID.Enabled = true;
      this.cmbBoxContactSource.Enabled = true;
      this.cmbBoxContactSourceCondition.Enabled = true;
    }

    private void cmbBoxActivity_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.enableAll();
      switch (this.cmbBoxActivity.Text)
      {
        case "Mail Merge":
          this.cmbBoxCommSubject.Enabled = false;
          this.txtBoxCommSubject.Enabled = false;
          this.cmbBoxCommDetails.Enabled = false;
          this.txtBoxCommDetails.Enabled = false;
          this.cmbBoxContactSource.Enabled = false;
          this.cmbBoxContactSourceCondition.Enabled = false;
          break;
        case "Email Merge":
          this.cmbBoxCommDetails.Enabled = false;
          this.txtBoxCommDetails.Enabled = false;
          this.cmbBoxContactSource.Enabled = false;
          this.cmbBoxContactSourceCondition.Enabled = false;
          break;
        case "Call":
        case "Email":
        case "Fax":
          this.cmbBoxMergeDocument.Enabled = false;
          this.txtBoxMergeDocument.Enabled = false;
          this.cmbBoxContactSource.Enabled = false;
          this.cmbBoxContactSourceCondition.Enabled = false;
          break;
        case "First Contact":
          this.cmbBoxCommUserIDCondition.Enabled = false;
          this.txtBoxCommUserID.Enabled = false;
          this.cmbBoxCommSubject.Enabled = false;
          this.txtBoxCommSubject.Enabled = false;
          this.cmbBoxCommDetails.Enabled = false;
          this.txtBoxCommDetails.Enabled = false;
          this.cmbBoxMergeDocument.Enabled = false;
          this.txtBoxMergeDocument.Enabled = false;
          break;
      }
      this.initForCtx();
    }

    private void cmbBoxCommUserIDCondition_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxCommUserIDCondition.Text.StartsWith("Is"))
      {
        this.txtBoxCommUserID.ReadOnly = true;
        this.txtBoxCommUserID.Text = "";
      }
      else
        this.txtBoxCommUserID.ReadOnly = false;
    }

    private void btnCommUserID_Click(object sender, EventArgs e)
    {
      using (ContactAssignment contactAssignment = new ContactAssignment(this.session, AclFeature.Cnt_Borrower_CreateNew, ""))
      {
        if (contactAssignment.ShowDialog() != DialogResult.OK)
          return;
        this.txtBoxCommUserID.Text = contactAssignment.AssigneeID;
      }
    }
  }
}
