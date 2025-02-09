// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PaymentDetailForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PaymentDetailForm : Form
  {
    private CheckDetailForm checkDetailForm;
    private AutoClearHouseForm clearHouseForm;
    private LockBoxForm lockBoxForm;
    private WireForm wireForm;
    private int nextPaymentNo;
    private double maxPrincipal;
    private double maxInterest;
    private IPaymentMethod subForm;
    private bool isReadOnly;
    private LoanData loan;
    private Hashtable paymentMethodFields;
    private bool refreshValuesFromLog = true;
    private bool addNew;
    private string preStatementDate = string.Empty;
    private string prePaymentDueDate = string.Empty;
    private string preLatePaymentDate = string.Empty;
    private double valueBefore;
    private PaymentTransactionLog paymentLog;
    private IContainer components;
    private Button btnSave;
    private Label label1;
    private TextBox boxNo;
    private ComboBox cboPaymentType;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private Label label11;
    private Label label12;
    private Label label13;
    private Label label16;
    private Label label17;
    private Label label18;
    private Label label19;
    private TextBox textBoxIndexRate;
    private TextBox textBoxInterestRate;
    private TextBox textBoxTotalAmount;
    private TextBox textBoxPrincipal;
    private TextBox textBoxInterest;
    private TextBox textBoxEscrow;
    private TextBox textBoxLateFee;
    private TextBox textBoxMiscFee;
    private TextBox textBoxAddPrincipal;
    private TextBox textBoxAddEscrow;
    private Button btnCancel;
    private Label label20;
    private TextBox textBox20;
    private TextBox textBox21;
    private Label label21;
    private Label label22;
    private Panel panelDetail;
    private Label label24;
    private Label label25;
    private Label label26;
    private Label label27;
    private Label label28;
    private Label label30;
    private Label label31;
    private Label label32;
    private Label label33;
    private Label labelCreatedBy;
    private Label labelModifiedBy;
    private Label label23;
    private TextBox textBoxAllocated;
    private Label label15;
    private Label label35;
    private TextBox textBoxDiff;
    private Label label29;
    private Label label36;
    private Label label34;
    private TextBox textBoxAmountDue;
    private Label label14;
    protected EMHelpLink emHelpLink1;
    private DatePicker textBoxStatementDate;
    private DatePicker textBoxDepositedDate;
    private DatePicker textBoxReceivedDate;
    private DatePicker textBoxLatePaymentDate;
    private DatePicker textBoxPaymentDueDate;
    private GroupContainer groupContainer1;
    private TextBox textBoxUSDAMonthlyPremium;
    private Label label46;
    private Label label47;
    private TextBox textBoxOther1Escrow;
    private Label label44;
    private Label label45;
    private TextBox textBoxCityPropertyTax;
    private Label label42;
    private Label label43;
    private TextBox textBoxFloodInsurance;
    private Label label40;
    private Label label41;
    private TextBox textBoxHazardInsurance;
    private Label label38;
    private Label label39;
    private TextBox textBoxTax;
    private Label label10;
    private Label label37;
    private TextBox textBoxMortgageInsurance;
    private Label label48;
    private Label label49;
    private TextBox textBoxOther3Escrow;
    private Label label53;
    private Label label52;
    private TextBox textBoxOther2Escrow;
    private Label label50;
    private Label label51;

    public PaymentDetailForm(PaymentTransactionLog paymentLog)
    {
      this.isReadOnly = true;
      this.paymentLog = paymentLog;
      this.InitializeComponent();
      this.Text = "View Transaction";
      this.btnCancel.Text = "OK";
      this.btnSave.Visible = false;
      this.initForm();
      this.labelCreatedBy.Text = "Created by " + this.paymentLog.CreatedByName + " on " + this.paymentLog.CreatedDateTime.ToString("MM/dd/yyyy hh:mm tt");
      if (this.paymentLog.ModifiedByName != "")
        this.labelModifiedBy.Text = "Last modified by " + this.paymentLog.ModifiedByName + " on " + this.paymentLog.ModifiedDateTime.ToString("MM/dd/yyyy hh:mm tt");
      else
        this.labelModifiedBy.Text = "";
    }

    public PaymentDetailForm(
      PaymentTransactionLog paymentLog,
      int transNo,
      int nextPaymentNo,
      double maxPrincipal,
      double maxInterest,
      LoanData loan,
      bool addNew)
    {
      this.addNew = addNew;
      this.nextPaymentNo = nextPaymentNo;
      this.paymentLog = paymentLog;
      this.maxPrincipal = maxPrincipal;
      this.maxInterest = maxInterest;
      this.loan = loan;
      this.InitializeComponent();
      this.initForm();
      if (paymentLog == null || nextPaymentNo != -1)
        this.boxNo.Text = nextPaymentNo.ToString();
      this.groupContainer1.Text = "T" + transNo.ToString("00") + " Payment";
      this.labelCreatedBy.Text = "Created by " + this.paymentLog.CreatedByName + " on " + this.paymentLog.CreatedDateTime.ToString("MM/dd/yyyy hh:mm tt");
      if (!this.addNew && this.paymentLog.ModifiedByName != "")
        this.labelModifiedBy.Text = "Last modified by " + this.paymentLog.ModifiedByName + " on " + this.paymentLog.ModifiedDateTime.ToString("MM/dd/yyyy hh:mm tt");
      else
        this.labelModifiedBy.Text = "";
      if (this.addNew)
        this.doAllocation(true, false);
      this.doCalculation();
      this.textBoxTotalAmount.Enter += new EventHandler(this.textBoxTotalAmount_Enter);
      this.textBoxTotalAmount.Leave += new EventHandler(this.decimal_FieldLeave);
      this.textBoxTotalAmount.TextChanged += new EventHandler(this.textBoxTotalAmount_TextChanged);
      if (this.loan.GetField("HUD24") == "" & addNew)
      {
        this.textBoxEscrow.Text = "";
      }
      else
      {
        if (!(this.loan.GetField("HUD24") != this.loan.GetField("HUD26") & addNew))
          return;
        double num = Math.Round((this.textBoxTax.Text != "" ? Utils.ParseDouble((object) this.textBoxTax.Text) : 0.0) + (this.textBoxMortgageInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxMortgageInsurance.Text) : 0.0) + (this.textBoxFloodInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxFloodInsurance.Text) : 0.0) + (this.textBoxHazardInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxHazardInsurance.Text) : 0.0) + (this.textBoxCityPropertyTax.Text != "" ? Utils.ParseDouble((object) this.textBoxCityPropertyTax.Text) : 0.0) + (this.textBoxOther1Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther1Escrow.Text) : 0.0) + (this.textBoxOther2Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther2Escrow.Text) : 0.0) + (this.textBoxOther3Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther3Escrow.Text) : 0.0) + (this.textBoxUSDAMonthlyPremium.Text != "" ? Utils.ParseDouble((object) this.textBoxUSDAMonthlyPremium.Text) : 0.0), 2);
        this.textBoxAllocated.Text = Math.Round(Utils.ParseDouble((object) this.textBoxPrincipal.Text) + Utils.ParseDouble((object) this.textBoxInterest.Text) + num + Utils.ParseDouble((object) this.textBoxLateFee.Text) + Utils.ParseDouble((object) this.textBoxMiscFee.Text) + Utils.ParseDouble((object) this.textBoxAddPrincipal.Text) + Utils.ParseDouble((object) this.textBoxAddEscrow.Text), 2).ToString("N2");
        this.textBoxDiff.Text = Math.Round(Utils.ParseDouble((object) this.textBoxAmountDue.Text) - Utils.ParseDouble((object) this.textBoxAllocated.Text), 2).ToString("N2");
        if (this.textBoxAllocated.Text == "0")
          this.textBoxAllocated.Text = "";
        if (!(this.textBoxDiff.Text == "0"))
          return;
        this.textBoxDiff.Text = "";
      }
    }

    private void textBoxTotalAmount_Enter(object sender, EventArgs e)
    {
      this.valueBefore = Utils.ParseDouble((object) this.textBoxTotalAmount.Text);
    }

    private void initForm()
    {
      this.cboPaymentType.Items.AddRange((object[]) ServicingEnum.ServicingPaymentMethodsUI);
      if (this.paymentLog == null)
        this.cboPaymentType.SelectedIndex = 1;
      this.cboPaymentType.Text = ServicingEnum.ServicingPaymentMethodsToUI(this.paymentLog.PaymentMethod);
      this.cboPaymentType_SelectedIndexChanged((object) null, (EventArgs) null);
      this.setFormFields(this.Controls);
      if (this.addNew)
        this.textBoxTotalAmount_TextChanged((object) null, (EventArgs) null);
      this.cboPaymentType.SelectedIndexChanged += new EventHandler(this.cboPaymentType_SelectedIndexChanged);
      this.preStatementDate = this.textBoxStatementDate.Text;
      this.prePaymentDueDate = this.textBoxPaymentDueDate.Text;
      this.preLatePaymentDate = this.textBoxLatePaymentDate.Text;
    }

    private void setFormFields(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox = (TextBox) c;
            if (this.isReadOnly)
            {
              textBox.ReadOnly = true;
              textBox.BackColor = Color.WhiteSmoke;
            }
            if (textBox != null && textBox.Tag != null)
            {
              string str = textBox.Tag.ToString();
              if (!(str == string.Empty))
              {
                if (this.refreshValuesFromLog)
                {
                  textBox.Text = this.paymentLog.GetField(str, this.loan?.GetField("4912") == "FiveDecimals");
                  continue;
                }
                if (this.paymentMethodFields.ContainsKey((object) str))
                {
                  textBox.Text = this.paymentMethodFields[(object) str].ToString();
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            if (this.isReadOnly)
              comboBox.Enabled = false;
            if (comboBox != null && comboBox.Tag != null)
            {
              string str = comboBox.Tag.ToString();
              if (!(str == string.Empty) && !(str == "PaymentMethod"))
              {
                if (this.refreshValuesFromLog)
                {
                  comboBox.Text = this.paymentLog.GetField(str);
                  continue;
                }
                if (this.paymentMethodFields.ContainsKey((object) str))
                {
                  comboBox.Text = this.paymentMethodFields[(object) str].ToString();
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case DatePicker _:
            DatePicker datePicker = (DatePicker) c;
            if (this.isReadOnly)
            {
              datePicker.Enabled = false;
              datePicker.BackColor = Color.WhiteSmoke;
            }
            if (datePicker != null && datePicker.Tag != null)
            {
              string str = datePicker.Tag.ToString();
              if (!(str == string.Empty))
              {
                if (this.refreshValuesFromLog)
                {
                  datePicker.Text = this.paymentLog.GetField(str);
                  continue;
                }
                if (this.paymentMethodFields.ContainsKey((object) str))
                {
                  datePicker.Text = this.paymentMethodFields[(object) str].ToString();
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          default:
            this.setFormFields(c.Controls);
            continue;
        }
      }
    }

    private void getFormFields(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox = (TextBox) c;
            if (textBox != null && textBox.Tag != null)
            {
              string id = textBox.Tag.ToString();
              if (!(id == string.Empty))
              {
                this.paymentLog.SetField(id, textBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            if (comboBox != null && comboBox.Tag != null)
            {
              string id = comboBox.Tag.ToString();
              if (!(id == string.Empty))
              {
                this.paymentLog.SetField(id, comboBox.Text);
                continue;
              }
              continue;
            }
            continue;
          case DatePicker _:
            DatePicker datePicker = (DatePicker) c;
            if (datePicker != null && datePicker.Tag != null)
            {
              string id = datePicker.Tag.ToString();
              if (!(id == string.Empty))
              {
                this.paymentLog.SetField(id, datePicker.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case Label _:
            continue;
          default:
            this.getFormFields(c.Controls);
            continue;
        }
      }
    }

    private void cboPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.paymentMethodFields == null)
        this.paymentMethodFields = new Hashtable();
      if (this.subForm != null)
        this.subForm.GetFieldValues(this.paymentMethodFields);
      this.subForm = (IPaymentMethod) null;
      switch (this.cboPaymentType.SelectedIndex)
      {
        case 1:
          this.checkDetailForm = new CheckDetailForm();
          this.subForm = (IPaymentMethod) this.checkDetailForm;
          break;
        case 2:
          this.clearHouseForm = new AutoClearHouseForm();
          this.subForm = (IPaymentMethod) this.clearHouseForm;
          break;
        case 3:
          this.lockBoxForm = new LockBoxForm();
          this.subForm = (IPaymentMethod) this.lockBoxForm;
          break;
        case 4:
          this.wireForm = new WireForm();
          this.subForm = (IPaymentMethod) this.wireForm;
          break;
      }
      foreach (Control control in (ArrangedElementCollection) this.panelDetail.Controls)
        this.panelDetail.Controls.Remove(control);
      if (this.subForm == null)
        return;
      this.panelDetail.Controls.Add((Control) this.subForm);
      this.refreshValuesFromLog = false;
      this.setFormFields(((Control) this.subForm).Controls);
      this.refreshValuesFromLog = true;
      this.textBoxTotalAmount_TextChanged((object) null, (EventArgs) null);
    }

    private void decimal_KeyUp(object sender, KeyEventArgs e)
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

    private void decimal_FieldLeave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num1 = Utils.ParseDouble((object) textBox.Text);
      if (num1 != 0.0)
        textBox.Text = num1.ToString("N2");
      else
        textBox.Text = "";
      if (textBox.Name == "textBoxTotalAmount")
      {
        if (this.valueBefore == num1 || num1 == Utils.ParseDouble((object) this.textBoxAmountDue.Text))
          return;
        this.doAllocation(false, false);
      }
      else
      {
        if (!(textBox.Name == "textBoxEscrow") || Utils.ParseDouble((object) this.textBoxTax.Text) + Utils.ParseDouble((object) this.textBoxMortgageInsurance.Text) + Utils.ParseDouble((object) this.textBoxFloodInsurance.Text) + Utils.ParseDouble((object) this.textBoxHazardInsurance.Text) + Utils.ParseDouble((object) this.textBoxCityPropertyTax.Text) + Utils.ParseDouble((object) this.textBoxOther1Escrow.Text) + Utils.ParseDouble((object) this.textBoxOther2Escrow.Text) + Utils.ParseDouble((object) this.textBoxOther3Escrow.Text) + Utils.ParseDouble((object) this.textBoxUSDAMonthlyPremium.Text) == num1)
          return;
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The Escrow Total must equal the sum of Taxes, Hazard Insurance, Mortgage Insurance, Flood Insurance, City Property Tax, Other1, Other2, Other3 and USDA Monthly Premium.\n\n Correct the approprite values.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (Utils.ParseDate((object) this.textBoxStatementDate.Text) == DateTime.MinValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Statement Date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxStatementDate.Focus();
      }
      else if (Utils.ParseDate((object) this.textBoxLatePaymentDate.Text) == DateTime.MinValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Late Payment Date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxLatePaymentDate.Focus();
      }
      else if (Utils.ParseDate((object) this.textBoxReceivedDate.Text) == DateTime.MinValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Payment Received Date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxReceivedDate.Focus();
      }
      else if (Utils.ParseDate((object) this.textBoxLatePaymentDate.Text) < Utils.ParseDate((object) this.textBoxPaymentDueDate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Payment Due Date cannot be greater than Late Payment Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxLatePaymentDate.Focus();
      }
      else if (Utils.ParseDate((object) this.textBoxLatePaymentDate.Text) < Utils.ParseDate((object) this.textBoxStatementDate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Statement Date cannot be greater than Late Payment Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxStatementDate.Focus();
      }
      else
      {
        double num1 = Math.Round((this.textBoxTax.Text != "" ? Utils.ParseDouble((object) this.textBoxTax.Text) : 0.0) + (this.textBoxMortgageInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxMortgageInsurance.Text) : 0.0) + (this.textBoxFloodInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxFloodInsurance.Text) : 0.0) + (this.textBoxHazardInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxHazardInsurance.Text) : 0.0) + (this.textBoxCityPropertyTax.Text != "" ? Utils.ParseDouble((object) this.textBoxCityPropertyTax.Text) : 0.0) + (this.textBoxOther1Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther1Escrow.Text) : 0.0) + (this.textBoxOther2Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther2Escrow.Text) : 0.0) + (this.textBoxOther3Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther3Escrow.Text) : 0.0) + (this.textBoxUSDAMonthlyPremium.Text != "" ? Utils.ParseDouble((object) this.textBoxUSDAMonthlyPremium.Text) : 0.0), 2);
        if ((this.textBoxEscrow.Text != "" ? Utils.ParseDouble((object) this.textBoxEscrow.Text) : 0.0) != num1)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The total Escrow is not equal to the sum of the Taxes, Hazard Insurance, Mortgage Insurance, Flood Insurance, City Property Tax, Other1, Other2, Other3 and USDA Monthly Premium.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (Utils.ParseDouble((object) this.textBoxDiff.Text) != 0.0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The Total Amount Received is not Equal To Total Amount Allocated.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.textBoxPrincipal.Focus();
        }
        else
        {
          this.getFormFields(this.Controls);
          if (!this.addNew)
          {
            this.paymentLog.ModifiedByID = Session.UserInfo.Userid;
            this.paymentLog.ModifiedByName = Session.UserInfo.FullName;
            this.paymentLog.ModifiedDateTime = DateTime.Now;
          }
          if (this.textBoxPaymentDueDate.Value < this.paymentLog.PaymentIndexDate)
            this.paymentLog.PaymentIndexDate = this.textBoxPaymentDueDate.Value;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    public PaymentTransactionLog PaymentLog => this.paymentLog;

    private void textBoxTotalAmount_TextChanged(object sender, EventArgs e)
    {
      if (this.subForm == null)
        return;
      double amount = Utils.ParseDouble((object) this.textBoxTotalAmount.Text);
      if (this.paymentLog.BuydownSubsidyAmount > 0.0)
        amount -= this.paymentLog.BuydownSubsidyAmount;
      this.subForm.UpdateAmount(amount);
      this.doCalculation();
    }

    private void doCalculation()
    {
      double val = Utils.ParseDouble((object) this.textBoxPrincipal.Text) + Utils.ParseDouble((object) this.textBoxInterest.Text) + Utils.ParseDouble((object) this.textBoxEscrow.Text) + Utils.ParseDouble((object) this.textBoxLateFee.Text) + Utils.ParseDouble((object) this.textBoxMiscFee.Text) + Utils.ParseDouble((object) this.textBoxAddPrincipal.Text) + Utils.ParseDouble((object) this.textBoxAddEscrow.Text);
      if (this.paymentLog.BuydownSubsidyAmount > 0.0)
        val += this.paymentLog.BuydownSubsidyAmount;
      if (val != 0.0)
        this.textBoxAllocated.Text = val.ToString("N2");
      else
        this.textBoxAllocated.Text = "";
      val -= Utils.ParseDouble((object) this.textBoxTotalAmount.Text);
      val *= -1.0;
      if (Utils.ArithmeticRounding(val, 2) != 0.0)
        this.textBoxDiff.Text = val.ToString("N2");
      else
        this.textBoxDiff.Text = "";
    }

    private bool doAllocation(bool skipMsg, bool checkOnly)
    {
      if (!skipMsg && !checkOnly && Utils.Dialog((IWin32Window) this, "The Total Amount Received is different from the Total Amount Due.\r\nThis payment will be applied first to Interest, followed by Escrow, Misc. Fee, Principal, Late Fee (if any), and Additional Principal (as appropriate). In addition, the Escrow total must equal the sum of the individual escrow items.\r\nYou can change the payment allocation later. ", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
        return false;
      double val1 = Utils.ParseDouble((object) this.textBoxTotalAmount.Text);
      double principal = this.paymentLog.Principal;
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X92")) + this.paymentLog.Interest, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X93")) + this.paymentLog.Escrow, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X94")) + this.paymentLog.MiscFee, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X95")) + this.paymentLog.MiscFee, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X130")) + this.paymentLog.EscowTaxes, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X131")) + this.paymentLog.MortgageInsurance, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X132")) + this.paymentLog.HazardInsurance, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X133")) + this.paymentLog.FloodInsurance, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X134")) + this.paymentLog.CityPropertyTax, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X135")) + this.paymentLog.Other1Escrow, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X136")) + this.paymentLog.Other2Escrow, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X137")) + this.paymentLog.Other3Escrow, 2);
      Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetField("SERVICE.X138")) + this.paymentLog.USDAMonthlyPremium, 2);
      if (!checkOnly)
      {
        this.textBoxPrincipal.Text = "";
        this.textBoxInterest.Text = "";
        this.textBoxEscrow.Text = "";
        this.textBoxMiscFee.Text = "";
        this.textBoxLateFee.Text = "";
        this.textBoxAddPrincipal.Text = "";
        this.textBoxAddEscrow.Text = "";
        this.textBoxTax.Text = "";
        this.textBoxMortgageInsurance.Text = "";
        this.textBoxFloodInsurance.Text = "";
        this.textBoxHazardInsurance.Text = "";
        this.textBoxCityPropertyTax.Text = "";
        this.textBoxOther1Escrow.Text = "";
        this.textBoxOther2Escrow.Text = "";
        this.textBoxOther3Escrow.Text = "";
        this.textBoxUSDAMonthlyPremium.Text = "";
      }
      TextBox textBoxTax = this.textBoxTax;
      double num;
      string str1;
      if (this.paymentLog.EscowTaxes <= 0.0)
      {
        str1 = "";
      }
      else
      {
        num = this.paymentLog.EscowTaxes;
        str1 = num.ToString("N2");
      }
      textBoxTax.Text = str1;
      TextBox mortgageInsurance = this.textBoxMortgageInsurance;
      string str2;
      if (this.paymentLog.MortgageInsurance <= 0.0)
      {
        str2 = "";
      }
      else
      {
        num = this.paymentLog.MortgageInsurance;
        str2 = num.ToString("N2");
      }
      mortgageInsurance.Text = str2;
      TextBox boxHazardInsurance = this.textBoxHazardInsurance;
      string str3;
      if (this.paymentLog.HazardInsurance <= 0.0)
      {
        str3 = "";
      }
      else
      {
        num = this.paymentLog.HazardInsurance;
        str3 = num.ToString("N2");
      }
      boxHazardInsurance.Text = str3;
      TextBox boxFloodInsurance = this.textBoxFloodInsurance;
      string str4;
      if (this.paymentLog.FloodInsurance <= 0.0)
      {
        str4 = "";
      }
      else
      {
        num = this.paymentLog.FloodInsurance;
        str4 = num.ToString("N2");
      }
      boxFloodInsurance.Text = str4;
      TextBox boxCityPropertyTax = this.textBoxCityPropertyTax;
      string str5;
      if (this.paymentLog.CityPropertyTax <= 0.0)
      {
        str5 = "";
      }
      else
      {
        num = this.paymentLog.CityPropertyTax;
        str5 = num.ToString("N2");
      }
      boxCityPropertyTax.Text = str5;
      TextBox textBoxOther1Escrow = this.textBoxOther1Escrow;
      string str6;
      if (this.paymentLog.Other1Escrow <= 0.0)
      {
        str6 = "";
      }
      else
      {
        num = this.paymentLog.Other1Escrow;
        str6 = num.ToString("N2");
      }
      textBoxOther1Escrow.Text = str6;
      TextBox textBoxOther2Escrow = this.textBoxOther2Escrow;
      string str7;
      if (this.paymentLog.Other2Escrow <= 0.0)
      {
        str7 = "";
      }
      else
      {
        num = this.paymentLog.Other2Escrow;
        str7 = num.ToString("N2");
      }
      textBoxOther2Escrow.Text = str7;
      TextBox textBoxOther3Escrow = this.textBoxOther3Escrow;
      string str8;
      if (this.paymentLog.Other3Escrow <= 0.0)
      {
        str8 = "";
      }
      else
      {
        num = this.paymentLog.Other3Escrow;
        str8 = num.ToString("N2");
      }
      textBoxOther3Escrow.Text = str8;
      TextBox usdaMonthlyPremium = this.textBoxUSDAMonthlyPremium;
      string str9;
      if (this.paymentLog.USDAMonthlyPremium <= 0.0)
      {
        str9 = "";
      }
      else
      {
        num = this.paymentLog.USDAMonthlyPremium;
        str9 = num.ToString("N2");
      }
      usdaMonthlyPremium.Text = str9;
      if (this.paymentLog.BuydownSubsidyAmount > 0.0)
      {
        if (val1 >= this.paymentLog.BuydownSubsidyAmount)
          val1 -= this.paymentLog.BuydownSubsidyAmount;
        else
          val1 = 0.0;
      }
      if (this.paymentLog.Interest > 0.0)
      {
        if (val1 >= this.paymentLog.Interest)
        {
          if (!checkOnly && this.paymentLog.Interest != 0.0)
          {
            TextBox textBoxInterest = this.textBoxInterest;
            num = this.paymentLog.Interest;
            string str10 = num.ToString("N2");
            textBoxInterest.Text = str10;
          }
          val1 -= this.paymentLog.Interest;
        }
        else
        {
          if (!checkOnly && val1 != 0.0)
            this.textBoxInterest.Text = val1.ToString("N2");
          val1 = 0.0;
        }
      }
      if (this.loan.GetField("HUD24") == "0")
        this.paymentLog.Escrow = 0.0;
      if (this.paymentLog.Escrow > 0.0)
      {
        if (val1 >= this.paymentLog.Escrow)
        {
          if (!checkOnly && this.paymentLog.Escrow != 0.0)
          {
            TextBox textBoxEscrow = this.textBoxEscrow;
            num = this.paymentLog.Escrow;
            string str11 = num.ToString("N2");
            textBoxEscrow.Text = str11;
          }
          val1 -= this.paymentLog.Escrow;
        }
        else
        {
          if (!checkOnly && val1 != 0.0)
            this.textBoxEscrow.Text = val1.ToString("N2");
          val1 = 0.0;
          if (val1 == 0.0)
          {
            this.textBoxTax.Text = this.textBoxMortgageInsurance.Text = this.textBoxHazardInsurance.Text = this.textBoxFloodInsurance.Text = this.textBoxCityPropertyTax.Text = "";
            this.textBoxOther1Escrow.Text = this.textBoxOther2Escrow.Text = this.textBoxOther3Escrow.Text = this.textBoxUSDAMonthlyPremium.Text = "";
            this.textBoxEscrow.Text = "";
          }
        }
      }
      if (this.paymentLog.MiscFee > 0.0)
      {
        if (val1 >= this.paymentLog.MiscFee)
        {
          if (!checkOnly && this.paymentLog.MiscFee != 0.0)
          {
            TextBox textBoxMiscFee = this.textBoxMiscFee;
            num = this.paymentLog.MiscFee;
            string str12 = num.ToString("N2");
            textBoxMiscFee.Text = str12;
          }
          val1 -= this.paymentLog.MiscFee;
        }
        else
        {
          if (!checkOnly && val1 != 0.0)
            this.textBoxMiscFee.Text = val1.ToString("N2");
          val1 = 0.0;
        }
      }
      if (this.paymentLog.Principal > 0.0)
      {
        if (val1 >= this.paymentLog.Principal)
        {
          if (!checkOnly && this.paymentLog.Principal != 0.0)
          {
            TextBox textBoxPrincipal = this.textBoxPrincipal;
            num = this.paymentLog.Principal;
            string str13 = num.ToString("N2");
            textBoxPrincipal.Text = str13;
          }
          val1 -= this.paymentLog.Principal;
        }
        else
        {
          if (!checkOnly && val1 != 0.0)
            this.textBoxPrincipal.Text = val1.ToString("N2");
          val1 = 0.0;
        }
      }
      if (this.paymentLog.LateFee > 0.0)
      {
        if (val1 >= this.paymentLog.LateFee)
        {
          if (!checkOnly && this.paymentLog.LateFee != 0.0)
          {
            TextBox textBoxLateFee = this.textBoxLateFee;
            num = this.paymentLog.LateFee;
            string str14 = num.ToString("N2");
            textBoxLateFee.Text = str14;
          }
          val1 -= this.paymentLog.LateFee;
        }
        else
        {
          if (!checkOnly && val1 != 0.0)
            this.textBoxLateFee.Text = val1.ToString("N2");
          val1 = 0.0;
        }
      }
      if (!checkOnly)
      {
        if (Utils.ArithmeticRounding(val1, 2) > 0.0)
        {
          TextBox textBoxAddPrincipal = this.textBoxAddPrincipal;
          num = Utils.ArithmeticRounding(val1, 2);
          string str15 = num.ToString("N2");
          textBoxAddPrincipal.Text = str15;
        }
        else
          this.textBoxAddPrincipal.Text = "";
      }
      else
      {
        double val2 = Utils.ParseDouble((object) this.textBoxAddPrincipal.Text) + Utils.ParseDouble((object) this.textBoxAddEscrow.Text);
        if (Utils.ArithmeticRounding(val1, 2) != Utils.ArithmeticRounding(val2, 2))
          return false;
      }
      return true;
    }

    private void textBoxStatementDate_ValueChanged(object sender, EventArgs e)
    {
      if (Utils.ParseDate((object) this.textBoxStatementDate.Text) > Utils.ParseDate((object) this.textBoxPaymentDueDate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Statement Date cannot be greater than the Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxStatementDate.ValueChanged -= new EventHandler(this.textBoxStatementDate_ValueChanged);
        this.textBoxStatementDate.Text = this.preStatementDate;
        this.textBoxStatementDate.ValueChanged += new EventHandler(this.textBoxStatementDate_ValueChanged);
        this.textBoxStatementDate.Focus();
      }
      this.preStatementDate = this.textBoxStatementDate.Text;
    }

    private void textBoxPaymentDueDate_ValueChanged(object sender, EventArgs e)
    {
      if (Utils.ParseDate((object) this.textBoxPaymentDueDate.Text) > Utils.ParseDate((object) this.textBoxLatePaymentDate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Payment Due Date cannot be greater than Late Payment Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxPaymentDueDate.ValueChanged -= new EventHandler(this.textBoxPaymentDueDate_ValueChanged);
        this.textBoxPaymentDueDate.Text = this.prePaymentDueDate;
        this.textBoxPaymentDueDate.ValueChanged += new EventHandler(this.textBoxPaymentDueDate_ValueChanged);
        this.textBoxPaymentDueDate.Focus();
      }
      else if (Utils.ParseDate((object) this.textBoxPaymentDueDate.Text) < Utils.ParseDate((object) this.textBoxStatementDate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Payment Due Date cannot be earlier than the Statement Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxPaymentDueDate.ValueChanged -= new EventHandler(this.textBoxPaymentDueDate_ValueChanged);
        this.textBoxPaymentDueDate.Text = this.prePaymentDueDate;
        this.textBoxPaymentDueDate.ValueChanged += new EventHandler(this.textBoxPaymentDueDate_ValueChanged);
        this.textBoxPaymentDueDate.Focus();
      }
      else
        this.prePaymentDueDate = this.textBoxPaymentDueDate.Text;
    }

    private void textBoxLatePaymentDate_ValueChanged(object sender, EventArgs e)
    {
      if (Utils.ParseDate((object) this.textBoxLatePaymentDate.Text) < Utils.ParseDate((object) this.textBoxPaymentDueDate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Late Payment Date cannot be earlier than the Payment Due Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxLatePaymentDate.ValueChanged -= new EventHandler(this.textBoxLatePaymentDate_ValueChanged);
        this.textBoxLatePaymentDate.Text = this.preLatePaymentDate;
        this.textBoxLatePaymentDate.ValueChanged += new EventHandler(this.textBoxLatePaymentDate_ValueChanged);
        this.textBoxLatePaymentDate.Focus();
      }
      else
        this.preLatePaymentDate = this.textBoxLatePaymentDate.Text;
    }

    internal void doEscrowSum()
    {
      this.textBoxEscrow.Text = Math.Round((this.textBoxTax.Text != "" ? Utils.ParseDouble((object) this.textBoxTax.Text) : 0.0) + (this.textBoxMortgageInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxMortgageInsurance.Text) : 0.0) + (this.textBoxFloodInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxFloodInsurance.Text) : 0.0) + (this.textBoxHazardInsurance.Text != "" ? Utils.ParseDouble((object) this.textBoxHazardInsurance.Text) : 0.0) + (this.textBoxCityPropertyTax.Text != "" ? Utils.ParseDouble((object) this.textBoxCityPropertyTax.Text) : 0.0) + (this.textBoxOther1Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther1Escrow.Text) : 0.0) + (this.textBoxOther2Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther2Escrow.Text) : 0.0) + (this.textBoxOther3Escrow.Text != "" ? Utils.ParseDouble((object) this.textBoxOther3Escrow.Text) : 0.0) + (this.textBoxUSDAMonthlyPremium.Text != "" ? Utils.ParseDouble((object) this.textBoxUSDAMonthlyPremium.Text) : 0.0), 2).ToString("N2");
      if (this.textBoxTax.Text == "0")
        this.textBoxTax.Text = "";
      if (this.textBoxMortgageInsurance.Text == "0")
        this.textBoxMortgageInsurance.Text = "";
      if (this.textBoxFloodInsurance.Text == "0")
        this.textBoxFloodInsurance.Text = "";
      if (this.textBoxHazardInsurance.Text == "0")
        this.textBoxHazardInsurance.Text = "";
      if (this.textBoxCityPropertyTax.Text == "0")
        this.textBoxCityPropertyTax.Text = "";
      if (this.textBoxOther1Escrow.Text == "0")
        this.textBoxOther1Escrow.Text = "";
      if (this.textBoxOther2Escrow.Text == "0")
        this.textBoxOther2Escrow.Text = "";
      if (this.textBoxOther3Escrow.Text == "0")
        this.textBoxOther3Escrow.Text = "";
      if (!(this.textBoxUSDAMonthlyPremium.Text == "0"))
        return;
      this.textBoxUSDAMonthlyPremium.Text = "";
    }

    private void textBoxEscrowItem_TextChanged(object sender, EventArgs e) => this.doEscrowSum();

    private void textBoxAddItem_TextChanged(object sender, EventArgs e) => this.doCalculation();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnSave = new Button();
      this.label1 = new Label();
      this.boxNo = new TextBox();
      this.cboPaymentType = new ComboBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label11 = new Label();
      this.label12 = new Label();
      this.label13 = new Label();
      this.label16 = new Label();
      this.label17 = new Label();
      this.label18 = new Label();
      this.label19 = new Label();
      this.textBoxIndexRate = new TextBox();
      this.textBoxInterestRate = new TextBox();
      this.textBoxTotalAmount = new TextBox();
      this.textBoxPrincipal = new TextBox();
      this.textBoxInterest = new TextBox();
      this.textBoxEscrow = new TextBox();
      this.textBoxLateFee = new TextBox();
      this.textBoxMiscFee = new TextBox();
      this.textBoxAddPrincipal = new TextBox();
      this.textBoxAddEscrow = new TextBox();
      this.btnCancel = new Button();
      this.label20 = new Label();
      this.textBox20 = new TextBox();
      this.textBox21 = new TextBox();
      this.label21 = new Label();
      this.label22 = new Label();
      this.panelDetail = new Panel();
      this.labelCreatedBy = new Label();
      this.label24 = new Label();
      this.label25 = new Label();
      this.label26 = new Label();
      this.label27 = new Label();
      this.label28 = new Label();
      this.label30 = new Label();
      this.label31 = new Label();
      this.label32 = new Label();
      this.label33 = new Label();
      this.labelModifiedBy = new Label();
      this.label36 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.label34 = new Label();
      this.textBoxAmountDue = new TextBox();
      this.label14 = new Label();
      this.label35 = new Label();
      this.textBoxDiff = new TextBox();
      this.label29 = new Label();
      this.label23 = new Label();
      this.textBoxAllocated = new TextBox();
      this.label15 = new Label();
      this.textBoxStatementDate = new DatePicker();
      this.textBoxPaymentDueDate = new DatePicker();
      this.textBoxLatePaymentDate = new DatePicker();
      this.textBoxReceivedDate = new DatePicker();
      this.textBoxDepositedDate = new DatePicker();
      this.groupContainer1 = new GroupContainer();
      this.textBoxOther3Escrow = new TextBox();
      this.label53 = new Label();
      this.label52 = new Label();
      this.textBoxOther2Escrow = new TextBox();
      this.label50 = new Label();
      this.label51 = new Label();
      this.textBoxMortgageInsurance = new TextBox();
      this.label48 = new Label();
      this.label49 = new Label();
      this.textBoxUSDAMonthlyPremium = new TextBox();
      this.label46 = new Label();
      this.label47 = new Label();
      this.textBoxOther1Escrow = new TextBox();
      this.label44 = new Label();
      this.label45 = new Label();
      this.textBoxCityPropertyTax = new TextBox();
      this.label42 = new Label();
      this.label43 = new Label();
      this.textBoxFloodInsurance = new TextBox();
      this.label40 = new Label();
      this.label41 = new Label();
      this.textBoxHazardInsurance = new TextBox();
      this.label38 = new Label();
      this.label39 = new Label();
      this.textBoxTax = new TextBox();
      this.label10 = new Label();
      this.label37 = new Label();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.Location = new Point(423, 660);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 22;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 42);
      this.label1.Name = "label1";
      this.label1.Size = new Size(58, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Payment #";
      this.boxNo.Location = new Point(166, 36);
      this.boxNo.Name = "boxNo";
      this.boxNo.ReadOnly = true;
      this.boxNo.Size = new Size(133, 20);
      this.boxNo.TabIndex = 1;
      this.boxNo.TabStop = false;
      this.boxNo.Tag = (object) "PaymentNo";
      this.boxNo.TextAlign = HorizontalAlignment.Right;
      this.cboPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPaymentType.FormattingEnabled = true;
      this.cboPaymentType.Location = new Point(409, 36);
      this.cboPaymentType.Name = "cboPaymentType";
      this.cboPaymentType.Size = new Size(171, 21);
      this.cboPaymentType.TabIndex = 18;
      this.cboPaymentType.Tag = (object) "PaymentMethod";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 63);
      this.label2.Name = "label2";
      this.label2.Size = new Size(81, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Statement Date";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 84);
      this.label3.Name = "label3";
      this.label3.Size = new Size(97, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Payment Due Date";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 105);
      this.label4.Name = "label4";
      this.label4.Size = new Size(98, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Late Payment Date";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 128);
      this.label5.Name = "label5";
      this.label5.Size = new Size(123, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Payment Received Date";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 149);
      this.label6.Name = "label6";
      this.label6.Size = new Size(125, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "Payment Deposited Date";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 170);
      this.label7.Name = "label7";
      this.label7.Size = new Size(59, 13);
      this.label7.TabIndex = 9;
      this.label7.Text = "Index Rate";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 191);
      this.label8.Name = "label8";
      this.label8.Size = new Size(68, 13);
      this.label8.TabIndex = 10;
      this.label8.Text = "Interest Rate";
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(10, 237);
      this.label9.Name = "label9";
      this.label9.Size = new Size(140, 13);
      this.label9.TabIndex = 11;
      this.label9.Text = "Total Amount Received";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(10, 259);
      this.label11.Name = "label11";
      this.label11.Size = new Size(47, 13);
      this.label11.TabIndex = 13;
      this.label11.Text = "Principal";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(10, 280);
      this.label12.Name = "label12";
      this.label12.Size = new Size(42, 13);
      this.label12.TabIndex = 14;
      this.label12.Text = "Interest";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(10, 304);
      this.label13.Name = "label13";
      this.label13.Size = new Size(42, 13);
      this.label13.TabIndex = 15;
      this.label13.Text = "Escrow";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(10, 533);
      this.label16.Name = "label16";
      this.label16.Size = new Size(49, 13);
      this.label16.TabIndex = 18;
      this.label16.Text = "Late Fee";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(10, 554);
      this.label17.Name = "label17";
      this.label17.Size = new Size(53, 13);
      this.label17.TabIndex = 19;
      this.label17.Text = "Misc. Fee";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(10, 577);
      this.label18.Name = "label18";
      this.label18.Size = new Size(96, 13);
      this.label18.TabIndex = 20;
      this.label18.Text = "Additional Principal";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(10, 598);
      this.label19.Name = "label19";
      this.label19.Size = new Size(91, 13);
      this.label19.TabIndex = 21;
      this.label19.Text = "Additional Escrow";
      this.textBoxIndexRate.Location = new Point(166, 168);
      this.textBoxIndexRate.Name = "textBoxIndexRate";
      this.textBoxIndexRate.ReadOnly = true;
      this.textBoxIndexRate.Size = new Size(64, 20);
      this.textBoxIndexRate.TabIndex = 7;
      this.textBoxIndexRate.TabStop = false;
      this.textBoxIndexRate.Tag = (object) "IndexRate";
      this.textBoxIndexRate.TextAlign = HorizontalAlignment.Right;
      this.textBoxInterestRate.Location = new Point(166, 190);
      this.textBoxInterestRate.Name = "textBoxInterestRate";
      this.textBoxInterestRate.ReadOnly = true;
      this.textBoxInterestRate.Size = new Size(64, 20);
      this.textBoxInterestRate.TabIndex = 8;
      this.textBoxInterestRate.TabStop = false;
      this.textBoxInterestRate.Tag = (object) "InterestRate";
      this.textBoxInterestRate.TextAlign = HorizontalAlignment.Right;
      this.textBoxTotalAmount.Location = new Point(166, 234);
      this.textBoxTotalAmount.Name = "textBoxTotalAmount";
      this.textBoxTotalAmount.Size = new Size(133, 20);
      this.textBoxTotalAmount.TabIndex = 10;
      this.textBoxTotalAmount.Tag = (object) "TotalAmountReceived";
      this.textBoxTotalAmount.TextAlign = HorizontalAlignment.Right;
      this.textBoxTotalAmount.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxPrincipal.Location = new Point(166, 256);
      this.textBoxPrincipal.Name = "textBoxPrincipal";
      this.textBoxPrincipal.Size = new Size(133, 20);
      this.textBoxPrincipal.TabIndex = 11;
      this.textBoxPrincipal.Tag = (object) "Principal";
      this.textBoxPrincipal.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrincipal.TextChanged += new EventHandler(this.textBoxAddItem_TextChanged);
      this.textBoxPrincipal.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxPrincipal.Leave += new EventHandler(this.decimal_FieldLeave);
      this.textBoxInterest.Location = new Point(166, 278);
      this.textBoxInterest.Name = "textBoxInterest";
      this.textBoxInterest.Size = new Size(133, 20);
      this.textBoxInterest.TabIndex = 12;
      this.textBoxInterest.Tag = (object) "Interest";
      this.textBoxInterest.TextAlign = HorizontalAlignment.Right;
      this.textBoxInterest.TextChanged += new EventHandler(this.textBoxAddItem_TextChanged);
      this.textBoxInterest.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxInterest.Leave += new EventHandler(this.decimal_FieldLeave);
      this.textBoxEscrow.Location = new Point(166, 300);
      this.textBoxEscrow.Name = "textBoxEscrow";
      this.textBoxEscrow.Size = new Size(133, 20);
      this.textBoxEscrow.TabIndex = 13;
      this.textBoxEscrow.Tag = (object) "Escrow";
      this.textBoxEscrow.TextAlign = HorizontalAlignment.Right;
      this.textBoxEscrow.TextChanged += new EventHandler(this.textBoxAddItem_TextChanged);
      this.textBoxEscrow.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxEscrow.Leave += new EventHandler(this.decimal_FieldLeave);
      this.textBoxLateFee.Location = new Point(166, 529);
      this.textBoxLateFee.Name = "textBoxLateFee";
      this.textBoxLateFee.Size = new Size(133, 20);
      this.textBoxLateFee.TabIndex = 14;
      this.textBoxLateFee.Tag = (object) "LateFee";
      this.textBoxLateFee.TextAlign = HorizontalAlignment.Right;
      this.textBoxLateFee.TextChanged += new EventHandler(this.textBoxAddItem_TextChanged);
      this.textBoxLateFee.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxLateFee.Leave += new EventHandler(this.decimal_FieldLeave);
      this.textBoxMiscFee.Location = new Point(166, 551);
      this.textBoxMiscFee.Name = "textBoxMiscFee";
      this.textBoxMiscFee.Size = new Size(133, 20);
      this.textBoxMiscFee.TabIndex = 15;
      this.textBoxMiscFee.Tag = (object) "MiscFee";
      this.textBoxMiscFee.TextAlign = HorizontalAlignment.Right;
      this.textBoxMiscFee.TextChanged += new EventHandler(this.textBoxAddItem_TextChanged);
      this.textBoxMiscFee.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxMiscFee.Leave += new EventHandler(this.decimal_FieldLeave);
      this.textBoxAddPrincipal.Location = new Point(166, 573);
      this.textBoxAddPrincipal.Name = "textBoxAddPrincipal";
      this.textBoxAddPrincipal.Size = new Size(133, 20);
      this.textBoxAddPrincipal.TabIndex = 16;
      this.textBoxAddPrincipal.Tag = (object) "AdditionalPrincipal";
      this.textBoxAddPrincipal.TextAlign = HorizontalAlignment.Right;
      this.textBoxAddPrincipal.TextChanged += new EventHandler(this.textBoxAddItem_TextChanged);
      this.textBoxAddPrincipal.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxAddPrincipal.Leave += new EventHandler(this.decimal_FieldLeave);
      this.textBoxAddEscrow.Location = new Point(166, 595);
      this.textBoxAddEscrow.Name = "textBoxAddEscrow";
      this.textBoxAddEscrow.Size = new Size(133, 20);
      this.textBoxAddEscrow.TabIndex = 17;
      this.textBoxAddEscrow.Tag = (object) "AdditionalEscrow";
      this.textBoxAddEscrow.TextAlign = HorizontalAlignment.Right;
      this.textBoxAddEscrow.TextChanged += new EventHandler(this.textBoxAddItem_TextChanged);
      this.textBoxAddEscrow.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxAddEscrow.Leave += new EventHandler(this.decimal_FieldLeave);
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(503, 660);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 23;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label20.AutoSize = true;
      this.label20.Location = new Point(309, 42);
      this.label20.Name = "label20";
      this.label20.Size = new Size(87, 13);
      this.label20.TabIndex = 41;
      this.label20.Text = "Payment Method";
      this.textBox20.Location = new Point(312, 259);
      this.textBox20.Multiline = true;
      this.textBox20.Name = "textBox20";
      this.textBox20.ScrollBars = ScrollBars.Vertical;
      this.textBox20.Size = new Size(268, 396);
      this.textBox20.TabIndex = 21;
      this.textBox20.Tag = (object) "Comments";
      this.textBox21.Location = new Point(409, 59);
      this.textBox21.Name = "textBox21";
      this.textBox21.Size = new Size(171, 20);
      this.textBox21.TabIndex = 19;
      this.textBox21.Tag = (object) "InstitutionName";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(309, 63);
      this.label21.Name = "label21";
      this.label21.Size = new Size(83, 13);
      this.label21.TabIndex = 44;
      this.label21.Text = "Institution Name";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(309, 244);
      this.label22.Name = "label22";
      this.label22.Size = new Size(56, 13);
      this.label22.TabIndex = 45;
      this.label22.Text = "Comments";
      this.panelDetail.Location = new Point(312, 81);
      this.panelDetail.Name = "panelDetail";
      this.panelDetail.Size = new Size(268, 156);
      this.panelDetail.TabIndex = 20;
      this.labelCreatedBy.AutoSize = true;
      this.labelCreatedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelCreatedBy.ForeColor = SystemColors.ControlText;
      this.labelCreatedBy.Location = new Point(10, 662);
      this.labelCreatedBy.Name = "labelCreatedBy";
      this.labelCreatedBy.Size = new Size(62, 13);
      this.labelCreatedBy.TabIndex = 1;
      this.labelCreatedBy.Text = "(CreatedBy)";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(230, 173);
      this.label24.Name = "label24";
      this.label24.Size = new Size(15, 13);
      this.label24.TabIndex = 64;
      this.label24.Text = "%";
      this.label25.AutoSize = true;
      this.label25.Location = new Point(230, 193);
      this.label25.Name = "label25";
      this.label25.Size = new Size(15, 13);
      this.label25.TabIndex = 65;
      this.label25.Text = "%";
      this.label26.AutoSize = true;
      this.label26.Location = new Point(151, 259);
      this.label26.Name = "label26";
      this.label26.Size = new Size(13, 13);
      this.label26.TabIndex = 66;
      this.label26.Text = "$";
      this.label27.AutoSize = true;
      this.label27.Location = new Point(151, 280);
      this.label27.Name = "label27";
      this.label27.Size = new Size(13, 13);
      this.label27.TabIndex = 67;
      this.label27.Text = "$";
      this.label28.AutoSize = true;
      this.label28.Location = new Point(151, 304);
      this.label28.Name = "label28";
      this.label28.Size = new Size(13, 13);
      this.label28.TabIndex = 68;
      this.label28.Text = "$";
      this.label30.AutoSize = true;
      this.label30.Location = new Point(151, 534);
      this.label30.Name = "label30";
      this.label30.Size = new Size(13, 13);
      this.label30.TabIndex = 70;
      this.label30.Text = "$";
      this.label31.AutoSize = true;
      this.label31.Location = new Point(151, 554);
      this.label31.Name = "label31";
      this.label31.Size = new Size(13, 13);
      this.label31.TabIndex = 71;
      this.label31.Text = "$";
      this.label32.AutoSize = true;
      this.label32.Location = new Point(151, 577);
      this.label32.Name = "label32";
      this.label32.Size = new Size(13, 13);
      this.label32.TabIndex = 72;
      this.label32.Text = "$";
      this.label33.AutoSize = true;
      this.label33.Location = new Point(151, 598);
      this.label33.Name = "label33";
      this.label33.Size = new Size(13, 13);
      this.label33.TabIndex = 73;
      this.label33.Text = "$";
      this.labelModifiedBy.AutoSize = true;
      this.labelModifiedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelModifiedBy.ForeColor = SystemColors.ControlText;
      this.labelModifiedBy.Location = new Point(10, 682);
      this.labelModifiedBy.Name = "labelModifiedBy";
      this.labelModifiedBy.Size = new Size(65, 13);
      this.labelModifiedBy.TabIndex = 75;
      this.labelModifiedBy.Text = "(ModifiedBy)";
      this.label36.AutoSize = true;
      this.label36.Location = new Point(151, 238);
      this.label36.Name = "label36";
      this.label36.Size = new Size(13, 13);
      this.label36.TabIndex = 86;
      this.label36.Text = "$";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Interum Escrow Disbursement";
      this.emHelpLink1.Location = new Point(15, 705);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 113;
      this.label34.AutoSize = true;
      this.label34.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label34.Location = new Point(10, 216);
      this.label34.Name = "label34";
      this.label34.Size = new Size(109, 13);
      this.label34.TabIndex = 85;
      this.label34.Text = "Total Amount Due";
      this.textBoxAmountDue.Location = new Point(166, 212);
      this.textBoxAmountDue.Name = "textBoxAmountDue";
      this.textBoxAmountDue.ReadOnly = true;
      this.textBoxAmountDue.Size = new Size(133, 20);
      this.textBoxAmountDue.TabIndex = 9;
      this.textBoxAmountDue.Tag = (object) "TotalAmountDue";
      this.textBoxAmountDue.TextAlign = HorizontalAlignment.Right;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(151, 216);
      this.label14.Name = "label14";
      this.label14.Size = new Size(13, 13);
      this.label14.TabIndex = 83;
      this.label14.Text = "$";
      this.label35.AutoSize = true;
      this.label35.Location = new Point(151, 642);
      this.label35.Name = "label35";
      this.label35.Size = new Size(13, 13);
      this.label35.TabIndex = 81;
      this.label35.Text = "$";
      this.textBoxDiff.Location = new Point(166, 639);
      this.textBoxDiff.Name = "textBoxDiff";
      this.textBoxDiff.ReadOnly = true;
      this.textBoxDiff.Size = new Size(133, 20);
      this.textBoxDiff.TabIndex = 80;
      this.textBoxDiff.TabStop = false;
      this.textBoxDiff.Tag = (object) "";
      this.textBoxDiff.TextAlign = HorizontalAlignment.Right;
      this.label29.AutoSize = true;
      this.label29.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label29.Location = new Point(10, 642);
      this.label29.Name = "label29";
      this.label29.Size = new Size(66, 13);
      this.label29.TabIndex = 79;
      this.label29.Text = "Difference";
      this.label23.AutoSize = true;
      this.label23.Location = new Point(151, 620);
      this.label23.Name = "label23";
      this.label23.Size = new Size(13, 13);
      this.label23.TabIndex = 78;
      this.label23.Text = "$";
      this.textBoxAllocated.Location = new Point(166, 617);
      this.textBoxAllocated.Name = "textBoxAllocated";
      this.textBoxAllocated.ReadOnly = true;
      this.textBoxAllocated.Size = new Size(133, 20);
      this.textBoxAllocated.TabIndex = 77;
      this.textBoxAllocated.TabStop = false;
      this.textBoxAllocated.Tag = (object) "";
      this.textBoxAllocated.TextAlign = HorizontalAlignment.Right;
      this.label15.AutoSize = true;
      this.label15.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label15.Location = new Point(10, 620);
      this.label15.Name = "label15";
      this.label15.Size = new Size(139, 13);
      this.label15.TabIndex = 76;
      this.label15.Text = "Total Amount Allocated";
      this.textBoxStatementDate.BackColor = SystemColors.Window;
      this.textBoxStatementDate.Location = new Point(166, 58);
      this.textBoxStatementDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.textBoxStatementDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.textBoxStatementDate.Name = "textBoxStatementDate";
      this.textBoxStatementDate.Size = new Size(133, 21);
      this.textBoxStatementDate.TabIndex = 2;
      this.textBoxStatementDate.Tag = (object) "StatementDate";
      this.textBoxStatementDate.ToolTip = "";
      this.textBoxStatementDate.Value = new DateTime(0L);
      this.textBoxStatementDate.ValueChanged += new EventHandler(this.textBoxStatementDate_ValueChanged);
      this.textBoxPaymentDueDate.BackColor = SystemColors.Window;
      this.textBoxPaymentDueDate.Location = new Point(166, 80);
      this.textBoxPaymentDueDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.textBoxPaymentDueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.textBoxPaymentDueDate.Name = "textBoxPaymentDueDate";
      this.textBoxPaymentDueDate.Size = new Size(133, 21);
      this.textBoxPaymentDueDate.TabIndex = 3;
      this.textBoxPaymentDueDate.Tag = (object) "PaymentDueDate";
      this.textBoxPaymentDueDate.ToolTip = "";
      this.textBoxPaymentDueDate.Value = new DateTime(0L);
      this.textBoxPaymentDueDate.ValueChanged += new EventHandler(this.textBoxPaymentDueDate_ValueChanged);
      this.textBoxLatePaymentDate.BackColor = SystemColors.Window;
      this.textBoxLatePaymentDate.Location = new Point(166, 102);
      this.textBoxLatePaymentDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.textBoxLatePaymentDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.textBoxLatePaymentDate.Name = "textBoxLatePaymentDate";
      this.textBoxLatePaymentDate.Size = new Size(133, 21);
      this.textBoxLatePaymentDate.TabIndex = 4;
      this.textBoxLatePaymentDate.Tag = (object) "LatePaymentDate";
      this.textBoxLatePaymentDate.ToolTip = "";
      this.textBoxLatePaymentDate.Value = new DateTime(0L);
      this.textBoxLatePaymentDate.ValueChanged += new EventHandler(this.textBoxLatePaymentDate_ValueChanged);
      this.textBoxReceivedDate.BackColor = SystemColors.Window;
      this.textBoxReceivedDate.Location = new Point(166, 124);
      this.textBoxReceivedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.textBoxReceivedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.textBoxReceivedDate.Name = "textBoxReceivedDate";
      this.textBoxReceivedDate.Size = new Size(133, 21);
      this.textBoxReceivedDate.TabIndex = 5;
      this.textBoxReceivedDate.Tag = (object) "PaymentReceivedDate";
      this.textBoxReceivedDate.ToolTip = "";
      this.textBoxReceivedDate.Value = new DateTime(0L);
      this.textBoxDepositedDate.BackColor = SystemColors.Window;
      this.textBoxDepositedDate.Location = new Point(166, 146);
      this.textBoxDepositedDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.textBoxDepositedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.textBoxDepositedDate.Name = "textBoxDepositedDate";
      this.textBoxDepositedDate.Size = new Size(133, 21);
      this.textBoxDepositedDate.TabIndex = 6;
      this.textBoxDepositedDate.Tag = (object) "PaymentDepositedDate";
      this.textBoxDepositedDate.ToolTip = "";
      this.textBoxDepositedDate.Value = new DateTime(0L);
      this.groupContainer1.Controls.Add((Control) this.textBoxOther3Escrow);
      this.groupContainer1.Controls.Add((Control) this.label53);
      this.groupContainer1.Controls.Add((Control) this.label52);
      this.groupContainer1.Controls.Add((Control) this.textBoxOther2Escrow);
      this.groupContainer1.Controls.Add((Control) this.label50);
      this.groupContainer1.Controls.Add((Control) this.label51);
      this.groupContainer1.Controls.Add((Control) this.textBoxMortgageInsurance);
      this.groupContainer1.Controls.Add((Control) this.label48);
      this.groupContainer1.Controls.Add((Control) this.label49);
      this.groupContainer1.Controls.Add((Control) this.textBoxUSDAMonthlyPremium);
      this.groupContainer1.Controls.Add((Control) this.label46);
      this.groupContainer1.Controls.Add((Control) this.label47);
      this.groupContainer1.Controls.Add((Control) this.textBoxOther1Escrow);
      this.groupContainer1.Controls.Add((Control) this.label44);
      this.groupContainer1.Controls.Add((Control) this.label45);
      this.groupContainer1.Controls.Add((Control) this.textBoxCityPropertyTax);
      this.groupContainer1.Controls.Add((Control) this.label42);
      this.groupContainer1.Controls.Add((Control) this.label43);
      this.groupContainer1.Controls.Add((Control) this.textBoxFloodInsurance);
      this.groupContainer1.Controls.Add((Control) this.label40);
      this.groupContainer1.Controls.Add((Control) this.label41);
      this.groupContainer1.Controls.Add((Control) this.textBoxHazardInsurance);
      this.groupContainer1.Controls.Add((Control) this.label38);
      this.groupContainer1.Controls.Add((Control) this.label39);
      this.groupContainer1.Controls.Add((Control) this.textBoxTax);
      this.groupContainer1.Controls.Add((Control) this.label10);
      this.groupContainer1.Controls.Add((Control) this.label37);
      this.groupContainer1.Controls.Add((Control) this.textBoxDepositedDate);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.textBoxReceivedDate);
      this.groupContainer1.Controls.Add((Control) this.textBoxPrincipal);
      this.groupContainer1.Controls.Add((Control) this.textBoxLatePaymentDate);
      this.groupContainer1.Controls.Add((Control) this.textBoxTotalAmount);
      this.groupContainer1.Controls.Add((Control) this.textBoxPaymentDueDate);
      this.groupContainer1.Controls.Add((Control) this.textBoxInterest);
      this.groupContainer1.Controls.Add((Control) this.textBoxStatementDate);
      this.groupContainer1.Controls.Add((Control) this.textBoxInterestRate);
      this.groupContainer1.Controls.Add((Control) this.emHelpLink1);
      this.groupContainer1.Controls.Add((Control) this.textBoxEscrow);
      this.groupContainer1.Controls.Add((Control) this.label36);
      this.groupContainer1.Controls.Add((Control) this.textBoxIndexRate);
      this.groupContainer1.Controls.Add((Control) this.label34);
      this.groupContainer1.Controls.Add((Control) this.textBoxLateFee);
      this.groupContainer1.Controls.Add((Control) this.textBoxAmountDue);
      this.groupContainer1.Controls.Add((Control) this.textBoxMiscFee);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.textBoxAddPrincipal);
      this.groupContainer1.Controls.Add((Control) this.label35);
      this.groupContainer1.Controls.Add((Control) this.textBoxAddEscrow);
      this.groupContainer1.Controls.Add((Control) this.textBoxDiff);
      this.groupContainer1.Controls.Add((Control) this.btnCancel);
      this.groupContainer1.Controls.Add((Control) this.label29);
      this.groupContainer1.Controls.Add((Control) this.label19);
      this.groupContainer1.Controls.Add((Control) this.label23);
      this.groupContainer1.Controls.Add((Control) this.label20);
      this.groupContainer1.Controls.Add((Control) this.textBoxAllocated);
      this.groupContainer1.Controls.Add((Control) this.label18);
      this.groupContainer1.Controls.Add((Control) this.label15);
      this.groupContainer1.Controls.Add((Control) this.textBox20);
      this.groupContainer1.Controls.Add((Control) this.label17);
      this.groupContainer1.Controls.Add((Control) this.textBox21);
      this.groupContainer1.Controls.Add((Control) this.labelModifiedBy);
      this.groupContainer1.Controls.Add((Control) this.label16);
      this.groupContainer1.Controls.Add((Control) this.btnSave);
      this.groupContainer1.Controls.Add((Control) this.label21);
      this.groupContainer1.Controls.Add((Control) this.labelCreatedBy);
      this.groupContainer1.Controls.Add((Control) this.label22);
      this.groupContainer1.Controls.Add((Control) this.boxNo);
      this.groupContainer1.Controls.Add((Control) this.label13);
      this.groupContainer1.Controls.Add((Control) this.cboPaymentType);
      this.groupContainer1.Controls.Add((Control) this.panelDetail);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label12);
      this.groupContainer1.Controls.Add((Control) this.label33);
      this.groupContainer1.Controls.Add((Control) this.label11);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label24);
      this.groupContainer1.Controls.Add((Control) this.label32);
      this.groupContainer1.Controls.Add((Control) this.label25);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.label31);
      this.groupContainer1.Controls.Add((Control) this.label26);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.label30);
      this.groupContainer1.Controls.Add((Control) this.label27);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label28);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(592, 726);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "T01 Payment";
      this.textBoxOther3Escrow.Location = new Point(232, 477);
      this.textBoxOther3Escrow.Name = "textBoxOther3Escrow";
      this.textBoxOther3Escrow.Size = new Size(67, 20);
      this.textBoxOther3Escrow.TabIndex = 139;
      this.textBoxOther3Escrow.Tag = (object) "Other3Escrow";
      this.textBoxOther3Escrow.TextAlign = HorizontalAlignment.Right;
      this.textBoxOther3Escrow.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxOther3Escrow.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxOther3Escrow.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label53.AutoSize = true;
      this.label53.Location = new Point(217, 481);
      this.label53.Name = "label53";
      this.label53.Size = new Size(13, 13);
      this.label53.TabIndex = 140;
      this.label53.Text = "$";
      this.label52.AutoSize = true;
      this.label52.Location = new Point(49, 481);
      this.label52.Name = "label52";
      this.label52.Size = new Size(39, 13);
      this.label52.TabIndex = 138;
      this.label52.Text = "Other3";
      this.textBoxOther2Escrow.Location = new Point(232, 456);
      this.textBoxOther2Escrow.Name = "textBoxOther2Escrow";
      this.textBoxOther2Escrow.Size = new Size(67, 20);
      this.textBoxOther2Escrow.TabIndex = 135;
      this.textBoxOther2Escrow.Tag = (object) "Other2Escrow";
      this.textBoxOther2Escrow.TextAlign = HorizontalAlignment.Right;
      this.textBoxOther2Escrow.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxOther2Escrow.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxOther2Escrow.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label50.AutoSize = true;
      this.label50.Location = new Point(49, 460);
      this.label50.Name = "label50";
      this.label50.Size = new Size(39, 13);
      this.label50.TabIndex = 136;
      this.label50.Text = "Other2";
      this.label51.AutoSize = true;
      this.label51.Location = new Point(217, 460);
      this.label51.Name = "label51";
      this.label51.Size = new Size(13, 13);
      this.label51.TabIndex = 137;
      this.label51.Text = "$";
      this.textBoxMortgageInsurance.Location = new Point(232, 372);
      this.textBoxMortgageInsurance.Name = "textBoxMortgageInsurance";
      this.textBoxMortgageInsurance.Size = new Size(67, 20);
      this.textBoxMortgageInsurance.TabIndex = 132;
      this.textBoxMortgageInsurance.Tag = (object) "MortgageInsurance";
      this.textBoxMortgageInsurance.TextAlign = HorizontalAlignment.Right;
      this.textBoxMortgageInsurance.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxMortgageInsurance.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxMortgageInsurance.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label48.AutoSize = true;
      this.label48.Location = new Point(49, 376);
      this.label48.Name = "label48";
      this.label48.Size = new Size(102, 13);
      this.label48.TabIndex = 133;
      this.label48.Text = "Mortgage Insurance";
      this.label49.AutoSize = true;
      this.label49.Location = new Point(217, 376);
      this.label49.Name = "label49";
      this.label49.Size = new Size(13, 13);
      this.label49.TabIndex = 134;
      this.label49.Text = "$";
      this.textBoxUSDAMonthlyPremium.Location = new Point(232, 498);
      this.textBoxUSDAMonthlyPremium.Name = "textBoxUSDAMonthlyPremium";
      this.textBoxUSDAMonthlyPremium.Size = new Size(67, 20);
      this.textBoxUSDAMonthlyPremium.TabIndex = 129;
      this.textBoxUSDAMonthlyPremium.Tag = (object) "USDAMonthlyPremium";
      this.textBoxUSDAMonthlyPremium.TextAlign = HorizontalAlignment.Right;
      this.textBoxUSDAMonthlyPremium.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxUSDAMonthlyPremium.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxUSDAMonthlyPremium.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label46.AutoSize = true;
      this.label46.Location = new Point(49, 502);
      this.label46.Name = "label46";
      this.label46.Size = new Size(120, 13);
      this.label46.TabIndex = 130;
      this.label46.Text = "USDA Monthly Premuim";
      this.label47.AutoSize = true;
      this.label47.Location = new Point(217, 502);
      this.label47.Name = "label47";
      this.label47.Size = new Size(13, 13);
      this.label47.TabIndex = 131;
      this.label47.Text = "$";
      this.textBoxOther1Escrow.Location = new Point(232, 435);
      this.textBoxOther1Escrow.Name = "textBoxOther1Escrow";
      this.textBoxOther1Escrow.Size = new Size(67, 20);
      this.textBoxOther1Escrow.TabIndex = 126;
      this.textBoxOther1Escrow.Tag = (object) "Other1Escrow";
      this.textBoxOther1Escrow.TextAlign = HorizontalAlignment.Right;
      this.textBoxOther1Escrow.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxOther1Escrow.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxOther1Escrow.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label44.AutoSize = true;
      this.label44.Location = new Point(49, 439);
      this.label44.Name = "label44";
      this.label44.Size = new Size(39, 13);
      this.label44.TabIndex = (int) sbyte.MaxValue;
      this.label44.Text = "Other1";
      this.label45.AutoSize = true;
      this.label45.Location = new Point(217, 439);
      this.label45.Name = "label45";
      this.label45.Size = new Size(13, 13);
      this.label45.TabIndex = 128;
      this.label45.Text = "$";
      this.textBoxCityPropertyTax.Location = new Point(232, 414);
      this.textBoxCityPropertyTax.Name = "textBoxCityPropertyTax";
      this.textBoxCityPropertyTax.Size = new Size(67, 20);
      this.textBoxCityPropertyTax.TabIndex = 123;
      this.textBoxCityPropertyTax.Tag = (object) "CityPropertyTax";
      this.textBoxCityPropertyTax.TextAlign = HorizontalAlignment.Right;
      this.textBoxCityPropertyTax.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxCityPropertyTax.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxCityPropertyTax.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label42.AutoSize = true;
      this.label42.Location = new Point(49, 418);
      this.label42.Name = "label42";
      this.label42.Size = new Size(87, 13);
      this.label42.TabIndex = 124;
      this.label42.Text = "City Property Tax";
      this.label43.AutoSize = true;
      this.label43.Location = new Point(217, 418);
      this.label43.Name = "label43";
      this.label43.Size = new Size(13, 13);
      this.label43.TabIndex = 125;
      this.label43.Text = "$";
      this.textBoxFloodInsurance.Location = new Point(232, 393);
      this.textBoxFloodInsurance.Name = "textBoxFloodInsurance";
      this.textBoxFloodInsurance.Size = new Size(67, 20);
      this.textBoxFloodInsurance.TabIndex = 120;
      this.textBoxFloodInsurance.Tag = (object) "FloodInsurance";
      this.textBoxFloodInsurance.TextAlign = HorizontalAlignment.Right;
      this.textBoxFloodInsurance.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxFloodInsurance.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxFloodInsurance.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label40.AutoSize = true;
      this.label40.Location = new Point(49, 397);
      this.label40.Name = "label40";
      this.label40.Size = new Size(83, 13);
      this.label40.TabIndex = 121;
      this.label40.Text = "Flood Insurance";
      this.label41.AutoSize = true;
      this.label41.Location = new Point(217, 397);
      this.label41.Name = "label41";
      this.label41.Size = new Size(13, 13);
      this.label41.TabIndex = 122;
      this.label41.Text = "$";
      this.textBoxHazardInsurance.Location = new Point(232, 351);
      this.textBoxHazardInsurance.Name = "textBoxHazardInsurance";
      this.textBoxHazardInsurance.Size = new Size(67, 20);
      this.textBoxHazardInsurance.TabIndex = 117;
      this.textBoxHazardInsurance.Tag = (object) "HazardInsurance";
      this.textBoxHazardInsurance.TextAlign = HorizontalAlignment.Right;
      this.textBoxHazardInsurance.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxHazardInsurance.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxHazardInsurance.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label38.AutoSize = true;
      this.label38.Location = new Point(49, 355);
      this.label38.Name = "label38";
      this.label38.Size = new Size(91, 13);
      this.label38.TabIndex = 118;
      this.label38.Text = "Hazard Insurance";
      this.label39.AutoSize = true;
      this.label39.Location = new Point(217, 355);
      this.label39.Name = "label39";
      this.label39.Size = new Size(13, 13);
      this.label39.TabIndex = 119;
      this.label39.Text = "$";
      this.textBoxTax.Location = new Point(232, 330);
      this.textBoxTax.Name = "textBoxTax";
      this.textBoxTax.Size = new Size(67, 20);
      this.textBoxTax.TabIndex = 114;
      this.textBoxTax.Tag = (object) "EscrowTaxes";
      this.textBoxTax.TextAlign = HorizontalAlignment.Right;
      this.textBoxTax.TextChanged += new EventHandler(this.textBoxEscrowItem_TextChanged);
      this.textBoxTax.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.textBoxTax.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(49, 334);
      this.label10.Name = "label10";
      this.label10.Size = new Size(36, 13);
      this.label10.TabIndex = 115;
      this.label10.Text = "Taxes";
      this.label37.AutoSize = true;
      this.label37.Location = new Point(217, 334);
      this.label37.Name = "label37";
      this.label37.Size = new Size(13, 13);
      this.label37.TabIndex = 116;
      this.label37.Text = "$";
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(592, 726);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PaymentDetailForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Transaction";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
