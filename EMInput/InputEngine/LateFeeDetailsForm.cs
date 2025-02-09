// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LateFeeDetailsForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LateFeeDetailsForm : Form
  {
    private IHtmlInput inputData;
    private GenericFormInputHandler inputHandler;
    private Sessions.Session session;
    private LoanData loan;
    private ExternalLateFeeSettings externalLateFeeSettings;
    private IContainer components;
    private Label label1;
    private Button btnCancel;
    private Button btnOK;
    private Panel panel1;
    private Label labelOf1;
    private Label labelOf2;
    private TextBox txtNotes;
    private Label label9;
    private TextBox txtTotalAdjust;
    private Label label10;
    private Label label11;
    private TextBox txtLateFeeAdditional;
    private Label label12;
    private TextBox txtLateFee;
    private Label label5;
    private TextBox txtTotalDays;
    private Label label6;
    private Label label7;
    private Label label8;
    private TextBox txtGraceDays;
    private Label label3;
    private DatePicker datEnd;
    private DatePicker datBegin;
    private DatePicker datGracePeriodStart;
    private ToolTip toolTipField;
    private FieldLockButton btnLockLateFeeAdditional;
    private FieldLockButton fieldLockButton5;
    private FieldLockButton fieldLockButton3;
    private FieldLockButton fieldLockButton1;
    private FieldLockButton lBtnApplicationDate;
    private TextBox txtFrequency;
    private Label labelChargeType2;
    private Label labelChargeType;
    private FieldLockButton fieldLockButton2;
    private Label label17;
    private TextBox txtTotalLateFee;
    private Label label19;
    private Label label13;
    private Label label4;
    private Label label2;
    private TextBox txtLateDaysEnd;
    private TextBox txtGracePeriodTriggerDate;
    private FieldLockButton fieldLockButton4;

    public LateFeeDetailsForm(IHtmlInput inputData, Sessions.Session session)
    {
      this.inputData = inputData;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.session = session;
      this.InitializeComponent();
      this.inputHandler = new GenericFormInputHandler(this.inputData, this.Controls, this.session);
      if (this.externalLateFeeSettings == null && this.loan != null)
        this.externalLateFeeSettings = this.session.ConfigurationManager.GetExternalOrgLateFeeSettings(this.inputData.GetField("TPO.X15"), true);
      this.initForm();
    }

    private void initForm()
    {
      this.inputHandler.SetFieldValuesToForm();
      this.txtGracePeriodTriggerDate.Enabled = false;
      this.txtLateDaysEnd.Enabled = false;
      if (this.txtGraceDays.Text != string.Empty)
        this.txtGraceDays.Text = Utils.ApplyFieldFormatting(this.txtGraceDays.Text.Trim(), FieldFormat.INTEGER);
      if (this.txtTotalDays.Text != string.Empty)
        this.txtTotalDays.Text = Utils.ApplyFieldFormatting(this.txtTotalDays.Text.Trim(), FieldFormat.INTEGER);
      this.inputHandler.SetBusinessRules(new ResourceManager(typeof (IncomeOtherForm)));
      this.inputHandler.SetFieldTip(this.toolTipField);
      for (int index = 0; index < this.inputHandler.FieldControls.Count; ++index)
        this.inputHandler.SetFieldEvents(this.inputHandler.FieldControls[index]);
      this.inputHandler.OnLeave += new EventHandler(this.textField_Leave);
      this.inputHandler.OnLockClicked += new EventHandler(this.lockButton_Clicked);
      this.datePicker_ValueChanged((object) null, (EventArgs) null);
      if (this.externalLateFeeSettings != null && this.externalLateFeeSettings.FeeHandledAs == 1)
      {
        this.btnLockLateFeeAdditional.Enabled = false;
        this.txtTotalLateFee.Text = this.txtLateFeeAdditional.Text = string.Empty;
        this.txtLateFeeAdditional.ReadOnly = true;
        this.txtLateFeeAdditional.BackColor = SystemColors.Control;
      }
      else
        this.txtTotalAdjust.Text = string.Empty;
      if (string.IsNullOrEmpty(this.loan.GetField("3571")) || !(Utils.ParseDecimal((object) this.loan.GetField("3571")) > 0M))
        return;
      this.labelChargeType.Text = "Current Principal";
      this.labelChargeType2.Text = "Current Principal";
    }

    private void lockButton_Clicked(object sender, EventArgs e) => this.lateFeeCalculation();

    private void textField_Leave(object sender, EventArgs e)
    {
      string str = string.Empty;
      TextBox textBox = (TextBox) null;
      if (sender is TextBox)
      {
        textBox = (TextBox) sender;
        str = textBox.Tag.ToString() ?? "";
        if (str == "3927" || str == "3930")
          textBox.Text = Utils.ApplyFieldFormatting(textBox.Text.Trim(), FieldFormat.INTEGER);
      }
      try
      {
        this.lateFeeCalculation();
      }
      catch (Exception ex)
      {
        if (str == "3927")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The Grace Period # of Days you just entered will cause Late Days Begin invalid! " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          if (textBox == null)
            return;
          textBox.Text = string.Empty;
          textBox.Focus();
        }
        else
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.inputHandler.SetFieldValuesToLoan();
      if (this.loan != null)
        this.loan.Calculator.FormCalculation("3926", (string) null, (string) null);
      this.DialogResult = DialogResult.OK;
    }

    private void datePicker_ValueChanged(object sender, EventArgs e)
    {
      if (sender != null)
      {
        string tag = (string) ((Control) sender).Tag;
        if (tag == "3928" || tag == "3929")
        {
          DateTime date1 = Utils.ParseDate((object) this.datBegin.Text);
          DateTime date2 = Utils.ParseDate((object) this.datEnd.Text);
          if (date1 != DateTime.MinValue && date2 != DateTime.MinValue && DateTime.Compare(date1, date2) > 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The Late Days Begin cannot be later than Late Days End!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            if (tag == "3928")
            {
              this.datBegin.Text = "";
              this.datBegin.Focus();
            }
            else
            {
              this.datEnd.Text = "";
              this.datEnd.Focus();
            }
          }
        }
      }
      this.lateFeeCalculation();
    }

    private void lateFeeCalculation()
    {
      if (this.externalLateFeeSettings != null)
      {
        List<DateTime> datesToCheck = new List<DateTime>();
        if ((this.externalLateFeeSettings.GracePeriodLaterOf & 1) == 1)
          datesToCheck.Add(Utils.ParseDate((object) this.inputData.GetField("3918")));
        if ((this.externalLateFeeSettings.GracePeriodLaterOf & 8) == 8)
          datesToCheck.Add(Utils.ParseDate((object) this.inputData.GetField("3919")));
        if ((this.externalLateFeeSettings.GracePeriodLaterOf & 2) == 2)
          datesToCheck.Add(Utils.ParseDate((object) this.inputData.GetField("3920")));
        if ((this.externalLateFeeSettings.GracePeriodLaterOf & 4) == 4)
          datesToCheck.Add(Utils.ParseDate((object) this.inputData.GetField("3926")));
        if ((this.externalLateFeeSettings.GracePeriodLaterOf & 16) == 16 && this.externalLateFeeSettings.OtherDate.StartsWith("Fields."))
        {
          string id = this.externalLateFeeSettings.OtherDate.Substring(7);
          if (id != string.Empty)
            datesToCheck.Add(Utils.ParseDate((object) this.inputData.GetField(id)));
        }
        if (this.datBegin.ReadOnly)
        {
          DateTime feeLatestBeginDate = this.loan.Calculator.GetCorrespondentLateFeeLatestBeginDate(datesToCheck, this.txtGraceDays.Text.Trim() != string.Empty ? Utils.ParseInt((object) this.txtGraceDays.Text) : 0, true);
          if (feeLatestBeginDate != DateTime.MinValue)
            this.datBegin.Text = feeLatestBeginDate != DateTime.MinValue ? feeLatestBeginDate.ToString("MM/dd/yyyy") : "";
        }
      }
      int lateFeeDays;
      if (this.txtTotalDays.ReadOnly)
      {
        lateFeeDays = Utils.GetTotalTimeSpanDays(this.datBegin.Text, this.datEnd.Text, true);
        this.txtTotalDays.Text = lateFeeDays > 0 ? lateFeeDays.ToString() : "";
      }
      else
        lateFeeDays = Utils.ParseInt((object) this.txtTotalDays.Text);
      if (this.txtTotalDays.ReadOnly && this.externalLateFeeSettings != null && this.externalLateFeeSettings.MaxLateDays > 0 && lateFeeDays > this.externalLateFeeSettings.MaxLateDays)
      {
        lateFeeDays = this.externalLateFeeSettings.MaxLateDays;
        this.txtTotalDays.Text = string.Concat((object) lateFeeDays);
      }
      Decimal num = this.loan.Calculator.CalcCorrespondentLateFeeCharge(Utils.ParseDecimal((object) this.txtLateFee.Text), Utils.ParseDecimal((object) this.txtLateFeeAdditional.Text), lateFeeDays);
      if (this.externalLateFeeSettings != null && this.externalLateFeeSettings.FeeHandledAs == 1)
      {
        this.txtTotalAdjust.Text = num.ToString("N5");
        this.txtLateFeeAdditional.Text = this.txtTotalLateFee.Text = string.Empty;
      }
      else
      {
        this.txtTotalLateFee.Text = num.ToString("N2");
        this.txtTotalAdjust.Text = string.Empty;
      }
      if (this.txtTotalDays.Text == string.Empty && this.externalLateFeeSettings != null && this.externalLateFeeSettings.CalculateAs == 1)
        this.txtTotalAdjust.Text = this.txtTotalLateFee.Text = string.Empty;
      if (this.txtFrequency.Text != string.Empty)
        this.labelOf1.Text = this.labelOf2.Text = "% of";
      else
        this.txtTotalAdjust.Text = this.txtTotalLateFee.Text = this.labelOf1.Text = this.labelOf2.Text = "";
      if (this.externalLateFeeSettings != null)
        return;
      this.txtTotalLateFee.Text = this.txtTotalAdjust.Text = string.Empty;
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
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.panel1 = new Panel();
      this.fieldLockButton4 = new FieldLockButton();
      this.txtLateDaysEnd = new TextBox();
      this.txtGracePeriodTriggerDate = new TextBox();
      this.label13 = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.label17 = new Label();
      this.txtTotalLateFee = new TextBox();
      this.label19 = new Label();
      this.fieldLockButton2 = new FieldLockButton();
      this.labelChargeType2 = new Label();
      this.labelChargeType = new Label();
      this.btnLockLateFeeAdditional = new FieldLockButton();
      this.fieldLockButton5 = new FieldLockButton();
      this.fieldLockButton3 = new FieldLockButton();
      this.fieldLockButton1 = new FieldLockButton();
      this.lBtnApplicationDate = new FieldLockButton();
      this.datEnd = new DatePicker();
      this.datBegin = new DatePicker();
      this.datGracePeriodStart = new DatePicker();
      this.labelOf1 = new Label();
      this.labelOf2 = new Label();
      this.txtNotes = new TextBox();
      this.label9 = new Label();
      this.txtTotalAdjust = new TextBox();
      this.label10 = new Label();
      this.txtFrequency = new TextBox();
      this.label11 = new Label();
      this.txtLateFeeAdditional = new TextBox();
      this.label12 = new Label();
      this.txtLateFee = new TextBox();
      this.label5 = new Label();
      this.txtTotalDays = new TextBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.txtGraceDays = new TextBox();
      this.label3 = new Label();
      this.toolTipField = new ToolTip(this.components);
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Grace Period Start Date";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(402, 422);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 11;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(311, 422);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.panel1.Controls.Add((Control) this.fieldLockButton4);
      this.panel1.Controls.Add((Control) this.txtLateDaysEnd);
      this.panel1.Controls.Add((Control) this.txtGracePeriodTriggerDate);
      this.panel1.Controls.Add((Control) this.label13);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label17);
      this.panel1.Controls.Add((Control) this.txtTotalLateFee);
      this.panel1.Controls.Add((Control) this.label19);
      this.panel1.Controls.Add((Control) this.fieldLockButton2);
      this.panel1.Controls.Add((Control) this.labelChargeType2);
      this.panel1.Controls.Add((Control) this.labelChargeType);
      this.panel1.Controls.Add((Control) this.btnLockLateFeeAdditional);
      this.panel1.Controls.Add((Control) this.fieldLockButton5);
      this.panel1.Controls.Add((Control) this.fieldLockButton3);
      this.panel1.Controls.Add((Control) this.fieldLockButton1);
      this.panel1.Controls.Add((Control) this.lBtnApplicationDate);
      this.panel1.Controls.Add((Control) this.datEnd);
      this.panel1.Controls.Add((Control) this.datBegin);
      this.panel1.Controls.Add((Control) this.datGracePeriodStart);
      this.panel1.Controls.Add((Control) this.labelOf1);
      this.panel1.Controls.Add((Control) this.labelOf2);
      this.panel1.Controls.Add((Control) this.txtNotes);
      this.panel1.Controls.Add((Control) this.label9);
      this.panel1.Controls.Add((Control) this.txtTotalAdjust);
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Controls.Add((Control) this.txtFrequency);
      this.panel1.Controls.Add((Control) this.label11);
      this.panel1.Controls.Add((Control) this.txtLateFeeAdditional);
      this.panel1.Controls.Add((Control) this.label12);
      this.panel1.Controls.Add((Control) this.txtLateFee);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.txtTotalDays);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.txtGraceDays);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Location = new Point(12, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(496, 404);
      this.panel1.TabIndex = 0;
      this.fieldLockButton4.Location = new Point(129, 10);
      this.fieldLockButton4.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton4.MaximumSize = new Size(16, 17);
      this.fieldLockButton4.MinimumSize = new Size(16, 17);
      this.fieldLockButton4.Name = "fieldLockButton4";
      this.fieldLockButton4.Size = new Size(16, 17);
      this.fieldLockButton4.TabIndex = 71;
      this.fieldLockButton4.TabStop = false;
      this.fieldLockButton4.Tag = (object) "4110";
      this.fieldLockButton4.UnlockedStateToolTip = "Enter Data Manually";
      this.txtLateDaysEnd.BorderStyle = BorderStyle.FixedSingle;
      this.txtLateDaysEnd.Location = new Point(338, 72);
      this.txtLateDaysEnd.MaxLength = 9;
      this.txtLateDaysEnd.Name = "txtLateDaysEnd";
      this.txtLateDaysEnd.Size = new Size(141, 20);
      this.txtLateDaysEnd.TabIndex = 70;
      this.txtLateDaysEnd.Tag = (object) "4112";
      this.txtGracePeriodTriggerDate.BorderStyle = BorderStyle.FixedSingle;
      this.txtGracePeriodTriggerDate.Location = new Point(338, 9);
      this.txtGracePeriodTriggerDate.MaxLength = 9;
      this.txtGracePeriodTriggerDate.Name = "txtGracePeriodTriggerDate";
      this.txtGracePeriodTriggerDate.Size = new Size(141, 20);
      this.txtGracePeriodTriggerDate.TabIndex = 69;
      this.txtGracePeriodTriggerDate.Tag = (object) "4111";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(266, 75);
      this.label13.Name = "label13";
      this.label13.Size = new Size(66, 13);
      this.label13.TabIndex = 68;
      this.label13.Text = "Trigger Date";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(266, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(66, 13);
      this.label4.TabIndex = 67;
      this.label4.Text = "Trigger Date";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(3, 114);
      this.label2.Name = "label2";
      this.label2.Size = new Size(125, 13);
      this.label2.TabIndex = 66;
      this.label2.Text = "(To maximum if specified)";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(132, 256);
      this.label17.Name = "label17";
      this.label17.Size = new Size(13, 13);
      this.label17.TabIndex = 65;
      this.label17.Text = "$";
      this.txtTotalLateFee.BackColor = SystemColors.Control;
      this.txtTotalLateFee.BorderStyle = BorderStyle.FixedSingle;
      this.txtTotalLateFee.Location = new Point(148, 264);
      this.txtTotalLateFee.Name = "txtTotalLateFee";
      this.txtTotalLateFee.ReadOnly = true;
      this.txtTotalLateFee.Size = new Size(100, 20);
      this.txtTotalLateFee.TabIndex = 63;
      this.txtTotalLateFee.TabStop = false;
      this.txtTotalLateFee.Tag = (object) "3937";
      this.txtTotalLateFee.TextAlign = HorizontalAlignment.Right;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(3, 271);
      this.label19.Name = "label19";
      this.label19.Size = new Size(76, 13);
      this.label19.TabIndex = 64;
      this.label19.Text = "Total Late Fee";
      this.fieldLockButton2.Location = new Point(129, 96);
      this.fieldLockButton2.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton2.MaximumSize = new Size(16, 17);
      this.fieldLockButton2.MinimumSize = new Size(16, 17);
      this.fieldLockButton2.Name = "fieldLockButton2";
      this.fieldLockButton2.Size = new Size(16, 17);
      this.fieldLockButton2.TabIndex = 62;
      this.fieldLockButton2.TabStop = false;
      this.fieldLockButton2.Tag = (object) "3930";
      this.fieldLockButton2.UnlockedStateToolTip = "Enter Data Manually";
      this.labelChargeType2.AutoSize = true;
      this.labelChargeType2.Location = new Point(277, 240);
      this.labelChargeType2.Name = "labelChargeType2";
      this.labelChargeType2.Size = new Size(155, 13);
      this.labelChargeType2.TabIndex = 60;
      this.labelChargeType2.Tag = (object) "3936";
      this.labelChargeType2.Text = "Total Loan Amount (preliminary)";
      this.labelChargeType.AutoSize = true;
      this.labelChargeType.Location = new Point(277, 168);
      this.labelChargeType.Name = "labelChargeType";
      this.labelChargeType.Size = new Size(155, 13);
      this.labelChargeType.TabIndex = 59;
      this.labelChargeType.Tag = (object) "3936";
      this.labelChargeType.Text = "Total Loan Amount (preliminary)";
      this.btnLockLateFeeAdditional.Location = new Point(129, 190);
      this.btnLockLateFeeAdditional.LockedStateToolTip = "Use Default Value";
      this.btnLockLateFeeAdditional.MaximumSize = new Size(16, 17);
      this.btnLockLateFeeAdditional.MinimumSize = new Size(16, 17);
      this.btnLockLateFeeAdditional.Name = "btnLockLateFeeAdditional";
      this.btnLockLateFeeAdditional.Size = new Size(16, 17);
      this.btnLockLateFeeAdditional.TabIndex = 56;
      this.btnLockLateFeeAdditional.TabStop = false;
      this.btnLockLateFeeAdditional.Tag = (object) "3932";
      this.btnLockLateFeeAdditional.UnlockedStateToolTip = "Enter Data Manually";
      this.fieldLockButton5.Location = new Point(129, 168);
      this.fieldLockButton5.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton5.MaximumSize = new Size(16, 17);
      this.fieldLockButton5.MinimumSize = new Size(16, 17);
      this.fieldLockButton5.Name = "fieldLockButton5";
      this.fieldLockButton5.Size = new Size(16, 17);
      this.fieldLockButton5.TabIndex = 55;
      this.fieldLockButton5.TabStop = false;
      this.fieldLockButton5.Tag = (object) "3931";
      this.fieldLockButton5.UnlockedStateToolTip = "Enter Data Manually";
      this.fieldLockButton3.Location = new Point(129, 74);
      this.fieldLockButton3.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton3.MaximumSize = new Size(16, 17);
      this.fieldLockButton3.MinimumSize = new Size(16, 17);
      this.fieldLockButton3.Name = "fieldLockButton3";
      this.fieldLockButton3.Size = new Size(16, 17);
      this.fieldLockButton3.TabIndex = 53;
      this.fieldLockButton3.TabStop = false;
      this.fieldLockButton3.Tag = (object) "3929";
      this.fieldLockButton3.UnlockedStateToolTip = "Enter Data Manually";
      this.fieldLockButton1.Location = new Point(129, 52);
      this.fieldLockButton1.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton1.MaximumSize = new Size(16, 17);
      this.fieldLockButton1.MinimumSize = new Size(16, 17);
      this.fieldLockButton1.Name = "fieldLockButton1";
      this.fieldLockButton1.Size = new Size(16, 17);
      this.fieldLockButton1.TabIndex = 52;
      this.fieldLockButton1.TabStop = false;
      this.fieldLockButton1.Tag = (object) "3928";
      this.fieldLockButton1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnApplicationDate.Location = new Point(129, 30);
      this.lBtnApplicationDate.LockedStateToolTip = "Use Default Value";
      this.lBtnApplicationDate.MaximumSize = new Size(16, 17);
      this.lBtnApplicationDate.MinimumSize = new Size(16, 17);
      this.lBtnApplicationDate.Name = "lBtnApplicationDate";
      this.lBtnApplicationDate.Size = new Size(16, 17);
      this.lBtnApplicationDate.TabIndex = 51;
      this.lBtnApplicationDate.TabStop = false;
      this.lBtnApplicationDate.Tag = (object) "3927";
      this.lBtnApplicationDate.UnlockedStateToolTip = "Enter Data Manually";
      this.datEnd.BackColor = SystemColors.Window;
      this.datEnd.ForeColor = SystemColors.ControlText;
      this.datEnd.Location = new Point(148, 72);
      this.datEnd.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.datEnd.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datEnd.Name = "datEnd";
      this.datEnd.Size = new Size(100, 21);
      this.datEnd.TabIndex = 5;
      this.datEnd.Tag = (object) "3929";
      this.datEnd.ToolTip = "";
      this.datEnd.Value = new DateTime(0L);
      this.datEnd.ValueChanged += new EventHandler(this.datePicker_ValueChanged);
      this.datBegin.BackColor = SystemColors.Window;
      this.datBegin.ForeColor = SystemColors.ControlText;
      this.datBegin.Location = new Point(148, 50);
      this.datBegin.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.datBegin.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datBegin.Name = "datBegin";
      this.datBegin.Size = new Size(100, 21);
      this.datBegin.TabIndex = 4;
      this.datBegin.Tag = (object) "3928";
      this.datBegin.ToolTip = "";
      this.datBegin.Value = new DateTime(0L);
      this.datBegin.ValueChanged += new EventHandler(this.datePicker_ValueChanged);
      this.datGracePeriodStart.BackColor = SystemColors.Window;
      this.datGracePeriodStart.ForeColor = SystemColors.ControlText;
      this.datGracePeriodStart.Location = new Point(148, 6);
      this.datGracePeriodStart.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.datGracePeriodStart.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datGracePeriodStart.Name = "datGracePeriodStart";
      this.datGracePeriodStart.ReadOnly = true;
      this.datGracePeriodStart.Size = new Size(100, 21);
      this.datGracePeriodStart.TabIndex = 0;
      this.datGracePeriodStart.TabStop = false;
      this.datGracePeriodStart.Tag = (object) "4110";
      this.datGracePeriodStart.ToolTip = "";
      this.datGracePeriodStart.Value = new DateTime(0L);
      this.labelOf1.AutoSize = true;
      this.labelOf1.Location = new Point(253, 168);
      this.labelOf1.Name = "labelOf1";
      this.labelOf1.Size = new Size(27, 13);
      this.labelOf1.TabIndex = 26;
      this.labelOf1.Text = "% of";
      this.labelOf2.AutoSize = true;
      this.labelOf2.Location = new Point(253, 240);
      this.labelOf2.Name = "labelOf2";
      this.labelOf2.Size = new Size(27, 13);
      this.labelOf2.TabIndex = 25;
      this.labelOf2.Text = "% of";
      this.txtNotes.BorderStyle = BorderStyle.FixedSingle;
      this.txtNotes.Location = new Point(148, 293);
      this.txtNotes.Multiline = true;
      this.txtNotes.Name = "txtNotes";
      this.txtNotes.ScrollBars = ScrollBars.Both;
      this.txtNotes.Size = new Size(331, 98);
      this.txtNotes.TabIndex = 9;
      this.txtNotes.Tag = (object) "3935";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(0, 293);
      this.label9.Name = "label9";
      this.label9.Size = new Size(35, 13);
      this.label9.TabIndex = 23;
      this.label9.Text = "Notes";
      this.txtTotalAdjust.BorderStyle = BorderStyle.FixedSingle;
      this.txtTotalAdjust.Location = new Point(148, 238);
      this.txtTotalAdjust.Name = "txtTotalAdjust";
      this.txtTotalAdjust.ReadOnly = true;
      this.txtTotalAdjust.Size = new Size(100, 20);
      this.txtTotalAdjust.TabIndex = 0;
      this.txtTotalAdjust.TabStop = false;
      this.txtTotalAdjust.Tag = (object) "3934";
      this.txtTotalAdjust.TextAlign = HorizontalAlignment.Right;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(3, 245);
      this.label10.Name = "label10";
      this.label10.Size = new Size(93, 13);
      this.label10.TabIndex = 21;
      this.label10.Text = "Total Price Adjust.";
      this.txtFrequency.BorderStyle = BorderStyle.FixedSingle;
      this.txtFrequency.Location = new Point(148, 209);
      this.txtFrequency.Name = "txtFrequency";
      this.txtFrequency.ReadOnly = true;
      this.txtFrequency.Size = new Size(100, 20);
      this.txtFrequency.TabIndex = 0;
      this.txtFrequency.TabStop = false;
      this.txtFrequency.Tag = (object) "3933";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(3, 212);
      this.label11.Name = "label11";
      this.label11.Size = new Size(57, 13);
      this.label11.TabIndex = 19;
      this.label11.Text = "Frequency";
      this.txtLateFeeAdditional.BorderStyle = BorderStyle.FixedSingle;
      this.txtLateFeeAdditional.Location = new Point(148, 187);
      this.txtLateFeeAdditional.MaxLength = 10;
      this.txtLateFeeAdditional.Name = "txtLateFeeAdditional";
      this.txtLateFeeAdditional.Size = new Size(100, 20);
      this.txtLateFeeAdditional.TabIndex = 8;
      this.txtLateFeeAdditional.Tag = (object) "3932";
      this.txtLateFeeAdditional.TextAlign = HorizontalAlignment.Right;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(106, 191);
      this.label12.Name = "label12";
      this.label12.Size = new Size(22, 13);
      this.label12.TabIndex = 17;
      this.label12.Text = "+ $";
      this.txtLateFee.BackColor = SystemColors.Window;
      this.txtLateFee.BorderStyle = BorderStyle.FixedSingle;
      this.txtLateFee.Location = new Point(148, 165);
      this.txtLateFee.MaxLength = 10;
      this.txtLateFee.Name = "txtLateFee";
      this.txtLateFee.Size = new Size(100, 20);
      this.txtLateFee.TabIndex = 7;
      this.txtLateFee.Tag = (object) "3931";
      this.txtLateFee.TextAlign = HorizontalAlignment.Right;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(3, 168);
      this.label5.Name = "label5";
      this.label5.Size = new Size(49, 13);
      this.label5.TabIndex = 15;
      this.label5.Text = "Late Fee";
      this.txtTotalDays.BorderStyle = BorderStyle.FixedSingle;
      this.txtTotalDays.Location = new Point(148, 94);
      this.txtTotalDays.MaxLength = 9;
      this.txtTotalDays.Name = "txtTotalDays";
      this.txtTotalDays.Size = new Size(100, 20);
      this.txtTotalDays.TabIndex = 6;
      this.txtTotalDays.Tag = (object) "3930";
      this.txtTotalDays.TextAlign = HorizontalAlignment.Right;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 97);
      this.label6.Name = "label6";
      this.label6.Size = new Size(82, 13);
      this.label6.TabIndex = 13;
      this.label6.Text = "Total Late Days";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(3, 75);
      this.label7.Name = "label7";
      this.label7.Size = new Size(77, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Late Days End";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3, 53);
      this.label8.Name = "label8";
      this.label8.Size = new Size(85, 13);
      this.label8.TabIndex = 9;
      this.label8.Text = "Late Days Begin";
      this.txtGraceDays.BorderStyle = BorderStyle.FixedSingle;
      this.txtGraceDays.Location = new Point(148, 28);
      this.txtGraceDays.MaxLength = 9;
      this.txtGraceDays.Name = "txtGraceDays";
      this.txtGraceDays.Size = new Size(100, 20);
      this.txtGraceDays.TabIndex = 3;
      this.txtGraceDays.Tag = (object) "3927";
      this.txtGraceDays.TextAlign = HorizontalAlignment.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3, 31);
      this.label3.Name = "label3";
      this.label3.Size = new Size(118, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Grace Period # of Days";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(520, 451);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LateFeeDetailsForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Late Fee Details";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
