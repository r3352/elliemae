// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolAssignmentDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MbsPoolAssignmentDialog : Form
  {
    private ITradeEditorBase tradeEditor;
    private MbsPoolAssignmentListScreen ctlTradeList;
    private FieldFilterList filters;
    private IContainer components;
    private Panel pnlTop;
    private Label lblHelp;
    private Panel pnlTradeList;
    private Panel pnlSearch;
    private Label label2;
    private TextBox txtMinAmount;
    private TextBox txtMaxAmount;
    private Label label3;
    private GroupContainer grpHeader;
    private Panel pnlSearchCriteria;
    private FlowLayoutPanel flpHeader;
    private Button btnSearch;
    private Button btnClear;
    private Panel panel1;
    private CollapsibleSplitter collapsibleSplitter1;
    private Label lblPoolID;
    private TextBox txtPoolID;
    private Label lblPoolNumber;
    private TextBox txtPoolNumber;
    private Label label20;
    private DatePicker dtSettlement;
    private ComboBox cmbTradeDesc;
    private Label label26;
    private Label label7;
    private TextBox txtInvestor;
    private StandardIconButton btnInvestorTemplate;
    private Label label6;
    private TextBox txtMinCoupon;
    private Label label4;
    private DatePicker dtCommitment;
    private Label label5;
    private TextBox txtMaxCoupon;

    public MbsPoolAssignmentDialog()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtMinAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtMaxAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtMinCoupon, TextBoxContentRule.NonNegativeDecimal, "#,##0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtMaxCoupon, TextBoxContentRule.NonNegativeDecimal, "#,##0.00000;;\"\"");
      this.LoadConfigurableFieldOptions();
    }

    public MbsPoolAssignmentDialog(ITradeEditorBase tradeEditor)
      : this()
    {
      this.tradeEditor = tradeEditor;
      this.ctlTradeList = !(tradeEditor is GseCommitmentEditor) ? new MbsPoolAssignmentListScreen(this.tradeEditor, true) : (MbsPoolAssignmentListScreen) new FannieMaePEMbsPoolAssignmentListScreen(this.tradeEditor, true);
      this.pnlTradeList.Controls.Clear();
      this.pnlTradeList.Controls.Add((Control) this.ctlTradeList);
      this.ctlTradeList.Dock = DockStyle.Fill;
      this.ctlTradeList.AssignedClicked += new EventHandler(this.ctlTradeList_AssignedClicked);
      this.InitForm();
    }

    private void InitForm()
    {
      if (!(this.tradeEditor is GseCommitmentEditor))
        return;
      this.Text = "Allocate Fannie Mae PE MBS Pools";
      this.lblHelp.Text = "You may allocate multiple Fannie Mae PE MBS pools to GSE Commitment";
    }

    public bool DataModified => this.ctlTradeList.DataModified;

    public TradeAssignmentByTradeBase[] GetCurrentAssignments()
    {
      return this.ctlTradeList.GetCurrentAssignments();
    }

    public TradeAssignmentByTradeBase[] GetEligibleAssignments()
    {
      return this.ctlTradeList.GetEligibleAssignments();
    }

    public void RefreshData(
      TradeAssignmentByTradeBase[] currentAssignments,
      TradeAssignmentByTradeBase[] eligibleAssignments)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.ctlTradeList.RefreshData(currentAssignments, eligibleAssignments, this.filters);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void appendCriteria()
    {
      this.filters = (FieldFilterList) null;
      this.appendCriteria(FieldTypes.IsString, "MbsPoolDetails.Name", this.txtPoolID.Text);
      this.appendCriteria(FieldTypes.IsNumeric, "MbsPoolDetails.TradeAmount", this.txtMinAmount.Text, this.txtMaxAmount.Text, OperatorTypes.Between);
      this.appendCriteria(FieldTypes.IsNumeric, "MbsPoolDetails.Coupon", this.txtMinCoupon.Text, this.txtMaxCoupon.Text, OperatorTypes.Between);
      this.appendCriteria(FieldTypes.IsString, "MbsPoolDetails.PoolNumber", this.txtPoolNumber.Text);
      this.appendCriteria(FieldTypes.IsString, "MbsPoolDetails.TradeDescription", this.cmbTradeDesc.Text);
      this.appendCriteria(FieldTypes.IsString, "MbsPoolDetails.InvestorName", this.txtInvestor.Text);
      this.appendCriteria(FieldTypes.IsDate, "MbsPoolDetails.SettlementDate", this.dtSettlement.Text);
      this.appendCriteria(FieldTypes.IsDate, "MbsPoolDetails.CommitmentDate", this.dtCommitment.Text);
      if (!(this.tradeEditor is GseCommitmentEditor))
        return;
      this.appendCriteria(FieldTypes.IsNumeric, "MbsPoolDetails.PoolMortgageType", 4.ToString());
    }

    private void appendCriteria(
      FieldTypes fieldType,
      string fieldName,
      string valueFrom,
      string valueTo = null,
      OperatorTypes operatorType = OperatorTypes.IsExact)
    {
      if (string.IsNullOrEmpty(valueFrom))
        return;
      if (this.filters == null)
        this.filters = new FieldFilterList();
      FieldFilter fieldFilter = !string.IsNullOrEmpty(valueTo) ? new FieldFilter(fieldType, fieldName, fieldName, fieldName, operatorType, valueFrom, valueTo) : new FieldFilter(fieldType, fieldName, fieldName, fieldName, operatorType, valueFrom);
      bool flag = false;
      for (int index = 0; index < this.filters.Count; ++index)
      {
        if (this.filters[index].CriterionName == fieldName)
        {
          this.filters[index] = fieldFilter;
          flag = true;
        }
      }
      if (!flag)
        this.filters.Add(fieldFilter);
      for (int index = 0; index < this.filters.Count - 1; ++index)
      {
        if (this.filters[index].JointToken == JointTokens.Nothing)
          this.filters[index].JointToken = JointTokens.And;
      }
    }

    private void LoadConfigurableFieldOptions()
    {
      this.cmbTradeDesc.Items.Clear();
      ArrayList secondaryFields = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption);
      if (secondaryFields == null)
        return;
      foreach (string str in secondaryFields)
        this.cmbTradeDesc.Items.Add((object) str);
    }

    private void onControlValueChanges(object sender, EventArgs e)
    {
    }

    private void validateTradeAmountRange(object sender, CancelEventArgs e)
    {
      if (this.txtMaxAmount.Text == "" || this.txtMinAmount.Text == "" || Utils.ParseDouble((object) this.txtMaxAmount.Text) >= Utils.ParseDouble((object) this.txtMinAmount.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum pool amount must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    private void validateCouponRange(object sender, CancelEventArgs e)
    {
      if (this.txtMinCoupon.Text == "")
        this.txtMinCoupon.Text = "0";
      if (this.txtMaxCoupon.Text == "" || this.txtMinCoupon.Text == "" || Utils.ParseDouble((object) this.txtMaxCoupon.Text) >= Utils.ParseDouble((object) this.txtMinCoupon.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum coupon must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    private void ctlTradeList_AssignedClicked(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.appendCriteria();
      this.RefreshData(this.ctlTradeList.GetCurrentAssignments(), this.ctlTradeList.GetEligibleAssignments());
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.filters = (FieldFilterList) null;
      foreach (object control in (ArrangedElementCollection) this.pnlSearchCriteria.Controls)
      {
        switch (control)
        {
          case TextBox _:
            ((Control) control).Text = "";
            continue;
          case DatePicker _:
            ((Control) control).Text = "";
            continue;
          case ComboBox _:
            ((Control) control).Text = "";
            continue;
          default:
            continue;
        }
      }
      this.RefreshData(this.ctlTradeList.GetCurrentAssignments(), this.ctlTradeList.GetEligibleAssignments());
    }

    private void btnInvestorTemplate_Click(object sender, EventArgs e)
    {
      using (InvestorTemplateSelector templateSelector = new InvestorTemplateSelector(true))
      {
        if (templateSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        InvestorTemplate selectedTemplate = templateSelector.SelectedTemplate;
        if (selectedTemplate == null)
          return;
        this.txtInvestor.Text = selectedTemplate.CompanyInformation.Name;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlTop = new Panel();
      this.lblHelp = new Label();
      this.pnlTradeList = new Panel();
      this.pnlSearch = new Panel();
      this.grpHeader = new GroupContainer();
      this.flpHeader = new FlowLayoutPanel();
      this.btnClear = new Button();
      this.btnSearch = new Button();
      this.pnlSearchCriteria = new Panel();
      this.label5 = new Label();
      this.txtMaxCoupon = new TextBox();
      this.label4 = new Label();
      this.dtCommitment = new DatePicker();
      this.label6 = new Label();
      this.txtMinCoupon = new TextBox();
      this.label7 = new Label();
      this.txtInvestor = new TextBox();
      this.btnInvestorTemplate = new StandardIconButton();
      this.cmbTradeDesc = new ComboBox();
      this.label26 = new Label();
      this.label20 = new Label();
      this.dtSettlement = new DatePicker();
      this.lblPoolNumber = new Label();
      this.txtPoolNumber = new TextBox();
      this.lblPoolID = new Label();
      this.txtPoolID = new TextBox();
      this.label2 = new Label();
      this.txtMaxAmount = new TextBox();
      this.txtMinAmount = new TextBox();
      this.label3 = new Label();
      this.panel1 = new Panel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlTop.SuspendLayout();
      this.pnlSearch.SuspendLayout();
      this.grpHeader.SuspendLayout();
      this.flpHeader.SuspendLayout();
      this.pnlSearchCriteria.SuspendLayout();
      ((ISupportInitialize) this.btnInvestorTemplate).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlTop.Controls.Add((Control) this.lblHelp);
      this.pnlTop.Dock = DockStyle.Top;
      this.pnlTop.Location = new Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Size = new Size(684, 29);
      this.pnlTop.TabIndex = 0;
      this.lblHelp.AutoSize = true;
      this.lblHelp.Location = new Point(4, 7);
      this.lblHelp.Name = "lblHelp";
      this.lblHelp.Size = new Size(291, 13);
      this.lblHelp.TabIndex = 0;
      this.lblHelp.Text = "You may allocate multiple pool commitments to security trade";
      this.pnlTradeList.Dock = DockStyle.Fill;
      this.pnlTradeList.Location = new Point(0, 151);
      this.pnlTradeList.Name = "pnlTradeList";
      this.pnlTradeList.Size = new Size(684, 196);
      this.pnlTradeList.TabIndex = 1;
      this.pnlSearch.Controls.Add((Control) this.grpHeader);
      this.pnlSearch.Dock = DockStyle.Top;
      this.pnlSearch.Location = new Point(0, 0);
      this.pnlSearch.Name = "pnlSearch";
      this.pnlSearch.Size = new Size(684, 144);
      this.pnlSearch.TabIndex = 2;
      this.grpHeader.Controls.Add((Control) this.flpHeader);
      this.grpHeader.Controls.Add((Control) this.pnlSearchCriteria);
      this.grpHeader.Dock = DockStyle.Fill;
      this.grpHeader.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpHeader.HeaderForeColor = SystemColors.ControlText;
      this.grpHeader.Location = new Point(0, 0);
      this.grpHeader.Name = "grpHeader";
      this.grpHeader.Size = new Size(684, 144);
      this.grpHeader.TabIndex = 4;
      this.grpHeader.Text = "Filter";
      this.flpHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpHeader.BackColor = Color.Transparent;
      this.flpHeader.Controls.Add((Control) this.btnClear);
      this.flpHeader.Controls.Add((Control) this.btnSearch);
      this.flpHeader.FlowDirection = FlowDirection.RightToLeft;
      this.flpHeader.Location = new Point(206, 2);
      this.flpHeader.Name = "flpHeader";
      this.flpHeader.Size = new Size(475, 22);
      this.flpHeader.TabIndex = 1;
      this.btnClear.Location = new Point(400, 0);
      this.btnClear.Margin = new Padding(0);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(75, 23);
      this.btnClear.TabIndex = 1;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.btnSearch.Location = new Point(325, 0);
      this.btnSearch.Margin = new Padding(0);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(75, 23);
      this.btnSearch.TabIndex = 0;
      this.btnSearch.Text = "Filter";
      this.btnSearch.TextAlign = ContentAlignment.TopCenter;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.pnlSearchCriteria.Controls.Add((Control) this.label5);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMaxCoupon);
      this.pnlSearchCriteria.Controls.Add((Control) this.label4);
      this.pnlSearchCriteria.Controls.Add((Control) this.dtCommitment);
      this.pnlSearchCriteria.Controls.Add((Control) this.label6);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMinCoupon);
      this.pnlSearchCriteria.Controls.Add((Control) this.label7);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtInvestor);
      this.pnlSearchCriteria.Controls.Add((Control) this.btnInvestorTemplate);
      this.pnlSearchCriteria.Controls.Add((Control) this.cmbTradeDesc);
      this.pnlSearchCriteria.Controls.Add((Control) this.label26);
      this.pnlSearchCriteria.Controls.Add((Control) this.label20);
      this.pnlSearchCriteria.Controls.Add((Control) this.dtSettlement);
      this.pnlSearchCriteria.Controls.Add((Control) this.lblPoolNumber);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtPoolNumber);
      this.pnlSearchCriteria.Controls.Add((Control) this.lblPoolID);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtPoolID);
      this.pnlSearchCriteria.Controls.Add((Control) this.label2);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMaxAmount);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMinAmount);
      this.pnlSearchCriteria.Controls.Add((Control) this.label3);
      this.pnlSearchCriteria.Dock = DockStyle.Fill;
      this.pnlSearchCriteria.Location = new Point(1, 26);
      this.pnlSearchCriteria.Name = "pnlSearchCriteria";
      this.pnlSearchCriteria.Size = new Size(682, 117);
      this.pnlSearchCriteria.TabIndex = 0;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(217, 60);
      this.label5.Name = "label5";
      this.label5.Size = new Size(11, 14);
      this.label5.TabIndex = 203;
      this.label5.Text = "-";
      this.txtMaxCoupon.Location = new Point(231, 57);
      this.txtMaxCoupon.MaxLength = 170;
      this.txtMaxCoupon.Name = "txtMaxCoupon";
      this.txtMaxCoupon.Size = new Size(100, 20);
      this.txtMaxCoupon.TabIndex = 130;
      this.txtMaxCoupon.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxCoupon.Validating += new CancelEventHandler(this.validateCouponRange);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(343, 79);
      this.label4.Name = "label4";
      this.label4.Size = new Size(89, 14);
      this.label4.TabIndex = 200;
      this.label4.Text = "Commitment Date";
      this.dtCommitment.BackColor = SystemColors.Window;
      this.dtCommitment.Location = new Point(457, 78);
      this.dtCommitment.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtCommitment.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtCommitment.Name = "dtCommitment";
      this.dtCommitment.Size = new Size(104, 22);
      this.dtCommitment.TabIndex = 190;
      this.dtCommitment.ToolTip = "";
      this.dtCommitment.Value = new DateTime(0L);
      this.dtCommitment.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(11, 62);
      this.label6.Name = "label6";
      this.label6.Size = new Size(44, 14);
      this.label6.TabIndex = 198;
      this.label6.Text = "Coupon";
      this.txtMinCoupon.Location = new Point(111, 57);
      this.txtMinCoupon.MaxLength = 170;
      this.txtMinCoupon.Name = "txtMinCoupon";
      this.txtMinCoupon.Size = new Size(100, 20);
      this.txtMinCoupon.TabIndex = 120;
      this.txtMinCoupon.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinCoupon.Validating += new CancelEventHandler(this.validateCouponRange);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(343, 30);
      this.label7.Name = "label7";
      this.label7.Size = new Size(46, 14);
      this.label7.TabIndex = 197;
      this.label7.Text = "Investor";
      this.txtInvestor.Location = new Point(457, 32);
      this.txtInvestor.MaxLength = 64;
      this.txtInvestor.Name = "txtInvestor";
      this.txtInvestor.Size = new Size(117, 20);
      this.txtInvestor.TabIndex = 170;
      this.txtInvestor.TextChanged += new EventHandler(this.onControlValueChanges);
      this.btnInvestorTemplate.BackColor = Color.Transparent;
      this.btnInvestorTemplate.Location = new Point(579, 33);
      this.btnInvestorTemplate.MouseDownImage = (Image) null;
      this.btnInvestorTemplate.Name = "btnInvestorTemplate";
      this.btnInvestorTemplate.Size = new Size(16, 16);
      this.btnInvestorTemplate.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnInvestorTemplate.TabIndex = 196;
      this.btnInvestorTemplate.TabStop = false;
      this.btnInvestorTemplate.Click += new EventHandler(this.btnInvestorTemplate_Click);
      this.cmbTradeDesc.FormattingEnabled = true;
      this.cmbTradeDesc.Location = new Point(457, 8);
      this.cmbTradeDesc.Name = "cmbTradeDesc";
      this.cmbTradeDesc.Size = new Size(139, 22);
      this.cmbTradeDesc.TabIndex = 160;
      this.label26.AutoSize = true;
      this.label26.Location = new Point(343, 9);
      this.label26.Name = "label26";
      this.label26.Size = new Size(92, 14);
      this.label26.TabIndex = 194;
      this.label26.Text = "Trade Description";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(343, 53);
      this.label20.Name = "label20";
      this.label20.Size = new Size(82, 14);
      this.label20.TabIndex = 191;
      this.label20.Text = "Settlement Date";
      this.dtSettlement.BackColor = SystemColors.Window;
      this.dtSettlement.Location = new Point(457, 54);
      this.dtSettlement.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtSettlement.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtSettlement.Name = "dtSettlement";
      this.dtSettlement.Size = new Size(104, 22);
      this.dtSettlement.TabIndex = 180;
      this.dtSettlement.ToolTip = "";
      this.dtSettlement.Value = new DateTime(0L);
      this.dtSettlement.TextChanged += new EventHandler(this.onControlValueChanges);
      this.lblPoolNumber.AutoSize = true;
      this.lblPoolNumber.Location = new Point(11, 38);
      this.lblPoolNumber.Name = "lblPoolNumber";
      this.lblPoolNumber.Size = new Size(67, 14);
      this.lblPoolNumber.TabIndex = 63;
      this.lblPoolNumber.Text = "Pool Number";
      this.txtPoolNumber.Location = new Point(111, 33);
      this.txtPoolNumber.MaxLength = 6;
      this.txtPoolNumber.Name = "txtPoolNumber";
      this.txtPoolNumber.Size = new Size(100, 20);
      this.txtPoolNumber.TabIndex = 110;
      this.txtPoolNumber.TextChanged += new EventHandler(this.onControlValueChanges);
      this.lblPoolID.AutoSize = true;
      this.lblPoolID.Location = new Point(11, 15);
      this.lblPoolID.Name = "lblPoolID";
      this.lblPoolID.Size = new Size(39, 14);
      this.lblPoolID.TabIndex = 11;
      this.lblPoolID.Text = "Pool ID";
      this.txtPoolID.Location = new Point(111, 10);
      this.txtPoolID.MaxLength = 64;
      this.txtPoolID.Name = "txtPoolID";
      this.txtPoolID.Size = new Size(138, 20);
      this.txtPoolID.TabIndex = 100;
      this.txtPoolID.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 85);
      this.label2.Name = "label2";
      this.label2.Size = new Size(66, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Pool Amount";
      this.txtMaxAmount.Location = new Point(231, 80);
      this.txtMaxAmount.Name = "txtMaxAmount";
      this.txtMaxAmount.Size = new Size(100, 20);
      this.txtMaxAmount.TabIndex = 150;
      this.txtMaxAmount.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxAmount.Validating += new CancelEventHandler(this.validateTradeAmountRange);
      this.txtMinAmount.Location = new Point(111, 80);
      this.txtMinAmount.Name = "txtMinAmount";
      this.txtMinAmount.Size = new Size(100, 20);
      this.txtMinAmount.TabIndex = 140;
      this.txtMinAmount.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinAmount.Validating += new CancelEventHandler(this.validateTradeAmountRange);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(217, 80);
      this.label3.Name = "label3";
      this.label3.Size = new Size(11, 14);
      this.label3.TabIndex = 2;
      this.label3.Text = "-";
      this.panel1.Controls.Add((Control) this.pnlTradeList);
      this.panel1.Controls.Add((Control) this.collapsibleSplitter1);
      this.panel1.Controls.Add((Control) this.pnlSearch);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 29);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(684, 347);
      this.panel1.TabIndex = 4;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlSearch;
      this.collapsibleSplitter1.Cursor = Cursors.HSplit;
      this.collapsibleSplitter1.Dock = DockStyle.Top;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 144);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 3;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(684, 376);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pnlTop);
      this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
      this.Name = nameof (MbsPoolAssignmentDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Allocate MBS Pools";
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.pnlSearch.ResumeLayout(false);
      this.grpHeader.ResumeLayout(false);
      this.flpHeader.ResumeLayout(false);
      this.pnlSearchCriteria.ResumeLayout(false);
      this.pnlSearchCriteria.PerformLayout();
      ((ISupportInitialize) this.btnInvestorTemplate).EndInit();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
