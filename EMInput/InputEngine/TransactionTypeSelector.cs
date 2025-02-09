// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TransactionTypeSelector
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine.InterimServicing;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TransactionTypeSelector : Form
  {
    private IContainer components;
    private RadioButton radioBtnPayment;
    private Label label1;
    private RadioButton radioBtnReversal;
    private RadioButton radioBtnDisbursement;
    private RadioButton radioBtnInterest;
    private Button btnCancel;
    private Button btnOK;
    private RadioButton radioBtnDisburseReversal;
    private RadioButton radioButtonOther;
    private Panel panelOutside;
    private Panel panelInside;
    private RadioButton radioBtnPrincipalDisbursement;

    public TransactionTypeSelector() => this.InitializeComponent();

    public ServicingTransactionTypes TransactionType
    {
      get
      {
        if (this.radioBtnReversal.Checked)
          return ServicingTransactionTypes.PaymentReversal;
        if (this.radioBtnDisbursement.Checked)
          return ServicingTransactionTypes.EscrowDisbursement;
        if (this.radioBtnInterest.Checked)
          return ServicingTransactionTypes.EscrowInterest;
        if (this.radioBtnDisburseReversal.Checked)
          return ServicingTransactionTypes.EscrowDisbursementReversal;
        if (this.radioButtonOther.Checked)
          return ServicingTransactionTypes.Other;
        return this.radioBtnPrincipalDisbursement.Checked ? ServicingTransactionTypes.PrincipalDisbursement : ServicingTransactionTypes.Payment;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.radioBtnPayment = new RadioButton();
      this.label1 = new Label();
      this.radioBtnReversal = new RadioButton();
      this.radioBtnDisbursement = new RadioButton();
      this.radioBtnInterest = new RadioButton();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.radioBtnDisburseReversal = new RadioButton();
      this.radioButtonOther = new RadioButton();
      this.panelOutside = new Panel();
      this.panelInside = new Panel();
      this.radioBtnPrincipalDisbursement = new RadioButton();
      this.panelOutside.SuspendLayout();
      this.panelInside.SuspendLayout();
      this.SuspendLayout();
      this.radioBtnPayment.AutoSize = true;
      this.radioBtnPayment.Checked = true;
      this.radioBtnPayment.Location = new Point(22, 31);
      this.radioBtnPayment.Name = "radioBtnPayment";
      this.radioBtnPayment.Size = new Size(66, 17);
      this.radioBtnPayment.TabIndex = 0;
      this.radioBtnPayment.TabStop = true;
      this.radioBtnPayment.Text = "Payment";
      this.radioBtnPayment.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(22, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(139, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Select a type of transaction:";
      this.radioBtnReversal.AutoSize = true;
      this.radioBtnReversal.Location = new Point(22, 53);
      this.radioBtnReversal.Name = "radioBtnReversal";
      this.radioBtnReversal.Size = new Size(111, 17);
      this.radioBtnReversal.TabIndex = 2;
      this.radioBtnReversal.TabStop = true;
      this.radioBtnReversal.Text = "Payment Reversal";
      this.radioBtnReversal.UseVisualStyleBackColor = true;
      this.radioBtnDisbursement.AutoSize = true;
      this.radioBtnDisbursement.Location = new Point(22, 97);
      this.radioBtnDisbursement.Name = "radioBtnDisbursement";
      this.radioBtnDisbursement.Size = new Size((int) sbyte.MaxValue, 17);
      this.radioBtnDisbursement.TabIndex = 3;
      this.radioBtnDisbursement.TabStop = true;
      this.radioBtnDisbursement.Text = "Escrow Disbursement";
      this.radioBtnDisbursement.UseVisualStyleBackColor = true;
      this.radioBtnInterest.AutoSize = true;
      this.radioBtnInterest.Location = new Point(22, 141);
      this.radioBtnInterest.Name = "radioBtnInterest";
      this.radioBtnInterest.Size = new Size(98, 17);
      this.radioBtnInterest.TabIndex = 4;
      this.radioBtnInterest.TabStop = true;
      this.radioBtnInterest.Text = "Escrow Interest";
      this.radioBtnInterest.UseVisualStyleBackColor = true;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(183, 198);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.BackColor = SystemColors.Control;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(103, 198);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.radioBtnDisburseReversal.AutoSize = true;
      this.radioBtnDisburseReversal.Location = new Point(22, 119);
      this.radioBtnDisburseReversal.Name = "radioBtnDisburseReversal";
      this.radioBtnDisburseReversal.Size = new Size(172, 17);
      this.radioBtnDisburseReversal.TabIndex = 9;
      this.radioBtnDisburseReversal.TabStop = true;
      this.radioBtnDisburseReversal.Text = "Escrow Disbursement Reversal";
      this.radioBtnDisburseReversal.UseVisualStyleBackColor = true;
      this.radioButtonOther.AutoSize = true;
      this.radioButtonOther.Location = new Point(22, 163);
      this.radioButtonOther.Name = "radioButtonOther";
      this.radioButtonOther.Size = new Size(51, 17);
      this.radioButtonOther.TabIndex = 10;
      this.radioButtonOther.TabStop = true;
      this.radioButtonOther.Text = "Other";
      this.radioButtonOther.UseVisualStyleBackColor = true;
      this.panelOutside.BackColor = SystemColors.Window;
      this.panelOutside.BorderStyle = BorderStyle.FixedSingle;
      this.panelOutside.Controls.Add((Control) this.panelInside);
      this.panelOutside.Dock = DockStyle.Fill;
      this.panelOutside.Location = new Point(0, 0);
      this.panelOutside.Name = "panelOutside";
      this.panelOutside.Size = new Size(276, 242);
      this.panelOutside.TabIndex = 81;
      this.panelInside.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelInside.BackColor = Color.WhiteSmoke;
      this.panelInside.Controls.Add((Control) this.radioBtnPrincipalDisbursement);
      this.panelInside.Controls.Add((Control) this.label1);
      this.panelInside.Controls.Add((Control) this.radioButtonOther);
      this.panelInside.Controls.Add((Control) this.radioBtnPayment);
      this.panelInside.Controls.Add((Control) this.radioBtnDisburseReversal);
      this.panelInside.Controls.Add((Control) this.radioBtnReversal);
      this.panelInside.Controls.Add((Control) this.btnCancel);
      this.panelInside.Controls.Add((Control) this.radioBtnDisbursement);
      this.panelInside.Controls.Add((Control) this.btnOK);
      this.panelInside.Controls.Add((Control) this.radioBtnInterest);
      this.panelInside.Location = new Point(4, 4);
      this.panelInside.Name = "panelInside";
      this.panelInside.Size = new Size(266, 232);
      this.panelInside.TabIndex = 0;
      this.radioBtnPrincipalDisbursement.AutoSize = true;
      this.radioBtnPrincipalDisbursement.Location = new Point(22, 75);
      this.radioBtnPrincipalDisbursement.Name = "radioBtnPrincipalDisbursement";
      this.radioBtnPrincipalDisbursement.Size = new Size(132, 17);
      this.radioBtnPrincipalDisbursement.TabIndex = 11;
      this.radioBtnPrincipalDisbursement.TabStop = true;
      this.radioBtnPrincipalDisbursement.Text = "Principal Disbursement";
      this.radioBtnPrincipalDisbursement.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(276, 242);
      this.Controls.Add((Control) this.panelOutside);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TransactionTypeSelector);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add New Transaction";
      this.panelOutside.ResumeLayout(false);
      this.panelInside.ResumeLayout(false);
      this.panelInside.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
