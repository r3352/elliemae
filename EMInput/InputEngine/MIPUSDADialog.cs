// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MIPUSDADialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
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
  public class MIPUSDADialog : Form
  {
    private Sessions.Session session = Session.DefaultInstance;
    private IHtmlInput inputData;
    private LoanData loan;
    private int priceType;
    private PopupBusinessRules popupRules;
    private bool inTemplate;
    private bool needLoanAmountRounding;
    private IContainer components;
    private GroupContainer groupBoxTop;
    private Label label1;
    private TextBox txtAppraisal;
    private Label label2;
    private TextBox txtBaseLoanAmount;
    private Label label4;
    private ToolTip fieldToolTip;
    private Panel pnlBody;
    private PictureBox pboxAsterisk;
    private PictureBox pboxDownArrow;
    private Button cancelBtn;
    private Button okBtn;
    private EMHelpLink emHelpLink1;
    private GroupContainer groupContainer2;
    private CheckBox chkPortion;
    private Label label3;
    private TextBox txtPortionTotalLoanAmount;
    private TextBox txtPortionFinancedAmount;
    private Label label16;
    private GroupContainer groupContainer1;
    private CheckBox chkAll;
    private Label label5;
    private TextBox txtAllTotalAmount;
    private TextBox txtAllFee;
    private Label label6;
    private TextBox txtPortionFee;
    private Label label17;
    private GroupContainer groupContainer3;
    private CheckBox chkDeclining;
    private Label label15;
    private CheckBox midpointChkbox;
    private Label label7;
    private CheckBox balanceChkbox;
    private Label label8;
    private Label label9;
    private ComboBox typeCombo;
    private Label label10;
    private TextBox rateMICancelTxt;
    private Label label11;
    private TextBox monthMI2Txt;
    private TextBox rateMI1Txt;
    private TextBox monthMI1Txt;
    private TextBox rateMI2Txt;
    private Label label14;
    private Label label12;
    private Label label13;
    private Label label18;
    private ComboBox cboUSDAPercent;

    public MIPUSDADialog(IHtmlInput inputData)
    {
      this.inputData = inputData;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      else
        this.inTemplate = true;
      this.InitializeComponent();
      if (!this.inTemplate)
        this.needLoanAmountRounding = inputData.GetField("4745") == "Y";
      this.initForm();
      this.fieldToolTip.SetToolTip((Control) this.txtAppraisal, "356");
      this.fieldToolTip.SetToolTip((Control) this.txtBaseLoanAmount, "1109");
      this.fieldToolTip.SetToolTip((Control) this.cboUSDAPercent, "3560");
      this.fieldToolTip.SetToolTip((Control) this.txtAllFee, "3561");
      this.fieldToolTip.SetToolTip((Control) this.txtAllTotalAmount, "3562");
      this.fieldToolTip.SetToolTip((Control) this.txtPortionFinancedAmount, "3563");
      this.fieldToolTip.SetToolTip((Control) this.txtPortionFee, "3564");
      this.fieldToolTip.SetToolTip((Control) this.txtPortionTotalLoanAmount, "3565");
      this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1757");
      this.fieldToolTip.SetToolTip((Control) this.balanceChkbox, "1775");
      this.fieldToolTip.SetToolTip((Control) this.rateMI1Txt, "1199");
      this.fieldToolTip.SetToolTip((Control) this.monthMI1Txt, "1198");
      this.fieldToolTip.SetToolTip((Control) this.rateMI2Txt, "1201");
      this.fieldToolTip.SetToolTip((Control) this.monthMI2Txt, "1200");
      this.fieldToolTip.SetToolTip((Control) this.rateMICancelTxt, "1205");
      this.fieldToolTip.SetToolTip((Control) this.chkDeclining, "3248");
      this.fieldToolTip.SetToolTip((Control) this.midpointChkbox, "1753");
      if (this.loan != null)
        this.setBusinessRule();
      if (this.inTemplate)
        this.txtAppraisal.ReadOnly = this.txtBaseLoanAmount.ReadOnly = this.txtPortionFinancedAmount.ReadOnly = true;
      if (!this.chkPortion.Checked)
        this.txtPortionFinancedAmount.ReadOnly = true;
      if (this.cboUSDAPercent.SelectedIndex != 0 || this.chkAll.Checked || this.chkPortion.Checked)
        return;
      this.okBtn.Enabled = false;
    }

    private void initForm()
    {
      this.txtAppraisal.Text = this.inputData.GetField("356");
      this.reformatValue(this.txtAppraisal, false);
      if (this.inputData.GetField("1109") != string.Empty)
        this.txtBaseLoanAmount.Text = this.needLoanAmountRounding ? Utils.ParseInt((object) this.inputData.GetField("1109")).ToString("0") : this.inputData.GetField("1109");
      this.reformatValue(this.txtBaseLoanAmount, false);
      this.cboUSDAPercent.Text = this.inputData.GetField("3560");
      this.txtAllFee.Text = this.inputData.GetField("3561");
      this.reformatValue(this.txtAllFee, false);
      this.txtAllTotalAmount.Text = this.inputData.GetField("3562");
      this.reformatValue(this.txtAllTotalAmount, false);
      this.txtPortionFinancedAmount.Text = this.inputData.GetField("3563");
      this.reformatValue(this.txtPortionFinancedAmount, false);
      this.txtPortionFee.Text = this.inputData.GetField("3564");
      this.reformatValue(this.txtPortionFee, false);
      this.txtPortionTotalLoanAmount.Text = this.inputData.GetField("3565");
      this.reformatValue(this.txtPortionTotalLoanAmount, false);
      this.chkAll.Checked = this.inputData.GetField("3566") == "FinancingAll";
      this.chkPortion.Checked = this.inputData.GetField("3566") == "FinancingPortion";
      this.balanceChkbox.Checked = true;
      this.inputData.GetField("1757");
      switch ("Loan Amount")
      {
        case "Purchase Price":
          this.priceType = 1;
          break;
        case "Appraisal Value":
          this.priceType = 2;
          break;
        case "Base Loan Amount":
          this.priceType = 3;
          break;
        default:
          this.priceType = 0;
          break;
      }
      if (this.typeCombo.Items.Count >= this.priceType)
        this.typeCombo.SelectedIndex = this.priceType;
      if (Utils.ParseDouble((object) this.inputData.GetField("1199")) != 0.0)
        this.rateMI1Txt.Text = this.inputData.GetField("1199");
      this.monthMI1Txt.Text = this.inputData.GetField("1198");
      if (Utils.ParseDouble((object) this.inputData.GetField("1201")) != 0.0)
        this.rateMI2Txt.Text = this.inputData.GetField("1201");
      this.monthMI2Txt.Text = this.inputData.GetField("1200");
      this.rateMICancelTxt.Text = this.inputData.GetField("1205");
      this.chkDeclining.Checked = false;
      this.midpointChkbox.Checked = this.chkDeclining.Checked = false;
      this.rateMICancelTxt.Enabled = this.midpointChkbox.Enabled = this.chkDeclining.Enabled = false;
    }

    private void setBusinessRule()
    {
      ResourceManager resources = new ResourceManager(typeof (MIPDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      this.popupRules.SetBusinessRules((object) this.txtAppraisal, "356");
      this.popupRules.SetBusinessRules((object) this.txtBaseLoanAmount, "1109");
      this.popupRules.SetBusinessRules((object) this.cboUSDAPercent, "3560");
      this.popupRules.SetBusinessRules((object) this.txtAllFee, "3561");
      this.popupRules.SetBusinessRules((object) this.txtAllTotalAmount, "3562");
      this.popupRules.SetBusinessRules((object) this.txtPortionFinancedAmount, "3563");
      this.popupRules.SetBusinessRules((object) this.txtPortionFee, "3564");
      this.popupRules.SetBusinessRules((object) this.txtPortionTotalLoanAmount, "3565");
      this.popupRules.SetBusinessRules((object) this.chkAll, "3566");
      this.popupRules.SetBusinessRules((object) this.chkPortion, "3566");
      this.popupRules.SetBusinessRules((object) this.rateMI1Txt, "1199");
      this.popupRules.SetBusinessRules((object) this.monthMI1Txt, "1198");
      this.popupRules.SetBusinessRules((object) this.rateMI2Txt, "1201");
      this.popupRules.SetBusinessRules((object) this.monthMI2Txt, "1200");
      this.popupRules.SetBusinessRules((object) this.rateMICancelTxt, "1205");
      this.popupRules.SetBusinessRules((object) this.midpointChkbox, "1753");
      this.popupRules.SetBusinessRules((object) this.typeCombo, "1757");
      this.popupRules.SetBusinessRules((object) this.balanceChkbox, "1775");
      this.popupRules.SetBusinessRules((object) this.chkDeclining, "3248");
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender != null && sender is TextBox)
      {
        TextBox textBox = (TextBox) sender;
        if (!textBox.ReadOnly)
        {
          if (!this.inTemplate && !this.popupRules.RuleValidate((object) textBox, (string) textBox.Tag))
            return;
          this.reformatValue(textBox, false);
        }
      }
      this.calculateFields();
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox != null && textBox.Name == "txtPrepaidMonth")
      {
        if (char.IsDigit(e.KeyChar))
          e.Handled = false;
        else
          e.Handled = true;
      }
      else
      {
        char keyChar;
        if (textBox != null && textBox.Name == "txtBaseLoanAmount" && this.needLoanAmountRounding)
        {
          keyChar = e.KeyChar;
          if (keyChar.Equals('.'))
          {
            e.Handled = true;
            return;
          }
        }
        if (!char.IsDigit(e.KeyChar))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('.'))
          {
            e.Handled = true;
            return;
          }
        }
        e.Handled = false;
      }
    }

    private void keyup(object sender, KeyEventArgs e) => this.reformatValue((TextBox) sender, true);

    private void reformatValue(TextBox box, bool setSelectionStart)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_3;
      if (box.Name == "txtBaseLoanAmount")
        dataFormat = !this.needLoanAmountRounding ? FieldFormat.DECIMAL_2 : FieldFormat.INTEGER;
      else if (box.Name == "txtAppraisal" || box.Name == "monthMI1Txt" || box.Name == "monthMI2Txt")
        dataFormat = FieldFormat.INTEGER;
      else if (box.Name == "txtPortionFinancedAmount")
        dataFormat = FieldFormat.DECIMAL_2;
      else if (box.Name == "rateMI1Txt" || box.Name == "rateMI2Txt" || box.Name == "rateFundingTxt")
        dataFormat = FieldFormat.DECIMAL_6;
      bool needsUpdate = false;
      string str = Utils.FormatInput(box.Text, dataFormat, ref needsUpdate);
      if (needsUpdate)
      {
        box.Text = str;
        if (setSelectionStart)
          box.SelectionStart = str.Length;
      }
      if (setSelectionStart)
        return;
      if (Utils.ParseDouble((object) str) == 0.0)
      {
        box.Text = string.Empty;
      }
      else
      {
        switch (dataFormat)
        {
          case FieldFormat.INTEGER:
            box.Text = Utils.ParseDouble((object) str).ToString("N0");
            break;
          case FieldFormat.DECIMAL_2:
            box.Text = Utils.ParseDouble((object) str).ToString("N2");
            break;
          case FieldFormat.DECIMAL_3:
            box.Text = Utils.ParseDouble((object) str).ToString("N3");
            break;
          case FieldFormat.DECIMAL_6:
            box.Text = Utils.ParseDouble((object) str).ToString("N6");
            break;
        }
      }
    }

    private void calculateFields()
    {
      double num1 = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.txtBaseLoanAmount.Text) / (1.0 - Utils.ParseDouble((object) this.cboUSDAPercent.Text) / 100.0), 2);
      double num2 = Utils.ArithmeticRounding(num1 * Utils.ParseDouble((object) this.cboUSDAPercent.Text) / 100.0, 2);
      double num3 = Utils.ParseDouble((object) this.txtBaseLoanAmount.Text) + num2;
      if (this.chkAll.Checked)
      {
        this.txtAllTotalAmount.Text = num1 != 0.0 ? num1.ToString("N2") : "";
        this.txtAllFee.Text = num2 != 0.0 ? num2.ToString("N2") : "";
      }
      else
      {
        if (!this.chkPortion.Checked)
          return;
        double num4 = Utils.ParseDouble((object) this.txtBaseLoanAmount.Text) + Utils.ParseDouble((object) this.txtPortionFinancedAmount.Text);
        this.txtPortionTotalLoanAmount.Text = num4 != 0.0 ? num4.ToString("N2") : "";
        double num5 = Utils.ArithmeticRounding(num4 * Utils.ParseDouble((object) this.cboUSDAPercent.Text) / 100.0, 2);
        this.txtPortionFee.Text = num5 != 0.0 ? num5.ToString("N2") : "";
      }
    }

    private void chkAll_CheckedChanged(object sender, EventArgs e)
    {
      this.calculateFields();
      if (this.chkAll.Checked)
      {
        this.chkPortion.Checked = false;
        this.txtPortionFinancedAmount.ReadOnly = true;
        this.txtPortionFinancedAmount.Text = "";
        this.txtPortionFee.Text = "";
        this.txtPortionTotalLoanAmount.Text = "";
      }
      else
      {
        this.txtPortionFinancedAmount.ReadOnly = false;
        this.chkAll.Checked = false;
        this.txtAllFee.Text = "";
        this.txtAllTotalAmount.Text = "";
      }
      this.displayFieldID(this.chkAll.Tag.ToString());
      this.okBtn.Enabled = true;
    }

    private void chkPortion_CheckedChanged(object sender, EventArgs e)
    {
      this.calculateFields();
      if (this.chkPortion.Checked)
      {
        this.txtPortionFinancedAmount.ReadOnly = false;
        this.chkAll.Checked = false;
        this.txtAllFee.Text = "";
        this.txtAllTotalAmount.Text = "";
      }
      else
      {
        this.txtPortionFinancedAmount.ReadOnly = true;
        this.txtPortionFinancedAmount.Text = "";
        this.txtPortionFee.Text = "";
        this.txtPortionTotalLoanAmount.Text = "";
      }
      this.displayFieldID(this.chkPortion.Tag.ToString());
      this.okBtn.Enabled = true;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.inTemplate)
        this.saveData();
      else if (!this.inputData.IsLocked("NEWHUD.X1301") && string.Compare(this.inputData.GetField("NEWHUD.X1299"), "guarantee fee", true) != 0 && (this.inputData.GetField("NEWHUD.X1299") != string.Empty || this.inputData.GetField("NEWHUD.X1300") != string.Empty || this.inputData.GetField("NEWHUD.X1301") != string.Empty || this.inputData.GetField("NEWHUD.X1302") != string.Empty))
      {
        if (Utils.Dialog((IWin32Window) this, "The Guarantee Fee cannot be populated to line 819 in " + (Utils.CheckIf2015RespaTila(this.inputData.GetField("3969")) ? "2015 Itemization" : "2010 Itemization") + " because this line is not blank. Do you want to overwrite it?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
          this.saveData();
        else
          this.DialogResult = DialogResult.Cancel;
      }
      else
        this.saveData();
    }

    private void saveData()
    {
      bool flag = this.inputData.IsLocked("NEWHUD.X1301");
      double num1 = 0.0;
      if (this.loan != null)
        num1 = !this.loan.IsLocked("NEWHUD.X1301") ? Utils.ParseDouble((object) this.loan.GetField("NEWHUD.X1302")) : Utils.ParseDouble((object) this.loan.GetField("562"));
      double num2 = Utils.ParseDouble((object) this.txtBaseLoanAmount.Text);
      double num3 = 0.0;
      switch (this.typeCombo.SelectedIndex)
      {
        case 0:
          this.inputData.SetCurrentField("1757", "Loan Amount");
          break;
        case 1:
          this.inputData.SetCurrentField("1757", "Purchase Price");
          if (this.loan != null)
          {
            num3 = Utils.ParseDouble((object) this.inputData.GetField("136"));
            break;
          }
          break;
        case 2:
          this.inputData.SetCurrentField("1757", "Appraisal Value");
          if (this.loan != null)
          {
            num3 = Utils.ParseDouble((object) this.inputData.GetField("356"));
            break;
          }
          break;
        case 3:
          this.inputData.SetCurrentField("1757", "Base Loan Amount");
          if (this.loan != null)
          {
            num3 = num2;
            break;
          }
          break;
      }
      this.inputData.SetCurrentField("1775", "Y");
      if (!this.inTemplate)
      {
        this.inputData.SetCurrentField("356", this.txtAppraisal.Text);
        this.inputData.SetCurrentField("1109", num2 != 0.0 ? num2.ToString("N2") : "");
        this.inputData.SetCurrentField("3561", this.txtAllFee.Text);
        this.inputData.SetCurrentField("3562", this.txtAllTotalAmount.Text);
        this.inputData.SetCurrentField("3563", this.txtPortionFinancedAmount.Text);
        this.inputData.SetCurrentField("3564", this.txtPortionFee.Text);
        this.inputData.SetCurrentField("3565", this.txtPortionTotalLoanAmount.Text);
        this.inputData.SetCurrentField("1806", "Base Loan Amount");
        this.inputData.SetCurrentField("1807", "");
      }
      this.inputData.SetCurrentField("3560", this.cboUSDAPercent.Text);
      this.inputData.SetCurrentField("3566", this.chkAll.Checked ? "FinancingAll" : (this.chkPortion.Checked ? "FinancingPortion" : ""));
      this.inputData.SetCurrentField("1199", this.rateMI1Txt.Text);
      this.inputData.SetCurrentField("1198", this.monthMI1Txt.Text);
      this.inputData.SetCurrentField("1201", this.rateMI2Txt.Text);
      this.inputData.SetCurrentField("1200", this.monthMI2Txt.Text);
      this.inputData.SetCurrentField("1205", "");
      this.inputData.SetCurrentField("3248", "");
      this.inputData.SetCurrentField("1753", "");
      double num4 = 0.0;
      double num5 = 0.0;
      double num6 = Utils.ArithmeticRounding(Utils.ArithmeticRounding((!this.chkAll.Checked ? Utils.ParseDouble((object) this.txtPortionFinancedAmount.Text) : Utils.ParseDouble((object) this.txtAllFee.Text)) % 1.0, 2), 2);
      double num7 = 0.0;
      if (this.chkAll.Checked)
      {
        num4 = num5 = Utils.ParseDouble((object) this.txtAllFee.Text);
        num7 = num4 - num6;
        this.inputData.SetCurrentField("1760", num6 != 0.0 ? num6.ToString("N2") : "");
        this.inputData.SetCurrentField("1765", "");
        if (this.typeCombo.SelectedIndex == 0)
          num3 = Utils.ParseDouble((object) this.txtAllTotalAmount.Text);
      }
      else if (this.chkPortion.Checked)
      {
        num5 = Utils.ParseDouble((object) this.txtPortionFee.Text);
        num6 = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.txtPortionFee.Text) - Utils.ParseDouble((object) this.txtPortionFinancedAmount.Text), 2);
        this.inputData.SetCurrentField("1760", num6 != 0.0 ? num6.ToString("N2") : "");
        this.inputData.SetCurrentField("1765", "Y");
        num4 = Utils.ParseDouble((object) this.txtPortionFinancedAmount.Text);
        num6 = Utils.ParseDouble((object) this.txtPortionFee.Text) - num4 + Utils.ArithmeticRounding(Utils.ArithmeticRounding(num4 % 1.0, 2), 2);
        num7 = num5 - num6;
        if (this.typeCombo.SelectedIndex == 0)
          num3 = Utils.ParseDouble((object) this.txtPortionTotalLoanAmount.Text);
      }
      this.inputData.SetCurrentField("1826", num5 != 0.0 ? num5.ToString("N2") : "");
      if (this.loan == null)
        return;
      double num8 = num2 != 0.0 ? Utils.ParseDouble((object) (num5 / num2 * 100.0), 6.0) : 0.0;
      this.inputData.SetCurrentField("1107", num8 != 0.0 ? num8.ToString("N6") : "");
      this.inputData.SetCurrentField("1045", num4 != 0.0 ? num4.ToString("N2") : "");
      if (!this.inputData.IsLocked("969"))
        this.inputData.SetCurrentField("969", num5 != 0.0 ? num5.ToString("N2") : "");
      double num9 = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.rateMI1Txt.Text) / 100.0 * num3 / 12.0, 2);
      this.inputData.SetCurrentField("1766", num9 != 0.0 ? num9.ToString("N2") : "");
      this.inputData.SetCurrentField("NEWHUD.X1707", num9 != 0.0 ? num9.ToString("N2") : "");
      this.inputData.SetCurrentField("232", "");
      num9 = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.rateMI2Txt.Text) / 100.0 * num3 / 12.0, 2);
      this.inputData.SetCurrentField("1770", num9 != 0.0 ? num9.ToString("N2") : "");
      if (flag)
      {
        num5 -= Utils.ParseDouble((object) this.loan.GetField("562"));
        this.loan.SetCurrentField("337", num5 != 0.0 ? num5.ToString("N2") : "");
        if (this.loan.Use2015RESPA)
        {
          this.loan.SetCurrentField("NEWHUD2.X2187", num7 != 0.0 ? num7.ToString("N2") : "");
          this.loan.SetCurrentField("NEWHUD2.X2188", num6 != 0.0 ? num6.ToString("N2") : "");
        }
        if (string.Compare(this.loan.GetField("NEWHUD.X1299"), "Guarantee Fee", true) == 0)
          this.loan.Calculator.FormCalculation("CLEARLINE819", (string) null, (string) null);
      }
      else
      {
        num5 -= Utils.ParseDouble((object) this.loan.GetField("NEWHUD.X1302"));
        if (string.Compare(this.loan.GetField("NEWHUD.X1299"), "Guarantee Fee", true) != 0 && !flag)
          this.loan.Calculator.FormCalculation("CLEARLINE819", (string) null, (string) null);
        this.loan.SetCurrentField("NEWHUD.X1299", "Guarantee Fee");
        if (this.loan.Use2015RESPA)
        {
          this.loan.SetCurrentField("NEWHUD2.X1593", num7 != 0.0 ? num7.ToString("N2") : "");
          this.loan.SetCurrentField("NEWHUD2.X1594", num6 != 0.0 ? num6.ToString("N2") : "");
        }
        this.loan.SetCurrentField("NEWHUD.X1301", num5 != 0.0 ? num5.ToString("N2") : "");
        this.loan.SetCurrentField("337", "");
      }
      this.loan.SetCurrentField("RE88395.X43", this.loan.GetField("337"));
      if (this.loan.GetField("232") != string.Empty)
        this.loan.Calculator.FormCalculation("COPYLINE1003TOLINE1010", (string) null, (string) null);
      this.loan.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X1301", false);
      this.loan.Calculator.CopyHUD2010ToGFE2010("337", false);
      this.loan.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X1707", false);
    }

    private void MIPDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void cboUSDAPercent_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.calculateFields();
      this.okBtn.Enabled = true;
    }

    private void displayFieldID(string id)
    {
      if (this.loan == null || this.loan.IsTemplate)
        return;
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(id);
    }

    private void field_Entered(object sender, EventArgs e)
    {
      Control control = (Control) sender;
      if (control == null || control.Tag == null)
        return;
      this.displayFieldID(control.Tag.ToString());
    }

    private void textField_TextChanged(object sender, EventArgs e) => this.okBtn.Enabled = true;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MIPUSDADialog));
      this.groupBoxTop = new GroupContainer();
      this.label18 = new Label();
      this.cboUSDAPercent = new ComboBox();
      this.label1 = new Label();
      this.txtAppraisal = new TextBox();
      this.label2 = new Label();
      this.txtBaseLoanAmount = new TextBox();
      this.label4 = new Label();
      this.fieldToolTip = new ToolTip(this.components);
      this.pnlBody = new Panel();
      this.groupContainer3 = new GroupContainer();
      this.chkDeclining = new CheckBox();
      this.label15 = new Label();
      this.midpointChkbox = new CheckBox();
      this.label7 = new Label();
      this.balanceChkbox = new CheckBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.typeCombo = new ComboBox();
      this.label10 = new Label();
      this.rateMICancelTxt = new TextBox();
      this.label11 = new Label();
      this.monthMI2Txt = new TextBox();
      this.rateMI1Txt = new TextBox();
      this.monthMI1Txt = new TextBox();
      this.rateMI2Txt = new TextBox();
      this.label14 = new Label();
      this.label12 = new Label();
      this.label13 = new Label();
      this.groupContainer2 = new GroupContainer();
      this.txtPortionFee = new TextBox();
      this.label17 = new Label();
      this.chkPortion = new CheckBox();
      this.label3 = new Label();
      this.txtPortionTotalLoanAmount = new TextBox();
      this.txtPortionFinancedAmount = new TextBox();
      this.label16 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.chkAll = new CheckBox();
      this.label5 = new Label();
      this.txtAllTotalAmount = new TextBox();
      this.txtAllFee = new TextBox();
      this.label6 = new Label();
      this.pboxAsterisk = new PictureBox();
      this.pboxDownArrow = new PictureBox();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.groupBoxTop.SuspendLayout();
      this.pnlBody.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      this.SuspendLayout();
      this.groupBoxTop.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxTop.Controls.Add((Control) this.label18);
      this.groupBoxTop.Controls.Add((Control) this.cboUSDAPercent);
      this.groupBoxTop.Controls.Add((Control) this.label1);
      this.groupBoxTop.Controls.Add((Control) this.txtAppraisal);
      this.groupBoxTop.Controls.Add((Control) this.label2);
      this.groupBoxTop.Controls.Add((Control) this.txtBaseLoanAmount);
      this.groupBoxTop.Controls.Add((Control) this.label4);
      this.groupBoxTop.Dock = DockStyle.Top;
      this.groupBoxTop.HeaderForeColor = SystemColors.ControlText;
      this.groupBoxTop.Location = new Point(0, 0);
      this.groupBoxTop.Name = "groupBoxTop";
      this.groupBoxTop.Size = new Size(411, 105);
      this.groupBoxTop.TabIndex = 0;
      this.groupBoxTop.Text = "Loan Information";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(383, 77);
      this.label18.Name = "label18";
      this.label18.Size = new Size(15, 13);
      this.label18.TabIndex = 41;
      this.label18.Text = "%";
      this.cboUSDAPercent.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboUSDAPercent.Items.AddRange(new object[15]
      {
        (object) "",
        (object) "0.50",
        (object) "1.00",
        (object) "1.25",
        (object) "1.50",
        (object) "1.75",
        (object) "2.00",
        (object) "2.25",
        (object) "2.50",
        (object) "2.75",
        (object) "3.00",
        (object) "3.25",
        (object) "3.50",
        (object) "3.75",
        (object) "4.00"
      });
      this.cboUSDAPercent.Location = new Point(303, 74);
      this.cboUSDAPercent.Name = "cboUSDAPercent";
      this.cboUSDAPercent.Size = new Size(78, 21);
      this.cboUSDAPercent.TabIndex = 32;
      this.cboUSDAPercent.Tag = (object) "3560";
      this.cboUSDAPercent.SelectedIndexChanged += new EventHandler(this.cboUSDAPercent_SelectedIndexChanged);
      this.cboUSDAPercent.Enter += new EventHandler(this.field_Entered);
      this.cboUSDAPercent.Leave += new EventHandler(this.leave);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(84, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Appraised Value";
      this.txtAppraisal.Location = new Point(303, 30);
      this.txtAppraisal.MaxLength = 16;
      this.txtAppraisal.Name = "txtAppraisal";
      this.txtAppraisal.Size = new Size(95, 20);
      this.txtAppraisal.TabIndex = 1;
      this.txtAppraisal.Tag = (object) "356";
      this.txtAppraisal.TextAlign = HorizontalAlignment.Right;
      this.txtAppraisal.Enter += new EventHandler(this.field_Entered);
      this.txtAppraisal.KeyPress += new KeyPressEventHandler(this.keypress);
      this.txtAppraisal.KeyUp += new KeyEventHandler(this.keyup);
      this.txtAppraisal.Leave += new EventHandler(this.leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(4, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(295, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Loan Amount (Must be less than or equal to Appraised Value)";
      this.txtBaseLoanAmount.Location = new Point(303, 52);
      this.txtBaseLoanAmount.MaxLength = 10;
      this.txtBaseLoanAmount.Name = "txtBaseLoanAmount";
      this.txtBaseLoanAmount.Size = new Size(95, 20);
      this.txtBaseLoanAmount.TabIndex = 3;
      this.txtBaseLoanAmount.Tag = (object) "1109";
      this.txtBaseLoanAmount.TextAlign = HorizontalAlignment.Right;
      this.txtBaseLoanAmount.Enter += new EventHandler(this.field_Entered);
      this.txtBaseLoanAmount.KeyPress += new KeyPressEventHandler(this.keypress);
      this.txtBaseLoanAmount.KeyUp += new KeyEventHandler(this.keyup);
      this.txtBaseLoanAmount.Leave += new EventHandler(this.leave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(4, 77);
      this.label4.Name = "label4";
      this.label4.Size = new Size(136, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Guarantee Fee Percentage";
      this.pnlBody.Controls.Add((Control) this.groupContainer3);
      this.pnlBody.Controls.Add((Control) this.groupContainer2);
      this.pnlBody.Controls.Add((Control) this.groupContainer1);
      this.pnlBody.Controls.Add((Control) this.groupBoxTop);
      this.pnlBody.Location = new Point(8, 7);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new Size(411, 526);
      this.pnlBody.TabIndex = 75;
      this.groupContainer3.Controls.Add((Control) this.chkDeclining);
      this.groupContainer3.Controls.Add((Control) this.label15);
      this.groupContainer3.Controls.Add((Control) this.midpointChkbox);
      this.groupContainer3.Controls.Add((Control) this.label7);
      this.groupContainer3.Controls.Add((Control) this.balanceChkbox);
      this.groupContainer3.Controls.Add((Control) this.label8);
      this.groupContainer3.Controls.Add((Control) this.label9);
      this.groupContainer3.Controls.Add((Control) this.typeCombo);
      this.groupContainer3.Controls.Add((Control) this.label10);
      this.groupContainer3.Controls.Add((Control) this.rateMICancelTxt);
      this.groupContainer3.Controls.Add((Control) this.label11);
      this.groupContainer3.Controls.Add((Control) this.monthMI2Txt);
      this.groupContainer3.Controls.Add((Control) this.rateMI1Txt);
      this.groupContainer3.Controls.Add((Control) this.monthMI1Txt);
      this.groupContainer3.Controls.Add((Control) this.rateMI2Txt);
      this.groupContainer3.Controls.Add((Control) this.label14);
      this.groupContainer3.Controls.Add((Control) this.label12);
      this.groupContainer3.Controls.Add((Control) this.label13);
      this.groupContainer3.Dock = DockStyle.Top;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(0, 335);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(411, 185);
      this.groupContainer3.TabIndex = 18;
      this.groupContainer3.Text = "Monthly Mortgage Insurance / USDA Annual Fee Premium";
      this.chkDeclining.Location = new Point(15, 159);
      this.chkDeclining.Name = "chkDeclining";
      this.chkDeclining.Size = new Size(235, 18);
      this.chkDeclining.TabIndex = 48;
      this.chkDeclining.Tag = (object) "3248";
      this.chkDeclining.Text = "Declining Renewals";
      this.chkDeclining.Enter += new EventHandler(this.field_Entered);
      this.chkDeclining.Leave += new EventHandler(this.leave);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(10, 33);
      this.label15.Name = "label15";
      this.label15.Size = new Size(107, 13);
      this.label15.TabIndex = 47;
      this.label15.Text = "Calculated Based On";
      this.midpointChkbox.Location = new Point(15, 140);
      this.midpointChkbox.Name = "midpointChkbox";
      this.midpointChkbox.Size = new Size(235, 16);
      this.midpointChkbox.TabIndex = 43;
      this.midpointChkbox.Tag = (object) "1753";
      this.midpointChkbox.Text = "Midpoint payment cancellation";
      this.midpointChkbox.Enter += new EventHandler(this.field_Entered);
      this.midpointChkbox.Leave += new EventHandler(this.leave);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(15, 57);
      this.label7.Name = "label7";
      this.label7.Size = new Size(16, 13);
      this.label7.TabIndex = 32;
      this.label7.Text = "1.";
      this.balanceChkbox.Location = new Point(15, 121);
      this.balanceChkbox.Name = "balanceChkbox";
      this.balanceChkbox.Size = new Size(235, 16);
      this.balanceChkbox.TabIndex = 42;
      this.balanceChkbox.Tag = (object) "1775";
      this.balanceChkbox.Text = "Calculate based on remaining balance";
      this.balanceChkbox.Enter += new EventHandler(this.field_Entered);
      this.balanceChkbox.Leave += new EventHandler(this.leave);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(15, 79);
      this.label8.Name = "label8";
      this.label8.Size = new Size(16, 13);
      this.label8.TabIndex = 35;
      this.label8.Text = "2.";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(111, 57);
      this.label9.Name = "label9";
      this.label9.Size = new Size(15, 13);
      this.label9.TabIndex = 37;
      this.label9.Text = "%";
      this.typeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.typeCombo.Enabled = false;
      this.typeCombo.Items.AddRange(new object[4]
      {
        (object) "Loan Amount",
        (object) "Purchase Price",
        (object) "Appraisal Value",
        (object) "Base Loan Amount"
      });
      this.typeCombo.Location = new Point(133, 29);
      this.typeCombo.Name = "typeCombo";
      this.typeCombo.Size = new Size(155, 21);
      this.typeCombo.TabIndex = 31;
      this.typeCombo.Tag = (object) "1757";
      this.typeCombo.Leave += new EventHandler(this.leave);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(111, 79);
      this.label10.Name = "label10";
      this.label10.Size = new Size(15, 13);
      this.label10.TabIndex = 39;
      this.label10.Text = "%";
      this.rateMICancelTxt.Location = new Point(133, 96);
      this.rateMICancelTxt.MaxLength = 7;
      this.rateMICancelTxt.Name = "rateMICancelTxt";
      this.rateMICancelTxt.Size = new Size(65, 20);
      this.rateMICancelTxt.TabIndex = 41;
      this.rateMICancelTxt.Tag = (object) "1205";
      this.rateMICancelTxt.TextAlign = HorizontalAlignment.Right;
      this.rateMICancelTxt.Enter += new EventHandler(this.field_Entered);
      this.rateMICancelTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.rateMICancelTxt.Leave += new EventHandler(this.leave);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(212, 101);
      this.label11.Name = "label11";
      this.label11.Size = new Size(15, 13);
      this.label11.TabIndex = 40;
      this.label11.Text = "%";
      this.monthMI2Txt.Location = new Point(133, 74);
      this.monthMI2Txt.MaxLength = 3;
      this.monthMI2Txt.Name = "monthMI2Txt";
      this.monthMI2Txt.Size = new Size(65, 20);
      this.monthMI2Txt.TabIndex = 38;
      this.monthMI2Txt.Tag = (object) "1200";
      this.monthMI2Txt.TextAlign = HorizontalAlignment.Right;
      this.monthMI2Txt.Enter += new EventHandler(this.field_Entered);
      this.monthMI2Txt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.monthMI2Txt.Leave += new EventHandler(this.leave);
      this.rateMI1Txt.Location = new Point(37, 52);
      this.rateMI1Txt.MaxLength = 9;
      this.rateMI1Txt.Name = "rateMI1Txt";
      this.rateMI1Txt.Size = new Size(63, 20);
      this.rateMI1Txt.TabIndex = 33;
      this.rateMI1Txt.Tag = (object) "1199";
      this.rateMI1Txt.TextAlign = HorizontalAlignment.Right;
      this.rateMI1Txt.Enter += new EventHandler(this.field_Entered);
      this.rateMI1Txt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.rateMI1Txt.Leave += new EventHandler(this.leave);
      this.monthMI1Txt.Location = new Point(133, 52);
      this.monthMI1Txt.MaxLength = 3;
      this.monthMI1Txt.Name = "monthMI1Txt";
      this.monthMI1Txt.Size = new Size(65, 20);
      this.monthMI1Txt.TabIndex = 34;
      this.monthMI1Txt.Tag = (object) "1198";
      this.monthMI1Txt.TextAlign = HorizontalAlignment.Right;
      this.monthMI1Txt.Enter += new EventHandler(this.field_Entered);
      this.monthMI1Txt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.monthMI1Txt.Leave += new EventHandler(this.leave);
      this.rateMI2Txt.Location = new Point(37, 74);
      this.rateMI2Txt.MaxLength = 9;
      this.rateMI2Txt.Name = "rateMI2Txt";
      this.rateMI2Txt.Size = new Size(63, 20);
      this.rateMI2Txt.TabIndex = 36;
      this.rateMI2Txt.Tag = (object) "1201";
      this.rateMI2Txt.TextAlign = HorizontalAlignment.Right;
      this.rateMI2Txt.Enter += new EventHandler(this.field_Entered);
      this.rateMI2Txt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.rateMI2Txt.Leave += new EventHandler(this.leave);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(69, 101);
      this.label14.Name = "label14";
      this.label14.Size = new Size(53, 13);
      this.label14.TabIndex = 46;
      this.label14.Text = "Cancel At";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(212, 57);
      this.label12.Name = "label12";
      this.label12.Size = new Size(42, 13);
      this.label12.TabIndex = 44;
      this.label12.Text = "Months";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(212, 79);
      this.label13.Name = "label13";
      this.label13.Size = new Size(42, 13);
      this.label13.TabIndex = 45;
      this.label13.Text = "Months";
      this.groupContainer2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.txtPortionFee);
      this.groupContainer2.Controls.Add((Control) this.label17);
      this.groupContainer2.Controls.Add((Control) this.chkPortion);
      this.groupContainer2.Controls.Add((Control) this.label3);
      this.groupContainer2.Controls.Add((Control) this.txtPortionTotalLoanAmount);
      this.groupContainer2.Controls.Add((Control) this.txtPortionFinancedAmount);
      this.groupContainer2.Controls.Add((Control) this.label16);
      this.groupContainer2.Dock = DockStyle.Top;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 210);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(411, 125);
      this.groupContainer2.TabIndex = 17;
      this.groupContainer2.Text = "For Financing a Portion of the Guarantee Fee";
      this.txtPortionFee.Location = new Point(303, 96);
      this.txtPortionFee.MaxLength = 10;
      this.txtPortionFee.Name = "txtPortionFee";
      this.txtPortionFee.ReadOnly = true;
      this.txtPortionFee.Size = new Size(95, 20);
      this.txtPortionFee.TabIndex = 32;
      this.txtPortionFee.Tag = (object) "3565";
      this.txtPortionFee.TextAlign = HorizontalAlignment.Right;
      this.txtPortionFee.Enter += new EventHandler(this.field_Entered);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(24, 99);
      this.label17.Name = "label17";
      this.label17.Size = new Size(117, 13);
      this.label17.TabIndex = 33;
      this.label17.Text = "Guarantee Fee Amount";
      this.chkPortion.Location = new Point(7, 32);
      this.chkPortion.Name = "chkPortion";
      this.chkPortion.Size = new Size(326, 18);
      this.chkPortion.TabIndex = 31;
      this.chkPortion.Tag = (object) "3566";
      this.chkPortion.Text = "A portion or none of the Guarantee Fee will be Financed";
      this.chkPortion.CheckedChanged += new EventHandler(this.chkPortion_CheckedChanged);
      this.chkPortion.TextChanged += new EventHandler(this.textField_TextChanged);
      this.chkPortion.Leave += new EventHandler(this.leave);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(24, 55);
      this.label3.Name = "label3";
      this.label3.Size = new Size(176, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Amount of Guarantee Fee Financed";
      this.txtPortionTotalLoanAmount.Location = new Point(303, 74);
      this.txtPortionTotalLoanAmount.MaxLength = 10;
      this.txtPortionTotalLoanAmount.Name = "txtPortionTotalLoanAmount";
      this.txtPortionTotalLoanAmount.ReadOnly = true;
      this.txtPortionTotalLoanAmount.Size = new Size(95, 20);
      this.txtPortionTotalLoanAmount.TabIndex = 5;
      this.txtPortionTotalLoanAmount.Tag = (object) "3564";
      this.txtPortionTotalLoanAmount.TextAlign = HorizontalAlignment.Right;
      this.txtPortionTotalLoanAmount.Enter += new EventHandler(this.field_Entered);
      this.txtPortionFinancedAmount.Location = new Point(303, 52);
      this.txtPortionFinancedAmount.MaxLength = 10;
      this.txtPortionFinancedAmount.Name = "txtPortionFinancedAmount";
      this.txtPortionFinancedAmount.Size = new Size(95, 20);
      this.txtPortionFinancedAmount.TabIndex = 3;
      this.txtPortionFinancedAmount.Tag = (object) "3563";
      this.txtPortionFinancedAmount.TextAlign = HorizontalAlignment.Right;
      this.txtPortionFinancedAmount.TextChanged += new EventHandler(this.textField_TextChanged);
      this.txtPortionFinancedAmount.Enter += new EventHandler(this.field_Entered);
      this.txtPortionFinancedAmount.KeyPress += new KeyPressEventHandler(this.keypress);
      this.txtPortionFinancedAmount.KeyUp += new KeyEventHandler(this.keyup);
      this.txtPortionFinancedAmount.Leave += new EventHandler(this.leave);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(24, 77);
      this.label16.Name = "label16";
      this.label16.Size = new Size(97, 13);
      this.label16.TabIndex = 9;
      this.label16.Text = "Total Loan Amount";
      this.groupContainer1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.chkAll);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.txtAllTotalAmount);
      this.groupContainer1.Controls.Add((Control) this.txtAllFee);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 105);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(411, 105);
      this.groupContainer1.TabIndex = 16;
      this.groupContainer1.Text = "Financed Guarantee Fee";
      this.chkAll.Location = new Point(7, 32);
      this.chkAll.Name = "chkAll";
      this.chkAll.Size = new Size(235, 18);
      this.chkAll.TabIndex = 31;
      this.chkAll.Tag = (object) "3566";
      this.chkAll.Text = "Entire Guarantee Fee will be financed";
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      this.chkAll.TextChanged += new EventHandler(this.textField_TextChanged);
      this.chkAll.Leave += new EventHandler(this.leave);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(24, 55);
      this.label5.Name = "label5";
      this.label5.Size = new Size(117, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Guarantee Fee Amount";
      this.txtAllTotalAmount.Location = new Point(303, 74);
      this.txtAllTotalAmount.MaxLength = 10;
      this.txtAllTotalAmount.Name = "txtAllTotalAmount";
      this.txtAllTotalAmount.ReadOnly = true;
      this.txtAllTotalAmount.Size = new Size(95, 20);
      this.txtAllTotalAmount.TabIndex = 5;
      this.txtAllTotalAmount.Tag = (object) "3562";
      this.txtAllTotalAmount.TextAlign = HorizontalAlignment.Right;
      this.txtAllTotalAmount.Enter += new EventHandler(this.field_Entered);
      this.txtAllFee.Location = new Point(303, 52);
      this.txtAllFee.MaxLength = 10;
      this.txtAllFee.Name = "txtAllFee";
      this.txtAllFee.ReadOnly = true;
      this.txtAllFee.Size = new Size(95, 20);
      this.txtAllFee.TabIndex = 3;
      this.txtAllFee.Tag = (object) "3561";
      this.txtAllFee.TextAlign = HorizontalAlignment.Right;
      this.txtAllFee.Enter += new EventHandler(this.field_Entered);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(24, 77);
      this.label6.Name = "label6";
      this.label6.Size = new Size(97, 13);
      this.label6.TabIndex = 9;
      this.label6.Text = "Total Loan Amount";
      this.pboxAsterisk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(155, 546);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 70;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.pboxDownArrow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(190, 544);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 74;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(344, 540);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 72;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(263, 540);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 71;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "MIP USDA Dialog";
      this.emHelpLink1.Location = new Point(8, 545);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 73;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(426, 571);
      this.Controls.Add((Control) this.pnlBody);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.emHelpLink1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MIPUSDADialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "MIP/PMI/Guarantee Fee Calculation";
      this.KeyPress += new KeyPressEventHandler(this.MIPDialog_KeyPress);
      this.groupBoxTop.ResumeLayout(false);
      this.groupBoxTop.PerformLayout();
      this.pnlBody.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      this.ResumeLayout(false);
    }
  }
}
