// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LinkedLoanDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LinkedLoanDialog : Form
  {
    private IContainer components;
    private Button btnOpen;
    private Button btnClose;
    private Label label1;
    private Label labelLoanNumber;
    private Label label2;
    private Label labelLinkedLoanNumber;
    private Panel panel1;
    private Label label3;

    public LinkedLoanDialog(string loanGUID, string linkedGUID)
    {
      this.InitializeComponent();
      this.labelLoanNumber.Text = loanGUID;
      this.labelLinkedLoanNumber.Text = linkedGUID == "" ? "(No loan number)" : linkedGUID;
    }

    private void btnOpen_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOpen = new Button();
      this.btnClose = new Button();
      this.label1 = new Label();
      this.labelLoanNumber = new Label();
      this.label2 = new Label();
      this.labelLinkedLoanNumber = new Label();
      this.panel1 = new Panel();
      this.label3 = new Label();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.btnOpen.Location = new Point(103, 108);
      this.btnOpen.Name = "btnOpen";
      this.btnOpen.Size = new Size(112, 23);
      this.btnOpen.TabIndex = 0;
      this.btnOpen.Text = "&Open Linked Loan";
      this.btnOpen.UseVisualStyleBackColor = true;
      this.btnOpen.Click += new EventHandler(this.btnOpen_Click);
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(221, 108);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(112, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(79, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(76, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Selected Loan";
      this.labelLoanNumber.AutoSize = true;
      this.labelLoanNumber.Dock = DockStyle.Left;
      this.labelLoanNumber.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelLoanNumber.Location = new Point(0, 0);
      this.labelLoanNumber.Name = "labelLoanNumber";
      this.labelLoanNumber.Size = new Size(90, 13);
      this.labelLoanNumber.TabIndex = 3;
      this.labelLoanNumber.Text = "(Loan Number)";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(79, 60);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "linked to Loan";
      this.labelLinkedLoanNumber.AutoSize = true;
      this.labelLinkedLoanNumber.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelLinkedLoanNumber.Location = new Point(155, 60);
      this.labelLinkedLoanNumber.Name = "labelLinkedLoanNumber";
      this.labelLinkedLoanNumber.Size = new Size(90, 13);
      this.labelLinkedLoanNumber.TabIndex = 6;
      this.labelLinkedLoanNumber.Text = "(Loan Number)";
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.labelLoanNumber);
      this.panel1.Location = new Point(155, 35);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(268, 13);
      this.panel1.TabIndex = 7;
      this.label3.AutoSize = true;
      this.label3.Dock = DockStyle.Left;
      this.label3.Location = new Point(90, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(14, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "is";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(438, 153);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.labelLinkedLoanNumber);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnOpen);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LinkedLoanDialog);
      this.Text = "Encompass";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
