// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BorrowerVestingForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class BorrowerVestingForm : CustomUserControl, IRefreshContents
  {
    private System.Windows.Forms.TextBox txtTrust1CorpName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private ComboBox cboTrust1State;
    private ComboBox cboTrust1Type;
    private System.Windows.Forms.TextBox txtTrust1Tax;
    private System.Windows.Forms.TextBox txtFinalVesting;
    private ComboBox cboTitle;
    private System.Windows.Forms.TextBox txtSelTax;
    private ComboBox cboSelType;
    private ComboBox cboSelState;
    private System.Windows.Forms.TextBox txtSelCorpName;
    private IContainer components;
    private System.Windows.Forms.Button btnFinal;
    private ToolTip fieldToolTip;
    private LoanData loan;
    private IMainScreen mainScreen;
    public static string[] VestingList;
    private static string[] orgType = new string[13]
    {
      "A Corporation",
      "A General Partnership",
      "A Sole Proprietorship",
      "A Limited Partnership",
      "A Partnership",
      "A Federal Savings Association",
      "A Federal Savings Bank",
      "A Federal Bank",
      "A Federal Credit Union",
      "A National Association",
      "A National Bank",
      "A National Banking Association",
      "A Limited Liability Company"
    };
    private WinFormInputHandler inputHandler;
    private System.Windows.Forms.TextBox txtTrust1Date;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtTrust1Beneficiary;
    private System.Windows.Forms.Button btnTrust1Beneficiary;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private GroupContainer groupContainer3;
    private GridView gvVesting;
    private StandardIconButton btnEdit;
    private System.Windows.Forms.TextBox txtTrust1AmendedYear;
    private System.Windows.Forms.Label label6;
    private GroupContainer groupContainer4;
    private System.Windows.Forms.TextBox txtTrust2AmendedYear;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox txtTrust2Beneficiary;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Button btnTrust2Beneficiary;
    private ComboBox cboTrust2State;
    private System.Windows.Forms.TextBox txtTrust2Date;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.TextBox txtTrust2Tax;
    private System.Windows.Forms.TextBox txtTrust2CorpName;
    private System.Windows.Forms.Label label18;
    private ComboBox cboTrust2Type;
    private System.Windows.Forms.TextBox txtSeller4;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.TextBox txtSeller3;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.TextBox txtSeller1;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.TextBox txtSeller2;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnAdd;
    private IconButton btnNboVideo;
    private StandardIconButton btnDelete;
    private System.Windows.Forms.TextBox txtSelOfcr2Title;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.TextBox txtSelOfcr2Name;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.TextBox txtSelOfcr1Title;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.TextBox txtSelOfcr1Name;
    private System.Windows.Forms.Label label23;
    private CalendarButton calendarButton1;
    private CalendarButton calendarButton3;
    private CalendarButton calendarButton4;
    private CalendarButton calendarButton2;
    private System.Windows.Forms.CheckBox prtTrustName;
    private System.Windows.Forms.Label prtTrustAddendum;
    private GroupContainer trustInformation;
    private PiggybackSynchronization piggySyncTool;

    static BorrowerVestingForm()
    {
      BorrowerVestingForm.VestingList = new string[50]
      {
        "",
        "A MARRIED MAN",
        "A MARRIED MAN AS HIS SOLE AND SEPARATE PROPERTY",
        "A MARRIED MAN AS TO AN UNDIVIDED 1/2 INTEREST",
        "A MARRIED WOMAN",
        "A MARRIED WOMAN AS HER SOLE AND SEPARATE PROPERTY",
        "A MARRIED WOMAN AS TO AN UNDIVIDED 1/2 INTEREST",
        "A SINGLE MAN",
        "A SINGLE MAN AS HIS SOLE AND SEPARATE PROPERTY",
        "A SINGLE MAN AS TO AN UNDIVIDED 1/2 INTEREST",
        "A SINGLE PERSON",
        "A SINGLE WOMAN",
        "A SINGLE WOMAN AS HER SOLE AND SEPARATE PROPERTY",
        "A SINGLE WOMAN AS TO AN UNDIVIDED 1/2 INTEREST",
        "AN INDIVIDUAL ADULT",
        "AN UNMARRIED MAN",
        "AN UNMARRIED MAN AS HIS SOLE AND SEPARATE PROPERTY",
        "AN UNMARRIED MAN AS TO AN UNDIVIDED 1/2 INTEREST",
        "AN UNMARRIED WOMAN",
        "AN UNMARRIED WOMAN AS HER SOLE AND SEPARATE PROPERTY",
        "AN UNMARRIED WOMAN AS TO AN UNDIVIDED 1/2 INTEREST",
        "A WIDOW",
        "AUTHORIZED MEMBER",
        "BOTH UNMARRIED",
        "CHIEF EXECUTIVE OFFICER",
        "DIVORCED AND NOT SINCE REMARRIED",
        "DOMESTIC PARTNERS",
        "GENERAL PARTNER",
        "HUSBAND AND WIFE",
        "HUSBAND AND WIFE AS COMMUNITY PROPERTY",
        "HUSBAND AND WIFE AS JOINT TENANTS",
        "HUSBAND AND WIFE AS JOINT TENANTS WITH RIGHT OF SURVIVORSHIP",
        "HUSBAND AND WIFE AS TENANTS IN COMMON",
        "HUSBAND AND WIFE AS TENANTS BY THE ENTIRETY",
        "HUSBAND AND WIFE AS TO AN UNDIVIDED 1/2 INTEREST",
        "MANAGER",
        "MANAGING MEMBER",
        "PERSONAL GUARANTOR",
        "PRESIDENT",
        "SECRETARY",
        "SENIOR VICE PRESIDENT",
        "SPOUSES MARRIED TO EACH OTHER",
        "TENANCY BY ENTIRETY",
        "TREASURER",
        "TRUSTEE",
        "VICE-PRESIDENT",
        "WIFE AND HUSBAND",
        "WIFE AND HUSBAND AS COMMUNITY PROPERTY",
        "WIFE AND HUSBAND AS JOINT TENANTS",
        "WIFE AND HUSBAND AS TENANTS IN COMMON"
      };
    }

    public BorrowerVestingForm(LoanData loan)
      : this(loan, (IMainScreen) null)
    {
    }

    public BorrowerVestingForm(LoanData loan, IMainScreen mainScreen)
    {
      this.mainScreen = mainScreen;
      this.loan = loan;
      this.piggySyncTool = new PiggybackSynchronization(this.loan);
      this.InitializeComponent();
      this.initStates();
      this.initFields();
      this.initTitle();
      this.loadBorrowerAndTrusteeList();
      BorrowerVestingForm.initStateType(this.cboTrust1Type, this.cboTrust1State.Text);
      BorrowerVestingForm.initStateType(this.cboTrust2Type, this.cboTrust2State.Text);
      BorrowerVestingForm.initStateType(this.cboSelType, this.cboSelState.Text);
      this.gvVesting_SelectedIndexChanged((object) null, (EventArgs) null);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BorrowerVestingForm));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.fieldToolTip = new ToolTip(this.components);
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.groupContainer4 = new GroupContainer();
      this.calendarButton3 = new CalendarButton();
      this.txtTrust2AmendedYear = new System.Windows.Forms.TextBox();
      this.calendarButton4 = new CalendarButton();
      this.txtTrust2Date = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.txtTrust2Beneficiary = new System.Windows.Forms.TextBox();
      this.label14 = new System.Windows.Forms.Label();
      this.btnTrust2Beneficiary = new System.Windows.Forms.Button();
      this.cboTrust2State = new ComboBox();
      this.label15 = new System.Windows.Forms.Label();
      this.label16 = new System.Windows.Forms.Label();
      this.label17 = new System.Windows.Forms.Label();
      this.txtTrust2Tax = new System.Windows.Forms.TextBox();
      this.txtTrust2CorpName = new System.Windows.Forms.TextBox();
      this.label18 = new System.Windows.Forms.Label();
      this.cboTrust2Type = new ComboBox();
      this.groupContainer2 = new GroupContainer();
      this.txtSelOfcr2Title = new System.Windows.Forms.TextBox();
      this.label25 = new System.Windows.Forms.Label();
      this.txtSelOfcr2Name = new System.Windows.Forms.TextBox();
      this.label26 = new System.Windows.Forms.Label();
      this.txtSelOfcr1Title = new System.Windows.Forms.TextBox();
      this.label24 = new System.Windows.Forms.Label();
      this.txtSelOfcr1Name = new System.Windows.Forms.TextBox();
      this.label23 = new System.Windows.Forms.Label();
      this.txtSeller4 = new System.Windows.Forms.TextBox();
      this.label22 = new System.Windows.Forms.Label();
      this.txtSeller3 = new System.Windows.Forms.TextBox();
      this.label21 = new System.Windows.Forms.Label();
      this.txtSeller2 = new System.Windows.Forms.TextBox();
      this.label20 = new System.Windows.Forms.Label();
      this.txtSeller1 = new System.Windows.Forms.TextBox();
      this.label19 = new System.Windows.Forms.Label();
      this.txtSelCorpName = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.cboSelType = new ComboBox();
      this.label10 = new System.Windows.Forms.Label();
      this.txtSelTax = new System.Windows.Forms.TextBox();
      this.cboSelState = new ComboBox();
      this.label12 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.groupContainer3 = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnNboVideo = new IconButton();
      this.gvVesting = new GridView();
      this.btnFinal = new System.Windows.Forms.Button();
      this.txtFinalVesting = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.cboTitle = new ComboBox();
      this.label8 = new System.Windows.Forms.Label();
      this.groupContainer1 = new GroupContainer();
      this.calendarButton2 = new CalendarButton();
      this.txtTrust1AmendedYear = new System.Windows.Forms.TextBox();
      this.calendarButton1 = new CalendarButton();
      this.txtTrust1Date = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.txtTrust1Beneficiary = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnTrust1Beneficiary = new System.Windows.Forms.Button();
      this.cboTrust1State = new ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.txtTrust1Tax = new System.Windows.Forms.TextBox();
      this.txtTrust1CorpName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.cboTrust1Type = new ComboBox();
      this.prtTrustName = new System.Windows.Forms.CheckBox();
      this.prtTrustAddendum = new System.Windows.Forms.Label();
      this.trustInformation = new GroupContainer();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.groupContainer4.SuspendLayout();
      ((ISupportInitialize) this.calendarButton3).BeginInit();
      ((ISupportInitialize) this.calendarButton4).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnNboVideo).BeginInit();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.calendarButton2).BeginInit();
      ((ISupportInitialize) this.calendarButton1).BeginInit();
      this.trustInformation.SuspendLayout();
      this.SuspendLayout();
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(68, 3);
      this.btnDelete.Margin = new Padding(5, 3, 0, 3);
      this.btnDelete.MouseDownImage = (System.Drawing.Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 22;
      this.btnDelete.TabStop = false;
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.btnDelete, "Delete Vesting Information");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(47, 3);
      this.btnEdit.Margin = new Padding(5, 3, 0, 3);
      this.btnEdit.MouseDownImage = (System.Drawing.Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 19;
      this.btnEdit.TabStop = false;
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.btnEdit, "Edit Vesting Information");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(26, 3);
      this.btnAdd.Margin = new Padding(5, 3, 0, 3);
      this.btnAdd.MouseDownImage = (System.Drawing.Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 21;
      this.btnAdd.TabStop = false;
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.btnAdd, "Add Vesting Information");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.calendarButton3);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.calendarButton4);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.txtTrust2AmendedYear);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.label13);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.txtTrust2Beneficiary);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.label14);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.btnTrust2Beneficiary);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.cboTrust2State);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.txtTrust2Date);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.label15);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.label16);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.label17);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.txtTrust2Tax);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.txtTrust2CorpName);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.label18);
      this.groupContainer4.Controls.Add((System.Windows.Forms.Control) this.cboTrust2Type);
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(328, 31);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(320, 229);
      this.groupContainer4.TabIndex = 2;
      this.groupContainer4.Text = "Trust 2";
      this.calendarButton3.DateControl = (System.Windows.Forms.Control) this.txtTrust2AmendedYear;
      ((IconButton) this.calendarButton3).Image = (System.Drawing.Image) componentResourceManager.GetObject("calendarButton3.Image");
      this.calendarButton3.Location = new Point(209, 150);
      this.calendarButton3.MouseDownImage = (System.Drawing.Image) null;
      this.calendarButton3.Name = "calendarButton3";
      this.calendarButton3.Size = new Size(16, 16);
      this.calendarButton3.SizeMode = PictureBoxSizeMode.AutoSize;
      this.calendarButton3.TabIndex = 158;
      this.calendarButton3.TabStop = false;
      this.txtTrust2AmendedYear.Location = new Point(125, 148);
      this.txtTrust2AmendedYear.MaxLength = 100;
      this.txtTrust2AmendedYear.Name = "txtTrust2AmendedYear";
      this.txtTrust2AmendedYear.Size = new Size(80, 26);
      this.txtTrust2AmendedYear.TabIndex = 5;
      this.txtTrust2AmendedYear.Tag = (object) "Vesting.Trst2AmdDate";
      this.calendarButton4.DateControl = (System.Windows.Forms.Control) this.txtTrust2Date;
      ((IconButton) this.calendarButton4).Image = (System.Drawing.Image) componentResourceManager.GetObject("calendarButton4.Image");
      this.calendarButton4.Location = new Point(209, 128);
      this.calendarButton4.MouseDownImage = (System.Drawing.Image) null;
      this.calendarButton4.Name = "calendarButton4";
      this.calendarButton4.Size = new Size(16, 16);
      this.calendarButton4.SizeMode = PictureBoxSizeMode.AutoSize;
      this.calendarButton4.TabIndex = 157;
      this.calendarButton4.TabStop = false;
      this.txtTrust2Date.Location = new Point(125, 126);
      this.txtTrust2Date.MaxLength = 100;
      this.txtTrust2Date.Name = "txtTrust2Date";
      this.txtTrust2Date.Size = new Size(80, 26);
      this.txtTrust2Date.TabIndex = 4;
      this.txtTrust2Date.Tag = (object) "Vesting.Trst2Date";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(10, 151);
      this.label13.Name = "label13";
      this.label13.Size = new Size(155, 19);
      this.label13.TabIndex = 154;
      this.label13.Text = "Amended Date/Year";
      this.txtTrust2Beneficiary.Location = new Point(125, 170);
      this.txtTrust2Beneficiary.Multiline = true;
      this.txtTrust2Beneficiary.Name = "txtTrust2Beneficiary";
      this.txtTrust2Beneficiary.ScrollBars = ScrollBars.Both;
      this.txtTrust2Beneficiary.Size = new Size(182, 49);
      this.txtTrust2Beneficiary.TabIndex = 7;
      this.txtTrust2Beneficiary.Tag = (object) "Vesting.Trst2Bfcry";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(10, 38);
      this.label14.Name = "label14";
      this.label14.Size = new Size(91, 19);
      this.label14.TabIndex = 7;
      this.label14.Text = "Trust Name";
      this.btnTrust2Beneficiary.BackColor = SystemColors.Control;
      this.btnTrust2Beneficiary.Location = new Point(10, 170);
      this.btnTrust2Beneficiary.Name = "btnTrust2Beneficiary";
      this.btnTrust2Beneficiary.Size = new Size(106, 22);
      this.btnTrust2Beneficiary.TabIndex = 6;
      this.btnTrust2Beneficiary.Text = "Build Beneficiary";
      this.btnTrust2Beneficiary.UseVisualStyleBackColor = true;
      this.btnTrust2Beneficiary.Click += new EventHandler(this.btnTrust2Beneficiary_Click);
      this.cboTrust2State.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTrust2State.Location = new Point(125, 57);
      this.cboTrust2State.MaxDropDownItems = 10;
      this.cboTrust2State.Name = "cboTrust2State";
      this.cboTrust2State.Size = new Size(136, 27);
      this.cboTrust2State.TabIndex = 1;
      this.cboTrust2State.Tag = (object) "Vesting.Trst2State";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(10, 84);
      this.label15.Name = "label15";
      this.label15.Size = new Size(81, 19);
      this.label15.TabIndex = 11;
      this.label15.Text = "Org. Type";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(10, 129);
      this.label16.Name = "label16";
      this.label16.Size = new Size(121, 19);
      this.label16.TabIndex = 152;
      this.label16.Text = "Trust Date/Year";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(10, 107);
      this.label17.Name = "label17";
      this.label17.Size = new Size(124, 19);
      this.label17.TabIndex = 13;
      this.label17.Text = "Tax ID/Trust No.";
      this.txtTrust2Tax.Location = new Point(125, 104);
      this.txtTrust2Tax.Name = "txtTrust2Tax";
      this.txtTrust2Tax.Size = new Size(184, 26);
      this.txtTrust2Tax.TabIndex = 3;
      this.txtTrust2Tax.Tag = (object) "Vesting.Trst2TaxID";
      this.txtTrust2CorpName.Location = new Point(125, 35);
      this.txtTrust2CorpName.Name = "txtTrust2CorpName";
      this.txtTrust2CorpName.Size = new Size(184, 26);
      this.txtTrust2CorpName.TabIndex = 0;
      this.txtTrust2CorpName.Tag = (object) "Vesting.Trst2Name";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(10, 60);
      this.label18.Name = "label18";
      this.label18.Size = new Size(83, 19);
      this.label18.TabIndex = 9;
      this.label18.Text = "Org. State";
      this.cboTrust2Type.Location = new Point(125, 81);
      this.cboTrust2Type.Name = "cboTrust2Type";
      this.cboTrust2Type.Size = new Size(184, 27);
      this.cboTrust2Type.TabIndex = 2;
      this.cboTrust2Type.Tag = (object) "Vesting.Trst2Type";
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSelOfcr2Title);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label25);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSelOfcr2Name);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label26);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSelOfcr1Title);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label24);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSelOfcr1Name);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label23);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSeller4);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label22);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSeller3);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label21);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSeller2);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label20);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSeller1);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label19);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSelCorpName);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label11);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.cboSelType);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label10);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtSelTax);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.cboSelState);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label12);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(4, 668);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(644, 219);
      this.groupContainer2.TabIndex = 4;
      this.groupContainer2.Text = "Seller";
      this.txtSelOfcr2Title.Location = new Point(422, 189);
      this.txtSelOfcr2Title.Name = "txtSelOfcr2Title";
      this.txtSelOfcr2Title.Size = new Size(184, 26);
      this.txtSelOfcr2Title.TabIndex = 14;
      this.txtSelOfcr2Title.Tag = (object) "Vesting.SelOfcr2Titl";
      this.label25.AutoSize = true;
      this.label25.Location = new Point(328, 192);
      this.label25.Name = "label25";
      this.label25.Size = new Size(106, 19);
      this.label25.TabIndex = 42;
      this.label25.Text = "Officer 2 Title";
      this.txtSelOfcr2Name.Location = new Point(422, 168);
      this.txtSelOfcr2Name.Name = "txtSelOfcr2Name";
      this.txtSelOfcr2Name.Size = new Size(184, 26);
      this.txtSelOfcr2Name.TabIndex = 13;
      this.txtSelOfcr2Name.Tag = (object) "Vesting.SelOfcr2Nm";
      this.label26.AutoSize = true;
      this.label26.Location = new Point(328, 171);
      this.label26.Name = "label26";
      this.label26.Size = new Size(120, 19);
      this.label26.TabIndex = 40;
      this.label26.Text = "Officer 2 Name";
      this.txtSelOfcr1Title.Location = new Point(422, 147);
      this.txtSelOfcr1Title.Name = "txtSelOfcr1Title";
      this.txtSelOfcr1Title.Size = new Size(184, 26);
      this.txtSelOfcr1Title.TabIndex = 12;
      this.txtSelOfcr1Title.Tag = (object) "Vesting.SelOfcr1Titl";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(328, 150);
      this.label24.Name = "label24";
      this.label24.Size = new Size(106, 19);
      this.label24.TabIndex = 38;
      this.label24.Text = "Officer 1 Title";
      this.txtSelOfcr1Name.Location = new Point(422, 126);
      this.txtSelOfcr1Name.Name = "txtSelOfcr1Name";
      this.txtSelOfcr1Name.Size = new Size(184, 26);
      this.txtSelOfcr1Name.TabIndex = 11;
      this.txtSelOfcr1Name.Tag = (object) "Vesting.SelOfcr1Nm";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(328, 129);
      this.label23.Name = "label23";
      this.label23.Size = new Size(120, 19);
      this.label23.TabIndex = 36;
      this.label23.Text = "Officer 1 Name";
      this.txtSeller4.Location = new Point(65, 99);
      this.txtSeller4.Name = "txtSeller4";
      this.txtSeller4.Size = new Size(250, 26);
      this.txtSeller4.TabIndex = 4;
      this.txtSeller4.Tag = (object) "Seller4.Name";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(10, 102);
      this.label22.Name = "label22";
      this.label22.Size = new Size(64, 19);
      this.label22.TabIndex = 34;
      this.label22.Text = "Seller 4";
      this.txtSeller3.Location = new Point(65, 78);
      this.txtSeller3.Name = "txtSeller3";
      this.txtSeller3.Size = new Size(250, 26);
      this.txtSeller3.TabIndex = 3;
      this.txtSeller3.Tag = (object) "Seller3.Name";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(10, 81);
      this.label21.Name = "label21";
      this.label21.Size = new Size(64, 19);
      this.label21.TabIndex = 32;
      this.label21.Text = "Seller 3";
      this.txtSeller2.Location = new Point(65, 57);
      this.txtSeller2.Name = "txtSeller2";
      this.txtSeller2.Size = new Size(250, 26);
      this.txtSeller2.TabIndex = 2;
      this.txtSeller2.Tag = (object) "VEND.X412";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(10, 60);
      this.label20.Name = "label20";
      this.label20.Size = new Size(64, 19);
      this.label20.TabIndex = 30;
      this.label20.Text = "Seller 2";
      this.txtSeller1.Location = new Point(65, 36);
      this.txtSeller1.Name = "txtSeller1";
      this.txtSeller1.Size = new Size(250, 26);
      this.txtSeller1.TabIndex = 1;
      this.txtSeller1.Tag = (object) "638";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(10, 39);
      this.label19.Name = "label19";
      this.label19.Size = new Size(64, 19);
      this.label19.TabIndex = 28;
      this.label19.Text = "Seller 1";
      this.txtSelCorpName.Location = new Point(422, 36);
      this.txtSelCorpName.Name = "txtSelCorpName";
      this.txtSelCorpName.Size = new Size(184, 26);
      this.txtSelCorpName.TabIndex = 5;
      this.txtSelCorpName.Tag = (object) "1863";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(328, 61);
      this.label11.Name = "label11";
      this.label11.Size = new Size(83, 19);
      this.label11.TabIndex = 22;
      this.label11.Text = "Org. State";
      this.cboSelType.Location = new Point(422, 82);
      this.cboSelType.Name = "cboSelType";
      this.cboSelType.Size = new Size(184, 27);
      this.cboSelType.TabIndex = 9;
      this.cboSelType.Tag = (object) "1865";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(328, 85);
      this.label10.Name = "label10";
      this.label10.Size = new Size(81, 19);
      this.label10.TabIndex = 24;
      this.label10.Text = "Org. Type";
      this.txtSelTax.Location = new Point(422, 105);
      this.txtSelTax.Name = "txtSelTax";
      this.txtSelTax.Size = new Size(184, 26);
      this.txtSelTax.TabIndex = 10;
      this.txtSelTax.Tag = (object) "1866";
      this.cboSelState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSelState.Location = new Point(422, 58);
      this.cboSelState.MaxDropDownItems = 10;
      this.cboSelState.Name = "cboSelState";
      this.cboSelState.Size = new Size(136, 27);
      this.cboSelState.TabIndex = 8;
      this.cboSelState.Tag = (object) "1864";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(328, 39);
      this.label12.Name = "label12";
      this.label12.Size = new Size(92, 19);
      this.label12.TabIndex = 20;
      this.label12.Text = "Corp Name";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(328, 108);
      this.label9.Name = "label9";
      this.label9.Size = new Size(54, 19);
      this.label9.TabIndex = 26;
      this.label9.Text = "Tax ID";
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.flowLayoutPanel1);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.gvVesting);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.btnFinal);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.txtFinalVesting);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.cboTitle);
      this.groupContainer3.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(4, 265);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(644, 398);
      this.groupContainer3.TabIndex = 3;
      this.groupContainer3.Text = "Vesting Information";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnEdit);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnAdd);
      this.flowLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.btnNboVideo);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(552, 1);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(84, 22);
      this.flowLayoutPanel1.TabIndex = 22;
      this.btnNboVideo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNboVideo.BackColor = Color.Transparent;
      this.btnNboVideo.DisabledImage = (System.Drawing.Image) null;
      this.btnNboVideo.ErrorImage = (System.Drawing.Image) null;
      this.btnNboVideo.Image = (System.Drawing.Image) Resources.video_file;
      this.btnNboVideo.InitialImage = (System.Drawing.Image) Resources.video_file;
      this.btnNboVideo.Location = new Point(5, 3);
      this.btnNboVideo.Margin = new Padding(5, 3, 0, 3);
      this.btnNboVideo.MouseDownImage = (System.Drawing.Image) null;
      this.btnNboVideo.MouseOverImage = (System.Drawing.Image) Resources.video_file_over;
      this.btnNboVideo.Name = "btnNboVideo";
      this.btnNboVideo.Size = new Size(16, 16);
      this.btnNboVideo.SizeMode = PictureBoxSizeMode.CenterImage;
      this.btnNboVideo.TabIndex = 21;
      this.btnNboVideo.TabStop = false;
      this.btnNboVideo.Click += new EventHandler(this.btnNboVideo_Click);
      this.gvVesting.AllowMultiselect = false;
      this.gvVesting.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.None;
      gvColumn1.Text = "Name";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.None;
      gvColumn2.Text = "SSN";
      gvColumn2.Width = 80;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "DOB";
      gvColumn3.Width = 70;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "AKA";
      gvColumn4.Width = 90;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.None;
      gvColumn5.Text = "Type";
      gvColumn5.Width = 105;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.SortMethod = GVSortMethod.None;
      gvColumn6.Text = "POA";
      gvColumn6.Width = 60;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.SortMethod = GVSortMethod.None;
      gvColumn7.Text = "Vesting";
      gvColumn7.Width = 148;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.SortMethod = GVSortMethod.None;
      gvColumn8.Text = "Sign";
      gvColumn8.Width = 43;
      this.gvVesting.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gvVesting.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvVesting.Location = new Point(0, 24);
      this.gvVesting.Name = "gvVesting";
      this.gvVesting.Size = new Size(644, 183);
      this.gvVesting.SortOption = GVSortOption.None;
      this.gvVesting.TabIndex = 20;
      this.gvVesting.SelectedIndexChanged += new EventHandler(this.gvVesting_SelectedIndexChanged);
      this.gvVesting.ItemDoubleClick += new GVItemEventHandler(this.gvVesting_ItemDoubleClick);
      this.btnFinal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnFinal.BackColor = SystemColors.Control;
      this.btnFinal.Location = new Point(517, 272);
      this.btnFinal.Name = "btnFinal";
      this.btnFinal.Size = new Size(116, 22);
      this.btnFinal.TabIndex = 14;
      this.btnFinal.Text = "Build Final Vesting";
      this.btnFinal.UseVisualStyleBackColor = true;
      this.btnFinal.Click += new EventHandler(this.btnFinal_Click);
      this.txtFinalVesting.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFinalVesting.Location = new Point(10, 298);
      this.txtFinalVesting.Multiline = true;
      this.txtFinalVesting.Name = "txtFinalVesting";
      this.txtFinalVesting.Size = new Size(623, 90);
      this.txtFinalVesting.TabIndex = 15;
      this.txtFinalVesting.Tag = (object) "1867";
      this.label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 276);
      this.label7.Name = "label7";
      this.label7.Size = new Size(166, 19);
      this.label7.TabIndex = 16;
      this.label7.Text = "Final Vesting To Read";
      this.cboTitle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.cboTitle.Location = new Point(10, 241);
      this.cboTitle.Name = "cboTitle";
      this.cboTitle.Size = new Size(623, 27);
      this.cboTitle.TabIndex = 13;
      this.cboTitle.Tag = (object) "33";
      this.label8.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 220);
      this.label8.Name = "label8";
      this.label8.Size = new Size(246, 19);
      this.label8.TabIndex = 18;
      this.label8.Text = "Manner in which Title will be held";
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.calendarButton2);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.calendarButton1);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.txtTrust1AmendedYear);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.txtTrust1Beneficiary);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.btnTrust1Beneficiary);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.cboTrust1State);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.txtTrust1Date);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.txtTrust1Tax);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.txtTrust1CorpName);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.cboTrust1Type);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(3, 31);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(320, 229);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Corporation/Trust 1";
      this.calendarButton2.DateControl = (System.Windows.Forms.Control) this.txtTrust1AmendedYear;
      ((IconButton) this.calendarButton2).Image = (System.Drawing.Image) componentResourceManager.GetObject("calendarButton2.Image");
      this.calendarButton2.Location = new Point(209, 150);
      this.calendarButton2.MouseDownImage = (System.Drawing.Image) null;
      this.calendarButton2.Name = "calendarButton2";
      this.calendarButton2.Size = new Size(16, 16);
      this.calendarButton2.SizeMode = PictureBoxSizeMode.AutoSize;
      this.calendarButton2.TabIndex = 156;
      this.calendarButton2.TabStop = false;
      this.txtTrust1AmendedYear.Location = new Point(125, 148);
      this.txtTrust1AmendedYear.MaxLength = 100;
      this.txtTrust1AmendedYear.Name = "txtTrust1AmendedYear";
      this.txtTrust1AmendedYear.Size = new Size(80, 26);
      this.txtTrust1AmendedYear.TabIndex = 5;
      this.txtTrust1AmendedYear.Tag = (object) "Vesting.Trst1AmdDate";
      this.calendarButton1.DateControl = (System.Windows.Forms.Control) this.txtTrust1Date;
      ((IconButton) this.calendarButton1).Image = (System.Drawing.Image) componentResourceManager.GetObject("calendarButton1.Image");
      this.calendarButton1.Location = new Point(209, 128);
      this.calendarButton1.MouseDownImage = (System.Drawing.Image) null;
      this.calendarButton1.Name = "calendarButton1";
      this.calendarButton1.Size = new Size(16, 16);
      this.calendarButton1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.calendarButton1.TabIndex = 155;
      this.calendarButton1.TabStop = false;
      this.txtTrust1Date.Location = new Point(125, 126);
      this.txtTrust1Date.MaxLength = 100;
      this.txtTrust1Date.Name = "txtTrust1Date";
      this.txtTrust1Date.Size = new Size(80, 26);
      this.txtTrust1Date.TabIndex = 4;
      this.txtTrust1Date.Tag = (object) "2554";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 151);
      this.label6.Name = "label6";
      this.label6.Size = new Size(155, 19);
      this.label6.TabIndex = 154;
      this.label6.Text = "Amended Date/Year";
      this.txtTrust1Beneficiary.Location = new Point(125, 170);
      this.txtTrust1Beneficiary.Multiline = true;
      this.txtTrust1Beneficiary.Name = "txtTrust1Beneficiary";
      this.txtTrust1Beneficiary.ScrollBars = ScrollBars.Both;
      this.txtTrust1Beneficiary.Size = new Size(182, 49);
      this.txtTrust1Beneficiary.TabIndex = 7;
      this.txtTrust1Beneficiary.Tag = (object) "2970";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(132, 19);
      this.label2.TabIndex = 7;
      this.label2.Text = "Corp/Trust Name";
      this.btnTrust1Beneficiary.BackColor = SystemColors.Control;
      this.btnTrust1Beneficiary.Location = new Point(10, 170);
      this.btnTrust1Beneficiary.Name = "btnTrust1Beneficiary";
      this.btnTrust1Beneficiary.Size = new Size(106, 22);
      this.btnTrust1Beneficiary.TabIndex = 6;
      this.btnTrust1Beneficiary.Text = "Build Beneficiary";
      this.btnTrust1Beneficiary.UseVisualStyleBackColor = true;
      this.btnTrust1Beneficiary.Click += new EventHandler(this.btnTrust1Beneficiary_Click);
      this.cboTrust1State.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTrust1State.Location = new Point(125, 57);
      this.cboTrust1State.MaxDropDownItems = 10;
      this.cboTrust1State.Name = "cboTrust1State";
      this.cboTrust1State.Size = new Size(136, 27);
      this.cboTrust1State.TabIndex = 1;
      this.cboTrust1State.Tag = (object) "1860";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 84);
      this.label4.Name = "label4";
      this.label4.Size = new Size(81, 19);
      this.label4.TabIndex = 11;
      this.label4.Text = "Org. Type";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 129);
      this.label1.Name = "label1";
      this.label1.Size = new Size(121, 19);
      this.label1.TabIndex = 152;
      this.label1.Text = "Trust Date/Year";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 107);
      this.label5.Name = "label5";
      this.label5.Size = new Size(124, 19);
      this.label5.TabIndex = 13;
      this.label5.Text = "Tax ID/Trust No.";
      this.txtTrust1Tax.Location = new Point(125, 104);
      this.txtTrust1Tax.Name = "txtTrust1Tax";
      this.txtTrust1Tax.Size = new Size(184, 26);
      this.txtTrust1Tax.TabIndex = 3;
      this.txtTrust1Tax.Tag = (object) "1862";
      this.txtTrust1CorpName.Location = new Point(125, 35);
      this.txtTrust1CorpName.Name = "txtTrust1CorpName";
      this.txtTrust1CorpName.Size = new Size(184, 26);
      this.txtTrust1CorpName.TabIndex = 0;
      this.txtTrust1CorpName.Tag = (object) "1859";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(83, 19);
      this.label3.TabIndex = 9;
      this.label3.Text = "Org. State";
      this.cboTrust1Type.Location = new Point(125, 81);
      this.cboTrust1Type.Name = "cboTrust1Type";
      this.cboTrust1Type.Size = new Size(184, 27);
      this.cboTrust1Type.TabIndex = 2;
      this.cboTrust1Type.Tag = (object) "1861";
      this.cboTrust1Type.SelectedIndexChanged += new EventHandler(this.cboTrust1Type_SelectedIndexChanged);
      this.prtTrustName.Location = new Point(422, 2);
      this.prtTrustName.Name = "prtTrustName";
      this.prtTrustName.BackColor = Color.Transparent;
      this.prtTrustName.Size = new Size(20, 20);
      this.prtTrustName.TabIndex = 3;
      this.prtTrustName.Tag = (object) "4662";
      this.prtTrustAddendum.AutoSize = true;
      this.prtTrustAddendum.Location = new Point(435, 5);
      this.prtTrustAddendum.BackColor = Color.Transparent;
      this.prtTrustAddendum.Name = "prtTrustAddendum";
      this.prtTrustAddendum.Size = new Size(309, 19);
      this.prtTrustAddendum.TabIndex = 42;
      this.prtTrustAddendum.Text = "Print Trust Name(s) on LE/CD Addendum";
      this.trustInformation.Controls.Add((System.Windows.Forms.Control) this.prtTrustAddendum);
      this.trustInformation.Controls.Add((System.Windows.Forms.Control) this.prtTrustName);
      this.trustInformation.HeaderForeColor = SystemColors.ControlText;
      this.trustInformation.Location = new Point(4, 0);
      this.trustInformation.Name = "trustInformation";
      this.trustInformation.Size = new Size(644, 26);
      this.trustInformation.TabIndex = 2;
      this.trustInformation.Text = "Trust Information";
      this.AutoScroll = true;
      this.Controls.Add((System.Windows.Forms.Control) this.trustInformation);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer4);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer2);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer3);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (BorrowerVestingForm);
      this.Size = new Size(655, 896);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      ((ISupportInitialize) this.calendarButton3).EndInit();
      ((ISupportInitialize) this.calendarButton4).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnNboVideo).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.calendarButton2).EndInit();
      ((ISupportInitialize) this.calendarButton1).EndInit();
      this.trustInformation.ResumeLayout(false);
      this.trustInformation.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initFields()
    {
      this.inputHandler = WinFormInputHandler.Create(this.loan);
      this.inputHandler.Attach((System.Windows.Forms.Control) this, this.fieldToolTip);
      this.cboTrust1State.SelectedIndexChanged += new EventHandler(this.state_SelectedIndexChanged);
      this.cboTrust2State.SelectedIndexChanged += new EventHandler(this.state_SelectedIndexChanged);
      this.cboSelState.SelectedIndexChanged += new EventHandler(this.state_SelectedIndexChanged);
    }

    private void initStates()
    {
      string[] items = new string[52]
      {
        "",
        "Alabama",
        "Alaska",
        "Arizona",
        "Arkansas",
        "California",
        "Colorado",
        "Connecticut",
        "Delaware",
        "Dist. of Col.",
        "Florida",
        "Georgia",
        "Hawaii",
        "Idaho",
        "Illinois",
        "Indiana",
        "Iowa",
        "Kansas",
        "Kentucky",
        "Louisiana",
        "Maine",
        "Maryland",
        "Massachusetts",
        "Michigan",
        "Minnesota",
        "Mississippi",
        "Missouri",
        "Montana",
        "Nebraska",
        "Nevada",
        "New Hampshire",
        "New Jersey",
        "New Mexico",
        "New York",
        "North Carolina",
        "North Dakota",
        "Ohio",
        "Oklahoma",
        "Oregon",
        "Pennsylvania",
        "Rhode Island",
        "South Carolina",
        "South Dakota",
        "Tennessee",
        "Texas",
        "Utah",
        "Vermont",
        "Virginia",
        "Washington",
        "West Virginia",
        "Wisconsin",
        "Wyoming"
      };
      this.cboTrust1State.Items.AddRange((object[]) items);
      this.cboTrust2State.Items.AddRange((object[]) items);
      this.cboSelState.Items.AddRange((object[]) items);
    }

    private void initTitle()
    {
      this.cboTitle.Items.Clear();
      string[] source = new string[5]
      {
        "Sole Ownership",
        "Life Estate",
        "Tenancy in Common",
        "Joint Tenancy with Right of Survivorship",
        "Tenancy by the Entirety"
      };
      string[] ruleDropdownOptions = Session.LoanDataMgr != null ? Session.LoanDataMgr.GetFieldRuleDropdownOptions("33") : (string[]) null;
      if (ruleDropdownOptions != null)
      {
        foreach (string str in ruleDropdownOptions)
        {
          if (!(this.loan.GetField("1825") != "2020") || !((IEnumerable<string>) source).Contains<string>(str))
            this.cboTitle.Items.Add((object) str);
        }
      }
      else
      {
        this.cboTitle.Items.Add((object) "");
        if (this.loan.GetField("1825") == "2020")
        {
          this.cboTitle.Items.Add((object) "Sole Ownership");
          this.cboTitle.Items.Add((object) "Life Estate");
          this.cboTitle.Items.Add((object) "Tenancy in Common");
          this.cboTitle.Items.Add((object) "Joint Tenancy with Right of Survivorship");
          this.cboTitle.Items.Add((object) "Tenancy by the Entirety");
        }
        this.cboTitle.Items.Add((object) "All as Joint Tenants");
        this.cboTitle.Items.Add((object) "All as Tenants in Common");
        this.cboTitle.Items.Add((object) "As Community Property");
        this.cboTitle.Items.Add((object) "As Her Sole And Separate Property");
        this.cboTitle.Items.Add((object) "As His Sole And Separate Property");
        this.cboTitle.Items.Add((object) "As Joint Tenants");
        this.cboTitle.Items.Add((object) "As Tenants in Common");
        this.cboTitle.Items.Add((object) "As Tenancy by Entirety");
        this.cboTitle.Items.Add((object) "Each as to An Undivided One Half Interest");
        this.cboTitle.Items.Add((object) "Each as to An Undivided One Third Interest");
        this.cboTitle.Items.Add((object) "Each as to An Undivided One Fourth Interest");
        this.cboTitle.Items.Add((object) "Husband And Wife");
        this.cboTitle.Items.Add((object) "Husband And Wife as Joint Tenants");
        this.cboTitle.Items.Add((object) "Husband And Wife as Joint Tenants With Right of Survivorship");
        this.cboTitle.Items.Add((object) "Husband And Wife as Tenants in Common");
        this.cboTitle.Items.Add((object) "As Joint Tenants With Right of Survivorship");
        this.cboTitle.Items.Add((object) "Wife And Husband");
        this.cboTitle.Items.Add((object) "Both Unmarried");
        this.cboTitle.Items.Add((object) "Spouses Married to Each Other");
        this.cboTitle.Items.Add((object) "Community Property");
        this.cboTitle.Items.Add((object) "Joint Tenants");
        this.cboTitle.Items.Add((object) "Single Man");
        this.cboTitle.Items.Add((object) "Single Woman");
        this.cboTitle.Items.Add((object) "Married Man");
        this.cboTitle.Items.Add((object) "Married Woman");
        this.cboTitle.Items.Add((object) "Tenants in Common");
        this.cboTitle.Items.Add((object) "Tenancy by Entirety");
        this.cboTitle.Items.Add((object) "To be Decided in Escrow");
        this.cboTitle.Items.Add((object) "Unmarried Man");
        this.cboTitle.Items.Add((object) "Unmarried Woman");
        this.cboTitle.Items.Add((object) "Other");
      }
    }

    private void loadBorrowerAndTrusteeList()
    {
      this.gvVesting.Items.Clear();
      foreach (VestingPartyFields vestingPartyField in this.loan.GetVestingPartyFields(true))
        this.gvVesting.Items.Add(this.createGVItemForVestingParty(vestingPartyField));
    }

    private GVItem createGVItemForVestingParty(VestingPartyFields fields)
    {
      return new GVItem(this.loan.GetField(fields.NameField, fields.BorrowerPair))
      {
        SubItems = {
          (object) this.loan.GetField(fields.SSNField, fields.BorrowerPair),
          (object) this.loan.GetField(fields.BorrowerDOB, fields.BorrowerPair),
          (object) this.loan.GetField(fields.AliasField, fields.BorrowerPair),
          (object) this.loan.GetField(fields.TypeField, fields.BorrowerPair),
          (object) this.loan.GetField(fields.POAField, fields.BorrowerPair),
          (object) this.loan.GetField(fields.VestingField, fields.BorrowerPair),
          (object) this.loan.GetField(fields.AuthToSignField, fields.BorrowerPair)
        },
        Tag = (object) fields
      };
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (BorrowerVestingDetail borrowerVestingDetail = new BorrowerVestingDetail(this.loan, VestingPartyFields.GetAdditionalVestingPartyInstanceFields(this.loan.GetNumberOfAdditionalVestingParties() + 1)))
      {
        if (borrowerVestingDetail.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.loadBorrowerAndTrusteeList();
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvVesting.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a borrower or trustee.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.editTrustee(this.gvVesting.SelectedItems[0]);
    }

    private void editTrustee(GVItem item)
    {
      object tag1 = item.Tag;
      switch (tag1)
      {
        case VestingPartyFields _:
          BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
          VestingPartyFields tag2 = (VestingPartyFields) item.Tag;
          BorrowerPair borrowerPair = tag2.BorrowerPair;
          if (borrowerPair != null && currentBorrowerPair != null && !borrowerPair.Equals((object) currentBorrowerPair))
            this.loan.SetBorrowerPair(borrowerPair);
          using (BorrowerVestingDetail borrowerVestingDetail = new BorrowerVestingDetail(this.loan, tag2))
          {
            if (borrowerVestingDetail.ShowDialog((IWin32Window) this) == DialogResult.OK)
              this.loadBorrowerAndTrusteeList();
          }
          if (borrowerPair == null || currentBorrowerPair == null || this.loan.CurrentBorrowerPair != null && !(currentBorrowerPair.Id != this.loan.CurrentBorrowerPair.Id))
            break;
          this.loan.SetBorrowerPair(currentBorrowerPair);
          break;
        case FileContactRecord.ContactFields _:
          FileContactRecord.ContactFields contactFields = (FileContactRecord.ContactFields) tag1;
          using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, "Non-Borrowing Owner", new InputFormInfo("FC_NonBorrowingOwnerContact", "FC_NonBorrowingOwnerContact#" + (object) contactFields.NBOCIndex), 600, 300, FieldSource.CurrentLoan, "NBOC", Session.DefaultInstance))
          {
            int num = (int) entryPopupDialog.ShowDialog((IWin32Window) Session.DefaultInstance.MainForm);
            string str = "NBOC";
            this.gvVesting.SelectedItems[0].Text = (this.loan.GetField(str + contactFields.NBOCIndex.ToString("00") + "01") + " " + this.loan.GetField(str + contactFields.NBOCIndex.ToString("00") + "02") + " " + this.loan.GetField(str + contactFields.NBOCIndex.ToString("00") + "03") + " " + this.loan.GetField(str + contactFields.NBOCIndex.ToString("00") + "04")).Trim();
            this.gvVesting.SelectedItems[0].SubItems[2].Text = this.loan.GetField(str + contactFields.NBOCIndex.ToString("00") + "16");
            this.gvVesting.SelectedItems[0].SubItems[2].Text = this.loan.GetField(str + contactFields.NBOCIndex.ToString("00") + "09");
            break;
          }
      }
    }

    private string getVestingBorrowerPair(VestingPartyFields fields)
    {
      return fields.BorrowerPair != null ? fields.BorrowerPair.Id : this.loan.GetSimpleField(fields.BorrowerPairField);
    }

    private void btnFinal_Click(object sender, EventArgs e)
    {
      Dictionary<string, List<string>> dictionary1 = new Dictionary<string, List<string>>();
      Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
      bool flag = false;
      for (int nItemIndex = 0; nItemIndex < this.gvVesting.Items.Count; ++nItemIndex)
      {
        VestingPartyFields tag = (VestingPartyFields) this.gvVesting.Items[nItemIndex].Tag;
        string simpleField1 = this.loan.GetSimpleField(tag.TypeField, tag.BorrowerPair);
        string str1 = this.loan.GetSimpleField(tag.NameField, tag.BorrowerPair);
        string simpleField2 = this.loan.GetSimpleField(tag.VestingField, tag.BorrowerPair);
        string simpleField3 = this.loan.GetSimpleField(tag.TrusteeOfField, tag.BorrowerPair);
        string vestingBorrowerPair = this.getVestingBorrowerPair(tag);
        if (VestingPartyFields.IsNonTrusteeBorrower(simpleField1))
        {
          if (!dictionary1.ContainsKey(vestingBorrowerPair))
            dictionary1[vestingBorrowerPair] = new List<string>();
          if (simpleField2 != "")
            str1 = str1 + ", " + simpleField2 + ",";
          dictionary1[vestingBorrowerPair].Add(str1);
        }
        else if (VestingPartyFields.IsCorporateOfficer(simpleField1) && !flag)
        {
          if (!dictionary1.ContainsKey(vestingBorrowerPair))
            dictionary1[vestingBorrowerPair] = new List<string>();
          string str2 = this.loan.GetField("1859");
          string field = this.loan.GetField("1861");
          if (field != "")
            str2 = str2 + ", " + field + ",";
          dictionary1[vestingBorrowerPair].Add(str2);
          flag = true;
        }
        else if (VestingPartyFields.IsTrusteeType(simpleField1))
        {
          if (!dictionary2.ContainsKey(simpleField3))
            dictionary2[simpleField3] = new List<string>();
          dictionary2[simpleField3].Add(str1);
        }
      }
      List<string> stringList = new List<string>();
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (dictionary1.ContainsKey(borrowerPairs[index].Id))
        {
          stringList.AddRange((IEnumerable<string>) dictionary1[borrowerPairs[index].Id]);
          dictionary1.Remove(borrowerPairs[index].Id);
        }
      }
      string[] strArray = new string[2]
      {
        "Trust 1",
        "Trust 2"
      };
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (dictionary2.ContainsKey(strArray[index]))
        {
          string str3 = index == 0 ? this.txtTrust1CorpName.Text : this.txtTrust2CorpName.Text;
          string str4 = index == 0 ? this.txtTrust1Date.Text : this.txtTrust2Date.Text;
          string str5 = index == 0 ? this.txtTrust1AmendedYear.Text : this.txtTrust2AmendedYear.Text;
          string str6 = index == 0 ? this.txtTrust1Tax.Text : this.txtTrust2Tax.Text;
          string str7 = index == 0 ? this.txtTrust1Beneficiary.Text : this.txtTrust2Beneficiary.Text;
          string[] array = dictionary2[strArray[index]].ToArray();
          string str8 = string.Join(" AND ", array);
          string str9 = (array.Length != 1 ? str8 + " AS TRUSTEES OF " : str8 + " AS TRUSTEE OF ") + str3;
          if (str6 != "")
            str9 = str9 + " UNDER TRUST INSTRUMENT " + str6;
          if (str4 != "" && str4 != "//")
            str9 = str9 + " DATED " + str4;
          if (str5 != "" && str5 != "//")
            str9 = str9 + ", AMENDED " + str5;
          if (str7 != "")
            str9 = str9 + ", FOR THE BENEFIT OF " + str7 + ",";
          stringList.Add(str9);
        }
      }
      foreach (List<string> collection in dictionary1.Values)
        stringList.AddRange((IEnumerable<string>) collection);
      string str = string.Join(" AND ", stringList.ToArray());
      while (str.EndsWith(","))
        str = str.Substring(0, str.Length - 1);
      if (this.cboTitle.Text.Trim() != "")
        str = str + ", " + this.cboTitle.Text.Trim();
      this.txtFinalVesting.Text = str.Trim().ToUpper();
      this.inputHandler.CommitValue((System.Windows.Forms.Control) this.txtFinalVesting);
    }

    private void state_SelectedIndexChanged(object sender, EventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      this.loan.SetCurrentField(comboBox.Tag.ToString(), comboBox.Text);
      if (comboBox == this.cboSelState)
        BorrowerVestingForm.initStateType(this.cboSelType, this.cboSelState.Text);
      else if (comboBox == this.cboTrust1State)
      {
        BorrowerVestingForm.initStateType(this.cboTrust1Type, this.cboTrust1State.Text);
      }
      else
      {
        if (comboBox != this.cboTrust2State)
          return;
        BorrowerVestingForm.initStateType(this.cboTrust2Type, this.cboTrust2State.Text);
      }
    }

    private static void initStateType(ComboBox cbo, string stateName)
    {
      string str = "A ";
      if (stateName != "")
      {
        string lower = stateName.Substring(0, 1).ToLower();
        if (lower == "a" || lower == "e" || lower == "i" || lower == "o")
          str = "An ";
      }
      cbo.Items.Clear();
      cbo.Items.Add((object) "");
      cbo.Items.Add((object) "An Inter Vivos Trust");
      if (stateName != "")
        cbo.Items.Add((object) (str + stateName + " Trust"));
      cbo.Items.Add((object) "A Trust");
      if (stateName != "")
      {
        cbo.Items.Add((object) (str + stateName + " Corporation"));
        cbo.Items.Add((object) (str + stateName + " Banking Corporation"));
        cbo.Items.Add((object) (str + stateName + " General Partnership"));
        cbo.Items.Add((object) (str + stateName + " Limited Partnership"));
        cbo.Items.Add((object) (str + stateName + " Limited Liability Company"));
      }
      cbo.Items.AddRange((object[]) BorrowerVestingForm.orgType);
    }

    private void formatFieldValue(object sender, FieldFormat format)
    {
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      string str = Utils.ApplyFieldFormatting(textBox.Text, format);
      if (!(str != textBox.Text))
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void onDateFieldKeyUp(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.D)
        return;
      ((System.Windows.Forms.Control) sender).Text = DateTime.Today.ToString("MM/dd/yyyy");
    }

    private void setFieldValue(string id, string val)
    {
      this.setFieldValue(id, val, (BorrowerPair) null);
    }

    private void setFieldValue(string id, string val, BorrowerPair pair)
    {
      if (pair != null)
      {
        this.loan.SetCurrentField(id, val, pair);
        this.piggySyncTool.SyncPiggyBackField(id, FieldSource.LinkedLoan, val, pair);
      }
      else
      {
        this.loan.SetCurrentField(id, val);
        this.piggySyncTool.SyncPiggyBackField(id, FieldSource.LinkedLoan, val);
      }
    }

    private void btnTrust1Beneficiary_Click(object sender, EventArgs e)
    {
      this.buildTrustBeneficiary("Trust 1", this.txtTrust1Beneficiary);
    }

    private void btnTrust2Beneficiary_Click(object sender, EventArgs e)
    {
      this.buildTrustBeneficiary("Trust 2", this.txtTrust2Beneficiary);
    }

    private void buildTrustBeneficiary(string trustID, System.Windows.Forms.TextBox textBox)
    {
      string str = "";
      for (int nItemIndex = 0; nItemIndex < this.gvVesting.Items.Count; ++nItemIndex)
      {
        VestingPartyFields tag = (VestingPartyFields) this.gvVesting.Items[nItemIndex].Tag;
        if (VestingPartyFields.IsBeneficiaryType(this.loan.GetSimpleField(tag.TypeField, tag.BorrowerPair)) && !(this.loan.GetSimpleField(tag.TrusteeOfField, tag.BorrowerPair) != trustID))
        {
          string simpleField = this.loan.GetSimpleField(tag.NameField, tag.BorrowerPair);
          if (str != "")
            str += " AND ";
          str += simpleField;
        }
      }
      textBox.Text = str.Trim().ToUpper();
      this.inputHandler.CommitValue((System.Windows.Forms.Control) textBox);
    }

    private void cboTrust1Type_SelectedIndexChanged(object sender, EventArgs e)
    {
      string lower = this.cboTrust1Type.Text.ToLower();
      if (lower.EndsWith("trust") && !(lower == "an inter vivos trust"))
        return;
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      if (borrowerPairs == null)
        return;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        this.setFieldValue("2968", "", borrowerPairs[index]);
        this.setFieldValue("2969", "", borrowerPairs[index]);
      }
      this.loadBorrowerAndTrusteeList();
    }

    private void gvVesting_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag1 = this.gvVesting.SelectedItems.Count > 0;
      bool flag2 = false;
      if (flag1)
      {
        switch (this.gvVesting.SelectedItems[0].Tag)
        {
          case VestingPartyFields _:
            if (((VestingPartyFields) this.gvVesting.SelectedItems[0].Tag).Type != VestingPartyType.Other)
            {
              flag2 = true;
              break;
            }
            break;
          case FileContactRecord.ContactFields _:
            flag1 = true;
            flag2 = true;
            break;
        }
      }
      this.btnEdit.Enabled = flag1;
      this.btnDelete.Enabled = flag1 && !flag2;
    }

    private void onYearFieldKeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void onDateFieldValidate(object sender, CancelEventArgs e)
    {
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      string val = textBox.Text.Trim();
      try
      {
        if (val.Length > 4)
        {
          textBox.Text = Utils.FormatDateValue(val);
        }
        else
        {
          if (Utils.IsInt((object) val))
            return;
          textBox.Text = "";
        }
      }
      catch (FormatException ex)
      {
        textBox.Text = "";
      }
    }

    private void onDateFieldKeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == '/')
        return;
      e.Handled = true;
    }

    private void gvVesting_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editTrustee(e.Item);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      VestingPartyFields tag = (VestingPartyFields) this.gvVesting.SelectedItems[0].Tag;
      if (tag.Type != VestingPartyType.Other)
        return;
      string field = this.loan.GetField(tag.NameField);
      string str = "You are about to delete the vesting information for '" + field + "'. Once completed, you should review or re-generate the beneficiary and final vesting statements to reflect this change.";
      if (this.loan.GetNBOLinkedVesting(tag.VestingPartyIndex - 1) > 0)
        str = this.loan.Use2015RESPA ? "You are about to delete the vesting information for " + field + ", a Non-Borrowing Owner. Once completed, you should review or re-generate any disclosures, documents, Closing Disclosures, beneficiary and final vesting statements to reflect this change." : "You are about to delete the vesting information for " + field + ", a Non-Borrowing Owner. Once completed, you should review or re-generate any disclosures, documents, HUD-1 Settlement Statements, beneficiary and final vesting statements to reflect this change.";
      if (Utils.Dialog((IWin32Window) this, str + Environment.NewLine + Environment.NewLine + "Would you like to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.loan.RemoveAdditionalVestingPartyAt(tag.VestingPartyIndex - 1);
      this.loadBorrowerAndTrusteeList();
    }

    private void btnNboVideo_Click(object sender, EventArgs e)
    {
      WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_BorrowerVestingNBO.html", "Borrower Vesting & Non-Borrowing Owners", 1046, 732);
    }

    private void prtTrustName_CheckedChanged(object sender, EventArgs e)
    {
    }

    public void RefreshContents() => this.refreshContents();

    public void RefreshLoanContents() => this.refreshContents();

    private void refreshContents()
    {
      this.inputHandler.RefreshContents(true);
      this.loadBorrowerAndTrusteeList();
    }
  }
}
