// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.EditProjectedPaymentTable
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class EditProjectedPaymentTable : Form, IOnlineHelpTarget, IHelp
  {
    private const string className = "EditProjectedPaymentTable";
    private IHtmlInput inputData;
    private GenericFormInputHandler genericFormInputHandler;
    private Sessions.Session session;
    private string currentValue;
    private bool validationEnabled = true;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private ComboBox cboNoColumns;
    private Label label1;
    private Label label2;
    private Label label3;
    private GroupContainer grpCol1;
    private CheckBox chkRangedYears1;
    private CheckBox chkRangedPayment1;
    private Panel panelAllColumns;
    private GroupContainer grpCol4;
    private Label labelP4Dash;
    private TextBox txtP4Year2;
    private Label labelP4Max;
    private Label labelP4Min;
    private TextBox txtP4MaxTotal;
    private TextBox txtP4MinTotal;
    private TextBox txtP4Escrow;
    private TextBox txtP4MI;
    private CheckBox chkP4Only;
    private TextBox txtP4MaxPI;
    private Label labelP4MaxPI;
    private TextBox txtP4MinPI;
    private Label labelP4MinPI;
    private TextBox txtP4Year1;
    private Label labelP4Year;
    private CheckBox chkRangedYears4;
    private CheckBox chkFinalPayment4;
    private CheckBox chkRangedPayment4;
    private GroupContainer grpCol3;
    private Label labelP3Dash;
    private TextBox txtP3Year2;
    private Label labelP3Max;
    private Label labelP3Min;
    private TextBox txtP3MaxTotal;
    private TextBox txtP3MinTotal;
    private TextBox txtP3Escrow;
    private TextBox txtP3MI;
    private CheckBox chkP3Only;
    private TextBox txtP3MaxPI;
    private Label labelP3MaxPI;
    private TextBox txtP3MinPI;
    private Label labelP3MinPI;
    private TextBox txtP3Year1;
    private Label labelP3Year;
    private CheckBox chkRangedYears3;
    private CheckBox chkFinalPayment3;
    private CheckBox chkRangedPayment3;
    private GroupContainer grpCol2;
    private Label labelP2Dash;
    private TextBox txtP2Year2;
    private Label labelP2Max;
    private Label labelP2Min;
    private TextBox txtP2MaxTotal;
    private TextBox txtP2MinTotal;
    private TextBox txtP2Escrow;
    private TextBox txtP2MI;
    private CheckBox chkP2Only;
    private TextBox txtP2MaxPI;
    private Label labelP2MaxPI;
    private TextBox txtP2MinPI;
    private Label labelP2MinPI;
    private TextBox txtP2Year1;
    private Label labelP2Year;
    private CheckBox chkRangedYears2;
    private CheckBox chkFinalPayment2;
    private CheckBox chkRangedPayment2;
    private Label label9;
    private Label label10;
    private Label label11;
    private TextBox txtEstPayment;
    private Label label12;
    private TextBox txtP1Year2;
    private Label labelP1Max;
    private Label labelP1Min;
    private TextBox txtP1MaxTotal;
    private TextBox txtP1MinTotal;
    private TextBox txtP1Escrow;
    private TextBox txtP1MI;
    private CheckBox chkP1Only;
    private TextBox txtP1MaxPI;
    private Label labelP1MaxPI;
    private TextBox txtP1MinPI;
    private Label labelP1MinPI;
    private Label labelP1Year;
    private GroupContainer grpCol0;
    private ToolTip toolTipField;
    private EMHelpLink emHelpLink1;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;

    public EditProjectedPaymentTable(Sessions.Session session, IHtmlInput inputData)
    {
      this.session = session;
      this.inputData = inputData;
      this.InitializeComponent();
      this.genericFormInputHandler = new GenericFormInputHandler(this.inputData, this.Controls, this.session);
      this.genericFormInputHandler.KeepZero = true;
      int num = Utils.ParseInt((object) this.inputData.GetField("NEWHUD2.XPJTCOLUMNS")) - 1;
      if (num < 0)
        num = 3;
      this.cboNoColumns.SelectedIndex = num;
      this.genericFormInputHandler.SetFieldValuesToForm();
      this.genericFormInputHandler.SetBusinessRules(new ResourceManager(typeof (IncomeOtherForm)));
      this.genericFormInputHandler.SetFieldTip(this.toolTipField);
      for (int index = 0; index < this.genericFormInputHandler.FieldControls.Count; ++index)
        this.genericFormInputHandler.SetFieldEvents(this.genericFormInputHandler.FieldControls[index]);
      this.setValueToControls();
      this.rangedPayment_Checked((object) this.chkRangedPayment1, (EventArgs) null);
      this.rangedPayment_Checked((object) this.chkRangedPayment2, (EventArgs) null);
      this.rangedPayment_Checked((object) this.chkRangedPayment3, (EventArgs) null);
      this.rangedPayment_Checked((object) this.chkRangedPayment4, (EventArgs) null);
      this.rangedYears_Checked((object) this.chkRangedYears1, (EventArgs) null);
      this.rangedYears_Checked((object) this.chkRangedYears2, (EventArgs) null);
      this.rangedYears_Checked((object) this.chkRangedYears3, (EventArgs) null);
      this.rangedYears_Checked((object) this.chkRangedYears4, (EventArgs) null);
      this.genericFormInputHandler.OnKeyUp += new EventHandler(this.calculateFields);
      this.genericFormInputHandler.OnLeave += new EventHandler(this.textField_Leave);
      this.genericFormInputHandler.OnEntered += new EventHandler(this.textField_Enter);
    }

    private void setValueToControls()
    {
      if (this.cboNoColumns.SelectedIndex >= 0)
        this.setColumnValues((TextBox) null, this.txtP1Year2, this.txtP1MinPI, this.txtP1MaxPI, this.chkP1Only, this.txtP1MI, this.txtP1Escrow, this.txtP1MinTotal, this.txtP1MaxTotal, this.chkRangedPayment1, this.chkRangedYears1, (CheckBox) null);
      if (this.cboNoColumns.SelectedIndex >= 1)
        this.setColumnValues(this.txtP2Year1, this.txtP2Year2, this.txtP2MinPI, this.txtP2MaxPI, this.chkP2Only, this.txtP2MI, this.txtP2Escrow, this.txtP2MinTotal, this.txtP2MaxTotal, this.chkRangedPayment2, this.chkRangedYears2, this.cboNoColumns.SelectedIndex == 1 ? this.chkFinalPayment2 : (CheckBox) null);
      if (this.cboNoColumns.SelectedIndex >= 2)
        this.setColumnValues(this.txtP3Year1, this.txtP3Year2, this.txtP3MinPI, this.txtP3MaxPI, this.chkP3Only, this.txtP3MI, this.txtP3Escrow, this.txtP3MinTotal, this.txtP3MaxTotal, this.chkRangedPayment3, this.chkRangedYears3, this.cboNoColumns.SelectedIndex == 2 ? this.chkFinalPayment3 : (CheckBox) null);
      if (this.cboNoColumns.SelectedIndex >= 3)
        this.setColumnValues(this.txtP4Year1, this.txtP4Year2, this.txtP4MinPI, this.txtP4MaxPI, this.chkP4Only, this.txtP4MI, this.txtP4Escrow, this.txtP4MinTotal, this.txtP4MaxTotal, this.chkRangedPayment4, this.chkRangedYears4, this.cboNoColumns.SelectedIndex == 3 ? this.chkFinalPayment4 : (CheckBox) null);
      bool flag = this.inputData.GetField("NEWHUD2.XPJT").EndsWith("Final_Payment");
      this.chkFinalPayment2.Checked = this.cboNoColumns.SelectedIndex == 1 & flag;
      this.chkFinalPayment3.Checked = this.cboNoColumns.SelectedIndex == 2 & flag;
      this.chkFinalPayment4.Checked = this.cboNoColumns.SelectedIndex == 3 & flag;
      this.txtMI_KeyUp((object) null, (KeyEventArgs) null);
      this.txtEscrow_KeyUp((object) null, (KeyEventArgs) null);
    }

    private void setColumnValues(
      TextBox txtYear1,
      TextBox txtYear2,
      TextBox txtMinPI,
      TextBox txtMaxPI,
      CheckBox chkOnly,
      TextBox txtMI,
      TextBox txtEscrow,
      TextBox txtMinTotal,
      TextBox txtMaxTotal,
      CheckBox chkRangedPayment,
      CheckBox chkRangedYears,
      CheckBox chkFinalPayment)
    {
      chkRangedPayment.Checked = Utils.ParseDouble((object) txtMinPI.Text.Trim().Replace(",", "")) != Utils.ParseDouble((object) txtMaxPI.Text.Trim().Replace(",", ""));
      chkRangedYears.Checked = txtYear1 != null ? Utils.ParseInt((object) txtYear1.Text) != Utils.ParseInt((object) txtYear2.Text) : Utils.ParseInt((object) txtYear2.Text) > 1;
      if (chkFinalPayment == null)
        return;
      chkFinalPayment.Checked = this.inputData.GetField("NEWHUD2.XPJT").EndsWith("Final_Payment");
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.chkRangedPayment1.Checked && !this.validateValues((object) this.txtP1MaxPI, (EventArgs) null) || this.cboNoColumns.SelectedIndex > 0 && !this.validateValues((object) this.txtP2MaxPI, (EventArgs) null) || this.cboNoColumns.SelectedIndex > 1 && !this.validateValues((object) this.txtP3MaxPI, (EventArgs) null) || this.cboNoColumns.SelectedIndex > 2 && !this.validateValues((object) this.txtP4MaxPI, (EventArgs) null) || this.chkRangedYears1.Checked && !this.validateValues((object) this.txtP1Year2, (EventArgs) null) || this.cboNoColumns.SelectedIndex > 0 && !this.validateValues((object) this.txtP2Year1, (EventArgs) null) || this.cboNoColumns.SelectedIndex > 1 && !this.validateValues((object) this.txtP3Year1, (EventArgs) null) || this.cboNoColumns.SelectedIndex > 2 && !this.validateValues((object) this.txtP4Year1, (EventArgs) null))
        return;
      int previousYearNumber = this.validYearSequence(0, this.chkRangedYears1, (TextBox) null, this.txtP1Year2, (CheckBox) null);
      if (previousYearNumber <= 0)
        return;
      if (this.cboNoColumns.SelectedIndex > 0)
      {
        previousYearNumber = this.validYearSequence(previousYearNumber, this.chkRangedYears2, this.txtP2Year1, this.txtP2Year2, this.chkFinalPayment2);
        if (previousYearNumber <= 0)
          return;
      }
      if (!this.validMIandEscrow(this.txtP1MI) || !this.validMIandEscrow(this.txtP1Escrow) || this.cboNoColumns.SelectedIndex > 0 && (!this.validMIandEscrow(this.txtP2MI) || !this.validMIandEscrow(this.txtP2Escrow)) || this.cboNoColumns.SelectedIndex > 1 && (!this.validMIandEscrow(this.txtP3MI) || !this.validMIandEscrow(this.txtP3Escrow)) || this.cboNoColumns.SelectedIndex > 2 && (!this.validMIandEscrow(this.txtP4MI) || !this.validMIandEscrow(this.txtP4Escrow)))
        return;
      if (this.cboNoColumns.SelectedIndex > 1)
      {
        previousYearNumber = this.validYearSequence(previousYearNumber, this.chkRangedYears3, this.txtP3Year1, this.txtP3Year2, this.chkFinalPayment3);
        if (previousYearNumber <= 0)
          return;
      }
      if (this.cboNoColumns.SelectedIndex > 2)
      {
        previousYearNumber = this.validYearSequence(previousYearNumber, this.chkRangedYears4, this.txtP4Year1, this.txtP4Year2, this.chkFinalPayment4);
        if (previousYearNumber <= 0)
          return;
      }
      if (previousYearNumber > 1)
      {
        int num1 = this.inputData.GetField("325") != "" ? Utils.ParseInt((object) this.inputData.GetField("325")) : Utils.ParseInt((object) this.inputData.GetField("4"));
        int num2 = (num1 - num1 % 12) / 12 + (num1 % 12 > 0 ? 1 : 0);
        if (previousYearNumber > 1 && previousYearNumber != num2 && this.inputData.GetField("19") != "ConstructionToPermanent")
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The number of years in the loan period must be equivalent to the loan term. Please try again.\r\n(Example: If loan term is 360 months, the loan period is 30 years.)", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          if (this.cboNoColumns.SelectedIndex == 3)
          {
            if (this.chkRangedYears4.Checked)
            {
              this.txtP4Year2.Focus();
              return;
            }
            this.txtP4Year1.Focus();
            return;
          }
          if (this.cboNoColumns.SelectedIndex == 2)
          {
            if (this.chkRangedYears3.Checked)
            {
              this.txtP3Year2.Focus();
              return;
            }
            this.txtP3Year1.Focus();
            return;
          }
          if (this.cboNoColumns.SelectedIndex == 1)
          {
            if (this.chkRangedYears2.Checked)
            {
              this.txtP2Year2.Focus();
              return;
            }
            this.txtP2Year1.Focus();
            return;
          }
          if (this.cboNoColumns.SelectedIndex != 0)
            return;
          this.txtP1Year2.Focus();
          return;
        }
      }
      this.setValueToLoan();
      if (this.inputData is LoanData)
        ((LoanData) this.inputData).Calculator.FormCalculation("PAYMENTTABLE.CUSTOMIZE", (string) null, (string) null);
      this.DialogResult = DialogResult.OK;
    }

    private bool validMIandEscrow(TextBox box)
    {
      if (!(box.Text == ""))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "The " + (box.Name.EndsWith("MI") ? "Mortgage Insurance" : "Estimated Escrow") + " field cannot be blank. Please enter 0 if there is no " + (box.Name.EndsWith("MI") ? "Mortgage Insurance" : "Estimated Escrow") + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      box.Focus();
      return false;
    }

    private int validYearSequence(
      int previousYearNumber,
      CheckBox chkRangedYears,
      TextBox txtYear1,
      TextBox txtYear2,
      CheckBox chkFinalPayment)
    {
      if (chkFinalPayment != null && chkFinalPayment.Checked)
        return 1;
      int num1 = txtYear1 != null ? Utils.ParseInt((object) txtYear1.Text) : 1;
      if (previousYearNumber > 0 && num1 - previousYearNumber != 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Year numbers must be in chronological order. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        txtYear1.Focus();
        return -1;
      }
      return !chkRangedYears.Checked ? num1 : Utils.ParseInt((object) txtYear2.Text);
    }

    private bool validateValues(object sender, EventArgs e)
    {
      if (!this.validationEnabled || !(sender is TextBox))
        return true;
      string text = "";
      TextBox textBox1 = (TextBox) sender;
      TextBox textBox2 = (TextBox) null;
      TextBox textBox3 = (TextBox) null;
      CheckBox checkBox1 = (CheckBox) null;
      if (textBox1.Name.EndsWith("MinPI") || textBox1.Name.EndsWith("MaxPI"))
      {
        if (textBox1.Name.IndexOf("P1") > -1)
        {
          textBox2 = this.txtP1MinPI;
          textBox3 = this.txtP1MaxPI;
          checkBox1 = this.chkRangedPayment1;
        }
        else if (textBox1.Name.IndexOf("P2") > -1)
        {
          textBox2 = this.txtP2MinPI;
          textBox3 = this.txtP2MaxPI;
          checkBox1 = this.chkRangedPayment2;
        }
        else if (textBox1.Name.IndexOf("P3") > -1)
        {
          textBox2 = this.txtP3MinPI;
          textBox3 = this.txtP3MaxPI;
          checkBox1 = this.chkRangedPayment3;
        }
        else if (textBox1.Name.IndexOf("P4") > -1)
        {
          textBox2 = this.txtP4MinPI;
          textBox3 = this.txtP4MaxPI;
          checkBox1 = this.chkRangedPayment4;
        }
        if (e == null)
        {
          if (textBox2.Text == "")
            text = "The minimum payment cannot be blank.";
          else if (checkBox1.Checked && textBox3.Text == "")
            text = "The maximum payment cannot be blank.";
        }
        if (text == "" && checkBox1.Checked && textBox2.Text != "" && (textBox3.Text != "" || e == null) && Utils.ParseDouble((object) textBox2.Text) > Utils.ParseDouble((object) textBox3.Text))
          text = "The minimum payment cannot be greater than the maximum payment.";
      }
      else if (textBox1.Name.EndsWith("Year1") || textBox1.Name.EndsWith("Year2"))
      {
        TextBox textBox4 = (TextBox) null;
        CheckBox checkBox2 = (CheckBox) null;
        CheckBox checkBox3 = (CheckBox) null;
        if (textBox1.Name.IndexOf("P1") > -1)
        {
          textBox2 = (TextBox) null;
          textBox3 = this.txtP1Year2;
          checkBox1 = this.chkRangedYears1;
          textBox4 = (TextBox) null;
          checkBox2 = (CheckBox) null;
          checkBox3 = (CheckBox) null;
        }
        else if (textBox1.Name.IndexOf("P2") > -1)
        {
          textBox2 = this.txtP2Year1;
          textBox3 = this.txtP2Year2;
          checkBox1 = this.chkRangedYears2;
          textBox4 = this.chkRangedYears1.Checked ? this.txtP1Year2 : (TextBox) null;
          checkBox2 = this.chkRangedYears1;
          checkBox3 = this.chkFinalPayment2;
        }
        else if (textBox1.Name.IndexOf("P3") > -1)
        {
          textBox2 = this.txtP3Year1;
          textBox3 = this.txtP3Year2;
          checkBox1 = this.chkRangedYears3;
          checkBox2 = this.chkRangedYears2;
          textBox4 = this.chkRangedYears2.Checked ? this.txtP2Year2 : this.txtP2Year1;
          checkBox3 = this.chkFinalPayment3;
        }
        else if (textBox1.Name.IndexOf("P4") > -1)
        {
          textBox2 = this.txtP4Year1;
          textBox3 = this.txtP4Year2;
          checkBox1 = this.chkRangedYears4;
          checkBox2 = this.chkRangedYears3;
          textBox4 = this.chkRangedYears3.Checked ? this.txtP3Year2 : this.txtP3Year1;
          checkBox3 = this.chkFinalPayment4;
        }
        if (e == null && (checkBox3 == null || !checkBox3.Checked))
        {
          if (textBox2 != null && textBox2.Text == "")
            text = "Year numbers cannot be blank.";
          else if (checkBox1.Checked && textBox3.Text == "")
            text = "Year numbers cannot be blank.";
        }
        if (text == "" && (checkBox3 == null || !checkBox3.Checked))
        {
          if (textBox2 != null && checkBox1.Checked && textBox2.Text != "" && textBox3.Text != "" && Utils.ParseInt((object) textBox2.Text) > Utils.ParseInt((object) textBox3.Text))
            text = "Year numbers must be in chronological order and cannot be used more than once. Please try again.";
          else if (textBox2 != null && textBox2.Text != "" && textBox4 != null && Utils.ParseInt((object) textBox2.Text) <= Utils.ParseInt((object) textBox4.Text))
            text = "Year numbers must be in chronological order and cannot be used more than once. Please try again.";
        }
      }
      if (!(text != ""))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      textBox1.Focus();
      textBox1.SelectionStart = textBox1.Text.Length;
      textBox1.SelectionLength = textBox1.Text.Length;
      return false;
    }

    private void setValueToLoan()
    {
      this.inputData.SetCurrentField("NEWHUD2.XPJTCOLUMNS", string.Concat((object) (this.cboNoColumns.SelectedIndex + 1)));
      this.setLoanValues((TextBox) null, this.txtP1Year2, this.txtP1MinPI, this.txtP1MaxPI, this.chkP1Only, this.txtP1MI, this.txtP1Escrow, this.txtP1MinTotal, this.txtP1MaxTotal, this.chkRangedPayment1, this.chkRangedYears1, (CheckBox) null, false);
      this.setLoanValues(this.txtP2Year1, this.txtP2Year2, this.txtP2MinPI, this.txtP2MaxPI, this.chkP2Only, this.txtP2MI, this.txtP2Escrow, this.txtP2MinTotal, this.txtP2MaxTotal, this.chkRangedPayment2, this.chkRangedYears2, this.cboNoColumns.SelectedIndex == 1 ? this.chkFinalPayment2 : (CheckBox) null, this.cboNoColumns.SelectedIndex < 1);
      this.setLoanValues(this.txtP3Year1, this.txtP3Year2, this.txtP3MinPI, this.txtP3MaxPI, this.chkP3Only, this.txtP3MI, this.txtP3Escrow, this.txtP3MinTotal, this.txtP3MaxTotal, this.chkRangedPayment3, this.chkRangedYears3, this.cboNoColumns.SelectedIndex == 2 ? this.chkFinalPayment3 : (CheckBox) null, this.cboNoColumns.SelectedIndex < 2);
      this.setLoanValues(this.txtP4Year1, this.txtP4Year2, this.txtP4MinPI, this.txtP4MaxPI, this.chkP4Only, this.txtP4MI, this.txtP4Escrow, this.txtP4MinTotal, this.txtP4MaxTotal, this.chkRangedPayment4, this.chkRangedYears4, this.cboNoColumns.SelectedIndex == 3 ? this.chkFinalPayment4 : (CheckBox) null, this.cboNoColumns.SelectedIndex < 3);
      if (this.cboNoColumns.SelectedIndex == 1 && this.chkFinalPayment2.Checked || this.cboNoColumns.SelectedIndex == 2 && this.chkFinalPayment3.Checked || this.cboNoColumns.SelectedIndex == 3 && this.chkFinalPayment4.Checked)
      {
        if (Utils.ParseInt((object) this.inputData.GetField("4")) > 0 && Utils.ParseInt((object) this.inputData.GetField("325")) > 0 && Utils.ParseInt((object) this.inputData.GetField("325")) < Utils.ParseInt((object) this.inputData.GetField("4")))
          this.inputData.SetField("NEWHUD2.XPJT", "Balloon_Final_Payment");
        else
          this.inputData.SetField("NEWHUD2.XPJT", "Final_Payment");
      }
      else
        this.inputData.SetField("NEWHUD2.XPJT", "");
      this.inputData.SetCurrentField("LE1.XD1", this.chkRangedPayment1.Checked ? "Y" : "N");
      this.inputData.SetCurrentField("CD1.XD1", this.chkRangedPayment1.Checked ? "Y" : "N");
      this.inputData.SetCurrentField("LE1.XD2", this.cboNoColumns.SelectedIndex < 1 || !this.chkRangedPayment2.Checked ? "N" : "Y");
      this.inputData.SetCurrentField("CD1.XD2", this.cboNoColumns.SelectedIndex < 1 || !this.chkRangedPayment2.Checked ? "N" : "Y");
      this.inputData.SetCurrentField("LE1.XD3", this.cboNoColumns.SelectedIndex < 2 || !this.chkRangedPayment3.Checked ? "N" : "Y");
      this.inputData.SetCurrentField("CD1.XD3", this.cboNoColumns.SelectedIndex < 2 || !this.chkRangedPayment3.Checked ? "N" : "Y");
      this.inputData.SetCurrentField("LE1.XD4", this.cboNoColumns.SelectedIndex < 3 || !this.chkRangedPayment4.Checked ? "N" : "Y");
      this.inputData.SetCurrentField("CD1.XD4", this.cboNoColumns.SelectedIndex < 3 || !this.chkRangedPayment4.Checked ? "N" : "Y");
    }

    private void setLoanValues(
      TextBox txtYear1,
      TextBox txtYear2,
      TextBox txtMinPI,
      TextBox txtMaxPI,
      CheckBox chkOnly,
      TextBox txtMI,
      TextBox txtEscrow,
      TextBox txtMinTotal,
      TextBox txtMaxTotal,
      CheckBox chkRangedPayment,
      CheckBox chkRangedYears,
      CheckBox chkFinalPayment,
      bool clearFields)
    {
      if (txtYear1 != null)
        this.inputData.SetCurrentField(txtYear1.Tag.ToString(), clearFields || chkFinalPayment != null && chkFinalPayment.Checked ? "" : txtYear1.Text);
      this.inputData.SetCurrentField(txtYear2.Tag.ToString(), clearFields || chkFinalPayment != null && chkFinalPayment.Checked ? "" : (chkRangedYears.Checked ? txtYear2.Text : (txtYear1 != null ? txtYear1.Text : "1")));
      this.inputData.SetCurrentField(txtMinPI.Tag.ToString(), clearFields ? "" : txtMinPI.Text);
      this.inputData.SetCurrentField(txtMaxPI.Tag.ToString(), clearFields ? "" : (chkRangedPayment.Checked ? txtMaxPI.Text : txtMinPI.Text));
      this.inputData.SetCurrentField(chkOnly.Tag.ToString(), clearFields ? "" : (chkOnly.Checked ? "Y" : ""));
      this.inputData.SetCurrentField(txtMI.Tag.ToString(), clearFields || txtMI.Text == "-" ? "" : txtMI.Text);
      this.inputData.SetCurrentField(txtEscrow.Tag.ToString(), clearFields || txtEscrow.Text == "-" ? "" : txtEscrow.Text);
      this.inputData.SetCurrentField(txtMinTotal.Tag.ToString(), clearFields ? "" : txtMinTotal.Text);
      this.inputData.SetCurrentField(txtMaxTotal.Tag.ToString(), clearFields ? "" : (chkRangedPayment.Checked ? txtMaxTotal.Text : txtMinTotal.Text));
    }

    private void cboNoColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.grpCol1.Visible = this.cboNoColumns.SelectedIndex >= 0;
      this.grpCol2.Visible = this.cboNoColumns.SelectedIndex >= 1;
      this.grpCol3.Visible = this.cboNoColumns.SelectedIndex >= 2;
      this.grpCol4.Visible = this.cboNoColumns.SelectedIndex >= 3;
      this.panelAllColumns.Width = this.grpCol0.Width + this.grpCol1.Width * (this.cboNoColumns.SelectedIndex + 1);
      this.Width = this.panelAllColumns.Left * 4 + this.panelAllColumns.Width;
      this.btnCancel.Left = this.panelAllColumns.Left + this.panelAllColumns.Width - this.btnCancel.Width;
      this.btnOK.Left = this.btnCancel.Left - 10 - this.btnOK.Width;
      this.chkFinalPayment2.Enabled = this.cboNoColumns.SelectedIndex == 1;
      this.chkFinalPayment3.Enabled = this.cboNoColumns.SelectedIndex == 2;
      this.chkFinalPayment4.Enabled = this.cboNoColumns.SelectedIndex == 3;
      if (!this.chkFinalPayment2.Enabled)
        this.chkFinalPayment2.Checked = false;
      if (!this.chkFinalPayment3.Enabled)
        this.chkFinalPayment3.Checked = false;
      if (!this.chkFinalPayment4.Enabled)
        this.chkFinalPayment4.Checked = false;
      this.txtMI_KeyUp((object) null, (KeyEventArgs) null);
      this.txtEscrow_KeyUp((object) null, (KeyEventArgs) null);
    }

    private void rangedPayment_Checked(object sender, EventArgs e)
    {
      CheckBox sender1 = (CheckBox) sender;
      Label label1 = (Label) null;
      Label label2 = (Label) null;
      Label label3 = (Label) null;
      Label label4 = (Label) null;
      TextBox textBox1 = (TextBox) null;
      TextBox textBox2 = (TextBox) null;
      TextBox textBox3 = (TextBox) null;
      if (sender1.Name.EndsWith("1"))
      {
        label1 = this.labelP1MinPI;
        label2 = this.labelP1MaxPI;
        label3 = this.labelP1Min;
        label4 = this.labelP1Max;
        textBox1 = this.txtP1MinPI;
        textBox2 = this.txtP1MaxPI;
        textBox3 = this.txtP1MaxTotal;
      }
      else if (sender1.Name.EndsWith("2"))
      {
        label1 = this.labelP2MinPI;
        label2 = this.labelP2MaxPI;
        label3 = this.labelP2Min;
        label4 = this.labelP2Max;
        textBox1 = this.txtP2MinPI;
        textBox2 = this.txtP2MaxPI;
        textBox3 = this.txtP2MaxTotal;
      }
      else if (sender1.Name.EndsWith("3"))
      {
        label1 = this.labelP3MinPI;
        label2 = this.labelP3MaxPI;
        label3 = this.labelP3Min;
        label4 = this.labelP3Max;
        textBox1 = this.txtP3MinPI;
        textBox2 = this.txtP3MaxPI;
        textBox3 = this.txtP3MaxTotal;
      }
      else if (sender1.Name.EndsWith("4"))
      {
        label1 = this.labelP4MinPI;
        label2 = this.labelP4MaxPI;
        label3 = this.labelP4Min;
        label4 = this.labelP4Max;
        textBox1 = this.txtP4MinPI;
        textBox2 = this.txtP4MaxPI;
        textBox3 = this.txtP4MaxTotal;
      }
      if (sender1.Checked)
        label1.Visible = label2.Visible = label3.Visible = label4.Visible = textBox2.Visible = textBox3.Visible = true;
      else
        label1.Visible = label2.Visible = label3.Visible = label4.Visible = textBox2.Visible = textBox3.Visible = false;
      double num1;
      int num2;
      if (textBox1.Text != "")
      {
        TextBox textBox4 = textBox1;
        string str;
        if (!sender1.Checked)
        {
          num1 = Utils.ParseDouble((object) textBox1.Text);
          str = num1.ToString("N2");
        }
        else
        {
          num2 = Utils.ParseInt((object) Utils.ArithmeticRounding(Utils.ParseDouble((object) textBox1.Text), 0));
          str = num2.ToString("0");
        }
        textBox4.Text = str;
      }
      if (textBox2.Text != "")
      {
        TextBox textBox5 = textBox2;
        string str;
        if (!sender1.Checked)
        {
          num1 = Utils.ParseDouble((object) textBox2.Text);
          str = num1.ToString("N2");
        }
        else
        {
          num2 = Utils.ParseInt((object) Utils.ArithmeticRounding(Utils.ParseDouble((object) textBox2.Text), 0));
          str = num2.ToString("0");
        }
        textBox5.Text = str;
      }
      this.calculateFields((object) sender1, (EventArgs) null);
    }

    private void finalPayment_Checked(object sender, EventArgs e)
    {
      CheckBox sender1 = (CheckBox) sender;
      Label label1 = (Label) null;
      Label label2 = (Label) null;
      TextBox textBox1 = (TextBox) null;
      TextBox textBox2 = (TextBox) null;
      TextBox textBox3 = (TextBox) null;
      TextBox textBox4 = (TextBox) null;
      CheckBox checkBox = (CheckBox) null;
      if (sender1.Name.EndsWith("2"))
      {
        label1 = this.labelP2Year;
        label2 = this.labelP2Dash;
        textBox1 = this.txtP2Year1;
        textBox2 = this.txtP2Year2;
        textBox3 = this.txtP2MI;
        textBox4 = this.txtP2Escrow;
        checkBox = this.chkRangedYears2;
      }
      else if (sender1.Name.EndsWith("3"))
      {
        label1 = this.labelP3Year;
        label2 = this.labelP3Dash;
        textBox1 = this.txtP3Year1;
        textBox2 = this.txtP3Year2;
        textBox3 = this.txtP3MI;
        textBox4 = this.txtP3Escrow;
        checkBox = this.chkRangedYears3;
      }
      else if (sender1.Name.EndsWith("4"))
      {
        label1 = this.labelP4Year;
        label2 = this.labelP4Dash;
        textBox1 = this.txtP4Year1;
        textBox2 = this.txtP4Year2;
        textBox3 = this.txtP4MI;
        textBox4 = this.txtP4Escrow;
        checkBox = this.chkRangedYears4;
      }
      if (sender1.Checked)
      {
        label1.Visible = false;
        label2.Visible = true;
        label2.Left = 30;
        label2.Text = "Final Payment";
        textBox1.Visible = textBox2.Visible = false;
        if (checkBox.Checked)
          checkBox.Checked = false;
        checkBox.Enabled = false;
        bool flag1 = this.miOrEscrowHasValue(false);
        textBox4.Text = flag1 ? "-" : "0.00";
        textBox4.TextAlign = flag1 ? HorizontalAlignment.Center : HorizontalAlignment.Right;
        textBox4.Enabled = false;
        bool flag2 = this.miOrEscrowHasValue(true);
        for (int index = 0; index < this.cboNoColumns.SelectedIndex; ++index)
        {
          if (index == 0 && Utils.ParseDecimal((object) this.txtP1MI.Text) != 0M || index == 1 && Utils.ParseDecimal((object) this.txtP2MI.Text) != 0M || index == 2 && Utils.ParseDecimal((object) this.txtP3MI.Text) != 0M)
          {
            flag2 = true;
            break;
          }
        }
        textBox3.Text = flag2 ? "-" : "0.00";
        textBox3.TextAlign = flag2 ? HorizontalAlignment.Center : HorizontalAlignment.Right;
        textBox3.Enabled = false;
        this.calculateFields((object) sender1, (EventArgs) null);
      }
      else
      {
        label2.Left = 82;
        label1.Visible = true;
        label2.Text = "-";
        textBox1.Visible = true;
        checkBox.Enabled = true;
        textBox4.Enabled = true;
        textBox4.Text = this.txtP1Escrow.Text;
        textBox4.TextAlign = HorizontalAlignment.Right;
        textBox3.Text = "";
        textBox3.Enabled = true;
        textBox3.TextAlign = HorizontalAlignment.Right;
        this.rangedYears_Checked(sender, e);
      }
    }

    private bool miOrEscrowHasValue(bool checkMI)
    {
      for (int index = 0; index < this.cboNoColumns.SelectedIndex; ++index)
      {
        if (index == 0 && Utils.ParseDecimal(checkMI ? (object) this.txtP1MI.Text : (object) this.txtP1Escrow.Text) != 0M || index == 1 && Utils.ParseDecimal(checkMI ? (object) this.txtP2MI.Text : (object) this.txtP2Escrow.Text) != 0M || index == 2 && Utils.ParseDecimal(checkMI ? (object) this.txtP3MI.Text : (object) this.txtP3Escrow.Text) != 0M)
          return true;
      }
      return false;
    }

    private void rangedYears_Checked(object sender, EventArgs e)
    {
      CheckBox sender1 = (CheckBox) sender;
      Label label1 = (Label) null;
      Label label2 = (Label) null;
      TextBox textBox = (TextBox) null;
      CheckBox checkBox1 = (CheckBox) null;
      CheckBox checkBox2 = (CheckBox) null;
      if (sender1.Name.EndsWith("1"))
      {
        label1 = this.labelP1Year;
        label2 = (Label) null;
        textBox = (TextBox) null;
        checkBox1 = (CheckBox) null;
        checkBox2 = this.chkRangedYears1;
      }
      else if (sender1.Name.EndsWith("2"))
      {
        label1 = this.labelP2Year;
        label2 = this.labelP2Dash;
        textBox = this.txtP2Year2;
        checkBox1 = this.chkFinalPayment2;
        checkBox2 = this.chkRangedYears2;
      }
      else if (sender1.Name.EndsWith("3"))
      {
        label1 = this.labelP3Year;
        label2 = this.labelP3Dash;
        textBox = this.txtP3Year2;
        checkBox1 = this.chkFinalPayment3;
        checkBox2 = this.chkRangedYears3;
      }
      else if (sender1.Name.EndsWith("4"))
      {
        label1 = this.labelP4Year;
        label2 = this.labelP4Dash;
        textBox = this.txtP4Year2;
        checkBox1 = this.chkFinalPayment4;
        checkBox2 = this.chkRangedYears4;
      }
      if (checkBox1 == null || checkBox1 != null && !checkBox1.Checked)
      {
        if (checkBox2.Checked)
        {
          if (sender1.Name.EndsWith("1"))
          {
            label1.Text = "Years 1  -";
            this.txtP1Year2.Enabled = true;
            if (this.txtP1Year2.Text == "1")
              this.txtP1Year2.Text = "2";
          }
          else
            label1.Text = "Years";
          if (label2 != null)
            label2.Visible = true;
          if (textBox != null)
            textBox.Visible = true;
        }
        else
        {
          label1.Text = "Year";
          if (label2 != null)
            label2.Visible = false;
          if (textBox != null)
            textBox.Visible = false;
          if (sender1.Name.EndsWith("1"))
          {
            this.txtP1Year2.Text = "1";
            this.txtP1Year2.Enabled = false;
          }
        }
      }
      this.calculateFields((object) sender1, (EventArgs) null);
    }

    private void textField_Enter(object sender, EventArgs e)
    {
      if (sender is TextBox)
        this.currentValue = ((Control) sender).Text;
      else
        this.currentValue = (string) null;
    }

    private void textField_Leave(object sender, EventArgs e)
    {
      TextBox sender1 = (TextBox) sender;
      if (!this.validateValues(sender, e))
        sender1.Text = this.currentValue;
      if (sender1.Text != "" && (sender1.Name.EndsWith("MinPI") || sender1.Name.EndsWith("MaxPI")))
      {
        CheckBox checkBox = (CheckBox) null;
        if (sender1.Name.IndexOf("P1") > -1)
          checkBox = this.chkRangedPayment1;
        else if (sender1.Name.IndexOf("P2") > -1)
          checkBox = this.chkRangedPayment2;
        else if (sender1.Name.IndexOf("P3") > -1)
          checkBox = this.chkRangedPayment3;
        else if (sender1.Name.IndexOf("P4") > -1)
          checkBox = this.chkRangedPayment4;
        if (checkBox.Checked)
          sender1.Text = Utils.ArithmeticRounding(Utils.ParseDouble((object) sender1.Text), 0).ToString("0");
      }
      this.calculateFields((object) sender1, (EventArgs) null);
    }

    private void calculateFields(object sender, EventArgs e)
    {
      if (this.cboNoColumns.SelectedIndex >= 0)
        this.calculateMinMaxTotal(this.txtP1MinPI, this.txtP1MaxPI, this.chkP1Only, this.txtP1MI, this.txtP1Escrow, this.txtP1MinTotal, this.txtP1MaxTotal);
      if (this.cboNoColumns.SelectedIndex >= 1)
        this.calculateMinMaxTotal(this.txtP2MinPI, this.txtP2MaxPI, this.chkP2Only, this.txtP2MI, this.txtP2Escrow, this.txtP2MinTotal, this.txtP2MaxTotal);
      if (this.cboNoColumns.SelectedIndex >= 2)
        this.calculateMinMaxTotal(this.txtP3MinPI, this.txtP3MaxPI, this.chkP3Only, this.txtP3MI, this.txtP3Escrow, this.txtP3MinTotal, this.txtP3MaxTotal);
      if (this.cboNoColumns.SelectedIndex < 3)
        return;
      this.calculateMinMaxTotal(this.txtP4MinPI, this.txtP4MaxPI, this.chkP4Only, this.txtP4MI, this.txtP4Escrow, this.txtP4MinTotal, this.txtP4MaxTotal);
    }

    private void calculateMinMaxTotal(
      TextBox txtMinPI,
      TextBox txtMaxPI,
      CheckBox chkOnly,
      TextBox txtMI,
      TextBox txtEscrow,
      TextBox txtMinTotal,
      TextBox txtMaxTotal)
    {
      if (chkOnly.Name.IndexOf("P2") > -1 && this.chkP2Only.Checked)
        this.chkP1Only.Checked = true;
      else if (chkOnly.Name.IndexOf("P3") > -1 && this.chkP3Only.Checked)
      {
        this.chkP1Only.Checked = true;
        this.chkP2Only.Checked = true;
      }
      else if (chkOnly.Name.IndexOf("P4") > -1 && this.chkP4Only.Checked)
      {
        this.chkP1Only.Checked = true;
        this.chkP2Only.Checked = true;
        this.chkP3Only.Checked = true;
      }
      CheckBox checkBox;
      if (chkOnly.Name.IndexOf("P2") > -1)
      {
        checkBox = this.chkRangedPayment2;
        if (this.chkFinalPayment2.Checked)
        {
          this.txtP2Escrow.Text = this.miOrEscrowHasValue(false) ? "-" : "0.00";
          this.txtP2MI.Text = this.miOrEscrowHasValue(true) ? "-" : "0.00";
        }
      }
      else if (chkOnly.Name.IndexOf("P3") > -1)
      {
        checkBox = this.chkRangedPayment3;
        if (this.chkFinalPayment3.Checked)
        {
          this.txtP3Escrow.Text = this.miOrEscrowHasValue(false) ? "-" : "0.00";
          this.txtP3MI.Text = this.miOrEscrowHasValue(true) ? "-" : "0.00";
        }
      }
      else if (chkOnly.Name.IndexOf("P4") > -1)
      {
        checkBox = this.chkRangedPayment4;
        if (this.chkFinalPayment4.Checked)
        {
          this.txtP4Escrow.Text = this.miOrEscrowHasValue(false) ? "-" : "0.00";
          this.txtP4MI.Text = this.miOrEscrowHasValue(true) ? "-" : "0.00";
        }
      }
      else
        checkBox = this.chkRangedPayment1;
      txtEscrow.TextAlign = txtEscrow.Text == "-" ? HorizontalAlignment.Center : HorizontalAlignment.Right;
      txtMI.TextAlign = txtMI.Text == "-" ? HorizontalAlignment.Center : HorizontalAlignment.Right;
      double num1 = Utils.ParseDouble((object) txtMI.Text) + Utils.ParseDouble((object) txtEscrow.Text);
      double num2 = Utils.ParseDouble((object) txtMinPI.Text) + num1;
      txtMinTotal.Text = checkBox.Checked ? num2.ToString("0") : num2.ToString("N2");
      double num3 = Utils.ParseDouble((object) txtMaxPI.Text) + num1;
      txtMaxTotal.Text = checkBox.Checked ? num3.ToString("0") : num3.ToString("N2");
    }

    private void EditProjectedPaymentTable_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    public string GetHelpTargetName() => "CustomizeProjectedPayments";

    private void interestOnly_Checked(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox) sender;
      if (!checkBox.Checked)
      {
        if (checkBox.Name.IndexOf("P1") > -1)
        {
          this.chkP2Only.Checked = false;
          this.chkP3Only.Checked = false;
          this.chkP4Only.Checked = false;
        }
        else if (checkBox.Name.IndexOf("P2") > -1)
        {
          this.chkP3Only.Checked = false;
          this.chkP4Only.Checked = false;
        }
        else if (checkBox.Name.IndexOf("P3") > -1)
          this.chkP4Only.Checked = false;
      }
      this.calculateFields((object) null, (EventArgs) null);
    }

    private void btnCancel_MouseEnter(object sender, EventArgs e) => this.validationEnabled = false;

    private void btnCancel_MouseLeave(object sender, EventArgs e) => this.validationEnabled = true;

    private void txtMinMax_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      CheckBox checkBox = (CheckBox) null;
      if (textBox.Name.IndexOf("P1") > -1)
        checkBox = this.chkRangedPayment1;
      else if (textBox.Name.IndexOf("P2") > -1)
        checkBox = this.chkRangedPayment2;
      else if (textBox.Name.IndexOf("P3") > -1)
        checkBox = this.chkRangedPayment3;
      else if (textBox.Name.IndexOf("P4") > -1)
        checkBox = this.chkRangedPayment4;
      if (!checkBox.Checked)
        return;
      FieldFormat dataFormat = FieldFormat.INTEGER;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void txtMI_KeyUp(object sender, KeyEventArgs e)
    {
      this.txtP2MI.Enabled = !this.chkFinalPayment2.Checked && this.txtP1MI.Text != "";
      if (!this.txtP2MI.Enabled)
        this.txtP2MI.Text = "";
      this.txtP3MI.Enabled = !this.chkFinalPayment3.Checked && this.txtP1MI.Text != "";
      if (!this.txtP3MI.Enabled)
        this.txtP3MI.Text = "";
      this.txtP4MI.Enabled = !this.chkFinalPayment4.Checked && this.txtP1MI.Text != "";
      if (this.txtP4MI.Enabled)
        return;
      this.txtP4MI.Text = "";
    }

    private void txtEscrow_KeyUp(object sender, KeyEventArgs e)
    {
      this.txtP2Escrow.Enabled = !this.chkFinalPayment2.Checked && this.txtP1Escrow.Text != "";
      if (!this.txtP2Escrow.Enabled)
        this.txtP2Escrow.Text = "";
      this.txtP3Escrow.Enabled = !this.chkFinalPayment3.Checked && this.txtP1Escrow.Text != "";
      if (!this.txtP3Escrow.Enabled)
        this.txtP3Escrow.Text = "";
      this.txtP4Escrow.Enabled = !this.chkFinalPayment4.Checked && this.txtP1Escrow.Text != "";
      if (this.txtP4Escrow.Enabled)
        return;
      this.txtP4Escrow.Text = "";
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditProjectedPaymentTable));
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.cboNoColumns = new ComboBox();
      this.label1 = new Label();
      this.panelAllColumns = new Panel();
      this.grpCol0 = new GroupContainer();
      this.label12 = new Label();
      this.label3 = new Label();
      this.txtEstPayment = new TextBox();
      this.label11 = new Label();
      this.label2 = new Label();
      this.label10 = new Label();
      this.label9 = new Label();
      this.grpCol1 = new GroupContainer();
      this.txtP1Year2 = new TextBox();
      this.labelP1Max = new Label();
      this.labelP1Min = new Label();
      this.txtP1MaxTotal = new TextBox();
      this.txtP1MinTotal = new TextBox();
      this.txtP1Escrow = new TextBox();
      this.txtP1MI = new TextBox();
      this.chkP1Only = new CheckBox();
      this.txtP1MaxPI = new TextBox();
      this.labelP1MaxPI = new Label();
      this.txtP1MinPI = new TextBox();
      this.labelP1MinPI = new Label();
      this.labelP1Year = new Label();
      this.chkRangedYears1 = new CheckBox();
      this.chkRangedPayment1 = new CheckBox();
      this.grpCol2 = new GroupContainer();
      this.labelP2Dash = new Label();
      this.txtP2Year2 = new TextBox();
      this.labelP2Max = new Label();
      this.labelP2Min = new Label();
      this.txtP2MaxTotal = new TextBox();
      this.txtP2MinTotal = new TextBox();
      this.txtP2Escrow = new TextBox();
      this.txtP2MI = new TextBox();
      this.chkP2Only = new CheckBox();
      this.txtP2MaxPI = new TextBox();
      this.labelP2MaxPI = new Label();
      this.txtP2MinPI = new TextBox();
      this.labelP2MinPI = new Label();
      this.txtP2Year1 = new TextBox();
      this.labelP2Year = new Label();
      this.chkRangedYears2 = new CheckBox();
      this.chkFinalPayment2 = new CheckBox();
      this.chkRangedPayment2 = new CheckBox();
      this.grpCol3 = new GroupContainer();
      this.labelP3Dash = new Label();
      this.txtP3Year2 = new TextBox();
      this.labelP3Max = new Label();
      this.labelP3Min = new Label();
      this.txtP3MaxTotal = new TextBox();
      this.txtP3MinTotal = new TextBox();
      this.txtP3Escrow = new TextBox();
      this.txtP3MI = new TextBox();
      this.chkP3Only = new CheckBox();
      this.txtP3MaxPI = new TextBox();
      this.labelP3MaxPI = new Label();
      this.txtP3MinPI = new TextBox();
      this.labelP3MinPI = new Label();
      this.txtP3Year1 = new TextBox();
      this.labelP3Year = new Label();
      this.chkRangedYears3 = new CheckBox();
      this.chkFinalPayment3 = new CheckBox();
      this.chkRangedPayment3 = new CheckBox();
      this.grpCol4 = new GroupContainer();
      this.labelP4Dash = new Label();
      this.txtP4Year2 = new TextBox();
      this.labelP4Max = new Label();
      this.labelP4Min = new Label();
      this.txtP4MaxTotal = new TextBox();
      this.txtP4MinTotal = new TextBox();
      this.txtP4Escrow = new TextBox();
      this.txtP4MI = new TextBox();
      this.chkP4Only = new CheckBox();
      this.txtP4MaxPI = new TextBox();
      this.labelP4MaxPI = new Label();
      this.txtP4MinPI = new TextBox();
      this.labelP4MinPI = new Label();
      this.txtP4Year1 = new TextBox();
      this.labelP4Year = new Label();
      this.chkRangedYears4 = new CheckBox();
      this.chkFinalPayment4 = new CheckBox();
      this.chkRangedPayment4 = new CheckBox();
      this.toolTipField = new ToolTip(this.components);
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.emHelpLink1 = new EMHelpLink();
      this.panelAllColumns.SuspendLayout();
      this.grpCol0.SuspendLayout();
      this.grpCol1.SuspendLayout();
      this.grpCol2.SuspendLayout();
      this.grpCol3.SuspendLayout();
      this.grpCol4.SuspendLayout();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(688, 320);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.MouseEnter += new EventHandler(this.btnCancel_MouseEnter);
      this.btnCancel.MouseLeave += new EventHandler(this.btnCancel_MouseLeave);
      this.btnOK.Location = new Point(607, 320);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.cboNoColumns.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboNoColumns.FormattingEnabled = true;
      this.cboNoColumns.Items.AddRange(new object[4]
      {
        (object) "1",
        (object) "2",
        (object) "3",
        (object) "4"
      });
      this.cboNoColumns.Location = new Point(112, 10);
      this.cboNoColumns.Name = "cboNoColumns";
      this.cboNoColumns.Size = new Size(71, 21);
      this.cboNoColumns.TabIndex = 0;
      this.cboNoColumns.SelectedIndexChanged += new EventHandler(this.cboNoColumns_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(99, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Number of Columns";
      this.panelAllColumns.Controls.Add((Control) this.grpCol0);
      this.panelAllColumns.Controls.Add((Control) this.grpCol1);
      this.panelAllColumns.Controls.Add((Control) this.grpCol2);
      this.panelAllColumns.Controls.Add((Control) this.grpCol3);
      this.panelAllColumns.Controls.Add((Control) this.grpCol4);
      this.panelAllColumns.Location = new Point(10, 38);
      this.panelAllColumns.Name = "panelAllColumns";
      this.panelAllColumns.Size = new Size(754, 274);
      this.panelAllColumns.TabIndex = 18;
      this.grpCol0.Controls.Add((Control) this.label12);
      this.grpCol0.Controls.Add((Control) this.label3);
      this.grpCol0.Controls.Add((Control) this.txtEstPayment);
      this.grpCol0.Controls.Add((Control) this.label11);
      this.grpCol0.Controls.Add((Control) this.label2);
      this.grpCol0.Controls.Add((Control) this.label10);
      this.grpCol0.Controls.Add((Control) this.label9);
      this.grpCol0.HeaderForeColor = SystemColors.ControlText;
      this.grpCol0.Location = new Point(1, 0);
      this.grpCol0.Name = "grpCol0";
      this.grpCol0.Size = new Size(188, 271);
      this.grpCol0.TabIndex = 21;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(137, 223);
      this.label12.Name = "label12";
      this.label12.Size = new Size(48, 13);
      this.label12.TabIndex = 23;
      this.label12.Text = "Payment";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(1, 114);
      this.label3.Name = "label3";
      this.label3.Size = new Size(94, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Principal && Interest";
      this.txtEstPayment.Location = new Point(56, 220);
      this.txtEstPayment.Name = "txtEstPayment";
      this.txtEstPayment.ReadOnly = true;
      this.txtEstPayment.Size = new Size(75, 20);
      this.txtEstPayment.TabIndex = 22;
      this.txtEstPayment.Tag = (object) "3291";
      this.txtEstPayment.TextAlign = HorizontalAlignment.Center;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(2, 223);
      this.label11.Name = "label11";
      this.label11.Size = new Size(52, 13);
      this.label11.TabIndex = 21;
      this.label11.Text = "Total Est.";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(1, 92);
      this.label2.Name = "label2";
      this.label2.Size = new Size(40, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Period ";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(1, 201);
      this.label10.Name = "label10";
      this.label10.Size = new Size(91, 13);
      this.label10.TabIndex = 20;
      this.label10.Text = "Estimated Escrow";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(1, 179);
      this.label9.Name = "label9";
      this.label9.Size = new Size(102, 13);
      this.label9.TabIndex = 19;
      this.label9.Text = "Mortgage Insurance";
      this.grpCol1.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.grpCol1.Controls.Add((Control) this.txtP1Year2);
      this.grpCol1.Controls.Add((Control) this.labelP1Max);
      this.grpCol1.Controls.Add((Control) this.labelP1Min);
      this.grpCol1.Controls.Add((Control) this.txtP1MaxTotal);
      this.grpCol1.Controls.Add((Control) this.txtP1MinTotal);
      this.grpCol1.Controls.Add((Control) this.txtP1Escrow);
      this.grpCol1.Controls.Add((Control) this.txtP1MI);
      this.grpCol1.Controls.Add((Control) this.chkP1Only);
      this.grpCol1.Controls.Add((Control) this.txtP1MaxPI);
      this.grpCol1.Controls.Add((Control) this.labelP1MaxPI);
      this.grpCol1.Controls.Add((Control) this.txtP1MinPI);
      this.grpCol1.Controls.Add((Control) this.labelP1MinPI);
      this.grpCol1.Controls.Add((Control) this.labelP1Year);
      this.grpCol1.Controls.Add((Control) this.chkRangedYears1);
      this.grpCol1.Controls.Add((Control) this.chkRangedPayment1);
      this.grpCol1.HeaderForeColor = SystemColors.ControlText;
      this.grpCol1.Location = new Point(188, 0);
      this.grpCol1.Name = "grpCol1";
      this.grpCol1.Size = new Size(142, 271);
      this.grpCol1.TabIndex = 17;
      this.grpCol1.Text = "            1st Period";
      this.txtP1Year2.Location = new Point(92, 89);
      this.txtP1Year2.MaxLength = 2;
      this.txtP1Year2.Name = "txtP1Year2";
      this.txtP1Year2.Size = new Size(41, 20);
      this.txtP1Year2.TabIndex = 3;
      this.txtP1Year2.Tag = (object) "CD1.X7";
      this.txtP1Year2.TextAlign = HorizontalAlignment.Right;
      this.labelP1Max.AutoSize = true;
      this.labelP1Max.Location = new Point(3, 245);
      this.labelP1Max.Name = "labelP1Max";
      this.labelP1Max.Size = new Size(27, 13);
      this.labelP1Max.TabIndex = 42;
      this.labelP1Max.Text = "Max";
      this.labelP1Min.AutoSize = true;
      this.labelP1Min.Location = new Point(3, 223);
      this.labelP1Min.Name = "labelP1Min";
      this.labelP1Min.Size = new Size(24, 13);
      this.labelP1Min.TabIndex = 41;
      this.labelP1Min.Text = "Min";
      this.txtP1MaxTotal.Location = new Point(41, 242);
      this.txtP1MaxTotal.Name = "txtP1MaxTotal";
      this.txtP1MaxTotal.ReadOnly = true;
      this.txtP1MaxTotal.Size = new Size(92, 20);
      this.txtP1MaxTotal.TabIndex = 40;
      this.txtP1MaxTotal.TabStop = false;
      this.txtP1MaxTotal.Tag = (object) "CD1.X14";
      this.txtP1MaxTotal.TextAlign = HorizontalAlignment.Right;
      this.txtP1MinTotal.Location = new Point(41, 220);
      this.txtP1MinTotal.Name = "txtP1MinTotal";
      this.txtP1MinTotal.ReadOnly = true;
      this.txtP1MinTotal.Size = new Size(92, 20);
      this.txtP1MinTotal.TabIndex = 39;
      this.txtP1MinTotal.TabStop = false;
      this.txtP1MinTotal.Tag = (object) "CD1.X13";
      this.txtP1MinTotal.TextAlign = HorizontalAlignment.Right;
      this.txtP1Escrow.Location = new Point(41, 198);
      this.txtP1Escrow.MaxLength = 8;
      this.txtP1Escrow.Name = "txtP1Escrow";
      this.txtP1Escrow.Size = new Size(92, 20);
      this.txtP1Escrow.TabIndex = 8;
      this.txtP1Escrow.Tag = (object) "CD1.X12";
      this.txtP1Escrow.TextAlign = HorizontalAlignment.Right;
      this.txtP1Escrow.KeyUp += new KeyEventHandler(this.txtEscrow_KeyUp);
      this.txtP1MI.Location = new Point(41, 176);
      this.txtP1MI.MaxLength = 8;
      this.txtP1MI.Name = "txtP1MI";
      this.txtP1MI.Size = new Size(92, 20);
      this.txtP1MI.TabIndex = 7;
      this.txtP1MI.Tag = (object) "CD1.X11";
      this.txtP1MI.TextAlign = HorizontalAlignment.Right;
      this.txtP1MI.KeyUp += new KeyEventHandler(this.txtMI_KeyUp);
      this.chkP1Only.AutoSize = true;
      this.chkP1Only.Location = new Point(42, 157);
      this.chkP1Only.Name = "chkP1Only";
      this.chkP1Only.Size = new Size(85, 17);
      this.chkP1Only.TabIndex = 6;
      this.chkP1Only.Tag = (object) "CD1.X10";
      this.chkP1Only.Text = "Interest Only";
      this.chkP1Only.UseVisualStyleBackColor = true;
      this.chkP1Only.CheckedChanged += new EventHandler(this.interestOnly_Checked);
      this.txtP1MaxPI.Location = new Point(41, 133);
      this.txtP1MaxPI.MaxLength = 12;
      this.txtP1MaxPI.Name = "txtP1MaxPI";
      this.txtP1MaxPI.Size = new Size(92, 20);
      this.txtP1MaxPI.TabIndex = 5;
      this.txtP1MaxPI.Tag = (object) "CD1.X9";
      this.txtP1MaxPI.TextAlign = HorizontalAlignment.Right;
      this.txtP1MaxPI.KeyUp += new KeyEventHandler(this.txtMinMax_KeyUp);
      this.labelP1MaxPI.AutoSize = true;
      this.labelP1MaxPI.Location = new Point(3, 136);
      this.labelP1MaxPI.Name = "labelP1MaxPI";
      this.labelP1MaxPI.Size = new Size(27, 13);
      this.labelP1MaxPI.TabIndex = 34;
      this.labelP1MaxPI.Text = "Max";
      this.txtP1MinPI.Location = new Point(41, 111);
      this.txtP1MinPI.MaxLength = 12;
      this.txtP1MinPI.Name = "txtP1MinPI";
      this.txtP1MinPI.Size = new Size(92, 20);
      this.txtP1MinPI.TabIndex = 4;
      this.txtP1MinPI.Tag = (object) "CD1.X8";
      this.txtP1MinPI.TextAlign = HorizontalAlignment.Right;
      this.txtP1MinPI.KeyUp += new KeyEventHandler(this.txtMinMax_KeyUp);
      this.labelP1MinPI.AutoSize = true;
      this.labelP1MinPI.Location = new Point(3, 114);
      this.labelP1MinPI.Name = "labelP1MinPI";
      this.labelP1MinPI.Size = new Size(24, 13);
      this.labelP1MinPI.TabIndex = 32;
      this.labelP1MinPI.Text = "Min";
      this.labelP1Year.AutoSize = true;
      this.labelP1Year.Location = new Point(37, 92);
      this.labelP1Year.Name = "labelP1Year";
      this.labelP1Year.Size = new Size(52, 13);
      this.labelP1Year.TabIndex = 30;
      this.labelP1Year.Text = "Years 1  -";
      this.chkRangedYears1.AutoSize = true;
      this.chkRangedYears1.Location = new Point(6, 48);
      this.chkRangedYears1.Name = "chkRangedYears1";
      this.chkRangedYears1.Size = new Size(106, 17);
      this.chkRangedYears1.TabIndex = 2;
      this.chkRangedYears1.Text = "More than a year";
      this.chkRangedYears1.UseVisualStyleBackColor = true;
      this.chkRangedYears1.CheckedChanged += new EventHandler(this.rangedYears_Checked);
      this.chkRangedPayment1.AutoSize = true;
      this.chkRangedPayment1.Location = new Point(6, 29);
      this.chkRangedPayment1.Name = "chkRangedPayment1";
      this.chkRangedPayment1.Size = new Size(77, 17);
      this.chkRangedPayment1.TabIndex = 1;
      this.chkRangedPayment1.Text = "P&&I Range";
      this.chkRangedPayment1.UseVisualStyleBackColor = true;
      this.chkRangedPayment1.CheckedChanged += new EventHandler(this.rangedPayment_Checked);
      this.grpCol2.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.grpCol2.Controls.Add((Control) this.labelP2Dash);
      this.grpCol2.Controls.Add((Control) this.txtP2Year2);
      this.grpCol2.Controls.Add((Control) this.labelP2Max);
      this.grpCol2.Controls.Add((Control) this.labelP2Min);
      this.grpCol2.Controls.Add((Control) this.txtP2MaxTotal);
      this.grpCol2.Controls.Add((Control) this.txtP2MinTotal);
      this.grpCol2.Controls.Add((Control) this.txtP2Escrow);
      this.grpCol2.Controls.Add((Control) this.txtP2MI);
      this.grpCol2.Controls.Add((Control) this.chkP2Only);
      this.grpCol2.Controls.Add((Control) this.txtP2MaxPI);
      this.grpCol2.Controls.Add((Control) this.labelP2MaxPI);
      this.grpCol2.Controls.Add((Control) this.txtP2MinPI);
      this.grpCol2.Controls.Add((Control) this.labelP2MinPI);
      this.grpCol2.Controls.Add((Control) this.txtP2Year1);
      this.grpCol2.Controls.Add((Control) this.labelP2Year);
      this.grpCol2.Controls.Add((Control) this.chkRangedYears2);
      this.grpCol2.Controls.Add((Control) this.chkFinalPayment2);
      this.grpCol2.Controls.Add((Control) this.chkRangedPayment2);
      this.grpCol2.HeaderForeColor = SystemColors.ControlText;
      this.grpCol2.Location = new Point(329, 0);
      this.grpCol2.Name = "grpCol2";
      this.grpCol2.Size = new Size(142, 271);
      this.grpCol2.TabIndex = 18;
      this.grpCol2.Text = "            2nd Period";
      this.labelP2Dash.AutoSize = true;
      this.labelP2Dash.Location = new Point(82, 92);
      this.labelP2Dash.Name = "labelP2Dash";
      this.labelP2Dash.Size = new Size(10, 13);
      this.labelP2Dash.TabIndex = 29;
      this.labelP2Dash.Text = "-";
      this.txtP2Year2.Location = new Point(92, 89);
      this.txtP2Year2.MaxLength = 2;
      this.txtP2Year2.Name = "txtP2Year2";
      this.txtP2Year2.Size = new Size(41, 20);
      this.txtP2Year2.TabIndex = 13;
      this.txtP2Year2.Tag = (object) "CD1.X16";
      this.txtP2Year2.TextAlign = HorizontalAlignment.Right;
      this.labelP2Max.AutoSize = true;
      this.labelP2Max.Location = new Point(3, 245);
      this.labelP2Max.Name = "labelP2Max";
      this.labelP2Max.Size = new Size(27, 13);
      this.labelP2Max.TabIndex = 27;
      this.labelP2Max.Text = "Max";
      this.labelP2Min.AutoSize = true;
      this.labelP2Min.Location = new Point(3, 223);
      this.labelP2Min.Name = "labelP2Min";
      this.labelP2Min.Size = new Size(24, 13);
      this.labelP2Min.TabIndex = 26;
      this.labelP2Min.Text = "Min";
      this.txtP2MaxTotal.Location = new Point(41, 242);
      this.txtP2MaxTotal.Name = "txtP2MaxTotal";
      this.txtP2MaxTotal.ReadOnly = true;
      this.txtP2MaxTotal.Size = new Size(92, 20);
      this.txtP2MaxTotal.TabIndex = 25;
      this.txtP2MaxTotal.TabStop = false;
      this.txtP2MaxTotal.Tag = (object) "CD1.X23";
      this.txtP2MaxTotal.TextAlign = HorizontalAlignment.Right;
      this.txtP2MinTotal.Location = new Point(41, 220);
      this.txtP2MinTotal.Name = "txtP2MinTotal";
      this.txtP2MinTotal.ReadOnly = true;
      this.txtP2MinTotal.Size = new Size(92, 20);
      this.txtP2MinTotal.TabIndex = 24;
      this.txtP2MinTotal.TabStop = false;
      this.txtP2MinTotal.Tag = (object) "CD1.X22";
      this.txtP2MinTotal.TextAlign = HorizontalAlignment.Right;
      this.txtP2Escrow.Location = new Point(41, 198);
      this.txtP2Escrow.MaxLength = 15;
      this.txtP2Escrow.Name = "txtP2Escrow";
      this.txtP2Escrow.Size = new Size(92, 20);
      this.txtP2Escrow.TabIndex = 18;
      this.txtP2Escrow.Tag = (object) "CD1.X21";
      this.txtP2Escrow.TextAlign = HorizontalAlignment.Right;
      this.txtP2Escrow.KeyUp += new KeyEventHandler(this.txtEscrow_KeyUp);
      this.txtP2MI.Location = new Point(41, 176);
      this.txtP2MI.MaxLength = 8;
      this.txtP2MI.Name = "txtP2MI";
      this.txtP2MI.Size = new Size(92, 20);
      this.txtP2MI.TabIndex = 17;
      this.txtP2MI.Tag = (object) "CD1.X20";
      this.txtP2MI.TextAlign = HorizontalAlignment.Right;
      this.txtP2MI.KeyUp += new KeyEventHandler(this.txtMI_KeyUp);
      this.chkP2Only.AutoSize = true;
      this.chkP2Only.Location = new Point(42, 157);
      this.chkP2Only.Name = "chkP2Only";
      this.chkP2Only.Size = new Size(85, 17);
      this.chkP2Only.TabIndex = 16;
      this.chkP2Only.Tag = (object) "CD1.X19";
      this.chkP2Only.Text = "Interest Only";
      this.chkP2Only.UseVisualStyleBackColor = true;
      this.chkP2Only.CheckedChanged += new EventHandler(this.interestOnly_Checked);
      this.txtP2MaxPI.Location = new Point(41, 133);
      this.txtP2MaxPI.MaxLength = 12;
      this.txtP2MaxPI.Name = "txtP2MaxPI";
      this.txtP2MaxPI.Size = new Size(92, 20);
      this.txtP2MaxPI.TabIndex = 15;
      this.txtP2MaxPI.Tag = (object) "CD1.X18";
      this.txtP2MaxPI.TextAlign = HorizontalAlignment.Right;
      this.txtP2MaxPI.KeyUp += new KeyEventHandler(this.txtMinMax_KeyUp);
      this.labelP2MaxPI.AutoSize = true;
      this.labelP2MaxPI.Location = new Point(3, 136);
      this.labelP2MaxPI.Name = "labelP2MaxPI";
      this.labelP2MaxPI.Size = new Size(27, 13);
      this.labelP2MaxPI.TabIndex = 19;
      this.labelP2MaxPI.Text = "Max";
      this.txtP2MinPI.Location = new Point(41, 111);
      this.txtP2MinPI.MaxLength = 12;
      this.txtP2MinPI.Name = "txtP2MinPI";
      this.txtP2MinPI.Size = new Size(92, 20);
      this.txtP2MinPI.TabIndex = 14;
      this.txtP2MinPI.Tag = (object) "CD1.X17";
      this.txtP2MinPI.TextAlign = HorizontalAlignment.Right;
      this.txtP2MinPI.KeyUp += new KeyEventHandler(this.txtMinMax_KeyUp);
      this.labelP2MinPI.AutoSize = true;
      this.labelP2MinPI.Location = new Point(3, 114);
      this.labelP2MinPI.Name = "labelP2MinPI";
      this.labelP2MinPI.Size = new Size(24, 13);
      this.labelP2MinPI.TabIndex = 17;
      this.labelP2MinPI.Text = "Min";
      this.txtP2Year1.Location = new Point(40, 89);
      this.txtP2Year1.MaxLength = 2;
      this.txtP2Year1.Name = "txtP2Year1";
      this.txtP2Year1.Size = new Size(41, 20);
      this.txtP2Year1.TabIndex = 12;
      this.txtP2Year1.Tag = (object) "CD1.X15";
      this.txtP2Year1.TextAlign = HorizontalAlignment.Right;
      this.labelP2Year.AutoSize = true;
      this.labelP2Year.Location = new Point(2, 92);
      this.labelP2Year.Name = "labelP2Year";
      this.labelP2Year.Size = new Size(34, 13);
      this.labelP2Year.TabIndex = 15;
      this.labelP2Year.Text = "Years";
      this.chkRangedYears2.AutoSize = true;
      this.chkRangedYears2.Location = new Point(6, 67);
      this.chkRangedYears2.Name = "chkRangedYears2";
      this.chkRangedYears2.Size = new Size(106, 17);
      this.chkRangedYears2.TabIndex = 11;
      this.chkRangedYears2.Text = "More than a year";
      this.chkRangedYears2.UseVisualStyleBackColor = true;
      this.chkRangedYears2.CheckedChanged += new EventHandler(this.rangedYears_Checked);
      this.chkFinalPayment2.AutoSize = true;
      this.chkFinalPayment2.Location = new Point(6, 48);
      this.chkFinalPayment2.Name = "chkFinalPayment2";
      this.chkFinalPayment2.Size = new Size(92, 17);
      this.chkFinalPayment2.TabIndex = 10;
      this.chkFinalPayment2.Text = "Final Payment";
      this.chkFinalPayment2.UseVisualStyleBackColor = true;
      this.chkFinalPayment2.CheckedChanged += new EventHandler(this.finalPayment_Checked);
      this.chkRangedPayment2.AutoSize = true;
      this.chkRangedPayment2.Location = new Point(6, 29);
      this.chkRangedPayment2.Name = "chkRangedPayment2";
      this.chkRangedPayment2.Size = new Size(77, 17);
      this.chkRangedPayment2.TabIndex = 9;
      this.chkRangedPayment2.Text = "P&&I Range";
      this.chkRangedPayment2.UseVisualStyleBackColor = true;
      this.chkRangedPayment2.CheckedChanged += new EventHandler(this.rangedPayment_Checked);
      this.grpCol3.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.grpCol3.Controls.Add((Control) this.labelP3Dash);
      this.grpCol3.Controls.Add((Control) this.txtP3Year2);
      this.grpCol3.Controls.Add((Control) this.labelP3Max);
      this.grpCol3.Controls.Add((Control) this.labelP3Min);
      this.grpCol3.Controls.Add((Control) this.txtP3MaxTotal);
      this.grpCol3.Controls.Add((Control) this.txtP3MinTotal);
      this.grpCol3.Controls.Add((Control) this.txtP3Escrow);
      this.grpCol3.Controls.Add((Control) this.txtP3MI);
      this.grpCol3.Controls.Add((Control) this.chkP3Only);
      this.grpCol3.Controls.Add((Control) this.txtP3MaxPI);
      this.grpCol3.Controls.Add((Control) this.labelP3MaxPI);
      this.grpCol3.Controls.Add((Control) this.txtP3MinPI);
      this.grpCol3.Controls.Add((Control) this.labelP3MinPI);
      this.grpCol3.Controls.Add((Control) this.txtP3Year1);
      this.grpCol3.Controls.Add((Control) this.labelP3Year);
      this.grpCol3.Controls.Add((Control) this.chkRangedYears3);
      this.grpCol3.Controls.Add((Control) this.chkFinalPayment3);
      this.grpCol3.Controls.Add((Control) this.chkRangedPayment3);
      this.grpCol3.HeaderForeColor = SystemColors.ControlText;
      this.grpCol3.Location = new Point(470, 0);
      this.grpCol3.Name = "grpCol3";
      this.grpCol3.Size = new Size(142, 271);
      this.grpCol3.TabIndex = 19;
      this.grpCol3.Text = "            3rd Period";
      this.labelP3Dash.AutoSize = true;
      this.labelP3Dash.Location = new Point(82, 92);
      this.labelP3Dash.Name = "labelP3Dash";
      this.labelP3Dash.Size = new Size(10, 13);
      this.labelP3Dash.TabIndex = 44;
      this.labelP3Dash.Text = "-";
      this.txtP3Year2.Location = new Point(92, 89);
      this.txtP3Year2.MaxLength = 2;
      this.txtP3Year2.Name = "txtP3Year2";
      this.txtP3Year2.Size = new Size(41, 20);
      this.txtP3Year2.TabIndex = 23;
      this.txtP3Year2.Tag = (object) "CD1.X25";
      this.txtP3Year2.TextAlign = HorizontalAlignment.Right;
      this.labelP3Max.AutoSize = true;
      this.labelP3Max.Location = new Point(3, 245);
      this.labelP3Max.Name = "labelP3Max";
      this.labelP3Max.Size = new Size(27, 13);
      this.labelP3Max.TabIndex = 42;
      this.labelP3Max.Text = "Max";
      this.labelP3Min.AutoSize = true;
      this.labelP3Min.Location = new Point(3, 223);
      this.labelP3Min.Name = "labelP3Min";
      this.labelP3Min.Size = new Size(24, 13);
      this.labelP3Min.TabIndex = 41;
      this.labelP3Min.Text = "Min";
      this.txtP3MaxTotal.Location = new Point(41, 242);
      this.txtP3MaxTotal.Name = "txtP3MaxTotal";
      this.txtP3MaxTotal.ReadOnly = true;
      this.txtP3MaxTotal.Size = new Size(92, 20);
      this.txtP3MaxTotal.TabIndex = 40;
      this.txtP3MaxTotal.TabStop = false;
      this.txtP3MaxTotal.Tag = (object) "CD1.X32";
      this.txtP3MaxTotal.TextAlign = HorizontalAlignment.Right;
      this.txtP3MinTotal.Location = new Point(41, 220);
      this.txtP3MinTotal.Name = "txtP3MinTotal";
      this.txtP3MinTotal.ReadOnly = true;
      this.txtP3MinTotal.Size = new Size(92, 20);
      this.txtP3MinTotal.TabIndex = 39;
      this.txtP3MinTotal.TabStop = false;
      this.txtP3MinTotal.Tag = (object) "CD1.X31";
      this.txtP3MinTotal.TextAlign = HorizontalAlignment.Right;
      this.txtP3Escrow.Location = new Point(41, 198);
      this.txtP3Escrow.MaxLength = 15;
      this.txtP3Escrow.Name = "txtP3Escrow";
      this.txtP3Escrow.Size = new Size(92, 20);
      this.txtP3Escrow.TabIndex = 28;
      this.txtP3Escrow.Tag = (object) "CD1.X30";
      this.txtP3Escrow.TextAlign = HorizontalAlignment.Right;
      this.txtP3Escrow.KeyUp += new KeyEventHandler(this.txtEscrow_KeyUp);
      this.txtP3MI.Location = new Point(41, 176);
      this.txtP3MI.MaxLength = 8;
      this.txtP3MI.Name = "txtP3MI";
      this.txtP3MI.Size = new Size(92, 20);
      this.txtP3MI.TabIndex = 27;
      this.txtP3MI.Tag = (object) "CD1.X29";
      this.txtP3MI.TextAlign = HorizontalAlignment.Right;
      this.txtP3MI.KeyUp += new KeyEventHandler(this.txtMI_KeyUp);
      this.chkP3Only.AutoSize = true;
      this.chkP3Only.Location = new Point(42, 157);
      this.chkP3Only.Name = "chkP3Only";
      this.chkP3Only.Size = new Size(85, 17);
      this.chkP3Only.TabIndex = 26;
      this.chkP3Only.Tag = (object) "CD1.X28";
      this.chkP3Only.Text = "Interest Only";
      this.chkP3Only.UseVisualStyleBackColor = true;
      this.chkP3Only.CheckedChanged += new EventHandler(this.interestOnly_Checked);
      this.txtP3MaxPI.Location = new Point(41, 133);
      this.txtP3MaxPI.MaxLength = 12;
      this.txtP3MaxPI.Name = "txtP3MaxPI";
      this.txtP3MaxPI.Size = new Size(92, 20);
      this.txtP3MaxPI.TabIndex = 25;
      this.txtP3MaxPI.Tag = (object) "CD1.X27";
      this.txtP3MaxPI.TextAlign = HorizontalAlignment.Right;
      this.txtP3MaxPI.KeyUp += new KeyEventHandler(this.txtMinMax_KeyUp);
      this.labelP3MaxPI.AutoSize = true;
      this.labelP3MaxPI.Location = new Point(3, 136);
      this.labelP3MaxPI.Name = "labelP3MaxPI";
      this.labelP3MaxPI.Size = new Size(27, 13);
      this.labelP3MaxPI.TabIndex = 34;
      this.labelP3MaxPI.Text = "Max";
      this.txtP3MinPI.Location = new Point(41, 111);
      this.txtP3MinPI.MaxLength = 12;
      this.txtP3MinPI.Name = "txtP3MinPI";
      this.txtP3MinPI.Size = new Size(92, 20);
      this.txtP3MinPI.TabIndex = 24;
      this.txtP3MinPI.Tag = (object) "CD1.X26";
      this.txtP3MinPI.TextAlign = HorizontalAlignment.Right;
      this.txtP3MinPI.KeyUp += new KeyEventHandler(this.txtMinMax_KeyUp);
      this.labelP3MinPI.AutoSize = true;
      this.labelP3MinPI.Location = new Point(3, 114);
      this.labelP3MinPI.Name = "labelP3MinPI";
      this.labelP3MinPI.Size = new Size(24, 13);
      this.labelP3MinPI.TabIndex = 32;
      this.labelP3MinPI.Text = "Min";
      this.txtP3Year1.Location = new Point(40, 89);
      this.txtP3Year1.MaxLength = 2;
      this.txtP3Year1.Name = "txtP3Year1";
      this.txtP3Year1.Size = new Size(41, 20);
      this.txtP3Year1.TabIndex = 22;
      this.txtP3Year1.Tag = (object) "CD1.X24";
      this.txtP3Year1.TextAlign = HorizontalAlignment.Right;
      this.labelP3Year.AutoSize = true;
      this.labelP3Year.Location = new Point(4, 92);
      this.labelP3Year.Name = "labelP3Year";
      this.labelP3Year.Size = new Size(34, 13);
      this.labelP3Year.TabIndex = 30;
      this.labelP3Year.Text = "Years";
      this.chkRangedYears3.AutoSize = true;
      this.chkRangedYears3.Location = new Point(6, 67);
      this.chkRangedYears3.Name = "chkRangedYears3";
      this.chkRangedYears3.Size = new Size(106, 17);
      this.chkRangedYears3.TabIndex = 21;
      this.chkRangedYears3.Text = "More than a year";
      this.chkRangedYears3.UseVisualStyleBackColor = true;
      this.chkRangedYears3.CheckedChanged += new EventHandler(this.rangedYears_Checked);
      this.chkFinalPayment3.AutoSize = true;
      this.chkFinalPayment3.Location = new Point(6, 48);
      this.chkFinalPayment3.Name = "chkFinalPayment3";
      this.chkFinalPayment3.Size = new Size(92, 17);
      this.chkFinalPayment3.TabIndex = 20;
      this.chkFinalPayment3.Text = "Final Payment";
      this.chkFinalPayment3.UseVisualStyleBackColor = true;
      this.chkFinalPayment3.CheckedChanged += new EventHandler(this.finalPayment_Checked);
      this.chkRangedPayment3.AutoSize = true;
      this.chkRangedPayment3.Location = new Point(6, 29);
      this.chkRangedPayment3.Name = "chkRangedPayment3";
      this.chkRangedPayment3.Size = new Size(77, 17);
      this.chkRangedPayment3.TabIndex = 19;
      this.chkRangedPayment3.Text = "P&&I Range";
      this.chkRangedPayment3.UseVisualStyleBackColor = true;
      this.chkRangedPayment3.CheckedChanged += new EventHandler(this.rangedPayment_Checked);
      this.grpCol4.Borders = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.grpCol4.Controls.Add((Control) this.labelP4Dash);
      this.grpCol4.Controls.Add((Control) this.txtP4Year2);
      this.grpCol4.Controls.Add((Control) this.labelP4Max);
      this.grpCol4.Controls.Add((Control) this.labelP4Min);
      this.grpCol4.Controls.Add((Control) this.txtP4MaxTotal);
      this.grpCol4.Controls.Add((Control) this.txtP4MinTotal);
      this.grpCol4.Controls.Add((Control) this.txtP4Escrow);
      this.grpCol4.Controls.Add((Control) this.txtP4MI);
      this.grpCol4.Controls.Add((Control) this.chkP4Only);
      this.grpCol4.Controls.Add((Control) this.txtP4MaxPI);
      this.grpCol4.Controls.Add((Control) this.labelP4MaxPI);
      this.grpCol4.Controls.Add((Control) this.txtP4MinPI);
      this.grpCol4.Controls.Add((Control) this.labelP4MinPI);
      this.grpCol4.Controls.Add((Control) this.txtP4Year1);
      this.grpCol4.Controls.Add((Control) this.labelP4Year);
      this.grpCol4.Controls.Add((Control) this.chkRangedYears4);
      this.grpCol4.Controls.Add((Control) this.chkFinalPayment4);
      this.grpCol4.Controls.Add((Control) this.chkRangedPayment4);
      this.grpCol4.HeaderForeColor = SystemColors.ControlText;
      this.grpCol4.Location = new Point(611, 0);
      this.grpCol4.Name = "grpCol4";
      this.grpCol4.Size = new Size(142, 271);
      this.grpCol4.TabIndex = 20;
      this.grpCol4.Text = "            4th Period";
      this.labelP4Dash.AutoSize = true;
      this.labelP4Dash.Location = new Point(82, 92);
      this.labelP4Dash.Name = "labelP4Dash";
      this.labelP4Dash.Size = new Size(10, 13);
      this.labelP4Dash.TabIndex = 44;
      this.labelP4Dash.Text = "-";
      this.txtP4Year2.Location = new Point(92, 89);
      this.txtP4Year2.MaxLength = 2;
      this.txtP4Year2.Name = "txtP4Year2";
      this.txtP4Year2.Size = new Size(41, 20);
      this.txtP4Year2.TabIndex = 33;
      this.txtP4Year2.Tag = (object) "CD1.X34";
      this.txtP4Year2.TextAlign = HorizontalAlignment.Right;
      this.labelP4Max.AutoSize = true;
      this.labelP4Max.Location = new Point(3, 245);
      this.labelP4Max.Name = "labelP4Max";
      this.labelP4Max.Size = new Size(27, 13);
      this.labelP4Max.TabIndex = 42;
      this.labelP4Max.Text = "Max";
      this.labelP4Min.AutoSize = true;
      this.labelP4Min.Location = new Point(3, 223);
      this.labelP4Min.Name = "labelP4Min";
      this.labelP4Min.Size = new Size(24, 13);
      this.labelP4Min.TabIndex = 41;
      this.labelP4Min.Text = "Min";
      this.txtP4MaxTotal.Location = new Point(41, 242);
      this.txtP4MaxTotal.Name = "txtP4MaxTotal";
      this.txtP4MaxTotal.ReadOnly = true;
      this.txtP4MaxTotal.Size = new Size(92, 20);
      this.txtP4MaxTotal.TabIndex = 40;
      this.txtP4MaxTotal.TabStop = false;
      this.txtP4MaxTotal.Tag = (object) "CD1.X41";
      this.txtP4MaxTotal.TextAlign = HorizontalAlignment.Right;
      this.txtP4MinTotal.Location = new Point(41, 220);
      this.txtP4MinTotal.Name = "txtP4MinTotal";
      this.txtP4MinTotal.ReadOnly = true;
      this.txtP4MinTotal.Size = new Size(92, 20);
      this.txtP4MinTotal.TabIndex = 39;
      this.txtP4MinTotal.TabStop = false;
      this.txtP4MinTotal.Tag = (object) "CD1.X40";
      this.txtP4MinTotal.TextAlign = HorizontalAlignment.Right;
      this.txtP4Escrow.Location = new Point(41, 198);
      this.txtP4Escrow.MaxLength = 15;
      this.txtP4Escrow.Name = "txtP4Escrow";
      this.txtP4Escrow.Size = new Size(92, 20);
      this.txtP4Escrow.TabIndex = 38;
      this.txtP4Escrow.Tag = (object) "CD1.X39";
      this.txtP4Escrow.TextAlign = HorizontalAlignment.Right;
      this.txtP4Escrow.KeyUp += new KeyEventHandler(this.txtEscrow_KeyUp);
      this.txtP4MI.Location = new Point(41, 176);
      this.txtP4MI.MaxLength = 8;
      this.txtP4MI.Name = "txtP4MI";
      this.txtP4MI.Size = new Size(92, 20);
      this.txtP4MI.TabIndex = 37;
      this.txtP4MI.Tag = (object) "CD1.X38";
      this.txtP4MI.TextAlign = HorizontalAlignment.Right;
      this.txtP4MI.KeyUp += new KeyEventHandler(this.txtMI_KeyUp);
      this.chkP4Only.AutoSize = true;
      this.chkP4Only.Location = new Point(42, 157);
      this.chkP4Only.Name = "chkP4Only";
      this.chkP4Only.Size = new Size(85, 17);
      this.chkP4Only.TabIndex = 36;
      this.chkP4Only.Tag = (object) "CD1.X37";
      this.chkP4Only.Text = "Interest Only";
      this.chkP4Only.UseVisualStyleBackColor = true;
      this.chkP4Only.CheckedChanged += new EventHandler(this.interestOnly_Checked);
      this.txtP4MaxPI.Location = new Point(41, 133);
      this.txtP4MaxPI.MaxLength = 12;
      this.txtP4MaxPI.Name = "txtP4MaxPI";
      this.txtP4MaxPI.Size = new Size(92, 20);
      this.txtP4MaxPI.TabIndex = 35;
      this.txtP4MaxPI.Tag = (object) "CD1.X36";
      this.txtP4MaxPI.TextAlign = HorizontalAlignment.Right;
      this.txtP4MaxPI.KeyUp += new KeyEventHandler(this.txtMinMax_KeyUp);
      this.labelP4MaxPI.AutoSize = true;
      this.labelP4MaxPI.Location = new Point(3, 136);
      this.labelP4MaxPI.Name = "labelP4MaxPI";
      this.labelP4MaxPI.Size = new Size(27, 13);
      this.labelP4MaxPI.TabIndex = 34;
      this.labelP4MaxPI.Text = "Max";
      this.txtP4MinPI.Location = new Point(41, 111);
      this.txtP4MinPI.MaxLength = 12;
      this.txtP4MinPI.Name = "txtP4MinPI";
      this.txtP4MinPI.Size = new Size(92, 20);
      this.txtP4MinPI.TabIndex = 34;
      this.txtP4MinPI.Tag = (object) "CD1.X35";
      this.txtP4MinPI.TextAlign = HorizontalAlignment.Right;
      this.txtP4MinPI.KeyUp += new KeyEventHandler(this.txtMinMax_KeyUp);
      this.labelP4MinPI.AutoSize = true;
      this.labelP4MinPI.Location = new Point(3, 114);
      this.labelP4MinPI.Name = "labelP4MinPI";
      this.labelP4MinPI.Size = new Size(24, 13);
      this.labelP4MinPI.TabIndex = 32;
      this.labelP4MinPI.Text = "Min";
      this.txtP4Year1.Location = new Point(40, 89);
      this.txtP4Year1.MaxLength = 2;
      this.txtP4Year1.Name = "txtP4Year1";
      this.txtP4Year1.Size = new Size(41, 20);
      this.txtP4Year1.TabIndex = 32;
      this.txtP4Year1.Tag = (object) "CD1.X33";
      this.txtP4Year1.TextAlign = HorizontalAlignment.Right;
      this.labelP4Year.AutoSize = true;
      this.labelP4Year.Location = new Point(3, 90);
      this.labelP4Year.Name = "labelP4Year";
      this.labelP4Year.Size = new Size(34, 13);
      this.labelP4Year.TabIndex = 30;
      this.labelP4Year.Text = "Years";
      this.chkRangedYears4.AutoSize = true;
      this.chkRangedYears4.Location = new Point(6, 67);
      this.chkRangedYears4.Name = "chkRangedYears4";
      this.chkRangedYears4.Size = new Size(106, 17);
      this.chkRangedYears4.TabIndex = 31;
      this.chkRangedYears4.Text = "More than a year";
      this.chkRangedYears4.UseVisualStyleBackColor = true;
      this.chkRangedYears4.CheckedChanged += new EventHandler(this.rangedYears_Checked);
      this.chkFinalPayment4.AutoSize = true;
      this.chkFinalPayment4.Location = new Point(6, 48);
      this.chkFinalPayment4.Name = "chkFinalPayment4";
      this.chkFinalPayment4.Size = new Size(92, 17);
      this.chkFinalPayment4.TabIndex = 30;
      this.chkFinalPayment4.Text = "Final Payment";
      this.chkFinalPayment4.UseVisualStyleBackColor = true;
      this.chkFinalPayment4.CheckedChanged += new EventHandler(this.finalPayment_Checked);
      this.chkRangedPayment4.AutoSize = true;
      this.chkRangedPayment4.Location = new Point(6, 29);
      this.chkRangedPayment4.Name = "chkRangedPayment4";
      this.chkRangedPayment4.Size = new Size(77, 17);
      this.chkRangedPayment4.TabIndex = 29;
      this.chkRangedPayment4.Text = "P&&I Range";
      this.chkRangedPayment4.UseVisualStyleBackColor = true;
      this.chkRangedPayment4.CheckedChanged += new EventHandler(this.rangedPayment_Checked);
      this.pboxDownArrow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(194, 320);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 74;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(159, 322);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 72;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "CustomizeProjectedPayments";
      this.emHelpLink1.Location = new Point(12, 321);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 73;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(773, 353);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.panelAllColumns);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboNoColumns);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditProjectedPaymentTable);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Edit Projected Payments Table";
      this.KeyPress += new KeyPressEventHandler(this.EditProjectedPaymentTable_KeyPress);
      this.panelAllColumns.ResumeLayout(false);
      this.grpCol0.ResumeLayout(false);
      this.grpCol0.PerformLayout();
      this.grpCol1.ResumeLayout(false);
      this.grpCol1.PerformLayout();
      this.grpCol2.ResumeLayout(false);
      this.grpCol2.PerformLayout();
      this.grpCol3.ResumeLayout(false);
      this.grpCol3.PerformLayout();
      this.grpCol4.ResumeLayout(false);
      this.grpCol4.PerformLayout();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
