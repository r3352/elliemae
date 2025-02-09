// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeeValueDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.DynamicDataManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FeeValueDlg : Form
  {
    public TextBox valueText;
    public Label advancedCodingLabel;
    public ComboBox valueCombo;
    public DDMFeeRuleValue feeRuleValue;
    private string advancedCodeText = "";
    private string specificValueText = "";
    private string tableValueText = "";
    private Label listOfValuesLabel;
    private ListView listOfValuesListbox;
    private Button listOfValuesAddButton;
    private Button listOfValuesEditButton;
    private Button listOfValuesRemoveButton;
    private Panel grpOptionFieldControls;
    private List<CheckBox> chkOptionFieldItems;
    private FieldDefinition triggerField;
    private ZipCountyStateCtrl zipCountryStateControl;
    private string zipCounty = string.Empty;
    private FeeValueDlg.ZipCountyStateObj ZipCountyObj = new FeeValueDlg.ZipCountyStateObj();
    private readonly string feeLineNumber = string.Empty;
    private string dtValuewithPrefix = string.Empty;
    private DataTableList dataTable;
    private bool offsetUIForLockableField;
    private FeeValueDlg.CurrentState curState = FeeValueDlg.CurrentState.NoVal;
    public DataTableFieldValues DtFieldValues;
    private DataTableField _currentDataTableField;
    public StandardIconButton btnRolodex;
    public Button validationBtn;
    public FeeValueDlg.ddmTab ddmCurrentTab;
    public bool deleteEmptyRowInDataTable;
    private FeeManagementPersonaInfo feeManagementPermission;
    private FeeManagementSetting feeManagementSetting;
    private List<string> feeManagementdpItems = new List<string>();
    private bool IsFMfieldEditable;
    private FeeSectionEnum feesecEnum;
    private bool applyItemizationChecked;
    private FeeValueDlg.SpecificValueMode specificValueMode;
    private bool IsSystemTable;
    private string[] FeeFieldsCriterion = new string[5]
    {
      "No Value Set",
      "Specific Value",
      "Table",
      "Calculation",
      "Clear value in loan file"
    };
    private string[] DTNumberDateCriterion = new string[9]
    {
      "Ignore this field",
      "=",
      "<",
      "<=",
      ">",
      ">=",
      "<>",
      "Range",
      "No value in loan file"
    };
    private string[] DT_NonNumeric_Phone_Criterion = new string[8]
    {
      "Ignore this field",
      "Equals",
      "Does not equal",
      "Contains",
      "Does not contain",
      "Begins with",
      "Ends with",
      "No value in loan file"
    };
    private string[] DT_NonNumeric_Enumerated_Criterion = new string[5]
    {
      "Ignore this field",
      "Equals",
      "Does not equal",
      "List Of Values",
      "No value in loan file"
    };
    private string[] DT_SSN_Criterion = new string[3]
    {
      "Ignore this field",
      "Specific Value",
      "No value in loan file"
    };
    private string[] DT_YN_Criterion = new string[3]
    {
      "<Blank>",
      "Yes",
      "No"
    };
    private string[] DT_ZIPCODE_Criterion = new string[4]
    {
      "Ignore this field",
      "Specific Value",
      "Find Zip",
      "No value in loan file"
    };
    private string[] DT_COUNTY_Criterion = new string[4]
    {
      "Ignore this field",
      "Specific Value",
      "Find County",
      "No value in loan file"
    };
    private string[] SellerObligatedCriterion = new string[3]
    {
      "No Value Set",
      "Specific Value",
      "Clear value in loan file"
    };
    private string[] DT_OUTPUT_Criterion = new string[4]
    {
      "Please Select",
      "Specific Value",
      "Calculation",
      "Clear value in loan file"
    };
    private Sessions.Session session;
    private int Form_Mtrx_Height_Large = 575;
    private int Form_Mtrx_Height_Small = 326;
    private int GroupBox_Mtrx_Height_Large = 471;
    private int GroupBox_Mtrx_Height_Small = 226;
    private int Layout_Mtrx_Offset = 40;
    private FeeValueDlg.ValueControlMode currentValueControlMode;
    private IContainer components;
    private Label label1;
    private TextBox fieldIDTextBx;
    private Label label2;
    private TextBox fieldDescTextBx;
    private Label label3;
    private ComboBox valueTypeCombo;
    private Label value_lbl;
    private Button ok_btn;
    private Panel panel1;
    private Button cancelBtn;
    private bool firstLoad;
    private GroupBox groupBox1;
    private Panel panel2;
    private Panel nextPrevBtnPnl;
    private Button BtnLastItem;
    private Button BtnNextItem;
    private Button BtnPrevItem;
    private Button BtnFirstItem;
    private Panel numericDatePnl;
    private Panel pnlMultiSelect;
    private CheckedListBox lstChkYN;
    private Panel datePnl;
    private Panel dateRangePnl;
    private DatePicker dptxtMaxDateValue;
    private DatePicker dptxtMinDateValue;
    private Label label11;
    private Label label9;
    private Panel datevaluePnl;
    private DatePicker dptxtDateValue;
    private Label label8;
    private Panel numericPnl;
    private Panel numericRangePnl;
    private TextBox txtDataMaxValue;
    private TextBox txtDataMinValue;
    private Label label7;
    private Label label10;
    private Panel numericValuePnl;
    private TextBox txtDataValue;
    private Label label6;
    private Label label4;
    private Label label5;
    private TextBox textBoxPair;
    private Panel pnlOutput;
    private Label label12;
    private ComboBox outputCmb;
    private Label label13;
    private PanelEx pnlLockableField;
    private FieldLockButton fieldLockButton1;
    private Label label14;
    private CheckBox isLockablechk;
    private Panel pnlRuleOverride;
    private FieldLockButton fieldLockButton2;
    private Label label19;
    private Label label18;
    private Label overrideLabel;

    public bool IsDirty { get; set; }

    public bool IsFeeManagementField
    {
      get => this.feesecEnum != FeeSectionEnum.Nothing && this.applyItemizationChecked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.fieldIDTextBx = new TextBox();
      this.label2 = new Label();
      this.fieldDescTextBx = new TextBox();
      this.label3 = new Label();
      this.valueTypeCombo = new ComboBox();
      this.value_lbl = new Label();
      this.ok_btn = new Button();
      this.panel1 = new Panel();
      this.pnlMultiSelect = new Panel();
      this.label4 = new Label();
      this.lstChkYN = new CheckedListBox();
      this.numericDatePnl = new Panel();
      this.datePnl = new Panel();
      this.dateRangePnl = new Panel();
      this.dptxtMaxDateValue = new DatePicker();
      this.dptxtMinDateValue = new DatePicker();
      this.label11 = new Label();
      this.label9 = new Label();
      this.datevaluePnl = new Panel();
      this.dptxtDateValue = new DatePicker();
      this.label8 = new Label();
      this.numericPnl = new Panel();
      this.numericRangePnl = new Panel();
      this.txtDataMaxValue = new TextBox();
      this.txtDataMinValue = new TextBox();
      this.label7 = new Label();
      this.label10 = new Label();
      this.numericValuePnl = new Panel();
      this.txtDataValue = new TextBox();
      this.label6 = new Label();
      this.cancelBtn = new Button();
      this.groupBox1 = new GroupBox();
      this.pnlRuleOverride = new Panel();
      this.fieldLockButton2 = new FieldLockButton();
      this.label19 = new Label();
      this.label18 = new Label();
      this.overrideLabel = new Label();
      this.pnlLockableField = new PanelEx();
      this.fieldLockButton1 = new FieldLockButton();
      this.label14 = new Label();
      this.isLockablechk = new CheckBox();
      this.pnlOutput = new Panel();
      this.label13 = new Label();
      this.label12 = new Label();
      this.outputCmb = new ComboBox();
      this.label5 = new Label();
      this.textBoxPair = new TextBox();
      this.panel2 = new Panel();
      this.nextPrevBtnPnl = new Panel();
      this.BtnLastItem = new Button();
      this.BtnNextItem = new Button();
      this.BtnPrevItem = new Button();
      this.BtnFirstItem = new Button();
      this.panel1.SuspendLayout();
      this.pnlMultiSelect.SuspendLayout();
      this.numericDatePnl.SuspendLayout();
      this.datePnl.SuspendLayout();
      this.dateRangePnl.SuspendLayout();
      this.datevaluePnl.SuspendLayout();
      this.numericPnl.SuspendLayout();
      this.numericRangePnl.SuspendLayout();
      this.numericValuePnl.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.pnlRuleOverride.SuspendLayout();
      this.pnlLockableField.SuspendLayout();
      this.pnlOutput.SuspendLayout();
      this.nextPrevBtnPnl.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(35, 33);
      this.label1.Margin = new Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(41, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Field Id";
      this.fieldIDTextBx.Location = new Point(142, 26);
      this.fieldIDTextBx.Margin = new Padding(2);
      this.fieldIDTextBx.Name = "fieldIDTextBx";
      this.fieldIDTextBx.ReadOnly = true;
      this.fieldIDTextBx.Size = new Size(226, 20);
      this.fieldIDTextBx.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(35, 63);
      this.label2.Margin = new Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(85, 13);
      this.label2.TabIndex = 200;
      this.label2.Text = "Field Description";
      this.fieldDescTextBx.Location = new Point(142, 56);
      this.fieldDescTextBx.Margin = new Padding(2);
      this.fieldDescTextBx.Name = "fieldDescTextBx";
      this.fieldDescTextBx.ReadOnly = true;
      this.fieldDescTextBx.Size = new Size(226, 20);
      this.fieldDescTextBx.TabIndex = 2;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(35, 123);
      this.label3.Margin = new Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(61, 13);
      this.label3.TabIndex = 400;
      this.label3.Text = "Value Type";
      this.valueTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.valueTypeCombo.FormattingEnabled = true;
      this.valueTypeCombo.Location = new Point(142, 116);
      this.valueTypeCombo.Margin = new Padding(2);
      this.valueTypeCombo.Name = "valueTypeCombo";
      this.valueTypeCombo.Size = new Size(226, 21);
      this.valueTypeCombo.TabIndex = 0;
      this.valueTypeCombo.SelectedIndexChanged += new EventHandler(this.valueTypeCombo_SelectedIndexChanged);
      this.value_lbl.AutoSize = true;
      this.value_lbl.Location = new Point(35, 153);
      this.value_lbl.Margin = new Padding(2, 0, 2, 0);
      this.value_lbl.Name = "value_lbl";
      this.value_lbl.Size = new Size(34, 13);
      this.value_lbl.TabIndex = 6;
      this.value_lbl.Text = "Value";
      this.value_lbl.Visible = false;
      this.value_lbl.Click += new EventHandler(this.label4_Click);
      this.ok_btn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.ok_btn.Location = new Point(396, 449);
      this.ok_btn.Margin = new Padding(2);
      this.ok_btn.Name = "ok_btn";
      this.ok_btn.Size = new Size(75, 23);
      this.ok_btn.TabIndex = 5;
      this.ok_btn.Text = "OK";
      this.ok_btn.UseVisualStyleBackColor = true;
      this.ok_btn.Click += new EventHandler(this.ok_btn_Click);
      this.panel1.Controls.Add((Control) this.pnlMultiSelect);
      this.panel1.Controls.Add((Control) this.numericDatePnl);
      this.panel1.Location = new Point(36, 183);
      this.panel1.Margin = new Padding(2);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(485, 252);
      this.panel1.TabIndex = 4;
      this.pnlMultiSelect.Controls.Add((Control) this.label4);
      this.pnlMultiSelect.Controls.Add((Control) this.lstChkYN);
      this.pnlMultiSelect.Location = new Point(210, 8);
      this.pnlMultiSelect.Name = "pnlMultiSelect";
      this.pnlMultiSelect.Size = new Size(379, 208);
      this.pnlMultiSelect.TabIndex = 14;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(1, 3);
      this.label4.Name = "label4";
      this.label4.Size = new Size(34, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Value";
      this.lstChkYN.CheckOnClick = true;
      this.lstChkYN.FormattingEnabled = true;
      this.lstChkYN.Location = new Point(106, 4);
      this.lstChkYN.Name = "lstChkYN";
      this.lstChkYN.Size = new Size(226, 199);
      this.lstChkYN.TabIndex = 0;
      this.lstChkYN.SelectedValueChanged += new EventHandler(this.lstChkYN_SelectedValueChanged);
      this.numericDatePnl.Controls.Add((Control) this.datePnl);
      this.numericDatePnl.Controls.Add((Control) this.numericPnl);
      this.numericDatePnl.Location = new Point(0, 0);
      this.numericDatePnl.Name = "numericDatePnl";
      this.numericDatePnl.Size = new Size(500, 232);
      this.numericDatePnl.TabIndex = 15;
      this.datePnl.Controls.Add((Control) this.dateRangePnl);
      this.datePnl.Controls.Add((Control) this.datevaluePnl);
      this.datePnl.Location = new Point(0, 95);
      this.datePnl.Name = "datePnl";
      this.datePnl.Size = new Size(438, 82);
      this.datePnl.TabIndex = 1;
      this.dateRangePnl.Controls.Add((Control) this.dptxtMaxDateValue);
      this.dateRangePnl.Controls.Add((Control) this.dptxtMinDateValue);
      this.dateRangePnl.Controls.Add((Control) this.label11);
      this.dateRangePnl.Controls.Add((Control) this.label9);
      this.dateRangePnl.Location = new Point(124, 3);
      this.dateRangePnl.Name = "dateRangePnl";
      this.dateRangePnl.Size = new Size(385, 46);
      this.dateRangePnl.TabIndex = 8;
      this.dptxtMaxDateValue.BackColor = SystemColors.Window;
      this.dptxtMaxDateValue.Location = new Point(243, 0);
      this.dptxtMaxDateValue.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dptxtMaxDateValue.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dptxtMaxDateValue.Name = "dptxtMaxDateValue";
      this.dptxtMaxDateValue.Size = new Size(130, 21);
      this.dptxtMaxDateValue.TabIndex = 10;
      this.dptxtMaxDateValue.ToolTip = "";
      this.dptxtMaxDateValue.Value = new DateTime(0L);
      this.dptxtMaxDateValue.ValueChanged += new EventHandler(this.DateValue_ValueChanged);
      this.dptxtMinDateValue.BackColor = SystemColors.Window;
      this.dptxtMinDateValue.Location = new Point(106, 0);
      this.dptxtMinDateValue.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dptxtMinDateValue.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dptxtMinDateValue.Name = "dptxtMinDateValue";
      this.dptxtMinDateValue.Size = new Size(121, 21);
      this.dptxtMinDateValue.TabIndex = 9;
      this.dptxtMinDateValue.ToolTip = "";
      this.dptxtMinDateValue.Value = new DateTime(0L);
      this.dptxtMinDateValue.ValueChanged += new EventHandler(this.DateValue_ValueChanged);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(239, 21);
      this.label11.Name = "label11";
      this.label11.Size = new Size(123, 13);
      this.label11.TabIndex = 8;
      this.label11.Text = "(includes the given date)";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(-1, 7);
      this.label9.Name = "label9";
      this.label9.Size = new Size(34, 13);
      this.label9.TabIndex = 5;
      this.label9.Text = "Value";
      this.datevaluePnl.Controls.Add((Control) this.dptxtDateValue);
      this.datevaluePnl.Controls.Add((Control) this.label8);
      this.datevaluePnl.Location = new Point(0, 13);
      this.datevaluePnl.Name = "datevaluePnl";
      this.datevaluePnl.Size = new Size(379, 36);
      this.datevaluePnl.TabIndex = 5;
      this.dptxtDateValue.BackColor = SystemColors.Window;
      this.dptxtDateValue.Location = new Point(106, 0);
      this.dptxtDateValue.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dptxtDateValue.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dptxtDateValue.Name = "dptxtDateValue";
      this.dptxtDateValue.Size = new Size(121, 21);
      this.dptxtDateValue.TabIndex = 3;
      this.dptxtDateValue.ToolTip = "";
      this.dptxtDateValue.Value = new DateTime(0L);
      this.dptxtDateValue.ValueChanged += new EventHandler(this.DateValue_ValueChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(-1, 7);
      this.label8.Name = "label8";
      this.label8.Size = new Size(34, 13);
      this.label8.TabIndex = 2;
      this.label8.Text = "Value";
      this.numericPnl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.numericPnl.Controls.Add((Control) this.numericRangePnl);
      this.numericPnl.Controls.Add((Control) this.numericValuePnl);
      this.numericPnl.Location = new Point(0, 0);
      this.numericPnl.Name = "numericPnl";
      this.numericPnl.Size = new Size(438, 83);
      this.numericPnl.TabIndex = 0;
      this.numericRangePnl.Controls.Add((Control) this.txtDataMaxValue);
      this.numericRangePnl.Controls.Add((Control) this.txtDataMinValue);
      this.numericRangePnl.Controls.Add((Control) this.label7);
      this.numericRangePnl.Controls.Add((Control) this.label10);
      this.numericRangePnl.Location = new Point(0, 50);
      this.numericRangePnl.Name = "numericRangePnl";
      this.numericRangePnl.Size = new Size(408, 48);
      this.numericRangePnl.TabIndex = 3;
      this.txtDataMaxValue.Location = new Point(242, 0);
      this.txtDataMaxValue.Name = "txtDataMaxValue";
      this.txtDataMaxValue.Size = new Size(100, 20);
      this.txtDataMaxValue.TabIndex = 4;
      this.txtDataMaxValue.TextChanged += new EventHandler(this.txtDataMaxValue_TextChanged);
      this.txtDataMinValue.Location = new Point(106, 0);
      this.txtDataMinValue.Name = "txtDataMinValue";
      this.txtDataMinValue.Size = new Size(100, 20);
      this.txtDataMinValue.TabIndex = 5;
      this.txtDataMinValue.TextChanged += new EventHandler(this.txtDataMinValue_TextChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(-1, 7);
      this.label7.Name = "label7";
      this.label7.Size = new Size(34, 13);
      this.label7.TabIndex = 2;
      this.label7.Text = "Value";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(239, 20);
      this.label10.Name = "label10";
      this.label10.Size = new Size(137, 13);
      this.label10.TabIndex = 3;
      this.label10.Text = "(includes the given number)";
      this.numericValuePnl.Controls.Add((Control) this.txtDataValue);
      this.numericValuePnl.Controls.Add((Control) this.label6);
      this.numericValuePnl.Location = new Point(0, 0);
      this.numericValuePnl.Name = "numericValuePnl";
      this.numericValuePnl.Size = new Size(364, 41);
      this.numericValuePnl.TabIndex = 2;
      this.txtDataValue.Location = new Point(106, 0);
      this.txtDataValue.Name = "txtDataValue";
      this.txtDataValue.Size = new Size(226, 20);
      this.txtDataValue.TabIndex = 1;
      this.txtDataValue.TextChanged += new EventHandler(this.txtDataValue_TextChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(-1, 7);
      this.label6.Name = "label6";
      this.label6.Size = new Size(34, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "Value";
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cancelBtn.Location = new Point(476, 449);
      this.cancelBtn.Margin = new Padding(2);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 6;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.groupBox1.Controls.Add((Control) this.pnlRuleOverride);
      this.groupBox1.Controls.Add((Control) this.pnlLockableField);
      this.groupBox1.Controls.Add((Control) this.pnlOutput);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.textBoxPair);
      this.groupBox1.Controls.Add((Control) this.panel2);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.cancelBtn);
      this.groupBox1.Controls.Add((Control) this.fieldIDTextBx);
      this.groupBox1.Controls.Add((Control) this.ok_btn);
      this.groupBox1.Controls.Add((Control) this.panel1);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.fieldDescTextBx);
      this.groupBox1.Controls.Add((Control) this.value_lbl);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.valueTypeCombo);
      this.groupBox1.Location = new Point(19, 55);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(558, 490);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.pnlRuleOverride.Controls.Add((Control) this.fieldLockButton2);
      this.pnlRuleOverride.Controls.Add((Control) this.label19);
      this.pnlRuleOverride.Controls.Add((Control) this.label18);
      this.pnlRuleOverride.Controls.Add((Control) this.overrideLabel);
      this.pnlRuleOverride.Location = new Point(4, 173);
      this.pnlRuleOverride.Name = "pnlRuleOverride";
      this.pnlRuleOverride.Size = new Size(516, 44);
      this.pnlRuleOverride.TabIndex = 405;
      this.pnlRuleOverride.Visible = false;
      this.fieldLockButton2.Location = new Point(41, 21);
      this.fieldLockButton2.MaximumSize = new Size(16, 16);
      this.fieldLockButton2.MinimumSize = new Size(16, 16);
      this.fieldLockButton2.Name = "fieldLockButton2";
      this.fieldLockButton2.Size = new Size(16, 16);
      this.fieldLockButton2.TabIndex = 5;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(63, 23);
      this.label19.Name = "label19";
      this.label19.Size = new Size(126, 13);
      this.label19.TabIndex = 4;
      this.label19.Text = "after the rule is executed)";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(15, 23);
      this.label18.Name = "label18";
      this.label18.Size = new Size(26, 13);
      this.label18.TabIndex = 3;
      this.label18.Text = "with";
      this.overrideLabel.AutoSize = true;
      this.overrideLabel.Location = new Point(11, 9);
      this.overrideLabel.Name = "overrideLabel";
      this.overrideLabel.Size = new Size(264, 13);
      this.overrideLabel.TabIndex = 0;
      this.overrideLabel.Text = "(The rule will override the calculation automatically and";
      this.pnlLockableField.Controls.Add((Control) this.fieldLockButton1);
      this.pnlLockableField.Controls.Add((Control) this.label14);
      this.pnlLockableField.Controls.Add((Control) this.isLockablechk);
      this.pnlLockableField.Location = new Point(373, 26);
      this.pnlLockableField.Name = "pnlLockableField";
      this.pnlLockableField.Size = new Size(167, 34);
      this.pnlLockableField.TabIndex = 404;
      this.pnlLockableField.Visible = false;
      this.fieldLockButton1.Enabled = false;
      this.fieldLockButton1.Location = new Point(47, 16);
      this.fieldLockButton1.MaximumSize = new Size(16, 16);
      this.fieldLockButton1.MinimumSize = new Size(16, 16);
      this.fieldLockButton1.Name = "fieldLockButton1";
      this.fieldLockButton1.Size = new Size(16, 16);
      this.fieldLockButton1.TabIndex = 409;
      this.label14.AutoSize = true;
      this.label14.Enabled = false;
      this.label14.Location = new Point(20, 16);
      this.label14.Name = "label14";
      this.label14.Size = new Size(26, 13);
      this.label14.TabIndex = 408;
      this.label14.Text = "field";
      this.isLockablechk.AutoSize = true;
      this.isLockablechk.Checked = true;
      this.isLockablechk.CheckState = CheckState.Checked;
      this.isLockablechk.Enabled = false;
      this.isLockablechk.Location = new Point(1, 2);
      this.isLockablechk.Name = "isLockablechk";
      this.isLockablechk.Size = new Size(165, 17);
      this.isLockablechk.TabIndex = 407;
      this.isLockablechk.Text = "This is a lockable/calculated ";
      this.isLockablechk.UseVisualStyleBackColor = true;
      this.pnlOutput.Controls.Add((Control) this.label13);
      this.pnlOutput.Controls.Add((Control) this.label12);
      this.pnlOutput.Controls.Add((Control) this.outputCmb);
      this.pnlOutput.Location = new Point(36, 169);
      this.pnlOutput.Name = "pnlOutput";
      this.pnlOutput.Size = new Size(342, 31);
      this.pnlOutput.TabIndex = 403;
      this.label13.AutoSize = true;
      this.label13.ForeColor = Color.Red;
      this.label13.Location = new Point(33, 14);
      this.label13.Name = "label13";
      this.label13.Size = new Size(11, 13);
      this.label13.TabIndex = 403;
      this.label13.Text = "*";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(-1, 12);
      this.label12.Margin = new Padding(2, 0, 2, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(34, 13);
      this.label12.TabIndex = 402;
      this.label12.Text = "Value";
      this.outputCmb.DropDownStyle = ComboBoxStyle.DropDownList;
      this.outputCmb.FormattingEnabled = true;
      this.outputCmb.Location = new Point(106, 5);
      this.outputCmb.Margin = new Padding(2);
      this.outputCmb.Name = "outputCmb";
      this.outputCmb.Size = new Size(226, 21);
      this.outputCmb.TabIndex = 401;
      this.outputCmb.DropDown += new EventHandler(this.outputCmb_DropDown);
      this.outputCmb.SelectedIndexChanged += new EventHandler(this.outputCmb_SelectedIndexChanged);
      this.outputCmb.DropDownClosed += new EventHandler(this.outputCmb_DropDownClosed);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(35, 93);
      this.label5.Margin = new Padding(2, 0, 2, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(25, 13);
      this.label5.TabIndex = 402;
      this.label5.Text = "Pair";
      this.textBoxPair.Location = new Point(142, 86);
      this.textBoxPair.Margin = new Padding(2);
      this.textBoxPair.Name = "textBoxPair";
      this.textBoxPair.ReadOnly = true;
      this.textBoxPair.Size = new Size(226, 20);
      this.textBoxPair.TabIndex = 401;
      this.panel2.Location = new Point(142, 146);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(300, 22);
      this.panel2.TabIndex = 3;
      this.nextPrevBtnPnl.Controls.Add((Control) this.BtnLastItem);
      this.nextPrevBtnPnl.Controls.Add((Control) this.BtnNextItem);
      this.nextPrevBtnPnl.Controls.Add((Control) this.BtnPrevItem);
      this.nextPrevBtnPnl.Controls.Add((Control) this.BtnFirstItem);
      this.nextPrevBtnPnl.Location = new Point(28, 16);
      this.nextPrevBtnPnl.Name = "nextPrevBtnPnl";
      this.nextPrevBtnPnl.Size = new Size(545, 28);
      this.nextPrevBtnPnl.TabIndex = 0;
      this.BtnLastItem.Location = new Point(456, 2);
      this.BtnLastItem.Name = "BtnLastItem";
      this.BtnLastItem.Size = new Size(75, 23);
      this.BtnLastItem.TabIndex = 3;
      this.BtnLastItem.Text = ">>";
      this.BtnLastItem.UseVisualStyleBackColor = true;
      this.BtnLastItem.Click += new EventHandler(this.BtnLastItem_Click);
      this.BtnNextItem.Location = new Point(360, 3);
      this.BtnNextItem.Name = "BtnNextItem";
      this.BtnNextItem.Size = new Size(75, 23);
      this.BtnNextItem.TabIndex = 2;
      this.BtnNextItem.Text = "Next Field >";
      this.BtnNextItem.UseVisualStyleBackColor = true;
      this.BtnNextItem.Click += new EventHandler(this.BtnNextItem_Click);
      this.BtnPrevItem.Location = new Point(98, 4);
      this.BtnPrevItem.Name = "BtnPrevItem";
      this.BtnPrevItem.Size = new Size(109, 23);
      this.BtnPrevItem.TabIndex = 1;
      this.BtnPrevItem.Text = "< Previous Field";
      this.BtnPrevItem.UseVisualStyleBackColor = true;
      this.BtnPrevItem.Click += new EventHandler(this.BtnPrevItem_Click);
      this.BtnFirstItem.Location = new Point(4, 3);
      this.BtnFirstItem.Name = "BtnFirstItem";
      this.BtnFirstItem.Size = new Size(75, 23);
      this.BtnFirstItem.TabIndex = 0;
      this.BtnFirstItem.Text = "<<";
      this.BtnFirstItem.UseVisualStyleBackColor = true;
      this.BtnFirstItem.Click += new EventHandler(this.BtnFirstItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(597, 558);
      this.Controls.Add((Control) this.nextPrevBtnPnl);
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Margin = new Padding(2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FeeValueDlg);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Set Field Value";
      this.Load += new EventHandler(this.FeeValueDlg_Load);
      this.panel1.ResumeLayout(false);
      this.pnlMultiSelect.ResumeLayout(false);
      this.pnlMultiSelect.PerformLayout();
      this.numericDatePnl.ResumeLayout(false);
      this.datePnl.ResumeLayout(false);
      this.dateRangePnl.ResumeLayout(false);
      this.dateRangePnl.PerformLayout();
      this.datevaluePnl.ResumeLayout(false);
      this.datevaluePnl.PerformLayout();
      this.numericPnl.ResumeLayout(false);
      this.numericRangePnl.ResumeLayout(false);
      this.numericRangePnl.PerformLayout();
      this.numericValuePnl.ResumeLayout(false);
      this.numericValuePnl.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.pnlRuleOverride.ResumeLayout(false);
      this.pnlRuleOverride.PerformLayout();
      this.pnlLockableField.ResumeLayout(false);
      this.pnlLockableField.PerformLayout();
      this.pnlOutput.ResumeLayout(false);
      this.pnlOutput.PerformLayout();
      this.nextPrevBtnPnl.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public FeeValueDlg(DDMFeeRuleValue DDMfeerule_value, Sessions.Session session)
    {
      this.firstLoad = true;
      this.initFeeValueConstructor(DDMfeerule_value, session);
    }

    public FeeValueDlg(
      DDMFeeRuleValue DDMfeerule_value,
      string lineNumber,
      Sessions.Session session)
    {
      this.firstLoad = true;
      this.feeLineNumber = lineNumber;
      this.initFeeValueConstructor(DDMfeerule_value, session);
    }

    public FeeValueDlg(DataTableFieldValues dtFieldValues, Sessions.Session session)
    {
      this.firstLoad = true;
      this.session = session;
      this.DtFieldValues = dtFieldValues;
      this._currentDataTableField = dtFieldValues.DataTableFields[dtFieldValues.CurrentItemIndex];
      this.ddmCurrentTab = FeeValueDlg.ddmTab.DataTable;
      this.InitializeComponent();
      this.PopulateDataTableControlValues();
      this.IsDirty = false;
    }

    public void GetFeeManagementDpItems(string fieldID)
    {
      this.feesecEnum = DDM_FieldAccess_Utils.GetFeeManagementFieldSectionEnum(fieldID);
      if (this.feesecEnum == FeeSectionEnum.Nothing)
        return;
      this.feeManagementSetting = this.session.ConfigurationManager.GetFeeManagement();
      if (this.feeManagementSetting != null)
        this.applyItemizationChecked = this.feeManagementSetting.CompanyOptIn;
      if (!this.applyItemizationChecked)
        return;
      if (this.feeManagementPermission == null)
        this.loadFeeManagementPermission();
      this.IsFMfieldEditable = this.feeManagementPermission.IsSectionEditable(this.feesecEnum);
      this.feeManagementdpItems = ((IEnumerable<string>) this.feeManagementSetting.GetFeeNames(this.feesecEnum)).ToList<string>();
    }

    private void loadFeeManagementPermission()
    {
      if (this.feeManagementPermission != null)
        return;
      this.session.ACL.GetAclManager(AclCategory.FieldAccess);
      List<int> intList = new List<int>();
      for (int index = 0; index < this.session.UserInfo.UserPersonas.Length; ++index)
        intList.Add(this.session.UserInfo.UserPersonas[index].ID);
      this.feeManagementPermission = ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).GetFeeManagementPermission(intList.ToArray());
    }

    private void initFeeValueConstructor(DDMFeeRuleValue DDMfeerule_value, Sessions.Session session)
    {
      this.session = session;
      this.feeRuleValue = DDMfeerule_value;
      this.ddmCurrentTab = FeeValueDlg.ddmTab.FeeField;
      this.InitializeComponent();
      this.GetFeeManagementDpItems(DDMfeerule_value.FieldID);
      this.PopulateControlValues();
      this.IsDirty = false;
    }

    public static FieldValueBase GenerateFieldValueClass(
      string FieldID,
      DDMCriteria criteria,
      FieldFormat format,
      string value)
    {
      string[] strArray;
      if (value == null)
        strArray = (string[]) null;
      else
        strArray = value.Split(new string[1]{ "|" }, StringSplitOptions.None);
      string[] vals = strArray;
      switch (format)
      {
        case FieldFormat.NONE:
          return (FieldValueBase) new DDM_OUTPUT_FieldValues(criteria, value);
        case FieldFormat.STRING:
          if (criteria != DDMCriteria.ListOfValues)
            return (FieldValueBase) new DDM_STRING_NonNumericFieldValues(FieldID, criteria, vals);
          return vals == null ? (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, (string[]) null) : (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, vals);
        case FieldFormat.YN:
        case FieldFormat.X:
        case FieldFormat.STATE:
          return (FieldValueBase) new DDMMultiFieldValues(FieldID, criteria, vals);
        case FieldFormat.ZIPCODE:
        case FieldFormat.PHONE:
          return (FieldValueBase) new DDM_Phone_NonNumericFieldValues(FieldID, criteria, vals);
        case FieldFormat.SSN:
          if (criteria != DDMCriteria.ListOfValues)
            return (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, vals);
          return vals == null ? (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, (string[]) null) : (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, vals);
        case FieldFormat.RA_STRING:
        case FieldFormat.RA_INTEGER:
        case FieldFormat.RA_DECIMAL_2:
        case FieldFormat.RA_DECIMAL_3:
          return (FieldValueBase) new DDM_RA_NonNumericFieldValues(FieldID, criteria, vals);
        case FieldFormat.INTEGER:
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
        case FieldFormat.DATETIME:
          if (criteria != DDMCriteria.Range)
            return (FieldValueBase) new NumericDateFixedFieldValue(FieldID, criteria, value);
          return criteria == DDMCriteria.ListOfValues ? (vals == null ? (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, (string[]) null) : (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, vals)) : (vals == null ? (FieldValueBase) new NumericDateRangeFieldValue(FieldID, (string) null, (string) null) : (FieldValueBase) new NumericDateRangeFieldValue(FieldID, vals[0], vals[1]));
        case FieldFormat.DROPDOWNLIST:
          if (criteria != DDMCriteria.ListOfValues)
            return (FieldValueBase) new DDM_STRING_NonNumericFieldValues(FieldID, criteria, vals);
          return vals == null ? (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, (string[]) null) : (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, vals);
        case FieldFormat.DROPDOWN:
          if (criteria != DDMCriteria.ListOfValues)
            return (FieldValueBase) new DDM_STRING_NonNumericFieldValues(FieldID, criteria, vals);
          return vals == null ? (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, (string[]) null) : (FieldValueBase) new DDMSSNFieldValues(FieldID, criteria, vals);
        default:
          return (FieldValueBase) null;
      }
    }

    private void insertListOfValuesOption(ComboBox cmbBox)
    {
      int count = cmbBox.Items.Count;
      object obj = cmbBox.Items[count - 1];
      if (obj != null && obj.ToString() == "No value in loan file")
        cmbBox.Items.Insert(count - 1, (object) "List Of Values");
      else
        cmbBox.Items.Add((object) "List Of Values");
    }

    private void populateDropdownItems()
    {
      this.valueTypeCombo.Items.Clear();
      this.valueTypeCombo.SelectedIndex = -1;
      this.valueTypeCombo.Text = "";
      if (this.ddmCurrentTab == FeeValueDlg.ddmTab.FeeField)
      {
        if (DDM_FieldAccess_Utils.IsTaxesField(this.feeLineNumber, this.feeRuleValue.FieldID) || DDM_FieldAccess_Utils.IsWholePocField(this.feeLineNumber, this.feeRuleValue.FieldID, HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED))
          this.valueTypeCombo.Items.AddRange((object[]) this.SellerObligatedCriterion);
        else
          this.valueTypeCombo.Items.AddRange((object[]) this.FeeFieldsCriterion);
      }
      if (this.ddmCurrentTab != FeeValueDlg.ddmTab.DataTable)
        return;
      switch (this._currentDataTableField.FieldType)
      {
        case FieldFormat.NONE:
          this.valueTypeCombo.Items.AddRange((object[]) this.DT_OUTPUT_Criterion);
          break;
        case FieldFormat.STRING:
        case FieldFormat.PHONE:
        case FieldFormat.RA_STRING:
        case FieldFormat.RA_INTEGER:
        case FieldFormat.RA_DECIMAL_2:
        case FieldFormat.RA_DECIMAL_3:
        case FieldFormat.DROPDOWNLIST:
        case FieldFormat.DROPDOWN:
          if (DDM_FieldAccess_Utils.IsCountyField(this._currentDataTableField.FieldID))
          {
            this.valueTypeCombo.Items.AddRange((object[]) this.DT_COUNTY_Criterion);
            break;
          }
          if (FieldValidationUtil.HasOptions(this._currentDataTableField.FieldID, this.session))
          {
            this.valueTypeCombo.Items.AddRange((object[]) this.DT_NonNumeric_Enumerated_Criterion);
            break;
          }
          this.valueTypeCombo.Items.AddRange((object[]) this.DT_NonNumeric_Phone_Criterion);
          this.insertListOfValuesOption(this.valueTypeCombo);
          break;
        case FieldFormat.YN:
          this.valueTypeCombo.Items.Add((object) "Ignore this field");
          this.valueTypeCombo.Items.Add((object) "Checkbox");
          this.valueTypeCombo.Items.Add((object) "No value in loan file");
          this.valueTypeCombo.SelectedIndex = 0;
          if (this._currentDataTableField.Value == null || this._currentDataTableField.Value.Criteria == DDMCriteria.NoValueInLoanFile)
            this.valueTypeCombo.SelectedIndex = 2;
          else if (this._currentDataTableField.Value.Criteria != DDMCriteria.IgnoreValueInLoanFile)
            this.valueTypeCombo.SelectedIndex = 1;
          this.populateCheckboxItems();
          break;
        case FieldFormat.X:
          this.valueTypeCombo.Items.Add((object) "Ignore this field");
          this.valueTypeCombo.Items.Add((object) "Checkbox");
          this.valueTypeCombo.Items.Add((object) "No value in loan file");
          this.valueTypeCombo.SelectedIndex = 0;
          if (this._currentDataTableField.Value == null || this._currentDataTableField.Value.Criteria == DDMCriteria.NoValueInLoanFile)
            this.valueTypeCombo.SelectedIndex = 2;
          else if (this._currentDataTableField.Value.Criteria != DDMCriteria.IgnoreValueInLoanFile)
            this.valueTypeCombo.SelectedIndex = 1;
          this.populateCustomCheckboxItems();
          break;
        case FieldFormat.ZIPCODE:
          this.valueTypeCombo.Items.AddRange((object[]) this.DT_ZIPCODE_Criterion);
          break;
        case FieldFormat.STATE:
          this.valueTypeCombo.Items.Add((object) "Ignore this field");
          this.valueTypeCombo.Items.Add((object) "List Of Values");
          this.valueTypeCombo.Items.Add((object) "No value in loan file");
          this.valueTypeCombo.SelectedIndex = 1;
          this.populateStateValuesCheckboxItems();
          break;
        case FieldFormat.SSN:
          this.valueTypeCombo.Items.AddRange((object[]) this.DT_SSN_Criterion);
          this.insertListOfValuesOption(this.valueTypeCombo);
          this.valueTypeCombo.SelectedIndex = 0;
          break;
        case FieldFormat.INTEGER:
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
        case FieldFormat.DATETIME:
          this.valueTypeCombo.Items.AddRange((object[]) this.DTNumberDateCriterion);
          this.insertListOfValuesOption(this.valueTypeCombo);
          break;
      }
    }

    private void populateStateValuesCheckboxItems()
    {
      string[] strArray = new string[51]
      {
        "Alabama",
        "Alaska",
        "Arizona",
        "Arkansas",
        "California",
        "Colorado",
        "Connecticut",
        "Delaware",
        "District of Columbia",
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
      this.lstChkYN.Items.Clear();
      foreach (string stateName in strArray)
        this.lstChkYN.Items.Add((object) new YnCheckBoxItemObject()
        {
          Text = stateName,
          Tag = Utils.GetStateAbbr(stateName)
        });
    }

    private void populateOptionItems()
    {
      FieldDefinition field = EncompassFields.GetField(this._currentDataTableField.FieldID);
      this.lstChkYN.Items.Clear();
      this.lstChkYN.Items.Add((object) new YnCheckBoxItemObject()
      {
        Text = "<Blank>",
        Tag = ""
      });
      foreach (string str in field.Options.GetValues())
        this.lstChkYN.Items.Add((object) new YnCheckBoxItemObject()
        {
          Text = str,
          Tag = str
        });
    }

    private void populateCheckboxItems()
    {
      this.lstChkYN.Items.Clear();
      this.lstChkYN.Items.Add((object) new YnCheckBoxItemObject()
      {
        Text = "<Blank>",
        Tag = ""
      });
      this.lstChkYN.Items.Add((object) new YnCheckBoxItemObject()
      {
        Text = "Yes",
        Tag = "Y"
      });
      this.lstChkYN.Items.Add((object) new YnCheckBoxItemObject()
      {
        Text = "No",
        Tag = "N"
      });
    }

    private void populateCustomCheckboxItems()
    {
      this.lstChkYN.Items.Clear();
      this.lstChkYN.Items.Add((object) new YnCheckBoxItemObject()
      {
        Text = "<Blank>",
        Tag = ""
      });
      this.lstChkYN.Items.Add((object) new YnCheckBoxItemObject()
      {
        Text = "X",
        Tag = "X"
      });
    }

    private bool IsNumericField(FieldFormat fldFormat)
    {
      return fldFormat == FieldFormat.INTEGER || fldFormat == FieldFormat.DECIMAL || fldFormat == FieldFormat.DECIMAL_1 || fldFormat == FieldFormat.DECIMAL_10 || fldFormat == FieldFormat.DECIMAL_2 || fldFormat == FieldFormat.DECIMAL_3 || fldFormat == FieldFormat.DECIMAL_4 || fldFormat == FieldFormat.DECIMAL_5 || fldFormat == FieldFormat.DECIMAL_6 || fldFormat == FieldFormat.DECIMAL_7;
    }

    private bool IsDropDownField(
      FieldFormat fldFormat,
      string fieldid,
      out FieldOptionCollection options)
    {
      FieldDefinition fieldDefinition = !fieldid.ToLower().StartsWith("cx.") ? EncompassFields.GetField(fieldid) : EncompassFields.GetField(fieldid, this.session.LoanManager.GetFieldSettings());
      if (fldFormat == FieldFormat.DROPDOWN || fldFormat == FieldFormat.DROPDOWNLIST || fieldDefinition.Options.Count > 0)
      {
        options = fieldDefinition.Options;
        return true;
      }
      options = (FieldOptionCollection) null;
      return false;
    }

    private bool IsDateField(FieldFormat fldFormat)
    {
      return fldFormat == FieldFormat.DATETIME || fldFormat == FieldFormat.DATE || fldFormat == FieldFormat.MONTHDAY;
    }

    private void PopulateDataTableControlValues()
    {
      this.triggerField = (FieldDefinition) null;
      this.panel2.Visible = false;
      this.value_lbl.Visible = false;
      this.panel1.Visible = false;
      this.clearPanel1Controls();
      this.pnlMultiSelect.Visible = false;
      this.populateDropdownItems();
      if (this.valueTypeCombo.SelectedItem != null)
      {
        if (this.IsNumericField(this._currentDataTableField.FieldType))
        {
          this.numericDatePnl.Visible = true;
          this.numericPnl.Visible = true;
          this.datePnl.Visible = false;
          this.pnlMultiSelect.Visible = false;
          this.panel1.Visible = true;
        }
        if (this.IsDateField(this._currentDataTableField.FieldType))
        {
          this.numericDatePnl.Visible = true;
          this.numericPnl.Visible = false;
          this.datePnl.Visible = true;
          this.pnlMultiSelect.Visible = false;
          this.panel1.Visible = true;
        }
        if ((this._currentDataTableField.FieldType == FieldFormat.YN || this._currentDataTableField.FieldType == FieldFormat.X) && (string) this.valueTypeCombo.SelectedItem != "Ignore this field" && (string) this.valueTypeCombo.SelectedItem != "No value in loan file" || this._currentDataTableField.FieldType == FieldFormat.STATE)
        {
          this.panel1.Visible = true;
          this.numericDatePnl.Visible = true;
          this.numericPnl.Visible = false;
          this.datePnl.Visible = false;
          this.pnlMultiSelect.Visible = true;
        }
        if (this._currentDataTableField.FieldType == FieldFormat.SSN)
        {
          this.numericDatePnl.Visible = false;
          this.numericPnl.Visible = false;
          this.datePnl.Visible = false;
          this.pnlMultiSelect.Visible = false;
          this.panel1.Visible = false;
        }
        if (this._currentDataTableField.FieldType == FieldFormat.RA_STRING || this._currentDataTableField.FieldType == FieldFormat.RA_INTEGER || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_2 || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_3 || this._currentDataTableField.FieldType == FieldFormat.STRING || this._currentDataTableField.FieldType == FieldFormat.DROPDOWN || this._currentDataTableField.FieldType == FieldFormat.DROPDOWNLIST || this._currentDataTableField.FieldType == FieldFormat.PHONE)
        {
          this.numericDatePnl.Visible = false;
          this.numericPnl.Visible = false;
          this.datePnl.Visible = false;
          this.pnlMultiSelect.Visible = false;
          this.panel1.Visible = true;
        }
        if (this._currentDataTableField.FieldType == FieldFormat.NONE)
        {
          this.numericDatePnl.Visible = false;
          this.numericPnl.Visible = false;
          this.datePnl.Visible = false;
          this.pnlMultiSelect.Visible = false;
          this.panel1.Visible = true;
        }
        if (this.valueTypeCombo.SelectedItem.ToString() == "List Of Values" && !FieldValidationUtil.HasOptions(this._currentDataTableField.FieldID, this.session) && this._currentDataTableField.FieldType != FieldFormat.STATE)
        {
          this.getddmListOfValuesControls();
          this.panel2.Visible = false;
          this.value_lbl.Visible = false;
          this.panel1.Visible = true;
        }
      }
      DDMField ddmField = new DDMField(FieldPairParser.ParseFieldPairInfo(this._currentDataTableField.FieldID));
      this.fieldIDTextBx.Text = this._currentDataTableField.IsOutput ? this._currentDataTableField.Description : ddmField.FieldId;
      this.textBoxPair.Text = ddmField.PairText;
      this.fieldDescTextBx.Text = this._currentDataTableField.IsOutput ? ddmField.FieldId : this._currentDataTableField.Description;
      if (this._currentDataTableField != null && this._currentDataTableField.Value != null)
      {
        if (this.IsNumericField(this._currentDataTableField.FieldType) || this.IsDateField(this._currentDataTableField.FieldType))
        {
          this.valueTypeCombo.SelectedItem = (object) this.getItemFromNumericCriteria(this._currentDataTableField.Value.Criteria);
          if (this.valueTypeCombo.SelectedItem == null)
          {
            switch (this._currentDataTableField.Value.Criteria)
            {
              case DDMCriteria.strEquals:
                this.valueTypeCombo.SelectedItem = (object) this.getItemFromNumericCriteria(DDMCriteria.Equals);
                break;
              case DDMCriteria.strNotEqual:
                this.valueTypeCombo.SelectedItem = (object) this.getItemFromNumericCriteria(DDMCriteria.NotEqual);
                break;
            }
          }
        }
        if (this._currentDataTableField.FieldType == FieldFormat.STATE)
          this.valueTypeCombo.SelectedItem = (object) this.getItemFromNumericCriteria(this._currentDataTableField.Value.Criteria);
        if (this._currentDataTableField.FieldType == FieldFormat.SSN)
          this.valueTypeCombo.SelectedItem = (object) this.getItemFromNumericCriteria(this._currentDataTableField.Value.Criteria);
        if (this._currentDataTableField.FieldType == FieldFormat.RA_STRING || this._currentDataTableField.FieldType == FieldFormat.RA_INTEGER || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_2 || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_3 || this._currentDataTableField.FieldType == FieldFormat.STRING || this._currentDataTableField.FieldType == FieldFormat.DROPDOWN || this._currentDataTableField.FieldType == FieldFormat.DROPDOWNLIST || this._currentDataTableField.FieldType == FieldFormat.PHONE || this._currentDataTableField.FieldType == FieldFormat.ZIPCODE)
        {
          this.valueTypeCombo.SelectedItem = (object) this.getItemFromNumericCriteria(this._currentDataTableField.Value.Criteria);
          if (this.valueTypeCombo.SelectedItem == null)
          {
            switch (this._currentDataTableField.Value.Criteria)
            {
              case DDMCriteria.Equals:
                this.valueTypeCombo.SelectedItem = (object) this.getItemFromNumericCriteria(DDMCriteria.strEquals);
                break;
              case DDMCriteria.NotEqual:
                this.valueTypeCombo.SelectedItem = (object) this.getItemFromNumericCriteria(DDMCriteria.strNotEqual);
                break;
            }
          }
        }
        if (this._currentDataTableField.FieldType == FieldFormat.NONE)
          this.valueTypeCombo.SelectedItem = !this._currentDataTableField.IsOutput && !(this._currentDataTableField.FieldID == "Output") || this._currentDataTableField.Value.Criteria != DDMCriteria.IgnoreValueInLoanFile && this._currentDataTableField.Value.Criteria != DDMCriteria.none ? (object) this.getItemFromNumericCriteria(this._currentDataTableField.Value.Criteria) : (object) "Please Select";
      }
      FieldValueBase fieldValueBase = this._currentDataTableField.Value;
      if (this.IsNumericField(this._currentDataTableField.FieldType))
      {
        if ((string) this.valueTypeCombo.SelectedItem == "Range")
        {
          if (fieldValueBase is NumericDateRangeFieldValue)
          {
            this.txtDataMinValue.Text = (fieldValueBase as NumericDateRangeFieldValue).Minimum;
            this.txtDataMaxValue.Text = (fieldValueBase as NumericDateRangeFieldValue).Maximum;
          }
        }
        else if (fieldValueBase is NumericDateFixedFieldValue)
          this.txtDataValue.Text = (fieldValueBase as NumericDateFixedFieldValue).Values;
      }
      if (this.IsDateField(this._currentDataTableField.FieldType))
      {
        if ((string) this.valueTypeCombo.SelectedItem == "Range")
        {
          if (fieldValueBase is NumericDateRangeFieldValue)
          {
            this.dptxtMinDateValue.Text = (fieldValueBase as NumericDateRangeFieldValue).Minimum;
            this.dptxtMaxDateValue.Text = (fieldValueBase as NumericDateRangeFieldValue).Maximum;
          }
        }
        else if (fieldValueBase is NumericDateFixedFieldValue)
          this.dptxtDateValue.Text = (fieldValueBase as NumericDateFixedFieldValue).Values;
      }
      if ((this._currentDataTableField.FieldType == FieldFormat.YN || this._currentDataTableField.FieldType == FieldFormat.STATE || this._currentDataTableField.FieldType == FieldFormat.X) && fieldValueBase is DDMMultiFieldValues)
      {
        string[] values = (fieldValueBase as DDMMultiFieldValues).Values;
        for (int index = 0; index < this.lstChkYN.Items.Count; ++index)
        {
          YnCheckBoxItemObject checkBoxItemObject = (YnCheckBoxItemObject) this.lstChkYN.Items[index];
          if (((IEnumerable<string>) values).Contains<string>(checkBoxItemObject.Tag))
            this.lstChkYN.SetItemChecked(index, true);
        }
      }
      if (this._currentDataTableField.FieldType == FieldFormat.SSN && fieldValueBase is DDMSSNFieldValues && fieldValueBase.Criteria == DDMCriteria.SSN_SpecificValue)
        this.setSpecificSSNValue((fieldValueBase as DDMSSNFieldValues).Values[0]);
      if ((this._currentDataTableField.FieldType == FieldFormat.RA_STRING || this._currentDataTableField.FieldType == FieldFormat.RA_INTEGER || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_2 || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_3) && fieldValueBase is DDM_RA_NonNumericFieldValues && this.IsNonNumericString(fieldValueBase.Criteria))
        this.setSpecificSSNValue((fieldValueBase as DDM_RA_NonNumericFieldValues).Values[0]);
      if (this._currentDataTableField.FieldType == FieldFormat.STRING && fieldValueBase is DDM_STRING_NonNumericFieldValues)
      {
        if (DDM_FieldAccess_Utils.IsCountyField(this._currentDataTableField.FieldID))
        {
          if (fieldValueBase.Criteria == DDMCriteria.county_SpecificValue)
            this.setSpecificSSNValue((fieldValueBase as DDM_STRING_NonNumericFieldValues).Values[0]);
          if (fieldValueBase.Criteria == DDMCriteria.county_FindByCounty)
            this.setFindbyZipValue((fieldValueBase as DDM_STRING_NonNumericFieldValues).Values);
        }
        if (this.IsNonNumericString(fieldValueBase.Criteria))
          this.setSpecificSSNValue((fieldValueBase as DDM_STRING_NonNumericFieldValues).Values[0]);
      }
      if ((this._currentDataTableField.FieldType == FieldFormat.DROPDOWN || this._currentDataTableField.FieldType == FieldFormat.DROPDOWNLIST) && fieldValueBase is DDM_STRING_NonNumericFieldValues && this.IsNonNumericString(fieldValueBase.Criteria))
        this.setSpecificSSNValue((fieldValueBase as DDM_STRING_NonNumericFieldValues).Values[0]);
      if (this._currentDataTableField.FieldType == FieldFormat.PHONE && fieldValueBase is DDM_Phone_NonNumericFieldValues && this.IsNonNumericString(fieldValueBase.Criteria))
        this.setSpecificSSNValue((fieldValueBase as DDM_Phone_NonNumericFieldValues).Values[0]);
      if (this._currentDataTableField.FieldType == FieldFormat.ZIPCODE && fieldValueBase is DDM_Phone_NonNumericFieldValues)
      {
        if (fieldValueBase.Criteria == DDMCriteria.zip_SpecificValue)
          this.setSpecificSSNValue((fieldValueBase as DDM_Phone_NonNumericFieldValues).Values[0]);
        if (fieldValueBase.Criteria == DDMCriteria.zip_FindByZip)
          this.setFindbyZipValue((fieldValueBase as DDM_Phone_NonNumericFieldValues).Values);
      }
      if (this._currentDataTableField.FieldType != FieldFormat.NONE || !(fieldValueBase is DDM_OUTPUT_FieldValues))
        return;
      if (fieldValueBase.Criteria == DDMCriteria.OP_SpecificValue)
        this.setSpecificSSNValue((fieldValueBase as DDM_OUTPUT_FieldValues).Value);
      if (fieldValueBase.Criteria != DDMCriteria.OP_AdvancedCoding)
        return;
      this.setAdvancedCodeForOutput((fieldValueBase as DDM_OUTPUT_FieldValues).Value);
    }

    private void setFindbyZipValue(string[] p)
    {
      string str1 = string.Empty;
      foreach (string str2 in p)
        str1 = str1 + str2 + ",";
      if (((IEnumerable<string>) p).Count<string>() > 0)
        str1 = str1.Substring(0, str1.Length - 1);
      this.zipCountryStateControl.ValuesExisting = str1;
      this.zipCountryStateControl.Refresh();
    }

    private bool IsNonNumericString(DDMCriteria criteria)
    {
      return criteria == DDMCriteria.strBegins || criteria == DDMCriteria.strContains || criteria == DDMCriteria.strEnds || criteria == DDMCriteria.strEquals || criteria == DDMCriteria.strNotContains || criteria == DDMCriteria.strNotEqual;
    }

    private void setSpecificSSNValue(string ssnValue)
    {
      if (this.currentValueControlMode == FeeValueDlg.ValueControlMode.DropDownEdit || this.currentValueControlMode == FeeValueDlg.ValueControlMode.DropDownNonEdit)
      {
        this.setSelectedItemForValueCombo(this.valueCombo, ssnValue);
        this.createTextBox();
        this.valueText.Visible = false;
        this.valueText.Text = ssnValue;
      }
      else
      {
        this.createTextBox();
        this.panel2.Controls.Add((Control) this.valueText);
        this.valueText.Text = ssnValue;
      }
    }

    private void getSpecificSSNValue()
    {
      this.createTextBox();
      this.panel2.Controls.Add((Control) this.valueText);
    }

    private void ShowHideLockableFieldUI(bool show)
    {
      if (show)
      {
        this.valueTypeCombo.Items.Add((object) "Use Calculated Value");
        this.pnlLockableField.Visible = true;
        this.pnlRuleOverride.Visible = true;
        this.offsetUIForLockableField = true;
        this.fieldLockButton2.Locked = true;
      }
      else
      {
        this.pnlLockableField.Visible = false;
        this.pnlRuleOverride.Visible = false;
        this.offsetUIForLockableField = false;
      }
    }

    private void PopulateControlValues()
    {
      this.populateDropdownItems();
      this.nextPrevBtnPnl.Visible = false;
      this.numericDatePnl.Visible = false;
      this.pnlMultiSelect.Visible = false;
      DDMField ddmField = new DDMField(FieldPairParser.ParseFieldPairInfo(this.feeRuleValue.FieldID));
      this.fieldIDTextBx.Text = ddmField.FieldId;
      this.textBoxPair.Text = ddmField.PairText;
      this.fieldDescTextBx.Text = this.feeRuleValue.Field_Name;
      this.triggerField = this.fieldIDTextBx.Text.ToLower().StartsWith("cx.") ? EncompassFields.GetField(this.feeRuleValue.FieldID, this.session.LoanManager.GetFieldSettings()) : EncompassFields.GetField(this.feeRuleValue.FieldID);
      this.ShowHideLockableFieldUI(this.triggerField.FieldLockIcon);
      if (this.feeRuleValue.Field_Value_Type != fieldValueType.none)
      {
        switch (this.feeRuleValue.Field_Value_Type)
        {
          case fieldValueType.ValueNotSet:
            this.valueTypeCombo.SelectedItem = (object) "No Value Set";
            break;
          case fieldValueType.SpecificValue:
          case fieldValueType.FeeManagement:
            this.valueTypeCombo.SelectedItem = (object) "Specific Value";
            break;
          case fieldValueType.Table:
          case fieldValueType.SystemTable:
            this.valueTypeCombo.SelectedItem = (object) "Table";
            break;
          case fieldValueType.Calculation:
            this.valueTypeCombo.SelectedItem = (object) "Calculation";
            break;
          case fieldValueType.ClearValueInLoanFile:
            this.valueTypeCombo.SelectedItem = (object) "Clear value in loan file";
            break;
          case fieldValueType.UseCalculatedValue:
            this.valueTypeCombo.SelectedItem = (object) "Use Calculated Value";
            break;
        }
      }
      if (!(this.feeRuleValue.Field_Value != ""))
        return;
      switch (this.valueTypeCombo.SelectedItem.ToString())
      {
        case "Specific Value":
          this.getValueSpecificControls(true);
          this.panel2.Visible = true;
          this.value_lbl.Visible = true;
          this.panel1.Visible = false;
          break;
        case "No Value Set":
          this.getNoValueSetControls();
          this.panel2.Visible = true;
          this.value_lbl.Visible = true;
          this.panel1.Visible = false;
          break;
        case "Calculation":
          this.getAdvancedCodingControls(true);
          this.panel2.Visible = false;
          this.value_lbl.Visible = false;
          this.panel1.Visible = true;
          this.panel1.BorderStyle = BorderStyle.Fixed3D;
          break;
        case "Table":
          this.getDdmDataTableControls(true);
          this.panel2.Visible = true;
          this.value_lbl.Visible = true;
          this.panel1.Visible = false;
          break;
      }
    }

    private void FeeFieldOkBtnClick()
    {
      if (this.valueTypeCombo == null || this.valueTypeCombo.SelectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a Valuetype", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        bool flag1 = this.IsFeeManagementField && this.valueTypeCombo.SelectedItem != null && (string) this.valueTypeCombo.SelectedItem == "Specific Value";
        bool flag2 = this.IsSystemTable && this.valueTypeCombo.SelectedItem != null && (string) this.valueTypeCombo.SelectedItem == "Table";
        fieldValueType fldValueType = !flag1 ? (!flag2 ? DDMFeeRuleValue.valueToTypeTable[this.valueTypeCombo.SelectedItem.ToString()] : fieldValueType.SystemTable) : fieldValueType.FeeManagement;
        FieldFormat fieldFormat = this.feeRuleValue.Field_Type;
        if (fldValueType == fieldValueType.ClearValueInLoanFile)
          fieldFormat = FieldFormat.NONE;
        if (fldValueType == fieldValueType.Calculation && !this.validateCalculation())
          return;
        if (fldValueType != fieldValueType.ValueNotSet)
        {
          if (fldValueType == fieldValueType.Table || fldValueType == fieldValueType.SystemTable)
          {
            if (string.IsNullOrEmpty(this.valueText.Text.Trim()))
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The Table field cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (fldValueType == fieldValueType.Table && this.outputCmb.Text == "Please Select...")
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "Output column is required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          if (fldValueType == fieldValueType.SystemTable && DDM_FieldAccess_Utils.DoesAffectOtherFieldsBySysTbl(this.feeRuleValue.FieldID))
          {
            if (Utils.Dialog((IWin32Window) this, string.Format("Changing to a system table setting will clear the value setting on the following field(s); {0}. Do you want to proceed?", (object) DDM_FieldAccess_Utils.GetAffectedFieldsBySysTable(this.feeRuleValue.FieldID)), MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            {
              this.valueText.Text = "";
              return;
            }
          }
          else if (!DDM_FieldAccess_Utils.isSpecialFeeLine(this.feeLineNumber) && DDM_FieldAccess_Utils.DoesAffectOtherFieldsByNonSysTbl(this.feeRuleValue.FieldID) && !string.Equals(this.valueTypeCombo.SelectedItem.ToString(), "Use Calculated Value"))
          {
            string text = "";
            Dictionary<string, object> sysTableWithAction = DDM_FieldAccess_Utils.GetAffectedFieldsDictByNonSysTableWithAction(this.feeRuleValue.FieldID);
            if (sysTableWithAction != null && sysTableWithAction.Values.Contains<object>((object) "R"))
              text = string.Format("Changing the field setting for {0}, may override the value calculated from {1}. Do you want to proceed?", (object) this.feeRuleValue.FieldID, (object) DDM_FieldAccess_Utils.GetAffectedFieldsBySysTable(this.feeRuleValue.FieldID));
            else if (this.feeLineNumber == "902" && this.feeRuleValue.FieldID == "3533")
            {
              if (fldValueType == fieldValueType.SpecificValue && this.valueCombo.Text == "Yes")
                text = "Setting for Lender Paid Mortgage Insurance scenario will clear the value setting for some of the fields under Upfront and Monthly Mortgage Insurance sections. Do you want to proceed?";
            }
            else
              text = string.Format("Changing this field setting may overwrite the value setting on the following field(s); {0}. Do you want to proceed?", (object) DDM_FieldAccess_Utils.GetAffectedFieldsBySysTable(this.feeRuleValue.FieldID));
            if (text.Length > 0 && Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            {
              if (this.valueText == null)
                return;
              this.valueText.Text = "";
              return;
            }
          }
        }
        Point pt = new Point(0, 0);
        Control control = (Control) null;
        if (this.panel1.Visible)
        {
          control = this.panel1.GetChildAtPoint(pt);
          if (control == null && ((IEnumerable<Control>) this.panel1.Controls.Find("valueText", true)).Any<Control>())
            control = this.panel1.Controls.Find("valueText", true)[0];
        }
        if (this.panel2.Visible)
        {
          control = this.panel2.GetChildAtPoint(pt);
          if (control == null && ((IEnumerable<Control>) this.panel2.Controls.Find("valueText", true)).Any<Control>())
            control = this.panel2.Controls.Find("valueText", true)[0];
        }
        if (control != null && control.GetType() == typeof (TextBox))
        {
          if (fldValueType != fieldValueType.Table && fldValueType != fieldValueType.SystemTable)
          {
            if (!this.ValidateFeeFieldRuleValueInput(fieldFormat, fldValueType, (string) this.valueTypeCombo.SelectedItem, this.valueText.Text))
              return;
            if (this.IsNumericField(fieldFormat) && fldValueType == fieldValueType.SpecificValue && !this.IsNumericOnly(this.valueText.Text))
            {
              int num4 = (int) Utils.Dialog((IWin32Window) this, "Please enter numeric value for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            if (this.IsNumericField(fieldFormat) && fldValueType == fieldValueType.SpecificValue && !this.checkValidNumChar(this.valueText.Text))
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this, "Numeric value can not be over " + (object) (this.triggerField != null ? this.triggerField.MaxLength : 14) + " characters.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            try
            {
              if (fldValueType == fieldValueType.SpecificValue)
              {
                if (fieldFormat == FieldFormat.DATE)
                  this.valueText.Text = Utils.FormatDateValue(this.valueText.Text, false);
                if (fieldFormat == FieldFormat.DATETIME)
                  this.valueText.Text = Utils.FormatDateValue(this.valueText.Text, true);
              }
            }
            catch (FormatException ex)
            {
              int num6 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            if (fieldFormat == FieldFormat.PHONE && fldValueType != fieldValueType.Calculation && !Utils.ValidatePhoneNumber(this.valueText.Text))
            {
              int num7 = (int) Utils.Dialog((IWin32Window) this, "Please enter " + fieldFormat.ToString() + " format " + this.getValidationMsg(fieldFormat) + " for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            if (fieldFormat == FieldFormat.STATE && fldValueType == fieldValueType.SpecificValue && !((IEnumerable<string>) Utils.GetStates()).Contains<string>(this.valueText.Text))
            {
              int num8 = (int) Utils.Dialog((IWin32Window) this, "Please enter a valid state.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
          }
          if (fldValueType != fieldValueType.Calculation && fldValueType != fieldValueType.Table && fldValueType != fieldValueType.SystemTable)
          {
            bool needsUpdate = false;
            Utils.FormatInput(this.valueText.Text, fieldFormat, ref needsUpdate);
            if (needsUpdate)
            {
              int num9 = (int) Utils.Dialog((IWin32Window) this, "Please enter " + fieldFormat.ToString() + " format " + this.getValidationMsg(fieldFormat) + " for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
          }
          this.feeRuleValue.Field_Value = fldValueType == fieldValueType.Table || fldValueType == fieldValueType.SystemTable ? this.dtValuewithPrefix : this.valueText.Text;
          this.feeRuleValue.Field_Value_Type = fldValueType;
        }
        else if (control != null && control.GetType() == typeof (ComboBox))
        {
          if (!this.IsFeeManagementField)
          {
            if (this.valueCombo.SelectedItem == null)
            {
              int num10 = (int) Utils.Dialog((IWin32Window) this, "Please enter value for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            this.feeRuleValue.Field_Value = this.getSettingValueFromValueCombo(this.valueCombo);
            this.feeRuleValue.Field_Value_Type = fldValueType;
          }
          else if (this.valueCombo.SelectedItem != null && this.valueCombo.SelectedItem.ToString().Trim() != string.Empty || this.valueCombo.Text.Trim() != string.Empty)
          {
            this.feeRuleValue.Field_Value = this.valueCombo.SelectedItem == null ? this.valueCombo.Text : this.valueCombo.SelectedItem.ToString();
            this.feeRuleValue.Field_Value_Type = fldValueType;
          }
          else
          {
            int num11 = (int) Utils.Dialog((IWin32Window) this, "Please enter value for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        }
        else if (control != null && control.GetType() == typeof (Label))
        {
          if (this.valueText.Text.Length == 0)
          {
            int num12 = (int) Utils.Dialog((IWin32Window) this, "Please enter value for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          this.feeRuleValue.Field_Value = this.valueText.Text;
          this.feeRuleValue.Field_Value_Type = fldValueType;
        }
        this.Close();
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool IsNumericOnly(string str)
    {
      return str.All<char>((Func<char, bool>) (c => c >= '0' && c <= '9' || c == '.'));
    }

    private void ok_btn_Click(object sender, EventArgs e)
    {
      if (this.ddmCurrentTab == FeeValueDlg.ddmTab.FeeField)
        this.FeeFieldOkBtnClick();
      if (this.ddmCurrentTab != FeeValueDlg.ddmTab.DataTable)
        return;
      this.DataTableOkBtnClick();
    }

    private bool ValidateFeeFieldRuleValueInput(
      FieldFormat fldFormat,
      fieldValueType fldValueType,
      string selectedOperation,
      string currentValue)
    {
      if (fldValueType == fieldValueType.UseCalculatedValue)
        return true;
      if (string.IsNullOrEmpty(currentValue) && !selectedOperation.Equals("No Value Set") && !selectedOperation.Equals("Clear value in loan file"))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter value for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.IsNumericField(fldFormat) && fldValueType == fieldValueType.SpecificValue && !this.IsNumericOnly(currentValue))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter numeric value for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if ((fldFormat == FieldFormat.STRING || fldFormat == FieldFormat.RA_STRING || fldFormat == FieldFormat.RA_INTEGER || fldFormat == FieldFormat.RA_DECIMAL_2 || fldFormat == FieldFormat.RA_DECIMAL_3) && fldValueType == fieldValueType.SpecificValue && string.IsNullOrEmpty(currentValue.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a non-empty value for field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.triggerField.Format == FieldFormat.ZIPCODE && fldValueType != fieldValueType.Calculation && fldValueType != fieldValueType.ClearValueInLoanFile && fldValueType != fieldValueType.ValueNotSet)
      {
        ZipCodeInfo zipCodeInfo = (ZipCodeInfo) null;
        if (currentValue != null && currentValue.Length >= 5)
          zipCodeInfo = ZipcodeSelector.GetZipCodeInfoWithUserDefined(currentValue.Substring(0, 5));
        if (zipCodeInfo == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The zip code is not valid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      return true;
    }

    private bool ValidateAndSetCurrentChangesToCollection()
    {
      FieldValueBase fieldValueBase = (FieldValueBase) null;
      if (this._currentDataTableField.FieldType != FieldFormat.NONE)
      {
        if (string.Equals((string) this.valueTypeCombo.SelectedItem, "Ignore this field", StringComparison.InvariantCultureIgnoreCase))
        {
          this._currentDataTableField.Value = (FieldValueBase) new DDM_STRING_NonNumericFieldValues(this._currentDataTableField.FieldID, DDMCriteria.IgnoreValueInLoanFile, new string[1]
          {
            ""
          });
          this.txtDataValue.Text = string.Empty;
          return true;
        }
        if (string.Equals(this.valueTypeCombo.SelectedItem.ToString(), "No value in loan file", StringComparison.InvariantCultureIgnoreCase))
        {
          this._currentDataTableField.Value = (FieldValueBase) new DDM_STRING_NonNumericFieldValues(this._currentDataTableField.FieldID, DDMCriteria.NoValueInLoanFile, new string[1]
          {
            ""
          });
          this.txtDataValue.Text = string.Empty;
          return true;
        }
      }
      if ((string) this.valueTypeCombo.SelectedItem == "List Of Values" && this._currentDataTableField.FieldType != FieldFormat.STATE)
      {
        this._currentDataTableField.Value = (FieldValueBase) new DDMSSNFieldValues(this._currentDataTableField.FieldID, DDMCriteria.ListOfValues, this.currentValueControlMode != FeeValueDlg.ValueControlMode.Checkboxes ? this.listOfValuesListbox.Items.Cast<ListViewItem>().Select<ListViewItem, string>((Func<ListViewItem, string>) (item => item.Text)).ToArray<string>() : this.chkOptionFieldItems.Cast<CheckBox>().Where<CheckBox>((Func<CheckBox, bool>) (item => item.Checked)).Select<CheckBox, string>((Func<CheckBox, string>) (item => item.Tag.ToString())).ToArray<string>());
        return true;
      }
      if (this.IsNumericField(this._currentDataTableField.FieldType))
      {
        if ((string) this.valueTypeCombo.SelectedItem == "Range")
        {
          if (!this.checkValidNumChar(this.txtDataMaxValue.Text) || !this.checkValidNumChar(this.txtDataMinValue.Text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Numeric value can not be over " + (object) (this.triggerField != null ? this.triggerField.MaxLength : 14) + " characters.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if (!FieldValidationUtil.validateRange(this._currentDataTableField.FieldID, this.txtDataMinValue.Text, this.txtDataMaxValue.Text, this._currentDataTableField.FieldType, this.session))
            return false;
          fieldValueBase = (FieldValueBase) new NumericDateRangeFieldValue(this._currentDataTableField.FieldID, this.txtDataMinValue.Text, this.txtDataMaxValue.Text);
          this._currentDataTableField.Value = fieldValueBase;
          this.txtDataMinValue.Text = this.txtDataMaxValue.Text = string.Empty;
        }
        else
        {
          string str = string.Empty;
          if (!this.checkValidNumChar(this.txtDataValue.Text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Numeric value can not be over " + (object) (this.triggerField != null ? this.triggerField.MaxLength : 14) + " characters.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          if (!string.Equals((string) this.valueTypeCombo.SelectedItem, "Ignore this field", StringComparison.InvariantCultureIgnoreCase))
          {
            if (!FieldValidationUtil.ValidateFormat(this._currentDataTableField.FieldID, this.txtDataValue.Text, this._currentDataTableField.FieldType, this.session))
              return false;
            str = this.txtDataValue.Text;
          }
          fieldValueBase = (FieldValueBase) new NumericDateFixedFieldValue(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), str);
          this._currentDataTableField.Value = fieldValueBase;
          this.txtDataValue.Text = string.Empty;
        }
      }
      if (this.IsDateField(this._currentDataTableField.FieldType))
      {
        if ((string) this.valueTypeCombo.SelectedItem == "Range")
        {
          if (!FieldValidationUtil.validateRange(this._currentDataTableField.FieldID, this.dptxtMinDateValue.Text, this.dptxtMaxDateValue.Text, this._currentDataTableField.FieldType, this.session))
            return false;
          fieldValueBase = (FieldValueBase) new NumericDateRangeFieldValue(this._currentDataTableField.FieldID, this.dptxtMinDateValue.Text, this.dptxtMaxDateValue.Text);
          this._currentDataTableField.Value = fieldValueBase;
        }
        else
        {
          if (!FieldValidationUtil.ValidateFormat(this._currentDataTableField.FieldID, this.dptxtDateValue.Text, this._currentDataTableField.FieldType, this.session))
            return false;
          fieldValueBase = (FieldValueBase) new NumericDateFixedFieldValue(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), this.dptxtDateValue.Text);
          this._currentDataTableField.Value = fieldValueBase;
        }
      }
      if (this._currentDataTableField.FieldType == FieldFormat.YN || this._currentDataTableField.FieldType == FieldFormat.STATE || this._currentDataTableField.FieldType == FieldFormat.X)
      {
        this.pnlMultiSelect.Visible = true;
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        string[] array = this.lstChkYN.CheckedItems.Cast<YnCheckBoxItemObject>().Select<YnCheckBoxItemObject, string>((Func<YnCheckBoxItemObject, string>) (item => item.Tag)).ToArray<string>();
        if (!this.validateValueList(this._currentDataTableField.FieldID, array))
          return false;
        this.pnlMultiSelect.Visible = true;
        fieldValueBase = (FieldValueBase) new DDMMultiFieldValues(this._currentDataTableField.FieldID, this._currentDataTableField.FieldType == FieldFormat.STATE ? DDMCriteria.st_ListOfValues : DDMCriteria.CheckBox, array);
        this._currentDataTableField.Value = fieldValueBase;
      }
      if (this._currentDataTableField.FieldType == FieldFormat.SSN)
      {
        this.pnlMultiSelect.Visible = false;
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.panel1.Visible = true;
        if (!FieldValidationUtil.ValidateFormat(this._currentDataTableField.FieldID, this.valueText.Text, this._currentDataTableField.FieldType, this.session))
          return false;
        fieldValueBase = (FieldValueBase) new DDMSSNFieldValues(this._currentDataTableField.FieldID, DDMCriteria.SSN_SpecificValue, new string[1]
        {
          this.valueText.Text
        });
        this._currentDataTableField.Value = fieldValueBase;
        this.valueText.Text = string.Empty;
      }
      if (this._currentDataTableField.FieldType == FieldFormat.RA_STRING || this._currentDataTableField.FieldType == FieldFormat.RA_INTEGER || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_2 || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_3)
      {
        this.pnlMultiSelect.Visible = false;
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.panel1.Visible = true;
        if (!FieldValidationUtil.ValidateFormat(this._currentDataTableField.FieldID, this.valueText.Text, this._currentDataTableField.FieldType, this.session))
          return false;
        fieldValueBase = (FieldValueBase) new DDM_RA_NonNumericFieldValues(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), new string[1]
        {
          this.valueText.Text
        });
        this._currentDataTableField.Value = fieldValueBase;
        this.valueText.Text = string.Empty;
      }
      if (this._currentDataTableField.FieldType == FieldFormat.STRING || this._currentDataTableField.FieldType == FieldFormat.DROPDOWN || this._currentDataTableField.FieldType == FieldFormat.DROPDOWNLIST)
      {
        this.pnlMultiSelect.Visible = false;
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.panel1.Visible = true;
        if (DDM_FieldAccess_Utils.IsCountyField(this._currentDataTableField.FieldID))
        {
          if ((string) this.valueTypeCombo.SelectedItem != "Find County")
          {
            if (this.valueText == null || string.IsNullOrWhiteSpace(this.valueText.Text))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "Value cannot be empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
            if (this.valueText != null && !string.IsNullOrEmpty(this.valueText.Text) && !ZipCodeUtils.IsValidCounty(this.valueText.Text.ToUpper()))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The County is not valid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
            fieldValueBase = (FieldValueBase) new DDM_STRING_NonNumericFieldValues(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), new string[1]
            {
              this.valueText.Text
            });
            this._currentDataTableField.Value = fieldValueBase;
            this.valueText.Text = string.Empty;
          }
          else
          {
            fieldValueBase = (FieldValueBase) new DDM_STRING_NonNumericFieldValues(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), this.CombineZipStringAndList(this.zipCountryStateControl.ValuesExisting, this.zipCountryStateControl.ZipCollection.ToArray()));
            this._currentDataTableField.Value = fieldValueBase;
            this.ClearZipCountyControl();
          }
        }
        else
        {
          if (this.currentValueControlMode == FeeValueDlg.ValueControlMode.DropDownEdit || this.currentValueControlMode == FeeValueDlg.ValueControlMode.DropDownNonEdit)
            this.valueText.Text = this.valueCombo.SelectedItem != null ? this.valueCombo.SelectedItem.ToString() : string.Empty;
          if ((string) this.valueTypeCombo.SelectedItem == "Equals" && this.currentValueControlMode != FeeValueDlg.ValueControlMode.DropDownNonEdit && !FieldValidationUtil.ValidateFormat(this._currentDataTableField.FieldID, this.valueText.Text, this._currentDataTableField.FieldType, this.session))
            return false;
          fieldValueBase = (FieldValueBase) new DDM_STRING_NonNumericFieldValues(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), new string[1]
          {
            this.valueText.Text
          });
          this._currentDataTableField.Value = fieldValueBase;
          this.valueText.Text = string.Empty;
        }
      }
      if (this._currentDataTableField.FieldType == FieldFormat.PHONE)
      {
        this.pnlMultiSelect.Visible = false;
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.panel1.Visible = true;
        if (!FieldValidationUtil.ValidateFormat(this._currentDataTableField.FieldID, this.valueText.Text, this._currentDataTableField.FieldType, this.session))
          return false;
        fieldValueBase = (FieldValueBase) new DDM_Phone_NonNumericFieldValues(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), new string[1]
        {
          this.valueText.Text
        });
        this._currentDataTableField.Value = fieldValueBase;
        this.valueText.Text = string.Empty;
      }
      if (this._currentDataTableField.FieldType == FieldFormat.ZIPCODE)
      {
        this.pnlMultiSelect.Visible = false;
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.panel1.Visible = true;
        if ((string) this.valueTypeCombo.SelectedItem != "Find Zip")
        {
          if (!FieldValidationUtil.ValidateFormat(this._currentDataTableField.FieldID, this.valueText.Text, this._currentDataTableField.FieldType, this.session))
            return false;
          if (ZipcodeSelector.GetZipCodeInfoWithUserDefined(this.valueText.Text.Length > 5 ? this.valueText.Text.Substring(0, 5) : this.valueText.Text) == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The zip code is not valid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          fieldValueBase = (FieldValueBase) new DDM_Phone_NonNumericFieldValues(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), new string[1]
          {
            this.valueText.Text
          });
          this._currentDataTableField.Value = fieldValueBase;
          this.valueText.Text = string.Empty;
        }
        else
        {
          fieldValueBase = (FieldValueBase) new DDM_Phone_NonNumericFieldValues(this._currentDataTableField.FieldID, this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), this.CombineZipStringAndList(this.zipCountryStateControl.ValuesExisting, this.zipCountryStateControl.ZipCollection.ToArray()));
          this._currentDataTableField.Value = fieldValueBase;
          this.ClearZipCountyControl();
        }
      }
      if (this._currentDataTableField.FieldType == FieldFormat.NONE)
      {
        this.pnlMultiSelect.Visible = false;
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.panel1.Visible = true;
        if (this.valueTypeCombo.SelectedIndex > 0 && this.valueTypeCombo.SelectedIndex != 3 && !this.validateOutput(this.valueText.Text, this.valueTypeCombo.SelectedItem.ToString() == "Calculation"))
          return false;
        if (this.valueTypeCombo.SelectedItem != null && (this.valueTypeCombo.SelectedItem.ToString() == "Please Select" || this.valueTypeCombo.SelectedItem.ToString() == "Clear value in loan file") && this.valueText != null)
          this.valueText.Text = string.Empty;
        fieldValueBase = this.valueTypeCombo.SelectedItem != null ? (FieldValueBase) new DDM_OUTPUT_FieldValues(this.getNumericCriteriaFromItem(this.valueTypeCombo.SelectedItem.ToString()), this.valueText != null ? this.valueText.Text : string.Empty) : (FieldValueBase) new DDM_OUTPUT_FieldValues(DDMCriteria.none, this.valueText != null ? this.valueText.Text : string.Empty);
        this._currentDataTableField.Value = fieldValueBase;
        if (this.valueText != null)
          this.valueText.Text = string.Empty;
      }
      if (fieldValueBase != null)
      {
        this.DtFieldValues.DataTableFields[this.DtFieldValues.CurrentItemIndex].Value = fieldValueBase;
        this.DtFieldValues.DataTableFields[this.DtFieldValues.CurrentItemIndex].IsDirty = true;
      }
      return true;
    }

    private bool checkValidNumChar(string str)
    {
      int num = this.triggerField != null ? this.triggerField.MaxLength : 14;
      try
      {
        return str.Length <= num || Decimal.Truncate(Utils.ParseDecimal((object) str)).ToString().Length <= num;
      }
      catch
      {
        return false;
      }
    }

    private void ClearZipCountyControl()
    {
      this.zipCountryStateControl.ZipCollection.Clear();
      this.zipCountryStateControl.ValuesExisting = string.Empty;
      this.zipCountryStateControl.SelectedStateIndex = 0;
      this.panel1.Controls.Remove((Control) this.zipCountryStateControl);
      this.ZipCountyObj.ExistingValue = string.Empty;
      this.ZipCountyObj.SelectedItems.Clear();
      this.ZipCountyObj.SelectedStateIdx = -1;
    }

    private string[] CombineZipStringAndList(
      string existingZip,
      string[] zips,
      bool overwriteExisting = true)
    {
      List<string> stringList = new List<string>();
      if (string.IsNullOrEmpty(existingZip))
        return zips;
      if (!overwriteExisting)
        stringList.AddRange((IEnumerable<string>) existingZip.Split(','));
      stringList.AddRange((IEnumerable<string>) zips);
      return stringList.ToArray();
    }

    private bool validateOutput(string text, bool isAdvancedCode = false)
    {
      if (isAdvancedCode)
      {
        if (!string.IsNullOrEmpty(text))
          return this.validateCalculation();
      }
      else if (text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please enter a value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      return true;
    }

    private bool validateValueList(string fieldID, string[] values)
    {
      if (!(!fieldID.ToLower().StartsWith("cx.") ? EncompassFields.GetField(fieldID) : EncompassFields.GetField(fieldID, this.session.LoanManager.GetFieldSettings())).Options.RequireValueFromList || ((IEnumerable<string>) values).Count<string>() != 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "One or more items must be selected within the Values list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void DataTableOkBtnClick()
    {
      if (!this.ValidateAndSetCurrentChangesToCollection())
        return;
      if (this.checkEmptyForAllFields())
      {
        switch (MessageBox.Show((IWin32Window) this, "Empty data row will not be saved. Do you want to continue?", "Save Changes?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
        {
          case DialogResult.OK:
            this.deleteEmptyRowInDataTable = true;
            this.Close();
            this.DialogResult = DialogResult.OK;
            return;
          case DialogResult.Cancel:
            this.PopulateDataTableControlValues();
            return;
        }
      }
      this.Close();
      this.DialogResult = DialogResult.OK;
    }

    private bool checkEmptyForAllFields()
    {
      bool flag = true;
      foreach (DataTableField dataTableField in this.DtFieldValues.DataTableFields)
      {
        if (dataTableField.Value.Criteria != DDMCriteria.OP_SpecificValue && dataTableField.Value.Criteria != DDMCriteria.OP_AdvancedCoding && dataTableField.Value.Criteria != DDMCriteria.none && dataTableField.Value.Criteria != DDMCriteria.IgnoreValueInLoanFile)
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    private void valueTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      FieldOptionCollection options = (FieldOptionCollection) null;
      string selectedItem = (string) this.valueTypeCombo.SelectedItem;
      this.value_lbl.Text = "Value";
      string str = "set the value";
      if (this.ddmCurrentTab == FeeValueDlg.ddmTab.FeeField && this.triggerField != null && this.triggerField.FieldLockIcon)
      {
        this.pnlRuleOverride.Visible = true;
        this.offsetUIForLockableField = selectedItem != "Use Calculated Value" && selectedItem != "No Value Set";
      }
      this.RefreshFormLayout(selectedItem);
      this.specificValueMode = FeeValueDlg.SpecificValueMode.TextBox;
      if (this.ddmCurrentTab == FeeValueDlg.ddmTab.FeeField)
      {
        this.panel1.Controls.Clear();
        this.panel2.Controls.Clear();
        this.value_lbl.Visible = true;
        switch ((string) this.valueTypeCombo.SelectedItem)
        {
          case "Specific Value":
            if (this.IsFeeManagementField)
            {
              this.CreateDropdownForFeeManagement();
              this.panel2.Visible = true;
              this.panel1.Visible = false;
              break;
            }
            switch (this.curState)
            {
              case FeeValueDlg.CurrentState.TableValText:
                this.tableValueText = this.valueText.Text;
                break;
              case FeeValueDlg.CurrentState.AdvanceCodeValText:
                this.advancedCodeText = this.valueText.Text;
                break;
            }
            this.curState = FeeValueDlg.CurrentState.SpecificValText;
            this.getValueSpecificControls();
            if (this.IsDropDownField(this.feeRuleValue.Field_Type, this.feeRuleValue.FieldID, out options))
            {
              if (this.valueCombo != null)
              {
                this.valueCombo.Visible = true;
                this.setSelectedItemForValueCombo(this.valueCombo, this.specificValueText);
              }
              if (this.valueText != null)
                this.valueText.Visible = false;
            }
            else if (this.specificValueMode == FeeValueDlg.SpecificValueMode.TextBox)
            {
              if (this.valueText != null)
              {
                this.valueText.Visible = true;
                this.valueText.Text = this.specificValueText;
              }
              if (this.valueCombo != null)
                this.valueCombo.Visible = false;
            }
            else
            {
              if (this.valueText != null)
              {
                this.valueText.Visible = false;
                this.valueText.Text = this.specificValueText;
              }
              if (this.valueCombo != null)
                this.valueCombo.Visible = true;
            }
            this.panel2.Visible = true;
            this.value_lbl.Visible = true;
            this.panel1.Visible = false;
            if (this.valueText != null)
            {
              this.valueText.ReadOnly = false;
              break;
            }
            break;
          case "No Value Set":
            switch (this.curState)
            {
              case FeeValueDlg.CurrentState.SpecificValText:
                this.specificValueText = this.getCurrentFieldValue();
                break;
              case FeeValueDlg.CurrentState.TableValText:
                this.tableValueText = this.valueText.Text;
                break;
              case FeeValueDlg.CurrentState.AdvanceCodeValText:
                this.advancedCodeText = this.valueText.Text;
                break;
            }
            this.curState = FeeValueDlg.CurrentState.NoVal;
            this.getNoValueSetControls();
            this.panel2.Visible = true;
            this.value_lbl.Visible = true;
            this.panel1.Visible = false;
            this.pnlRuleOverride.Visible = false;
            break;
          case "Calculation":
            switch (this.curState)
            {
              case FeeValueDlg.CurrentState.SpecificValText:
                this.specificValueText = this.getCurrentFieldValue();
                break;
              case FeeValueDlg.CurrentState.TableValText:
                this.tableValueText = this.valueText.Text;
                break;
            }
            this.curState = FeeValueDlg.CurrentState.AdvanceCodeValText;
            if (this.valueText != null)
            {
              this.valueText.Text = this.advancedCodeText;
              this.valueText.ReadOnly = false;
            }
            this.getAdvancedCodingControls();
            this.panel2.Visible = false;
            this.value_lbl.Visible = false;
            this.panel1.Visible = true;
            this.panel1.BorderStyle = BorderStyle.Fixed3D;
            break;
          case "Table":
            this.value_lbl.Text = "Table Name";
            switch (this.curState)
            {
              case FeeValueDlg.CurrentState.SpecificValText:
                this.specificValueText = this.getCurrentFieldValue();
                break;
              case FeeValueDlg.CurrentState.AdvanceCodeValText:
                this.advancedCodeText = this.valueText.Text;
                break;
            }
            this.curState = FeeValueDlg.CurrentState.TableValText;
            this.panel1.Visible = false;
            this.getDdmDataTableControls();
            break;
          case "Use Calculated Value":
            switch (this.curState)
            {
              case FeeValueDlg.CurrentState.SpecificValText:
                this.specificValueText = this.getCurrentFieldValue();
                break;
              case FeeValueDlg.CurrentState.TableValText:
                this.tableValueText = this.valueText.Text;
                break;
              case FeeValueDlg.CurrentState.AdvanceCodeValText:
                this.advancedCodeText = this.valueText.Text;
                break;
            }
            this.curState = FeeValueDlg.CurrentState.NoVal;
            this.getNoValueSetControls();
            this.panel2.Visible = true;
            this.value_lbl.Visible = true;
            this.panel1.Visible = false;
            this.pnlRuleOverride.Visible = false;
            break;
          case "Clear value in loan file":
            switch (this.curState)
            {
              case FeeValueDlg.CurrentState.SpecificValText:
                this.specificValueText = this.getCurrentFieldValue();
                break;
              case FeeValueDlg.CurrentState.TableValText:
                this.tableValueText = this.valueText.Text;
                break;
              case FeeValueDlg.CurrentState.AdvanceCodeValText:
                this.advancedCodeText = this.valueText.Text;
                break;
            }
            this.curState = FeeValueDlg.CurrentState.NoVal;
            this.getNoValueSetControls();
            this.panel2.Visible = true;
            this.value_lbl.Visible = true;
            this.panel1.Visible = false;
            str = "clear the value";
            break;
        }
        if (this.triggerField.FieldLockIcon)
          this.overrideLabel.Text = string.Format("(The rule will override the calculation automatically and {0}. The field will be displayed", (object) str);
      }
      if (this.ddmCurrentTab == FeeValueDlg.ddmTab.DataTable)
      {
        this.panel2.Controls.Clear();
        this.panel1.Visible = true;
        this.populatefields();
      }
      this.IsDirty = true;
    }

    private string getCurrentFieldValue()
    {
      FieldOptionCollection options = (FieldOptionCollection) null;
      if (!this.IsDropDownField(this.feeRuleValue.Field_Type, this.feeRuleValue.FieldID, out options))
        return this.valueText.Text;
      return this.valueCombo.SelectedItem != null ? this.getSettingValueFromValueCombo(this.valueCombo) : string.Empty;
    }

    private string getValidationMsg(FieldFormat fieldType)
    {
      switch (fieldType)
      {
        case FieldFormat.ZIPCODE:
          return "XXXXX or XXXXX-XXXX";
        case FieldFormat.STATE:
          return "Two Letter Abbreviation of the State";
        case FieldFormat.PHONE:
          return "XXX-XXX-XXXX or XXX-XXX-XXXX XXXX";
        case FieldFormat.SSN:
          return "XXX-XX-XXXX";
        case FieldFormat.DECIMAL_1:
          return "X,XXX.X";
        case FieldFormat.DECIMAL_2:
          return "X,XXX.XX";
        case FieldFormat.DECIMAL_3:
          return "X,XXX.XXX";
        case FieldFormat.DECIMAL_4:
          return "X,XXX.XXXX";
        case FieldFormat.DECIMAL_6:
          return "X,XXX.XXXXXX";
        case FieldFormat.DECIMAL_5:
          return "X,XXX.XXXXX";
        case FieldFormat.DECIMAL_7:
          return "X,XXX.XXXXXXX";
        case FieldFormat.DECIMAL_10:
          return "X,XXX.XXXXXXXX";
        case FieldFormat.DATE:
          return "MM/DD/YYYY";
        default:
          return "";
      }
    }

    private void populatefields()
    {
      this.panel1.Visible = false;
      this.clearPanel1Controls();
      this.numericValuePnl.Visible = false;
      this.numericRangePnl.Visible = false;
      this.datevaluePnl.Visible = false;
      this.dateRangePnl.Visible = false;
      this.numericPnl.Visible = false;
      this.datePnl.Visible = false;
      this.numericDatePnl.Visible = true;
      this.nextPrevBtnPnl.Visible = true;
      this.currentValueControlMode = FeeValueDlg.ValueControlMode.RegularTextbox;
      if ((string) this.valueTypeCombo.SelectedItem == "=" || (string) this.valueTypeCombo.SelectedItem == "<" || (string) this.valueTypeCombo.SelectedItem == "<=" || (string) this.valueTypeCombo.SelectedItem == ">" || (string) this.valueTypeCombo.SelectedItem == ">=" || (string) this.valueTypeCombo.SelectedItem == "<>")
      {
        if (this.IsNumericField(this._currentDataTableField.FieldType))
        {
          this.datePnl.Visible = false;
          this.numericPnl.Visible = true;
          this.numericValuePnl.Visible = true;
          this.numericRangePnl.Visible = false;
        }
        if (this.IsDateField(this._currentDataTableField.FieldType))
        {
          this.numericPnl.Visible = false;
          this.datePnl.Visible = true;
          this.datevaluePnl.Visible = true;
          this.dateRangePnl.Visible = false;
        }
        this.panel1.Visible = true;
        this.pnlMultiSelect.Visible = false;
      }
      if ((string) this.valueTypeCombo.SelectedItem == "Range")
      {
        if (this.IsNumericField(this._currentDataTableField.FieldType))
        {
          this.datePnl.Visible = false;
          this.numericPnl.Visible = true;
          this.numericValuePnl.Visible = false;
          this.numericRangePnl.Visible = true;
        }
        if (this.IsDateField(this._currentDataTableField.FieldType))
        {
          this.numericPnl.Visible = false;
          this.datePnl.Visible = true;
          this.dateRangePnl.Visible = true;
          this.datevaluePnl.Visible = false;
        }
        this.panel1.Visible = true;
        this.pnlMultiSelect.Visible = false;
      }
      if (((string) this.valueTypeCombo.SelectedItem == "Checkbox" || this._currentDataTableField.FieldType == FieldFormat.STATE) && (string) this.valueTypeCombo.SelectedItem != "Ignore this field" && (string) this.valueTypeCombo.SelectedItem != "No value in loan file")
      {
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.dateRangePnl.Visible = false;
        this.datevaluePnl.Visible = false;
        this.panel1.Visible = true;
        this.pnlMultiSelect.Visible = true;
      }
      if ((string) this.valueTypeCombo.SelectedItem == "List Of Values" && this._currentDataTableField.FieldType != FieldFormat.STATE && (string) this.valueTypeCombo.SelectedItem != "Ignore this field" && (string) this.valueTypeCombo.SelectedItem != "No value in loan file")
      {
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.dateRangePnl.Visible = false;
        this.datevaluePnl.Visible = false;
        this.getddmListOfValuesControls();
        this.panel2.Visible = false;
        this.value_lbl.Visible = false;
        this.panel1.Visible = true;
      }
      if ((string) this.valueTypeCombo.SelectedItem == "Find County" && DDM_FieldAccess_Utils.IsCountyField(this._currentDataTableField.FieldID))
      {
        this.numericDatePnl.Visible = false;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.dateRangePnl.Visible = false;
        this.datevaluePnl.Visible = false;
        if (this.valueText != null && !string.IsNullOrEmpty(this.valueText.Text))
          this.specificValueText = this.valueText.Text;
        this.getddmFindByCountryControls(true);
        if (this.ZipCountyObj != null)
        {
          this.zipCountryStateControl.DestinationList = this.ZipCountyObj.SelectedItems;
          this.zipCountryStateControl.SelectedStateIndex = this.ZipCountyObj.SelectedStateIdx;
          this.zipCountryStateControl.ValuesExisting = this.ZipCountyObj.ExistingValue;
        }
        this.panel2.Visible = false;
        this.value_lbl.Visible = false;
        this.panel1.Visible = true;
        this.panel1.AutoSize = true;
      }
      if ((string) this.valueTypeCombo.SelectedItem == this.DT_COUNTY_Criterion[1] && this._currentDataTableField.FieldType == FieldFormat.STRING && (string) this.valueTypeCombo.SelectedItem == "Specific Value")
      {
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.dateRangePnl.Visible = false;
        this.datevaluePnl.Visible = false;
        this.pnlMultiSelect.Visible = false;
        this.panel1.Visible = false;
        this.panel2.Visible = true;
        this.value_lbl.Visible = true;
        if (this.zipCountryStateControl != null)
        {
          this.ZipCountyObj.SelectedItems = this.zipCountryStateControl.DestinationList;
          this.ZipCountyObj.SelectedStateIdx = this.zipCountryStateControl.SelectedStateIndex;
          this.ZipCountyObj.ExistingValue = this.zipCountryStateControl.ValuesExisting;
        }
        this.createTextBox(true);
        if (!string.IsNullOrEmpty(this.specificValueText))
          this.valueText.Text = this.specificValueText;
        this.panel2.Controls.Add((Control) this.valueText);
      }
      if ((string) this.valueTypeCombo.SelectedItem == this.DT_SSN_Criterion[1] && this._currentDataTableField.FieldType == FieldFormat.SSN)
      {
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.dateRangePnl.Visible = false;
        this.datevaluePnl.Visible = false;
        this.pnlMultiSelect.Visible = false;
        this.panel1.Visible = false;
        this.panel2.Visible = true;
        this.value_lbl.Visible = true;
        this.createTextBox(true);
        this.panel2.Controls.Add((Control) this.valueText);
      }
      if (((IEnumerable<object>) this.DT_NonNumeric_Phone_Criterion).Contains<object>(this.valueTypeCombo.SelectedItem) && !string.Equals(this.valueTypeCombo.SelectedItem.ToString(), "Ignore this field", StringComparison.InvariantCultureIgnoreCase) && (this._currentDataTableField.FieldType == FieldFormat.RA_STRING || this._currentDataTableField.FieldType == FieldFormat.RA_INTEGER || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_2 || this._currentDataTableField.FieldType == FieldFormat.RA_DECIMAL_3 || this._currentDataTableField.FieldType == FieldFormat.DROPDOWN || this._currentDataTableField.FieldType == FieldFormat.DROPDOWNLIST || this._currentDataTableField.FieldType == FieldFormat.STRING || this._currentDataTableField.FieldType == FieldFormat.PHONE))
      {
        this.value_lbl.Visible = true;
        this.numericPnl.Visible = false;
        this.datePnl.Visible = false;
        this.dateRangePnl.Visible = false;
        this.datevaluePnl.Visible = false;
        this.pnlMultiSelect.Visible = false;
        this.panel1.Visible = false;
        this.panel2.Visible = true;
        this.value_lbl.Visible = true;
        FieldOptionCollection options;
        if (this.IsDropDownField(this._currentDataTableField.FieldType, this._currentDataTableField.FieldID, out options) && (string) this.valueTypeCombo.SelectedItem != "No value in loan file" && (string) this.valueTypeCombo.SelectedItem != "Ignore this field")
        {
          this.currentValueControlMode = FeeValueDlg.ValueControlMode.DropDownNonEdit;
          this.replaceValueTextWithCombo(this._currentDataTableField.FieldID, options: options);
          this.panel2.Controls.Add((Control) this.valueCombo);
          this.createTextBox(true);
          this.valueText.Visible = false;
        }
        else
        {
          this.createTextBox(true);
          this.panel2.Controls.Add((Control) this.valueText);
        }
        if (DDM_FieldAccess_Utils.IsRolodexField(this._currentDataTableField.FieldID))
          this.createRolodex();
      }
      if (this._currentDataTableField.FieldType == FieldFormat.ZIPCODE)
      {
        if ((string) this.valueTypeCombo.SelectedItem == "Specific Value")
        {
          this.numericPnl.Visible = false;
          this.datePnl.Visible = false;
          this.dateRangePnl.Visible = false;
          this.datevaluePnl.Visible = false;
          this.pnlMultiSelect.Visible = false;
          this.panel1.Visible = false;
          this.panel2.Visible = true;
          this.value_lbl.Visible = true;
          if (this.zipCountryStateControl != null)
          {
            this.ZipCountyObj.SelectedItems = this.zipCountryStateControl.DestinationList;
            this.ZipCountyObj.SelectedStateIdx = this.zipCountryStateControl.SelectedStateIndex;
            this.ZipCountyObj.ExistingValue = this.zipCountryStateControl.ValuesExisting;
          }
          this.createTextBox(true);
          if (!string.IsNullOrEmpty(this.specificValueText))
            this.valueText.Text = this.specificValueText;
          this.panel2.Controls.Add((Control) this.valueText);
        }
        if ((string) this.valueTypeCombo.SelectedItem == "Find Zip")
        {
          this.numericDatePnl.Visible = false;
          this.numericPnl.Visible = false;
          this.datePnl.Visible = false;
          this.dateRangePnl.Visible = false;
          this.datevaluePnl.Visible = false;
          if (this.valueText != null && !string.IsNullOrEmpty(this.valueText.Text))
            this.specificValueText = this.valueText.Text;
          this.getddmFindByCountryControls();
          if (this.ZipCountyObj != null)
          {
            this.zipCountryStateControl.DestinationList = this.ZipCountyObj.SelectedItems;
            this.zipCountryStateControl.SelectedStateIndex = this.ZipCountyObj.SelectedStateIdx;
            this.zipCountryStateControl.ValuesExisting = this.ZipCountyObj.ExistingValue;
          }
          this.panel2.Visible = false;
          this.value_lbl.Visible = false;
          this.panel1.Visible = true;
          this.panel1.AutoSize = true;
        }
      }
      if (this._currentDataTableField.FieldType == FieldFormat.NONE)
      {
        if ((string) this.valueTypeCombo.SelectedItem == "Specific Value")
        {
          this.numericPnl.Visible = false;
          this.datePnl.Visible = false;
          this.dateRangePnl.Visible = false;
          this.datevaluePnl.Visible = false;
          this.pnlMultiSelect.Visible = false;
          this.panel1.Visible = false;
          this.panel2.Visible = true;
          this.value_lbl.Visible = true;
          if (this.valueText != null && this.valueText.Width != 226)
          {
            this.advancedCodeText = this.valueText.Text;
            this.valueText.Text = this.specificValueText;
          }
          this.createTextBox();
          this.panel2.Controls.Add((Control) this.valueText);
          if (DDM_FieldAccess_Utils.IsRolodexField(this._currentDataTableField.FieldID))
            this.createRolodex();
        }
        if ((string) this.valueTypeCombo.SelectedItem == "Calculation")
        {
          this.numericPnl.Visible = false;
          this.numericDatePnl.Visible = false;
          this.datePnl.Visible = false;
          this.dateRangePnl.Visible = false;
          this.datevaluePnl.Visible = false;
          this.pnlMultiSelect.Visible = false;
          this.getAdvancedCodeForOutput();
          this.panel2.Visible = false;
          this.value_lbl.Visible = false;
          this.panel1.Visible = true;
          this.panel1.BorderStyle = BorderStyle.Fixed3D;
        }
        if ((string) this.valueTypeCombo.SelectedItem == "Clear value in loan file")
          this.value_lbl.Visible = false;
      }
      if (!string.Equals(this.valueTypeCombo.SelectedItem.ToString(), "Ignore this field", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(this.valueTypeCombo.SelectedItem.ToString(), "No value in loan file", StringComparison.InvariantCultureIgnoreCase) || this.valueText == null)
        return;
      this.valueText.Text = string.Empty;
      this.value_lbl.Visible = false;
      this.valueText.Visible = false;
    }

    private void replaceValueTextWithCombo(
      string fieldId,
      bool editable = false,
      FieldOptionCollection options = null)
    {
      if (this.valueText != null)
        this.valueText.Visible = false;
      if (this.valueCombo != null)
        this.valueCombo.Visible = true;
      else
        this.createCombo(editable);
      this.valueCombo.Items.Clear();
      this.valueCombo.Items.Add((object) "");
      if (options == null)
        return;
      foreach (object obj in options.GetValues())
        this.valueCombo.Items.Add(obj);
      this.adjustDropDownWidth(this.valueCombo);
    }

    private void adjustDropDownWidth(ComboBox cmbControl)
    {
      int dropDownWidth = cmbControl.DropDownWidth;
      int num = 0;
      Label label = new Label();
      foreach (object obj in cmbControl.Items)
      {
        label.Text = obj.ToString();
        int preferredWidth = label.PreferredWidth;
        if (preferredWidth > num)
          num = preferredWidth;
      }
      label.Dispose();
      cmbControl.DropDownWidth = num > 0 ? num : dropDownWidth;
    }

    private void RefreshFormLayout(string selectedText)
    {
      this.pnlOutput.Visible = false;
      this.panel1.Size = new Size(485, 252);
      this.numericPnl.Location = new Point(0, 0);
      this.numericValuePnl.Location = new Point(0, 0);
      this.numericRangePnl.Location = new Point(0, 0);
      this.datePnl.Location = new Point(0, 0);
      this.datevaluePnl.Location = new Point(0, 0);
      this.dateRangePnl.Location = new Point(0, 0);
      this.pnlMultiSelect.Location = new Point(0, 0);
      this.panel1.Location = new Point(36, 173);
      if (this.ddmCurrentTab == FeeValueDlg.ddmTab.FeeField)
      {
        this.groupBox1.Location = new Point(19, 10);
        if (this.getLayoutMode(selectedText).Equals("small"))
        {
          this.panel1.Height = 50;
          if (this.offsetUIForLockableField)
          {
            this.Height = this.Form_Mtrx_Height_Small;
            this.groupBox1.Height = this.GroupBox_Mtrx_Height_Small + this.Layout_Mtrx_Offset;
            int y = 173;
            if (selectedText.Equals("Table"))
            {
              y += 25;
              this.Height += 25;
              this.groupBox1.Height += 25;
            }
            this.pnlRuleOverride.Location = new Point(4, y);
          }
          else
          {
            this.groupBox1.Height = this.GroupBox_Mtrx_Height_Small;
            this.Height = this.Form_Mtrx_Height_Small - this.Layout_Mtrx_Offset;
          }
        }
        else
        {
          this.panel1.Height = 232;
          this.panel1.PerformLayout();
          this.groupBox1.Height = this.GroupBox_Mtrx_Height_Large;
          if (this.offsetUIForLockableField)
          {
            this.panel1.Location = new Point(36, 193);
            this.pnlRuleOverride.Location = new Point(4, 143);
          }
          this.groupBox1.Height = this.GroupBox_Mtrx_Height_Large;
          this.Height = this.Form_Mtrx_Height_Large - this.Layout_Mtrx_Offset;
        }
      }
      else
      {
        this.groupBox1.Location = new Point(19, 55);
        this.panel1.Location = new Point(36, 146);
        if (this.getLayoutMode(selectedText).Equals("small"))
        {
          this.panel1.Height = 50;
          this.groupBox1.Height = this.GroupBox_Mtrx_Height_Small;
          this.Height = this.Form_Mtrx_Height_Small;
        }
        else
        {
          this.panel1.Height = 232;
          this.groupBox1.Height = this.GroupBox_Mtrx_Height_Large;
          this.Height = this.Form_Mtrx_Height_Large;
        }
      }
    }

    private string getLayoutMode(string selectedOp)
    {
      string lower = selectedOp.ToLower();
      return lower == "calculation" || lower == "list of values" || lower == "checkbox" || lower == "find zip" || lower == "find county" ? "large" : "small";
    }

    private void getNoValueSetControls()
    {
      this.createTextBox();
      this.valueText.Text = string.Empty;
      this.valueText.Enabled = false;
      this.panel2.Controls.Add((Control) this.valueText);
    }

    private DDMCriteria getNumericCriteriaFromItem(string option)
    {
      switch (option)
      {
        case "<":
          return DDMCriteria.LessThan;
        case "<=":
          return DDMCriteria.LessThanOrEqual;
        case "<>":
          return DDMCriteria.NotEqual;
        case "=":
          return DDMCriteria.Equals;
        case ">":
          return DDMCriteria.GreaterThan;
        case ">=":
          return DDMCriteria.GreaterThanOrEqual;
        case "Begins with":
          return DDMCriteria.strBegins;
        case "Calculation":
          return DDMCriteria.OP_AdvancedCoding;
        case "Clear value in loan file":
          return DDMCriteria.OP_ClearValueInLoanFile;
        case "Contains":
          return DDMCriteria.strContains;
        case "Does not contain":
          return DDMCriteria.strNotContains;
        case "Does not equal":
          return DDMCriteria.strNotEqual;
        case "Ends with":
          return DDMCriteria.strEnds;
        case "Equals":
          return DDMCriteria.strEquals;
        case "Find County":
          return DDMCriteria.county_FindByCounty;
        case "Find Zip":
          return DDMCriteria.zip_FindByZip;
        case "Range":
          return DDMCriteria.Range;
        case "Specific Value":
          if (this._currentDataTableField.FieldType == FieldFormat.ZIPCODE)
            return DDMCriteria.zip_SpecificValue;
          return DDM_FieldAccess_Utils.IsCountyField(this._currentDataTableField.FieldID) ? DDMCriteria.county_SpecificValue : DDMCriteria.OP_SpecificValue;
        default:
          return DDMCriteria.none;
      }
    }

    private string getItemFromNumericCriteria(DDMCriteria option)
    {
      switch (option)
      {
        case DDMCriteria.none:
          return "Ignore this field";
        case DDMCriteria.Equals:
          return "=";
        case DDMCriteria.LessThan:
          return "<";
        case DDMCriteria.LessThanOrEqual:
          return "<=";
        case DDMCriteria.GreaterThan:
          return ">";
        case DDMCriteria.GreaterThanOrEqual:
          return ">=";
        case DDMCriteria.NotEqual:
          return "<>";
        case DDMCriteria.Range:
          return "Range";
        case DDMCriteria.CheckBox:
          return "CheckBox";
        case DDMCriteria.SSN_SpecificValue:
        case DDMCriteria.OP_SpecificValue:
        case DDMCriteria.zip_SpecificValue:
        case DDMCriteria.county_SpecificValue:
          return "Specific Value";
        case DDMCriteria.SSN_ListofValues:
        case DDMCriteria.ListOfValues:
        case DDMCriteria.st_ListOfValues:
          return "List Of Values";
        case DDMCriteria.strEquals:
          return "Equals";
        case DDMCriteria.strNotEqual:
          return "Does not equal";
        case DDMCriteria.strContains:
          return "Contains";
        case DDMCriteria.strNotContains:
          return "Does not contain";
        case DDMCriteria.strBegins:
          return "Begins with";
        case DDMCriteria.strEnds:
          return "Ends with";
        case DDMCriteria.OP_AdvancedCoding:
          return "Calculation";
        case DDMCriteria.zip_FindByZip:
          return "Find Zip";
        case DDMCriteria.county_FindByCounty:
          return "Find County";
        case DDMCriteria.NoValueInLoanFile:
          return "No value in loan file";
        case DDMCriteria.OP_ClearValueInLoanFile:
          return "Clear value in loan file";
        default:
          return "Ignore this field";
      }
    }

    private bool validateCalculation()
    {
      if (this.valueText.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Calculation field cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.valueText.Focus();
        return false;
      }
      using (RuntimeContext context = RuntimeContext.Create())
      {
        try
        {
          CustomFieldInfo customFieldInfo = new CustomFieldInfo("CX.TEST", FieldFormat.DECIMAL);
          customFieldInfo.Calculation = this.valueText.Text.Trim();
          if (new CustomFieldsInfo(new BinaryObject(new CustomFieldsInfo(false)
          {
            customFieldInfo
          }.ToString(), Encoding.Default).ToString(Encoding.Default)).GetField("CX.TEST").Calculation != customFieldInfo.Calculation)
          {
            int num = (int) MessageBox.Show((IWin32Window) this, "Validation failed: the calculation contains one or more invalid characters.", "Calculation Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          new CalculationBuilder().CreateImplementation(new CustomCalculation(customFieldInfo.Calculation), context);
          return true;
        }
        catch (CompileException ex)
        {
          if (EnConfigurationSettings.GlobalSettings.Debug)
          {
            ErrorDialog.Display((Exception) ex);
            return false;
          }
          int num = (int) MessageBox.Show((IWin32Window) this, "Validation failed: the calculation contains errors or is not a valid formula.", "Calculation Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        catch (Exception ex)
        {
          ErrorDialog.Display(ex);
          return false;
        }
      }
    }

    private void createAdvancedCodingControls()
    {
      if (this.valueText == null)
      {
        this.valueText = new TextBox();
        this.valueText.Size = new Size(460, 170);
        this.valueText.Name = "valueText";
      }
      this.valueText.Visible = true;
      this.valueText.Width = 460;
      this.valueText.Height = 170;
      this.valueText.Enabled = true;
      this.valueText.Multiline = true;
      this.valueText.Location = new Point(10, 27);
      this.valueText.ScrollBars = ScrollBars.Both;
      this.valueText.WordWrap = false;
      if (this.advancedCodingLabel == null)
      {
        this.advancedCodingLabel = new Label();
        this.advancedCodingLabel.Size = new Size(325, 20);
        this.advancedCodingLabel.Location = new Point(10, 3);
        this.advancedCodingLabel.Text = "Calculation";
      }
      if (this.validationBtn != null)
        return;
      this.validationBtn = new Button();
      this.validationBtn.Size = new Size(80, 25);
      this.validationBtn.Location = new Point(390, 203);
      this.validationBtn.Text = "Validation";
      this.validationBtn.Click += new EventHandler(this.validateBtn_click);
    }

    private void getAdvancedCodingControls(bool set = false)
    {
      this.createAdvancedCodingControls();
      this.panel1.Controls.Add((Control) this.advancedCodingLabel);
      this.panel1.Controls.Add((Control) this.valueText);
      this.panel1.Controls.Add((Control) this.validationBtn);
      if (!set)
        return;
      this.valueText.Text = this.feeRuleValue.Field_Value;
    }

    private void createCheckBoxesForEnumeratedField()
    {
      this.triggerField = this.fieldIDTextBx.Text.ToLower().StartsWith("cx.") ? EncompassFields.GetField(this.fieldIDTextBx.Text, this.session.LoanManager.GetFieldSettings()) : EncompassFields.GetField(this.fieldIDTextBx.Text);
      this.value_lbl.Visible = false;
      if (this.listOfValuesLabel == null)
      {
        this.listOfValuesLabel = new Label();
        this.listOfValuesLabel.Text = "Values";
      }
      this.listOfValuesLabel.Visible = true;
      this.listOfValuesLabel.Width = 42;
      this.listOfValuesLabel.Height = 20;
      this.listOfValuesLabel.Location = new Point(-1, 7);
      if (this.chkOptionFieldItems != null)
        this.chkOptionFieldItems.Clear();
      else
        this.chkOptionFieldItems = new List<CheckBox>();
      FieldOptionCollection options;
      if (this.IsDropDownField(this._currentDataTableField.FieldType, this._currentDataTableField.FieldID, out options))
      {
        int x = 3;
        int y = 3;
        CheckBox checkBox1 = new CheckBox();
        checkBox1.Text = "<Blank>";
        checkBox1.Tag = (object) "";
        checkBox1.Width = 300;
        checkBox1.Height = 17;
        checkBox1.TextAlign = ContentAlignment.MiddleLeft;
        checkBox1.Location = new Point(x, y);
        this.chkOptionFieldItems.Add(checkBox1);
        foreach (string str in options.GetValues())
        {
          CheckBox checkBox2 = new CheckBox();
          checkBox2.Text = str;
          checkBox2.Tag = (object) str;
          checkBox2.Width = 300;
          checkBox2.Height = 17;
          checkBox2.TextAlign = ContentAlignment.MiddleLeft;
          checkBox2.Location = new Point(x, y += 16);
          this.chkOptionFieldItems.Add(checkBox2);
        }
      }
      this.grpOptionFieldControls = new Panel();
      this.grpOptionFieldControls.Location = new Point(106, 0);
      this.grpOptionFieldControls.Size = new Size(335, 235);
      this.grpOptionFieldControls.BackColor = Color.White;
      this.grpOptionFieldControls.AutoScroll = true;
      this.grpOptionFieldControls.Controls.AddRange((Control[]) this.chkOptionFieldItems.ToArray());
    }

    private void createListOfValuesControls()
    {
      this.triggerField = this.fieldIDTextBx.Text.ToLower().StartsWith("cx.") ? EncompassFields.GetField(this.fieldIDTextBx.Text, this.session.LoanManager.GetFieldSettings()) : EncompassFields.GetField(this.fieldIDTextBx.Text);
      this.value_lbl.Visible = false;
      if (this.listOfValuesLabel == null)
      {
        this.listOfValuesLabel = new Label();
        this.listOfValuesLabel.Text = "Values:";
      }
      this.listOfValuesLabel.Visible = true;
      this.listOfValuesLabel.Width = 42;
      this.listOfValuesLabel.Height = 20;
      this.listOfValuesLabel.Location = new Point(10, 3);
      if (this.listOfValuesListbox == null)
      {
        this.listOfValuesListbox = new ListView();
        this.listOfValuesListbox.Location = new Point(55, 3);
        this.listOfValuesListbox.Size = new Size(335, 235);
        this.listOfValuesListbox.LabelEdit = true;
        this.listOfValuesListbox.LabelWrap = false;
        this.listOfValuesListbox.View = View.List;
        this.listOfValuesListbox.FullRowSelect = true;
        this.listOfValuesListbox.Columns.AddRange(new ColumnHeader[1]
        {
          new ColumnHeader() { Text = "Values", Width = 258 }
        });
        this.listOfValuesListbox.FullRowSelect = true;
        this.listOfValuesListbox.HeaderStyle = ColumnHeaderStyle.None;
        this.listOfValuesListbox.HideSelection = false;
        this.listOfValuesListbox.LabelEdit = true;
        this.listOfValuesListbox.AfterLabelEdit += new LabelEditEventHandler(this.listOfValuesListbox_AfterLabelEdit);
        this.listOfValuesListbox.SelectedIndexChanged += new EventHandler(this.listOfValuesListbox_SelectedIndexChanged);
      }
      if (this.listOfValuesAddButton == null)
      {
        this.listOfValuesAddButton = new Button();
        this.listOfValuesAddButton.Size = new Size(80, 25);
        this.listOfValuesAddButton.Location = new Point(this.listOfValuesListbox.Size.Width + 65, 3);
        this.listOfValuesAddButton.Text = "Add";
        this.listOfValuesAddButton.Click += new EventHandler(this.listOfValuesAddButton_click);
      }
      if (this.listOfValuesEditButton == null)
      {
        this.listOfValuesEditButton = new Button();
        this.listOfValuesEditButton.Size = new Size(80, 25);
        this.listOfValuesEditButton.Location = new Point(this.listOfValuesListbox.Size.Width + 65, 30);
        this.listOfValuesEditButton.Text = "Edit";
        this.listOfValuesEditButton.Click += new EventHandler(this.listOfValuesEditButton_click);
        this.listOfValuesEditButton.Enabled = false;
      }
      if (this.listOfValuesRemoveButton != null)
        return;
      this.listOfValuesRemoveButton = new Button();
      this.listOfValuesRemoveButton.Size = new Size(80, 25);
      this.listOfValuesRemoveButton.Location = new Point(this.listOfValuesListbox.Size.Width + 65, 57);
      this.listOfValuesRemoveButton.Text = "Remove";
      this.listOfValuesRemoveButton.Click += new EventHandler(this.listOfValuesRemoveButton_click);
      this.listOfValuesRemoveButton.Enabled = false;
    }

    private void listOfValuesListbox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.listOfValuesEditButton.Enabled = this.listOfValuesRemoveButton.Enabled = this.listOfValuesListbox.SelectedItems.Count > 0;
    }

    private void listOfValuesListbox_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.validateConditionListEdit), (object) e.Item);
    }

    private void listOfValuesRemoveButton_click(object sender, EventArgs e)
    {
      if (this.listOfValuesListbox.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the value(s) from the list to be removed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Remove the selected values from the list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.removeSelectedListItems(this.listOfValuesListbox);
      }
    }

    private void removeSelectedListItems(ListView listView)
    {
      while (listView.SelectedIndices.Count > 0)
        listView.Items.RemoveAt(listView.SelectedIndices[0]);
    }

    private void listOfValuesEditButton_click(object sender, EventArgs e)
    {
      if (this.listOfValuesListbox.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Select the item from the list to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.listOfValuesListbox.SelectedItems.Count > 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Only one item can be selected to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.listOfValuesListbox.SelectedItems[0].BeginEdit();
    }

    private void listOfValuesAddButton_click(object sender, EventArgs e)
    {
      this.listOfValuesListbox.Items.Add(new ListViewItem("<New Value>")).BeginEdit();
    }

    private void validateConditionListEdit(object indexAsObj)
    {
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) new WaitCallback(this.validateConditionListEdit), indexAsObj);
      }
      else
      {
        int index = (int) indexAsObj;
        if (index >= this.listOfValuesListbox.Items.Count)
          return;
        ListViewItem listViewItem = this.listOfValuesListbox.Items[index];
        if (listViewItem.Text.Trim() == "")
        {
          listViewItem.Remove();
        }
        else
        {
          if (this.validateFormat(listViewItem.Text))
            return;
          int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + listViewItem.Text + "' is not valid for this field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          listViewItem.BeginEdit();
        }
      }
    }

    private bool validateFormat(string text)
    {
      bool needsUpdate = false;
      if (text.Trim() == "")
        return true;
      try
      {
        string str = this.triggerField.ValidateFormat(text);
        if (this._currentDataTableField.FieldType == FieldFormat.SSN || this.IsNumericField(this._currentDataTableField.FieldType) || this._currentDataTableField.FieldType == FieldFormat.PHONE || this._currentDataTableField.FieldType == FieldFormat.STATE)
        {
          Utils.FormatInput(text, this._currentDataTableField.FieldType, ref needsUpdate);
          return !needsUpdate;
        }
        if (this._currentDataTableField.FieldType == FieldFormat.DATETIME)
        {
          if (!(str != "0001-01-01 00:00:00"))
            return false;
          Utils.FormatInput(Convert.ToDateTime(str).ToString("MM/dd/yyyy"), FieldFormat.DATE, ref needsUpdate);
          if (needsUpdate)
            return false;
        }
        if (this._currentDataTableField.FieldType == FieldFormat.DATE)
        {
          Utils.FormatInput(Convert.ToDateTime(str).ToString("MM/dd/yyyy"), FieldFormat.DATE, ref needsUpdate);
          if (needsUpdate)
            return false;
        }
        if (str != "")
          return true;
      }
      catch
      {
      }
      return false;
    }

    private void getddmListOfValuesControls()
    {
      if (FieldValidationUtil.HasOptions(this._currentDataTableField.FieldID, this.session))
      {
        this.currentValueControlMode = FeeValueDlg.ValueControlMode.Checkboxes;
        this.createCheckBoxesForEnumeratedField();
        this.panel1.Controls.Add((Control) this.listOfValuesLabel);
        this.panel1.Controls.Add((Control) this.grpOptionFieldControls);
        if (this._currentDataTableField.Value.ToString() == null)
          return;
        string[] strArray = this._currentDataTableField.Value.ToString().Split('|');
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (string key in strArray)
          dictionary.Add(key, key);
        foreach (CheckBox chkOptionFieldItem in this.chkOptionFieldItems)
        {
          if (dictionary.ContainsKey(chkOptionFieldItem.Tag.ToString()))
            chkOptionFieldItem.Checked = true;
        }
      }
      else
      {
        this.createListOfValuesControls();
        this.panel1.Controls.Add((Control) this.listOfValuesLabel);
        this.panel1.Controls.Add((Control) this.listOfValuesListbox);
        this.panel1.Controls.Add((Control) this.listOfValuesAddButton);
        this.panel1.Controls.Add((Control) this.listOfValuesEditButton);
        this.panel1.Controls.Add((Control) this.listOfValuesRemoveButton);
        this.listOfValuesListbox.Items.Clear();
        if (string.IsNullOrEmpty(this._currentDataTableField.Value.ToString()))
          return;
        string str = this._currentDataTableField.Value.ToString();
        char[] chArray = new char[1]{ '|' };
        foreach (string text in str.Split(chArray))
          this.listOfValuesListbox.Items.Add(text);
      }
    }

    private void getddmFindByCountryControls(bool IsZip = false)
    {
      this.zipCountryStateControl = new ZipCountyStateCtrl(IsZip, this.session);
      this.zipCountryStateControl.Visible = true;
      this.zipCountryStateControl.Width = 500;
      this.zipCountryStateControl.Height = 300;
      this.zipCountryStateControl.Enabled = true;
      this.zipCountryStateControl.Dock = DockStyle.Fill;
      this.zipCountryStateControl.Location = new Point(10, 27);
      this.clearPanel1Controls();
      this.panel1.Width = 500;
      this.panel1.Height = 300;
      this.panel1.Controls.Add((Control) this.zipCountryStateControl);
    }

    private void getAdvancedCodeForOutput()
    {
      if (this.valueText != null && this.valueText.Width != 460)
      {
        this.specificValueText = this.valueText.Text;
        this.valueText.Text = this.advancedCodeText;
      }
      this.createAdvancedCodingControls();
      this.panel1.Controls.Add((Control) this.advancedCodingLabel);
      this.panel1.Controls.Add((Control) this.valueText);
      this.panel1.Controls.Add((Control) this.validationBtn);
    }

    private void clearPanel1Controls()
    {
      if (this.panel1.Controls.Contains((Control) this.advancedCodingLabel))
        this.panel1.Controls.Remove((Control) this.advancedCodingLabel);
      if (this.panel1.Controls.Contains((Control) this.valueText))
        this.panel1.Controls.Remove((Control) this.valueText);
      if (this.panel1.Controls.Contains((Control) this.validationBtn))
        this.panel1.Controls.Remove((Control) this.validationBtn);
      if (this.panel1.Controls.Contains((Control) this.listOfValuesLabel))
        this.panel1.Controls.Remove((Control) this.listOfValuesLabel);
      if (this.panel1.Controls.Contains((Control) this.listOfValuesListbox))
        this.panel1.Controls.Remove((Control) this.listOfValuesListbox);
      if (this.panel1.Controls.Contains((Control) this.listOfValuesAddButton))
        this.panel1.Controls.Remove((Control) this.listOfValuesAddButton);
      if (this.panel1.Controls.Contains((Control) this.listOfValuesEditButton))
        this.panel1.Controls.Remove((Control) this.listOfValuesEditButton);
      if (this.panel1.Controls.Contains((Control) this.listOfValuesRemoveButton))
        this.panel1.Controls.Remove((Control) this.listOfValuesRemoveButton);
      if (this.panel1.Controls.Contains((Control) this.zipCountryStateControl))
        this.panel1.Controls.Remove((Control) this.zipCountryStateControl);
      if (this.panel1.Controls.Contains((Control) this.grpOptionFieldControls))
        this.panel1.Controls.Remove((Control) this.grpOptionFieldControls);
      this.panel1.BorderStyle = BorderStyle.None;
    }

    private void setAdvancedCodeForOutput(string output)
    {
      this.createAdvancedCodingControls();
      this.panel1.Controls.Add((Control) this.advancedCodingLabel);
      this.panel1.Controls.Add((Control) this.valueText);
      this.panel1.Controls.Add((Control) this.validationBtn);
      if (string.IsNullOrEmpty(output))
        return;
      this.valueText.Text = output;
    }

    private void validateBtn_click(object sender, EventArgs e)
    {
      if (!this.validateCalculation())
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "The calculation is valid", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void setValueSpecificControls()
    {
      if (DDM_FieldAccess_Utils.IsRolodexField(this.feeRuleValue.FieldID))
        this.createRolodex();
      else if (this.feeRuleValue.Field_Type == FieldFormat.YN)
      {
        this.createCombo();
        this.valueCombo.Items.Clear();
        this.valueCombo.Items.Add((object) new YnCheckBoxItemObject()
        {
          Text = "",
          Tag = ""
        });
        this.valueCombo.Items.Add((object) new YnCheckBoxItemObject()
        {
          Text = "Yes",
          Tag = "Y"
        });
        this.valueCombo.Items.Add((object) new YnCheckBoxItemObject()
        {
          Text = "No",
          Tag = "N"
        });
        this.adjustDropDownWidth(this.valueCombo);
        this.setSelectedItemForValueCombo(this.valueCombo, this.feeRuleValue.Field_Value);
        this.panel1.Controls.Add((Control) this.valueCombo);
      }
      else
      {
        FieldOptionCollection options;
        if (this.IsDropDownField(this.feeRuleValue.Field_Type, this.feeRuleValue.FieldID, out options))
        {
          if (this.valueCombo == null)
            this.createCombo();
          this.valueCombo.Items.Clear();
          this.valueCombo.Items.Add((object) "");
          foreach (object obj in options.GetValues())
            this.valueCombo.Items.Add(obj);
          this.adjustDropDownWidth(this.valueCombo);
          this.valueCombo.SelectedItem = (object) this.feeRuleValue.Field_Value;
          this.panel1.Controls.Add((Control) this.valueCombo);
        }
        else
        {
          this.createTextBox();
          this.panel1.Controls.Add((Control) this.valueText);
          this.valueText.Text = this.feeRuleValue.Field_Value;
        }
      }
    }

    private void getValueSpecificControls(bool set = false)
    {
      if (this.feeRuleValue.Field_Type == FieldFormat.YN)
      {
        this.createCombo();
        this.valueCombo.Items.Clear();
        this.valueCombo.Items.Add((object) new YnCheckBoxItemObject()
        {
          Text = "",
          Tag = ""
        });
        this.valueCombo.Items.Add((object) new YnCheckBoxItemObject()
        {
          Text = "Yes",
          Tag = "Y"
        });
        this.valueCombo.Items.Add((object) new YnCheckBoxItemObject()
        {
          Text = "No",
          Tag = "N"
        });
        this.adjustDropDownWidth(this.valueCombo);
        if (set && this.feeRuleValue.Field_Value.Length > 0)
          this.setSelectedItemForValueCombo(this.valueCombo, this.feeRuleValue.Field_Value);
        this.panel2.Controls.Add((Control) this.valueCombo);
      }
      else if (this.IsFeeManagementField)
      {
        this.CreateDropdownForFeeManagement();
      }
      else
      {
        FieldOptionCollection options;
        if (this.IsDropDownField(this.feeRuleValue.Field_Type, this.feeRuleValue.FieldID, out options))
        {
          if (this.valueCombo == null)
            this.createCombo();
          this.valueCombo.Items.Clear();
          foreach (object obj in options.GetValues())
            this.valueCombo.Items.Add(obj);
          if (this.shouldAddBorrower(this.feeRuleValue.FieldID))
          {
            bool flag = false;
            foreach (string str in this.valueCombo.Items)
            {
              if (str == "Borrower" || str == "")
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              this.valueCombo.Items.Insert(0, (object) "");
          }
          this.adjustDropDownWidth(this.valueCombo);
          if (set)
            this.valueCombo.SelectedItem = (object) this.feeRuleValue.Field_Value;
          this.panel2.Controls.Add((Control) this.valueCombo);
        }
        else if (DDM_FieldAccess_Utils.IsRolodexField(this.feeRuleValue.FieldID))
        {
          this.createTextBox();
          this.panel2.Controls.Add((Control) this.valueText);
          this.createRolodex();
          if (!set)
            return;
          this.valueText.Text = this.feeRuleValue.Field_Value;
        }
        else
        {
          this.createTextBox();
          this.panel2.Controls.Add((Control) this.valueText);
          if (!set)
            return;
          this.valueText.Text = this.feeRuleValue.Field_Value;
        }
      }
    }

    private bool shouldAddBorrower(string fieldID)
    {
      return HUDGFE2010Fields.PAIDBYPOPTFIELDS.Contains((object) fieldID);
    }

    private void CreateDropdownForFeeManagement()
    {
      this.specificValueMode = FeeValueDlg.SpecificValueMode.ComboBox;
      if (this.valueCombo == null)
        this.createCombo(this.IsFMfieldEditable);
      this.valueCombo.Items.Clear();
      foreach (string managementdpItem in this.feeManagementdpItems)
      {
        if (managementdpItem.Trim() != string.Empty)
          this.valueCombo.Items.Add((object) managementdpItem);
      }
      this.adjustDropDownWidth(this.valueCombo);
      if ((this.feeRuleValue.Field_Value_Type == fieldValueType.FeeManagement || this.feeRuleValue.Field_Value_Type == fieldValueType.SpecificValue) && !this.valueCombo.Items.Contains((object) this.feeRuleValue.Field_Value))
        this.valueCombo.Items.Add((object) this.feeRuleValue.Field_Value);
      this.valueCombo.SelectedItem = (object) this.feeRuleValue.Field_Value;
      this.panel2.Controls.Add((Control) this.valueCombo);
    }

    private void getDdmDataTableControls(bool set = false) => this.createDdmDataTableControls(set);

    private void createDdmDataTableControls(bool set = false)
    {
      if (this.valueText == null)
      {
        this.valueText = new TextBox();
        this.valueText.Name = "valueText";
        this.valueText.Visible = false;
        this.valueText.Location = new Point(0, 0);
      }
      this.SetValueTextSmall(true);
      this.panel2.Visible = true;
      this.value_lbl.Visible = true;
      this.panel2.Controls.Add((Control) this.valueText);
      if (set)
        this.valueText.Text = this.feeRuleValue.Field_Value;
      if (set)
        this.findDdmDataTable(this.valueText.Text);
      else
        this.populateDdmDataTableGrid();
    }

    private void findDdmDataTable(string formattedTableName)
    {
      string[] strArray = formattedTableName.Split('|');
      int num = 0;
      if (strArray.Length <= 1)
        return;
      this.dtValuewithPrefix = formattedTableName;
      this.valueText.Text = strArray[1];
      if (strArray.Length >= 4)
        num = (int) Convert.ToInt16(strArray[3]);
      if (num > this.outputCmb.Items.Count - 1)
        return;
      this.outputCmb.SelectedIndex = num;
    }

    private void populateDdmDataTableGrid()
    {
      this.pnlOutput.Visible = true;
      if (!this.firstLoad || this.feeRuleValue.Field_Value == string.Empty || this.feeRuleValue.Field_Value_Type != fieldValueType.SystemTable && this.feeRuleValue.Field_Value_Type != fieldValueType.Table)
      {
        this.dataTable = new DataTableList(this.ddmCurrentTab, this.session, this.feeLineNumber, this.feeRuleValue);
        if (this.dataTable.ShowDialog() == DialogResult.OK)
        {
          this.outputCmb.Items.Clear();
          this.outputCmb.Items.Add((object) "Please Select...");
          TableDefinitionTag tableDefinition = this.dataTable.TableDefinition;
          this.valueText.Text = tableDefinition.TableName;
          int num = 0;
          if (tableDefinition.Type == "DDM")
          {
            foreach (string outputColumn1 in tableDefinition.OutputColumns)
            {
              OutputColumn outputColumn2 = new OutputColumn()
              {
                Index = num,
                Name = outputColumn1
              };
              ++num;
              this.outputCmb.Items.Add((object) outputColumn2);
            }
            this.outputCmb.SelectedIndex = 0;
          }
          else
          {
            this.pnlOutput.Visible = false;
            this.updateDtValueWithPrefix();
          }
        }
        else
        {
          fieldValueType fieldValueType = this.feeRuleValue.Field_Value_Type;
          if (fieldValueType == fieldValueType.Calculation)
            this.panel1.Visible = true;
          this.valueTypeCombo.SelectedIndex = this.valueTypeCombo.FindStringExact(DDMFeeRuleValue.typeToValueTable[fieldValueType]);
        }
      }
      else
      {
        DDMDataTableBpmManager bpmManager = (DDMDataTableBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataTables);
        string[] strArray1 = this.feeRuleValue.Field_Value.Split('|');
        if (strArray1.Length > 1)
        {
          this.valueText.Text = strArray1[1];
          if (strArray1[0] == "DDM")
          {
            DDMDataTable fieldValuesByName;
            try
            {
              fieldValuesByName = bpmManager.GetDDMDataTableAndFieldValuesByName(strArray1[1], true);
            }
            catch (Exception ex)
            {
              this.feeRuleValue.Field_Value = "";
              int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "This datatable no longer exists. Please choose another data table or another value type.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              this.populateDdmDataTableGrid();
              return;
            }
            string[] strArray2 = fieldValuesByName.OutputIdList.Split('|');
            int num1 = 0;
            foreach (string str in strArray2)
            {
              OutputColumn outputColumn = new OutputColumn()
              {
                Index = num1,
                Name = str
              };
              ++num1;
              this.outputCmb.Items.Add((object) outputColumn);
            }
            this.findDdmDataTable(this.feeRuleValue.Field_Value);
          }
          else
            this.pnlOutput.Visible = false;
        }
      }
      this.firstLoad = false;
    }

    private void createTextBox(bool reset = false)
    {
      if (this.valueText == null)
      {
        this.valueText = new TextBox();
        this.valueText.Size = new Size(226, 22);
        this.valueText.Location = new Point(0, 0);
        this.valueText.Name = "valueText";
        this.valueText.TextChanged += new EventHandler(this.ValidateControl);
      }
      if (reset)
        this.valueText.Text = string.Empty;
      this.SetValueTextSmall();
    }

    private void SetValueTextSmall(bool isReadonly = false)
    {
      this.valueText.Visible = true;
      if (!isReadonly)
        this.valueText.Enabled = true;
      else
        this.valueText.ReadOnly = true;
      this.valueText.Width = 226;
      this.valueText.Height = 22;
      this.valueText.Multiline = false;
      this.valueText.ScrollBars = ScrollBars.None;
      this.valueText.Location = new Point(0, 0);
    }

    private void ValidateControl(object sender, EventArgs e)
    {
      bool needsUpdate = false;
      int newCursorPos = 0;
      FieldFormat dataFormat = FieldFormat.NONE;
      if (this._currentDataTableField != null)
        dataFormat = this._currentDataTableField.FieldType;
      else if (this.feeRuleValue != null)
        dataFormat = this.feeRuleValue.Field_Type;
      if ((string) this.valueTypeCombo.SelectedItem != "Specific Value" && this.ddmCurrentTab == FeeValueDlg.ddmTab.FeeField)
        return;
      string str = Utils.FormatInput(((Control) sender).Text, dataFormat, ref needsUpdate, ((TextBoxBase) sender).SelectionStart, ref newCursorPos);
      if (!needsUpdate)
        return;
      ((Control) sender).Text = str;
      ((TextBoxBase) sender).SelectionStart = newCursorPos;
    }

    private void createRolodex()
    {
      if (this.btnRolodex == null)
      {
        this.btnRolodex = new StandardIconButton();
        this.btnRolodex.BackColor = Color.Transparent;
        this.btnRolodex.Location = new Point(235, 0);
        this.btnRolodex.Name = "btnRolodex";
        this.btnRolodex.Size = new Size(16, 16);
        this.btnRolodex.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
        this.btnRolodex.Click += new EventHandler(this.btnRolodex_Click);
      }
      this.panel2.Controls.Add((Control) this.btnRolodex);
    }

    private void btnRolodex_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      string fieldId;
      string fieldValue;
      if (this.ddmCurrentTab == FeeValueDlg.ddmTab.FeeField)
      {
        fieldId = this.feeRuleValue.FieldID;
        fieldValue = this.feeRuleValue.Field_Value;
      }
      else
      {
        fieldId = this._currentDataTableField.FieldID;
        fieldValue = this._currentDataTableField.Value.ToString();
      }
      string categoryName;
      DDM_FieldAccess_Utils.IsRolodexField(fieldId, out categoryName);
      if (string.IsNullOrEmpty(categoryName) || categoryName.ToLower().Equals("all"))
        categoryName = "";
      rxContact.CategoryName = categoryName;
      CRMRoleType crmRoleType = CRMRoleType.NotFound;
      rxContact.CompanyName = fieldValue;
      RxBusinessContact rxBusinessContact = new RxBusinessContact(categoryName, fieldValue, "", rxContact, true, true, crmRoleType, false, RxBusinessContact.ActionMode.SelectMode, "");
      if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.valueText.Text = rxBusinessContact.RxContactRecord.CompanyName;
    }

    private void createAdvancedCodingTextBox()
    {
      if (this.valueText == null)
      {
        this.valueText = new TextBox();
        this.valueText.Name = "valueText";
      }
      this.valueText.Size = new Size(200, 300);
      this.valueText.Location = new Point(0, 0);
      this.valueText.Enabled = true;
    }

    private void createCombo(bool edit = false)
    {
      if (this.valueCombo == null)
      {
        this.valueCombo = new ComboBox();
        this.valueCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      }
      if (edit)
        this.valueCombo.DropDownStyle = ComboBoxStyle.DropDown;
      this.valueCombo.Location = new Point(0, 0);
      this.valueCombo.Size = new Size(226, 22);
      this.valueCombo.TabStop = true;
      this.valueCombo.TabIndex = 3;
    }

    [Obsolete("Do not use this. Use validateCalculation instead", true)]
    private bool validateAdvancedCode()
    {
      if (this.valueText.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Calculation field cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.valueText.Focus();
        return false;
      }
      try
      {
        using (RuntimeContext context = RuntimeContext.Create())
          new AdvancedCodeFieldRule("", "", (RuleCondition) PredefinedCondition.Empty, this.valueText.Text, new string[0], FieldFormat.STRING).CreateImplementation(context);
        return true;
      }
      catch (CompileException ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display((Exception) ex);
          return false;
        }
        CompilerError error = ex.Errors[0];
        if (error.LineIndexOfRegion >= 0)
          Utils.HighlightLine(this.valueText, error.LineIndexOfRegion, false);
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Validation failed: " + error.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      catch (Exception ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          ErrorDialog.Display(ex);
          return false;
        }
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Error validating expression: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private void label4_Click(object sender, EventArgs e)
    {
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      this.Close();
      this.DialogResult = DialogResult.Cancel;
    }

    private void FeeValueDlg_Load(object sender, EventArgs e)
    {
      this.setNavigationButtonState();
      this.valueTypeCombo.Focus();
    }

    private void setNavigationButtonState()
    {
      this.BtnFirstItem.Enabled = this.BtnNextItem.Enabled = this.BtnPrevItem.Enabled = this.BtnLastItem.Enabled = true;
      if (this.DtFieldValues == null)
        return;
      if (this.DtFieldValues.CurrentItemIndex == 0)
      {
        this.BtnFirstItem.Enabled = false;
        this.BtnPrevItem.Enabled = false;
        if (((IEnumerable<DataTableField>) this.DtFieldValues.DataTableFields).Count<DataTableField>() == 1)
        {
          this.BtnNextItem.Enabled = false;
          this.BtnLastItem.Enabled = false;
        }
      }
      if (this.DtFieldValues.CurrentItemIndex != ((IEnumerable<DataTableField>) this.DtFieldValues.DataTableFields).Count<DataTableField>() - 1)
        return;
      this.BtnLastItem.Enabled = false;
      this.BtnNextItem.Enabled = false;
    }

    private void BtnFirstItem_Click(object sender, EventArgs e)
    {
      if (!this.ValidateAndSetCurrentChangesToCollection())
        return;
      this.DtFieldValues.CurrentItemIndex = 0;
      this._currentDataTableField = this.DtFieldValues.DataTableFields[this.DtFieldValues.CurrentItemIndex];
      this.PopulateDataTableControlValues();
      this.setNavigationButtonState();
    }

    private void BtnNextItem_Click(object sender, EventArgs e)
    {
      if (!this.ValidateAndSetCurrentChangesToCollection() || this.DtFieldValues.CurrentItemIndex >= ((IEnumerable<DataTableField>) this.DtFieldValues.DataTableFields).Count<DataTableField>() - 1)
        return;
      ++this.DtFieldValues.CurrentItemIndex;
      this._currentDataTableField = this.DtFieldValues.DataTableFields[this.DtFieldValues.CurrentItemIndex];
      this.PopulateDataTableControlValues();
      this.setNavigationButtonState();
    }

    private void BtnLastItem_Click(object sender, EventArgs e)
    {
      if (!this.ValidateAndSetCurrentChangesToCollection())
        return;
      this.DtFieldValues.CurrentItemIndex = ((IEnumerable<DataTableField>) this.DtFieldValues.DataTableFields).Count<DataTableField>() - 1;
      this._currentDataTableField = this.DtFieldValues.DataTableFields[this.DtFieldValues.CurrentItemIndex];
      this.PopulateDataTableControlValues();
      this.setNavigationButtonState();
    }

    private void BtnPrevItem_Click(object sender, EventArgs e)
    {
      if (!this.ValidateAndSetCurrentChangesToCollection() || this.DtFieldValues.CurrentItemIndex <= 0)
        return;
      --this.DtFieldValues.CurrentItemIndex;
      this._currentDataTableField = this.DtFieldValues.DataTableFields[this.DtFieldValues.CurrentItemIndex];
      this.PopulateDataTableControlValues();
      this.setNavigationButtonState();
    }

    private void txtDataValue_TextChanged(object sender, EventArgs e)
    {
      this.ValidateControl(sender, e);
    }

    private void txtDataMinValue_TextChanged(object sender, EventArgs e)
    {
      this.ValidateControl(sender, e);
    }

    private void txtDataMaxValue_TextChanged(object sender, EventArgs e)
    {
      this.ValidateControl(sender, e);
    }

    private void txt_TextChanged(object sender, EventArgs e) => this.IsDirty = true;

    private void dptxtDateValue_ValueChanged(object sender, EventArgs e) => this.IsDirty = true;

    private void lstChkYN_SelectedValueChanged(object sender, EventArgs e) => this.IsDirty = true;

    private void DateValue_ValueChanged(object sender, EventArgs e) => this.IsDirty = true;

    private void setSelectedItemForValueCombo(ComboBox comboControl, string value)
    {
      if (comboControl.Items[0] is YnCheckBoxItemObject)
      {
        foreach (YnCheckBoxItemObject checkBoxItemObject in comboControl.Items)
        {
          if (checkBoxItemObject.Tag.Equals(value))
          {
            comboControl.SelectedItem = (object) checkBoxItemObject;
            break;
          }
        }
      }
      else
        comboControl.SelectedItem = (object) value;
    }

    private string getSettingValueFromValueCombo(ComboBox comboControl)
    {
      object selectedItem = comboControl.SelectedItem;
      if (selectedItem == null)
        return comboControl.Text;
      return selectedItem is YnCheckBoxItemObject ? ((YnCheckBoxItemObject) selectedItem).Tag : comboControl.SelectedItem.ToString();
    }

    private void outputCmb_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.updateDtValueWithPrefix();
    }

    private void updateDtValueWithPrefix()
    {
      if (this.outputCmb.Text == "Please Select...")
        return;
      if (this.dataTable != null)
      {
        this.IsSystemTable = !this.dataTable.TableDefinition.Type.Equals("DDM");
        this.dtValuewithPrefix = string.Format("{0}|{1}|{2}|{3}", (object) this.dataTable.TableDefinition.Type, (object) this.dataTable.TableDefinition.TableName, (object) this.dataTable.TableDefinition.LineNumber, (object) this.outputCmb.SelectedIndex);
      }
      else
      {
        string[] strArray = this.dtValuewithPrefix.Split('|');
        this.IsSystemTable = !strArray[0].Equals("DDM");
        this.dtValuewithPrefix = string.Format("{0}|{1}|{2}|{3}", (object) strArray[0], (object) strArray[1], (object) strArray[2], (object) this.outputCmb.SelectedIndex);
      }
    }

    private void outputCmb_DropDown(object sender, EventArgs e)
    {
      if (!this.outputCmb.Items.Contains((object) "Please Select..."))
        return;
      this.outputCmb.Items.Remove((object) "Please Select...");
    }

    private void outputCmb_DropDownClosed(object sender, EventArgs e)
    {
      if (this.outputCmb.SelectedIndex != -1)
        return;
      this.outputCmb.Items.Add((object) "Please Select...");
      this.outputCmb.Text = "Please Select...";
    }

    private class ZipCountyStateObj
    {
      public List<ZipCountyState> SelectedItems = new List<ZipCountyState>();
      public int SelectedStateIdx = -1;
      public string ExistingValue = string.Empty;
    }

    private enum CurrentState
    {
      SpecificValText,
      TableValText,
      AdvanceCodeValText,
      NoVal,
    }

    private enum SpecificValueMode
    {
      TextBox,
      ComboBox,
    }

    public enum ddmTab
    {
      FeeField,
      DataTable,
    }

    private enum ValueControlMode
    {
      RegularTextbox,
      DropDownNonEdit,
      DropDownEdit,
      Checkboxes,
    }
  }
}
