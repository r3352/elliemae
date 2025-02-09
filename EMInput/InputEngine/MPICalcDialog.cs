// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MPICalcDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MPICalcDialog : System.Windows.Forms.Form
  {
    private LoanData loandata;
    private string fieldId;
    private bool isMonthlypi;
    private List<string> mpifields = new List<string>()
    {
      "28",
      "26",
      "27",
      "18"
    };
    private List<string> maxpifields = new List<string>()
    {
      "31",
      "29",
      "30",
      "19"
    };
    private IContainer components;
    private System.Windows.Forms.Panel pnlMainContainer;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnclose;
    private System.Windows.Forms.Button btnApply;
    private System.Windows.Forms.TextBox txtTerm;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txtIntRate;
    private System.Windows.Forms.TextBox txtLoanAmount;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtmonthlyPayment;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;

    public MPICalcDialog() => this.InitializeComponent();

    public MPICalcDialog(LoanData ln, IMainScreen mainScreen, FieldSource fs)
    {
      this.loandata = fs == FieldSource.CurrentLoan ? ln : ln.LinkedData;
      this.InitializeComponent();
    }

    public MPICalcDialog(LoanData ln, string fieldId)
    {
      this.loandata = ln;
      this.InitializeComponent();
      List<string> stringList;
      if (fieldId.StartsWith("monthlypi"))
      {
        this.fieldId = fieldId.Replace("monthlypi", "");
        stringList = this.mpifields;
        this.isMonthlypi = true;
      }
      else
      {
        this.fieldId = fieldId.Replace("maximumpi", "");
        stringList = this.maxpifields;
      }
      this.txtLoanAmount.Text = ln.GetSimpleField(this.fieldId + stringList[0]).Trim();
      this.txtIntRate.Text = ln.GetSimpleField(this.fieldId + stringList[1]).Trim();
      this.txtTerm.Text = ln.GetSimpleField(this.fieldId + stringList[2]).Trim();
      this.txtmonthlyPayment.Text = ln.GetSimpleField(this.fieldId + stringList[3]).Trim();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void HelocCalcDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void HelocCalcDialog_Load(object sender, EventArgs e)
    {
    }

    private void btnApply_Click(object sender, EventArgs e)
    {
      this.loandata.SetField(this.fieldId + (this.isMonthlypi ? this.mpifields[3] : this.maxpifields[3]), this.txtmonthlyPayment.Text);
      this.DialogResult = DialogResult.OK;
    }

    private void leave(object sender, EventArgs e)
    {
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      double num1;
      try
      {
        double y = !(this.txtTerm.Text.Trim() != string.Empty) ? 0.0 : (double) Utils.ParseInt((object) this.txtTerm.Text);
        double num2 = Utils.ParseDouble((object) this.txtIntRate.Text) / 1200.0;
        double num3 = Math.Round(Utils.ParseDouble((object) this.txtLoanAmount.Text.Replace(",", string.Empty)), 2);
        this.txtLoanAmount.Text = num3.ToString("#,#.00");
        double num4 = Math.Pow(1.0 + num2, y);
        num1 = num4 <= 1.0 ? 0.0 : num3 * num2 * num4 / (num4 - 1.0);
        if (textBox == this.txtTerm)
          this.btnApply.Focus();
      }
      catch (Exception ex)
      {
        num1 = 0.0;
      }
      this.txtmonthlyPayment.Text = Math.Round(num1, 2).ToString("N2");
      short int16 = Convert.ToInt16(textBox.Tag.ToString());
      this.loandata.SetField(this.fieldId + (this.isMonthlypi ? this.mpifields[(int) int16] : this.maxpifields[(int) int16]), textBox.Text);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlMainContainer = new System.Windows.Forms.Panel();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnclose = new System.Windows.Forms.Button();
      this.btnApply = new System.Windows.Forms.Button();
      this.txtmonthlyPayment = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtTerm = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.txtIntRate = new System.Windows.Forms.TextBox();
      this.txtLoanAmount = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.pnlMainContainer.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.panel1);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.txtTerm);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.txtIntRate);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.txtLoanAmount);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.pnlMainContainer.Dock = DockStyle.Fill;
      this.pnlMainContainer.Location = new Point(0, 0);
      this.pnlMainContainer.Name = "pnlMainContainer";
      this.pnlMainContainer.Size = new Size(360, 156);
      this.pnlMainContainer.TabIndex = 1;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(273, 60);
      this.label5.Name = "label5";
      this.label5.Size = new Size(29, 13);
      this.label5.TabIndex = 21;
      this.label5.Text = "mths";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(273, 36);
      this.label4.Name = "label4";
      this.label4.Size = new Size(15, 13);
      this.label4.TabIndex = 20;
      this.label4.Text = "%";
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.btnclose);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.btnApply);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.txtmonthlyPayment);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.panel1.Location = new Point(4, 82);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(350, 67);
      this.panel1.TabIndex = 19;
      this.btnclose.Location = new Point(261, 30);
      this.btnclose.Name = "btnclose";
      this.btnclose.Size = new Size(75, 23);
      this.btnclose.TabIndex = 11;
      this.btnclose.Text = "Cancel";
      this.btnclose.UseVisualStyleBackColor = true;
      this.btnclose.Click += new EventHandler(this.btnCancel_Click);
      this.btnApply.Location = new Point(167, 30);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(92, 23);
      this.btnApply.TabIndex = 10;
      this.btnApply.Text = "OK";
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.txtmonthlyPayment.Enabled = false;
      this.txtmonthlyPayment.Location = new Point(167, 3);
      this.txtmonthlyPayment.MaxLength = 100;
      this.txtmonthlyPayment.Name = "txtmonthlyPayment";
      this.txtmonthlyPayment.Size = new Size(172, 20);
      this.txtmonthlyPayment.TabIndex = 7;
      this.txtmonthlyPayment.TextAlign = HorizontalAlignment.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(88, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Monthly Payment";
      this.txtTerm.Location = new Point(172, 57);
      this.txtTerm.MaxLength = 100;
      this.txtTerm.Name = "txtTerm";
      this.txtTerm.Size = new Size(98, 20);
      this.txtTerm.TabIndex = 18;
      this.txtTerm.Tag = (object) "2";
      this.txtTerm.TextAlign = HorizontalAlignment.Right;
      this.txtTerm.Leave += new EventHandler(this.leave);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 60);
      this.label7.Name = "label7";
      this.label7.Size = new Size(31, 13);
      this.label7.TabIndex = 17;
      this.label7.Text = "Term";
      this.txtIntRate.Location = new Point(171, 33);
      this.txtIntRate.MaxLength = 100;
      this.txtIntRate.Name = "txtIntRate";
      this.txtIntRate.Size = new Size(99, 20);
      this.txtIntRate.TabIndex = 16;
      this.txtIntRate.Tag = (object) "1";
      this.txtIntRate.TextAlign = HorizontalAlignment.Right;
      this.txtIntRate.Leave += new EventHandler(this.leave);
      this.txtLoanAmount.Location = new Point(171, 9);
      this.txtLoanAmount.MaxLength = 100;
      this.txtLoanAmount.Name = "txtLoanAmount";
      this.txtLoanAmount.Size = new Size(172, 20);
      this.txtLoanAmount.TabIndex = 5;
      this.txtLoanAmount.Tag = (object) "0";
      this.txtLoanAmount.TextAlign = HorizontalAlignment.Right;
      this.txtLoanAmount.Leave += new EventHandler(this.leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Interest Rate";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(70, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Loan Amount";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(360, 156);
      this.Controls.Add((System.Windows.Forms.Control) this.pnlMainContainer);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MPICalcDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Other Financial Payment Calculation";
      this.Load += new EventHandler(this.HelocCalcDialog_Load);
      this.KeyPress += new KeyPressEventHandler(this.HelocCalcDialog_KeyPress);
      this.pnlMainContainer.ResumeLayout(false);
      this.pnlMainContainer.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
