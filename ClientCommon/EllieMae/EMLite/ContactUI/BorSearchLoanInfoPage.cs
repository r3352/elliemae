// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorSearchLoanInfoPage
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
  public class BorSearchLoanInfoPage : ContactSearchPage
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
    private Label label4;
    private Label label2;
    private Label label3;
    private Label label1;
    private System.ComponentModel.Container components;
    private DateTimePicker dateTimePickerStartDate2;
    private DateTimePicker dateTimePickerStartDate1;
    private ComboBox cmbBoxStartDate;
    private Label label10;
    private ComboBox cmbBoxSeachLoans;
    private Label label11;
    private Label lblAndStartDate;
    private Label lblTerm;
    private string _ContactLabel = string.Empty;
    private Label lblAndLTV;
    private TextBox txtBoxLTV2;
    private TextBox txtBoxLTV1;
    private ComboBox cmbBoxLTV;
    private Label lblLTV;
    private ContactSearchContext searchContext;

    public BorSearchLoanInfoPage(ContactSearchContext searchContext)
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
      this.txtBoxLTV1 = new TextBox();
      this.cmbBoxLTV = new ComboBox();
      this.lblLTV = new Label();
      this.dateTimePickerStartDate2 = new DateTimePicker();
      this.dateTimePickerStartDate1 = new DateTimePicker();
      this.lblAndStartDate = new Label();
      this.cmbBoxStartDate = new ComboBox();
      this.label10 = new Label();
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
      this.lblTerm = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label1 = new Label();
      this.cmbBoxSeachLoans = new ComboBox();
      this.label11 = new Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.lblAndLTV);
      this.groupBox1.Controls.Add((Control) this.txtBoxLTV2);
      this.groupBox1.Controls.Add((Control) this.txtBoxLTV1);
      this.groupBox1.Controls.Add((Control) this.cmbBoxLTV);
      this.groupBox1.Controls.Add((Control) this.lblLTV);
      this.groupBox1.Controls.Add((Control) this.dateTimePickerStartDate2);
      this.groupBox1.Controls.Add((Control) this.dateTimePickerStartDate1);
      this.groupBox1.Controls.Add((Control) this.lblAndStartDate);
      this.groupBox1.Controls.Add((Control) this.cmbBoxStartDate);
      this.groupBox1.Controls.Add((Control) this.label10);
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
      this.groupBox1.Controls.Add((Control) this.lblTerm);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(16, 32);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(424, 264);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Loan Info";
      this.lblAndLTV.Location = new Point(292, 236);
      this.lblAndLTV.Name = "lblAndLTV";
      this.lblAndLTV.Size = new Size(24, 16);
      this.lblAndLTV.TabIndex = 36;
      this.lblAndLTV.Text = "and";
      this.txtBoxLTV2.Location = new Point(320, 232);
      this.txtBoxLTV2.MaxLength = 10;
      this.txtBoxLTV2.Name = "txtBoxLTV2";
      this.txtBoxLTV2.Size = new Size(84, 20);
      this.txtBoxLTV2.TabIndex = 21;
      this.txtBoxLTV1.Location = new Point(200, 232);
      this.txtBoxLTV1.MaxLength = 10;
      this.txtBoxLTV1.Name = "txtBoxLTV1";
      this.txtBoxLTV1.Size = new Size(84, 20);
      this.txtBoxLTV1.TabIndex = 20;
      this.cmbBoxLTV.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxLTV.Location = new Point(100, 232);
      this.cmbBoxLTV.Name = "cmbBoxLTV";
      this.cmbBoxLTV.Size = new Size(84, 21);
      this.cmbBoxLTV.TabIndex = 19;
      this.cmbBoxLTV.SelectedIndexChanged += new EventHandler(this.cmbBoxLTV_SelectedIndexChanged);
      this.lblLTV.Location = new Point(12, 236);
      this.lblLTV.Name = "lblLTV";
      this.lblLTV.Size = new Size(80, 16);
      this.lblLTV.TabIndex = 33;
      this.lblLTV.Text = "LTV";
      this.dateTimePickerStartDate2.CustomFormat = "' '";
      this.dateTimePickerStartDate2.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerStartDate2.Location = new Point(320, 16);
      this.dateTimePickerStartDate2.Name = "dateTimePickerStartDate2";
      this.dateTimePickerStartDate2.Size = new Size(84, 20);
      this.dateTimePickerStartDate2.TabIndex = 2;
      this.dateTimePickerStartDate2.CloseUp += new EventHandler(this.dateTimePickerStartDate2_CloseUp);
      this.dateTimePickerStartDate1.CustomFormat = "' '";
      this.dateTimePickerStartDate1.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerStartDate1.Location = new Point(200, 16);
      this.dateTimePickerStartDate1.Name = "dateTimePickerStartDate1";
      this.dateTimePickerStartDate1.Size = new Size(84, 20);
      this.dateTimePickerStartDate1.TabIndex = 1;
      this.dateTimePickerStartDate1.CloseUp += new EventHandler(this.dateTimePickerStartDate1_CloseUp);
      this.lblAndStartDate.Location = new Point(292, 20);
      this.lblAndStartDate.Name = "lblAndStartDate";
      this.lblAndStartDate.Size = new Size(24, 16);
      this.lblAndStartDate.TabIndex = 31;
      this.lblAndStartDate.Text = "and";
      this.cmbBoxStartDate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxStartDate.Location = new Point(100, 16);
      this.cmbBoxStartDate.Name = "cmbBoxStartDate";
      this.cmbBoxStartDate.Size = new Size(84, 21);
      this.cmbBoxStartDate.TabIndex = 0;
      this.cmbBoxStartDate.SelectedIndexChanged += new EventHandler(this.cmbBoxStartDate_SelectedIndexChanged);
      this.label10.Location = new Point(12, 20);
      this.label10.Name = "label10";
      this.label10.Size = new Size(112, 16);
      this.label10.TabIndex = 28;
      this.label10.Text = "Origination Date";
      this.dateTimePickerClosedDate2.CustomFormat = "' '";
      this.dateTimePickerClosedDate2.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerClosedDate2.Location = new Point(320, 40);
      this.dateTimePickerClosedDate2.Name = "dateTimePickerClosedDate2";
      this.dateTimePickerClosedDate2.Size = new Size(84, 20);
      this.dateTimePickerClosedDate2.TabIndex = 5;
      this.dateTimePickerClosedDate2.CloseUp += new EventHandler(this.dateTimePickerClosedDate2_CloseUp);
      this.dateTimePickerClosedDate1.CustomFormat = "' '";
      this.dateTimePickerClosedDate1.Format = DateTimePickerFormat.Custom;
      this.dateTimePickerClosedDate1.Location = new Point(200, 40);
      this.dateTimePickerClosedDate1.Name = "dateTimePickerClosedDate1";
      this.dateTimePickerClosedDate1.Size = new Size(84, 20);
      this.dateTimePickerClosedDate1.TabIndex = 4;
      this.dateTimePickerClosedDate1.CloseUp += new EventHandler(this.dateTimePickerClosedDate1_CloseUp);
      this.lblAndTerm.Location = new Point(292, 140);
      this.lblAndTerm.Name = "lblAndTerm";
      this.lblAndTerm.Size = new Size(24, 16);
      this.lblAndTerm.TabIndex = 20;
      this.lblAndTerm.Text = "and";
      this.txtBoxTerm2.Location = new Point(320, 136);
      this.txtBoxTerm2.MaxLength = 9;
      this.txtBoxTerm2.Name = "txtBoxTerm2";
      this.txtBoxTerm2.Size = new Size(84, 20);
      this.txtBoxTerm2.TabIndex = 15;
      this.cmbBoxTerm.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxTerm.Location = new Point(100, 136);
      this.cmbBoxTerm.Name = "cmbBoxTerm";
      this.cmbBoxTerm.Size = new Size(84, 21);
      this.cmbBoxTerm.TabIndex = 13;
      this.cmbBoxTerm.SelectedIndexChanged += new EventHandler(this.cmbBoxTerm_SelectedIndexChanged);
      this.lblAndLoanAmount.Location = new Point(292, 92);
      this.lblAndLoanAmount.Name = "lblAndLoanAmount";
      this.lblAndLoanAmount.Size = new Size(24, 16);
      this.lblAndLoanAmount.TabIndex = 13;
      this.lblAndLoanAmount.Text = "and";
      this.lblAndRate.Location = new Point(292, 68);
      this.lblAndRate.Name = "lblAndRate";
      this.lblAndRate.Size = new Size(24, 16);
      this.lblAndRate.TabIndex = 8;
      this.lblAndRate.Text = "and";
      this.lblAndClosedDate.Location = new Point(292, 44);
      this.lblAndClosedDate.Name = "lblAndClosedDate";
      this.lblAndClosedDate.Size = new Size(24, 16);
      this.lblAndClosedDate.TabIndex = 3;
      this.lblAndClosedDate.Text = "and";
      this.cmbBoxLien.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxLien.Location = new Point(200, 208);
      this.cmbBoxLien.Name = "cmbBoxLien";
      this.cmbBoxLien.Size = new Size(204, 21);
      this.cmbBoxLien.TabIndex = 18;
      this.cmbBoxLoanType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxLoanType.Location = new Point(200, 184);
      this.cmbBoxLoanType.Name = "cmbBoxLoanType";
      this.cmbBoxLoanType.Size = new Size(204, 21);
      this.cmbBoxLoanType.TabIndex = 17;
      this.cmbBoxPurpose.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxPurpose.Location = new Point(200, 160);
      this.cmbBoxPurpose.Name = "cmbBoxPurpose";
      this.cmbBoxPurpose.Size = new Size(204, 21);
      this.cmbBoxPurpose.TabIndex = 16;
      this.cmbBoxAmor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxAmor.Location = new Point(200, 112);
      this.cmbBoxAmor.Name = "cmbBoxAmor";
      this.cmbBoxAmor.Size = new Size(204, 21);
      this.cmbBoxAmor.TabIndex = 12;
      this.txtBoxTerm1.Location = new Point(200, 136);
      this.txtBoxTerm1.MaxLength = 9;
      this.txtBoxTerm1.Name = "txtBoxTerm1";
      this.txtBoxTerm1.Size = new Size(84, 20);
      this.txtBoxTerm1.TabIndex = 14;
      this.txtBoxPrice2.Location = new Point(320, 88);
      this.txtBoxPrice2.MaxLength = 13;
      this.txtBoxPrice2.Name = "txtBoxPrice2";
      this.txtBoxPrice2.Size = new Size(84, 20);
      this.txtBoxPrice2.TabIndex = 11;
      this.txtBoxPrice1.Location = new Point(200, 88);
      this.txtBoxPrice1.MaxLength = 13;
      this.txtBoxPrice1.Name = "txtBoxPrice1";
      this.txtBoxPrice1.Size = new Size(84, 20);
      this.txtBoxPrice1.TabIndex = 10;
      this.txtBoxRate2.Location = new Point(320, 64);
      this.txtBoxRate2.MaxLength = 10;
      this.txtBoxRate2.Name = "txtBoxRate2";
      this.txtBoxRate2.Size = new Size(84, 20);
      this.txtBoxRate2.TabIndex = 8;
      this.txtBoxRate1.Location = new Point(200, 64);
      this.txtBoxRate1.MaxLength = 10;
      this.txtBoxRate1.Name = "txtBoxRate1";
      this.txtBoxRate1.Size = new Size(84, 20);
      this.txtBoxRate1.TabIndex = 7;
      this.cmbBoxPrice.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxPrice.Location = new Point(100, 88);
      this.cmbBoxPrice.Name = "cmbBoxPrice";
      this.cmbBoxPrice.Size = new Size(84, 21);
      this.cmbBoxPrice.TabIndex = 9;
      this.cmbBoxPrice.SelectedIndexChanged += new EventHandler(this.cmbBoxPrice_SelectedIndexChanged);
      this.cmbBoxRate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxRate.Location = new Point(100, 64);
      this.cmbBoxRate.Name = "cmbBoxRate";
      this.cmbBoxRate.Size = new Size(84, 21);
      this.cmbBoxRate.TabIndex = 6;
      this.cmbBoxRate.SelectedIndexChanged += new EventHandler(this.cmbBoxRate_SelectedIndexChanged);
      this.cmbBoxClosedDate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxClosedDate.Location = new Point(100, 40);
      this.cmbBoxClosedDate.Name = "cmbBoxClosedDate";
      this.cmbBoxClosedDate.Size = new Size(84, 21);
      this.cmbBoxClosedDate.TabIndex = 3;
      this.cmbBoxClosedDate.SelectedIndexChanged += new EventHandler(this.cmbBoxClosedDate_SelectedIndexChanged);
      this.label8.Location = new Point(12, 212);
      this.label8.Name = "label8";
      this.label8.Size = new Size(80, 16);
      this.label8.TabIndex = 26;
      this.label8.Text = "Lien Position";
      this.label7.Location = new Point(12, 188);
      this.label7.Name = "label7";
      this.label7.Size = new Size(80, 16);
      this.label7.TabIndex = 24;
      this.label7.Text = "Loan Type";
      this.label6.Location = new Point(12, 164);
      this.label6.Name = "label6";
      this.label6.Size = new Size(80, 16);
      this.label6.TabIndex = 22;
      this.label6.Text = "Purpose";
      this.lblTerm.Location = new Point(12, 140);
      this.lblTerm.Name = "lblTerm";
      this.lblTerm.Size = new Size(80, 16);
      this.lblTerm.TabIndex = 17;
      this.lblTerm.Text = "Term";
      this.label4.Location = new Point(12, 116);
      this.label4.Name = "label4";
      this.label4.Size = new Size(80, 16);
      this.label4.TabIndex = 15;
      this.label4.Text = "Amortization";
      this.label2.Location = new Point(12, 92);
      this.label2.Name = "label2";
      this.label2.Size = new Size(80, 16);
      this.label2.TabIndex = 10;
      this.label2.Text = "Loan Amount";
      this.label3.Location = new Point(12, 68);
      this.label3.Name = "label3";
      this.label3.Size = new Size(80, 16);
      this.label3.TabIndex = 5;
      this.label3.Text = "Rate %";
      this.label1.Location = new Point(12, 44);
      this.label1.Name = "label1";
      this.label1.Size = new Size(112, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Completion Date";
      this.cmbBoxSeachLoans.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxSeachLoans.Location = new Point(116, 4);
      this.cmbBoxSeachLoans.Name = "cmbBoxSeachLoans";
      this.cmbBoxSeachLoans.Size = new Size(184, 21);
      this.cmbBoxSeachLoans.TabIndex = 0;
      this.cmbBoxSeachLoans.SelectedIndexChanged += new EventHandler(this.cmbBoxSeachLoans_SelectedIndexChanged);
      this.label11.Location = new Point(24, 8);
      this.label11.Name = "label11";
      this.label11.Size = new Size(112, 16);
      this.label11.TabIndex = 31;
      this.label11.Text = "Search Loans";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(456, 304);
      this.Controls.Add((Control) this.cmbBoxSeachLoans);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BorSearchLoanInfoPage);
      this.Text = nameof (BorSearchLoanInfoPage);
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
      this._ContactLabel = "borrowers";
      this.cmbBoxSeachLoans.Items.Clear();
      this.cmbBoxSeachLoans.Items.AddRange(BorContactLoanSearchEnumUtil.GetDisplayNames());
      this.cmbBoxSeachLoans.SelectedIndex = 0;
      this.cmbBoxStartDate.Items.Clear();
      this.cmbBoxStartDate.Items.AddRange(DateConditionEnumUtil.GetDisplayNames());
      this.cmbBoxStartDate.SelectedIndex = 0;
      this.lblAndStartDate.Visible = false;
      this.dateTimePickerStartDate2.Enabled = false;
      this.dateTimePickerStartDate2.Visible = false;
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

    public override void Reset()
    {
      this.cmbBoxStartDate.SelectedIndex = 0;
      this.dateTimePickerStartDate1.CustomFormat = "' '";
      this.dateTimePickerStartDate1.Value = DateTime.Now;
      this.dateTimePickerStartDate2.CustomFormat = "' '";
      this.dateTimePickerStartDate2.Value = DateTime.Now;
      this.cmbBoxSeachLoans.SelectedIndex = 0;
      this.cmbBoxStartDate.SelectedIndex = 0;
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

    public override void SetLoanMatchType(RelatedLoanMatchType matchType)
    {
      this.loanMatchType = matchType;
      switch (matchType)
      {
        case RelatedLoanMatchType.AnyClosed:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.AnyCompleted);
          break;
        case RelatedLoanMatchType.LastClosed:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.LastCompleted);
          break;
        case RelatedLoanMatchType.AnyOriginated:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.AnyOriginated);
          break;
        case RelatedLoanMatchType.LastOriginated:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.LastOriginated);
          break;
        default:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.Blank);
          break;
      }
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
          case "RelatedLoan.DateStarted":
            this.cmbBoxStartDate.Text = contactQueryItem.Condition;
            this.dateTimePickerStartDate1.CustomFormat = "MM'/'dd";
            this.dateTimePickerStartDate1.Value = Utils.ParseDate((object) contactQueryItem.Value1);
            if (contactQueryItem.Condition.ToLower() == "between")
            {
              this.dateTimePickerStartDate2.CustomFormat = "MM'/'dd";
              this.dateTimePickerStartDate2.Value = Utils.ParseDate((object) contactQueryItem.Value2);
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
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.AnyCompleted);
          break;
        case RelatedLoanMatchType.LastClosed:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.LastCompleted);
          break;
        case RelatedLoanMatchType.AnyOriginated:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.AnyOriginated);
          break;
        case RelatedLoanMatchType.LastOriginated:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.LastOriginated);
          break;
        default:
          this.cmbBoxSeachLoans.Text = BorContactLoanSearchEnumUtil.ValueToName(BorContactLoanSearchEnum.Blank);
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
            case "RelatedLoan.DateStarted":
              this.initializeDateField(this.cmbBoxStartDate, this.dateTimePickerStartDate1, this.dateTimePickerStartDate2, (DateValueCriterion) defaultCriterion, "MM'/'dd'/'yyyy");
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
      BorContactLoanSearchEnum contactLoanSearchEnum = BorContactLoanSearchEnumUtil.NameToValue(this.cmbBoxSeachLoans.Text);
      switch (contactLoanSearchEnum)
      {
        case BorContactLoanSearchEnum.AnyOriginated:
          levelOneName = "Any Loan Originated";
          this.loanMatchType = RelatedLoanMatchType.AnyOriginated;
          this.GetStartDateQuery(levelOneName);
          break;
        case BorContactLoanSearchEnum.LastOriginated:
          levelOneName = "Last Loan Originated";
          this.loanMatchType = RelatedLoanMatchType.LastOriginated;
          this.GetStartDateQuery(levelOneName);
          break;
        case BorContactLoanSearchEnum.AnyCompleted:
          levelOneName = "Any Loan Completed";
          this.loanMatchType = RelatedLoanMatchType.AnyClosed;
          this.GetClosedDateQuery(levelOneName);
          break;
        case BorContactLoanSearchEnum.LastCompleted:
          levelOneName = "Last Loan Completed";
          this.loanMatchType = RelatedLoanMatchType.LastClosed;
          this.GetClosedDateQuery(levelOneName);
          break;
        default:
          this.loanMatchType = RelatedLoanMatchType.None;
          break;
      }
      if (contactLoanSearchEnum != BorContactLoanSearchEnum.Blank)
      {
        this.GetRateQuery(levelOneName);
        this.GetPriceQuery(levelOneName);
        this.GetLTVQuery(levelOneName);
        this.GetAmorQuery(levelOneName);
        this.GetTermQuery(levelOneName);
        this.GetPurposeQuery(levelOneName);
        this.GetLoanTypeQuery(levelOneName);
        this.GetLienQuery(levelOneName);
      }
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

    private void GetStartDateQuery(string levelOneName)
    {
      if (this.dateTimePickerStartDate1.Text.Trim() == string.Empty || this.cmbBoxStartDate.Text == "Between" && this.dateTimePickerStartDate2.Text.Trim() == string.Empty)
        return;
      this._StaticQuery.Items = (ContactQueryItem[]) ArrayUtil.Add((Array) this._StaticQuery.Items, (object) new ContactQueryItem()
      {
        FieldDisplayName = "Date Originated",
        FieldName = "RelatedLoan.DateStarted",
        GroupName = "LoanInfo",
        Condition = this.cmbBoxStartDate.Text,
        Value1 = this.dateTimePickerStartDate1.Text.Trim(),
        Value2 = this.dateTimePickerStartDate2.Text.Trim(),
        ValueType = "System.DateTime"
      });
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

    private void cmbBoxSeachLoans_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (BorContactLoanSearchEnumUtil.NameToValue(this.cmbBoxSeachLoans.Text))
      {
        case BorContactLoanSearchEnum.AnyOriginated:
        case BorContactLoanSearchEnum.LastOriginated:
          this.groupBox1.Enabled = true;
          this.cmbBoxStartDate.Enabled = true;
          this.dateTimePickerStartDate1.Enabled = true;
          this.dateTimePickerStartDate2.Enabled = true;
          this.cmbBoxClosedDate.Enabled = false;
          this.cmbBoxClosedDate.SelectedIndex = 0;
          this.dateTimePickerClosedDate1.Enabled = false;
          this.dateTimePickerClosedDate1.CustomFormat = "' '";
          this.dateTimePickerClosedDate1.Value = DateTime.Now;
          this.dateTimePickerClosedDate2.Enabled = false;
          this.dateTimePickerClosedDate2.CustomFormat = "' '";
          this.dateTimePickerClosedDate2.Value = DateTime.Now;
          this.cmbBoxLoanType.Enabled = false;
          this.cmbBoxLoanType.SelectedIndex = 0;
          this.cmbBoxLien.Enabled = false;
          this.cmbBoxLien.SelectedIndex = 0;
          break;
        case BorContactLoanSearchEnum.AnyCompleted:
        case BorContactLoanSearchEnum.LastCompleted:
          this.groupBox1.Enabled = true;
          this.cmbBoxStartDate.Enabled = false;
          this.cmbBoxStartDate.SelectedIndex = 0;
          this.dateTimePickerStartDate1.Enabled = false;
          this.dateTimePickerStartDate1.CustomFormat = "' '";
          this.dateTimePickerStartDate1.Value = DateTime.Now;
          this.dateTimePickerStartDate2.Enabled = false;
          this.dateTimePickerStartDate2.CustomFormat = "' '";
          this.dateTimePickerStartDate2.Value = DateTime.Now;
          this.cmbBoxClosedDate.Enabled = true;
          this.dateTimePickerClosedDate1.Enabled = true;
          this.dateTimePickerClosedDate2.Enabled = true;
          this.cmbBoxLoanType.Enabled = true;
          this.cmbBoxLien.Enabled = true;
          break;
        default:
          this.groupBox1.Enabled = false;
          break;
      }
    }

    private void cmbBoxStartDate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxStartDate.Text == "Between")
      {
        this.lblAndStartDate.Visible = true;
        this.dateTimePickerStartDate2.Enabled = true;
        this.dateTimePickerStartDate2.Visible = true;
      }
      else
      {
        this.lblAndStartDate.Visible = false;
        this.dateTimePickerStartDate2.Enabled = false;
        this.dateTimePickerStartDate2.Visible = false;
      }
    }

    private void dateTimePickerStartDate1_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerStartDate1.CustomFormat = "MM'/'dd'/'yyyy";
    }

    private void dateTimePickerStartDate2_CloseUp(object sender, EventArgs e)
    {
      this.dateTimePickerStartDate2.CustomFormat = "MM'/'dd'/'yyyy";
    }
  }
}
