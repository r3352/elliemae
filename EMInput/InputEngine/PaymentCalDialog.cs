// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PaymentCalDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
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
  internal class PaymentCalDialog : Form
  {
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private TextBox payment;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox loanTxt;
    private TextBox rateTxt;
    private TextBox termTxt;
    private ToolTip fieldToolTip;
    private PictureBox pboxAsterisk;
    private IContainer components;
    private LoanData loan;
    private PictureBox pboxDownArrow;
    private BorderPanel borderPanel1;
    private BorderPanel borderPanel2;
    private PopupBusinessRules popupRules;
    private double amount;
    private double loanamt;
    private double term;
    private double intrate;

    internal PaymentCalDialog(LoanData loan, string caption)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.Text = caption;
      ResourceManager resources = new ResourceManager(typeof (PaymentCalDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      if (caption == "Mortgage Payment Calculation")
      {
        this.fieldToolTip.SetToolTip((Control) this.loanTxt, "LOANAMT1");
        this.fieldToolTip.SetToolTip((Control) this.rateTxt, "INTRATE1");
        this.fieldToolTip.SetToolTip((Control) this.termTxt, "TERM1");
        this.fieldToolTip.SetToolTip((Control) this.payment, "PAYMENT1");
        this.popupRules.SetBusinessRules((object) this.loanTxt, "LOANAMT1");
        this.popupRules.SetBusinessRules((object) this.rateTxt, "INTRATE1");
        this.popupRules.SetBusinessRules((object) this.termTxt, "TERM1");
        this.loanTxt.Tag = (object) "LOANAMT1";
        this.rateTxt.Tag = (object) "INTRATE1";
        this.termTxt.Tag = (object) "TERM1";
      }
      else
      {
        this.fieldToolTip.SetToolTip((Control) this.loanTxt, "LOANAMT2");
        this.fieldToolTip.SetToolTip((Control) this.rateTxt, "INTRATE2");
        this.fieldToolTip.SetToolTip((Control) this.termTxt, "TERM2");
        this.fieldToolTip.SetToolTip((Control) this.payment, "PAYMENT2");
        this.popupRules.SetBusinessRules((object) this.loanTxt, "LOANAMT2");
        this.popupRules.SetBusinessRules((object) this.rateTxt, "INTRATE2");
        this.popupRules.SetBusinessRules((object) this.termTxt, "TERM2");
        this.loanTxt.Tag = (object) "LOANAMT2";
        this.rateTxt.Tag = (object) "INTRATE2";
        this.termTxt.Tag = (object) "TERM2";
      }
    }

    internal void UpdateCalcPymt(string loanamt, string rate, string term, string pymt)
    {
      this.loanTxt.Text = loanamt;
      this.rateTxt.Text = rate;
      this.termTxt.Text = term;
      this.payment.Text = pymt;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PaymentCalDialog));
      this.label1 = new Label();
      this.loanTxt = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.rateTxt = new TextBox();
      this.termTxt = new TextBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.payment = new TextBox();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.fieldToolTip = new ToolTip(this.components);
      this.pboxAsterisk = new PictureBox();
      this.pboxDownArrow = new PictureBox();
      this.borderPanel1 = new BorderPanel();
      this.borderPanel2 = new BorderPanel();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(70, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Loan Amount";
      this.loanTxt.Location = new Point(117, 11);
      this.loanTxt.Name = "loanTxt";
      this.loanTxt.Size = new Size(118, 20);
      this.loanTxt.TabIndex = 1;
      this.loanTxt.TextAlign = HorizontalAlignment.Right;
      this.loanTxt.Leave += new EventHandler(this.leave);
      this.loanTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.loanTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 37);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Interest Rate";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(31, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Term";
      this.rateTxt.Location = new Point(117, 34);
      this.rateTxt.Name = "rateTxt";
      this.rateTxt.Size = new Size(72, 20);
      this.rateTxt.TabIndex = 2;
      this.rateTxt.TextAlign = HorizontalAlignment.Right;
      this.rateTxt.Leave += new EventHandler(this.leave);
      this.rateTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.rateTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.termTxt.Location = new Point(117, 57);
      this.termTxt.Name = "termTxt";
      this.termTxt.Size = new Size(72, 20);
      this.termTxt.TabIndex = 3;
      this.termTxt.TextAlign = HorizontalAlignment.Right;
      this.termTxt.Leave += new EventHandler(this.leave);
      this.termTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.termTxt.KeyPress += new KeyPressEventHandler(this.keypressInt);
      this.label4.Location = new Point(9, 90);
      this.label4.Name = "label4";
      this.label4.Size = new Size(108, 24);
      this.label4.TabIndex = 7;
      this.label4.Text = "Monthly Payment";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(203, 37);
      this.label5.Name = "label5";
      this.label5.Size = new Size(15, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "%";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(203, 60);
      this.label6.Name = "label6";
      this.label6.Size = new Size(29, 13);
      this.label6.TabIndex = 9;
      this.label6.Text = "mths";
      this.payment.BackColor = Color.WhiteSmoke;
      this.payment.Location = new Point(117, 90);
      this.payment.Name = "payment";
      this.payment.ReadOnly = true;
      this.payment.Size = new Size(118, 20);
      this.payment.TabIndex = 10;
      this.payment.TabStop = false;
      this.payment.TextAlign = HorizontalAlignment.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(102, 146);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 12;
      this.okBtn.Text = "&OK";
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(183, 146);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 13;
      this.cancelBtn.Text = "&Cancel";
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(83, 42);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 19;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(83, 60);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 68;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Controls.Add((Control) this.borderPanel2);
      this.borderPanel1.Controls.Add((Control) this.pboxDownArrow);
      this.borderPanel1.Controls.Add((Control) this.label1);
      this.borderPanel1.Controls.Add((Control) this.pboxAsterisk);
      this.borderPanel1.Controls.Add((Control) this.label2);
      this.borderPanel1.Controls.Add((Control) this.label3);
      this.borderPanel1.Controls.Add((Control) this.label5);
      this.borderPanel1.Controls.Add((Control) this.label6);
      this.borderPanel1.Controls.Add((Control) this.payment);
      this.borderPanel1.Controls.Add((Control) this.loanTxt);
      this.borderPanel1.Controls.Add((Control) this.label4);
      this.borderPanel1.Controls.Add((Control) this.termTxt);
      this.borderPanel1.Controls.Add((Control) this.rateTxt);
      this.borderPanel1.Location = new Point(12, 12);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(246, 124);
      this.borderPanel1.TabIndex = 0;
      this.borderPanel1.TabStop = true;
      this.borderPanel2.Location = new Point(0, 83);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(245, 1);
      this.borderPanel2.TabIndex = 0;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(271, 186);
      this.Controls.Add((Control) this.borderPanel1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PaymentCalDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "MonthlyPaymentCalDialog";
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    internal double Amount => this.amount;

    internal double LoanAmt => this.loanamt;

    internal double Term => this.term;

    internal double IntRate => this.intrate;

    private void keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_3;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void keypressInt(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void leave(object sender, EventArgs e)
    {
      TextBox ctrl = (TextBox) sender;
      this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag);
      this.amount = 0.0;
      double num1 = 0.0;
      double num2 = 0.0;
      double y = 0.0;
      try
      {
        y = !(this.termTxt.Text.Trim() != string.Empty) ? 0.0 : (double) Utils.ParseInt((object) this.termTxt.Text);
        num2 = Utils.ParseDouble((object) this.rateTxt.Text) / 1200.0;
        num1 = Utils.ParseDouble((object) this.loanTxt.Text.Replace(",", string.Empty));
        num1 = Math.Round(num1, 2);
        this.loanTxt.Text = num1.ToString("#,#.00");
        double num3 = Math.Pow(1.0 + num2, y);
        this.amount = num3 <= 1.0 ? 0.0 : num1 * num2 * num3 / (num3 - 1.0);
        if (ctrl == this.termTxt)
          this.okBtn.Focus();
      }
      catch (Exception ex)
      {
        this.amount = 0.0;
      }
      this.amount = Math.Round(this.amount, 2);
      this.payment.Text = this.amount.ToString("N2");
      this.loanamt = num1;
      this.intrate = num2 * 1200.0;
      this.term = y;
    }
  }
}
