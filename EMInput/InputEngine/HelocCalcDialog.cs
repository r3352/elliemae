// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HelocCalcDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HelocCalcDialog : System.Windows.Forms.Form
  {
    private LoanData loandata;
    private IContainer components;
    private System.Windows.Forms.Panel pnlMainContainer;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnclose;
    private System.Windows.Forms.Button btnApply;
    private System.Windows.Forms.TextBox txtloanamout;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtDrawAmount;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtTHCLT;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txtTCLTV;
    private System.Windows.Forms.TextBox txtValuationUsed;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;

    public HelocCalcDialog() => this.InitializeComponent();

    public HelocCalcDialog(LoanData ln, IMainScreen mainScreen, FieldSource fs)
    {
      this.loandata = fs == FieldSource.CurrentLoan ? ln : ln.LinkedData;
      this.InitializeComponent();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void calculateHelocLoanAmountAndDraw()
    {
      this.loandata.Calculator.FormCalculation("HELOCDRAWCALC");
      this.loandata.Calculator.FormCalculation("HELOCLOANCALC");
    }

    private void initForm()
    {
      double num = this.loandata.FltVal("427") + this.loandata.FltVal("428");
      string val1 = (num + this.loandata.FltVal("CASASRN.X167") - this.loandata.FltVal("1888")).ToString();
      string val2 = (num + this.loandata.FltVal("CASASRN.X168") - this.loandata.FltVal("1109")).ToString();
      string field = this.loandata.GetField("358");
      this.loandata.SetField("4521", val1);
      this.loandata.SetField("4522", val2);
      this.txtValuationUsed.Text = field;
      this.loandata.SetField("4520", field, true);
      this.txtTCLTV.Text = this.loandata.GetField("4523");
      this.txtTHCLT.Text = this.loandata.GetField("4524");
      this.calculateHelocLoanAmountAndDraw();
      this.txtDrawAmount.Text = this.loandata.GetField("4525");
      this.txtloanamout.Text = this.loandata.GetField("4526");
    }

    private void HelocCalcDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void HelocCalcDialog_Load(object sender, EventArgs e) => this.initForm();

    private void btnApply_Click(object sender, EventArgs e)
    {
      if (this.loandata.FltVal("4524") < this.loandata.FltVal("4523"))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "HCLTV must be >= CLTV", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.loandata.SetField("1109", this.loandata.GetField("4526"), true);
        this.loandata.SetField("1888", this.loandata.GetField("4525"), true);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void txtValuationUsed_TextChanged(object sender, EventArgs e)
    {
    }

    private void txtTCLTV_TextChanged(object sender, EventArgs e)
    {
    }

    private void txtTHCLT_TextChanged(object sender, EventArgs e)
    {
    }

    private void txtValuationUsed_Leave(object sender, EventArgs e)
    {
      this.loandata.SetField("4520", this.txtValuationUsed.Text);
      this.calculateHelocLoanAmountAndDraw();
      this.txtDrawAmount.Text = this.loandata.GetField("4525");
    }

    private void txtTCLTV_Leave(object sender, EventArgs e)
    {
      this.loandata.SetField("4523", this.txtTCLTV.Text);
      this.calculateHelocLoanAmountAndDraw();
      this.txtDrawAmount.Text = this.loandata.GetField("4525");
      this.txtloanamout.Text = this.loandata.GetField("4526");
    }

    private void txtTHCLT_Leave(object sender, EventArgs e)
    {
      this.loandata.SetField("4524", this.txtTHCLT.Text);
      this.calculateHelocLoanAmountAndDraw();
      this.txtloanamout.Text = this.loandata.GetField("4526");
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
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnclose = new System.Windows.Forms.Button();
      this.btnApply = new System.Windows.Forms.Button();
      this.txtloanamout = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.txtDrawAmount = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtTHCLT = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.txtTCLTV = new System.Windows.Forms.TextBox();
      this.txtValuationUsed = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.pnlMainContainer.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.panel1);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.txtTHCLT);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.txtTCLTV);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.txtValuationUsed);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.pnlMainContainer.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.pnlMainContainer.Dock = DockStyle.Fill;
      this.pnlMainContainer.Location = new Point(0, 0);
      this.pnlMainContainer.Name = "pnlMainContainer";
      this.pnlMainContainer.Size = new Size(360, 181);
      this.pnlMainContainer.TabIndex = 1;
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.btnclose);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.btnApply);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.txtloanamout);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.txtDrawAmount);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.panel1.Location = new Point(4, 82);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(350, 91);
      this.panel1.TabIndex = 19;
      this.btnclose.Location = new Point(261, 58);
      this.btnclose.Name = "btnclose";
      this.btnclose.Size = new Size(75, 23);
      this.btnclose.TabIndex = 11;
      this.btnclose.Text = "Close";
      this.btnclose.UseVisualStyleBackColor = true;
      this.btnclose.Click += new EventHandler(this.btnCancel_Click);
      this.btnApply.Location = new Point(167, 58);
      this.btnApply.Name = "btnApply";
      this.btnApply.Size = new Size(92, 23);
      this.btnApply.TabIndex = 10;
      this.btnApply.Text = "Apply to Loan";
      this.btnApply.UseVisualStyleBackColor = true;
      this.btnApply.Click += new EventHandler(this.btnApply_Click);
      this.txtloanamout.Enabled = false;
      this.txtloanamout.Location = new Point(167, 29);
      this.txtloanamout.MaxLength = 100;
      this.txtloanamout.Name = "txtloanamout";
      this.txtloanamout.Size = new Size(172, 20);
      this.txtloanamout.TabIndex = 9;
      this.txtloanamout.TextAlign = HorizontalAlignment.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 32);
      this.label4.Name = "label4";
      this.label4.Size = new Size(123, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Calculated Loan Amount";
      this.txtDrawAmount.Enabled = false;
      this.txtDrawAmount.Location = new Point(167, 3);
      this.txtDrawAmount.MaxLength = 100;
      this.txtDrawAmount.Name = "txtDrawAmount";
      this.txtDrawAmount.Size = new Size(172, 20);
      this.txtDrawAmount.TabIndex = 7;
      this.txtDrawAmount.TextAlign = HorizontalAlignment.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(124, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Calculated Draw Amount";
      this.txtTHCLT.Location = new Point(172, 57);
      this.txtTHCLT.MaxLength = 100;
      this.txtTHCLT.Name = "txtTHCLT";
      this.txtTHCLT.Size = new Size(172, 20);
      this.txtTHCLT.TabIndex = 18;
      this.txtTHCLT.TextAlign = HorizontalAlignment.Right;
      this.txtTHCLT.Leave += new EventHandler(this.txtTHCLT_Leave);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 60);
      this.label7.Name = "label7";
      this.label7.Size = new Size(76, 13);
      this.label7.TabIndex = 17;
      this.label7.Text = "Target HCLTV";
      this.txtTCLTV.Location = new Point(171, 33);
      this.txtTCLTV.MaxLength = 100;
      this.txtTCLTV.Name = "txtTCLTV";
      this.txtTCLTV.Size = new Size(172, 20);
      this.txtTCLTV.TabIndex = 16;
      this.txtTCLTV.TextAlign = HorizontalAlignment.Right;
      this.txtTCLTV.Leave += new EventHandler(this.txtTCLTV_Leave);
      this.txtValuationUsed.Location = new Point(171, 9);
      this.txtValuationUsed.MaxLength = 100;
      this.txtValuationUsed.Name = "txtValuationUsed";
      this.txtValuationUsed.Size = new Size(172, 20);
      this.txtValuationUsed.TabIndex = 5;
      this.txtValuationUsed.TextAlign = HorizontalAlignment.Right;
      this.txtValuationUsed.Leave += new EventHandler(this.txtValuationUsed_Leave);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Target CLTV";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(79, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Valuation Used";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(360, 181);
      this.Controls.Add((System.Windows.Forms.Control) this.pnlMainContainer);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HelocCalcDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Calculate HELOC Amount";
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
