// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.RETaxesDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class RETaxesDialog : System.Windows.Forms.Form
  {
    private const string className = "RETaxesDialog";
    protected static string sw = Tracing.SwDataEngine;
    private bool inTemplate;
    private LoanData loan;
    private IHtmlInput lpLoan;
    private double amount;
    private string priceType = string.Empty;
    private double rateFactor;
    private PopupBusinessRules popupRules;
    private Sessions.Session session;
    private FeeManagementSetting feeManagementSetting;
    private FeeManagementPersonaInfo feeManagementPermission;
    private Sessions.Session sessionDefault = Session.DefaultInstance;
    private bool formLoading;
    private IContainer components;
    private System.Windows.Forms.Label label;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox amountTxt;
    private System.Windows.Forms.TextBox rateTxt;
    private System.Windows.Forms.TextBox loanTxt;
    private System.Windows.Forms.CheckBox chkTax;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button cancelBtn;
    private System.Windows.Forms.Button okBtn;
    private ComboBox typeCombo;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textBox1009;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBox1008;
    private System.Windows.Forms.TextBox textBox1007;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox textBox1003;
    private System.Windows.Forms.TextBox textBox1009Des;
    private System.Windows.Forms.TextBox textBox1008Des;
    private System.Windows.Forms.TextBox textBox1007Des;
    private ToolTip fieldToolTip;
    private System.Windows.Forms.TextBox textBoxTotal;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox textBox1004;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private BorderPanel borderPanel1;
    private BorderPanel borderPanel2;
    private BorderPanel borderPanel3;
    private System.Windows.Forms.Label label10;
    private PictureBox pb1628;
    private PictureBox pb660;
    private PictureBox pb661;

    internal RETaxesDialog(
      string caption,
      IHtmlInput lpLoan,
      FeeManagementSetting feeManagementSetting,
      FeeManagementPersonaInfo feeManagementPermission)
    {
      this.formLoading = true;
      this.inTemplate = true;
      this.lpLoan = lpLoan;
      this.feeManagementSetting = feeManagementSetting;
      this.feeManagementPermission = feeManagementPermission;
      this.InitializeComponent();
      this.Text = caption;
      this.setFieldToolTips();
      this.rateTxt.Text = lpLoan.GetField("1752");
      this.textBox1003.Text = lpLoan.GetField("L268");
      this.textBox1004.Text = lpLoan.GetField("231");
      this.textBox1007.Text = lpLoan.GetField("1630");
      this.textBox1008.Text = lpLoan.GetField("253");
      this.textBox1009.Text = lpLoan.GetField("254");
      this.textBox1007Des.Text = lpLoan.GetField("1628");
      this.textBox1008Des.Text = lpLoan.GetField("660");
      this.textBox1009Des.Text = lpLoan.GetField("661");
      string field1 = lpLoan.GetField("19");
      string field2 = lpLoan.GetField("1964");
      string str = lpLoan.GetField("1751");
      if (field1 == "ConstructionOnly" || field1 == "ConstructionToPermanent")
        str = !(field2 == "Y") ? "As Completed Appraised Value" : "As Completed Purchase Price";
      this.rateFactor = Utils.ParseDouble((object) this.rateTxt.Text);
      switch (str)
      {
        case "Purchase Price":
          this.typeCombo.SelectedIndex = 1;
          break;
        case "Appraisal Value":
          this.typeCombo.SelectedIndex = 2;
          break;
        case "Base Loan Amount":
          this.typeCombo.SelectedIndex = 3;
          break;
        case "As Completed Purchase Price":
          this.typeCombo.SelectedIndex = 4;
          break;
        case "As Completed Appraised Value":
          this.typeCombo.SelectedIndex = 5;
          break;
        default:
          this.typeCombo.SelectedIndex = 0;
          break;
      }
      this.chkTax.Checked = lpLoan.GetField("USEGFETAX") == "Y";
      this.typeCombo_SelectedIndexChanged((object) null, (EventArgs) null);
      this.setFieldAccessStatus();
      this.formLoading = false;
    }

    internal RETaxesDialog(
      string caption,
      LoanData loan,
      Sessions.Session session,
      FeeManagementSetting feeManagementSetting,
      FeeManagementPersonaInfo feeManagementPermission)
    {
      this.formLoading = true;
      this.loan = loan;
      this.inTemplate = false;
      this.session = session;
      this.feeManagementSetting = feeManagementSetting;
      this.feeManagementPermission = feeManagementPermission;
      this.InitializeComponent();
      this.Text = caption;
      this.setFieldToolTips();
      this.rateTxt.Text = loan.GetField("1752");
      this.textBox1003.Text = loan.GetField("L268");
      this.textBox1004.Text = loan.GetField("231");
      this.textBox1007.Text = loan.GetField("1630");
      this.textBox1008.Text = loan.GetField("253");
      this.textBox1009.Text = loan.GetField("254");
      this.textBox1007Des.Text = loan.GetField("1628");
      this.textBox1008Des.Text = loan.GetField("660");
      this.textBox1009Des.Text = loan.GetField("661");
      string field1 = loan.GetField("19");
      string field2 = loan.GetField("1964");
      string str = loan.GetField("1751");
      if (field1 == "ConstructionOnly" || field1 == "ConstructionToPermanent")
        str = !(field2 == "Y") ? "As Completed Appraised Value" : "As Completed Purchase Price";
      this.rateFactor = Utils.ParseDouble((object) this.rateTxt.Text);
      switch (str)
      {
        case "Purchase Price":
          this.typeCombo.SelectedIndex = 1;
          break;
        case "Appraisal Value":
          this.typeCombo.SelectedIndex = 2;
          break;
        case "Base Loan Amount":
          this.typeCombo.SelectedIndex = 3;
          break;
        case "As Completed Purchase Price":
          this.typeCombo.SelectedIndex = 4;
          break;
        case "As Completed Appraised Value":
          this.typeCombo.SelectedIndex = 5;
          break;
        case "":
          if (loan.GetField("19") == "Purchase")
          {
            this.typeCombo.SelectedIndex = 1;
            break;
          }
          this.typeCombo.SelectedIndex = 0;
          break;
        default:
          this.typeCombo.SelectedIndex = 0;
          break;
      }
      this.chkTax.Checked = loan.GetField("USEGFETAX") == "Y";
      this.chkTax_CheckedChanged((object) null, (EventArgs) null);
      this.typeCombo_SelectedIndexChanged((object) null, (EventArgs) null);
      this.setFieldAccessStatus();
      this.formLoading = false;
    }

    private void setFieldToolTips()
    {
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.rateTxt, "1752");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.typeCombo, "1751");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.chkTax, "USEGFETAX");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBox1003, "L268");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBox1004, "231");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBox1007, "1630");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBox1008, "253");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBox1009, "254");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBox1007Des, "1628");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBox1008Des, "660");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBox1009Des, "661");
    }

    private void setBusinessRule()
    {
      try
      {
        ResourceManager resources = new ResourceManager(typeof (MIPDialog));
        this.popupRules = new PopupBusinessRules(this.loan, resources, (System.Drawing.Image) resources.GetObject("pboxAsterisk.Image"), (System.Drawing.Image) resources.GetObject("pboxDownArrow.Image"), this.sessionDefault);
        this.popupRules.SetBusinessRules((object) this.rateTxt, "1752");
        this.popupRules.SetBusinessRules((object) this.typeCombo, "1751");
        this.popupRules.SetBusinessRules((object) this.chkTax, "USEGFETAX");
        this.popupRules.SetBusinessRules((object) this.textBox1003, "L268");
        this.popupRules.SetBusinessRules((object) this.textBox1004, "231");
        this.popupRules.SetBusinessRules((object) this.textBox1007, "1630");
        this.popupRules.SetBusinessRules((object) this.textBox1008, "253");
        this.popupRules.SetBusinessRules((object) this.textBox1009, "254");
        this.popupRules.SetBusinessRules((object) this.textBox1007Des, "1628");
        this.popupRules.SetBusinessRules((object) this.textBox1008Des, "660");
        this.popupRules.SetBusinessRules((object) this.textBox1009Des, "661");
      }
      catch (Exception ex)
      {
        Tracing.Log(RETaxesDialog.sw, TraceLevel.Error, nameof (RETaxesDialog), "Cannot set Field access right. Error: " + ex.Message);
      }
    }

    private void setFieldAccessStatus()
    {
      if (this.popupRules == null)
      {
        ResourceManager resources = new ResourceManager(typeof (OtherExpenseDialog));
        this.popupRules = new PopupBusinessRules(this.loan, resources, (System.Drawing.Image) resources.GetObject("pboxAsterisk.Image"), (System.Drawing.Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      }
      this.setBusinessRule();
      System.Windows.Forms.Control[] controlArray = new System.Windows.Forms.Control[8]
      {
        (System.Windows.Forms.Control) this.textBox1003,
        (System.Windows.Forms.Control) this.textBox1004,
        (System.Windows.Forms.Control) this.textBox1007,
        (System.Windows.Forms.Control) this.textBox1008,
        (System.Windows.Forms.Control) this.textBox1009,
        (System.Windows.Forms.Control) this.textBox1007Des,
        (System.Windows.Forms.Control) this.textBox1008Des,
        (System.Windows.Forms.Control) this.textBox1009Des
      };
      for (int index = 0; index < controlArray.Length; ++index)
      {
        ((TextBoxBase) controlArray[index]).ReadOnly = false;
        if (this.formLoading)
          this.popupRules.SetBusinessRules((object) controlArray[index], controlArray[index].Tag.ToString());
        else
          this.popupRules.SetBusinessRules((object) controlArray[index], controlArray[index].Tag.ToString(), false);
        System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) controlArray[index];
        if (!this.chkTax.Checked && !textBox.ReadOnly)
        {
          switch (textBox.Name)
          {
            case "textBox1007Des":
              this.pb1628.Visible = true;
              continue;
            case "textBox1008Des":
              this.pb660.Visible = true;
              continue;
            case "textBox1009Des":
              this.pb661.Visible = true;
              continue;
            default:
              ((TextBoxBase) controlArray[index]).ReadOnly = true;
              continue;
          }
        }
      }
      if (this.chkTax.Checked || this.feeManagementSetting == null || !this.feeManagementSetting.CompanyOptIn)
      {
        this.pb1628.Visible = this.pb660.Visible = this.pb661.Visible = false;
      }
      else
      {
        if (this.chkTax.Checked)
        {
          if (!this.textBox1007Des.ReadOnly)
            this.pb1628.Visible = true;
          if (!this.textBox1008Des.ReadOnly)
            this.pb660.Visible = true;
          if (!this.textBox1009Des.ReadOnly)
            this.pb661.Visible = true;
        }
        bool flag = this.feeManagementPermission == null || this.feeManagementPermission.IsSectionEditable(FeeSectionEnum.For1000);
        if (this.feeManagementPermission != null && !this.textBox1007Des.ReadOnly)
          this.textBox1007Des.ReadOnly = !flag;
        if (this.feeManagementPermission != null && !this.textBox1008Des.ReadOnly)
          this.textBox1008Des.ReadOnly = !flag;
        if (this.feeManagementPermission == null || this.textBox1009Des.ReadOnly)
          return;
        this.textBox1009Des.ReadOnly = !flag;
      }
    }

    private void typeCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.inTemplate)
        return;
      switch (this.typeCombo.SelectedItem.ToString())
      {
        case "Loan Amount":
          this.loanTxt.Text = this.loan.GetField("2");
          break;
        case "Purchase Price":
          this.loanTxt.Text = this.loan.GetField("136");
          break;
        case "Appraisal Value":
          this.loanTxt.Text = this.loan.GetField("356");
          break;
        case "Base Loan Amount":
          this.loanTxt.Text = this.loan.GetField("1109");
          break;
        case "As Completed Purchase Price":
          this.loanTxt.Text = this.loan.GetField("CONST.X58");
          break;
        case "As Completed Appraised Value":
          this.loanTxt.Text = this.loan.GetField("CONST.X59");
          break;
      }
      this.Field_Leave((object) null, (EventArgs) null);
    }

    private void Field_Leave(object sender, EventArgs e)
    {
      this.rateFactor = Utils.ParseDouble((object) this.rateTxt.Text);
      this.amount = Math.Round(Utils.ParseDouble((object) this.loanTxt.Text.Replace(",", string.Empty)) * (this.rateFactor / 1200.0), 2);
      this.amountTxt.Text = this.amount.ToString("N2");
      switch (this.typeCombo.SelectedIndex)
      {
        case 1:
          this.priceType = "Purchase Price";
          break;
        case 2:
          this.priceType = "Appraisal Value";
          break;
        case 3:
          this.priceType = "Base Loan Amount";
          break;
        case 4:
          this.priceType = "As Completed Purchase Price";
          break;
        case 5:
          this.priceType = "As Completed Appraised Value";
          break;
        default:
          this.priceType = "Loan Amount";
          break;
      }
      double num = Utils.ParseDouble((object) this.textBox1003.Text.Trim()) + Utils.ParseDouble((object) this.textBox1004.Text.Trim()) + this.addTaxField(this.textBox1007Des.Text.Trim(), this.textBox1007) + this.addTaxField(this.textBox1008Des.Text.Trim(), this.textBox1008) + this.addTaxField(this.textBox1009Des.Text.Trim(), this.textBox1009);
      this.textBoxTotal.Text = num.ToString("N2");
      if (this.chkTax.Checked)
        this.amount = num;
      if (this.formLoading)
        return;
      this.setFieldAccessStatus();
    }

    private double addTaxField(string desc, System.Windows.Forms.TextBox box)
    {
      double num = Utils.ParseDouble((object) box.Text.Trim());
      if (num != 0.0)
        box.Text = num.ToString("N2");
      return SharedCalculations.IsTaxFee(desc) ? num : 0.0;
    }

    private void Num4Field_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatField(FieldFormat.DECIMAL_4, (System.Windows.Forms.TextBox) sender);
    }

    private void Num2Field_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatField(FieldFormat.DECIMAL_2, (System.Windows.Forms.TextBox) sender);
    }

    private void formatField(FieldFormat format, System.Windows.Forms.TextBox box)
    {
      bool needsUpdate = false;
      string str = Utils.FormatInput(box.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      box.Text = str;
      box.SelectionStart = str.Length;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.Field_Leave((object) null, (EventArgs) null);
      if (this.inTemplate)
      {
        if (this.chkTax.Checked)
          this.lpLoan.SetCurrentField("USEGFETAX", "Y");
        else
          this.lpLoan.SetCurrentField("USEGFETAX", "");
        this.lpLoan.SetCurrentField("L268", this.textBox1003.Text.Trim());
        this.lpLoan.SetCurrentField("231", this.textBox1004.Text.Trim());
        this.lpLoan.SetCurrentField("1630", this.textBox1007.Text.Trim());
        this.lpLoan.SetCurrentField("253", this.textBox1008.Text.Trim());
        this.lpLoan.SetCurrentField("254", this.textBox1009.Text.Trim());
        this.lpLoan.SetCurrentField("1628", this.textBox1007Des.Text.Trim());
        this.lpLoan.SetCurrentField("660", this.textBox1008Des.Text.Trim());
        this.lpLoan.SetCurrentField("661", this.textBox1009Des.Text.Trim());
        this.lpLoan.SetCurrentField("1751", this.priceType.ToString());
        this.lpLoan.SetCurrentField("1752", this.rateFactor.ToString());
        this.lpLoan.SetField("1405", this.amount.ToString("N2"));
      }
      else
      {
        if (this.chkTax.Checked)
          this.loan.SetCurrentField("USEGFETAX", "Y");
        else
          this.loan.SetCurrentField("USEGFETAX", "");
        this.loan.SetCurrentField("L268", this.textBox1003.Text.Trim());
        this.loan.SetCurrentField("231", this.textBox1004.Text.Trim());
        this.loan.SetCurrentField("1630", this.textBox1007.Text.Trim());
        this.loan.SetCurrentField("253", this.textBox1008.Text.Trim());
        this.loan.SetCurrentField("254", this.textBox1009.Text.Trim());
        this.loan.SetCurrentField("1628", this.textBox1007Des.Text.Trim());
        this.loan.SetCurrentField("660", this.textBox1008Des.Text.Trim());
        this.loan.SetCurrentField("661", this.textBox1009Des.Text.Trim());
        this.loan.SetCurrentField("1751", this.priceType.ToString());
        this.loan.SetCurrentField("1752", this.rateFactor.ToString());
        this.loan.SetField("1405", this.amount.ToString("N2"));
      }
      this.DialogResult = DialogResult.OK;
    }

    private void chkTax_CheckedChanged(object sender, EventArgs e)
    {
      if (this.formLoading)
        return;
      this.setFieldAccessStatus();
    }

    private void RETaxesDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void displayCustomCodeRuntimeError(string eventName, Exception ex)
    {
      ErrorDialog.Display("An error has occurred in the '" + eventName + " Error : " + ex.Message, ex);
    }

    private void populatepicklist(System.Windows.Forms.TextBox textBox)
    {
      if (this.feeManagementSetting == null)
        return;
      string[] feeNames = this.feeManagementSetting.GetFeeNames(FeeSectionEnum.For1000);
      DropdownOption[] options = new DropdownOption[feeNames.Length];
      for (int index = 0; index < feeNames.Length; ++index)
        options[index] = new DropdownOption(feeNames[index]);
      int w = Math.Max(textBox.Width, 260);
      int x1 = Cursor.Position.X;
      int y1 = Cursor.Position.Y;
      int x2 = x1 - w;
      int y2 = y1 + 10;
      using (FieldRuleDropdownDialog ruleDropdownDialog = new FieldRuleDropdownDialog(options, "test"))
      {
        if (ruleDropdownDialog.Height + y2 > Screen.PrimaryScreen.Bounds.Height)
          y2 = y1 - ruleDropdownDialog.Height - 40;
        ruleDropdownDialog.SetListBoxWidth(w);
        ruleDropdownDialog.Location = new Point(x2, y2);
        if (ruleDropdownDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        try
        {
          textBox.Text = ruleDropdownDialog.SelectedItem.Text;
        }
        catch (Exception ex)
        {
          this.displayCustomCodeRuntimeError(nameof (populatepicklist), ex);
        }
      }
    }

    private void pb1628_Click(object sender, EventArgs e)
    {
      this.populatepicklist(this.textBox1007Des);
    }

    private void pb661_Click(object sender, EventArgs e)
    {
      this.populatepicklist(this.textBox1008Des);
    }

    private void pb660_Click(object sender, EventArgs e)
    {
      this.populatepicklist(this.textBox1009Des);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RETaxesDialog));
      this.cancelBtn = new System.Windows.Forms.Button();
      this.okBtn = new System.Windows.Forms.Button();
      this.fieldToolTip = new ToolTip(this.components);
      this.borderPanel3 = new BorderPanel();
      this.label9 = new System.Windows.Forms.Label();
      this.chkTax = new System.Windows.Forms.CheckBox();
      this.textBox1004 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.textBoxTotal = new System.Windows.Forms.TextBox();
      this.textBox1003 = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.textBox1007 = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.textBox1008 = new System.Windows.Forms.TextBox();
      this.textBox1009Des = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.textBox1008Des = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.textBox1007Des = new System.Windows.Forms.TextBox();
      this.textBox1009 = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.borderPanel1 = new BorderPanel();
      this.label10 = new System.Windows.Forms.Label();
      this.label = new System.Windows.Forms.Label();
      this.borderPanel2 = new BorderPanel();
      this.typeCombo = new ComboBox();
      this.amountTxt = new System.Windows.Forms.TextBox();
      this.rateTxt = new System.Windows.Forms.TextBox();
      this.loanTxt = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.pb1628 = new PictureBox();
      this.pb660 = new PictureBox();
      this.pb661 = new PictureBox();
      this.borderPanel3.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      ((ISupportInitialize) this.pb1628).BeginInit();
      ((ISupportInitialize) this.pb660).BeginInit();
      ((ISupportInitialize) this.pb661).BeginInit();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(242, 345);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 15;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(161, 345);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 14;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.borderPanel3.BackColor = Color.WhiteSmoke;
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.pb661);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.pb660);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.pb1628);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.chkTax);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBox1004);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBoxTotal);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBox1003);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBox1007);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBox1008);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBox1009Des);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBox1008Des);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBox1007Des);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.textBox1009);
      this.borderPanel3.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.borderPanel3.Location = new Point(12, 118);
      this.borderPanel3.Name = "borderPanel3";
      this.borderPanel3.Size = new Size(329, 219);
      this.borderPanel3.TabIndex = 4;
      this.borderPanel3.TabStop = true;
      this.label9.Location = new Point(10, 184);
      this.label9.Name = "label9";
      this.label9.Size = new Size(285, 32);
      this.label9.TabIndex = 32;
      this.label9.Text = "The description must have the word \"tax\" to be included in the Total Tax.";
      this.chkTax.Location = new Point(13, 13);
      this.chkTax.Name = "chkTax";
      this.chkTax.Size = new Size(245, 16);
      this.chkTax.TabIndex = 5;
      this.chkTax.Text = "Use / Update Monthly Values from Itemization";
      this.chkTax.CheckedChanged += new EventHandler(this.chkTax_CheckedChanged);
      this.chkTax.Click += new EventHandler(this.Field_Leave);
      this.textBox1004.Location = new Point(219, 60);
      this.textBox1004.Name = "textBox1004";
      this.textBox1004.Size = new Size(96, 20);
      this.textBox1004.TabIndex = 7;
      this.textBox1004.Tag = (object) "231";
      this.textBox1004.TextAlign = HorizontalAlignment.Right;
      this.textBox1004.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.textBox1004.Leave += new EventHandler(this.Field_Leave);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 39);
      this.label3.Name = "label3";
      this.label3.Size = new Size(117, 13);
      this.label3.TabIndex = 21;
      this.label3.Text = "1003  City Property Tax";
      this.textBoxTotal.BackColor = Color.WhiteSmoke;
      this.textBoxTotal.Location = new Point(219, 152);
      this.textBoxTotal.Name = "textBoxTotal";
      this.textBoxTotal.ReadOnly = true;
      this.textBoxTotal.Size = new Size(96, 20);
      this.textBoxTotal.TabIndex = 29;
      this.textBoxTotal.TabStop = false;
      this.textBoxTotal.TextAlign = HorizontalAlignment.Right;
      this.textBox1003.Location = new Point(219, 37);
      this.textBox1003.Name = "textBox1003";
      this.textBox1003.Size = new Size(96, 20);
      this.textBox1003.TabIndex = 6;
      this.textBox1003.Tag = (object) "L268";
      this.textBox1003.TextAlign = HorizontalAlignment.Right;
      this.textBox1003.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.textBox1003.Leave += new EventHandler(this.Field_Leave);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(128, 156);
      this.label7.Name = "label7";
      this.label7.Size = new Size(78, 13);
      this.label7.TabIndex = 28;
      this.label7.Text = "Total Tax Fees";
      this.textBox1007.Location = new Point(219, 83);
      this.textBox1007.Name = "textBox1007";
      this.textBox1007.Size = new Size(96, 20);
      this.textBox1007.TabIndex = 9;
      this.textBox1007.Tag = (object) "1630";
      this.textBox1007.TextAlign = HorizontalAlignment.Right;
      this.textBox1007.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.textBox1007.Leave += new EventHandler(this.Field_Leave);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 62);
      this.label8.Name = "label8";
      this.label8.Size = new Size(98, 13);
      this.label8.TabIndex = 30;
      this.label8.Text = "1004  Tax Reserve";
      this.textBox1008.Location = new Point(219, 106);
      this.textBox1008.Name = "textBox1008";
      this.textBox1008.Size = new Size(96, 20);
      this.textBox1008.TabIndex = 11;
      this.textBox1008.Tag = (object) "253";
      this.textBox1008.TextAlign = HorizontalAlignment.Right;
      this.textBox1008.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.textBox1008.Leave += new EventHandler(this.Field_Leave);
      this.textBox1009Des.Location = new Point(42, 129);
      this.textBox1009Des.Name = "textBox1009Des";
      this.textBox1009Des.Size = new Size(148, 20);
      this.textBox1009Des.TabIndex = 12;
      this.textBox1009Des.Tag = (object) "661";
      this.textBox1009Des.Leave += new EventHandler(this.Field_Leave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 87);
      this.label4.Name = "label4";
      this.label4.Size = new Size(31, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "1007";
      this.textBox1008Des.Location = new Point(42, 106);
      this.textBox1008Des.Name = "textBox1008Des";
      this.textBox1008Des.Size = new Size(148, 20);
      this.textBox1008Des.TabIndex = 10;
      this.textBox1008Des.Tag = (object) "660";
      this.textBox1008Des.Leave += new EventHandler(this.Field_Leave);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 110);
      this.label5.Name = "label5";
      this.label5.Size = new Size(31, 13);
      this.label5.TabIndex = 25;
      this.label5.Text = "1008";
      this.textBox1007Des.Location = new Point(42, 83);
      this.textBox1007Des.Name = "textBox1007Des";
      this.textBox1007Des.Size = new Size(148, 20);
      this.textBox1007Des.TabIndex = 8;
      this.textBox1007Des.Tag = (object) "1628";
      this.textBox1007Des.Leave += new EventHandler(this.Field_Leave);
      this.textBox1009.Location = new Point(219, 129);
      this.textBox1009.Name = "textBox1009";
      this.textBox1009.Size = new Size(96, 20);
      this.textBox1009.TabIndex = 13;
      this.textBox1009.Tag = (object) "254";
      this.textBox1009.TextAlign = HorizontalAlignment.Right;
      this.textBox1009.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.textBox1009.Leave += new EventHandler(this.Field_Leave);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 133);
      this.label6.Name = "label6";
      this.label6.Size = new Size(31, 13);
      this.label6.TabIndex = 27;
      this.label6.Text = "1009";
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label10);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.borderPanel2);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.typeCombo);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.amountTxt);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.rateTxt);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.loanTxt);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.borderPanel1.Location = new Point(12, 12);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(329, 100);
      this.borderPanel1.TabIndex = 0;
      this.borderPanel1.TabStop = true;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(97, 40);
      this.label10.Name = "label10";
      this.label10.Size = new Size(14, 13);
      this.label10.TabIndex = 23;
      this.label10.Text = "X";
      this.label.AutoSize = true;
      this.label.Location = new Point(96, 65);
      this.label.Name = "label";
      this.label.Size = new Size(88, 13);
      this.label.TabIndex = 20;
      this.label.Text = "Monthly Payment";
      this.borderPanel2.Location = new Point(99, 58);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(230, 1);
      this.borderPanel2.TabIndex = 22;
      this.typeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.typeCombo.Items.AddRange(new object[6]
      {
        (object) "Loan Amount",
        (object) "Purchase Price",
        (object) "Appraisal Value",
        (object) "Base Loan Amount",
        (object) "As Completed Purchase Price",
        (object) "As Completed Appraised Value"
      });
      this.typeCombo.Location = new Point(14, 13);
      this.typeCombo.Name = "typeCombo";
      this.typeCombo.Size = new Size(174, 21);
      this.typeCombo.TabIndex = 1;
      this.typeCombo.SelectedIndexChanged += new EventHandler(this.typeCombo_SelectedIndexChanged);
      this.amountTxt.BackColor = Color.WhiteSmoke;
      this.amountTxt.Enabled = false;
      this.amountTxt.Location = new Point(194, 62);
      this.amountTxt.Name = "amountTxt";
      this.amountTxt.ReadOnly = true;
      this.amountTxt.Size = new Size(128, 20);
      this.amountTxt.TabIndex = 16;
      this.amountTxt.TabStop = false;
      this.amountTxt.TextAlign = HorizontalAlignment.Right;
      this.rateTxt.Location = new Point(194, 35);
      this.rateTxt.MaxLength = 8;
      this.rateTxt.Name = "rateTxt";
      this.rateTxt.Size = new Size(107, 20);
      this.rateTxt.TabIndex = 2;
      this.rateTxt.TextAlign = HorizontalAlignment.Right;
      this.rateTxt.KeyUp += new KeyEventHandler(this.Num4Field_KeyUp);
      this.rateTxt.Leave += new EventHandler(this.Field_Leave);
      this.loanTxt.BackColor = Color.WhiteSmoke;
      this.loanTxt.Location = new Point(194, 13);
      this.loanTxt.Name = "loanTxt";
      this.loanTxt.ReadOnly = true;
      this.loanTxt.Size = new Size(128, 20);
      this.loanTxt.TabIndex = 13;
      this.loanTxt.TabStop = false;
      this.loanTxt.TextAlign = HorizontalAlignment.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(154, 39);
      this.label1.Name = "label1";
      this.label1.Size = new Size(30, 13);
      this.label1.TabIndex = 18;
      this.label1.Text = "Rate";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(307, 40);
      this.label2.Name = "label2";
      this.label2.Size = new Size(15, 13);
      this.label2.TabIndex = 21;
      this.label2.Text = "%";
      this.pb1628.Image = (System.Drawing.Image) componentResourceManager.GetObject("pb1628.Image");
      this.pb1628.Location = new Point(191, 83);
      this.pb1628.Name = "pb1628";
      this.pb1628.Size = new Size(17, 17);
      this.pb1628.TabIndex = 70;
      this.pb1628.TabStop = false;
      this.pb1628.Visible = false;
      this.pb1628.Click += new EventHandler(this.pb1628_Click);
      this.pb660.Image = (System.Drawing.Image) componentResourceManager.GetObject("pb660.Image");
      this.pb660.Location = new Point(191, 106);
      this.pb660.Name = "pb660";
      this.pb660.Size = new Size(17, 17);
      this.pb660.TabIndex = 71;
      this.pb660.TabStop = false;
      this.pb660.Visible = false;
      this.pb660.Click += new EventHandler(this.pb661_Click);
      this.pb661.Image = (System.Drawing.Image) componentResourceManager.GetObject("pb661.Image");
      this.pb661.Location = new Point(191, 128);
      this.pb661.Name = "pb661";
      this.pb661.Size = new Size(17, 17);
      this.pb661.TabIndex = 72;
      this.pb661.TabStop = false;
      this.pb661.Visible = false;
      this.pb661.Click += new EventHandler(this.pb660_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(353, 382);
      this.Controls.Add((System.Windows.Forms.Control) this.borderPanel3);
      this.Controls.Add((System.Windows.Forms.Control) this.borderPanel1);
      this.Controls.Add((System.Windows.Forms.Control) this.cancelBtn);
      this.Controls.Add((System.Windows.Forms.Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RETaxesDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Taxes";
      this.KeyPress += new KeyPressEventHandler(this.RETaxesDialog_KeyPress);
      this.borderPanel3.ResumeLayout(false);
      this.borderPanel3.PerformLayout();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      ((ISupportInitialize) this.pb1628).EndInit();
      ((ISupportInitialize) this.pb660).EndInit();
      ((ISupportInitialize) this.pb661).EndInit();
      this.ResumeLayout(false);
    }
  }
}
