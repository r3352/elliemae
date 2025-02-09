// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizSearchLoanInfoPage
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizSearchLoanInfoPage : ContactSearchPage
  {
    private GroupBox groupBox1;
    private DateTimePicker dateTimePickerClosedDate2;
    private DateTimePicker dateTimePickerClosedDate1;
    private Label lblAndTerm;
    private TextBox txtBoxTerm2;
    private ComboBox cmbBoxTerm;
    private Label lblAndLoanAmount;
    private Label lblAndRate;
    private Label lblAndClosedDate;
    private ComboBox cmbBoxLien;
    private ComboBox cmbBoxLoanType;
    private ComboBox cmbBoxPurpose;
    private ComboBox cmbBoxAmor;
    private TextBox txtBoxTerm1;
    private TextBox txtBoxPrice2;
    private TextBox txtBoxPrice1;
    private TextBox txtBoxRate2;
    private TextBox txtBoxRate1;
    private ComboBox cmbBoxPrice;
    private ComboBox cmbBoxRate;
    private ComboBox cmbBoxClosedDate;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label2;
    private Label label3;
    private Label label1;
    private RadioButton rbtnAllClosedLoans;
    private RadioButton rbtnLastLoanClosed;
    private System.ComponentModel.Container components;
    private RadioButton rbtnAllContacts;
    private string _ContactLabel = string.Empty;
    private Label lblAndLTV;
    private TextBox txtBoxLTV2;
    private ComboBox cmbBoxLTV;
    private TextBox txtBoxLTV1;
    private Label lblLTV;
    private ContactSearchContext searchContext;

    public BizSearchLoanInfoPage(ContactSearchContext searchContext)
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
      this.groupBox1 = new GroupBox();
      this.lblAndLTV = new Label();
      this.txtBoxLTV2 = new TextBox();
      this.cmbBoxLTV = new ComboBox();
      this.txtBoxLTV1 = new TextBox();
      this.lblLTV = new Label();
      this.dateTimePickerClosedDate2 = new DateTimePicker();
      this.dateTimePickerClosedDate1 = new DateTimePicker();
      this.lblAndTerm = new Label();
      this.txtBoxTerm2 = new TextBox();
      this.cmbBoxTerm = new ComboBox();
      this.lblAndLoanAmount = new Label();
      this.lblAndRate = new Label();
      this.lblAndClosedDate = new Label();
      this.cmbBoxLien = new ComboBox();
      this.cmbBoxLoanType = new ComboBox();
      this.cmbBoxPurpose = new ComboBox();
      this.cmbBoxAmor = new ComboBox();
      this.txtBoxTerm1 = new TextBox();
      this.txtBoxPrice2 = new TextBox();
      this.txtBoxPrice1 = new TextBox();
      this.txtBoxRate2 = new TextBox();
      this.txtBoxRate1 = new TextBox();
      this.cmbBoxPrice = new ComboBox();
      this.cmbBoxRate = new ComboBox();
      this.cmbBoxClosedDate = new ComboBox();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label1 = new Label();
      this.rbtnAllClosedLoans = new RadioButton();
      this.rbtnLastLoanClosed = new RadioButton();
      this.rbtnAllContacts = new RadioButton();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.lblAndLTV);
      this.groupBox1.Controls.Add((Control) this.txtBoxLTV2);
      this.groupBox1.Controls.Add((Control) this.cmbBoxLTV);
      this.groupBox1.Controls.Add((Control) this.txtBoxLTV1);
      this.groupBox1.Controls.Add((Control) this.lblLTV);
      this.groupBox1.Controls.Add((Control) this.dateTimePickerClosedDate2);
      this.groupBox1.Controls.Add((Control) this.dateTimePickerClosedDate1);
      this.groupBox1.Controls.Add((Control) this.lblAndTerm);
      this.groupBox1.Controls.Add((Control) this.txtBoxTerm2);
      this.groupBox1.Controls.Add((Control) this.cmbBoxTerm);
      this.groupBox1.Controls.Add((Control) this.lblAndLoanAmount);
      this.groupBox1.Controls.Add((Control) this.lblAndRate);
      this.groupBox1.Controls.Add((Control) this.lblAndClosedDate);
      this.groupBox1.Controls.Add((Control) this.cmbBoxLien);
      this.groupBox1.Controls.Add((Control) this.cmbBoxLoanType);
      this.groupBox1.Controls.Add((Control) this.cmbBoxPurpose);
      this.groupBox1.Controls.Add((Control) this.cmbBoxAmor);
      this.groupBox1.Controls.Add((Control) this.txtBoxTerm1);
      this.groupBox1.Controls.Add((Control) this.txtBoxPrice2);
      this.groupBox1.Controls.Add((Control) this.txtBoxPrice1);
      this.groupBox1.Controls.Add((Control) this.txtBoxRate2);
      this.groupBox1.Controls.Add((Control) this.txtBoxRate1);
      this.groupBox1.Controls.Add((Control) this.cmbBoxPrice);
      this.groupBox1.Controls.Add((Control) this.cmbBoxRate);
      this.groupBox1.Controls.Add((Control) this.cmbBoxClosedDate);
      this.groupBox1.Controls.Add((Control) this.label8);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(16, 32);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(424, 261);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Loan Info";
      this.lblAndLTV.Location = new Point(293, 234);
      this.lblAndLTV.Name = "lblAndLTV";
      this.lblAndLTV.Size = new Size(24, 16);
      this.lblAndLTV.TabIndex = 31;
      this.lblAndLTV.Text = "and";
      this.txtBoxLTV2.Location = new Point(321, 232);
      this.txtBoxLTV2.MaxLength = 9;
      this.txtBoxLTV2.Name = "txtBoxLTV2";
      this.txtBoxLTV2.Size = new Size(84, 20);
      this.txtBoxLTV2.TabIndex = 18;
      this.cmbBoxLTV.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxLTV.Location = new Point(103, 232);
      this.cmbBoxLTV.Name = "cmbBoxLTV";
      this.cmbBoxLTV.Size = new Size(84, 21);
      this.cmbBoxLTV.TabIndex = 16;
      this.cmbBoxLTV.SelectedIndexChanged += new EventHandler(this.cmbBoxLTV_SelectedIndexChanged);
      this.txtBoxLTV1.Location = new Point(201, 232);
      this.txtBoxLTV1.MaxLength = 9;
      this.txtBoxLTV1.Name = "txtBoxLTV1";
      this.txtBoxLTV1.Size = new Size(84, 20);
      this.txtBoxLTV1.TabIndex = 17;
      this.lblLTV.Location = new Point(13, 234);
      this.lblLTV.Name = "lblLTV";
      this.lblLTV.Size = new Size(80, 16);
      this.lblLTV.TabIndex = 28;
      this.lblLTV.Text = "LTV";
      this.dateTimePickerClosedDate2.CustomFormat = "' '";
      this.dateTimePickerClosedDate2.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerClosedDate2.Location = new Point(320, 24);
      this.dateTimePickerClosedDate2.Name = "dateTimePickerClosedDate2";
      this.dateTimePickerClosedDate2.Size = new Size(84, 20);
      this.dateTimePickerClosedDate2.TabIndex = 2;
      this.dateTimePickerClosedDate2.CloseUp += new EventHandler(this.dateTimePickerClosedDate2_CloseUp);
      this.dateTimePickerClosedDate1.CustomFormat = "' '";
      this.dateTimePickerClosedDate1.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerClosedDate1.Location = new Point(200, 24);
      this.dateTimePickerClosedDate1.Name = "dateTimePickerClosedDate1";
      this.dateTimePickerClosedDate1.Size = new Size(84, 20);
      this.dateTimePickerClosedDate1.TabIndex = 1;
      this.dateTimePickerClosedDate1.CloseUp += new EventHandler(this.dateTimePickerClosedDate1_CloseUp);
      this.lblAndTerm.Location = new Point(292, 128);
      this.lblAndTerm.Name = "lblAndTerm";
      this.lblAndTerm.Size = new Size(24, 16);
      this.lblAndTerm.TabIndex = 20;
      this.lblAndTerm.Text = "and";
      this.txtBoxTerm2.Location = new Point(320, 124);
      this.txtBoxTerm2.MaxLength = 9;
      this.txtBoxTerm2.Name = "txtBoxTerm2";
      this.txtBoxTerm2.Size = new Size(84, 20);
      this.txtBoxTerm2.TabIndex = 12;
      this.cmbBoxTerm.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxTerm.Location = new Point(102, 124);
      this.cmbBoxTerm.Name = "cmbBoxTerm";
      this.cmbBoxTerm.Size = new Size(84, 21);
      this.cmbBoxTerm.TabIndex = 10;
      this.cmbBoxTerm.SelectedIndexChanged += new EventHandler(this.cmbBoxTerm_SelectedIndexChanged);
      this.lblAndLoanAmount.Location = new Point(292, 76);
      this.lblAndLoanAmount.Name = "lblAndLoanAmount";
      this.lblAndLoanAmount.Size = new Size(24, 16);
      this.lblAndLoanAmount.TabIndex = 13;
      this.lblAndLoanAmount.Text = "and";
      this.lblAndRate.Location = new Point(292, 52);
      this.lblAndRate.Name = "lblAndRate";
      this.lblAndRate.Size = new Size(24, 16);
      this.lblAndRate.TabIndex = 8;
      this.lblAndRate.Text = "and";
      this.lblAndClosedDate.Location = new Point(292, 28);
      this.lblAndClosedDate.Name = "lblAndClosedDate";
      this.lblAndClosedDate.Size = new Size(24, 16);
      this.lblAndClosedDate.TabIndex = 3;
      this.lblAndClosedDate.Text = "and";
      this.cmbBoxLien.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxLien.Location = new Point(200, 204);
      this.cmbBoxLien.Name = "cmbBoxLien";
      this.cmbBoxLien.Size = new Size(204, 21);
      this.cmbBoxLien.TabIndex = 15;
      this.cmbBoxLoanType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxLoanType.Location = new Point(200, 176);
      this.cmbBoxLoanType.Name = "cmbBoxLoanType";
      this.cmbBoxLoanType.Size = new Size(204, 21);
      this.cmbBoxLoanType.TabIndex = 14;
      this.cmbBoxPurpose.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxPurpose.Location = new Point(200, 148);
      this.cmbBoxPurpose.Name = "cmbBoxPurpose";
      this.cmbBoxPurpose.Size = new Size(204, 21);
      this.cmbBoxPurpose.TabIndex = 13;
      this.cmbBoxAmor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxAmor.Location = new Point(200, 96);
      this.cmbBoxAmor.Name = "cmbBoxAmor";
      this.cmbBoxAmor.Size = new Size(204, 21);
      this.cmbBoxAmor.TabIndex = 9;
      this.txtBoxTerm1.Location = new Point(200, 124);
      this.txtBoxTerm1.MaxLength = 9;
      this.txtBoxTerm1.Name = "txtBoxTerm1";
      this.txtBoxTerm1.Size = new Size(84, 20);
      this.txtBoxTerm1.TabIndex = 11;
      this.txtBoxPrice2.Location = new Point(320, 72);
      this.txtBoxPrice2.MaxLength = 13;
      this.txtBoxPrice2.Name = "txtBoxPrice2";
      this.txtBoxPrice2.Size = new Size(84, 20);
      this.txtBoxPrice2.TabIndex = 8;
      this.txtBoxPrice1.Location = new Point(200, 72);
      this.txtBoxPrice1.MaxLength = 13;
      this.txtBoxPrice1.Name = "txtBoxPrice1";
      this.txtBoxPrice1.Size = new Size(84, 20);
      this.txtBoxPrice1.TabIndex = 7;
      this.txtBoxRate2.Location = new Point(320, 48);
      this.txtBoxRate2.MaxLength = 10;
      this.txtBoxRate2.Name = "txtBoxRate2";
      this.txtBoxRate2.Size = new Size(84, 20);
      this.txtBoxRate2.TabIndex = 5;
      this.txtBoxRate1.Location = new Point(200, 48);
      this.txtBoxRate1.MaxLength = 10;
      this.txtBoxRate1.Name = "txtBoxRate1";
      this.txtBoxRate1.Size = new Size(84, 20);
      this.txtBoxRate1.TabIndex = 4;
      this.cmbBoxPrice.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxPrice.Location = new Point(102, 72);
      this.cmbBoxPrice.Name = "cmbBoxPrice";
      this.cmbBoxPrice.Size = new Size(84, 21);
      this.cmbBoxPrice.TabIndex = 6;
      this.cmbBoxPrice.SelectedIndexChanged += new EventHandler(this.cmbBoxPrice_SelectedIndexChanged);
      this.cmbBoxRate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxRate.Location = new Point(102, 48);
      this.cmbBoxRate.Name = "cmbBoxRate";
      this.cmbBoxRate.Size = new Size(84, 21);
      this.cmbBoxRate.TabIndex = 3;
      this.cmbBoxRate.SelectedIndexChanged += new EventHandler(this.cmbBoxRate_SelectedIndexChanged);
      this.cmbBoxClosedDate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxClosedDate.Location = new Point(102, 24);
      this.cmbBoxClosedDate.Name = "cmbBoxClosedDate";
      this.cmbBoxClosedDate.Size = new Size(84, 21);
      this.cmbBoxClosedDate.TabIndex = 0;
      this.cmbBoxClosedDate.SelectedIndexChanged += new EventHandler(this.cmbBoxClosedDate_SelectedIndexChanged);
      this.label8.Location = new Point(12, 208);
      this.label8.Name = "label8";
      this.label8.Size = new Size(80, 16);
      this.label8.TabIndex = 26;
      this.label8.Text = "Lien Position";
      this.label7.Location = new Point(12, 180);
      this.label7.Name = "label7";
      this.label7.Size = new Size(80, 16);
      this.label7.TabIndex = 24;
      this.label7.Text = "Loan Type";
      this.label6.Location = new Point(12, 152);
      this.label6.Name = "label6";
      this.label6.Size = new Size(80, 16);
      this.label6.TabIndex = 22;
      this.label6.Text = "Purpose";
      this.label5.Location = new Point(12, 128);
      this.label5.Name = "label5";
      this.label5.Size = new Size(80, 16);
      this.label5.TabIndex = 17;
      this.label5.Text = "Term";
      this.label4.Location = new Point(12, 100);
      this.label4.Name = "label4";
      this.label4.Size = new Size(80, 16);
      this.label4.TabIndex = 15;
      this.label4.Text = "Amortization";
      this.label2.Location = new Point(12, 76);
      this.label2.Name = "label2";
      this.label2.Size = new Size(80, 16);
      this.label2.TabIndex = 10;
      this.label2.Text = "Loan Amount";
      this.label3.Location = new Point(12, 52);
      this.label3.Name = "label3";
      this.label3.Size = new Size(80, 16);
      this.label3.TabIndex = 5;
      this.label3.Text = "Rate";
      this.label1.Location = new Point(12, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(100, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Completion Date";
      this.rbtnAllClosedLoans.Location = new Point(309, 8);
      this.rbtnAllClosedLoans.Name = "rbtnAllClosedLoans";
      this.rbtnAllClosedLoans.Size = new Size(128, 20);
      this.rbtnAllClosedLoans.TabIndex = 2;
      this.rbtnAllClosedLoans.Text = "Any Completed Loan";
      this.rbtnLastLoanClosed.Location = new Point(168, 8);
      this.rbtnLastLoanClosed.Name = "rbtnLastLoanClosed";
      this.rbtnLastLoanClosed.Size = new Size(136, 20);
      this.rbtnLastLoanClosed.TabIndex = 1;
      this.rbtnLastLoanClosed.Text = "Last Completed Loan";
      this.rbtnAllContacts.Location = new Point(32, 8);
      this.rbtnAllContacts.Name = "rbtnAllContacts";
      this.rbtnAllContacts.Size = new Size(128, 20);
      this.rbtnAllContacts.TabIndex = 0;
      this.rbtnAllContacts.Text = "Regardless of Loans";
      this.rbtnAllContacts.CheckedChanged += new EventHandler(this.rbtnAllContacts_CheckedChanged);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(456, 304);
      this.Controls.Add((Control) this.rbtnAllContacts);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.rbtnAllClosedLoans);
      this.Controls.Add((Control) this.rbtnLastLoanClosed);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BizSearchLoanInfoPage);
      this.Text = "BorSearchLoanInfoPage";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForCampaignCtx()
    {
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
      this.rbtnAllContacts.Checked = true;
      this.cmbBoxClosedDate.Items.Clear();
      this.cmbBoxClosedDate.Items.AddRange(DateConditionEnumUtil.GetDisplayNames());
      this.cmbBoxClosedDate.SelectedIndex = 0;
      this.lblAndClosedDate.Visible = false;
      this.dateTimePickerClosedDate2.Enabled = false;
      this.dateTimePickerClosedDate2.Visible = false;
      this.cmbBoxRate.Items.Clear();
      this.cmbBoxRate.Items.AddRange(NumberConditionEnumUtil.GetDisplayNames());
      this.cmbBoxRate.SelectedIndex = 0;
      this.lblAndRate.Visible = false;
      this.txtBoxRate2.Enabled = false;
      this.txtBoxRate2.Visible = false;
      this.cmbBoxPrice.Items.Clear();
      this.cmbBoxPrice.Items.AddRange(NumberConditionEnumUtil.GetDisplayNames());
      this.cmbBoxPrice.SelectedIndex = 0;
      this.lblAndLoanAmount.Visible = false;
      this.txtBoxPrice2.Enabled = false;
      this.txtBoxPrice2.Visible = false;
      this.cmbBoxLTV.Items.Clear();
      this.cmbBoxLTV.Items.AddRange(NumberConditionEnumUtil.GetDisplayNames());
      this.cmbBoxLTV.SelectedIndex = 0;
      this.lblAndLTV.Visible = false;
      this.txtBoxLTV2.Enabled = false;
      this.txtBoxLTV2.Visible = false;
      this.cmbBoxTerm.Items.Clear();
      this.cmbBoxTerm.Items.AddRange(NumberConditionEnumUtil.GetDisplayNames());
      this.cmbBoxTerm.SelectedIndex = 0;
      this.lblAndTerm.Visible = false;
      this.txtBoxTerm2.Enabled = false;
      this.txtBoxTerm2.Visible = false;
      this.cmbBoxAmor.Items.Clear();
      this.cmbBoxAmor.Items.AddRange(AmortizationTypeEnumUtil.GetDisplayNamesInContactSearch());
      this.cmbBoxAmor.SelectedIndex = 0;
      this.cmbBoxPurpose.Items.Clear();
      this.cmbBoxPurpose.Items.AddRange(LoanPurposeEnumUtil.GetDisplayNamesInContactSearch());
      this.cmbBoxPurpose.SelectedIndex = 0;
      this.cmbBoxLoanType.Items.Clear();
      this.cmbBoxLoanType.Items.AddRange(LoanTypeEnumUtil.GetDisplayNamesInContactSearch());
      this.cmbBoxLoanType.SelectedIndex = 0;
      this.cmbBoxLien.Items.Clear();
      this.cmbBoxLien.Items.AddRange(new object[3]
      {
        (object) string.Empty,
        (object) "FirstLien",
        (object) "SecondLien"
      });
      this.cmbBoxLien.SelectedIndex = 0;
      this.initForCtx();
    }

    public override void SetLoanMatchType(RelatedLoanMatchType matchType)
    {
      this.loanMatchType = matchType;
      if (matchType == RelatedLoanMatchType.LastClosed)
        this.rbtnLastLoanClosed.Checked = true;
      else if (matchType == RelatedLoanMatchType.AnyClosed)
        this.rbtnAllClosedLoans.Checked = true;
      else
        this.rbtnAllContacts.Checked = true;
    }

    public override void Reset()
    {
      this.rbtnAllContacts.Checked = true;
      this.cmbBoxClosedDate.SelectedIndex = 0;
      this.dateTimePickerClosedDate1.CustomFormat = "' '";
      this.dateTimePickerClosedDate1.Value = DateTime.Now;
      this.dateTimePickerClosedDate2.CustomFormat = "' '";
      this.dateTimePickerClosedDate2.Value = DateTime.Now;
      this.cmbBoxRate.SelectedIndex = 0;
      this.txtBoxRate1.Text = "";
      this.txtBoxRate2.Text = "";
      this.cmbBoxPrice.SelectedIndex = 0;
      this.txtBoxPrice1.Text = "";
      this.txtBoxPrice2.Text = "";
      this.cmbBoxLTV.SelectedIndex = 0;
      this.txtBoxLTV1.Text = "";
      this.txtBoxLTV2.Text = "";
      this.cmbBoxAmor.SelectedIndex = 0;
      this.cmbBoxTerm.SelectedIndex = 0;
      this.txtBoxTerm1.Text = "";
      this.txtBoxTerm2.Text = "";
      this.cmbBoxPurpose.SelectedIndex = 0;
      this.cmbBoxLoanType.SelectedIndex = 0;
      this.cmbBoxLien.SelectedIndex = 0;
    }

    protected override void loadQuery()
    {
      this.Reset();
      foreach (ContactQueryItem contactQueryItem in this._StaticQuery.Items)
      {
        switch (contactQueryItem.FieldName)
        {
          case "RelatedLoan.Amortization":
            if (contactQueryItem.Value1 == "AdjustableRate")
            {
              this.cmbBoxAmor.Text = "ARM";
              break;
            }
            this.cmbBoxAmor.Text = contactQueryItem.Value1;
            break;
          case "RelatedLoan.ClosedDate":
          case "RelatedLoan.DateCompleted":
            this.cmbBoxClosedDate.Text = contactQueryItem.Condition;
            this.dateTimePickerClosedDate1.CustomFormat = "MM'/'dd";
            this.dateTimePickerClosedDate1.Value = Utils.ParseDate((object) contactQueryItem.Value1);
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.dateTimePickerClosedDate2.CustomFormat = "MM'/'dd";
              this.dateTimePickerClosedDate2.Value = Utils.ParseDate((object) contactQueryItem.Value2);
              break;
            }
            break;
          case "RelatedLoan.InterestRate":
            this.cmbBoxRate.Text = contactQueryItem.Condition;
            this.txtBoxRate1.Text = contactQueryItem.Value1;
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.txtBoxRate2.Text = contactQueryItem.Value2;
              break;
            }
            break;
          case "RelatedLoan.LTV":
            this.cmbBoxLTV.Text = contactQueryItem.Condition;
            this.txtBoxLTV1.Text = contactQueryItem.Value1;
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.txtBoxLTV2.Text = contactQueryItem.Value2;
              break;
            }
            break;
          case "RelatedLoan.LienPosition":
            this.cmbBoxLien.Text = contactQueryItem.Value1;
            break;
          case "RelatedLoan.LoanAmount":
            this.cmbBoxPrice.Text = contactQueryItem.Condition;
            this.txtBoxPrice1.Text = contactQueryItem.Value1;
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.txtBoxPrice2.Text = contactQueryItem.Value2;
              break;
            }
            break;
          case "RelatedLoan.LoanType":
            this.cmbBoxLoanType.Text = contactQueryItem.Value1;
            break;
          case "RelatedLoan.Purpose":
            this.cmbBoxPurpose.Text = contactQueryItem.Value1;
            break;
          case "RelatedLoan.Term":
            this.cmbBoxTerm.Text = contactQueryItem.Condition;
            this.txtBoxTerm1.Text = contactQueryItem.Value1;
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.txtBoxTerm2.Text = contactQueryItem.Value2;
              break;
            }
            break;
        }
      }
    }

    public override void LoadQuery(
      QueryCriterion[] defaultCriteria,
      RelatedLoanMatchType defaultMatchType)
    {
      switch (defaultMatchType)
      {
        case RelatedLoanMatchType.AnyClosed:
          this.rbtnAllClosedLoans.Checked = true;
          break;
        case RelatedLoanMatchType.LastClosed:
          this.rbtnLastLoanClosed.Checked = true;
          break;
        default:
          this.rbtnAllContacts.Checked = true;
          break;
      }
      int num = 0;
      for (int index = 0; index < defaultCriteria.Length; ++index)
      {
        if (defaultCriteria[index] is FieldValueCriterion defaultCriterion)
        {
          switch (defaultCriterion.FieldName)
          {
            case "RelatedLoan.Amortization":
              this.initializeEnumField(this.cmbBoxAmor, (StringValueCriterion) defaultCriterion);
              continue;
            case "RelatedLoan.ClosedDate":
            case "RelatedLoan.DateCompleted":
              this.initializeDateField(this.cmbBoxClosedDate, this.dateTimePickerClosedDate1, this.dateTimePickerClosedDate2, (DateValueCriterion) defaultCriterion, "MM'/'dd'/'yyyy");
              continue;
            case "RelatedLoan.InterestRate":
              this.initializeNumericField(this.cmbBoxRate, this.txtBoxRate1, this.txtBoxRate2, (OrdinalValueCriterion) defaultCriterion);
              continue;
            case "RelatedLoan.LTV":
              this.initializeNumericField(this.cmbBoxLTV, this.txtBoxLTV1, this.txtBoxLTV2, (OrdinalValueCriterion) defaultCriterion);
              continue;
            case "RelatedLoan.LienPosition":
              this.initializeEnumField(this.cmbBoxLien, (StringValueCriterion) defaultCriterion);
              continue;
            case "RelatedLoan.LoanAmount":
              this.initializeNumericField(this.cmbBoxPrice, this.txtBoxPrice1, this.txtBoxPrice2, (OrdinalValueCriterion) defaultCriterion);
              continue;
            case "RelatedLoan.LoanType":
              this.initializeEnumField(this.cmbBoxLoanType, (StringValueCriterion) defaultCriterion);
              continue;
            case "RelatedLoan.Purpose":
              this.initializeEnumField(this.cmbBoxPurpose, (StringValueCriterion) defaultCriterion);
              continue;
            case "RelatedLoan.Term":
              this.initializeNumericField(this.cmbBoxTerm, this.txtBoxTerm1, this.txtBoxTerm2, (OrdinalValueCriterion) defaultCriterion);
              continue;
            default:
              ++num;
              continue;
          }
        }
      }
    }

    public override ContactQuery GetSearchCriteria()
    {
      this._StaticQuery.Items = new ContactQueryItem[0];
      this.description = "";
      string levelOneName = string.Empty;
      if (this.rbtnLastLoanClosed.Checked)
      {
        levelOneName = "Last Loan Completed";
        this.loanMatchType = RelatedLoanMatchType.LastClosed;
      }
      else if (this.rbtnAllClosedLoans.Checked)
      {
        levelOneName = "Any Loan Completed";
        this.loanMatchType = RelatedLoanMatchType.AnyClosed;
      }
      else
      {
        this.description = "Showing " + this._ContactLabel + " whose\n";
        this.loanMatchType = RelatedLoanMatchType.None;
      }
      if (!this.rbtnAllContacts.Checked)
      {
        this.GetClosedDateQuery(levelOneName);
        this.GetRateQuery(levelOneName);
        this.GetPriceQuery(levelOneName);
        this.GetLTVQuery(levelOneName);
        this.GetAmorQuery(levelOneName);
        this.GetTermQuery(levelOneName);
        this.GetPurposeQuery(levelOneName);
        this.GetLoanTypeQuery(levelOneName);
        this.GetLienQuery(levelOneName);
      }
      if (this.rbtnLastLoanClosed.Checked && this._StaticQuery.Items.Length != 0)
        this.description = "Showing " + this._ContactLabel + " whose last closed loan has\n" + this.description;
      else if (this.rbtnAllClosedLoans.Checked && this._StaticQuery.Items.Length != 0)
        this.description = "Showing " + this._ContactLabel + " whose closed loans have\n" + this.description;
      return this._StaticQuery;
    }

    public override bool ValidateUserInput()
    {
      if (this.txtBoxRate1.Text.Trim() != string.Empty && !Utils.IsDecimal((object) this.txtBoxRate1.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Rate should be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxRate1.Focus();
        return false;
      }
      try
      {
        if (this.txtBoxRate2.Enabled)
        {
          if (this.txtBoxRate2.Text.Trim() != string.Empty)
            Convert.ToDecimal(this.txtBoxRate2.Text);
        }
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Rate should be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxRate2.Focus();
        return false;
      }
      try
      {
        if (this.txtBoxPrice1.Text.Trim() != string.Empty)
          Convert.ToDecimal(this.txtBoxPrice1.Text);
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Loan amount should be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxPrice1.Focus();
        return false;
      }
      try
      {
        if (this.txtBoxPrice2.Enabled)
        {
          if (this.txtBoxPrice2.Text.Trim() != string.Empty)
            Convert.ToDecimal(this.txtBoxPrice2.Text);
        }
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Loan amount should be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxPrice2.Focus();
        return false;
      }
      try
      {
        if (this.txtBoxLTV1.Text.Trim() != string.Empty)
          Convert.ToDecimal(this.txtBoxLTV1.Text);
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "LTV should be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxLTV1.Focus();
        return false;
      }
      try
      {
        if (this.txtBoxLTV2.Enabled)
        {
          if (this.txtBoxLTV2.Text.Trim() != string.Empty)
            Convert.ToDecimal(this.txtBoxLTV2.Text);
        }
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "LTV should be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxLTV2.Focus();
        return false;
      }
      try
      {
        if (this.txtBoxTerm1.Text.Trim() != string.Empty)
          Convert.ToDecimal(this.txtBoxTerm1.Text);
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Term should be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxTerm1.Focus();
        return false;
      }
      try
      {
        if (this.txtBoxTerm2.Enabled)
        {
          if (this.txtBoxTerm2.Text.Trim() != string.Empty)
            Convert.ToDecimal(this.txtBoxTerm2.Text);
        }
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Loan amount should be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtBoxTerm2.Focus();
        return false;
      }
      return true;
    }

    private void GetClosedDateQuery(string levelOneName)
    {
      if (this.dateTimePickerClosedDate1.Text.Trim() == string.Empty || this.cmbBoxClosedDate.Text == "Between" && this.dateTimePickerClosedDate2.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Date Completed",
        FieldName = "RelatedLoan.DateCompleted",
        GroupName = "LoanInfo",
        Condition = this.cmbBoxClosedDate.Text,
        Value1 = this.dateTimePickerClosedDate1.Text.Trim(),
        Value2 = this.dateTimePickerClosedDate2.Text.Trim(),
        ValueType = "System.DateTime"
      });
    }

    private void GetRateQuery(string levelOneName)
    {
      if (this.txtBoxRate1.Text.Trim() == string.Empty || this.cmbBoxRate.Text == "Between" && this.txtBoxRate2.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Rate",
        FieldName = "RelatedLoan.InterestRate",
        GroupName = "LoanInfo",
        Condition = this.cmbBoxRate.Text,
        Value1 = this.txtBoxRate1.Text.Trim(),
        Value2 = this.txtBoxRate2.Text.Trim(),
        ValueType = "System.Double"
      });
    }

    private void GetPriceQuery(string levelOneName)
    {
      if (this.txtBoxPrice1.Text.Trim() == string.Empty || this.cmbBoxPrice.Text == "Between" && this.txtBoxPrice2.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Loan Amount",
        FieldName = "RelatedLoan.LoanAmount",
        GroupName = "LoanInfo",
        Condition = this.cmbBoxPrice.Text,
        Value1 = this.txtBoxPrice1.Text.Trim(),
        Value2 = this.txtBoxPrice2.Text.Trim(),
        ValueType = "System.Double"
      });
    }

    private void GetLTVQuery(string levelOneName)
    {
      if (this.txtBoxLTV1.Text.Trim() == string.Empty || this.cmbBoxLTV.Text == "Between" && this.txtBoxLTV2.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Loan LTV",
        FieldName = "RelatedLoan.LTV",
        GroupName = "LoanInfo",
        Condition = this.cmbBoxLTV.Text,
        Value1 = this.txtBoxLTV1.Text.Trim(),
        Value2 = this.txtBoxLTV2.Text.Trim(),
        ValueType = "System.Double"
      });
    }

    private void GetTermQuery(string levelOneName)
    {
      if (this.txtBoxTerm1.Text.Trim() == string.Empty || this.cmbBoxTerm.Text == "Between" && this.txtBoxTerm2.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Term",
        FieldName = "RelatedLoan.Term",
        GroupName = "LoanInfo",
        Condition = this.cmbBoxTerm.Text,
        Value1 = this.txtBoxTerm1.Text.Trim(),
        Value2 = this.txtBoxTerm2.Text.Trim(),
        ValueType = "System.Int32"
      });
    }

    private void GetAmorQuery(string levelOneName)
    {
      if (this.cmbBoxAmor.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Amortization",
        FieldName = "RelatedLoan.Amortization",
        GroupName = "LoanInfo",
        Condition = "Is",
        Value1 = this.cmbBoxAmor.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetPurposeQuery(string levelOneName)
    {
      if (this.cmbBoxPurpose.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Purpose",
        FieldName = "RelatedLoan.Purpose",
        GroupName = "LoanInfo",
        Condition = "Is",
        Value1 = this.cmbBoxPurpose.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetLoanTypeQuery(string levelOneName)
    {
      if (this.cmbBoxLoanType.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Loan Type",
        FieldName = "RelatedLoan.LoanType",
        GroupName = "LoanInfo",
        Condition = "Is",
        Value1 = this.cmbBoxLoanType.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void GetLienQuery(string levelOneName)
    {
      if (this.cmbBoxLien.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Lien Position",
        FieldName = "RelatedLoan.LienPosition",
        GroupName = "LoanInfo",
        Condition = "Is",
        Value1 = this.cmbBoxLien.Text.Trim(),
        Value2 = string.Empty,
        ValueType = "System.String"
      });
    }

    private void cmbBoxClosedDate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxClosedDate.Text == "Between")
      {
        this.lblAndClosedDate.Visible = true;
        this.dateTimePickerClosedDate2.Enabled = true;
        this.dateTimePickerClosedDate2.Visible = true;
      }
      else
      {
        this.lblAndClosedDate.Visible = false;
        this.dateTimePickerClosedDate2.Enabled = false;
        this.dateTimePickerClosedDate2.Visible = false;
      }
    }

    private void cmbBoxRate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxRate.Text == "Between")
      {
        this.lblAndRate.Visible = true;
        this.txtBoxRate2.Enabled = true;
        this.txtBoxRate2.Visible = true;
      }
      else
      {
        this.lblAndRate.Visible = false;
        this.txtBoxRate2.Enabled = false;
        this.txtBoxRate2.Visible = false;
      }
    }

    private void cmbBoxPrice_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxPrice.Text == "Between")
      {
        this.lblAndLoanAmount.Visible = true;
        this.txtBoxPrice2.Enabled = true;
        this.txtBoxPrice2.Visible = true;
      }
      else
      {
        this.lblAndLoanAmount.Visible = false;
        this.txtBoxPrice2.Enabled = false;
        this.txtBoxPrice2.Visible = false;
      }
    }

    private void cmbBoxLTV_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxLTV.Text == "Between")
      {
        this.lblAndLTV.Visible = true;
        this.txtBoxLTV2.Enabled = true;
        this.txtBoxLTV2.Visible = true;
      }
      else
      {
        this.lblAndLTV.Visible = false;
        this.txtBoxLTV2.Enabled = false;
        this.txtBoxLTV2.Visible = false;
      }
    }

    private void cmbBoxTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxTerm.Text == "Between")
      {
        this.lblAndTerm.Visible = true;
        this.txtBoxTerm2.Enabled = true;
        this.txtBoxTerm2.Visible = true;
      }
      else
      {
        this.lblAndTerm.Visible = false;
        this.txtBoxTerm2.Enabled = false;
        this.txtBoxTerm2.Visible = false;
      }
    }

    private void dateTimePickerClosedDate1_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerClosedDate1.CustomFormat = "MM'/'dd'/'yyyy";
    }

    private void dateTimePickerClosedDate2_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerClosedDate2.CustomFormat = "MM'/'dd'/'yyyy";
    }

    private void rbtnAllContacts_CheckedChanged(object sender, EventArgs e)
    {
      this.groupBox1.Enabled = !this.rbtnAllContacts.Checked;
    }
  }
}
