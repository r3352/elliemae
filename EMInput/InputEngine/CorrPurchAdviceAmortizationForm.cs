// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CorrPurchAdviceAmortizationForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CorrPurchAdviceAmortizationForm : CustomUserControl, IRefreshContents
  {
    private ToolTip fieldToolTip;
    private Label label3;
    private IContainer components;
    private LoanData loan;
    private TextBox tbOrigonalLoanAmount;
    private GroupContainer groupContainer1;
    private TableContainer tableContainer1;
    private GroupContainer groupContainer3;
    private GroupContainer groupContainer2;
    private Label label9;
    private TextBox tbPrincipalReduction;
    private Label label8;
    private Label label7;
    private DatePickerCustom dpNoteDate;
    private Label label6;
    private TextBox tbLoanPeriodMonths;
    private Label label2;
    private TextBox tbAnnualInterestRate;
    private Label label11;
    private TextBox tbFirstPaymentDueInvestor;
    private Label label10;
    private DatePickerCustom dpAnticipatedPurchaseDate;
    private DatePickerCustom dpFirstPaymentDueDate;
    private Label label12;
    private TextBox tbCalculatedPurchasePricipal;
    private GridView gvAmortization;
    private GroupContainer groupContainer4;
    private StandardIconButton btnNew;
    private DatePickerCustom dpAdditlPaymentDate;
    private StandardIconButton btnDelete;
    private GridView gvExtraPayments;
    private TextBox tbAdditlPaymentAmmount;
    private PopupBusinessRules popupRules;
    private Hashtable companySettings = new Hashtable();
    private int NumMonths = 24;
    private int CutoffDay = 14;
    private bool Show1stPaymentDuefromBorrowerWarning = true;

    public CorrPurchAdviceAmortizationForm(LoanData loan)
    {
      this.loan = loan;
      this.companySettings = Session.SessionObjects.GetCompanySettingsFromCache("POLICIES");
      this.NumMonths = Utils.ParseInt(this.companySettings[(object) "NumberOfMonths"], 24);
      this.CutoffDay = Utils.ParseInt(this.companySettings[(object) "CutoffCalendarDay"], 14);
      this.InitializeComponent();
      this.tbOrigonalLoanAmount.Tag = (object) "2";
      this.tbAnnualInterestRate.Tag = (object) "3";
      this.tbLoanPeriodMonths.Tag = (object) "4";
      this.dpNoteDate.Tag = (object) "CPA.PaymentHistory.NoteDate";
      this.dpFirstPaymentDueDate.Tag = (object) "CPA.PaymentHistory.FirstBorrowerPaymentDueDate";
      this.tbPrincipalReduction.Tag = (object) "CPA.PaymentHistory.PricipalReduction";
      this.dpAnticipatedPurchaseDate.Tag = (object) "CPA.PaymentHistory.AnticipatedPurchaseDate";
      this.tbFirstPaymentDueInvestor.Tag = (object) "CPA.PaymentHistory.FirstInvestorPaymentDate";
      this.tbCalculatedPurchasePricipal.Tag = (object) "CPA.PaymentHistory.CalculatedPurchasedPrincipal";
      this.initForm();
      ResourceManager resources = new ResourceManager(typeof (CorrPurchAdviceAmortizationForm));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      this.popupRules.SetBusinessRules((object) this.tbOrigonalLoanAmount, "VEND.X302");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      this.fieldToolTip = new ToolTip(this.components);
      this.label3 = new Label();
      this.tbOrigonalLoanAmount = new TextBox();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer3 = new GroupContainer();
      this.groupContainer4 = new GroupContainer();
      this.btnNew = new StandardIconButton();
      this.tbAdditlPaymentAmmount = new TextBox();
      this.dpAdditlPaymentDate = new DatePickerCustom();
      this.btnDelete = new StandardIconButton();
      this.gvExtraPayments = new GridView();
      this.groupContainer2 = new GroupContainer();
      this.dpAnticipatedPurchaseDate = new DatePickerCustom();
      this.dpFirstPaymentDueDate = new DatePickerCustom();
      this.label12 = new Label();
      this.label11 = new Label();
      this.tbCalculatedPurchasePricipal = new TextBox();
      this.label9 = new Label();
      this.tbFirstPaymentDueInvestor = new TextBox();
      this.tbPrincipalReduction = new TextBox();
      this.label10 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.dpNoteDate = new DatePickerCustom();
      this.label6 = new Label();
      this.tbLoanPeriodMonths = new TextBox();
      this.label2 = new Label();
      this.tbAnnualInterestRate = new TextBox();
      this.tableContainer1 = new TableContainer();
      this.gvAmortization = new GridView();
      this.groupContainer1.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.tableContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(4, 33);
      this.label3.Name = "label3";
      this.label3.Size = new Size(109, 14);
      this.label3.TabIndex = 5;
      this.label3.Text = "Original Loan Amount";
      this.tbOrigonalLoanAmount.Enabled = false;
      this.tbOrigonalLoanAmount.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tbOrigonalLoanAmount.Location = new Point(211, 30);
      this.tbOrigonalLoanAmount.Name = "tbOrigonalLoanAmount";
      this.tbOrigonalLoanAmount.Size = new Size(105, 20);
      this.tbOrigonalLoanAmount.TabIndex = 10;
      this.tbOrigonalLoanAmount.TabStop = false;
      this.tbOrigonalLoanAmount.Enter += new EventHandler(this.enterField);
      this.tbOrigonalLoanAmount.Leave += new EventHandler(this.leaveField);
      this.groupContainer1.BackColor = Color.Gainsboro;
      this.groupContainer1.Controls.Add((Control) this.groupContainer3);
      this.groupContainer1.Controls.Add((Control) this.groupContainer2);
      this.groupContainer1.Controls.Add((Control) this.tableContainer1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(686, 684);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Payment History Calculator";
      this.groupContainer3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer3.Controls.Add((Control) this.groupContainer4);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(331, 29);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(350, 293);
      this.groupContainer3.TabIndex = 72;
      this.groupContainer3.Text = "Extra Principal Payments";
      this.groupContainer4.Controls.Add((Control) this.btnNew);
      this.groupContainer4.Controls.Add((Control) this.tbAdditlPaymentAmmount);
      this.groupContainer4.Controls.Add((Control) this.dpAdditlPaymentDate);
      this.groupContainer4.Controls.Add((Control) this.btnDelete);
      this.groupContainer4.Controls.Add((Control) this.gvExtraPayments);
      this.groupContainer4.Dock = DockStyle.Fill;
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(1, 26);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(348, 266);
      this.groupContainer4.TabIndex = 24;
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(231, 5);
      this.btnNew.Margin = new Padding(2, 3, 3, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 23;
      this.btnNew.TabStop = false;
      this.btnNew.Click += new EventHandler(this.btnNewAddnlPayment_Click);
      this.tbAdditlPaymentAmmount.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tbAdditlPaymentAmmount.Location = new Point(114, 2);
      this.tbAdditlPaymentAmmount.Name = "tbAdditlPaymentAmmount";
      this.tbAdditlPaymentAmmount.Size = new Size(109, 20);
      this.tbAdditlPaymentAmmount.TabIndex = 6;
      this.tbAdditlPaymentAmmount.TextChanged += new EventHandler(this.dpAdditlPayment_ValueChanged);
      this.tbAdditlPaymentAmmount.KeyUp += new KeyEventHandler(this.currencyField_Keyup);
      this.tbAdditlPaymentAmmount.Leave += new EventHandler(this.leaveCurrencyField);
      this.dpAdditlPaymentDate.Location = new Point(6, 2);
      this.dpAdditlPaymentDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAdditlPaymentDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAdditlPaymentDate.Name = "dpAdditlPaymentDate";
      this.dpAdditlPaymentDate.Size = new Size(102, 21);
      this.dpAdditlPaymentDate.TabIndex = 5;
      this.dpAdditlPaymentDate.ToolTip = "";
      this.dpAdditlPaymentDate.Value = new DateTime(0L);
      this.dpAdditlPaymentDate.ValueChanged += new EventHandler(this.dpAdditlPayment_ValueChanged);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(253, 5);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 22;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDeleteAddnlPayment_Click);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ClmPaymentDate";
      gvColumn1.SortMethod = GVSortMethod.Date;
      gvColumn1.Text = "Payment Date";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 110;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ClmAdditlPrincipal";
      gvColumn2.Text = "Additional Principal";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 110;
      this.gvExtraPayments.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvExtraPayments.Dock = DockStyle.Fill;
      this.gvExtraPayments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvExtraPayments.Location = new Point(1, 26);
      this.gvExtraPayments.Name = "gvExtraPayments";
      this.gvExtraPayments.Size = new Size(346, 239);
      this.gvExtraPayments.TabIndex = 7;
      this.gvExtraPayments.SelectedIndexChanged += new EventHandler(this.gvExtraPayments_SelectedIndexChanged);
      this.groupContainer2.Controls.Add((Control) this.dpAnticipatedPurchaseDate);
      this.groupContainer2.Controls.Add((Control) this.dpFirstPaymentDueDate);
      this.groupContainer2.Controls.Add((Control) this.label12);
      this.groupContainer2.Controls.Add((Control) this.label11);
      this.groupContainer2.Controls.Add((Control) this.tbCalculatedPurchasePricipal);
      this.groupContainer2.Controls.Add((Control) this.label9);
      this.groupContainer2.Controls.Add((Control) this.tbFirstPaymentDueInvestor);
      this.groupContainer2.Controls.Add((Control) this.tbPrincipalReduction);
      this.groupContainer2.Controls.Add((Control) this.label10);
      this.groupContainer2.Controls.Add((Control) this.label8);
      this.groupContainer2.Controls.Add((Control) this.label7);
      this.groupContainer2.Controls.Add((Control) this.dpNoteDate);
      this.groupContainer2.Controls.Add((Control) this.label6);
      this.groupContainer2.Controls.Add((Control) this.tbLoanPeriodMonths);
      this.groupContainer2.Controls.Add((Control) this.label2);
      this.groupContainer2.Controls.Add((Control) this.tbAnnualInterestRate);
      this.groupContainer2.Controls.Add((Control) this.label3);
      this.groupContainer2.Controls.Add((Control) this.tbOrigonalLoanAmount);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(5, 29);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(320, 293);
      this.groupContainer2.TabIndex = 0;
      this.groupContainer2.Text = "Principal Balance Calculator";
      this.dpAnticipatedPurchaseDate.Location = new Point(211, 190);
      this.dpAnticipatedPurchaseDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpAnticipatedPurchaseDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpAnticipatedPurchaseDate.Name = "dpAnticipatedPurchaseDate";
      this.dpAnticipatedPurchaseDate.Size = new Size(105, 21);
      this.dpAnticipatedPurchaseDate.TabIndex = 4;
      this.dpAnticipatedPurchaseDate.ToolTip = "";
      this.dpAnticipatedPurchaseDate.Value = new DateTime(0L);
      this.dpAnticipatedPurchaseDate.ValueChanged += new EventHandler(this.dtDate_ValueChanged);
      this.dpAnticipatedPurchaseDate.Leave += new EventHandler(this.dateField_Leave);
      this.dpFirstPaymentDueDate.Location = new Point(211, 126);
      this.dpFirstPaymentDueDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpFirstPaymentDueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpFirstPaymentDueDate.Name = "dpFirstPaymentDueDate";
      this.dpFirstPaymentDueDate.Size = new Size(105, 21);
      this.dpFirstPaymentDueDate.TabIndex = 2;
      this.dpFirstPaymentDueDate.ToolTip = "";
      this.dpFirstPaymentDueDate.Value = new DateTime(0L);
      this.dpFirstPaymentDueDate.ValueChanged += new EventHandler(this.dtDate_ValueChanged);
      this.dpFirstPaymentDueDate.Leave += new EventHandler(this.dateField_Leave);
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(4, 258);
      this.label12.Name = "label12";
      this.label12.Size = new Size(176, 14);
      this.label12.TabIndex = 85;
      this.label12.Text = "Calculated Purchased Principal";
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(4, 218);
      this.label11.Name = "label11";
      this.label11.Size = new Size(142, 14);
      this.label11.TabIndex = 75;
      this.label11.Text = "1st Payment Due to Investor";
      this.tbCalculatedPurchasePricipal.Enabled = false;
      this.tbCalculatedPurchasePricipal.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tbCalculatedPurchasePricipal.Location = new Point(211, (int) byte.MaxValue);
      this.tbCalculatedPurchasePricipal.Name = "tbCalculatedPurchasePricipal";
      this.tbCalculatedPurchasePricipal.Size = new Size(105, 20);
      this.tbCalculatedPurchasePricipal.TabIndex = 90;
      this.tbCalculatedPurchasePricipal.TabStop = false;
      this.tbCalculatedPurchasePricipal.Enter += new EventHandler(this.enterField);
      this.tbCalculatedPurchasePricipal.Leave += new EventHandler(this.leaveField);
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(4, 153);
      this.label9.Name = "label9";
      this.label9.Size = new Size(201, 14);
      this.label9.TabIndex = 55;
      this.label9.Text = "Reduction prior to 1st Payment Due Date";
      this.tbFirstPaymentDueInvestor.Enabled = false;
      this.tbFirstPaymentDueInvestor.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tbFirstPaymentDueInvestor.Location = new Point(211, 215);
      this.tbFirstPaymentDueInvestor.Name = "tbFirstPaymentDueInvestor";
      this.tbFirstPaymentDueInvestor.Size = new Size(105, 20);
      this.tbFirstPaymentDueInvestor.TabIndex = 80;
      this.tbFirstPaymentDueInvestor.TabStop = false;
      this.tbFirstPaymentDueInvestor.TextChanged += new EventHandler(this.leaveField);
      this.tbFirstPaymentDueInvestor.Enter += new EventHandler(this.enterField);
      this.tbFirstPaymentDueInvestor.Leave += new EventHandler(this.leaveField);
      this.tbPrincipalReduction.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tbPrincipalReduction.Location = new Point(211, 150);
      this.tbPrincipalReduction.Name = "tbPrincipalReduction";
      this.tbPrincipalReduction.Size = new Size(105, 20);
      this.tbPrincipalReduction.TabIndex = 3;
      this.tbPrincipalReduction.Enter += new EventHandler(this.enterField);
      this.tbPrincipalReduction.KeyUp += new KeyEventHandler(this.currencyField_Keyup);
      this.tbPrincipalReduction.Leave += new EventHandler(this.leaveField);
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(4, 194);
      this.label10.Name = "label10";
      this.label10.Size = new Size(135, 14);
      this.label10.TabIndex = 65;
      this.label10.Text = "Anticipated Purchase Date";
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(4, 129);
      this.label8.Name = "label8";
      this.label8.Size = new Size(163, 14);
      this.label8.TabIndex = 45;
      this.label8.Text = "1st Payment Due from Borrower";
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(4, 105);
      this.label7.Name = "label7";
      this.label7.Size = new Size(54, 14);
      this.label7.TabIndex = 35;
      this.label7.Text = "Note Date";
      this.dpNoteDate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dpNoteDate.Location = new Point(211, 102);
      this.dpNoteDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpNoteDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpNoteDate.Name = "dpNoteDate";
      this.dpNoteDate.Size = new Size(105, 22);
      this.dpNoteDate.TabIndex = 1;
      this.dpNoteDate.ToolTip = "";
      this.dpNoteDate.Value = new DateTime(0L);
      this.dpNoteDate.ValueChanged += new EventHandler(this.dtDate_ValueChanged);
      this.dpNoteDate.Enter += new EventHandler(this.enterField);
      this.dpNoteDate.Leave += new EventHandler(this.dateField_Leave);
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(4, 81);
      this.label6.Name = "label6";
      this.label6.Size = new Size(113, 14);
      this.label6.TabIndex = 25;
      this.label6.Text = "Loan Period in Months";
      this.tbLoanPeriodMonths.Enabled = false;
      this.tbLoanPeriodMonths.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tbLoanPeriodMonths.Location = new Point(211, 78);
      this.tbLoanPeriodMonths.Name = "tbLoanPeriodMonths";
      this.tbLoanPeriodMonths.Size = new Size(105, 20);
      this.tbLoanPeriodMonths.TabIndex = 3;
      this.tbLoanPeriodMonths.TabStop = false;
      this.tbLoanPeriodMonths.Enter += new EventHandler(this.enterField);
      this.tbLoanPeriodMonths.Leave += new EventHandler(this.leaveField);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(4, 57);
      this.label2.Name = "label2";
      this.label2.Size = new Size(105, 14);
      this.label2.TabIndex = 15;
      this.label2.Text = "Annual Interest Rate";
      this.tbAnnualInterestRate.Enabled = false;
      this.tbAnnualInterestRate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tbAnnualInterestRate.Location = new Point(211, 54);
      this.tbAnnualInterestRate.Name = "tbAnnualInterestRate";
      this.tbAnnualInterestRate.Size = new Size(105, 20);
      this.tbAnnualInterestRate.TabIndex = 20;
      this.tbAnnualInterestRate.TabStop = false;
      this.tbAnnualInterestRate.Enter += new EventHandler(this.enterField);
      this.tbAnnualInterestRate.Leave += new EventHandler(this.leaveField);
      this.tableContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableContainer1.Controls.Add((Control) this.gvAmortization);
      this.tableContainer1.Location = new Point(4, 328);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(678, 352);
      this.tableContainer1.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.tableContainer1.TabIndex = 71;
      this.tableContainer1.Text = "Payment Amortization Table";
      this.gvAmortization.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "clmPaymentNumber";
      gvColumn3.SortMethod = GVSortMethod.None;
      gvColumn3.Text = "No.";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 28;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "clmPaymentDate";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "Payment Date";
      gvColumn4.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "clmBeginningBalance";
      gvColumn5.SortMethod = GVSortMethod.None;
      gvColumn5.Text = "Beg Balance";
      gvColumn5.Width = 88;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "clmScheduledPayment";
      gvColumn6.SortMethod = GVSortMethod.None;
      gvColumn6.Text = "Sched Payment";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 88;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "clmExtraPayment";
      gvColumn7.Text = "Extra Principal";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 81;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "clmTotalPayment";
      gvColumn8.Text = "Total Payment";
      gvColumn8.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn8.Width = 82;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "clmPrincipal";
      gvColumn9.Text = "Principal";
      gvColumn9.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn9.Width = 60;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "clmInterest";
      gvColumn10.Text = "Interest";
      gvColumn10.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn10.Width = 60;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "clmUnpaidBalance";
      gvColumn11.Text = "UPB Balance";
      gvColumn11.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn11.Width = 90;
      this.gvAmortization.Columns.AddRange(new GVColumn[9]
      {
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11
      });
      this.gvAmortization.Dock = DockStyle.Fill;
      this.gvAmortization.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gvAmortization.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAmortization.Location = new Point(1, 26);
      this.gvAmortization.Name = "gvAmortization";
      this.gvAmortization.Size = new Size(676, 325);
      this.gvAmortization.SortOption = GVSortOption.None;
      this.gvAmortization.TabIndex = 0;
      this.gvAmortization.TabStop = false;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (CorrPurchAdviceAmortizationForm);
      this.Size = new Size(686, 684);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.tableContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private string FormatCurrencyString(Decimal value) => value.ToString("c").TrimStart('$');

    private void AddRowToAmortization(
      string PaymentNo,
      string PaymentDate,
      string BegBalance,
      string SchedPayment,
      string ExtraPayment,
      string TotalPayment,
      string Pricipal,
      string Interest,
      string UPDBalance)
    {
      GVItem gvItem = new GVItem(PaymentNo);
      double result1;
      if (!double.TryParse(ExtraPayment, out result1))
        result1 = 0.0;
      Decimal result2;
      if (!Decimal.TryParse(BegBalance, out result2))
        result2 = 0M;
      gvItem.SubItems.Add((object) PaymentDate);
      gvItem.SubItems.Add((object) BegBalance);
      gvItem.SubItems.Add((object) SchedPayment);
      gvItem.SubItems.Add(result1 == 0.0 ? (object) string.Empty : (object) result1.ToString("N2"));
      gvItem.SubItems.Add((object) TotalPayment);
      gvItem.SubItems.Add((object) Pricipal);
      gvItem.SubItems.Add((object) Interest);
      gvItem.SubItems.Add((object) UPDBalance);
      gvItem.Tag = (object) new Tuple<DateTime, Decimal>(Utils.ParseDate((object) PaymentDate), result2);
      this.gvAmortization.Items.Add(gvItem);
    }

    private void AddRowToExtraPayments(DateTime PaymentDate, Decimal ExtraPrincipal)
    {
      GVItem gvItem = new GVItem(PaymentDate.ToShortDateString());
      gvItem.SubItems.Add((object) this.FormatCurrencyString(ExtraPrincipal));
      gvItem.Tag = (object) new Tuple<DateTime, Decimal>(PaymentDate, ExtraPrincipal);
      if (this.gvExtraPayments.Items.Count == 0)
      {
        this.gvExtraPayments.Items.Add(gvItem);
      }
      else
      {
        for (int index = 0; index < this.gvExtraPayments.Items.Count; ++index)
        {
          if (((Tuple<DateTime, Decimal>) this.gvExtraPayments.Items[index].Tag).Item1 > PaymentDate)
          {
            this.gvExtraPayments.Items.Insert(index, gvItem);
            return;
          }
        }
        this.gvExtraPayments.Items.Add(gvItem);
      }
    }

    private void SaveExtraPaymentFields()
    {
      for (int nItemIndex = 0; nItemIndex < 11; ++nItemIndex)
      {
        string id1 = string.Format("CPA.PaymentHistory.ExtraPayment.Date.{0:D2}", (object) (nItemIndex + 1));
        string id2 = string.Format("CPA.PaymentHistory.ExtraPayment.Amount.{0:D2}", (object) (nItemIndex + 1));
        string val1 = string.Empty;
        string val2 = string.Empty;
        if (nItemIndex < this.gvExtraPayments.Items.Count)
        {
          Tuple<DateTime, Decimal> tag = (Tuple<DateTime, Decimal>) this.gvExtraPayments.Items[nItemIndex].Tag;
          val1 = tag.Item1.ToShortDateString();
          val2 = tag.Item2.ToString("C2").TrimStart('$');
        }
        this.loan.SetField(id1, val1);
        this.loan.SetField(id2, val2);
      }
    }

    private void LoadExtraPaymentFields()
    {
      this.gvExtraPayments.Items.Clear();
      this.gvExtraPayments.BeginUpdate();
      for (int index = 0; index < 11; ++index)
      {
        string id1 = string.Format("CPA.PaymentHistory.ExtraPayment.Date.{0:D2}", (object) (index + 1));
        string id2 = string.Format("CPA.PaymentHistory.ExtraPayment.Amount.{0:D2}", (object) (index + 1));
        string field1 = this.loan.GetField(id1);
        string field2 = this.loan.GetField(id2);
        if (!string.IsNullOrEmpty(field1) && field1 != "//" && !string.IsNullOrEmpty(field2))
        {
          DateTime result1 = DateTime.MinValue;
          DateTime.TryParse(field1, out result1);
          Decimal result2 = -1M;
          Decimal.TryParse(field2, out result2);
          if (result1 != DateTime.MinValue && result2 > 0M)
            this.AddRowToExtraPayments(result1, result2);
        }
      }
      this.gvExtraPayments.EndUpdate();
    }

    private void calculateFirstInvestorPaymentDate()
    {
      DateTime dateTime1 = this.dpAnticipatedPurchaseDate.Value.AddMonths(this.dpAnticipatedPurchaseDate.Value.Day < this.CutoffDay ? 1 : 2);
      DateTime dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, 1);
      if (dateTime2 < this.dpFirstPaymentDueDate.Value)
        dateTime2 = this.dpFirstPaymentDueDate.Value;
      this.tbFirstPaymentDueInvestor.Text = dateTime2.ToString("MM/dd/yyyy");
      this.loan.SetField("CPA.PaymentHistory.FirstInvestorPaymentDate", this.tbFirstPaymentDueInvestor.Text);
    }

    private void initForm()
    {
      if (!Session.SessionObjects.GetPolicySetting("EnablePaymentHistoryAndCalc"))
        return;
      this.tbOrigonalLoanAmount.Text = this.loan.GetField("2");
      this.tbAnnualInterestRate.Text = this.loan.GetField("3");
      this.tbLoanPeriodMonths.Text = this.loan.GetField("4");
      this.LoadExtraPaymentFields();
      this.CalculateAmortization();
      this.RefreshFieldData();
    }

    private void enterField(object sender, EventArgs e)
    {
      string id = string.Empty;
      switch (sender)
      {
        case TextBox _:
          id = (string) ((Control) sender).Tag;
          break;
        case DatePicker _:
          id = (string) ((Control) sender).Tag;
          break;
      }
      Session.Application.GetService<IStatusDisplay>()?.DisplayFieldID(id);
    }

    private void leaveField(object sender, EventArgs e)
    {
      if (!(sender is TextBox))
        return;
      TextBox ctrl = (TextBox) sender;
      if (ctrl == null)
        return;
      string tag = (string) ctrl.Tag;
      if ((tag ?? "") == "" || this.loan.IsFieldReadOnly(tag))
        return;
      if (this.popupRules == null || this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag))
        this.loan.SetField(tag, ctrl.Text);
      if (tag == "CPA.PaymentHistory.PricipalReduction")
        this.leaveCurrencyField(sender, e);
      if (tag == "CPA.PaymentHistory.PricipalReduction" || tag == "CPA.PaymentHistory.FirstInvestorPaymentDate")
        this.CalculateAmortization();
      this.updateBusinessRule();
      this.RefreshFieldData();
    }

    private void dateField_Leave(object sender, EventArgs e)
    {
      if (!(sender is DatePicker datePicker))
        return;
      string tag = (string) datePicker.Tag;
      if (string.IsNullOrWhiteSpace(tag))
        return;
      if (datePicker.Text == "//" || datePicker.Text == "")
        this.loan.SetField(tag, "");
      else
        this.loan.SetField(tag, datePicker.Text);
      if (tag == "CPA.PaymentHistory.NoteDate" || tag == "CPA.PaymentHistory.FirstBorrowerPaymentDueDate" || tag == "CPA.PaymentHistory.AnticipatedPurchaseDate")
      {
        if (!this.ValidateAnticipatedPurchaseDate())
          this.dpAnticipatedPurchaseDate.Text = "//";
        this.validateFirstPaymentDate();
        this.CalculateAmortization();
      }
      this.updateBusinessRule();
      this.RefreshFieldData();
    }

    private void validateFirstPaymentDate()
    {
      DateTime minValue = DateTime.MinValue;
      if (this.dpFirstPaymentDueDate.Value.Date.Day == 1 || !this.Show1stPaymentDuefromBorrowerWarning)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "1st Payment Due from Borrower may only be specified as the 1st of the month. This must be corrected before the Calculated Purchased Principal can be determined", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.Show1stPaymentDuefromBorrowerWarning = false;
    }

    private void RefreshFieldData()
    {
      this.dpNoteDate.Text = this.loan.GetField("CPA.PaymentHistory.NoteDate");
      this.dpFirstPaymentDueDate.Text = this.loan.GetField("CPA.PaymentHistory.FirstBorrowerPaymentDueDate");
      this.tbPrincipalReduction.Text = this.loan.GetField("CPA.PaymentHistory.PricipalReduction");
      this.dpAnticipatedPurchaseDate.Text = this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate");
      this.tbFirstPaymentDueInvestor.Text = this.loan.GetField("CPA.PaymentHistory.FirstInvestorPaymentDate");
      this.tbCalculatedPurchasePricipal.Text = this.loan.GetField("CPA.PaymentHistory.CalculatedPurchasedPrincipal");
    }

    public void RefreshContents() => this.initForm();

    public void RefreshLoanContents() => this.RefreshContents();

    private void dtDate_ValueChanged(object sender, EventArgs e) => this.dateField_Leave(sender, e);

    private void updateBusinessRule()
    {
      try
      {
        if (this.loan == null || this.loan.IsTemplate)
          return;
        Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
      }
    }

    private void currencyField_Keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private bool ValidateAnticipatedPurchaseDate()
    {
      bool flag = true;
      if ((this.dpNoteDate.Value == DateTime.MinValue || this.dpFirstPaymentDueDate.Value == DateTime.MinValue) && this.dpAnticipatedPurchaseDate.Value != DateTime.MinValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Note Date and 1st Payment Due from Borrower must be entered before the Anticipated Purchase Date is entered.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        flag = false;
      }
      if (this.dpAnticipatedPurchaseDate.Value != DateTime.MinValue)
      {
        if (this.dpAnticipatedPurchaseDate.Value < this.dpNoteDate.Value)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Anticipated Purchase Date cannot be prior to Note Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          flag = false;
        }
        if (this.dpAnticipatedPurchaseDate.Value > DateTime.Today.AddMonths(1))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Anticipated Purchase Date cannot be more than 1 month in the future.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          flag = false;
        }
      }
      else
        flag = false;
      if (flag)
        return true;
      this.gvAmortization.Items.Clear();
      return false;
    }

    private void leaveCurrencyField(object sender, EventArgs e)
    {
      TextBox textBox = sender as TextBox;
      if (sender == null)
        return;
      Decimal result = 0M;
      if (string.IsNullOrEmpty(textBox.Text) || !Decimal.TryParse(textBox.Text, out result) || !(result > 0M))
        return;
      string str = result.ToString("C2").TrimStart('$');
      if (!(str != textBox.Text))
        return;
      textBox.Text = str;
    }

    private void dpAdditlPayment_ValueChanged(object sender, EventArgs e)
    {
      Decimal result = -1M;
      this.btnNew.Enabled = !string.IsNullOrEmpty(this.dpAdditlPaymentDate.Text) && this.dpAdditlPaymentDate.Text != "//" && this.dpAdditlPaymentDate.Value != DateTime.MaxValue && this.dpAdditlPaymentDate.Value != DateTime.MinValue && Decimal.TryParse(this.tbAdditlPaymentAmmount.Text, out result) && this.gvExtraPayments.Items.Count < 11;
    }

    private void btnNewAddnlPayment_Click(object sender, EventArgs e)
    {
      DateTime PaymentDate = this.dpAdditlPaymentDate.Value;
      if (PaymentDate < this.dpFirstPaymentDueDate.Value || PaymentDate >= this.dpFirstPaymentDueDate.Value.AddMonths(this.NumMonths))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Payment Date is outside of the Payment Amortization Table range. Entry not added.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.dpAdditlPaymentDate.Text = string.Empty;
        this.tbAdditlPaymentAmmount.Text = string.Empty;
      }
      else
      {
        DateTime result1 = DateTime.MinValue;
        if (DateTime.TryParse(this.tbFirstPaymentDueInvestor.Text, out result1) && PaymentDate >= result1)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Payment Date is outside of the Seller payment date range. Entry not added.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.dpAdditlPaymentDate.Text = string.Empty;
          this.tbAdditlPaymentAmmount.Text = string.Empty;
        }
        else
        {
          Decimal result2 = -1M;
          Decimal.TryParse(this.tbAdditlPaymentAmmount.Text, out result2);
          this.AddRowToExtraPayments(PaymentDate, result2);
          this.SaveExtraPaymentFields();
          this.dpAdditlPaymentDate.Text = string.Empty;
          this.tbAdditlPaymentAmmount.Text = string.Empty;
          this.CalculateAmortization();
        }
      }
    }

    private void btnDeleteAddnlPayment_Click(object sender, EventArgs e)
    {
      if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "This action will remove Extra Principal Payments from the Grid. Do you wish to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
      {
        this.gvExtraPayments.SelectedItems.Clear();
      }
      else
      {
        foreach (GVItem selectedItem in this.gvExtraPayments.SelectedItems)
          this.gvExtraPayments.Items.Remove(selectedItem);
        this.SaveExtraPaymentFields();
        this.CalculateAmortization();
      }
    }

    private void gvExtraPayments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDelete.Enabled = this.gvExtraPayments.SelectedItems.Count > 0;
    }

    private Decimal CalculateMonthlyPayment(
      Decimal Principal,
      Decimal monthlyInterst,
      int NumPayments)
    {
      Decimal num = (Decimal) Math.Pow((double) (1.0M + monthlyInterst), (double) NumPayments);
      return Math.Round(monthlyInterst * Principal * num / (num - 1M), 2);
    }

    private Decimal GetExtraPaymentsForPeriod(DateTime start, DateTime end)
    {
      Decimal paymentsForPeriod = 0M;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvExtraPayments.Items)
      {
        if (gvItem.Tag is Tuple<DateTime, Decimal> tag && tag.Item1 >= start && tag.Item1 < end)
          paymentsForPeriod += tag.Item2;
      }
      return paymentsForPeriod;
    }

    private void addScheduletoUI()
    {
      List<string[]> correspondentPaymentSchedule = this.loan.Calculator.GetCorrespondentPaymentSchedule();
      if (correspondentPaymentSchedule == null)
        return;
      foreach (string[] strArray in correspondentPaymentSchedule)
        this.AddRowToAmortization(strArray[0], strArray[1], strArray[2], strArray[3], strArray[4], strArray[5], strArray[6], strArray[7], strArray[8]);
    }

    private void CalculateAmortization()
    {
      this.gvAmortization.Items.Clear();
      this.addScheduletoUI();
    }
  }
}
