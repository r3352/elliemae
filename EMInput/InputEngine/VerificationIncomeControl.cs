// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerificationIncomeControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class VerificationIncomeControl : UserControl, IVerificationDetails
  {
    public EventHandler OnStatusChanged;
    private VerificationDetailsControl.BorrowerEditMode editMode;
    private IContainer components;
    private GroupContainer groupContainerBor;
    private CheckBox chkW2;
    private CheckBox chk1099;
    private CheckBox chkEmploymentOther;
    private CheckBox chkMilitary;
    private CheckBox chkPension;
    private CheckBox chkTaxReturns;
    private CheckBox chkPaystubs;
    private Label label1;
    private Panel panelNonEmployment;
    private TextBox txtNonOther;
    private CheckBox chkNonOther;
    private GradientPanel gradientPanelHeader2;
    private Label label3;
    private CheckBox chkAlimony;
    private CheckBox chkRentalIncome;
    private CheckBox chkChildSupport;
    private Panel panelEmployment;
    private CheckBox chkSSN;
    private GradientPanel gradientPanelHeader1;
    private Label label2;
    private CheckBox chk401K;
    private TextBox txtEmploymentOther;
    private TextBox txtTaxReturnsYrs;
    private Panel panelHeader;

    public VerificationIncomeControl(
      VerificationDetailsControl.BorrowerEditMode editMode)
    {
      this.editMode = editMode;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.SetBorrowerType(editMode);
    }

    public void SetBorrowerType(
      VerificationDetailsControl.BorrowerEditMode editMode)
    {
      this.editMode = editMode;
      if (this.editMode == VerificationDetailsControl.BorrowerEditMode.CoBorrower)
        this.groupContainerBor.Text = "Co-Borrower";
      else
        this.groupContainerBor.Text = "Borrower";
    }

    public void SetDetails(VerificationTimelineLog verificationLog)
    {
      VerificationTimelineIncomeLog timelineIncomeLog = (VerificationTimelineIncomeLog) verificationLog;
      this.chkPaystubs.Checked = timelineIncomeLog.IsPaystubs;
      this.chkTaxReturns.Checked = timelineIncomeLog.IsTaxReturns;
      this.txtTaxReturnsYrs.Text = !timelineIncomeLog.IsTaxReturns || timelineIncomeLog.TaxReturnYear == 0 ? "" : timelineIncomeLog.TaxReturnYear.ToString();
      this.chkPension.Checked = timelineIncomeLog.IsPension;
      this.chkMilitary.Checked = timelineIncomeLog.IsMilitary;
      this.chkW2.Checked = timelineIncomeLog.IsW2;
      this.chk1099.Checked = timelineIncomeLog.Is1099;
      this.chkSSN.Checked = timelineIncomeLog.IsSocialSecurity;
      this.chk401K.Checked = timelineIncomeLog.Is401K;
      this.chkEmploymentOther.Checked = timelineIncomeLog.IsOtherEmployment;
      this.txtEmploymentOther.Text = timelineIncomeLog.IsOtherEmployment ? timelineIncomeLog.OtherEmploymentDescription : "";
      this.chkAlimony.Checked = timelineIncomeLog.IsAlimonyOrMaintenance;
      this.chkRentalIncome.Checked = timelineIncomeLog.IsRentalIncome;
      this.chkChildSupport.Checked = timelineIncomeLog.IsChildSupport;
      this.chkNonOther.Checked = timelineIncomeLog.IsOtherNonEmployment;
      this.txtNonOther.Text = timelineIncomeLog.IsOtherNonEmployment ? timelineIncomeLog.OtherNonEmploymentDescription : "";
      this.checkbox_CheckedChanged((object) this.chkTaxReturns, (EventArgs) null);
      this.checkbox_CheckedChanged((object) this.chkEmploymentOther, (EventArgs) null);
      this.checkbox_CheckedChanged((object) this.chkNonOther, (EventArgs) null);
    }

    public void GetDetails(VerificationTimelineLog verificationLog)
    {
      VerificationTimelineIncomeLog timelineIncomeLog = (VerificationTimelineIncomeLog) verificationLog;
      timelineIncomeLog.IsPaystubs = this.chkPaystubs.Checked;
      timelineIncomeLog.IsTaxReturns = this.chkTaxReturns.Checked;
      timelineIncomeLog.TaxReturnYear = !timelineIncomeLog.IsTaxReturns || this.txtTaxReturnsYrs.Text.Trim() == "" ? 0 : Utils.ParseInt((object) this.txtTaxReturnsYrs.Text.Trim());
      timelineIncomeLog.IsPension = this.chkPension.Checked;
      timelineIncomeLog.IsMilitary = this.chkMilitary.Checked;
      timelineIncomeLog.IsW2 = this.chkW2.Checked;
      timelineIncomeLog.Is1099 = this.chk1099.Checked;
      timelineIncomeLog.IsSocialSecurity = this.chkSSN.Checked;
      timelineIncomeLog.Is401K = this.chk401K.Checked;
      timelineIncomeLog.IsOtherEmployment = this.chkEmploymentOther.Checked;
      timelineIncomeLog.OtherEmploymentDescription = timelineIncomeLog.IsOtherEmployment ? this.txtEmploymentOther.Text.Trim() : "";
      timelineIncomeLog.IsAlimonyOrMaintenance = this.chkAlimony.Checked;
      timelineIncomeLog.IsRentalIncome = this.chkRentalIncome.Checked;
      timelineIncomeLog.IsChildSupport = this.chkChildSupport.Checked;
      timelineIncomeLog.IsOtherNonEmployment = this.chkNonOther.Checked;
      timelineIncomeLog.OtherNonEmploymentDescription = timelineIncomeLog.IsOtherNonEmployment ? this.txtNonOther.Text.Trim() : "";
    }

    private void onYearFieldKeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void checkbox_CheckedChanged(object sender, EventArgs e)
    {
      if (sender == null)
        return;
      CheckBox checkBox = (CheckBox) sender;
      if (checkBox.Checked)
      {
        if (checkBox.Name != this.chkPaystubs.Name)
          this.chkPaystubs.Checked = false;
        if (checkBox.Name != this.chkTaxReturns.Name)
          this.chkTaxReturns.Checked = false;
        if (checkBox.Name != this.chkPension.Name)
          this.chkPension.Checked = false;
        if (checkBox.Name != this.chkMilitary.Name)
          this.chkMilitary.Checked = false;
        if (checkBox.Name != this.chkW2.Name)
          this.chkW2.Checked = false;
        if (checkBox.Name != this.chk1099.Name)
          this.chk1099.Checked = false;
        if (checkBox.Name != this.chkSSN.Name)
          this.chkSSN.Checked = false;
        if (checkBox.Name != this.chk401K.Name)
          this.chk401K.Checked = false;
        if (checkBox.Name != this.chkEmploymentOther.Name)
          this.chkEmploymentOther.Checked = false;
        if (checkBox.Name != this.chkAlimony.Name)
          this.chkAlimony.Checked = false;
        if (checkBox.Name != this.chkRentalIncome.Name)
          this.chkRentalIncome.Checked = false;
        if (checkBox.Name != this.chkChildSupport.Name)
          this.chkChildSupport.Checked = false;
        if (checkBox.Name != this.chkNonOther.Name)
          this.chkNonOther.Checked = false;
      }
      if (checkBox.Name == "chkTaxReturns")
      {
        this.txtTaxReturnsYrs.ReadOnly = !checkBox.Checked;
        if (!checkBox.Checked)
          this.txtTaxReturnsYrs.Text = string.Empty;
      }
      else if (checkBox.Name == "chkEmploymentOther")
      {
        this.txtEmploymentOther.ReadOnly = !checkBox.Checked;
        if (!checkBox.Checked)
          this.txtEmploymentOther.Text = string.Empty;
      }
      else if (checkBox.Name == "chkNonOther")
      {
        this.txtNonOther.ReadOnly = !checkBox.Checked;
        if (!checkBox.Checked)
          this.txtNonOther.Text = string.Empty;
      }
      if (this.OnStatusChanged == null)
        return;
      this.OnStatusChanged(sender, new EventArgs());
    }

    public string BuildWhatVerified()
    {
      string str = string.Empty;
      if (this.chkPaystubs.Checked)
        str += "Paystubs";
      if (this.chkTaxReturns.Checked)
        str = str + (str != string.Empty ? "," : "") + "Tax Returns";
      if (this.chkPension.Checked)
        str = str + (str != string.Empty ? "," : "") + "Pension";
      if (this.chkMilitary.Checked)
        str = str + (str != string.Empty ? "," : "") + "Military";
      if (this.chkW2.Checked)
        str = str + (str != string.Empty ? "," : "") + "W2";
      if (this.chk1099.Checked)
        str = str + (str != string.Empty ? "," : "") + "1099";
      if (this.chkSSN.Checked)
        str = str + (str != string.Empty ? "," : "") + "Social Security";
      if (this.chk401K.Checked)
        str = str + (str != string.Empty ? "," : "") + "401K";
      if (this.chkEmploymentOther.Checked)
        str = str + (str != string.Empty ? "," : "") + "Other Employment";
      if (this.chkAlimony.Checked)
        str = str + (str != string.Empty ? "," : "") + "Alimony/Maintenance";
      if (this.chkRentalIncome.Checked)
        str = str + (str != string.Empty ? "," : "") + "Rental Income";
      if (this.chkChildSupport.Checked)
        str = str + (str != string.Empty ? "," : "") + "Child Support";
      if (this.chkNonOther.Checked)
        str = str + (str != string.Empty ? "," : "") + "Other Non-Employment";
      return str;
    }

    public void SetReadOnly()
    {
      this.chkPaystubs.Enabled = false;
      this.chkTaxReturns.Enabled = false;
      this.txtTaxReturnsYrs.ReadOnly = true;
      this.chkPension.Enabled = false;
      this.chkMilitary.Enabled = false;
      this.chkW2.Enabled = false;
      this.chk1099.Enabled = false;
      this.chkSSN.Enabled = false;
      this.chk401K.Enabled = false;
      this.chkEmploymentOther.Enabled = false;
      this.txtEmploymentOther.ReadOnly = true;
      this.chkAlimony.Enabled = false;
      this.chkRentalIncome.Enabled = false;
      this.chkChildSupport.Enabled = false;
      this.chkNonOther.Enabled = false;
      this.txtNonOther.ReadOnly = true;
    }

    private void txtTaxReturnsYrs_Leave(object sender, EventArgs e)
    {
      if (this.txtTaxReturnsYrs.Text.Trim() == string.Empty)
        return;
      int num1 = Utils.ParseInt((object) this.txtTaxReturnsYrs.Text.Trim());
      if (num1 >= 1900 && num1 <= 2199)
        return;
      int num2 = (int) Utils.Dialog((IWin32Window) this, "Value '" + this.txtTaxReturnsYrs.Text.Trim() + "' is out of range. Must be between 1900 and 2199.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.txtTaxReturnsYrs.Text = string.Empty;
      this.txtTaxReturnsYrs.Focus();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainerBor = new GroupContainer();
      this.panelNonEmployment = new Panel();
      this.txtNonOther = new TextBox();
      this.chkNonOther = new CheckBox();
      this.gradientPanelHeader2 = new GradientPanel();
      this.label3 = new Label();
      this.chkAlimony = new CheckBox();
      this.chkRentalIncome = new CheckBox();
      this.chkChildSupport = new CheckBox();
      this.panelEmployment = new Panel();
      this.chkSSN = new CheckBox();
      this.gradientPanelHeader1 = new GradientPanel();
      this.label2 = new Label();
      this.chkPaystubs = new CheckBox();
      this.chk401K = new CheckBox();
      this.chk1099 = new CheckBox();
      this.chkTaxReturns = new CheckBox();
      this.chkEmploymentOther = new CheckBox();
      this.txtEmploymentOther = new TextBox();
      this.chkW2 = new CheckBox();
      this.chkPension = new CheckBox();
      this.chkMilitary = new CheckBox();
      this.txtTaxReturnsYrs = new TextBox();
      this.panelHeader = new Panel();
      this.label1 = new Label();
      this.groupContainerBor.SuspendLayout();
      this.panelNonEmployment.SuspendLayout();
      this.gradientPanelHeader2.SuspendLayout();
      this.panelEmployment.SuspendLayout();
      this.gradientPanelHeader1.SuspendLayout();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.groupContainerBor.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerBor.Controls.Add((Control) this.panelNonEmployment);
      this.groupContainerBor.Controls.Add((Control) this.panelEmployment);
      this.groupContainerBor.Controls.Add((Control) this.panelHeader);
      this.groupContainerBor.Dock = DockStyle.Fill;
      this.groupContainerBor.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerBor.Location = new Point(0, 0);
      this.groupContainerBor.Name = "groupContainerBor";
      this.groupContainerBor.Size = new Size(650, 253);
      this.groupContainerBor.TabIndex = 1;
      this.groupContainerBor.Text = "Borrower";
      this.panelNonEmployment.BackColor = Color.White;
      this.panelNonEmployment.Controls.Add((Control) this.txtNonOther);
      this.panelNonEmployment.Controls.Add((Control) this.chkNonOther);
      this.panelNonEmployment.Controls.Add((Control) this.gradientPanelHeader2);
      this.panelNonEmployment.Controls.Add((Control) this.chkAlimony);
      this.panelNonEmployment.Controls.Add((Control) this.chkRentalIncome);
      this.panelNonEmployment.Controls.Add((Control) this.chkChildSupport);
      this.panelNonEmployment.Dock = DockStyle.Fill;
      this.panelNonEmployment.Location = new Point(1, 175);
      this.panelNonEmployment.Name = "panelNonEmployment";
      this.panelNonEmployment.Size = new Size(648, 77);
      this.panelNonEmployment.TabIndex = 14;
      this.txtNonOther.BorderStyle = BorderStyle.FixedSingle;
      this.txtNonOther.Location = new Point(268, 43);
      this.txtNonOther.Name = "txtNonOther";
      this.txtNonOther.Size = new Size(133, 20);
      this.txtNonOther.TabIndex = 5;
      this.chkNonOther.AutoSize = true;
      this.chkNonOther.Location = new Point(211, 45);
      this.chkNonOther.Name = "chkNonOther";
      this.chkNonOther.Size = new Size(55, 17);
      this.chkNonOther.TabIndex = 4;
      this.chkNonOther.Text = "Other:";
      this.chkNonOther.UseVisualStyleBackColor = true;
      this.chkNonOther.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.gradientPanelHeader2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanelHeader2.Controls.Add((Control) this.label3);
      this.gradientPanelHeader2.Dock = DockStyle.Top;
      this.gradientPanelHeader2.GradientColor1 = Color.White;
      this.gradientPanelHeader2.GradientColor2 = Color.Silver;
      this.gradientPanelHeader2.Location = new Point(0, 0);
      this.gradientPanelHeader2.Name = "gradientPanelHeader2";
      this.gradientPanelHeader2.Size = new Size(648, 23);
      this.gradientPanelHeader2.TabIndex = 12;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(2, 5);
      this.label3.Name = "label3";
      this.label3.Size = new Size(87, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Non-Employment";
      this.chkAlimony.AutoSize = true;
      this.chkAlimony.Location = new Point(10, 27);
      this.chkAlimony.Name = "chkAlimony";
      this.chkAlimony.Size = new Size(129, 17);
      this.chkAlimony.TabIndex = 1;
      this.chkAlimony.Text = "Alimony/Maintenance";
      this.chkAlimony.UseVisualStyleBackColor = true;
      this.chkAlimony.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkRentalIncome.AutoSize = true;
      this.chkRentalIncome.Location = new Point(10, 45);
      this.chkRentalIncome.Name = "chkRentalIncome";
      this.chkRentalIncome.Size = new Size(95, 17);
      this.chkRentalIncome.TabIndex = 2;
      this.chkRentalIncome.Text = "Rental Income";
      this.chkRentalIncome.UseVisualStyleBackColor = true;
      this.chkRentalIncome.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkChildSupport.AutoSize = true;
      this.chkChildSupport.Location = new Point(211, 27);
      this.chkChildSupport.Name = "chkChildSupport";
      this.chkChildSupport.Size = new Size(89, 17);
      this.chkChildSupport.TabIndex = 3;
      this.chkChildSupport.Text = "Child Support";
      this.chkChildSupport.UseVisualStyleBackColor = true;
      this.chkChildSupport.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.panelEmployment.BackColor = Color.White;
      this.panelEmployment.Controls.Add((Control) this.chkSSN);
      this.panelEmployment.Controls.Add((Control) this.gradientPanelHeader1);
      this.panelEmployment.Controls.Add((Control) this.chkPaystubs);
      this.panelEmployment.Controls.Add((Control) this.chk401K);
      this.panelEmployment.Controls.Add((Control) this.chk1099);
      this.panelEmployment.Controls.Add((Control) this.chkTaxReturns);
      this.panelEmployment.Controls.Add((Control) this.chkEmploymentOther);
      this.panelEmployment.Controls.Add((Control) this.txtEmploymentOther);
      this.panelEmployment.Controls.Add((Control) this.chkW2);
      this.panelEmployment.Controls.Add((Control) this.chkPension);
      this.panelEmployment.Controls.Add((Control) this.chkMilitary);
      this.panelEmployment.Controls.Add((Control) this.txtTaxReturnsYrs);
      this.panelEmployment.Dock = DockStyle.Top;
      this.panelEmployment.Location = new Point(1, 49);
      this.panelEmployment.Name = "panelEmployment";
      this.panelEmployment.Size = new Size(648, 126);
      this.panelEmployment.TabIndex = 13;
      this.chkSSN.AutoSize = true;
      this.chkSSN.Location = new Point(211, 63);
      this.chkSSN.Name = "chkSSN";
      this.chkSSN.Size = new Size(96, 17);
      this.chkSSN.TabIndex = 10;
      this.chkSSN.Text = "Social Security";
      this.chkSSN.UseVisualStyleBackColor = true;
      this.chkSSN.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.gradientPanelHeader1.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gradientPanelHeader1.Controls.Add((Control) this.label2);
      this.gradientPanelHeader1.Dock = DockStyle.Top;
      this.gradientPanelHeader1.GradientColor1 = Color.White;
      this.gradientPanelHeader1.GradientColor2 = Color.Silver;
      this.gradientPanelHeader1.Location = new Point(0, 0);
      this.gradientPanelHeader1.Name = "gradientPanelHeader1";
      this.gradientPanelHeader1.Size = new Size(648, 23);
      this.gradientPanelHeader1.TabIndex = 0;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(2, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(64, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Employment";
      this.chkPaystubs.AutoSize = true;
      this.chkPaystubs.Location = new Point(10, 27);
      this.chkPaystubs.Name = "chkPaystubs";
      this.chkPaystubs.Size = new Size(69, 17);
      this.chkPaystubs.TabIndex = 1;
      this.chkPaystubs.Text = "Paystubs";
      this.chkPaystubs.UseVisualStyleBackColor = true;
      this.chkPaystubs.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chk401K.AutoSize = true;
      this.chk401K.Location = new Point(211, 81);
      this.chk401K.Name = "chk401K";
      this.chk401K.Size = new Size(51, 17);
      this.chk401K.TabIndex = 11;
      this.chk401K.Text = "401K";
      this.chk401K.UseVisualStyleBackColor = true;
      this.chk401K.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chk1099.AutoSize = true;
      this.chk1099.Location = new Point(211, 45);
      this.chk1099.Name = "chk1099";
      this.chk1099.Size = new Size(50, 17);
      this.chk1099.TabIndex = 9;
      this.chk1099.Text = "1099";
      this.chk1099.UseVisualStyleBackColor = true;
      this.chk1099.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkTaxReturns.AutoSize = true;
      this.chkTaxReturns.Location = new Point(10, 45);
      this.chkTaxReturns.Name = "chkTaxReturns";
      this.chkTaxReturns.Size = new Size(111, 17);
      this.chkTaxReturns.TabIndex = 2;
      this.chkTaxReturns.Text = "Tax Returns   Yrs.";
      this.chkTaxReturns.UseVisualStyleBackColor = true;
      this.chkTaxReturns.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkEmploymentOther.AutoSize = true;
      this.chkEmploymentOther.Location = new Point(10, 99);
      this.chkEmploymentOther.Name = "chkEmploymentOther";
      this.chkEmploymentOther.Size = new Size(55, 17);
      this.chkEmploymentOther.TabIndex = 6;
      this.chkEmploymentOther.Text = "Other:";
      this.chkEmploymentOther.UseVisualStyleBackColor = true;
      this.chkEmploymentOther.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.txtEmploymentOther.BorderStyle = BorderStyle.FixedSingle;
      this.txtEmploymentOther.Location = new Point(65, 97);
      this.txtEmploymentOther.Name = "txtEmploymentOther";
      this.txtEmploymentOther.Size = new Size(133, 20);
      this.txtEmploymentOther.TabIndex = 7;
      this.chkW2.AutoSize = true;
      this.chkW2.Location = new Point(211, 27);
      this.chkW2.Name = "chkW2";
      this.chkW2.Size = new Size(43, 17);
      this.chkW2.TabIndex = 8;
      this.chkW2.Text = "W2";
      this.chkW2.UseVisualStyleBackColor = true;
      this.chkW2.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkPension.AutoSize = true;
      this.chkPension.Location = new Point(10, 63);
      this.chkPension.Name = "chkPension";
      this.chkPension.Size = new Size(64, 17);
      this.chkPension.TabIndex = 4;
      this.chkPension.Text = "Pension";
      this.chkPension.UseVisualStyleBackColor = true;
      this.chkPension.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkMilitary.AutoSize = true;
      this.chkMilitary.Location = new Point(10, 81);
      this.chkMilitary.Name = "chkMilitary";
      this.chkMilitary.Size = new Size(58, 17);
      this.chkMilitary.TabIndex = 5;
      this.chkMilitary.Text = "Military";
      this.chkMilitary.UseVisualStyleBackColor = true;
      this.chkMilitary.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.txtTaxReturnsYrs.BorderStyle = BorderStyle.FixedSingle;
      this.txtTaxReturnsYrs.Location = new Point(121, 43);
      this.txtTaxReturnsYrs.MaxLength = 4;
      this.txtTaxReturnsYrs.Name = "txtTaxReturnsYrs";
      this.txtTaxReturnsYrs.Size = new Size(77, 20);
      this.txtTaxReturnsYrs.TabIndex = 3;
      this.txtTaxReturnsYrs.KeyPress += new KeyPressEventHandler(this.onYearFieldKeyPress);
      this.txtTaxReturnsYrs.Leave += new EventHandler(this.txtTaxReturnsYrs_Leave);
      this.panelHeader.BackColor = Color.White;
      this.panelHeader.Controls.Add((Control) this.label1);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 25);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(648, 24);
      this.panelHeader.TabIndex = 12;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(7, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(180, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Income Supporting Information";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.groupContainerBor);
      this.Name = nameof (VerificationIncomeControl);
      this.Size = new Size(650, 253);
      this.groupContainerBor.ResumeLayout(false);
      this.panelNonEmployment.ResumeLayout(false);
      this.panelNonEmployment.PerformLayout();
      this.gradientPanelHeader2.ResumeLayout(false);
      this.gradientPanelHeader2.PerformLayout();
      this.panelEmployment.ResumeLayout(false);
      this.panelEmployment.PerformLayout();
      this.gradientPanelHeader1.ResumeLayout(false);
      this.gradientPanelHeader1.PerformLayout();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
