// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PurchaseAdviceDetailForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PurchaseAdviceDetailForm : Form
  {
    private LoanPurchaseLog purchasedLog;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnCancel;
    private Label label32;
    private TextBox txtInvestorLoanNumber;
    private TextBox txtPurchaseAdviceDate;
    private TextBox txtInvestor;
    private TextBox txtFirstPaymenttoInvestor;
    private Label label14;
    private Label label15;
    private Label label16;
    private Label labelCreatedBy;
    private Button button1;
    private TextBox txtPurchaseAmount;
    private Label label1;

    public PurchaseAdviceDetailForm(LoanPurchaseLog purchasedlog, LoanData loan)
    {
      this.purchasedLog = purchasedlog;
      this.InitializeComponent();
      this.txtPurchaseAdviceDate.Text = purchasedlog.PurchaseAdviceDate.ToString("MM/dd/yyyy");
      this.txtInvestor.Text = purchasedlog.Investor;
      this.txtInvestorLoanNumber.Text = purchasedlog.InvestorLoanNumber;
      this.txtFirstPaymenttoInvestor.Text = purchasedlog.FirstPaymenttoInvestor.ToString("MM/dd/yyyy");
      this.txtPurchaseAmount.Text = purchasedlog.PurchaseAmount.ToString("N2");
      this.labelCreatedBy.Text = "Created by Admin User on " + this.purchasedLog.CreatedDateTime.ToString("MM/dd/yyyy hh:mm tt");
    }

    public LoanPurchaseLog PurchasedLog => this.purchasedLog;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.txtPurchaseAmount = new TextBox();
      this.label1 = new Label();
      this.button1 = new Button();
      this.labelCreatedBy = new Label();
      this.btnCancel = new Button();
      this.label32 = new Label();
      this.txtInvestorLoanNumber = new TextBox();
      this.txtPurchaseAdviceDate = new TextBox();
      this.txtInvestor = new TextBox();
      this.txtFirstPaymenttoInvestor = new TextBox();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.txtPurchaseAmount);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.button1);
      this.groupContainer1.Controls.Add((Control) this.labelCreatedBy);
      this.groupContainer1.Controls.Add((Control) this.btnCancel);
      this.groupContainer1.Controls.Add((Control) this.label32);
      this.groupContainer1.Controls.Add((Control) this.txtInvestorLoanNumber);
      this.groupContainer1.Controls.Add((Control) this.txtPurchaseAdviceDate);
      this.groupContainer1.Controls.Add((Control) this.txtInvestor);
      this.groupContainer1.Controls.Add((Control) this.txtFirstPaymenttoInvestor);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.label15);
      this.groupContainer1.Controls.Add((Control) this.label16);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(373, 227);
      this.groupContainer1.TabIndex = 165;
      this.groupContainer1.Text = "T01 Loan Purchase";
      this.txtPurchaseAmount.BackColor = Color.WhiteSmoke;
      this.txtPurchaseAmount.Location = new Point(167, 132);
      this.txtPurchaseAmount.Name = "txtPurchaseAmount";
      this.txtPurchaseAmount.ReadOnly = true;
      this.txtPurchaseAmount.Size = new Size(185, 20);
      this.txtPurchaseAmount.TabIndex = 153;
      this.txtPurchaseAmount.Tag = (object) "";
      this.txtPurchaseAmount.TextAlign = HorizontalAlignment.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 134);
      this.label1.Name = "label1";
      this.label1.Size = new Size(101, 13);
      this.label1.TabIndex = 152;
      this.label1.Text = "Purchased Principal";
      this.button1.BackColor = SystemColors.Control;
      this.button1.DialogResult = DialogResult.Cancel;
      this.button1.Location = new Point(275, 161);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 151;
      this.button1.Text = "&Close";
      this.button1.UseVisualStyleBackColor = true;
      this.labelCreatedBy.AutoSize = true;
      this.labelCreatedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelCreatedBy.ForeColor = SystemColors.ControlText;
      this.labelCreatedBy.Location = new Point(8, 196);
      this.labelCreatedBy.Name = "labelCreatedBy";
      this.labelCreatedBy.Size = new Size(62, 13);
      this.labelCreatedBy.TabIndex = 150;
      this.labelCreatedBy.Text = "(CreatedBy)";
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(225, 454);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 116;
      this.btnCancel.Text = "&Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label32.AutoSize = true;
      this.label32.Location = new Point(8, 86);
      this.label32.Name = "label32";
      this.label32.Size = new Size(112, 13);
      this.label32.TabIndex = 149;
      this.label32.Text = "Investor Loan Number";
      this.txtInvestorLoanNumber.BackColor = Color.WhiteSmoke;
      this.txtInvestorLoanNumber.Location = new Point(167, 84);
      this.txtInvestorLoanNumber.Name = "txtInvestorLoanNumber";
      this.txtInvestorLoanNumber.ReadOnly = true;
      this.txtInvestorLoanNumber.Size = new Size(185, 20);
      this.txtInvestorLoanNumber.TabIndex = 148;
      this.txtInvestorLoanNumber.Tag = (object) "";
      this.txtPurchaseAdviceDate.BackColor = Color.WhiteSmoke;
      this.txtPurchaseAdviceDate.Location = new Point(167, 39);
      this.txtPurchaseAdviceDate.Name = "txtPurchaseAdviceDate";
      this.txtPurchaseAdviceDate.ReadOnly = true;
      this.txtPurchaseAdviceDate.Size = new Size(185, 20);
      this.txtPurchaseAdviceDate.TabIndex = 128;
      this.txtPurchaseAdviceDate.Tag = (object) "";
      this.txtInvestor.BackColor = Color.WhiteSmoke;
      this.txtInvestor.Location = new Point(167, 61);
      this.txtInvestor.Name = "txtInvestor";
      this.txtInvestor.ReadOnly = true;
      this.txtInvestor.Size = new Size(185, 20);
      this.txtInvestor.TabIndex = 129;
      this.txtInvestor.Tag = (object) "";
      this.txtFirstPaymenttoInvestor.BackColor = Color.WhiteSmoke;
      this.txtFirstPaymenttoInvestor.Location = new Point(167, 108);
      this.txtFirstPaymenttoInvestor.Name = "txtFirstPaymenttoInvestor";
      this.txtFirstPaymenttoInvestor.ReadOnly = true;
      this.txtFirstPaymenttoInvestor.Size = new Size(185, 20);
      this.txtFirstPaymenttoInvestor.TabIndex = 130;
      this.txtFirstPaymenttoInvestor.Tag = (object) "";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(8, 42);
      this.label14.Name = "label14";
      this.label14.Size = new Size(114, 13);
      this.label14.TabIndex = 131;
      this.label14.Text = "Purchase Advice Date";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(8, 64);
      this.label15.Name = "label15";
      this.label15.Size = new Size(45, 13);
      this.label15.TabIndex = 132;
      this.label15.Text = "Investor";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(8, 108);
      this.label16.Name = "label16";
      this.label16.Size = new Size(123, 13);
      this.label16.TabIndex = 133;
      this.label16.Text = "First Payment to Investor";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(373, 227);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (PurchaseAdviceDetailForm);
      this.Text = "Loan Purchase Transaction";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
