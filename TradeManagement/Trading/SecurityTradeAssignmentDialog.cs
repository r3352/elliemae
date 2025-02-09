// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeAssignmentDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class SecurityTradeAssignmentDialog : Form
  {
    private ITradeEditorBase tradeEditor;
    private SecurityTradeAssignmentListScreen ctlTradeList;
    private FieldFilterList filters;
    private IContainer components;
    private Panel pnlTop;
    private Label label1;
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
    private Label lblSecurityID;
    private TextBox txtSecurityID;
    private Label label8;
    private ComboBox cboTradeDescription;
    private Label label13;
    private Label label14;
    private ComboBox cboSecurityType;
    private Label label19;
    private Label label18;
    private TextBox txtTerm2;
    private Label label16;
    private TextBox txtTerm1;
    private Label label5;
    private TextBox txtMaxCoupon;
    private Label label6;
    private TextBox txtMinCoupon;
    private Label label21;
    private Label label4;
    private DatePicker dtSettlementDate;
    private Label label7;
    private TextBox txtDealer;
    private Label label15;
    private ComboBox cboProgramType;
    private Label label9;
    private TextBox txtMaxPrice;
    private TextBox txtMinPrice;
    private ComboBox cboCommitmentType;

    public SecurityTradeAssignmentDialog()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtTerm1, TextBoxContentRule.NonNegativeInteger, "##0");
      TextBoxFormatter.Attach(this.txtTerm2, TextBoxContentRule.NonNegativeInteger, "##0");
      TextBoxFormatter.Attach(this.txtMinCoupon, TextBoxContentRule.NonNegativeDecimal, "#,##0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtMaxCoupon, TextBoxContentRule.NonNegativeDecimal, "#,##0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtMaxPrice, TextBoxContentRule.NonNegativeDecimal, "#,##0.0000000;;\"\"");
      TextBoxFormatter.Attach(this.txtMaxPrice, TextBoxContentRule.NonNegativeDecimal, "#,##0.0000000;;\"\"");
      TextBoxFormatter.Attach(this.txtMinAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtMaxAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      this.refreshConfigurableFieldOptions();
    }

    public SecurityTradeAssignmentDialog(ITradeEditorBase tradeEditor)
      : this()
    {
      this.tradeEditor = tradeEditor;
      this.ctlTradeList = new SecurityTradeAssignmentListScreen(this.tradeEditor, true);
      this.pnlTradeList.Controls.Clear();
      this.pnlTradeList.Controls.Add((Control) this.ctlTradeList);
      this.ctlTradeList.Dock = DockStyle.Fill;
      this.ctlTradeList.AssignedClicked += new EventHandler(this.ctlTradeList_AssignedClicked);
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

    private void refreshConfigurableFieldOptions()
    {
      this.cboCommitmentType.Items.Clear();
      ArrayList secondaryFields1 = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.CommitmentTypeOption);
      if (secondaryFields1 != null)
      {
        foreach (string str in secondaryFields1)
          this.cboCommitmentType.Items.Add((object) str);
      }
      this.cboTradeDescription.Items.Clear();
      ArrayList secondaryFields2 = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption);
      if (secondaryFields2 != null)
      {
        foreach (string str in secondaryFields2)
          this.cboTradeDescription.Items.Add((object) str);
      }
      this.cboSecurityType.Items.Clear();
      foreach (DataRow row in (InternalDataCollectionBase) Session.ConfigurationManager.GetSecondarySecurityTypes().Rows)
        this.cboSecurityType.Items.Add(row["Name"]);
    }

    private void appendCriteria()
    {
      this.filters = (FieldFilterList) null;
      this.appendCriteria(FieldTypes.IsString, "SecurityTradeDetails.Name", this.txtSecurityID.Text);
      this.appendCriteria(FieldTypes.IsString, "SecurityTradeDetails.CommitmentType", this.cboCommitmentType.Text);
      this.appendCriteria(FieldTypes.IsString, "SecurityTradeDetails.TradeDescription", this.cboTradeDescription.Text);
      this.appendCriteria(FieldTypes.IsString, "SecurityTradeDetails.SecurityType", this.cboSecurityType.Text);
      this.appendCriteria(FieldTypes.IsString, "SecurityTradeDetails.ProgramType", this.cboProgramType.Text);
      this.appendCriteria(FieldTypes.IsNumeric, "SecurityTradeDetails.Term1", this.txtTerm1.Text);
      this.appendCriteria(FieldTypes.IsNumeric, "SecurityTradeDetails.Term2", this.txtTerm2.Text);
      this.appendCriteria(FieldTypes.IsNumeric, "SecurityTradeDetails.TradeAmount", this.txtMinAmount.Text, this.txtMaxAmount.Text, OperatorTypes.Between);
      this.appendCriteria(FieldTypes.IsNumeric, "SecurityTradeDetails.Coupon", this.txtMinCoupon.Text, this.txtMaxCoupon.Text, OperatorTypes.Between);
      this.appendCriteria(FieldTypes.IsNumeric, "SecurityTradeDetails.Price", this.txtMinPrice.Text, this.txtMaxPrice.Text, OperatorTypes.Between);
      this.appendCriteria(FieldTypes.IsDate, "SecurityTradeDetails.SettlementDate", this.dtSettlementDate.Text);
      this.appendCriteria(FieldTypes.IsString, "SecurityTradeDetails.DealerName", this.txtDealer.Text);
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

    private void onControlValueChanges(object sender, EventArgs e)
    {
    }

    private void validateTradeAmountRange(object sender, CancelEventArgs e)
    {
      if (this.txtMinAmount.Text == "")
        this.txtMinAmount.Text = "0";
      if (this.txtMaxAmount.Text == "" || this.txtMinAmount.Text == "" || Utils.ParseDouble((object) this.txtMaxAmount.Text) >= Utils.ParseDouble((object) this.txtMinAmount.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum trade amount must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

    private void validatePriceRange(object sender, CancelEventArgs e)
    {
      if (this.txtMinPrice.Text == "")
        this.txtMinPrice.Text = "0";
      if (this.txtMaxPrice.Text == "" || this.txtMinPrice.Text == "" || Utils.ParseDouble((object) this.txtMaxPrice.Text) >= Utils.ParseDouble((object) this.txtMinPrice.Text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This minimum price must be less than or equal to the maximum.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      e.Cancel = true;
    }

    private void cboSecurityType_SelectedIndexChanged(object sender, EventArgs e)
    {
      foreach (DataRow row in (InternalDataCollectionBase) Session.ConfigurationManager.GetSecondarySecurityTypes().Rows)
      {
        if (this.cboSecurityType.Text == (string) row["Name"])
        {
          if (!string.IsNullOrEmpty((string) row["ProgramType"]))
            this.cboProgramType.Text = (string) row["ProgramType"];
          if ((int) row["Term1"] != -1)
            this.txtTerm1.Text = row["Term1"].ToString();
          if ((int) row["Term2"] != -1)
            this.txtTerm2.Text = row["Term2"].ToString();
        }
      }
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlTop = new Panel();
      this.label1 = new Label();
      this.pnlTradeList = new Panel();
      this.pnlSearch = new Panel();
      this.grpHeader = new GroupContainer();
      this.flpHeader = new FlowLayoutPanel();
      this.btnClear = new Button();
      this.btnSearch = new Button();
      this.pnlSearchCriteria = new Panel();
      this.cboCommitmentType = new ComboBox();
      this.label9 = new Label();
      this.txtMaxPrice = new TextBox();
      this.txtMinPrice = new TextBox();
      this.label15 = new Label();
      this.cboProgramType = new ComboBox();
      this.label7 = new Label();
      this.txtDealer = new TextBox();
      this.label4 = new Label();
      this.dtSettlementDate = new DatePicker();
      this.label21 = new Label();
      this.label5 = new Label();
      this.txtMaxCoupon = new TextBox();
      this.label6 = new Label();
      this.txtMinCoupon = new TextBox();
      this.label19 = new Label();
      this.label18 = new Label();
      this.txtTerm2 = new TextBox();
      this.label16 = new Label();
      this.txtTerm1 = new TextBox();
      this.label14 = new Label();
      this.cboSecurityType = new ComboBox();
      this.cboTradeDescription = new ComboBox();
      this.label13 = new Label();
      this.label8 = new Label();
      this.lblSecurityID = new Label();
      this.txtSecurityID = new TextBox();
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
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlTop.Controls.Add((Control) this.label1);
      this.pnlTop.Dock = DockStyle.Top;
      this.pnlTop.Location = new Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Size = new Size(684, 29);
      this.pnlTop.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 7);
      this.label1.Name = "label1";
      this.label1.Size = new Size(284, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "You may assign multiple security trades to pool commitment";
      this.pnlTradeList.Dock = DockStyle.Fill;
      this.pnlTradeList.Location = new Point(0, 207);
      this.pnlTradeList.Name = "pnlTradeList";
      this.pnlTradeList.Size = new Size(684, 340);
      this.pnlTradeList.TabIndex = 1;
      this.pnlSearch.Controls.Add((Control) this.grpHeader);
      this.pnlSearch.Dock = DockStyle.Top;
      this.pnlSearch.Location = new Point(0, 0);
      this.pnlSearch.Name = "pnlSearch";
      this.pnlSearch.Size = new Size(684, 200);
      this.pnlSearch.TabIndex = 2;
      this.grpHeader.Controls.Add((Control) this.flpHeader);
      this.grpHeader.Controls.Add((Control) this.pnlSearchCriteria);
      this.grpHeader.Dock = DockStyle.Fill;
      this.grpHeader.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpHeader.HeaderForeColor = SystemColors.ControlText;
      this.grpHeader.Location = new Point(0, 0);
      this.grpHeader.Name = "grpHeader";
      this.grpHeader.Size = new Size(684, 200);
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
      this.pnlSearchCriteria.Controls.Add((Control) this.cboCommitmentType);
      this.pnlSearchCriteria.Controls.Add((Control) this.label9);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMaxPrice);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMinPrice);
      this.pnlSearchCriteria.Controls.Add((Control) this.label15);
      this.pnlSearchCriteria.Controls.Add((Control) this.cboProgramType);
      this.pnlSearchCriteria.Controls.Add((Control) this.label7);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtDealer);
      this.pnlSearchCriteria.Controls.Add((Control) this.label4);
      this.pnlSearchCriteria.Controls.Add((Control) this.dtSettlementDate);
      this.pnlSearchCriteria.Controls.Add((Control) this.label21);
      this.pnlSearchCriteria.Controls.Add((Control) this.label5);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMaxCoupon);
      this.pnlSearchCriteria.Controls.Add((Control) this.label6);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMinCoupon);
      this.pnlSearchCriteria.Controls.Add((Control) this.label19);
      this.pnlSearchCriteria.Controls.Add((Control) this.label18);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtTerm2);
      this.pnlSearchCriteria.Controls.Add((Control) this.label16);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtTerm1);
      this.pnlSearchCriteria.Controls.Add((Control) this.label14);
      this.pnlSearchCriteria.Controls.Add((Control) this.cboSecurityType);
      this.pnlSearchCriteria.Controls.Add((Control) this.cboTradeDescription);
      this.pnlSearchCriteria.Controls.Add((Control) this.label13);
      this.pnlSearchCriteria.Controls.Add((Control) this.label8);
      this.pnlSearchCriteria.Controls.Add((Control) this.lblSecurityID);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtSecurityID);
      this.pnlSearchCriteria.Controls.Add((Control) this.label2);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMaxAmount);
      this.pnlSearchCriteria.Controls.Add((Control) this.txtMinAmount);
      this.pnlSearchCriteria.Controls.Add((Control) this.label3);
      this.pnlSearchCriteria.Dock = DockStyle.Fill;
      this.pnlSearchCriteria.Location = new Point(1, 26);
      this.pnlSearchCriteria.Name = "pnlSearchCriteria";
      this.pnlSearchCriteria.Size = new Size(682, 173);
      this.pnlSearchCriteria.TabIndex = 0;
      this.cboCommitmentType.FormattingEnabled = true;
      this.cboCommitmentType.Location = new Point(117, 28);
      this.cboCommitmentType.Name = "cboCommitmentType";
      this.cboCommitmentType.Size = new Size(161, 22);
      this.cboCommitmentType.TabIndex = 254;
      this.cboCommitmentType.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(563, 54);
      this.label9.Name = "label9";
      this.label9.Size = new Size(11, 14);
      this.label9.TabIndex = 252;
      this.label9.Text = "-";
      this.txtMaxPrice.Location = new Point(577, 51);
      this.txtMaxPrice.MaxLength = 170;
      this.txtMaxPrice.Name = "txtMaxPrice";
      this.txtMaxPrice.Size = new Size(100, 20);
      this.txtMaxPrice.TabIndex = 253;
      this.txtMaxPrice.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxPrice.Validating += new CancelEventHandler(this.validatePriceRange);
      this.txtMinPrice.Location = new Point(457, 51);
      this.txtMinPrice.MaxLength = 170;
      this.txtMinPrice.Name = "txtMinPrice";
      this.txtMinPrice.Size = new Size(100, 20);
      this.txtMinPrice.TabIndex = 251;
      this.txtMinPrice.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinPrice.Validating += new CancelEventHandler(this.validatePriceRange);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(12, 102);
      this.label15.Name = "label15";
      this.label15.Size = new Size(73, 14);
      this.label15.TabIndex = 214;
      this.label15.Text = "Program Type";
      this.cboProgramType.FormattingEnabled = true;
      this.cboProgramType.Items.AddRange(new object[2]
      {
        (object) "Fixed",
        (object) "Adjustable"
      });
      this.cboProgramType.Location = new Point(118, 100);
      this.cboProgramType.Name = "cboProgramType";
      this.cboProgramType.Size = new Size(161, 22);
      this.cboProgramType.TabIndex = 150;
      this.cboProgramType.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(352, 102);
      this.label7.Name = "label7";
      this.label7.Size = new Size(38, 14);
      this.label7.TabIndex = 213;
      this.label7.Text = "Dealer";
      this.txtDealer.Location = new Point(457, 99);
      this.txtDealer.MaxLength = 64;
      this.txtDealer.Name = "txtDealer";
      this.txtDealer.Size = new Size(117, 20);
      this.txtDealer.TabIndex = 250;
      this.txtDealer.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(352, 79);
      this.label4.Name = "label4";
      this.label4.Size = new Size(82, 14);
      this.label4.TabIndex = 210;
      this.label4.Text = "Settlement Date";
      this.dtSettlementDate.BackColor = SystemColors.Window;
      this.dtSettlementDate.Location = new Point(457, 74);
      this.dtSettlementDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtSettlementDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtSettlementDate.Name = "dtSettlementDate";
      this.dtSettlementDate.Size = new Size(104, 22);
      this.dtSettlementDate.TabIndex = 240;
      this.dtSettlementDate.ToolTip = "";
      this.dtSettlementDate.Value = new DateTime(0L);
      this.dtSettlementDate.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label21.AutoSize = true;
      this.label21.Location = new Point(352, 56);
      this.label21.Name = "label21";
      this.label21.Size = new Size(31, 14);
      this.label21.TabIndex = 208;
      this.label21.Text = "Price";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(563, 32);
      this.label5.Name = "label5";
      this.label5.Size = new Size(11, 14);
      this.label5.TabIndex = 207;
      this.label5.Text = "-";
      this.txtMaxCoupon.Location = new Point(577, 29);
      this.txtMaxCoupon.MaxLength = 170;
      this.txtMaxCoupon.Name = "txtMaxCoupon";
      this.txtMaxCoupon.Size = new Size(100, 20);
      this.txtMaxCoupon.TabIndex = 210;
      this.txtMaxCoupon.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxCoupon.Validating += new CancelEventHandler(this.validateCouponRange);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(352, 33);
      this.label6.Name = "label6";
      this.label6.Size = new Size(44, 14);
      this.label6.TabIndex = 206;
      this.label6.Text = "Coupon";
      this.txtMinCoupon.Location = new Point(457, 29);
      this.txtMinCoupon.MaxLength = 170;
      this.txtMinCoupon.Name = "txtMinCoupon";
      this.txtMinCoupon.Size = new Size(100, 20);
      this.txtMinCoupon.TabIndex = 200;
      this.txtMinCoupon.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinCoupon.Validating += new CancelEventHandler(this.validateCouponRange);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(589, 9);
      this.label19.Name = "label19";
      this.label19.Size = new Size(30, 14);
      this.label19.TabIndex = 157;
      this.label19.Text = "mths";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(517, 8);
      this.label18.Name = "label18";
      this.label18.Size = new Size(11, 14);
      this.label18.TabIndex = 158;
      this.label18.Text = "-";
      this.txtTerm2.Location = new Point(528, 6);
      this.txtTerm2.MaxLength = 12;
      this.txtTerm2.Name = "txtTerm2";
      this.txtTerm2.Size = new Size(59, 20);
      this.txtTerm2.TabIndex = 190;
      this.txtTerm2.TextAlign = HorizontalAlignment.Right;
      this.txtTerm2.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(352, 12);
      this.label16.Name = "label16";
      this.label16.Size = new Size(30, 14);
      this.label16.TabIndex = 159;
      this.label16.Text = "Term";
      this.txtTerm1.Location = new Point(457, 6);
      this.txtTerm1.MaxLength = 12;
      this.txtTerm1.Name = "txtTerm1";
      this.txtTerm1.Size = new Size(59, 20);
      this.txtTerm1.TabIndex = 180;
      this.txtTerm1.TextAlign = HorizontalAlignment.Right;
      this.txtTerm1.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(11, 79);
      this.label14.Name = "label14";
      this.label14.Size = new Size(73, 14);
      this.label14.TabIndex = 155;
      this.label14.Text = "Security Type";
      this.cboSecurityType.FormattingEnabled = true;
      this.cboSecurityType.Location = new Point(117, 76);
      this.cboSecurityType.Name = "cboSecurityType";
      this.cboSecurityType.Size = new Size(161, 22);
      this.cboSecurityType.TabIndex = 140;
      this.cboSecurityType.SelectedIndexChanged += new EventHandler(this.cboSecurityType_SelectedIndexChanged);
      this.cboSecurityType.TextChanged += new EventHandler(this.onControlValueChanges);
      this.cboTradeDescription.FormattingEnabled = true;
      this.cboTradeDescription.Location = new Point(117, 52);
      this.cboTradeDescription.Name = "cboTradeDescription";
      this.cboTradeDescription.Size = new Size(161, 22);
      this.cboTradeDescription.TabIndex = 130;
      this.cboTradeDescription.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label13.AutoSize = true;
      this.label13.Location = new Point(11, 57);
      this.label13.Name = "label13";
      this.label13.Size = new Size(92, 14);
      this.label13.TabIndex = 153;
      this.label13.Text = "Trade Description";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(11, 33);
      this.label8.Name = "label8";
      this.label8.Size = new Size(90, 14);
      this.label8.TabIndex = 152;
      this.label8.Text = "Commitment Type";
      this.lblSecurityID.AutoSize = true;
      this.lblSecurityID.Location = new Point(11, 8);
      this.lblSecurityID.Name = "lblSecurityID";
      this.lblSecurityID.Size = new Size(59, 14);
      this.lblSecurityID.TabIndex = 101;
      this.lblSecurityID.Text = "Security ID";
      this.txtSecurityID.Location = new Point(117, 6);
      this.txtSecurityID.MaxLength = 64;
      this.txtSecurityID.Name = "txtSecurityID";
      this.txtSecurityID.Size = new Size(138, 20);
      this.txtSecurityID.TabIndex = 110;
      this.txtSecurityID.TextChanged += new EventHandler(this.onControlValueChanges);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 126);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Trade Amount";
      this.txtMaxAmount.Location = new Point(237, 125);
      this.txtMaxAmount.Name = "txtMaxAmount";
      this.txtMaxAmount.Size = new Size(100, 20);
      this.txtMaxAmount.TabIndex = 170;
      this.txtMaxAmount.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMaxAmount.Validating += new CancelEventHandler(this.validateTradeAmountRange);
      this.txtMinAmount.Location = new Point(117, 125);
      this.txtMinAmount.Name = "txtMinAmount";
      this.txtMinAmount.Size = new Size(100, 20);
      this.txtMinAmount.TabIndex = 160;
      this.txtMinAmount.TextChanged += new EventHandler(this.onControlValueChanges);
      this.txtMinAmount.Validating += new CancelEventHandler(this.validateTradeAmountRange);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(223, 125);
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
      this.panel1.Size = new Size(684, 547);
      this.panel1.TabIndex = 4;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlSearch;
      this.collapsibleSplitter1.Cursor = Cursors.HSplit;
      this.collapsibleSplitter1.Dock = DockStyle.Top;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 200);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 3;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(684, 576);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pnlTop);
      this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
      this.Name = nameof (SecurityTradeAssignmentDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Allocate Security Trades";
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.pnlSearch.ResumeLayout(false);
      this.grpHeader.ResumeLayout(false);
      this.flpHeader.ResumeLayout(false);
      this.pnlSearchCriteria.ResumeLayout(false);
      this.pnlSearchCriteria.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
